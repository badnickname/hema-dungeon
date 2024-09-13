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

    public int? PushUp { get; set; }

    public int? PullUp { get; set; }

    public int? Abdominal { get; set; }

    public int? RunTwenty { get; set; }

    public int? Rang { get; set; }

    public int? Score { get; set; }

    public int? Rope { get; set; }
}