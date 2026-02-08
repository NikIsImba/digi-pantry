# Agent Guide (digi-pantry)

This repo contains two apps:

- `digi-pantry-web/`: React 19 + TypeScript + Vite (rolldown-vite) + Tailwind v4
- `digi-pantry-backend/`: ASP.NET Core minimal API (`web-api/`, `net10.0`)

Use this doc as the default "how to work here" playbook for agentic coding tools.

## Commands (build/lint/test)

### Frontend (`digi-pantry-web/`)

Package manager: Bun (there is a `bun.lock`).

- Install deps: `cd digi-pantry-web && bun install`
- Dev server: `cd digi-pantry-web && bun run dev`
- Production build: `cd digi-pantry-web && bun run build`
- Preview build: `cd digi-pantry-web && bun run preview`

Lint/format:

- ESLint (configured): `cd digi-pantry-web && bun run lint`
- ESLint fix: `cd digi-pantry-web && bun run lint -- --fix`
- Biome check (lint + format + import organizing): `cd digi-pantry-web && bunx biome check .`
- Biome auto-fix: `cd digi-pantry-web && bunx biome check --write .`

Tests:

- There are currently no frontend test runners configured (no Vitest/Jest/Playwright).
- If/when Vitest is added, the usual single-test pattern is:
  `bun run test -- -t "test name"` or `bun run test -- path/to/file.test.ts`

### Backend (`digi-pantry-backend/`)

Requires a .NET SDK that supports `net10.0` (likely .NET 10 preview).

- Restore: `cd digi-pantry-backend && dotnet restore`
- Build: `cd digi-pantry-backend && dotnet build`
- Run API: `cd digi-pantry-backend && dotnet run --project web-api/web-api.csproj`
- Run w/ hot reload: `cd digi-pantry-backend && dotnet watch --project web-api/web-api.csproj run`

Tests:

- There are currently no backend test projects in this repo.
- If/when a test project is added, run all tests:
  `cd digi-pantry-backend && dotnet test`
- Run a single test (xUnit/NUnit/MSTest all support `--filter`):
  - By fully-qualified name contains:
    `dotnet test --filter "FullyQualifiedName~Namespace.ClassName.Method"`
  - By exact test name:
    `dotnet test --filter "Name=MyTest"`
  - By trait/category (example):
    `dotnet test --filter "Category=Integration"`

## Repo rules (Cursor/Copilot)

- Cursor rules: none found (`.cursor/rules/` and `.cursorrules` do not exist).
- Copilot instructions: none found (`.github/copilot-instructions.md` does not exist).

## Code style (follow the repo, not personal taste)

### General

- Prefer small, reviewable PRs; keep changes focused.
- Don't commit generated/build outputs: `node_modules/`, `dist/`, `bin/`, `obj/`, etc.
- Don't commit secrets: `.env` is ignored in backend; treat `appsettings*.json` as potentially sensitive.
- Make failures loud: avoid swallowing errors; surface actionable messages.

### Frontend style (`digi-pantry-web/`)

Tooling:

- Formatting + import organization: Biome (`digi-pantry-web/biome.json`).
  - Indentation: tabs (Biome `indentStyle: tab`).
  - Quotes: single quotes (Biome `quoteStyle: single`).
  - Let the formatter decide semicolons/wrapping; don't hand-format.
- Linting: ESLint flat config (`digi-pantry-web/eslint.config.js`).
  - Includes `@eslint/js`, `typescript-eslint` recommended, `react-hooks`, `react-refresh`.

TypeScript:

- `strict: true` is enabled (`digi-pantry-web/tsconfig.app.json`). Don't weaken it.
- Avoid `any`. Prefer `unknown` for untrusted inputs and narrow with type guards.
- Use `import type { ... }` for type-only imports when it improves clarity.
- Keep public component props typed and exported types stable.

Imports:

- Prefer absolute clarity over cleverness; keep import lists short.
- Run `bunx biome check --write .` to organize imports consistently.
- Don't rely on side-effect imports; TS config has `noUncheckedSideEffectImports: true`.

React:

- Use function components + hooks.
- Follow rules-of-hooks; keep effects minimal and dependency-correct.
- Prefer controlled components for forms; keep state close to where it's used.
- Keep UI pure: put async/data-fetching in hooks or dedicated modules.

Tailwind/CSS:

- Prefer Tailwind utilities for layout/spacing.
- If a set of classes repeats, extract a component or a small helper.
- Keep global CSS minimal (`digi-pantry-web/src/index.css`).

Error handling:

- For async UI flows, catch at boundaries, show user-safe messages, and log details in dev.
- Don't silently ignore promise rejections.

### Backend style (`digi-pantry-backend/`)

Tooling:

- Formatting/naming are guided by `digi-pantry-backend/.editorconfig`.
  - Indentation: 4 spaces for `*.cs`.
  - Prefer explicit types (the repo discourages `var`).
  - Prefer file-scoped namespaces.
  - Usings: System first; separate groups; outside namespace.

Naming:

- Types/namespaces/methods/properties/events: PascalCase.
- Interfaces: `IThing`.
- Type parameters: `TThing`.
- Private fields: `_camelCase`.
- Private static fields: `s_camelCase`.
- Parameters/local variables: camelCase.

Nullability + APIs:

- Nullable is enabled in the project (`<Nullable>enable</Nullable>`). Don't suppress warnings unless necessary.
- Validate inputs early; prefer guard clauses.
- Favor `async` APIs and pass `CancellationToken` through when doing I/O.

Minimal API patterns:

- Keep `Program.cs` readable; extract endpoint groups/services as the app grows.
- Prefer returning `Results<...>` / `IResult` and use ProblemDetails for errors.
- Log with `ILogger<T>` (avoid `Console.WriteLine` in production code).

Error handling:

- Use structured logging and meaningful event contexts.
- Don't catch `Exception` unless you can add value (context, fallback, translation to ProblemDetails).
- Prefer not throwing for normal control flow; use validation and typed results.

## When you change code

- Frontend: run `cd digi-pantry-web && bun run lint` and (ideally) `bun run build`.
- Backend: run `cd digi-pantry-backend && dotnet build` (and `dotnet test` if/when tests exist).
- If formatting changes are expected, apply them with Biome (web) or editorconfig-aware tooling (C#).
