using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TherapyNotesUITests
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
            driver.Navigate().GoToUrl("https://www.therapynotes.com");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.CssSelector("a[href='/app/login/']")).Displayed);

            var loginLink = driver.FindElement(By.CssSelector("a[href='/app/login/']"));
            loginLink.Click();

            wait.Until(d => d.FindElement(By.Id("PracticeCode")).Displayed);
            var practiceCodeField = driver.FindElement(By.Id("PracticeCode"));
            practiceCodeField.SendKeys("QAInterviewPractice");
            var practiceSubmit = driver.FindElement(By.Id("Continue__ContinueButton"));
            practiceSubmit.Click();

            wait.Until(d => d.FindElement(By.Id("Login__UsernameField")).Displayed);
            var usernameField = driver.FindElement(By.Id("Login__UsernameField"));
            var passwordField = driver.FindElement(By.Id("Login__Password"));
            usernameField.SendKeys("TestUser");
            passwordField.SendKeys("HorshamPA19044@@");

            var loginButton = driver.FindElement(By.Id("Login__LogInButton"));
            loginButton.Click();

            wait.Until(d => {
                try
                {
                    return d.FindElement(By.CssSelector("[data-testid='home-welcome-header']")).Displayed;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
            var welcomeHeader = driver.FindElement(By.CssSelector("[data-testid='home-welcome-header']"));
            Assert.True(welcomeHeader.Displayed);
            Assert.Contains("Welcome", welcomeHeader.Text);
            Assert.Contains("/app/", driver.Url);
            
        }

        public void Dispose()
        {
            driver.Quit();
        }
    }
}