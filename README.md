# TherapyNotes Login Test

A Selenium WebDriver test written in C# and xUnit validating the login flow 
for TherapyNotes.com.

## Tech Stack
- C# / .NET 10
- xUnit
- Selenium WebDriver 4.x
- ChromeDriver (managed automatically via Selenium Manager)

## What It Tests
- Navigation to TherapyNotes.com
- Two-step login flow (Practice Code → Username/Password)
- Successful authentication and dashboard load verification

## How To Run
1. Clone the repo
2. Open `TherapyNotesUITests.sln` in Visual Studio
3. Restore NuGet packages
4. Run via Test Explorer

## Notes
See [SESSION_LOG.md](SESSION_LOG.md) for full implementation details and 
decisions made during development.