# Smab.TTInfo.TT365

A .NET library for accessing and summarizing table tennis league data from the [Table Tennis 365 (TT365)](https://www.tabletennis365.com) platform. This library provides services for retrieving league, team, player, and fixture information, with support for caching, CSV/iCal export, and flexible configuration.

## Features
- Retrieve league, team, player, and fixture data from TT365
- Caching support to reduce API calls
- Generate CSV and iCal (calendar) exports from fixture data
- Strongly-typed models for TT365 entities
- Easily configurable via dependency injection and options

## Installation
Add a reference to the project or install via NuGet (if available):

```
dotnet add package Smab.TTInfo.TT365
```

## Usage
1. **Register the service in your DI container:**

```csharp
services.AddTT365Service();
```

2. **Inject and use `ITT365Reader` in your application:**

```csharp
public class MyService
{
    private readonly ITT365Reader _reader;
    public MyService(ITT365Reader reader) => _reader = reader;

    public async Task<List<Fixture>> GetFixtures(string leagueId)
        => await _reader.GetAllFixtures(leagueId);
}
```

## Configuration
You can configure TT365 options (such as cache folder, cache duration, and test file usage) via configuration or code:

```json
"TT365Options": {
  "CacheFolder": "./cache",
  "CacheHours": 12,
  "UseTestFiles": false
}
```

Or in code:

```csharp
services.AddTT365Service(options => {
    options.CacheFolder = "./cache";
    options.CacheHours = 12;
    options.UseTestFiles = false;
});
```

## Main Components
- `ITT365Reader` / `TT365Reader`: Main service for accessing TT365 data
- `TT365Options`: Configuration options for the service
- Models: Strongly-typed representations of leagues, teams, players, fixtures, etc.
- Utility methods for CSV/iCal export

## Project Structure
- `Services/`: Service interfaces and implementations
- `Models/`: Data models for TT365 entities
- `AddTT365Service.cs`: Extension methods for DI registration

## Relational Models Class Diagram

Below is a class diagram showing all types in the `Smab.TTInfo.TT365.Models.TT365` namespace and their relationships:

```mermaid
---
config:
  look: classic
  layout: elk
---
classDiagram
    class League["League (derived from the site)"] {
        string Id
        string Name
        string Description
        string URL
        string Theme
        Season CurrentSeason
        List~Season~ Seasons
    }
    class Season {
        string Id
        string Name
        LookupTables Lookups
        List~Division~ Divisions
        int DivisionCount
    }
    class Division {
        string Id
        string Name
        List~Team~ Teams
        int TeamCount
    }
    class LookupTables {
        List~IdNamePair~ DivisionLookup
        List~IdNamePair~ ClubLookup
        List~IdNamePair~ TeamLookup
        List~IdNamePair~ VenueLookup
    }
    class IdNamePair {
        string Id
        string Name
    }
    class Team {
        string Id
        string URL
        string Caption
        string Name
        string ShortName
        int ClubId
        string DivisionName
        string Captain
        ICollection~Player~ Players
        ICollection~Fixture~ Fixtures
        ICollection~TeamResult~ Results
        int? LeaguePosition
        int Played
        int Won
        int Drawn
        int Lost
        int SetsFor
        int SetsAgainst
        int Points
        int PointsAdjustment
    }
    class Player {
        string Name
        string PlayerURL
        int Played
        float WinPercentage
        string PoMAwards
        string Form
        int ClubRanking
        int LeagueRanking
        int CountyRanking
        int RegionalRanking
        int NationalRanking
        int PlayerId
        int Id
        string SeasonId
        List~PlayerResult~ PlayerResults
    }
    class PlayerResult {
        int Id
        string Name
        int OriginalSortOrder
        DateOnly Date
        string PlayerTeamName
        Player Opponent
        string OpponentTeam
        string Division
        string Scores
        int? RankingDiff
        string Result
        string ResultReason
        string MatchCardURL
        string FormattedRankingDiff
        List~Score~ Games
        Score GameScore
    }
    class Fixture {
        string Division
        string Description
        DateOnly Date
        string HomeTeam
        string AwayTeam
        string Venue
    }
    class CompletedFixture {
        int ForHome
        int ForAway
        string PlayerOfTheMatch
        string CardURL
        List~MatchPlayer~ HomePlayers
        List~MatchPlayer~ AwayPlayers
        string? Other
        int Id
        string DoublesWinner
        string Score
    }
    class PostponedFixture {
        string Reason
    }
    class RearrangedFixture {
        DateOnly OriginalDate
        string Reason
    }
    class VoidFixture {
        string Reason
    }
    class TeamResult {
        string Opposition
        string HomeOrAway
        int Points
        int ScoreForTeam
        int ScoreForOpposition
        string MatchResult
    }
    class MatchPlayer {
        string Name
        int Id
        int SetsWon
        bool PoM
    }
    class Score {
        int Score1
        int Score2
        +ToString() string
    }
    class FixturesView {
        string Caption
        string URL
        FixturesViewOptions Options
        ICollection~Fixture~ Fixtures
    }
    class FixturesViewOptions {
        string Season
        string DivisionName
        string ClubId
        string TeamId
        string VenueId
        int ViewModeType
        bool HideCompletedFixtures
        bool MergeDivisions
        bool ShowByWeekNo
    }

    %% Inheritance
    CompletedFixture  --|> Fixture
    PostponedFixture  --|> Fixture
    RearrangedFixture --|> Fixture
    VoidFixture       --|> Fixture
    TeamResult  --|> CompletedFixture

    %% Relationships
    League o-- Season : CurrentSeason
    League o-- "*" Season : Seasons
    Season o-- LookupTables : Lookups
    Season o-- "*" Division : Divisions
    Division o-- "*" Team : Teams
    Team o-- "*" Player : Players
    Team o-- "*" Fixture : Fixtures
    Team o-- "*" TeamResult : Results
    Player o-- "*" PlayerResult : PlayerResults
    PlayerResult o-- Player : Opponent
    PlayerResult o-- "*" Score : Games
    CompletedFixture o-- "*" MatchPlayer : HomePlayers
    CompletedFixture o-- "*" MatchPlayer : AwayPlayers
    FixturesView o-- FixturesViewOptions : Options
    FixturesView o-- "*" Fixture : Fixtures
    LookupTables o-- "*" IdNamePair : DivisionLookup
    LookupTables o-- "*" IdNamePair : ClubLookup
    LookupTables o-- "*" IdNamePair : TeamLookup
    LookupTables o-- "*" IdNamePair : VenueLookup
```

 
## License
See repository for license details.
