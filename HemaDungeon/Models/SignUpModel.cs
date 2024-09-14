using System.ComponentModel.DataAnnotations;

namespace HemaDungeon.Models;

public sealed class SignUpModel
{
    public string Name { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public string Story { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string? Author { get; set; }

    public int? PushUp { get; set; }

    public int? PullUp { get; set; }

    public int? Abdominal { get; set; }

    public float? RunTwenty { get; set; }

    public int? RunFifteen { get; set; }

    public int? Rang { get; set; }

    public int? Score { get; set; }

    public int? Rope { get; set; }
}