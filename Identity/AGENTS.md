# Identity Module - AGENTS.md

## 1. Purpose
This file is a local overlay for the `Identity/` bounded context.
Use it together with the root `AGENTS.md`; do not duplicate global architecture and workflow rules here.

Identity owns user identity lifecycle and condominium membership access context.

## 2. Ownership Boundaries
Identity owns:
- User identity records (`ExternalId`, email, name) and idempotent user bootstrap.
- Membership records between user and condominium, including role assignment.
- Contracts exported to other modules for identity-driven operations.

Identity does not own:
- Condominium master data (name, address, city/state, towers).
- Address lookup data and location normalization.
- HTTP host concerns (middleware, global auth wiring, global exception handling).

Decision rule:
- If the behavior changes who the user is or what memberships/roles they have, it belongs to Identity.
- If it changes condominium/address source-of-truth data, it belongs to Condominiums.

## 3. Local Invariants
- `ExternalId` uniquely identifies a user from the auth provider boundary.
- User bootstrap must remain idempotent for the same authenticated principal.
- Membership creation requires an existing user mapped by `ExternalId`.
- Membership must always bind exactly one `UserId` to one `CondominiumId` with an explicit `UserRole`.
- Membership writes must persist through Identity-owned data access only.
- Membership read flows may enrich response data via Contracts, never by direct cross-module DB access.
- Identity is the source of truth for role assignment semantics in memberships.
- New validation rules for identity commands must live in `Identity.Application`.

## 4. Integration Contracts (Inbound / Outbound)
Inbound (implemented by Identity, consumed by other modules):
- `Identity.Contracts.CondominiumMemberships.CreateCondominiumMembership.ICreateCondominiumMembershipHandler`
  used by Condominiums after successful condominium creation.

Outbound (consumed by Identity, implemented by other modules):
- `Condominiums.Contracts.Addresses.GetAddressesInfos.IGetCondominiumsAddressInfosHandler`
  used to enrich membership responses with condominium address metadata.

Contract boundary rule:
- Cross-module communication must use `*.Contracts` interfaces only.
- Do not reference other modules' `Application` or `Infrastructure` projects.

## 5. Change Playbooks
Add endpoint (Identity context):
1. Add or update controller action in `Condolife.Api/Controllers/Identity`.
2. Add command/result/use case in `Identity.Application`.
3. Add/update FluentValidation validator if input semantics changed.
4. Keep controller thin: auth context extraction + use case delegation only.

Add application flow:
1. Place orchestration in `Identity.Application`.
2. Use `IIdentityDbContext` abstraction, not infrastructure concrete type.
3. If external module data is required, consume it via `*.Contracts`.

Add or evolve contract:
1. Add interface/DTO in `Identity.Contracts`.
2. Implement handler in `Identity.Application`.
3. Register DI in module dependency resolver.
4. Preserve backward compatibility or update all consumers in same change set.

Add migration:
1. Update `Identity.Infrastructure/IdentityDbContext` model mapping.
2. Add migration under `Identity.Infrastructure/Migrations`.
3. Validate startup wiring and build from solution root.

Critical flows to keep stable:
- Authenticated user bootstrap (`POST api/User/me`).
- Authenticated membership listing (`GET api/CondominiumMembership/me`).
- Cross-module membership creation triggered by Condominiums contract call.

## 6. Anti-Patterns To Avoid
- Duplicating root `AGENTS.md` content in this file.
- Direct DB or entity access into Condominiums data from Identity.
- Pushing business logic into API controllers.
- Creating entity-by-entity documentation here; document only non-obvious invariants.
- Maintaining a full endpoint catalog here; document only critical/high-risk flows.

## 7. Module Skills (Reference)
- `docs/skills/modular-monolith.md`
- `docs/skills/dotnet-pro.md`
- `docs/skills/dotnet-clean-arch.md`
- `docs/skills/efcore-postgres.md`
- `docs/skills/api-contracts.md`
