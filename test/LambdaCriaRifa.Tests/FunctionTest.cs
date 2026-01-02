using Amazon.Lambda.SQSEvents;
using Amazon.Lambda.TestUtilities;
using LambdaCriaRifa.Models;
using LambdaCriaRifa.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Xunit;

namespace LambdaCriaRifa.Tests;

/// <summary>
/// Unit tests for the Lambda Function handler.
/// </summary>
public class FunctionTest
{
    /// <summary>
    /// Tests that the handler processes a valid SQS message successfully.
    /// </summary>
    [Fact]
    public async Task FunctionHandler_ValidMessage_ProcessesSuccessfully()
    {
        // Arrange
        var mockLotteryService = new Mock<ILotteryService>();
        mockLotteryService
            .Setup(s => s.CreateLotteryAsync(It.IsAny<CreateLotteryRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());
        services.AddScoped<ILotteryService>(sp => mockLotteryService.Object);

        var serviceProvider = services.BuildServiceProvider();
        var function = new Function(serviceProvider);

        var request = new CreateLotteryRequest
        {
            Name = "Test Lottery",
            NumTicketsTicketbook = 10,
            NumTicketbooks = 5,
            PriceTicket = 10.50m,
            DoubleChance = true
        };

        var sqsEvent = new SQSEvent
        {
            Records = new List<SQSEvent.SQSMessage>
            {
                new SQSEvent.SQSMessage
                {
                    MessageId = "test-message-id",
                    Body = JsonSerializer.Serialize(request)
                }
            }
        };

        var context = new TestLambdaContext
        {
            FunctionName = "LambdaCriaRifa",
            FunctionVersion = "1",
            RemainingTime = TimeSpan.FromMinutes(5)
        };

        // Act
        var response = await function.FunctionHandler(sqsEvent, context);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.BatchItemFailures);
        mockLotteryService.Verify(
            s => s.CreateLotteryAsync(It.IsAny<CreateLotteryRequest>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Tests that the handler returns a batch item failure for invalid messages.
    /// </summary>
    [Fact]
    public async Task FunctionHandler_InvalidMessage_ReturnsFailure()
    {
        // Arrange
        var mockLotteryService = new Mock<ILotteryService>();
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());
        services.AddScoped<ILotteryService>(sp => mockLotteryService.Object);

        var serviceProvider = services.BuildServiceProvider();
        var function = new Function(serviceProvider);

        var sqsEvent = new SQSEvent
        {
            Records = new List<SQSEvent.SQSMessage>
            {
                new SQSEvent.SQSMessage
                {
                    MessageId = "test-message-id",
                    Body = "{ invalid json }"
                }
            }
        };

        var context = new TestLambdaContext
        {
            FunctionName = "LambdaCriaRifa",
            FunctionVersion = "1",
            RemainingTime = TimeSpan.FromMinutes(5)
        };

        // Act
        var response = await function.FunctionHandler(sqsEvent, context);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.BatchItemFailures);
        Assert.Equal("test-message-id", response.BatchItemFailures[0].ItemIdentifier);
        mockLotteryService.Verify(
            s => s.CreateLotteryAsync(It.IsAny<CreateLotteryRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Tests that the handler returns a batch item failure for messages with empty lottery name.
    /// </summary>
    [Fact]
    public async Task FunctionHandler_EmptyLotteryName_ReturnsFailure()
    {
        // Arrange
        var mockLotteryService = new Mock<ILotteryService>();
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());
        services.AddScoped<ILotteryService>(sp => mockLotteryService.Object);

        var serviceProvider = services.BuildServiceProvider();
        var function = new Function(serviceProvider);

        var request = new CreateLotteryRequest
        {
            Name = "",
            NumTicketsTicketbook = 10,
            NumTicketbooks = 5,
            PriceTicket = 10.50m,
            DoubleChance = true
        };

        var sqsEvent = new SQSEvent
        {
            Records = new List<SQSEvent.SQSMessage>
            {
                new SQSEvent.SQSMessage
                {
                    MessageId = "test-message-id",
                    Body = JsonSerializer.Serialize(request)
                }
            }
        };

        var context = new TestLambdaContext
        {
            FunctionName = "LambdaCriaRifa",
            FunctionVersion = "1",
            RemainingTime = TimeSpan.FromMinutes(5)
        };

        // Act
        var response = await function.FunctionHandler(sqsEvent, context);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.BatchItemFailures);
        Assert.Equal("test-message-id", response.BatchItemFailures[0].ItemIdentifier);
        mockLotteryService.Verify(
            s => s.CreateLotteryAsync(It.IsAny<CreateLotteryRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Tests that the handler processes multiple messages correctly.
    /// </summary>
    [Fact]
    public async Task FunctionHandler_MultipleMessages_ProcessesBatch()
    {
        // Arrange
        var mockLotteryService = new Mock<ILotteryService>();
        mockLotteryService
            .Setup(s => s.CreateLotteryAsync(It.IsAny<CreateLotteryRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());
        services.AddScoped<ILotteryService>(sp => mockLotteryService.Object);

        var serviceProvider = services.BuildServiceProvider();
        var function = new Function(serviceProvider);

        var validRequest = new CreateLotteryRequest
        {
            Name = "Valid Lottery",
            NumTicketsTicketbook = 10,
            NumTicketbooks = 5,
            PriceTicket = 10.50m,
            DoubleChance = true
        };

        var sqsEvent = new SQSEvent
        {
            Records = new List<SQSEvent.SQSMessage>
            {
                new SQSEvent.SQSMessage
                {
                    MessageId = "valid-message-1",
                    Body = JsonSerializer.Serialize(validRequest)
                },
                new SQSEvent.SQSMessage
                {
                    MessageId = "invalid-message",
                    Body = "{ invalid json }"
                },
                new SQSEvent.SQSMessage
                {
                    MessageId = "valid-message-2",
                    Body = JsonSerializer.Serialize(validRequest)
                }
            }
        };

        var context = new TestLambdaContext
        {
            FunctionName = "LambdaCriaRifa",
            FunctionVersion = "1",
            RemainingTime = TimeSpan.FromMinutes(5)
        };

        // Act
        var response = await function.FunctionHandler(sqsEvent, context);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.BatchItemFailures);
        Assert.Equal("invalid-message", response.BatchItemFailures[0].ItemIdentifier);
        mockLotteryService.Verify(
            s => s.CreateLotteryAsync(It.IsAny<CreateLotteryRequest>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }
}
