# Roadmap — Plataforma de Carteira Digital (Digital Wallet)

> Projeto de portfólio com foco em fintech, microsserviços e boas práticas de arquitetura.
> Guiado por Claude Code — atualizado conforme o progresso.

---

## Objetivo

Construir uma plataforma de carteira digital do zero até o deploy em nuvem, aplicando:
- Clean Architecture + DDD
- Clean Code + SOLID
- Microsserviços com comunicação assíncrona
- Baixa latência com Redis
- Docker, CI/CD e cloud

---

## Visão Geral da Arquitetura

```
┌─────────────────────────────────────────────────────┐
│                   API Gateway                        │
│              (YARP / Ocelot)                         │
└────────┬──────────┬──────────┬──────────┬───────────┘
         │          │          │          │
    ┌────▼───┐ ┌────▼───┐ ┌───▼────┐ ┌───▼──────┐
    │Account │ │Transact│ │Notific.│ │  Auth    │
    │Service │ │Service │ │Service │ │  Service │
    └────┬───┘ └────┬───┘ └───┬────┘ └──────────┘
         │          │         │
    ┌────▼───┐ ┌────▼────┐ ┌──▼──────────┐
    │SQL Svr │ │  Redis  │ │  RabbitMQ   │
    └────────┘ └─────────┘ └─────────────┘
```

---

## Fases do Projeto

### Fase 1 — Fundamentos Teóricos ✅ Concluída em 2026-05-20
> Antes de codar, entender o porquê de cada prática.

- [x] **Clean Code** — nomenclatura, funções pequenas, SRP
- [ ] **SOLID** — os 5 princípios com exemplos práticos em C#
- [x] **Clean Architecture** — camadas, dependências, por que separar
- [x] **DDD (Introdução)** — Domain, Aggregates, Entities, Value Objects
- [ ] **Microsserviços** — quando faz sentido, trade-offs vs monolito
- [x] Configurar ambiente: .NET 8, Docker Desktop, Git, VS Code

**Entregável:** conceitos aplicados na prática durante o Auth Service.

---

### Fase 2 — Auth Service ✅ Concluída em 2026-05-21
> Primeiro serviço: simples, mas com a estrutura correta desde o início.

- [x] Criar solução com Clean Architecture (Domain / Application / Infrastructure / API)
- [x] Implementar registro e login de usuários
- [x] JWT gerado com chave configurável
- [x] Hash de senha com BCrypt
- [x] Testes unitários na camada de domínio (4 testes)
- [x] Middleware de tratamento de erros (DomainException → 422)
- [x] Docker + SQL Server rodando localmente
- [x] Migration do EF Core aplicada automaticamente no startup
- [x] Dockerfile para o serviço
- [ ] Refresh token (futuro)

**Entregável:** API de autenticação rodando em container Docker. ✅

---

### Fase 3 — Account Service ✅ Concluída em 2026-05-25
> Domínio mais rico — aqui o DDD começa a fazer sentido.

- [x] Aggregate `Account` com regras de negócio (saldo não pode ser negativo, etc.)
- [x] Value Object `Money` (valor + moeda)
- [x] Repository pattern com Entity Framework + SQL Server
- [x] CQRS com MediatR (Commands para escrita, Queries para leitura)
- [x] Endpoint: criar conta, consultar saldo, extrato
- [x] Testes unitários no domínio
- [x] Dockerfile

**Entregável:** serviço de contas com DDD real, rodando em Docker. ✅

---

### Fase 4 — Transaction Service ✅ Concluída em 2026-05-25
> O coração da aplicação — onde mora a baixa latência.

- [x] Aggregate `Transaction` com regras (transferência, depósito, saque)
- [x] **Idempotency Key** — evitar transações duplicadas
- [x] **Redis** como cache de idempotency key (cache-aside pattern)
- [x] Publicar evento `TransactionCreated` no RabbitMQ ao concluir transação
- [x] Testes unitários no domínio
- [x] Dockerfile

**Conceitos-chave:** idempotência, eventual consistency, cache-aside pattern.

**Entregável:** serviço de transações com baixa latência e publicação de eventos. ✅

---

### Fase 5 — Notification Service ✅ Concluída em 2026-05-26
> Consumidor de eventos — introdução a arquitetura orientada a eventos.

- [x] Consumir evento `TransactionCreated` do RabbitMQ com MassTransit
- [x] Enviar e-mail de confirmação (MailKit + MailHog para desenvolvimento)
- [ ] Padrão Outbox para garantir entrega (opcional/avançado)
- [x] Dockerfile
- [x] SharedContracts — contrato de evento compartilhado entre serviços

**Entregável:** notificações automáticas via fila de mensagens. ✅

---

### Fase 6 — API Gateway ✅ Concluída em 2026-05-30
> Ponto de entrada único — unifica os serviços para o frontend.

- [x] Configurar YARP como reverse proxy
- [x] Roteamento para cada serviço
- [x] Rate limiting por IP (100 req/min, fixed window)
- [x] Validação de JWT centralizada
- [x] Dockerfile

**Entregável:** um único endpoint que roteia para todos os serviços. ✅

---

### Fase 7 — Frontend Angular (Semanas 10-11)
> Interface que consome a API Gateway.

- [ ] Estrutura de módulos e lazy loading
- [ ] Tela de login e registro
- [ ] Dashboard com saldo e extrato
- [ ] Tela de transferência com feedback em tempo real
- [ ] HTTP Interceptor para JWT
- [ ] Guards de rota (rotas protegidas)
- [ ] Dockerfile + Nginx para servir o build

**Entregável:** frontend Angular completo rodando em container.

---

### Fase 8 — Docker Compose + Ambiente Local Completo (Semana 12)
> Tudo rodando com um único comando.

- [ ] `docker-compose.yml` com todos os serviços
- [ ] Variáveis de ambiente com `.env`
- [ ] Health checks para cada serviço
- [ ] Volumes para persistência do SQL Server e Redis

**Entregável:** `docker-compose up` sobe toda a plataforma localmente.

---

### Fase 9 — CI/CD e Deploy em Nuvem (Semanas 13-14)
> Projeto no ar, link no README.

- [ ] Repositório no GitHub com branches organizadas (`main`, `develop`)
- [ ] GitHub Actions: build + testes + push de imagem Docker
- [ ] Deploy no **Railway** ou **Azure** (App Service + Azure SQL + Azure Cache for Redis)
- [ ] Variáveis de ambiente seguras (GitHub Secrets)
- [ ] Badge de CI no README

**Entregável:** aplicação no ar com URL pública e pipeline verde.

---

### Fase 10 — AI Features com Claude SDK (Semana 15)
> Integração de inteligência artificial sobre a fundação enterprise já construída.

- [ ] Adicionar pacote `Anthropic` (SDK C# oficial) ao projeto
- [ ] Criar `AIService` como microsserviço separado ou módulo interno
- [ ] **Análise de fraude**: detectar padrões suspeitos em transações via Claude
- [ ] **Categorização de gastos**: classificar transações automaticamente (alimentação, transporte, etc.)
- [ ] **Resumo financeiro**: gerar insights mensais sobre os gastos do usuário
- [ ] Integrar com Transaction Service via eventos RabbitMQ
- [ ] Dockerfile

**Conceito-chave:** AI como capacidade enterprise integrada à arquitetura, não como demo isolado.

**Entregável:** features de AI funcionando sobre microsserviços reais com arquitetura sólida.

---

### Fase 11 — Polimento do Portfólio (Semana 16)
> O que o recrutador vai ver primeiro.

- [ ] README profissional: arquitetura, tecnologias, como rodar, link do deploy
- [ ] Diagrama de arquitetura no README
- [ ] Cobertura de testes visível
- [ ] Commits com mensagens descritivas e histórico limpo
- [ ] Vídeo curto de demonstração (opcional, mas diferencia muito)

**Entregável:** repositório pronto para mostrar em entrevista.

---

## Stack Resumida

| Área | Tecnologia |
|---|---|
| Backend | .NET 8 Web API |
| Frontend | Angular 18 |
| Banco de dados | SQL Server |
| Cache | Redis |
| Mensageria | RabbitMQ + MassTransit |
| ORM | Entity Framework Core |
| CQRS | MediatR |
| API Gateway | YARP |
| Containers | Docker + Docker Compose |
| CI/CD | GitHub Actions |
| Nuvem | Railway ou Azure |
| AI | Claude SDK (C# / Anthropic) |

---

## Conceitos Praticados

| Conceito | Onde aparece |
|---|---|
| Clean Architecture | Todos os serviços |
| DDD | Account Service, Transaction Service |
| Clean Code + SOLID | Todos os serviços |
| CQRS | Account Service, Transaction Service |
| Domain Events | Transaction Service |
| Idempotência | Transaction Service |
| Cache-aside Pattern | Transaction Service + Redis |
| Event-driven Architecture | RabbitMQ + Notification Service |
| AI Integration | AI Service + Claude SDK |
| Microsserviços | Toda a plataforma |
| Docker / Containers | Todos os serviços |
| CI/CD | GitHub Actions |

---

## Status Atual

- [x] Definição do projeto e arquitetura
- [x] Fase 1 — Fundamentos Teóricos ✅
- [x] Fase 2 — Auth Service ✅ (completo — rodando em Docker com SQL Server)
- [x] Fase 3 — Account Service ✅ (completo — rodando em Docker com SQL Server)
- [x] Fase 4 — Transaction Service ✅ (completo — Redis, RabbitMQ, idempotência)
- [x] Fase 5 — Notification Service ✅ (completo — consumer RabbitMQ, e-mail via MailKit)
- [x] Fase 6 — API Gateway ✅ (YARP, JWT centralizado, rate limiting por IP)
- [ ] Fase 7 — Frontend Angular
- [ ] Fase 8 — Docker Compose
- [ ] Fase 9 — CI/CD e Deploy
- [ ] Fase 10 — AI Features
- [ ] Fase 11 — Polimento do Portfólio

---

> Atualizado em: 2026-05-25
> Guiado por: Claude Code
