# TherapyNotes Login Test

![CI](https://github.com/jamesmyers4/therapynotes-login-test/actions/workflows/ci.yml/badge.svg)

A Selenium WebDriver test written in C# and xUnit validating the login flow 
for TherapyNotes.com, implemented using the Page Object Model pattern.

## Tech Stack
- **Language:** C# / .NET 10
- **Test Framework:** xUnit ([Fact], [Theory], [InlineData])
- **BDD Framework:** Reqnroll (SpecFlow successor) with Gherkin feature files
- **Browser Automation:** Selenium WebDriver 4.x
- **Driver Management:** Selenium Manager (built into Selenium 4.6+, no manual ChromeDriver install)
- **Design Pattern:** Page Object Model (POM)
- **API Testing:** HttpClient + System.Text.Json
- **CI/CD:** GitHub Actions
- **IDE:** Visual Studio 2022

## Project Structure
```
TherapyNotesUITests/
├── .github/
│   └── workflows/
│       └── ci.yml                # GitHub Actions CI workflow
├── Config/
│   └── TestConfig.cs             # Centralized config — URLs, credentials, headless toggle
├── Features/
│   └── Login.feature             # Reqnroll Gherkin scenarios
├── Pages/
│   ├── LoginPage.cs              # Homepage nav, login link, practice code, credentials
│   └── DashboardPage.cs          # Post-login verification — URL check, welcome header
├── StepDefinitions/
│   └── LoginSteps.cs             # Reqnroll step bindings wired to POM
├── Tests/
│   ├── LoginTests.cs             # Happy path, negative, data-driven (10 tests)
│   ├── AccessibilityTests.cs     # aria labels, disabled state, input constraints (5 tests)
│   ├── SecurityTests.cs          # HTTPS, password masking, auth redirect (3 tests)
│   ├── ResponsiveTests.cs        # iPhone 14, iPad, Desktop viewports (3 tests)
│   └── ApiTests.cs               # HTTP layer demo via JSONPlaceholder (4 tests)
├── README.md
├── SESSION_LOG.md                # Full implementation history and decisions
└── CONTEXT.md                    # This file
```

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
| Class | Tests | Focus |
|---|---|---|
| LoginTests | 10 | Happy path, negative, data-driven, forgot password flow |
| AccessibilityTests | 5 | aria labels, disabled state, input constraints |
| SecurityTests | 3 | HTTPS enforcement, password masking, auth redirect |
| ResponsiveTests | 3 | Mobile/tablet/desktop viewport validation |
| ApiTests | 4 | HTTP layer patterns via JSONPlaceholder |
| Reqnroll (BDD) | 3 | Gherkin scenarios — happy path, invalid code, invalid credentials |
| **Total** | **28** | |

**Full suite runtime:** ~27 seconds non-destructive (26 tests); ~75 seconds full suite (28 tests)
**API tests:** 67-144ms each (no browser overhead)
**BDD scenarios:** 3-5 seconds each (browser-driven)
**UI tests:** 1.5-5 seconds each (network-bound, not render-bound)

## Test Categories
Tests are tagged by risk profile to prevent accidental account lockout:
- [Trait("Category", "Destructive")] in xUnit — tests that submit invalid credentials
- @Destructive in Reqnroll — BDD scenarios that submit invalid credentials
- Standard run (excludes destructive): `dotnet test --filter "Category!=Destructive"`
- Deliberate destructive run: `dotnet test --filter "Category=Destructive"`

## How To Run
1. Clone the repo
2. Open `TherapyNotesUITests.slnx` in Visual Studio
3. Restore NuGet packages
4. Set environment variables (see Setup section)
5. Standard run (recommended — excludes destructive tests):
   `dotnet test --filter "Category!=Destructive" --verbosity normal`
6. Destructive tests only (may trigger account lockout — run deliberately):
   `dotnet test --filter "Category=Destructive" --verbosity normal`
7. Full suite:
   `dotnet test --verbosity normal`

## Setup

Before running tests, set the following environment variables with the 
credentials provided by TherapyNotes:

```powershell
[System.Environment]::SetEnvironmentVariable("TN_PRACTICE_CODE", "your_practice_code", "User")
[System.Environment]::SetEnvironmentVariable("TN_USERNAME", "your_username", "User")
[System.Environment]::SetEnvironmentVariable("TN_PASSWORD", "your_password", "User")
```

Restart Visual Studio or your terminal after running these commands.
For CI/CD, set these as repository secrets named `TN_PRACTICE_CODE`, 
`TN_USERNAME`, and `TN_PASSWORD`.

## Headless Mode
Set `Headless = true` in `Config/TestConfig.cs` to run without a visible browser.
Useful for CI/CD pipelines or reducing visual noise during local runs.

## Full Suite Runtime
- Non-destructive suite (26 tests): ~27 seconds (headless, local environment)
- Full suite (28 tests): ~75 seconds including destructive tests
- UI tests: 1.5-5 seconds each (browser-driven)
- BDD scenarios: 3-5 seconds each (browser-driven)
- API tests: 67-144ms each (pure HTTP, no browser)

## Notes
See [SESSION_LOG.md](SESSION_LOG.md) for full implementation details and 
decisions made during development.