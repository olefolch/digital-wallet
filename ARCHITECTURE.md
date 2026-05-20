# Digital Wallet — Arquitetura e Convenções

Plataforma de carteira digital com foco em fintech, construída como projeto de portfólio.
Objetivo: demonstrar domínio de Clean Architecture, DDD, microsserviços e boas práticas de engenharia.

---

## Microsserviços

| Serviço | Responsabilidade |
|---|---|
| **Auth Service** | Autenticação, JWT, refresh token |
| **Account Service** | Contas, saldo, extrato (DDD rico) |
| **Transaction Service** | Transferências, depósitos, saques (baixa latência, Redis, idempotência) |
| **Notification Service** | Consumidor de eventos RabbitMQ, envio de e-mail |
| **API Gateway** | YARP, roteamento, rate limiting, validação de JWT |

Frontend: Angular 18 consumindo o API Gateway.

---

## Stack

| Área | Tecnologia |
|---|---|
| Backend | .NET 8 Web API |
| Frontend | Angular 18 |
| Banco de dados | SQL Server (um por serviço) |
| Cache | Redis |
| Mensageria | RabbitMQ + MassTransit |
| ORM | Entity Framework Core |
| CQRS | MediatR |
| API Gateway | YARP |
| Containers | Docker + Docker Compose |
| CI/CD | GitHub Actions |
| Nuvem | Railway ou Azure |

---

## Estrutura de Pastas (por serviço .NET)

```
ServiceName/
├── src/
│   ├── ServiceName.Domain/          # Entidades, Value Objects, Aggregates, Domain Events
│   ├── ServiceName.Application/     # Commands, Queries, Handlers, DTOs, Interfaces
│   ├── ServiceName.Infrastructure/  # EF Core, Repositórios, serviços externos
│   └── ServiceName.API/             # Controllers, Middlewares, Program.cs
└── tests/
    ├── ServiceName.Domain.Tests/
    └── ServiceName.Application.Tests/
```

---

## Princípios de Arquitetura

1. **Dependências sempre apontam para dentro** — Infrastructure depende de Application, Application depende de Domain. Nunca o contrário.
2. **Domain não referencia nada externo** — sem EF Core, sem bibliotecas de terceiros na camada de domínio.
3. **Lógica de negócio vive no Domain** — Application orquestra, Domain decide.
4. **Sem lógica nos Controllers** — controllers apenas recebem, delegam e respondem.
5. **Value Objects são imutáveis** — criados via construtor ou factory method, sem setters públicos.

---

## Convenções de Código

### Nomenclatura
- Classes, métodos, propriedades: **PascalCase**
- Variáveis locais, parâmetros: **camelCase**
- Campos privados: **_camelCase**
- Interfaces: prefixo `I` — `IUserRepository`, `IJwtTokenGenerator`
- Código em **inglês**, comentários em **português**

### Commits
Seguem o padrão **Conventional Commits** em português:
```
feat(auth-service): adicionar endpoint de login
fix(account-service): corrigir cálculo de saldo
test(domain): adicionar testes da entidade User
chore: configurar Docker Compose
```

### Testes
Nomenclatura: `Metodo_Contexto_ResultadoEsperado`

Exemplos:
- `Create_WithValidData_ShouldReturnUser`
- `Transfer_WhenBalanceIsInsufficient_ShouldThrowDomainException`

---

## Padrões de API REST

- Rotas em kebab-case com versionamento: `/api/v1/accounts`
- Valores monetários sempre em centavos (evita ponto flutuante)
- Operações financeiras exigem `Idempotency-Key` no header
- Erros retornam formato RFC 7807 (Problem Details)
