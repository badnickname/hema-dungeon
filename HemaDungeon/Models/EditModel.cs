using System.ComponentModel.DataAnnotations;

namespace HemaDungeon.Models;

public sealed class EditModel
{
    public string Name { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public string Story { get; set; }

    public string? Author { get; set; }
}