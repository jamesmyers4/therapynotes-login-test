# TherapyNotes Login Test

![CI](https://github.com/jamesmyers4/therapynotes-login-test/actions/workflows/ci.yml/badge.svg)

A Selenium WebDriver test written in C# and xUnit validating the login flow 
for TherapyNotes.com, implemented using the Page Object Model pattern.

## Tech Stack
- C# / .NET 10
- xUnit ([Fact] and [Theory] with [InlineData])
- Selenium WebDriver 4.x
- ChromeDriver (managed automatically via Selenium Manager)
- Page Object Model (POM) design pattern
- HttpClient for API layer testing
- Headless Chrome support via TestConfig toggle

## Project Structure
Pages/
  LoginPage.cs
  DashboardPage.cs
Tests/
  LoginTests.cs
  AccessibilityTests.cs
  SecurityTests.cs
  ResponsiveTests.cs
  ApiTests.cs
Config/
  TestConfig.cs

## What It Tests
- Navigation to TherapyNotes.com
- Two-step login flow (Practice Code → Username/Password)
- Successful authentication and dashboard load verification
- Invalid practice code error handling
- Invalid credentials error handling
- Empty field validation via aria-disabled state
- Forgot practice code link visibility and navigation
- Accessibility — aria labels and disabled state on interactive elements
- Password field masking and input constraints
- Security — HTTPS enforcement and unauthenticated access redirect
- Responsive layout across mobile, tablet, and desktop viewports
- API layer — HTTP status codes, response structure, and error handling

## Test Suite Summary
| Test Class | Tests | Coverage |
|---|---|---|
| LoginTests | 10 | Happy path, negative, data-driven, forgot password |
| AccessibilityTests | 5 | aria labels, disabled state, input constraints |
| SecurityTests | 3 | HTTPS, password masking, auth redirect |
| ResponsiveTests | 3 | iPhone 14, iPad, Desktop viewports |
| ApiTests | 4 | GET, POST, 404 handling |
| **Total** | **25** | |

## How To Run
1. Clone the repo
2. Open `TherapyNotesUITests.sln` in Visual Studio
3. Restore NuGet packages
4. Run via Test Explorer or `dotnet test`

## Headless Mode
Set `Headless = true` in `Config/TestConfig.cs` to run without a visible browser.
Useful for CI/CD pipelines or reducing visual noise during local runs.

## Full Suite Runtime
~55 seconds for all 25 tests (local environment, headless)
- UI tests: 1.5-5 seconds each (browser-driven)
- API tests: 67-144ms each (pure HTTP, no browser)

## Notes
See [SESSION_LOG.md](SESSION_LOG.md) for full implementation details and 
decisions made during development.