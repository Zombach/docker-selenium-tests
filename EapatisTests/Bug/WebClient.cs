using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Reflection;

namespace EapatisTests.Bug;

public class WebClient(IWebDriver webDriver, string urlHub) : IDisposable
{

    public IWebElement? FindElement(By by, int timeoutInSeconds = 0)
    {
        if (timeoutInSeconds is 0) { return webDriver.FindElement(by); }

        var fluentWait = new DefaultWait<IWebDriver>(webDriver)
        {
            Timeout = TimeSpan.FromSeconds(timeoutInSeconds),
            PollingInterval = TimeSpan.FromMilliseconds(500),
            Message = "Element to be searched not found"
        };

        fluentWait.IgnoreExceptionTypes
        (
            typeof(NoSuchElementException),
            typeof(TargetInvocationException),
            typeof(InvalidOperationException)
        );

        return fluentWait.Until(driver =>
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException or NoSuchElementException or InvalidOperationException)
                {
                    return null;
                }
                throw;
            }
        });
    }

    public void NavigateEapatis(string uri) => webDriver.Navigate().GoToUrl(uri);

    public void Authorization(string login = "skozlov", string password = "skozlov")
    {
        var loginInput = webDriver.FindElement(By.Name("USER"));
        loginInput.SendKeys(login);

        var passwordInput = webDriver.FindElement(By.Name("PASS"));
        passwordInput.SendKeys(password);

        var loginButton = webDriver.FindElement(By.CssSelector("input[type=SUBMIT]"));
        loginButton.Click();
    }

    public void Dispose()
    {
        webDriver.Quit();
        webDriver.Dispose();
    }
}