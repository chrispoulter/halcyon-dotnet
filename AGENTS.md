# Repository Guidelines

## Purpose

- This file guides agentic coding assistants working in this repository.
- Follow existing project conventions; prefer minimal, focused changes.
- Update this document if new tools or conventions are introduced.

## Project Layout

- `Halcyon.Api/` contains the ASP.NET Core API and EF Core data access.
- `Halcyon.Web/` contains the React + Vite frontend.
- `Halcyon.AppHost/` is the .NET Aspire host for local orchestration.
- `Halcyon.ServiceDefaults/` provides shared service defaults.
- Root solution file: `halcyon.slnx`.

## Build, Run, Lint, Test

### Backend (.NET)

- Restore: `dotnet restore`.
- Build all projects: `dotnet build`.
- Run the Aspire host: `dotnet run --project "Halcyon.AppHost/Halcyon.AppHost.csproj"`.
- Run API only (if needed): `dotnet run --project "Halcyon.Api/Halcyon.Api.csproj"`.
- Migrations are applied at startup via background service; avoid manual edits.

### Frontend (React)

- Install deps (from `Halcyon.Web/`): `npm install`.
- Dev server: `npm run dev`.
- Build: `npm run build`.
- Lint: `npm run lint`.
- Format: `npm run format`.

### Tests

- No automated tests exist yet.
- If tests are added, prefer `Halcyon.*.Tests` projects.
- Run all tests: `dotnet test`.
- Run a single test (if tests exist):
  - `dotnet test --filter FullyQualifiedName~Namespace.TypeName.TestName`
  - `dotnet test --filter Category=Unit` (if categories are used)
- For frontend, add tests only if a test framework is introduced.

## Code Style

### C# Formatting

- C# formatting uses CSharpier (`.csharpierrc`).
- Indentation: 4 spaces.
- Line width: 100 columns.
- Prefer file-scoped namespaces.
- Use `var` where the type is obvious.

### TypeScript/React Formatting

- Frontend formatting uses Prettier with Tailwind plugin.
- Indentation: 4 spaces, single quotes.
- ESLint config is flat and extends recommended rules.
- TypeScript config is strict; avoid `any`.

## Naming Conventions

### C#

- Types/methods/properties: PascalCase.
- Locals/parameters: camelCase.
- Async methods: suffix `Async`.
- Interfaces: prefix `I`.
- Endpoint classes: `<Action>Endpoint`.
- Request/response DTOs: `<Action>Request`, `<Action>Response`.

### TypeScript/React

- Components: PascalCase.
- Hooks: `useThing` prefix.
- Files and folders: kebab-case.
- Feature folders: `src/features/<area>/<feature>`.
- Utility modules: `src/lib/`.

## Imports and Organization

### C#

- Group `using` directives at top of file.
- Prefer explicit namespaces for shared features.
- Keep endpoint wiring in `IEndpoint` implementations.

### TypeScript/React

- Use path aliases via `@/` for `src`.
- Order imports: external, then internal, then relative.
- Avoid unused imports; ESLint enforces this.

## API and Endpoint Patterns

- Endpoints implement `IEndpoint` and register in `MapEndpoints`.
- Use `WithTags`, `WithSummary`, `WithDescription` on endpoints.
- Use validation filters (`AddValidationFilter<T>`).
- Return `Results.Problem` for error cases using consistent messaging.
- Prefer `AsNoTracking` for read-only EF Core queries.

## Error Handling

### C#

- Prefer returning `Results.Problem` for client-visible errors.
- Use `ProblemDetails` and status codes consistently.
- Avoid throwing for expected validation errors.

### Frontend

- API errors are normalized in `src/lib/api-client.ts`.
- Prefer surfacing `ProblemDetails.title` for user-visible messages.
- Use TanStack Query hooks for data fetching and error states.

## Types and Validation

- Backend uses FluentValidation; keep validators near request types.
- Frontend uses Zod for forms; co-locate schema with form.
- Keep DTOs and UI models separate where reasonable.

## Data Access

- EF Core context: `HalcyonDbContext`.
- Prefer async query APIs and cancellation tokens.
- Avoid tracking for read queries unless update is required.

## UI Patterns

- UI components live in `Halcyon.Web/src/components`.
- shadcn/ui components are in `src/components/ui`.
- Use Tailwind utilities via class strings; Prettier handles ordering.

## Configuration and Secrets

- Backend secrets: `Halcyon.Api/appsettings.Development.json` (gitignored).
- Frontend runtime values: `Halcyon.Web/.env.local`.
- Do not commit real credentials.

## Commit and PR Guidelines

- Commit messages are short and descriptive (imperative or descriptive).
- Include issue numbers when relevant, e.g. `(#116)`.
- PRs should include a summary, manual test steps, and screenshots for UI.

## Cursor/Copilot Rules

- No `.cursor/rules/`, `.cursorrules`, or `.github/copilot-instructions.md` found.
- If added later, copy the instructions here.
