# Smab.TTInfo.Server

A .NET Blazor Server project that provides web server access to table tennis league information from Table Tennis 365 (TT365) and TTLeagues. This project exposes API endpoints and Blazor UI for retrieving, summarizing, and exporting league, team, player, and fixture data.

## Features
- Blazor Server web application for table tennis league data
- REST API endpoints for fixtures, teams, players, and calendars
- Integration with TT365 and TTLeagues data sources
- Calendar export in iCal, CSV, JSON, and plain text formats
- Health checks and localization support
- Dependency injection for extensibility

## Installation
1. Clone the repository and restore dependencies:
   ```
dotnet restore
```
2. Build and run the server:
   ```
dotnet run --project src/Smab.TTInfo.Server/Smab.TTInfo.Server.csproj
```

## Usage
- Access the Blazor web UI at the configured server URL (default: `https://localhost:5001`)
- Use the REST API endpoints for programmatic access:
  - `/api/fixtures/{leagueId}`
  - `/api/team/{leagueId}/{teamName}`
  - `/calendar/{LeagueName}/{TeamName}?command=ics|csv|json|text`

## Main Components
- `Program.cs`: Application startup, DI, and endpoint registration
- `EndPoints/`: API and calendar endpoint definitions
- `Components/`: Blazor UI components and layouts
- Integration with TT365 and TTLeagues service libraries

## Project Structure
- `Components/`: Blazor UI components
- `EndPoints/`: API and calendar endpoint logic
- `Program.cs`: Main entry point and configuration

## License
See repository for license details.
