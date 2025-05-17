# Smab.TTInfo.Console

A .NET console application for accessing and displaying table tennis league information from Table Tennis 365 (TT365) and TTLeagues. This tool provides command-line access to league, team, player, and fixture data, making it easy to retrieve and summarize information directly from the terminal.

## Features
- Command-line interface for TT365 and TTLeagues data
- Retrieve and display league, team, player, and fixture information
- Output summaries and details in a terminal-friendly format
- Supports both TT365 and TTLeagues data sources
- Built with [Spectre.Console](https://spectreconsole.net/) for rich terminal output

## Installation
1. Restore dependencies:
   ```
   dotnet restore
   ```
2. Build the console app:
   ```
   dotnet build src/Smab.TTInfo.Console/Smab.TTInfo.Console.csproj
   ```

## Usage
Run the console application with:
```
dotnet run --project src/Smab.TTInfo.Console/Smab.TTInfo.Console.csproj [arguments]
```

Arguments and commands will depend on your implementation (see source or help output for details). Typical usage might include:
- Listing leagues or teams
- Displaying fixtures for a team or league
- Showing player statistics

## Main Components
- `Program.cs`: Application entry point and command-line logic
- Integration with TT365 and TTLeagues service libraries
- Uses Smab.TTInfo.Shared for common models and helpers

## Project Structure
- `Program.cs`: Main entry point
- Additional files: Command and output logic (see source)

## License
See repository for license details.
