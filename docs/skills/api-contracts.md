# Skill: api-contracts

## Purpose
Design and evolve contracts safely for HTTP/API and cross-module communication.

## Use When
- Creating or changing DTOs/interfaces in `*.Contracts`.
- Introducing cross-module calls.
- Evolving endpoint payloads or response shapes.

## Guardrails
- Contracts must be explicit and purpose-specific.
- Cross-module calls must depend on interfaces in `Contracts`.
- Avoid leaking internal domain/infrastructure concerns into contracts.
- If compatibility breaks, update all known consumers in the same change set.

## Quick Checklist
- Contract change is minimal and clear.
- Ownership of interface is correct.
- Consumers and implementations are updated consistently.
- No direct module-to-module implementation dependency introduced.

