# Smab.TTInfo.Shared

A .NET shared library providing common models, configuration options, constants, and helper utilities for the Smab.TTInfo suite of projects. This library is intended to be referenced by other projects (such as TT365, TTLeagues, and server/client apps) to ensure consistency and reduce code duplication.

## Features
- **Base configuration options** (`TTInfoOptions`) for caching, test data, and more
- **Global constants** for configuration and shared values
- **Date/time and HTML helper utilities** for formatting and conversions
- **Custom JSON converters** for `DateOnly`, `TimeOnly`, and Unix date handling
- **Global usings** for streamlined code in referencing projects

## Installation
Add a reference to this project from your other .NET projects:

```
dotnet add reference ../Smab.TTInfo.Shared/Smab.TTInfo.Shared.csproj
```

## Usage
- Reference this library in your project file or via Visual Studio project references.
- Use the provided models, helpers, and constants in your code:

```csharp
using Smab.TTInfo.Shared.Models;
using Smab.Shared.Helpers;

// Example: Using TTInfoOptions
public class MyOptions : TTInfoOptions { }

// Example: Using a helper
string formatted = DateTime.Now.ToDateLongMonthYearString();
```

## Main Components
- `Models/TTInfoOptions.cs`: Base options for configuration
- `GlobalConstants.cs`: Shared constant values
- `Helpers/`: Utility classes for date, time, HTML, and JSON
- `GlobalUsings.cs`: Common usings for referencing projects

## Project Structure
- `Models/`: Shared data models and configuration
- `Helpers/`: Utility and extension methods
- `GlobalConstants.cs`: Application-wide constants
- `GlobalUsings.cs`: Global using directives

## License
See repository for license details.
