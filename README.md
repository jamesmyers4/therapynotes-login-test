# TherapyNotes Login Test

A Selenium WebDriver test written in C# and xUnit validating the login flow 
for TherapyNotes.com, implemented using the Page Object Model pattern.

## Tech Stack
- C# / .NET 10
- xUnit ([Fact] and [Theory] with [InlineData])
- Selenium WebDriver 4.x
- ChromeDriver (managed automatically via Selenium Manager)
- Page Object Model (POM) design pattern

## Project Structure
Pages/
  LoginPage.cs
  DashboardPage.cs
Tests/
  LoginTests.cs
  AccessibilityTests.cs
Config/
  TestConfig.cs

## What It Tests
- Navigation to TherapyNotes.com
- Two-step login flow (Practice Code → Username/Password)
- Successful authentication and dashboard load verification
- Invalid practice code error handling
- Invalid credentials error handling
- Empty field validation via aria-disabled state
- Accessibility — aria labels and disabled state on interactive elements

## Test Suite Summary
| Test Class | Tests | Coverage |
|---|---|---|
| LoginTests | 6 | Happy path, negative, data-driven |
| AccessibilityTests | 3 | aria labels, disabled state |
| **Total** | **9** | |

## How To Run
1. Clone the repo
2. Open `TherapyNotesUITests.sln` in Visual Studio
3. Restore NuGet packages
4. Run via Test Explorer or `dotnet test`

## Full Suite Runtime
~28 seconds for all 9 tests (local environment)

## Notes
See [SESSION_LOG.md](SESSION_LOG.md) for full implementation details and 
decisions made during development.