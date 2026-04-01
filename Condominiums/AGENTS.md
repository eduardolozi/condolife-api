# AGENTS - Condominiums Module

## Objetivo do módulo
`Condominiums` é responsável por cadastro e consulta de condomínios, torres e dados de endereço (cidade/estado).

## Estrutura e responsabilidades
- `Condominiums.Domain`: entidades (`Condominium`, `Tower`, `City`, `State`, `Address`) e exceções de domínio.
- `Condominiums.Application`: casos de uso, validações, handlers e abstrações (`ICondominiumDbContext`).
- `Condominiums.Infrastructure`: `CondominiumDbContext`, migrations, seeds SQL e registro de DI.
- `Condominiums.Contracts`: contratos públicos para consumo por outros módulos.

## Regras locais de implementação
- Casos de uso novos devem ser implementados em `Condominiums.Application` com comandos/DTOs explícitos.
- Validação de entrada deve usar FluentValidation em `Application`.
- Persistência deve usar `ICondominiumDbContext`; acesso direto ao `DbContext` concreto fica em `Infrastructure`.
- Schema de banco do módulo é `condominium`; alteração estrutural exige migration no projeto `Infrastructure`.
- Em fluxos de criação, validar consistência de cidade/UF antes de persistir.

## Integração com outros módulos
- Integrações cross-module devem ser feitas por contratos.
- Para integração com `Identity`, usar interfaces de `Identity.Contracts` (ex.: criação de membership após criar condomínio).
- Não depender de `Identity.Application` ou `Identity.Infrastructure`.
- Não acessar dados de outro módulo por tabela/contexto direto.

## API relacionada ao contexto Condominiums
Controller no host (`Condolife.Api`) que pertence a este contexto:
- `Controllers/Condominiums/CondominiumController.cs`

Regras:
- Controller apenas recebe HTTP, extrai contexto do usuário e delega para UseCase.
- Não inserir regra de negócio no controller.

## Fluxos padrão de mudança
- Novo caso de uso: comando + validator + use case em `Condominiums.Application`.
- Novo contrato público: definir em `Condominiums.Contracts` e implementar handler correspondente em `Application`.
- Mudança de dados de endereço: ajustar entidades/consultas, validar impactos e atualizar migration.

## Skills recomendadas
- `dotnet-clean-arch`
- `dotnet-pro`
- `efcore-postgres`
- `api-contracts`
- `modular-monolith`

