# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade src\Smab.TTInfo.Shared\Smab.TTInfo.Shared.csproj
4. Upgrade src\Smab.TTInfo.TT365\Smab.TTInfo.TT365.csproj
5. Upgrade src\Smab.TTInfo.TTLeagues\Smab.TTInfo.TTLeagues.csproj
6. Upgrade src\Smab.TTInfo.TTClubs\Smab.TTInfo.TTClubs.csproj
7. Upgrade src\Smab.TTInfo.TTLeaguesAdapter\Smab.TTInfo.TTLeaguesAdapter.csproj
8. Upgrade src\Smab.TTInfo.Server\Smab.TTInfo.Server.csproj
9. Upgrade src\Smab.TTInfo.Console\Smab.TTInfo.Console.csproj

## Settings

This section contains settings and data used by execution steps.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                                           | Current Version | New Version              | Description                    |
|:-------------------------------------------------------|:---------------:|:------------------------:|:-------------------------------|
| Microsoft.AspNetCore.Components.QuickGrid              | 9.0.10          | 10.0.0-rc.2.25502.107    | Recommended for .NET 10.0      |
| Microsoft.AspNetCore.Components.Web                    | 9.0.10          | 10.0.0-rc.2.25502.107    | Recommended for .NET 10.0      |
| Microsoft.Extensions.Http                              | 9.0.10          | 10.0.0-rc.2.25502.107    | Recommended for .NET 10.0      |
| Microsoft.Extensions.Options.ConfigurationExtensions   | 9.0.10          | 10.0.0-rc.2.25502.107    | Recommended for .NET 10.0      |
| Microsoft.Extensions.Options.DataAnnotations           | 9.0.10          | 10.0.0-rc.2.25502.107    | Recommended for .NET 10.0      |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### src\Smab.TTInfo.Shared\Smab.TTInfo.Shared.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - Microsoft.AspNetCore.Components.Web should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)

#### src\Smab.TTInfo.TT365\Smab.TTInfo.TT365.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - Microsoft.AspNetCore.Components.Web should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Options.ConfigurationExtensions should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Options.DataAnnotations should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)

#### src\Smab.TTInfo.TTLeagues\Smab.TTInfo.TTLeagues.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - Microsoft.AspNetCore.Components.Web should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Http should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Options.ConfigurationExtensions should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Options.DataAnnotations should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)

#### src\Smab.TTInfo.TTClubs\Smab.TTInfo.TTClubs.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - Microsoft.AspNetCore.Components.QuickGrid should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)
  - Microsoft.AspNetCore.Components.Web should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Http should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Options.ConfigurationExtensions should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Options.DataAnnotations should be updated from `9.0.10` to `10.0.0-rc.2.25502.107` (*recommended for .NET 10.0*)

#### src\Smab.TTInfo.TTLeaguesAdapter\Smab.TTInfo.TTLeaguesAdapter.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

#### src\Smab.TTInfo.Server\Smab.TTInfo.Server.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

#### src\Smab.TTInfo.Console\Smab.TTInfo.Console.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`
