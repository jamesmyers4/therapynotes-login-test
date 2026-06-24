using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TherapyNotesUITests.Pages;

namespace TherapyNotesUITests.Tests
{
    public class AccessibilityTests : IDisposable
    {
        private IWebDriver driver;

        public AccessibilityTests()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Fact]
        public void LoginPage_PracticeCodeField_HasAccessibleLabel()
        {
            driver.Navigate().GoToUrl(TestConfig.BaseUrl + "/app/login/");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("PracticeCode")).Displayed);
            var practiceCodeInput = driver.FindElement(By.Id("PracticeCode"));
            Assert.False(string.IsNullOrEmpty(practiceCodeInput.GetAttribute("aria-describedby")));
        }

        [Fact]
        public void LoginPage_ContinueButton_HasCorrectAriaState()
        {
            driver.Navigate().GoToUrl(TestConfig.BaseUrl + "/app/login/");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("Continue__ContinueButton")).Displayed);
            var continueButton = driver.FindElement(By.Id("Continue__ContinueButton"));
            Assert.Equal("true", continueButton.GetAttribute("aria-disabled"));
        }

        [Fact]
        public void LoginPage_UsernameField_HasAccessibleLabel()
        {
            driver.Navigate().GoToUrl(TestConfig.BaseUrl + "/app/login/");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var loginPage = new LoginPage(driver, wait);
            loginPage.EnterPracticeCode(TestConfig.PracticeCode);
            wait.Until(d => d.FindElement(By.Id("Login__UsernameField")).Displayed);
            var usernameField = driver.FindElement(By.Id("Login__UsernameField"));
            Assert.False(string.IsNullOrEmpty(usernameField.GetAttribute("aria-describedby")));
        }

        public void Dispose()
        {
            driver.Quit();
        }
    }
}