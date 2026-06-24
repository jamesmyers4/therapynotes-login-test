using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TherapyNotesUITests.Pages;

namespace TherapyNotesUITests.Tests
{
    public class SecurityTests : IDisposable
    {
        private IWebDriver driver;

        public SecurityTests()
        {
            var options = new ChromeOptions();
            if (TestConfig.Headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
            }
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
        }

        [Fact]
        public void LoginPage_ServedOverHttps()
        {
            driver.Navigate().GoToUrl(TestConfig.BaseUrl + "/app/login/");
            Assert.StartsWith("https://", driver.Url);
        }

        [Fact]
        public void PasswordField_TypeAttributeIsPassword()
        {
            driver.Navigate().GoToUrl(TestConfig.BaseUrl + "/app/login/");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var loginPage = new LoginPage(driver, wait);
            loginPage.EnterPracticeCode(TestConfig.PracticeCode);
            wait.Until(d => d.FindElement(By.Id("Login__Password")).Displayed);
            var passwordField = driver.FindElement(By.Id("Login__Password"));
            Assert.Equal("password", passwordField.GetAttribute("type"));
        }

        [Fact]
        public void DirectDashboardAccess_RedirectsToLogin()
        {
            driver.Navigate().GoToUrl(TestConfig.BaseUrl + "/app/");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/app/login/") || d.Url.Contains("/app/"));
            Assert.Contains("/app/login/", driver.Url);
        }

        public void Dispose()
        {
            driver.Quit();
        }
    }
}