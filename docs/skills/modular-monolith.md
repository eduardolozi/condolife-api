# Skill: modular-monolith

## Purpose
Guide architectural decisions in a modular monolith without introducing accidental coupling.

## Use When
- A change touches more than one bounded context.
- You need to decide where behavior should live.
- You need to design or review cross-module communication.

## Guardrails
- Keep business logic in the owning module.
- Use `*.Contracts` for cross-module interaction.
- Never reference another module's `Application` or `Infrastructure`.
- Prefer local change first; expand scope only when required by ownership.

## Quick Checklist
- Owning module identified.
- No forbidden project reference added.
- Cross-module interaction done through contracts.
- Controller remains orchestration only.

