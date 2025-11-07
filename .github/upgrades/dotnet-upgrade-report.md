# .NET 10.0 Upgrade Report

## Project target framework modifications

| Project name                                                                 | Old Target Framework | New Target Framework | Commits                   |
|:-----------------------------------------------------------------------------|:--------------------:|:--------------------:|---------------------------|
| All projects (via Directory.Build.props)                                     | net9.0               | net10.0              | 071f963f                  |
| src\Smab.TTInfo.TTLeaguesAdapter\Smab.TTInfo.TTLeaguesAdapter.csproj         | net9.0               | net10.0              | ae169b0e                  |

## NuGet Packages

| Package Name                                         | Old Version | New Version              | Commit ID                 |
|:-----------------------------------------------------|:-----------:|:------------------------:|---------------------------|
| Microsoft.AspNetCore.Components.QuickGrid            | 9.0.10      | 10.0.0-rc.2.25502.107    | a6ba09a5                  |
| Microsoft.AspNetCore.Components.Web                  | 9.0.10      | 10.0.0-rc.2.25502.107    | f1e01491                  |
| Microsoft.Extensions.Http                            | 9.0.10      | 10.0.0-rc.2.25502.107    | b6a67496                  |
| Microsoft.Extensions.Options.ConfigurationExtensions | 9.0.10      | 10.0.0-rc.2.25502.107    | 6fccd67f                  |
| Microsoft.Extensions.Options.DataAnnotations         | 9.0.10      | 10.0.0-rc.2.25502.107    | 6fccd67f                  |

## All commits

| Commit ID        | Description                                                                          |
|:-----------------|:-------------------------------------------------------------------------------------|
| fa70df2f         | Commit upgrade plan                                                                  |
| 071f963f         | Update Directory.Build.props to target net10.0                                       |
| f1e01491         | Update Microsoft.AspNetCore.Components.Web in Directory.Packages.props               |
| 6fccd67f         | Update Microsoft.Extensions.Options packages in Directory.Packages.props             |
| b6a67496         | Update Microsoft.Extensions.Http version in Directory.Packages.props                 |
| a6ba09a5         | Update QuickGrid package version in Directory.Packages.props                         |
| ae169b0e         | Update target framework to net10.0 in TTLeaguesAdapter.csproj                        |
| ae092b2c         | Store final changes for step 'Upgrade src\Smab.TTInfo.Console\Smab.TTInfo.Console.csproj' |

## Project feature upgrades

### src\Smab.TTInfo.Console\Smab.TTInfo.Console.csproj

The following changes were made during the upgrade:

- **Spectre.Console.Cli API Breaking Change**: Updated the `Execute` method signature in `TTInfoCliCommand` class to include the new `CancellationToken` parameter required by the updated Spectre.Console.Cli package in .NET 10.0. The method signature changed from `Execute(CommandContext context, Settings settings)` to `Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)`.

## Next steps

- Test your Blazor application thoroughly to ensure all features work correctly with .NET 10.0
- Review the .NET 10.0 preview release notes for any additional breaking changes that may affect your application
- Consider updating to stable .NET 10.0 packages when they become available
- Run your test suite to validate all functionality
