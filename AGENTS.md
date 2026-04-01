# AGENTS - Condolife API

## Objetivo
Este arquivo define regras globais para agentes que trabalham no monolito modular `condolife-api`.

## Arquitetura global
- A solução usa monolito modular com módulos independentes por domínio (`Identity`, `Condominiums`, `Notifications`).
- Cada módulo segue Clean Architecture em quatro projetos: `Domain`, `Application`, `Infrastructure`, `Contracts`.
- `Condolife.Api` é o host HTTP e ponto de composição de dependências.

## Regras de camadas
- `Domain`: entidades, enums e exceções de domínio; não depende de `Application`/`Infrastructure`.
- `Application`: casos de uso, validações, interfaces de persistência e handlers de contrato.
- `Infrastructure`: implementações de banco e DI do módulo.
- `Contracts`: interfaces e DTOs de integração entre módulos.

## Regras de dependência entre módulos
- Integração entre módulos deve ocorrer via `*.Contracts`.
- Não criar referência direta de `Application` ou `Infrastructure` de um módulo para `Application`/`Infrastructure` de outro módulo.
- Composição de módulos deve ser feita no host (`Condolife.Api`) e nos `DependencyResolver` de cada módulo.

## Regras de API
- Controllers em `Condolife.Api` apenas recebem HTTP, extraem contexto de autenticação e delegam para UseCases.
- Não colocar regra de negócio em controllers.
- Tratamento de erros deve continuar centralizado em exception handlers globais.

## Skills globais
Use estas skills como base técnica para qualquer módulo:
- `modular-monolith`: decisões de fronteira e acoplamento entre módulos.
- `dotnet-pro`: padrões avançados de C#/.NET e organização de solução.
- `dotnet-clean-arch`: aplicação consistente de Clean Architecture.
- `efcore-postgres`: modelagem, migrations e consultas com EF Core + PostgreSQL.
- `api-contracts`: design e evolução de contratos entre módulos e HTTP.

## Checklist mínimo para mudanças
- A mudança respeita fronteiras do módulo e direção de dependência.
- Toda regra de negócio nova está em `Application`/`Domain`, não no controller.
- Integração cross-module usa `Contracts` e DI.
- Se houver mudança de persistência, inclui migration e ajuste de contexto do módulo.
- Validação de entrada está em FluentValidation no módulo correto.

