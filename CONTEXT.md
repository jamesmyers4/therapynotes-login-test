# CONTEXT.md — TherapyNotes Login Test Suite

## Project Purpose
A C# xUnit Selenium test suite validating the TherapyNotes.com login flow.
Built as technical interview preparation for a QA Engineer role at TherapyNotes,
then expanded into a full demonstration of professional test engineering practices.

This is not a toy project — it tests a live production system, uses real DOM
inspection, handles a genuine two-step login flow, and is organized the way a
real test suite in a professional shop would be.

---

## Tech Stack
- **Language:** C# / .NET 10
- **Test Framework:** xUnit ([Fact], [Theory], [InlineData])
- **BDD Framework:** Reqnroll (SpecFlow successor) with Gherkin feature files
- **Browser Automation:** Selenium WebDriver 4.x
- **Driver Management:** Selenium Manager (built into Selenium 4.6+, no manual ChromeDriver install)
- **Design Pattern:** Page Object Model (POM)
- **API Testing:** HttpClient + System.Text.Json
- **CI/CD:** GitHub Actions
- **IDE:** Visual Studio 2026

---

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
---

## Architecture Decisions

### Page Object Model
All element locators and interactions live in `Pages/` — never in test methods.
Test methods read like plain English. Selector changes require one file update, not many.

### TestConfig
Single source of truth for all configuration:
- BaseUrl, LoginPath, DashboardPath
- PracticeCode, Username, Password
- Headless boolean toggle

In a real environment TestConfig would read from appsettings.json or environment
variables so the same suite runs against dev, staging, and production without
touching test code.

### Explicit Waits
No Thread.Sleep anywhere in the suite. All waits use WebDriverWait with lambda
conditions. The try/catch lambda pattern handles elements that may not exist yet
without throwing immediately:

```csharp
wait.Until(d => {
    try { return d.FindElement(By.CssSelector("[data-testid='...']")).Displayed; }
    catch (NoSuchElementException) { return false; }
});
```

### data-testid Selectors
Preferred over ID, class, or XPath where available. data-testid attributes are
placed explicitly for testing, survive UI refactors, and signal accessibility intent.

### Headless Toggle
All test class constructors check TestConfig.Headless. Flip one value to switch
the entire suite between headed (demo/debug) and headless (CI/CD) mode.

---

## The Login Flow (Two-Step)
TherapyNotes uses a two-step login that the original scaffold was unaware of:

1. Navigate to www.therapynotes.com
2. Click "Log In" link (anchor tag with href=/app/login/)
3. Enter Practice Code → click Continue (Continue__ContinueButton)
4. Enter Username + Password → click Log In (Login__LogInButton)
5. Assert dashboard loaded (/app/ URL + home-welcome-header data-testid)

---

## Real Field IDs (from DOM inspection)
| Field | ID |
|---|---|
| Practice Code input | PracticeCode |
| Continue button | Continue__ContinueButton |
| Username input | Login__UsernameField |
| Password input | Login__Password |
| Log In button | Login__LogInButton |
| Welcome header | data-testid="home-welcome-header" |
| Error banner | data-testid="login-banner-error-message" |
| Forgot practice code | data-testid="login-forgot-practice-code-link" |
| Lockout dialog | data-testid="login-error-dialog-message" |
---

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

---

## Test Categories
Tests are tagged by risk profile to prevent accidental account lockout:
- [Trait("Category", "Destructive")] in xUnit — tests that submit invalid credentials
- @Destructive in Reqnroll — BDD scenarios that submit invalid credentials
- Standard run (excludes destructive): `dotnet test --filter "Category!=Destructive"`
- Deliberate destructive run: `dotnet test --filter "Category=Destructive"`

---

## What's Not Tested (and Why)
- **Full forgot password flow** — triggers real emails, outside scope of test account
- **Session timeout** — requires waiting for inactivity period, not practical in this context
- **Rate limiting / lockout** — would lock the test account, destructive
- **Internal API routes** — not publicly documented, require internal auth tokens
- **Database layer** — no access to TherapyNotes internal infrastructure
- **Account lockout** — triggering lockout requires 5+ failed logins and locks 
  the real account for 15 minutes. Discovered during test development and documented 
  in SESSION_LOG entry #19. Tagged as Destructive to prevent accidental triggering.


---

## Known Constraints
- Tests run against live production TherapyNotes.com — no sandbox/staging access
- Test credentials (QAInterviewPractice / TestUser) provided by TherapyNotes for assessment
- Manual login with these credentials does not work — account appears to be
  automation-only by design (possible IP restriction or session configuration)
- ApiTests use JSONPlaceholder as a structural demonstration — not TherapyNotes' real API 
- Destructive tests (invalid credential submissions) are tagged [Trait("Category", "Destructive")] 
  and excluded from standard runs to prevent account lockout

---

## If Expanding This Suite (TODO)
- Add appsettings.json for environment-specific config
- Add parallel test execution (xUnit supports this natively)
- Add screenshot on failure for debugging
- Add Allure or similar for test reporting
- Add AccountLockout_ShowsLockoutDialog test against a dedicated lockout account