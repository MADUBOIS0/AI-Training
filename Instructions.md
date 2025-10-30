# Instructions for AI Code Generator

Audience: GitHub Copilot or Codex-like assistant inside Visual Studio 2022

Project name: API Healthboard and Request Collections

Goal: Build a local Postman-like website that can save API requests, run health checks on schedules, and show results. Keep everything local with .NET 8 and SQLite. Include presets for Devolutions Server.

Timebox: 8 to 12 hours. Favor incremental delivery. Keep scope tight.

## High-level requirements

- Save environments and variables
- Save request collections and individual requests
- Run requests from the UI with timing and response viewer
- Define health checks with simple assertions
- Background runner executes due health checks and stores results
- Dashboard shows latest status and simple trends
- Import and export collections and environments as JSON
- Preset wizard for Devolutions Server endpoints

## Tech stack

- .NET 8
- ASP.NET Core Razor Pages
- EF Core 8 with Microsoft.Data.Sqlite
- HttpClientFactory
- xUnit and FluentAssertions
- Optional Cronos for cron strings. Default to IntervalSeconds first
- Optional Chart.js copied into wwwroot. No CDN
- No paid services. Everything runs locally

## Architecture

Projects
- ApiBoard.Web  UI and hosting
- ApiBoard.Core  domain entities, interfaces, lightweight services
- ApiBoard.Infrastructure  EF Core, repositories, migrations
- ApiBoard.Tests  unit, integration, and infrastructure tests

Key concepts
- Environment holds a set of key value variables and optional secrets
- RequestCollection groups ApiRequests
- ApiRequest has Method, Url, Headers, Body, Environment reference
- HealthCheck references an ApiRequest and has Assertions plus schedule
- ResponseLog stores each manual send response
- HealthResult stores each automated health check run

Non goals
- No OAuth flows beyond static token for v1
- No swagger import in v1
- No distributed or multi user support

## Data model

Entities and fields
- Environment: Id, Name, Notes, CreatedAt, UpdatedAt
- EnvironmentVar: Id, EnvironmentId, Key, Value, IsSecret
- RequestCollection: Id, Name, Notes, CreatedAt, UpdatedAt
- ApiRequest: Id, CollectionId, EnvironmentId nullable, Name, Method, Url, HeadersJson, Body, CreatedAt, UpdatedAt
- HealthCheck: Id, ApiRequestId, Name, IntervalSeconds, IsEnabled, AssertionsJson, CreatedAt, UpdatedAt
- ResponseLog: Id, ApiRequestId, StatusCode, DurationMs, HeadersJson, Body, CreatedAt
- HealthResult: Id, HealthCheckId, Status enum Pass or Fail, StatusCode, DurationMs, FailureReason, CreatedAt

Assertions JSON
- StatusEquals: integer
- JsonPathExists: array of json paths
- MaxLatencyMs: integer

Example
```json
{
  "StatusEquals": 200,
  "JsonPathExists": ["$.status"],
  "MaxLatencyMs": 500
}
```

## Phase plan

### Phase 1  scaffolding

Tasks
1. Create solution with projects Web, Core, Infrastructure, Tests
2. Wire references  Web depends on Core and Infrastructure. Infrastructure depends on Core
3. Add entities and DbContext with configurations and SQLite
4. Apply initial migration
5. Seed one Environment and one Request inside Program.cs for demo

Prompts for AI
- Create a .NET 8 solution with projects ApiBoard.Web, ApiBoard.Core, ApiBoard.Infrastructure, and ApiBoard.Tests. Wire references as described. Add standard editorconfig with 2 space indent for Razor and 4 space for C#
- In ApiBoard.Core add entities Environment, EnvironmentVar, RequestCollection, ApiRequest, HealthCheck, ResponseLog, HealthResult. All with CreatedAt and UpdatedAt except log tables which only need CreatedAt
- In ApiBoard.Infrastructure create AppDbContext with DbSet for each entity. Configure relationships, required fields, and JSON columns for headers and assertions using string storage
- Enable SQLite WAL and foreign keys pragma on open. Add initial migration and update database
- In Web set up DI for DbContext using SQLite file api_board.db in the content root

### Phase 2  core features

Tasks
1. Razor Pages CRUD for Environments, Collections, and ApiRequests
2. RequestRunner service that performs variable substitution then uses HttpClientFactory to send the request and time it
3. Response viewer with tabs Status, Headers, Pretty JSON, Raw
4. Save ResponseLog after each manual send
5. Import and export JSON for all environments, collections, and requests

Prompts for AI
- Generate Razor Pages for CRUD over Environment and EnvironmentVar child collection. Use tag helpers and validation. Keep layout minimal
- Generate Razor Pages for CRUD over RequestCollection and ApiRequest. ApiRequest editor supports Method select, URL textbox, headers repeater control, body textarea, and environment selector
- Implement IRequestRunner with SendAsync(ApiRequest request, Environment envNullable). Substitute variables in Url, Headers, Body using double curly tokens like {{Token}}. Return a result object with StatusCode, Headers, Body, DurationMs
- Create a Response viewer partial with tabs and copy to clipboard buttons. Pretty print JSON if content type contains application/json
- Add endpoints to export and import a single JSON document that contains Environments and Collections with nested requests. Include a SchemaVersion field

### Phase 3  health checks and tests

Tasks
1. HealthCheck model editor and list page
2. HealthCheckRunner BackgroundService that wakes every 30 seconds, selects due checks by comparing last HealthResult timestamp, runs them with a small concurrency limit, evaluates assertions, and writes HealthResult
3. Dashboard page that shows each HealthCheck with last status, last duration, and a sparkline for last 24 results
4. Tests  variable resolver, assertions evaluator, integration test with a stub API

Prompts for AI
- Add an AssertionsEvaluator with rules StatusEquals, JsonPathExists, MaxLatencyMs. Accept the raw HTTP result and the Assertions JSON. Return Pass or Fail with FailureReason
- Implement a HealthCheckRunner that limits to 3 concurrent checks using SemaphoreSlim. Store next due time as createdAt plus IntervalSeconds or use last result as reference
- Build a Dashboard Razor Page that queries latest HealthResult per HealthCheck and shows a small sparkline. Use a tiny inline canvas and Chart.js served from wwwroot
- Write xUnit tests for VariableResolver. Given env with Token=abc, turn Authorization: Bearer {{Token}} into Authorization: Bearer abc
- Write integration tests using WebApplicationFactory and a stub API endpoint GET /health that returns { "status": "ok" } with Content-Type application/json. Assert Pass with status 200 and latency constraint

### Phase 4  presets and polish

Tasks
1. Add a PresetsService that seeds a Devolutions Server collection with two requests  GET {BaseUrl}/api/health and GET {BaseUrl}/api/version
2. Add a wizard page to fill BaseUrl and Token then create an Environment and the preset collection
3. Add basic dark theme and a clean top nav
4. Finish README with Quickstart for Devolutions Server and screenshots

Prompts for AI
- Create PresetsService with a method SeedDevolutionsServer(baseUrl, tokenVarName). Generate ApiRequests that use {{BaseUrl}} and Authorization: Bearer {{Token}}
- Create a Razor Page Presets/DevolutionsServerWizard with a simple form BaseUrl and Token. On submit create Environment with variables BaseUrl and Token and create the DS collection
- Apply Bootstrap locally. Add site.css with a dark background and high contrast table classes
- Generate a Quickstart section for README with steps to create a DS preset and run first health check

## Minimal UI plan

- Navbar links  Dashboard, Health Checks, Requests, Collections, Environments, Presets
- Tables for index pages with Create and Edit buttons
- Forms use validation summary and input tag helpers
- Response viewer as a partial with tabs and monospaced block for JSON

## Import and export JSON schema

Top level
```json
{
  "SchemaVersion": 1,
  "Environments": [ ... ],
  "Collections": [ ... ]
}
```

Environment
```json
{
  "Name": "Local",
  "Vars": [
    { "Key": "BaseUrl", "Value": "https://localhost:5001", "IsSecret": false },
    { "Key": "Token", "Value": "REPLACE_ME", "IsSecret": true }
  ]
}
```

Collection
```json
{
  "Name": "Devolutions Server",
  "Requests": [
    {
      "Name": "Health",
      "Method": "GET",
      "Url": "{{BaseUrl}}/api/health",
      "Headers": { "Authorization": "Bearer {{Token}}" },
      "Body": null
    },
    {
      "Name": "Version",
      "Method": "GET",
      "Url": "{{BaseUrl}}/api/version",
      "Headers": { "Authorization": "Bearer {{Token}}" },
      "Body": null
    }
  ]
}
```

## Coding guidelines

- Follow Clean Architecture dependencies. Web -> Core and Infrastructure. Infrastructure -> Core
- Keep business rules in Core and IO in Infrastructure
- Prefer async methods
- Use CancellationToken in all async public methods
- Use HttpClientFactory. One named client default is fine
- Guard against null and invalid inputs with explicit validation
- Log minimal info to console. Do not log secrets
- Keep UI simple. No SPA

## Testing strategy

- Unit tests for VariableResolver and AssertionsEvaluator
- Repository tests with a temp SQLite file and migrations applied
- Integration tests with WebApplicationFactory and a stub API
- For timing tests, assert thresholds with buffers to avoid flakiness

## Definition of done

- All CRUD pages work
- Can send a request and see status, headers, JSON, and raw body
- ResponseLog saved on each send
- Can define a HealthCheck with StatusEquals and MaxLatencyMs
- Background runner executes checks at the chosen interval
- Dashboard lists checks with last result and basic sparkline trend
- Import and export round trips the main entities
- DS preset wizard creates env and requests
- Tests pass on a clean clone
- README explains setup, run, and DS quickstart

## Commands to expect in README

- `dotnet new webapp -n ApiBoard.Web`
- `dotnet new classlib -n ApiBoard.Core`
- `dotnet new classlib -n ApiBoard.Infrastructure`
- `dotnet new xunit -n ApiBoard.Tests`
- `dotnet sln add **/*.csproj`
- `dotnet add ApiBoard.Web reference ApiBoard.Core ApiBoard.Infrastructure`
- `dotnet add ApiBoard.Infrastructure reference ApiBoard.Core`
- `dotnet add ApiBoard.Infrastructure package Microsoft.EntityFrameworkCore.Sqlite`
- `dotnet add ApiBoard.Infrastructure package Microsoft.EntityFrameworkCore.Design`
- `dotnet add ApiBoard.Web package Microsoft.EntityFrameworkCore.Sqlite`
- `dotnet add ApiBoard.Tests package FluentAssertions`
- `dotnet ef migrations add InitialCreate --project ApiBoard.Infrastructure --startup-project ApiBoard.Web`
- `dotnet ef database update --project ApiBoard.Infrastructure --startup-project ApiBoard.Web`

## Commit style

- Conventional commits
- feat: for features
- fix: for bug fixes
- test: for tests
- chore: for scaffolding and config
- refactor: for non behavior changes

## Ready to start

Open a new empty repository. Add this file as Instructions.md. Then follow Phase 1 prompts to generate the scaffolding. Keep commits small and frequent.