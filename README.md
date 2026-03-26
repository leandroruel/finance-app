# FinanceApp

Aplicação de finanças fullstack construída com C# e .NET 8, seguindo Clean Architecture, DDD e TDD. O objetivo é simular um sistema bancário com domínios de clientes, contas, transações e cartões — usando o ecossistema AWS emulado localmente via LocalStack.

## Objetivo

Servir como referência prática de uma aplicação fintech com boas práticas de arquitetura e infraestrutura:

- **Clean Architecture** com separação clara entre Domain, Application, Infrastructure e API
- **DDD** com aggregates, value objects e domain events por domínio
- **CQRS** via MediatR com commands, queries e pipeline behaviors
- **TDD** com xUnit, FluentAssertions e Testcontainers
- **Event-driven** com MassTransit + Amazon SQS + Outbox Pattern

## Domínios

| Domínio | Responsabilidade |
|---|---|
| Customer | Cadastro e verificação de clientes (KYC) |
| Account | Contas corrente, poupança e investimento |
| Transaction | Depósitos, saques e transferências |
| Card | Cartões de débito e crédito |

## Stack

| Camada | Tecnologia |
|---|---|
| API | ASP.NET Core 8 |
| Banco | PostgreSQL 16 + EF Core 9 |
| Mensageria | MassTransit 9 + Amazon SQS (LocalStack) |
| Auth | Keycloak (OAuth2/OIDC) |
| Storage | Amazon S3 (LocalStack) |
| Tracing | OpenTelemetry + Jaeger |
| Infra-as-Code | Terraform |
| Orquestração | Kubernetes + Helm |
| CI/CD | GitHub Actions |

## Como rodar localmente

**Pré-requisitos:** Docker, .NET 8 SDK, Terraform (opcional)

```bash
# 1. Subir infraestrutura local
cd docker && docker-compose up -d

# 2. Aplicar migrations
dotnet ef database update --project src/FinanceApp.Infrastructure --startup-project src/FinanceApp.API

# 3. Rodar a API
dotnet run --project src/FinanceApp.API

# 4. Rodar os testes
dotnet test
```

A API estará disponível em `http://localhost:5000` com Swagger em `http://localhost:5000/swagger`.

## Serviços locais

| Serviço | URL |
|---|---|
| API | http://localhost:5000 |
| Keycloak | http://localhost:8080 |
| LocalStack (AWS) | http://localhost:4566 |
| Jaeger (tracing) | http://localhost:16686 |
