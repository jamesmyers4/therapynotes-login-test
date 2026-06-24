using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TherapyNotesUITests.Pages
{
    public class DashboardPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public DashboardPage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        public bool IsLoaded()
        {
            wait.Until(d => d.Url.Contains("/app/"));
            return driver.Url.Contains("/app/");
        }

        public bool IsWelcomeHeaderDisplayed()
        {
            try
            {
                wait.Until(d => d.FindElement(By.CssSelector("[data-testid='home-welcome-header']")).Displayed);
                return driver.FindElement(By.CssSelector("[data-testid='home-welcome-header']")).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public string GetWelcomeText()
        {
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
            return driver.FindElement(By.CssSelector("[data-testid='home-welcome-header']")).Text;
        }
    }
}