---
applyTo: '**/*.cs'
---
Coding standards, domain knowledge, and preferences that AI should follow.

## Namespaces
- use file-scoped namespaces that match the folder structure.

## Logic
- Prefer collection expressions.
- Prefer swictch expressions over if statements.

## Immutability
- Prefer immutable types unless mutability is requested.
- Prefer records over classes for immutable types.

## Files Organization
- Define one type per file.

## Record Design
- Define record's properties on the same line with the record declaration.
- Accompany each record `<anme>` with a `<name>facory` static factory class.
- Place the factory class in the same file as the record.
- Expose static `Create` method in the factory class for instantiation.
- Place argument validation in the `Create` method.
- Never use record's constructor when there is a factory method.
- Use immutable collections in records unless requested otherwise.
- Use `ImmutableList<T>` in records whenever possible.
- Define record behavior in extension methods in other static classes.

## Discriminated Unions Design    
- Prefer using records for discriminated unions.
- Derive specific types from a base abstract record.
- Define the one one all entire discriminated union in one file.
- Define static factories class per discriminated union.
- Expose one static factory method per variant.
- Follow rules for record design when designing a discriminated union.
