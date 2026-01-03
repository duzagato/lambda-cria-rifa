# Lambda Cria Rifa - Arquitetura em Camadas

Sistema de criação de rifas desenvolvido em .NET 8.0 com arquitetura em camadas, permitindo execução local com SQS mockado.

## 📋 Estrutura do Projeto

```
src/
├── LambdaCriaRifa.Application/    # Camada de Aplicação (Console App)
│   ├── Handlers/                  # Manipuladores de mensagens
│   ├── Workers/                   # Worker service para processar SQS
│   ├── Models/                    # DTOs e modelos de requisição
│   ├── MockData/                  # Mensagens SQS mockadas
│   ├── Program.cs                 # Ponto de entrada da aplicação
│   └── appsettings.json           # Configurações da aplicação
├── LambdaCriaRifa.Domain/         # Camada de Domínio (Class Library)
│   ├── Models/                    # Entidades de domínio
│   ├── Services/                  # Serviços de negócio
│   └── Interfaces/                # Interfaces de repositórios
└── LambdaCriaRifa.Infra/          # Camada de Infraestrutura (Class Library)
    ├── Data/                      # Contexto do Entity Framework
    └── Repositories/              # Implementação de repositórios
test/
└── LambdaCriaRifa.Tests/          # Testes unitários
```

## 🎯 Arquitetura

### Camada de Aplicação (Application)
- **Responsabilidade**: Ponto de entrada da aplicação e orquestração
- **Contém**: Workers, Handlers, configuração de DI
- **Tipo**: Console Application executável
- **Executa localmente**: ✅ Sim, com `dotnet run`

### Camada de Domínio (Domain)
- **Responsabilidade**: Lógica de negócio e regras do domínio
- **Contém**: Entidades, Serviços, Interfaces
- **Tipo**: Class Library
- **Dependências**: Apenas abstrações (Microsoft.Extensions.Logging.Abstractions)

### Camada de Infraestrutura (Infra)
- **Responsabilidade**: Acesso a dados e recursos externos
- **Contém**: DbContext, Repositórios, implementações concretas
- **Tipo**: Class Library
- **Dependências**: Entity Framework Core, Npgsql

## 🚀 Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (opcional para desenvolvimento local)

## 📦 Instalação

1. Clone o repositório:
```bash
git clone https://github.com/duzagato/lambda-cria-rifa.git
cd lambda-cria-rifa
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Compile a solução:
```bash
dotnet build
```

## ⚙️ Configuração

### Banco de Dados (Opcional)

Se você tiver PostgreSQL instalado, edite o arquivo `src/LambdaCriaRifa.Application/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=rifas_db;Username=postgres;Password=sua_senha"
  }
}
```

**Nota**: A aplicação continuará funcionando mesmo sem o banco de dados configurado, mas as operações de persistência falharão.

### Mensagens Mockadas

As mensagens SQS mockadas estão em `src/LambdaCriaRifa.Application/MockData/sqs-messages.json`. 
Você pode editar este arquivo para adicionar ou modificar as mensagens de teste.

## ▶️ Execução

### Executar a aplicação:

```bash
dotnet run --project src/LambdaCriaRifa.Application
```

Ou navegue até o diretório do projeto:

```bash
cd src/LambdaCriaRifa.Application
dotnet run
```

### O que acontece ao executar:

1. A aplicação inicia e configura a injeção de dependências
2. Verifica/cria o banco de dados (se configurado)
3. O SQS Worker começa a processar as mensagens mockadas
4. Cada mensagem é processada pelo `CriaRifaHandler`
5. O serviço `RifaService` aplica as regras de negócio
6. Os dados são persistidos via `RifaRepository` (se banco configurado)
7. Logs detalhados são exibidos no console

### Exemplo de saída:

```
==============================================
Lambda Cria Rifa - Aplicação em Camadas
==============================================
Pressione Ctrl+C para encerrar

info: LambdaCriaRifa.Application.Workers.SqsWorker[0]
      SQS Worker iniciado. Processando mensagens mockadas...
info: LambdaCriaRifa.Application.Workers.SqsWorker[0]
      Encontradas 3 mensagens para processar
info: LambdaCriaRifa.Application.Handlers.CriaRifaHandler[0]
      Processando mensagem para criar rifa
info: LambdaCriaRifa.Domain.Services.RifaService[0]
      Criando nova rifa: Rifa Notebook Dell
info: LambdaCriaRifa.Domain.Services.RifaService[0]
      Rifa criada com sucesso. ID: 12345678-1234-1234-1234-123456789012
```

## 🧪 Testes

Execute os testes unitários:

```bash
dotnet test
```

## 🏗️ Build

Para compilar a solução completa:

```bash
dotnet build
```

Para compilar em modo Release:

```bash
dotnet build -c Release
```

## 📝 Funcionalidades

- ✅ Arquitetura em camadas (Application, Domain, Infra)
- ✅ Separação de responsabilidades
- ✅ Injeção de dependências configurada
- ✅ SQS mockado para testes locais
- ✅ Entity Framework Core com PostgreSQL
- ✅ Logging estruturado
- ✅ Validações de regras de negócio
- ✅ Executável localmente sem infraestrutura AWS

## 🔧 Tecnologias Utilizadas

- .NET 8.0
- Entity Framework Core 8.0
- Npgsql (PostgreSQL)
- Microsoft.Extensions.Hosting
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Logging

## 📚 Fluxo de Execução

1. **Aplicação inicia** → `Program.cs` configura DI e inicia o host
2. **SQS Worker** → Lê mensagens do arquivo JSON mockado
3. **Handler** → `CriaRifaHandler` processa cada mensagem
4. **Service** → `RifaService` aplica regras de negócio e validações
5. **Repository** → `RifaRepository` persiste dados no banco
6. **DbContext** → `AppDbContext` gerencia a conexão com PostgreSQL

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.

## 📄 Licença

Este projeto está sob a licença MIT.