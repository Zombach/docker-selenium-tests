namespace EapatisTests.Configuration;

public sealed record EapatisOptions(string Uri, string Login, string Password)
{
    public const string SectionKey = nameof(EapatisOptions);
}