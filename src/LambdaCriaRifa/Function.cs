using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using LambdaCriaRifa.Data;
using LambdaCriaRifa.Models;
using LambdaCriaRifa.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaCriaRifa;

/// <summary>
/// Lambda function handler for processing SQS messages containing lottery creation requests.
/// </summary>
public class Function
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Function> _logger;

    /// <summary>
    /// Default constructor that initializes the function with dependency injection.
    /// </summary>
    public Function()
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        // Setup dependency injection
        var services = new ServiceCollection();

        // Configure logging
        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
        });

        // Register services
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found in configuration.");

        services.AddSingleton<IDbConnectionFactory>(sp => new DbConnectionFactory(connectionString));
        services.AddScoped<ILotteryService, LotteryService>();

        _serviceProvider = services.BuildServiceProvider();
        _logger = _serviceProvider.GetRequiredService<ILogger<Function>>();

        _logger.LogInformation("Lambda function initialized successfully.");
    }

    /// <summary>
    /// Constructor for testing purposes.
    /// </summary>
    /// <param name="serviceProvider">Service provider for dependency injection.</param>
    public Function(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = _serviceProvider.GetRequiredService<ILogger<Function>>();
    }

    /// <summary>
    /// Lambda function handler that processes SQS events.
    /// </summary>
    /// <param name="sqsEvent">The SQS event containing lottery creation messages.</param>
    /// <param name="context">The Lambda context.</param>
    /// <returns>SQSBatchResponse indicating which messages failed processing.</returns>
    public async Task<SQSBatchResponse> FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        _logger.LogInformation("Processing SQS event with {MessageCount} messages.", sqsEvent.Records.Count);

        var batchItemFailures = new List<SQSBatchResponse.BatchItemFailure>();

        // Create cancellation token from remaining time
        using var cts = new CancellationTokenSource(context.RemainingTime.Subtract(TimeSpan.FromSeconds(1)));
        var cancellationToken = cts.Token;

        foreach (var record in sqsEvent.Records)
        {
            try
            {
                _logger.LogInformation("Processing message with ID: {MessageId}", record.MessageId);

                // Deserialize the SQS message body
                var request = JsonSerializer.Deserialize<CreateLotteryRequest>(record.Body);

                if (request == null)
                {
                    _logger.LogError("Failed to deserialize message with ID: {MessageId}", record.MessageId);
                    batchItemFailures.Add(new SQSBatchResponse.BatchItemFailure
                    {
                        ItemIdentifier = record.MessageId
                    });
                    continue;
                }

                // Validate the request
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    _logger.LogError("Invalid lottery name in message ID: {MessageId}", record.MessageId);
                    batchItemFailures.Add(new SQSBatchResponse.BatchItemFailure
                    {
                        ItemIdentifier = record.MessageId
                    });
                    continue;
                }

                // Process the lottery creation
                using var scope = _serviceProvider.CreateScope();
                var lotteryService = scope.ServiceProvider.GetRequiredService<ILotteryService>();
                await lotteryService.CreateLotteryAsync(request, cancellationToken);

                _logger.LogInformation("Successfully processed message with ID: {MessageId}", record.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message with ID: {MessageId}", record.MessageId);
                batchItemFailures.Add(new SQSBatchResponse.BatchItemFailure
                {
                    ItemIdentifier = record.MessageId
                });
            }
        }

        _logger.LogInformation("Completed processing. Success: {SuccessCount}, Failed: {FailedCount}",
            sqsEvent.Records.Count - batchItemFailures.Count, batchItemFailures.Count);

        return new SQSBatchResponse
        {
            BatchItemFailures = batchItemFailures
        };
    }
}
