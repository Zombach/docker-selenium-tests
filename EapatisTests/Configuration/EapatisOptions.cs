namespace EapatisTests.Configuration;

public sealed record EapatisOptions(string Uri)
{
    public const string SectionKey = nameof(EapatisOptions);
}