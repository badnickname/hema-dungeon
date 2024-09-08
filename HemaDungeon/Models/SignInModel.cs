using System.ComponentModel.DataAnnotations;

namespace HemaDungeon.Models;

public sealed class SignInModel
{
    [RegularExpression("^[^><;]+$")]
    public string Email { get; set; }

    [RegularExpression("^[^><;]+$")]
    public string Password { get; set; }
}