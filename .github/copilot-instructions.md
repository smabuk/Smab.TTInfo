# Instructions for GitHub and VisualStudio Copilot

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
* Always use explict types instead of `var`. NEVER USE `var`.
* Apply coding style defined in `.editorconfig`.
* Use collection expressions
* Use pattern matching and switch expressions wherever possible.
* Use functional programming techniques instead of imperative programming.
* Prefer file-scoped namespace declarations and single-line using directives.
* Use records where appropriate for data transfer objects (DTOs) or immutable types or wherever is useful.
* Use primary constructors.

### Nullable Reference Types

* Declare variables non-nullable, and check for `null` at entry points.
* Always use `is null` or `is not null` instead of `== null` or `!= null`.
* Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.
