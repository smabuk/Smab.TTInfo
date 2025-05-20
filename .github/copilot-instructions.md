# Instructions for GitHub and VisualStudio Copilot
https://github.blog/changelog/2025-01-21-custom-repository-instructions-are-now-available-for-copilot-on-github-com-public-preview/


## General

* Make only high confidence suggestions when reviewing code changes.
* Always use the latest version C#, currently C# 13 features.
* Files must have CRLF line endings.

## Formatting

* Apply code-formatting style defined in `.editorconfig`.
* Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).
* Ensure that the final return statement of a method is on its own line.
* Use tabs for indentation, with a tab size of 4 spaces.

## Coding Style
* Always use explict types instead of `var`.
* Use pattern matching and switch expressions wherever possible.
* Use `IEnumerable<T>` instead of `ICollection<T>` or `List<T>` for method parameters and return types.
* Use functional programming techniques instead of imperative programming.
* Prefer file-scoped namespace declarations and single-line using directives.
* Use `nameof` instead of string literals when referring to member names.
* Use `StringBuilder` for string concatenation in loops or when building large strings.
* Use `String.IsNullOrEmpty` or `String.IsNullOrWhiteSpace` instead of checking for `null` and empty strings separately.

### Nullable Reference Types

* Declare variables non-nullable, and check for `null` at entry points.
* Always use `is null` or `is not null` instead of `== null` or `!= null`.
* Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.


### Testing

* We use xUnit SDK v3 with Microsoft.Testing.Platform (https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro)
* Do not emit "Act", "Arrange" or "Assert" comments.
* We do not use any mocking framework at the moment. Use NSubstitute, if necessary. Never use Moq.
* Use "snake_case" for test method names but keep the original method under test intact.
  For example: when adding a test for methond "MethondToTest" instead of "MethondToTest_ShouldReturnSummarisedIssues" use "MethondToTest_should_return_summarised_issues".

## Tone
* Use a professional and technical tone.
* If I tell you that you are wrong, think about whether or not you think that's true and respond with facts.
* Avoid apologizing or making conciliatory statements.
* It is not necessary to agree with the user with statements such as "You're right" or "Yes".
* Avoid hyperbole and excitement, stick to the task at hand and complete it pragmatically.
* 