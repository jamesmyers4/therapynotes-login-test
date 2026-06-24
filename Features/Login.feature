Feature: Login
  As a TherapyNotes user
  I want to log in to my practice account
  So that I can access the dashboard

  Scenario: Successful login with valid credentials
    Given I navigate to the TherapyNotes homepage
    And I enter a valid practice code and click Continue
    And I enter valid credentials and click Log In
    Then I should see the dashboard welcome header
    And the URL should contain "/app/"

  Scenario: Invalid practice code shows error message
    Given I navigate to the TherapyNotes homepage
    And I enter an invalid practice code and click Continue
    Then I should see a login error message
    And the URL should remain on the login page

  Scenario: Invalid credentials show error message
    Given I navigate to the TherapyNotes homepage
    And I enter a valid practice code and click Continue
    And I enter invalid credentials and click Log In
    Then I should see a login error message
    And the URL should remain on the login page