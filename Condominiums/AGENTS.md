# Condominiums Module - AGENTS.md

## 1. Purpose
This file is a local overlay for the `Condominiums/` bounded context.
Use it with the root `AGENTS.md`; keep global architecture and workflow guidance at root level only.

Condominiums owns condominium registry data and address/location-backed creation and retrieval flows.

## 2. Ownership Boundaries
Condominiums owns:
- Condominium aggregate creation and persistence.
- Address-linked data consistency for condominium records.
- City/state lookup usage for write and read-side projection flows.
- Contracts exported to other modules for condominium/address data access.

Condominiums does not own:
- User identity lifecycle and membership role semantics.
- Auth provider identity mapping rules.
- HTTP host middleware/auth infrastructure concerns.

Decision rule:
- If the behavior changes condominium or location source-of-truth data, it belongs to Condominiums.
- If it changes who can access/role semantics for user membership, it belongs to Identity.

## 3. Local Invariants
- `IbgeCode` must resolve to exactly one known city before condominium creation.
- Provided `StateCode` must match the resolved city state.
- Condominium persistence must complete before triggering cross-module membership creation.
- Address fields used in outbound projections must be read from condominium-owned data.
- City and state lookup constraints remain module-owned consistency guards.
- New write validations must be implemented in `Condominiums.Application` validators.
- Read projections should remain explicit and avoid over-fetching.
- Cross-module side effects must be performed through Contracts only.

## 4. Integration Contracts (Inbound / Outbound)
Inbound (implemented by Condominiums, consumed by other modules):
- `Condominiums.Contracts.Addresses.GetAddressesInfos.IGetCondominiumsAddressInfosHandler`
  used by Identity to enrich membership responses.

Outbound (consumed by Condominiums, implemented by other modules):
- `Identity.Contracts.CondominiumMemberships.CreateCondominiumMembership.ICreateCondominiumMembershipHandler`
  called after condominium creation to create initial membership.

Contract boundary rule:
- Cross-module integration must stay in `*.Contracts`.
- Do not depend on Identity `Application`/`Infrastructure`.

## 5. Change Playbooks
Add create/read endpoint (Condominiums context):
1. Add or update controller action in `Condolife.Api/Controllers/Condominiums`.
2. Add command/result/use case in `Condominiums.Application`.
3. Add/update validator for input and business preconditions.
4. Keep controller thin: auth context extraction + use case call.

Add application flow:
1. Orchestrate in `Condominiums.Application`.
2. Use `ICondominiumDbContext` abstraction only.
3. Use contract handlers for external side effects or data needs.

Add or evolve contract:
1. Add interface/DTO in `Condominiums.Contracts`.
2. Implement adapter/handler in `Condominiums.Application`.
3. Register DI in module dependency resolver.
4. Update consumers in same change set when compatibility is not preserved.

Add migration:
1. Update `Condominiums.Infrastructure/CondominiumDbContext` mappings.
2. Add migration under `Condominiums.Infrastructure/Migrations`.
3. Validate model snapshot consistency and build from solution root.

Critical flows to keep stable:
- Condominium creation with city/state validation.
- Post-create contract call to Identity membership handler.
- Address information projection handler for external module consumption.

## 6. Anti-Patterns To Avoid
- Repeating root `AGENTS.md` architecture content in this file.
- Directly writing Identity-owned data from Condominiums.
- Running cross-module logic without contract interfaces.
- Documenting every entity or every endpoint in this file.
- Adding controller business logic that belongs in use cases.

## 7. Module Skills (Reference)
- `docs/skills/modular-monolith.md`
- `docs/skills/dotnet-pro.md`
- `docs/skills/dotnet-clean-arch.md`
- `docs/skills/efcore-postgres.md`
- `docs/skills/api-contracts.md`
