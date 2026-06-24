using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

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
}

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