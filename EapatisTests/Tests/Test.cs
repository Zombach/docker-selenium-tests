using EapatisTests.Common.Extension;
using EapatisTests.Providers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace EapatisTests.Tests;

public class WorkCheckingTests(ITestOutputHelper output)
: FixtureBase(collection => collection.Register()
    .AddScoped<TestContainersProvider>(),
    output
)
{
    [Theory]
    [InlineData("Рыба")]
    [InlineData("Овощи")]
    public async Task Test(string searchText)
    {
        await using var testContainersProvider = await GetTestContainersProvider();
        var webClientProvider = GetWebClientProvider(testContainersProvider);

        await webClientProvider.NavigateEapatis();
        await webClientProvider.Authorization();

        var find = webClientProvider.FindElement(By.Id("th02"));
        find.Should().NotBeNull();
        find?.Click();

        var db = webClientProvider.FindElement(By.Id("cbEATXT"));
        db.Should().NotBeNull();
        db?.Click();

        var questText = webClientProvider.FindElement(By.Id("questText"));
        questText.Should().NotBeNull();
        questText?.SendKeys(searchText);

        var findButton = webClientProvider.FindElement(By.Id("sButtons"))?.FindElement(By.CssSelector("input[type=button]"));
        findButton.Should().NotBeNull();
        findButton?.Click();

        var listButtonEatxt2 = webClientProvider.FindElement(By.Id("list_button_EATXT1"), 30);
        listButtonEatxt2.Should().NotBeNull();
        listButtonEatxt2?.Click();

        //await DisposeAsync(CurrentTestId);
    }

    [Fact]
    public void NotValid()
    {
        Assert.Equal("qwe", "123");
    }

    [Fact]
    public void Valid()
    {
        Assert.Equal(1, 1);
    }
}