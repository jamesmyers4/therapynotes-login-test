using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using TherapyNotesUITests;
using TherapyNotesUITests.Pages;
using Xunit;

namespace TherapyNotesUITests.StepDefinitions
{
    [Binding]
    public class LoginSteps : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly LoginPage _loginPage;
        private readonly DashboardPage _dashboardPage;

        public LoginSteps()
        {
            var options = new ChromeOptions();
            if (TestConfig.Headless)
                options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _loginPage = new LoginPage(_driver, _wait);
            _dashboardPage = new DashboardPage(_driver, _wait);
        }

        [Given(@"I navigate to the TherapyNotes homepage")]
        public void GivenINavigateToTheTherapyNotesHomepage()
        {
            _loginPage.Navigate();
            _loginPage.ClickLoginLink();
        }

        [Given(@"I enter a valid practice code and click Continue")]
        public void WhenIEnterAValidPracticeCodeAndClickContinue()
        {
            _loginPage.EnterPracticeCode(TestConfig.PracticeCode);
        }

        [Given(@"I enter an invalid practice code and click Continue")]
        public void WhenIEnterAnInvalidPracticeCodeAndClickContinue()
        {
            _loginPage.EnterPracticeCode("InvalidPracticeCode999");
        }

        [Given(@"I enter valid credentials and click Log In")]
        public void WhenIEnterValidCredentialsAndClickLogIn()
        {
            _loginPage.EnterCredentials(TestConfig.Username, TestConfig.Password);
        }

        [Given(@"I enter invalid credentials and click Log In")]
        public void WhenIEnterInvalidCredentialsAndClickLogIn()
        {
            _loginPage.EnterCredentials("wronguser", "wrongpassword");
        }

        [Then(@"I should see the dashboard welcome header")]
        public void ThenIShouldSeeTheDashboardWelcomeHeader()
        {
            Assert.True(_dashboardPage.IsWelcomeHeaderDisplayed());
        }

        [Then(@"the URL should contain ""(.*)""")]
        public void ThenTheUrlShouldContain(string path)
        {
            Assert.Contains(path, _driver.Url);
        }

        [Then(@"I should see a login error message")]
        public void ThenIShouldSeeALoginErrorMessage()
        {
            Assert.True(_loginPage.IsErrorMessageDisplayed());
        }

        [Then(@"the URL should remain on the login page")]
        public void ThenTheUrlShouldRemainOnTheLoginPage()
        {
            Assert.Contains("/app/login", _driver.Url);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}