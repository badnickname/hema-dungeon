using System.Text.Json.Serialization;

namespace HemaDungeon.Entities;

public sealed class Page
{
    public string Id { get; set; }

    [JsonIgnore]
    public Character Character { get; set; }

    public int Number { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}