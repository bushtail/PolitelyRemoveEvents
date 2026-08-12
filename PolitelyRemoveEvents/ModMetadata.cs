using JetBrains.Annotations;
using SPTarkov.Server.Core.Models.Spt.Mod;
using Range = SemanticVersioning.Range;
using Version = SemanticVersioning.Version;

namespace PolitelyRemoveEvents;

[UsedImplicitly]
public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "ca.bushtail.politelyremoveevents";
    public string Name { get; init; } = "PolitelyRemoveEvents";
    public string Author { get; init; } = "bushtail";
    public List<string>? Contributors { get; init; }
    public Version Version { get; init; } = new("1.0.1");
    public Range SptVersion { get; init; } = new("~4.0.0");
    public bool HasPrepatcher { get; init; }
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, Range>? ModDependencies { get; init; }
    public string? Url { get; init; }
    public string License { get; init; } = "MIT";
}