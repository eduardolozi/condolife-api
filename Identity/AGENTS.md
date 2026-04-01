# AGENTS - Identity Module

## Objetivo do módulo
`Identity` é responsável por usuários, convites e vínculos de usuário com condomínio (membership), incluindo papéis.

## Estrutura e responsabilidades
- `Identity.Domain`: entidades (`User`, `Invite`, `CondominiumMembership`), enums e exceções de domínio.
- `Identity.Application`: casos de uso, comandos, validators, handlers e abstrações (`IIdentityDbContext`).
- `Identity.Infrastructure`: `IdentityDbContext`, migrations e registro de DI.
- `Identity.Contracts`: contratos públicos para integração de outros módulos com `Identity`.

## Regras locais de implementação
- Todo caso de uso novo deve entrar em `Identity.Application` com comando/resultado explícitos.
- Validação de entrada deve usar FluentValidation na camada `Application`.
- Persistência deve usar `IIdentityDbContext`; não acessar `IdentityDbContext` diretamente fora de `Infrastructure`.
- Schema de banco do módulo é `identity`; alterações de estrutura exigem migration no projeto `Infrastructure`.
- Use `externalUserId` como chave de fronteira para usuário autenticado em fluxos de aplicação.

## Integração com outros módulos
- Expor integrações para outros módulos via `Identity.Contracts`.
- Não depender de `Application` ou `Infrastructure` de outros módulos.
- Não acessar tabela/contexto de outro módulo diretamente.

## API relacionada ao contexto Identity
Controllers no host (`Condolife.Api`) que pertencem a este contexto:
- `Controllers/Identity/UserController.cs`
- `Controllers/Identity/CondominiumMembershipController.cs`
- `Controllers/Identity/InviteController.cs`

Regras:
- Controller delega para UseCase; sem regra de negócio.
- Claims devem ser extraídas por extensões compartilhadas.

## Fluxos padrão de mudança
- Novo endpoint Identity: controller no host + use case/validator/DTO no módulo.
- Nova integração inbound/outbound: contrato em `Identity.Contracts` + handler em `Identity.Application`.
- Mudança de banco: ajustar `IdentityDbContext`, gerar migration e revisar impacto nos use cases.

## Skills recomendadas
- `dotnet-clean-arch`
- `dotnet-pro`
- `efcore-postgres`
- `api-contracts`
- `modular-monolith`

