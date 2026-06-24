using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TherapyNotesUITests.Pages;

namespace TherapyNotesUITests.Tests
{
    public class LoginTests : IDisposable
    {
        private IWebDriver driver;

        public LoginTests()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Fact]
        public void LoginToTherapyNotes()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var loginPage = new LoginPage(driver, wait);
            var dashboard = new DashboardPage(driver, wait);
            loginPage.Navigate();
            loginPage.ClickLoginLink();
            loginPage.EnterPracticeCode(TestConfig.PracticeCode);
            loginPage.EnterCredentials(TestConfig.Username, TestConfig.Password);
            Assert.True(dashboard.IsLoaded());
            Assert.Contains("Welcome", dashboard.GetWelcomeText());
            Assert.True(dashboard.GetWelcomeText().Length > 0);
        }

        [Fact]
        public void InvalidPracticeCode_ShowsErrorMessage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var loginPage = new LoginPage(driver, wait);
            loginPage.Navigate();
            loginPage.ClickLoginLink();
            loginPage.EnterPracticeCode("InvalidPracticeCode");
            wait.Until(d => {
                try
                {
                    return d.FindElement(By.CssSelector("[data-testid='login-banner-error-message']")).Displayed;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
            var error = driver.FindElement(By.CssSelector("[data-testid='login-banner-error-message']"));
            Assert.True(error.Displayed);
            Assert.Contains("practice code", error.Text);
            Assert.Contains("/app/login/", driver.Url);
        }

        [Fact]
        public void InvalidCredentials_ShowsErrorMessage()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var loginPage = new LoginPage(driver, wait);
            loginPage.Navigate();
            loginPage.ClickLoginLink();
            loginPage.EnterPracticeCode(TestConfig.PracticeCode);
            loginPage.EnterCredentials("WrongUser", "WrongPassword");
            wait.Until(d => {
                try
                {
                    return d.FindElement(By.CssSelector("[data-testid='login-banner-error-message']")).Displayed;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
            var error = driver.FindElement(By.CssSelector("[data-testid='login-banner-error-message']"));
            Assert.True(error.Displayed);
            Assert.Contains("username and password", error.Text);
            Assert.Contains("/app/login/", driver.Url);
        }

        [Theory]
        [InlineData("", "TestUser", "HorshamPA19044@@")]
        [InlineData("QAInterviewPractice", "", "HorshamPA19044@@")]
        [InlineData("QAInterviewPractice", "TestUser", "")]
        public void EmptyFields_PreventLogin(string practiceCode, string username, string password)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var loginPage = new LoginPage(driver, wait);
            loginPage.Navigate();
            loginPage.ClickLoginLink();
            if (string.IsNullOrEmpty(practiceCode))
            {
                wait.Until(d => d.FindElement(By.Id("PracticeCode")).Displayed);
                var continueButton = driver.FindElement(By.Id("Continue__ContinueButton"));
                Assert.Equal("true", continueButton.GetAttribute("aria-disabled"));
            }
            else
            {
                loginPage.EnterPracticeCode(practiceCode);
                wait.Until(d => d.FindElement(By.Id("Login__UsernameField")).Displayed);
                driver.FindElement(By.Id("Login__UsernameField")).SendKeys(username);
                driver.FindElement(By.Id("Login__Password")).SendKeys(password);
                var loginButton = driver.FindElement(By.Id("Login__LogInButton"));
                Assert.Equal("true", loginButton.GetAttribute("aria-disabled"));
            }
            Assert.Contains("/app/login/", driver.Url);
        }
        public void Dispose()
        {
            driver.Quit();
        }
    }
}