# Skill: efcore-postgres

## Purpose
Guide EF Core + PostgreSQL changes safely inside module infrastructure boundaries.

## Use When
- Changing `DbContext` mappings.
- Adding or updating migrations.
- Optimizing query projections.

## Guardrails
- Each module owns its own `DbContext` and migrations.
- Do not introduce repository abstractions in this codebase.
- Prefer `Select` projections over loading full graphs.
- Keep schema changes scoped to the owning module.

## Quick Checklist
- Mapping change is in the right module context.
- Migration is generated in the correct module infrastructure project.
- Query uses explicit projection where applicable.
- Build succeeds after migration/model changes.

