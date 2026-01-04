using System.Text.Json;
using CreateLotteryLambda.Infrastructure.Context;
using Microsoft.Extensions.Configuration;

// Load configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// 1. Mock SQS message
var sqsMessage = new { MessageId = "123", Body = "Test lottery creation message" };
Console.WriteLine($"Received SQS Message: {JsonSerializer.Serialize(sqsMessage)}");

// 2. Connect to database
var connectionString = configuration.GetConnectionString("PostgreSQL") 
    ?? throw new InvalidOperationException("PostgreSQL connection string not found in configuration.");

Console.WriteLine("\nAttempting to connect to PostgreSQL database...");

try
{
    var dbContext = new DbConnectionContext(connectionString);
    using var connection = dbContext.CreateConnection();
    Console.WriteLine("Successfully connected to PostgreSQL database!");
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to connect to database: {ex.Message}");
    Console.WriteLine("Please ensure PostgreSQL is running and the connection string in appsettings.json is correct.");
}

// 3. End
Console.WriteLine("\nExecution completed.");

