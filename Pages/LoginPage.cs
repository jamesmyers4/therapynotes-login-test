using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TherapyNotesUITests.Pages
{
    public class LoginPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public LoginPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        public void Navigate()
        {
            driver.Navigate().GoToUrl("https://www.therapynotes.com");
            wait.Until(d => d.FindElement(By.CssSelector("a[href='/app/login/']")).Displayed);
                    }

        public void ClickLoginLink()
        {
            driver.FindElement(By.CssSelector("a[href='/app/login/']")).Click();
        }

        public void EnterPracticeCode(string code)
        {
            wait.Until(d => d.FindElement(By.Id("PracticeCode")).Displayed);
            driver.FindElement(By.Id("PracticeCode")).SendKeys(code);
            driver.FindElement(By.Id("Continue__ContinueButton")).Click();
        }

        public void EnterCredentials(string username, string password)
        {
            wait.Until(d => d.FindElement(By.Id("Login__UsernameField")).Displayed);
            driver.FindElement(By.Id("Login__UsernameField")).SendKeys(username);
            driver.FindElement(By.Id("Login__Password")).SendKeys(password);
            driver.FindElement(By.Id("Login__LogInButton")).Click();
        }

        public bool IsErrorMessageDisplayed()
        {
            try
            {
                wait.Until(d => d.FindElement(By.CssSelector("[data-testid='login-banner-error-message']")).Displayed);
                return driver.FindElement(By.CssSelector("[data-testid='login-banner-error-message']")).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}