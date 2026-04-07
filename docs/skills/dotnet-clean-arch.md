# Skill: dotnet-clean-arch

## Purpose
Preserve Clean Architecture boundaries in module-level implementation changes.

## Use When
- Adding a feature across `Domain`, `Application`, `Infrastructure`, and `Contracts`.
- Deciding where new logic should be placed.
- Reviewing layered dependency direction.

## Guardrails
- `Domain` has no infrastructure concerns.
- `Application` orchestrates use cases and abstractions.
- `Infrastructure` implements IO/persistence details.
- `Contracts` expose external DTO/interfaces only.

## Quick Checklist
- New logic is in the correct layer.
- Dependency direction is preserved.
- Controller contains no business logic.
- New integration points are contracts-first.

