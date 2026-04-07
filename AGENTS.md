# AGENTS.md

## Purpose
This file defines the default agent rules for the entire repository. Use it as the baseline policy unless a more specific `AGENTS.md` exists inside the module being changed.

## Scope & Precedence
- This file applies repo-wide by default.
- If a subdirectory contains another `AGENTS.md`, that file overrides or extends this one for its subtree.
- The closest `AGENTS.md` to the changed files takes precedence.
- Module-level `AGENTS.md` files should contain only local rules or exceptions, not a copy of the full root policy.

## Repository Structure
This solution is a modular monolith.

- `Condolife.Api/`: HTTP entrypoint, controllers, middleware, DI, and runtime configuration
- `Condominiums/`, `Identity/`, `Notifications/`: bounded contexts

Each module follows the same layering:
- `Domain`: business rules only, no dependencies
- `Application`: use cases and orchestration, depends on Domain and Contracts
- `Contracts`: DTOs and interfaces exposed externally, no dependencies
- `Infrastructure`: EF Core and integrations, depends on Application

## Agent Working Rules
- Prefer minimal, focused changes.
- Do not modify unrelated files or modules.
- Keep changes inside the owning bounded context whenever possible.
- Respect existing layer boundaries and folder patterns.
- Do not introduce new cross-module dependencies unless the task clearly requires it.
- Add or update EF Core migrations only in the affected module's `Infrastructure` project.

## Coding & Architecture Guidelines
- Use standard C# conventions: 4-space indentation, file-scoped namespaces, `PascalCase` for types and public members, `camelCase` for locals and parameters.
- Keep controllers thin; business logic belongs in `Application`.
- Prefer existing use case, command, handler, and validator patterns over introducing new abstractions.
- Prefer explicit projections and avoid unnecessary data loading.

## Cross-Module Communication

- Modules must not depend on other modules' implementations.
- All cross-module interaction must happen through the `Contracts` layer.
- A module may reference another module's `Contracts`, but never its `Application` or `Infrastructure`.
- Interfaces are defined in `Contracts`, and implemented inside the owning module.
- Do not bypass Contracts to access another module directly.

## EF Core Usage

- Each module owns its own DbContext inside `Infrastructure`.
- Migrations are scoped per module.
- Do not introduce repository abstractions.

Instead:
- The `Application` layer defines an interface for the DbContext.
- This interface exposes `DbSet<>` and `SaveChangesAsync()`.
- The concrete DbContext in `Infrastructure` implements this interface.
- The interface is injected into Application services via DI.

- Prefer projections (`Select`) instead of loading full entities.

## Validation Expectations
- Always run `dotnet build condolife-api.sln` before finishing changes.
- If API behavior changes, verify the affected endpoint locally.
- There are currently no automated test projects in this repository. Treat build validation and targeted manual verification as the minimum standard.

## Development Commands
- `dotnet restore condolife-api.sln`
- `dotnet build condolife-api.sln`
- `dotnet run --project Condolife.Api`
- `docker compose up -d`
- `dotnet ef migrations add -p {Module}.Infrastructure -s Condolife.Api -c {Module}DbContext`
- `dotnet ef database update -p {Module}.Infrastructure -s Condolife.Api -c {Module}DbContext`

## Git & Configuration Rules
- Do not perform git workflow actions such as commits, rebases, branch creation, or pull requests unless explicitly requested.
- Do not commit real connection strings, JWT settings, or email credentials.
- Keep secrets out of tracked configuration files; prefer environment variables or local development secrets.

## When in Doubt

- Prefer the smallest change that solves the problem.
- Follow existing patterns before introducing new structures.
- Keep logic inside the owning module.
- Ask: "Which module owns this behavior?" before making changes.