# Copilot instructions for FinanceTrackerApp

This file contains concise, project-specific guidance to help an AI coding agent be productive immediately.

## Big picture
- Multi-project .NET solution (API / Domain / Infrastructure / Services). `FinanceTracker.API` is the web surface; data access lives in `FinanceTracker.Infrastructure`; domain objects in `FinanceTracker.Domain`; application logic in `FinanceTracker.Services`.
- MongoDB is the primary datastore. Models use MongoDB.Mappings (Bson attributes) and prefer reference-by-id (ObjectId) over embedding.

## Key files and what they mean
- `Program.cs` — application startup, DI registrations (repositories/services), JWT config.
- `FinanceTracker.Domain/*` — domain entities annotated with `[BsonRepresentation]` and `[BsonIgnore]` (watch for Decimal128 and ObjectId mappings).
- `FinanceTracker.Infrastructure/Database/migrations/` — mongosh migration scripts, validators, and `examples/` JSON seed files.
- `FinanceTracker.Infrastructure/Repositories/GenericRepository.cs` — base repository; app code should prefer `UpdatePartialAsync` (`$set`) over `ReplaceOneAsync` to avoid accidental data loss.
- `FinanceTracker.API/Handlers/PasswordHashHandler.cs` — PBKDF2 hashing format: `{iterations}.{saltBase64}.{hashBase64}`; use this when migrating passwords.

## How to build / run (developer workflow)
1. Build: `dotnet build FinanceTracker.API/FinanceTracker.API.csproj`
2. Run: `dotnet run --project FinanceTracker.API/FinanceTracker.API.csproj`
3. Swagger UI served at root (Program prints the port).

DB commands (examples):
- Run migration script: `mongosh "<CONN>" --file FinanceTracker.Infrastructure/Database/migrations/migration_cleanup.js`
- Import example JSON: `mongoimport --uri="<CONN>" --collection=users --file=... --jsonArray --mode=upsert --upsertFields=_id`

## Project conventions and gotchas (must know)
- Domain properties annotated `[BsonRepresentation(BsonType.ObjectId)]` are strings in C# but stored as ObjectId in Mongo; when accepting IDs from clients, validate 24‑hex strings.
- Monetary fields use `[BsonRepresentation(BsonType.Decimal128)]` — incoming JSON should be converted to Decimal128 (migration scripts handle legacy strings).
- Controllers map DTO -> Domain using AutoMapper and then call GenericService which uses GenericRepository.AddAsync/UpdateAsync. Prefer partial updates via `UpdatePartialAsync` when changing a subset of fields.
- Passwords must be hashed via `PasswordHashHandler.HashPassword`; do not store plain text. If migrating plain-text users, perform hashing in a C# migration (not in mongosh).
- JWT config is under `JwtConfig` in `appsettings.*.json` and `JwtService` is registered in DI.

## Where to change behavior safely
- DB migrations & validators: `FinanceTracker.Infrastructure/Database/migrations/` — modify scripts and test on staging copy first.
- Add repository-level queries in `FinanceTracker.Infrastructure/Repositories/*Repository.cs` (they inherit `GenericRepository<T>` and can use the protected `Collection` property).
- Add business logic in `FinanceTracker.Services/*Service.cs` and expose via interfaces in `FinanceTracker.Services.Interfaces.Services`.

## Example tasks an AI agent can do immediately
- Add a date-range or type filter endpoint (see `TransactionsController.cs`) — use `DateOnly` for query binding, convert to DateTime range server-side as implemented.
- Add validators to `db_validators.js` (follow existing patterns) and create indexes in `create_indexes.js`.
- Write safe C# migration job to re-hash passwords using `PasswordHashHandler` and `UserRepository`.

## Tests & CI
- There are no integration tests currently. If adding tests, target small, fast unit/integration runs that exercise migration scripts against a local mongod instance or in-memory fixtures.

## Questions to ask before risky changes
- Are we working on dev/staging or production data? Always backup (mongodump) before migrations.
- Will the change require schema validators? If yes, prepare a staged validator run (`validationLevel: 'moderate'`).

---
If anything above is unclear or you want the file adjusted (language, more examples, or added one-click scripts), tell me what to add or change.
