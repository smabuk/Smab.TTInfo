# Smab.TTInfo.TTLeagues

A .NET library for accessing and summarizing table tennis league data from the [TTLeagues](https://www.ttleagues.com) platform. This library provides services for retrieving league, team, player, and fixture information, with support for caching and flexible configuration.

## Features
- Retrieve league, team, player, and fixture data from TTLeagues
- Caching support to reduce API calls
- Strongly-typed models for TTLeagues entities
- Easily configurable via dependency injection and options

## Installation
Add a reference to the project or install via NuGet (if available):

```
dotnet add package Smab.TTInfo.TTLeagues
```

## Usage
1. **Register the service in your DI container:**

```csharp
services.AddTTLeaguesService();
```

2. **Inject and use `TTLeaguesReader` in your application:**

```csharp
public class MyService
{
    private readonly TTLeaguesReader _reader;
    public MyService(TTLeaguesReader reader) => _reader = reader;

    // Example usage
    public async Task DoSomethingAsync()
    {
        // Use _reader to access TTLeagues data
    }
}
```

## Configuration
You can configure TTLeagues options (such as cache folder, cache duration, and test file usage) via configuration or code:

```json
"TTLeaguesOptions": {
  "CacheFolder": "./cache",
  "CacheHours": 12,
  "UseTestFiles": false
}
```

Or in code:

```csharp
services.AddTTLeaguesService(options => {
    options.CacheFolder = "./cache";
    options.CacheHours = 12;
    options.UseTestFiles = false;
});
```

## Main Components
- `TTLeaguesReader`: Main service for accessing TTLeagues data
- `TTLeaguesOptions`: Configuration options for the service
- Models: Strongly-typed representations of leagues, teams, players, fixtures, etc.

## Project Structure
- `Services/`: Service interfaces and implementations
- `Models/`: Data models for TTLeagues entities
- `AddTTLeaguesService.cs`: Extension methods for DI registration

## License
See repository for license details.
