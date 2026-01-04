using System.Text.Json;
using CreateLotteryLambda.Domain.Models.DTOs.Lottery;
using CreateLotteryLambda.Infrastructure.Context;
using CreateLotteryLambda.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// Configure logging
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger<Program>();

// Load configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// 1. Mock SQS message with CreateLotteryDTO
var createLotteryDto = new CreateLotteryDTO
{
    Name = "Rifa Beneficente",
    NumTicketsTicketbook = 10,
    NumTicketbooks = 100,
    PriceTicket = 5.00m,
    DoubleChance = false
};
logger.LogInformation("Received SQS Message: {Message}", JsonSerializer.Serialize(createLotteryDto));

// 2. Connect to database
var connectionString = configuration.GetConnectionString("PostgreSQL") 
    ?? throw new InvalidOperationException("PostgreSQL connection string not found in configuration.");

logger.LogInformation("Attempting to connect to PostgreSQL database...");

try
{
    var dbContext = new DbConnectionContext(connectionString);
    using var connection = dbContext.CreateConnection();
    logger.LogInformation("Successfully connected to PostgreSQL database!");
    
    // Instantiate LotteryRepository
    var lotteryRepository = new LotteryRepository(connection);
    logger.LogInformation("LotteryRepository instantiated successfully");
}
catch (Exception ex)
{
    logger.LogError(ex, "Failed to connect to database: {Message}", ex.Message);
    logger.LogWarning("Please ensure PostgreSQL is running and the connection string in appsettings.json is correct.");
}

// 3. End
logger.LogInformation("Execution completed.");

