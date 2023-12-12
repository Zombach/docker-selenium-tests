using System.Reflection;
using EapatisTests.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace EapatisTests.Providers;

public class WebClientProvider(EapatisOptions eapatisOptions) : IDisposable
{
    private RemoteWebDriver? _remoteWebDriver;

    public void Initial(RemoteWebDriver remoteWebDriver)
    => _remoteWebDriver = remoteWebDriver;

    public IWebElement? FindElement(By by, int timeoutInSeconds = 0)
    {
        ArgumentNullException.ThrowIfNull(_remoteWebDriver, $"Не инициализирован, {_remoteWebDriver}");

        if (timeoutInSeconds is 0) { return _remoteWebDriver.FindElement(by); }

        var fluentWait = new DefaultWait<IWebDriver>(_remoteWebDriver)
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

    public IWebElement? FindElementWithinElement(IWebElement element, By by, int timeoutInSeconds = 0)
    {
        ArgumentNullException.ThrowIfNull(_remoteWebDriver, $"Не инициализирован, {_remoteWebDriver}");
        throw new NotImplementedException();
    }

    public async Task NavigateEapatis()
    {
        ArgumentNullException.ThrowIfNull(_remoteWebDriver, $"Не инициализирован, {_remoteWebDriver}");
        _remoteWebDriver.Navigate().GoToUrl(eapatisOptions.Uri);
        await Task.Delay(TimeSpan.FromMilliseconds(500));
    }

    public async Task Navigate(string uri)
    {
        ArgumentNullException.ThrowIfNull(_remoteWebDriver, $"Не инициализирован, {_remoteWebDriver}");
        _remoteWebDriver.Navigate().GoToUrl(uri);
        await Task.Delay(TimeSpan.FromMilliseconds(500));
    }

    public async Task Authorization()
    => await Authorization(eapatisOptions.Login, eapatisOptions.Password);

    public async Task Authorization(string login, string password)
    {
        ArgumentNullException.ThrowIfNull(_remoteWebDriver, $"Не инициализирован, {_remoteWebDriver}");

        var loginInput = _remoteWebDriver.FindElement(By.Name("USER"));
        loginInput.SendKeys(login);
        await Task.Delay(TimeSpan.FromMilliseconds(100));

        var passwordInput = _remoteWebDriver.FindElement(By.Name("PASS"));
        passwordInput.SendKeys(password);
        await Task.Delay(TimeSpan.FromMilliseconds(100));

        var loginButton = _remoteWebDriver.FindElement(By.CssSelector("input[type=SUBMIT]"));
        loginButton.Click();
        await Task.Delay(TimeSpan.FromMilliseconds(200));
    }

    public void Dispose()
    {
        if (_remoteWebDriver is not null)
        {
            _remoteWebDriver.Quit();
            _remoteWebDriver.Dispose();
        }
    }
}