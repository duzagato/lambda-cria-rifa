# Lambda Cria Rifa

AWS Lambda function in C# (.NET 8) for processing SQS messages containing lottery creation requests from the `edoha-core` project.

## 📋 Overview

This Lambda function is triggered by SQS messages sent from the `POST /Lottery` endpoint in the `edoha-core` project. It processes lottery creation requests and will integrate with a PostgreSQL database to persist lottery information.

## 🏗️ Project Structure

```
lambda-cria-rifa/
├── src/
│   └── LambdaCriaRifa/
│       ├── Function.cs                    # Main Lambda handler
│       ├── LambdaCriaRifa.csproj          # Project file with dependencies
│       ├── appsettings.json               # Configuration file
│       ├── Models/
│       │   └── CreateLotteryRequest.cs    # DTO for lottery creation
│       ├── Services/
│       │   ├── ILotteryService.cs         # Service interface
│       │   └── LotteryService.cs          # Service implementation
│       └── Data/
│           ├── IDbConnectionFactory.cs    # Database connection factory interface
│           └── DbConnectionFactory.cs     # PostgreSQL connection factory
├── test/
│   └── LambdaCriaRifa.Tests/
│       ├── FunctionTest.cs                # Unit tests for Lambda handler
│       └── LambdaCriaRifa.Tests.csproj    # Test project file
├── LambdaCriaRifa.sln                     # Solution file
├── .gitignore                             # Git ignore patterns
└── README.md                              # This file
```

## 🚀 Features

- **SQS Event Processing**: Processes SQS messages containing lottery creation requests
- **Batch Processing**: Uses `SQSBatchResponse` for handling individual message failures
- **Dependency Injection**: Configured DI container with service registration
- **Configuration Management**: Supports `appsettings.json` and environment variable overrides
- **PostgreSQL Ready**: Database connection factory prepared for PostgreSQL integration
- **Comprehensive Logging**: Structured logging using Microsoft.Extensions.Logging
- **Error Handling**: Robust error handling with batch item failures for retries
- **Unit Tests**: Complete test coverage using xUnit and Moq

## 📦 NuGet Packages

The project uses the following key packages:

- **Amazon.Lambda.Core** (2.8.0) - Core Lambda functionality
- **Amazon.Lambda.SQSEvents** (2.2.0) - SQS event handling
- **Amazon.Lambda.Serialization.SystemTextJson** (2.4.4) - JSON serialization
- **Microsoft.Extensions.Configuration*** - Configuration management
- **Microsoft.Extensions.DependencyInjection** (10.0.1) - Dependency injection
- **Microsoft.Extensions.Logging.Console** (10.0.1) - Console logging
- **Npgsql** (10.0.1) - PostgreSQL data provider

## 📝 Data Model

### CreateLotteryRequest

```csharp
{
    "Name": "string",                    // Name of the lottery
    "NumTicketsTicketbook": 0,          // Number of tickets per ticketbook
    "NumTicketbooks": 0,                // Number of ticketbooks
    "PriceTicket": 0.0,                 // Price per ticket (decimal)
    "DoubleChance": false               // Indicates if lottery has double chance
}
```

### Example SQS Message Payload

```json
{
    "Name": "Super Rifa 2026",
    "NumTicketsTicketbook": 100,
    "NumTicketbooks": 10,
    "PriceTicket": 10.50,
    "DoubleChance": true
}
```

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=edoha;User Id=postgres;Password=postgres;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    }
  }
}
```

### Environment Variables

Configuration can be overridden using environment variables:

- `ConnectionStrings__Default` - PostgreSQL connection string

## 🛠️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [AWS CLI](https://aws.amazon.com/cli/) (for deployment)
- [AWS SAM CLI](https://aws.amazon.com/serverless/sam/) (optional, for local testing)
- PostgreSQL (when implementing database operations)

## 🏃 Local Development

### Build the Solution

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

### Run Tests with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Restore NuGet Packages

```bash
dotnet restore
```

## 🧪 Testing

The project includes comprehensive unit tests covering:

- ✅ Valid message processing
- ✅ Invalid JSON handling
- ✅ Empty lottery name validation
- ✅ Batch processing with mixed success/failure messages

Run the tests with:

```bash
cd test/LambdaCriaRifa.Tests
dotnet test --verbosity normal
```

## 🚢 Deployment

### Package the Lambda

```bash
cd src/LambdaCriaRifa
dotnet lambda package
```

### Deploy with AWS CLI

```bash
# Create deployment package
dotnet publish -c Release -o ./publish

# Create zip file
cd publish
zip -r ../lambda-deployment.zip .
cd ..

# Deploy to AWS Lambda (example)
aws lambda update-function-code \
    --function-name lambda-cria-rifa \
    --zip-file fileb://lambda-deployment.zip
```

## 🔧 AWS Configuration

### Required AWS Resources

1. **SQS Queue**: Source queue for lottery creation messages
2. **Lambda Function**: This function configured to trigger on SQS messages
3. **IAM Role**: Lambda execution role with permissions:
   - SQS message reading (`sqs:ReceiveMessage`, `sqs:DeleteMessage`, `sqs:GetQueueAttributes`)
   - CloudWatch Logs (`logs:CreateLogGroup`, `logs:CreateLogStream`, `logs:PutLogEvents`)
   - VPC access (if PostgreSQL is in VPC)

### Lambda Configuration

- **Runtime**: .NET 8 (Custom Runtime)
- **Handler**: `LambdaCriaRifa::LambdaCriaRifa.Function::FunctionHandler`
- **Timeout**: 30 seconds (recommended)
- **Memory**: 512 MB (recommended)
- **Environment Variables**:
  - `ConnectionStrings__Default`: PostgreSQL connection string

### SQS Trigger Configuration

- **Batch Size**: 10 (recommended)
- **Maximum Batching Window**: 0 seconds
- **Report Batch Item Failures**: Enabled (required for partial batch responses)

## 📊 Monitoring

The function logs important events:

- Message processing start/completion
- Individual message success/failure
- Validation errors
- Database operations (when implemented)
- Batch processing summary

View logs in CloudWatch Logs:

```bash
aws logs tail /aws/lambda/lambda-cria-rifa --follow
```

## 🔒 Security Considerations

- Store database credentials in AWS Secrets Manager or Parameter Store
- Use VPC for secure database access
- Enable encryption at rest for SQS queues
- Use IAM roles with least privilege principle
- Enable AWS CloudTrail for audit logging

## 🔄 Future Enhancements

- [ ] Implement database schema and persistence logic
- [ ] Add dead letter queue handling
- [ ] Implement circuit breaker pattern for database connections
- [ ] Add AWS X-Ray tracing
- [ ] Implement custom metrics for CloudWatch
- [ ] Add integration tests with LocalStack
- [ ] Implement idempotency handling
- [ ] Add API Gateway integration for direct invocation

## 📖 Related Projects

- **edoha-core**: Main application that sends lottery creation messages to SQS

## 🤝 Contributing

1. Create a feature branch
2. Make your changes
3. Run tests: `dotnet test`
4. Build the solution: `dotnet build`
5. Create a pull request

## 📄 License

[Add your license information here]

## 👥 Authors

[Add author information here]

## 📞 Support

[Add support contact information here]