using System.ComponentModel.DataAnnotations;

namespace HemaDungeon.Models;

public sealed class EditModel
{
    [RegularExpression("^[^><;]+$")]
    public string Name { get; set; }

    public int Age { get; set; }

    [RegularExpression("^[^><;]+$")]
    public string Gender { get; set; }

    [RegularExpression("^[^><]+$")]
    public string Story { get; set; }

    [RegularExpression("^[^><;]+$")]
    public string Author { get; set; }
}