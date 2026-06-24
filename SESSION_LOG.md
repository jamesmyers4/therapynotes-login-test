# SESSION_LOG.md — TherapyNotes Login Test

## Overview
Built as technical interview preparation for a QA Engineer role at TherapyNotes.
Reconstructed and improved upon a C# xUnit Selenium test scaffold, implementing
a full login flow against the live TherapyNotes website.

## Original Scaffold Issues

### 1. Duplicate variable name
The scaffold declared `var usernameField` twice — once for the practice code field 
and once for the actual username field. This would cause a compiler error in real C#.

**Fix:** Renamed the first declaration to `practiceCodeField` to accurately reflect 
its purpose and eliminate the naming conflict.

### 2. Incorrect field IDs
The scaffold used generic assumed IDs (`username`, `practice_code`, `password`). 
Inspecting the live site revealed the actual IDs were different.

**Fix:** Replaced with real IDs found via browser inspection:
- `PracticeCode`
- `Login__UsernameField`  
- `Login__Password`
- `Login__LogInButton`

### 3. Unaware of two-step login flow
The scaffold assumed a single login form. TherapyNotes actually uses a two-step flow:
1. Enter practice code → click Continue
2. Enter username and password → click Log In

**Fix:** Added the Continue button step using `Continue__ContinueButton` and 
restructured the credential entry accordingly.

### 4. Incorrect link selector
The scaffold used `By.LinkText("Log In")` to find the homepage login link. 
The actual element is an `<a>` tag containing a `<span>` — `LinkText` doesn't 
work here.

**Fix:** Used `By.CssSelector("a[href='/app/login/']")` to target the anchor 
directly by its href.

### 5. Static waits replaced with explicit waits
The scaffold used `System.Threading.Thread.Sleep(2000)` for page transitions.

**Fix:** Replaced all static waits with `WebDriverWait` explicit waits targeting 
specific elements, making the test faster and more reliable.

### 6. Improved assertions
The scaffold checked a generic `h1` element.

**Fix:** 
- Used `data-testid='home-welcome-header'` for a more resilient selector
- Added `welcomeHeader.Displayed` assertion
- Added URL assertion `Assert.Contains("/app/", driver.Url)`
- Avoided asserting the specific user's name to prevent brittle data-dependent failures

### 7. Refactored to Page Object Model
The initial implementation had all selectors and interactions inline in the test method.

**Fix:** Refactored into three files following POM convention:
- `Pages/LoginPage.cs` — encapsulates homepage navigation, login link, 
  practice code entry, and credential entry
- `Pages/DashboardPage.cs` — encapsulates post-login verification (URL check, 
  welcome header text)
- `Tests/LoginTests.cs` — clean test method that reads like plain English, 
  no raw selectors

**Why:** In a real test suite, raw selectors scattered through test methods 
become a maintenance nightmare. POM centralizes element definitions so a 
selector change only needs to be updated in one place.

### 8. Centralized credentials via TestConfig
Credentials and URLs were hardcoded directly in test methods.

**Fix:** Created `Config/TestConfig.cs` with static constants for BaseUrl,
PracticeCode, Username, Password, and path segments. All tests reference
TestConfig rather than inline strings.

**Why:** One URL or credential change updates every test automatically.
Also keeps sensitive data out of test logic and in one auditable location.

### 9. Negative test cases
The initial implementation only covered the happy path.

**Fix:** Added two negative tests:
- `InvalidPracticeCode_ShowsErrorMessage` — verifies error banner appears
  and URL remains on login page when an invalid practice code is entered
- `InvalidCredentials_ShowsErrorMessage` — verifies error banner appears
  and URL remains on login page when wrong username/password are entered

**Why:** Happy path tests only prove the system works when everything goes
right. Negative tests prove the system fails correctly — especially critical
in healthcare software where unauthorized access must be explicitly prevented.

### 10. Data-driven testing via [Theory] and [InlineData]
No coverage for empty field submission behavior.

**Fix:** Added `EmptyFields_PreventLogin` using xUnit's [Theory] with three
[InlineData] sets — empty practice code, empty username, empty password.
Discovered TherapyNotes uses aria-disabled="true" on submit buttons when
fields are empty, so assertions check aria state rather than attempting
a click.

**Why:** [Theory] tests multiple scenarios without duplicating test code.
The aria-disabled discovery also confirmed TherapyNotes implements
accessibility-compliant form validation.

### 11. Accessibility test coverage
No accessibility assertions existed in the initial implementation.

**Fix:** Added `AccessibilityTests.cs` with three tests:
- `LoginPage_PracticeCodeField_HasAccessibleLabel` — verifies aria-describedby
  is present on the practice code input
- `LoginPage_ContinueButton_HasCorrectAriaState` — verifies aria-disabled is
  true on Continue when no practice code is entered
- `LoginPage_UsernameField_HasAccessibleLabel` — verifies aria-describedby
  is present on the username field after the two-step flow

**Why:** TherapyNotes serves clinicians who may use assistive technology.
Accessibility is not optional in healthcare software.

### 12. API test layer via HttpClient
No API-level test coverage existed in the initial implementation.

**Fix:** Added `ApiTests.cs` demonstrating HTTP layer testing using HttpClient
against JSONPlaceholder (jsonplaceholder.typicode.com) — a public REST API
designed for testing. Four tests covering:
- GET request returns 200 OK
- GET response contains expected JSON fields (id, title, body)
- GET invalid resource returns 404
- POST request returns 201 Created

**Why:** In a real TherapyNotes context these patterns apply directly to internal
API routes with authentication headers added via
client.DefaultRequestHeaders.Authorization. API tests run at 72-156ms vs
2-4 seconds for UI tests — pushing coverage down to the API layer where
possible is faster, cheaper, and more reliable than browser-driven tests.

**Note:** JSONPlaceholder is used here as a structural demonstration. Real
API test suites would target internal endpoints with environment-specific
base URLs stored in TestConfig.

### 13. Security test coverage
No security-focused assertions existed in the initial implementation.

**Fix:** Added `SecurityTests.cs` with three tests:
- `LoginPage_ServedOverHttps` — verifies the login page is served over HTTPS
- `PasswordField_TypeAttributeIsPassword` — verifies password input is masked
- `DirectDashboardAccess_RedirectsToLogin` — verifies unauthenticated users
  cannot access the dashboard directly and are redirected to login

**Why:** In healthcare software, security validation is non-negotiable.
HIPAA-adjacent systems must enforce HTTPS, mask sensitive input, and
prevent unauthorized access. These tests verify TherapyNotes implements
all three correctly.

### 14. Responsive layout testing
No viewport or device testing existed in the initial implementation.

**Fix:** Added `ResponsiveTests.cs` using [Theory] with [InlineData] to
test three viewports in one method:
- 390x844 — iPhone 14
- 768x1024 — iPad
- 1920x1080 — Desktop

Each assertion includes the device name in the failure message for
immediate identification of which viewport broke.

**Why:** TherapyNotes serves clinicians who may access the platform from
any device. The login form must be functional at every viewport.
[Theory] with device-labeled assertions makes failures immediately
actionable without running tests one at a time.

### 15. Headless Chrome toggle via TestConfig
Tests previously always ran with a visible browser, making CI/CD
integration impossible and local runs visually noisy.

**Fix:** Added `Headless` boolean to `TestConfig.cs`. All test class
constructors check this value and apply Chrome options accordingly.
Flip one value to switch all tests between headed and headless mode.

**Why:** CI/CD environments like GitHub Actions have no display — headless
is required for automated pipeline runs. The toggle also allows developers
to run headless locally when visual confirmation isn't needed, reducing
distraction without losing the ability to watch tests run when debugging.

### 16. Reqnroll BDD layer added
Added a Gherkin feature file and step definitions using Reqnroll (the actively
maintained SpecFlow successor) covering three login scenarios:
- Successful login with valid credentials
- Invalid practice code shows error message  
- Invalid credentials show error message

Step definitions wire directly to existing POM methods — no duplication.
The [Given] attribute is used throughout since Reqnroll inherits keyword
context from And steps, requiring the preceding keyword's attribute type.

Note: The successful login scenario shares the same known constraint as
LoginToTherapyNotes — the test account does not complete a real dashboard
load against the live production site. Both tests are structurally correct
and pass when valid session credentials are available.

### 17. GitHub Actions CI workflow added
Added .github/workflows/ci.yml triggering on push and pull_request to main.
Credentials injected via GitHub Secrets as TN_PRACTICE_CODE, TN_USERNAME,
TN_PASSWORD. TN_HEADLESS hardcoded to true for CI environment.

### 18. TestConfig refactored to read from environment variables
Credentials moved out of hardcoded constants into Environment.GetEnvironmentVariable
calls with fallback values. Prevents credential exposure in source control.
Password fallback is an empty string — real value lives in env vars and GitHub Secrets only.

### 19. Account lockout behavior discovered during test execution
During repeated test runs while debugging the Reqnroll BDD layer, the
TherapyNotes test account triggered the platform's lockout mechanism —
5 or more failed login attempts within 15 minutes locks the account
for 15 minutes.

This surfaced a previously undocumented data-testid:
- data-testid="login-error-dialog-message" — lockout dialog message

This is distinct from data-testid="login-banner-error-message" which
appears on standard invalid credential errors.

A test for this behavior (AccountLockout_ShowsLockoutDialog) is a valid
addition but cannot be run in isolation against production — it requires
triggering 5 failed logins first, which locks a real account. In a
proper test environment this would run against a dedicated lockout test
account or a staging environment where accounts can be reset
programmatically. Flagged as a known gap with documented reasoning.

To mitigate lockout risk during normal runs, invalid credential tests
were tagged with [Trait("Category", "Destructive")] in xUnit and
@Destructive in Reqnroll. These tests are excluded from standard runs
via --filter "Category!=Destructive" and run deliberately when needed.
This maps to a real-world pattern of categorizing tests by risk profile:
Smoke (safe, every commit), Regression (full suite, nightly), and
Destructive (deliberate, never unattended against production).

## Final Project Structure
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

## Final Test Suite
- 25 tests total across 5 test classes
- Full suite runtime: ~55 seconds (headless, local environment)
- UI tests: 1.5-5 seconds each (browser-driven)
- API tests: 67-144ms each (pure HTTP, no browser)
- All UI tests use explicit waits — no static Thread.Sleep
- Headless toggle via TestConfig.Headless

## Key Learnings
- Always inspect real DOM elements rather than assuming field IDs
- Two-step login flows require understanding the full user journey before writing selectors
- Explicit waits are always preferable to static sleeps
- data-testid attributes are the gold standard for test selectors
- Flaky timing issues should be resolved with try/catch lambda waits, not sleep
- aria-disabled is the correct way to assert disabled button state in accessible UIs
- Negative tests are as important as happy path tests — especially in healthcare software
- [Theory] with [InlineData] eliminates test duplication for data-driven scenarios
- POM centralizes selectors so one UI change requires one code change
- API tests run 20-50x faster than UI tests — push coverage down the stack where possible
- Headless mode is required for CI/CD and reduces visual noise in local runs
- Security and accessibility are first-class test concerns in healthcare software
- Responsive testing with labeled assertion messages makes viewport failures immediately actionable