# Repository Guidelines

## Project Structure & Module Organization
- `Halcyon.Api/` is the ASP.NET Core API with EF Core, authentication, and feature endpoints.
- `Halcyon.Web/` is the React + Vite frontend (Tailwind, TanStack Query, shadcn/ui).
- `Halcyon.AppHost/` is the .NET Aspire app host used to run the distributed app locally.
- `Halcyon.ServiceDefaults/` provides shared service defaults and extensions.
- Root files include `halcyon.slnx`, `docker-compose.yml`, and CI config in `.github/`.

## Build, Test, and Development Commands
- `dotnet restore` restores NuGet packages for the solution.
- `dotnet build` builds all .NET projects in `halcyon.slnx`.
- `dotnet run --project "Halcyon.AppHost/Halcyon.AppHost.csproj"` runs the Aspire host and API.
- `npm install` (from `Halcyon.Web/`) installs frontend dependencies.
- `npm run dev` starts the Vite dev server on `http://localhost:5173`.
- `npm run build`, `npm run lint`, `npm run format` build, lint, and format the frontend.
- Verify changes by running `dotnet build` for backend work or `npm run build` from `Halcyon.Web/` for frontend work.

## Coding Style & Naming Conventions
- C# formatting uses CSharpier (`.csharpierrc`): 4-space indentation, 100-char line width.
- Frontend formatting uses Prettier and linting uses ESLint (`Halcyon.Web/`).
- Ensure updates follow CSharpier/Prettier formatting and ESLint rules for the areas touched.
- Use PascalCase for C# types/methods, camelCase for locals, and kebab-case for feature folders.
- Keep feature endpoints grouped under `Halcyon.Api/Features/<Area>/<Feature>/`.

## Testing Guidelines
- No automated test projects are present yet.
- If you add tests, prefer a `Halcyon.*.Tests` project and run with `dotnet test`.
- Name test files and types after the unit under test (e.g., `UserServiceTests`).

## Commit & Pull Request Guidelines
- Recent commits use short, imperative or descriptive subjects (e.g., `update migrations`).
- Avoid long commit bodies unless needed; include issue numbers when relevant (e.g., `(#116)`).
- PRs should describe the change, list manual test steps, and include UI screenshots for frontend work.

## Security & Configuration Tips
- API secrets live in `Halcyon.Api/appsettings.Development.json` (gitignored).
- Frontend secrets and runtime values go in `Halcyon.Web/.env.local`.
- Do not commit real credentials; use local-only config overrides.
