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

## Final Project Structure
Pages/
  LoginPage.cs
  DashboardPage.cs
Tests/
  LoginTests.cs
  AccessibilityTests.cs
Config/
  TestConfig.cs

## Final Test Suite
- 9 tests total across 2 test classes
- Full suite runtime: ~28 seconds
- Individual happy path: ~4.2 seconds
- All tests use explicit waits — no static Thread.Sleep

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

## Key Learnings
- Always inspect real DOM elements rather than assuming field IDs
- Two-step login flows require understanding the full user journey before writing selectors
- Explicit waits are always preferable to static sleeps
- `data-testid` attributes are the gold standard for test selectors
- Flaky timing issues should be resolved with try/catch lambda waits, not sleep