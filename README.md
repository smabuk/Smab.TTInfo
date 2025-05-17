# Smab.TTInfo Solution

A .NET solution for accessing, summarizing, and presenting table tennis league data from Table Tennis 365 (TT365) and TTLeagues. This solution includes a Blazor Server web application, REST API, shared libraries, and a console application for flexible data access and integration.

## Projects

- **[Smab.TTInfo.Server](src/Smab.TTInfo.Server/README.md)**  
  Blazor Server web application and REST API for league, team, player, and fixture data.
  - Web UI for browsing and exporting data
  - API endpoints for programmatic access
  - Calendar export in iCal, CSV, JSON, and text formats

- **[Smab.TTInfo.TT365](src/Smab.TTInfo.TT365/README.md)**  
  .NET library for accessing and summarizing data from Table Tennis 365 (TT365).
  - Strongly-typed models and service for TT365 data
  - Caching and export utilities

- **[Smab.TTInfo.TTLeagues](src/Smab.TTInfo.TTLeagues/README.md)**  
  .NET library for accessing and summarizing data from TTLeagues.
  - Strongly-typed models and service for TTLeagues data
  - Caching and configuration support

- **[Smab.TTInfo.Shared](src/Smab.TTInfo.Shared/README.md)**  
  Shared library with common models, configuration options, constants, and helper utilities used by other projects.

- **[Smab.TTInfo.Console](src/Smab.TTInfo.Console/README.md)**  
  Console application for accessing and displaying table tennis league information from TT365 and TTLeagues.

## Features
- Blazor Server web UI for interactive browsing and export
- REST API endpoints for fixtures, teams, players, and calendars
- Integration with TT365 and TTLeagues data sources
- Calendar export in iCal, CSV, JSON, and plain text
- Health checks and localization
- Shared configuration and helper utilities
- Console application for command-line access

## Getting Started

1. **Restore dependencies**dotnet restore2. **Build the solution**dotnet build3. **Run the Blazor Server app**dotnet run --project src/Smab.TTInfo.Server/Smab.TTInfo.Server.csproj   Access the web UI at `https://localhost:5001` (default).

4. **Use the Console app**dotnet run --project src/Smab.TTInfo.Console/Smab.TTInfo.Console.csproj
## Project Structure

- `src/Smab.TTInfo.Server/` — Blazor Server app and API
- `src/Smab.TTInfo.TT365/` — TT365 data access library
- `src/Smab.TTInfo.TTLeagues/` — TTLeagues data access library
- `src/Smab.TTInfo.Shared/` — Shared models and helpers
- `src/Smab.TTInfo.Console/` — Console application

## License
See individual project directories or the repository root for license details.
