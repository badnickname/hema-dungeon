using System.ComponentModel.DataAnnotations;

namespace HemaDungeon.Models;

public sealed class SignUpModel
{
    [RegularExpression("^[^><;]+$")]
    public string Name { get; set; }

    public int Age { get; set; }

    [RegularExpression("^[^><;]+$")]
    public string Gender { get; set; }

    [RegularExpression("^[^><]+$")]
    public string Story { get; set; }

    [RegularExpression("^[^><;]+$")]
    public string Email { get; set; }

    [RegularExpression("^[^><;]+$")]
    public string Password { get; set; }
}