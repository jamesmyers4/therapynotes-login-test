using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TherapyNotesUITests.Tests
{
    public class ResponsiveTests : IDisposable
    {
        private IWebDriver driver;

        public ResponsiveTests()
        {
            var options = new ChromeOptions();
            if (TestConfig.Headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
            }
            driver = new ChromeDriver(options);
        }

        [Theory]
        [InlineData(390, 844, "iPhone 14")]
        [InlineData(768, 1024, "iPad")]
        [InlineData(1920, 1080, "Desktop")]
        public void LoginPage_RendersCorrectlyAtViewport(int width, int height, string device)
        {
            driver.Manage().Window.Size = new System.Drawing.Size(width, height);
            driver.Navigate().GoToUrl(TestConfig.BaseUrl + "/app/login/");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("PracticeCode")).Displayed);
            var practiceCodeField = driver.FindElement(By.Id("PracticeCode"));
            Assert.True(practiceCodeField.Displayed, $"PracticeCode field not visible on {device} ({width}x{height})");
        }

        public void Dispose()
        {
            driver.Quit();
        }
    }
}