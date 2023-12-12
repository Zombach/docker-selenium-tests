using FluentAssertions;
using OpenQA.Selenium;
using Xunit;

namespace EapatisTests.Bug;

public class EapatisInternalTests
{
    private const string ExternalUri = "http://www.eapatis.com/";

    //[Fact]
    //public async Task Test1()
    //{
    //    await Container.ContainerFactory();
    //    await Task.Delay(TimeSpan.FromSeconds(5));

    //    using WebClient webClient = new WebClient("http://localhost:4444/wd/hub");


    //    webClient.NavigateEapatis(ExternalUri);
    //    webClient.Authorization();

    //    var find = webClient.FindElement(By.Id("th02"));
    //    find.Should().NotBeNull();
    //    find?.Click();

    //    var db = webClient.FindElement(By.Id("cbEATXT"));
    //    db.Should().NotBeNull();
    //    db?.Click();

    //    var questText = webClient.FindElement(By.Id("questText"));
    //    questText.Should().NotBeNull();
    //    questText?.SendKeys("Рыба");

    //    var findButton = webClient.FindElement(By.Id("sButtons"))?.FindElement(By.CssSelector("input[type=button]"));
    //    findButton.Should().NotBeNull();
    //    findButton?.Click();

    //    var listButtonEatxt2 = webClient.FindElement(By.Id("list_button_EATXT1"), 30);
    //    listButtonEatxt2.Should().NotBeNull();
    //    listButtonEatxt2?.Click();

    //    Assert.Equal("", "");
    //}

    //[Fact]
    //public async Task Test2()
    //{
    //    await Container.ContainerFactory();
    //    await Task.Delay(TimeSpan.FromSeconds(5));

    //    WebClient webClient = new WebClient("http://localhost:4444/wd/hub");
    //    webClient.Authorization();

    //    var find = webClient.FindElement(By.Id("th02"));
    //    find.Should().NotBeNull();
    //    find?.Click();

    //    var db = webClient.FindElement(By.Id("cbEATXT"));
    //    db.Should().NotBeNull();
    //    db?.Click();

    //    var questText = webClient.FindElement(By.Id("questText"));
    //    questText.Should().NotBeNull();
    //    questText?.SendKeys("Овощи");

    //    var findButton = webClient.FindElement(By.Id("sButtons"))?.FindElement(By.CssSelector("input[type=button]"));
    //    findButton.Should().NotBeNull();
    //    findButton?.Click();

    //    var listButtonEatxt2 = webClient.FindElement(By.Id("list_button_EATXT1"), 30);
    //    listButtonEatxt2.Should().NotBeNull();
    //    listButtonEatxt2?.Click();

    //    Assert.Equal("", "");
    //}
}