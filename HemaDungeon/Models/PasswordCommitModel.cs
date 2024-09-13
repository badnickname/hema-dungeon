namespace HemaDungeon.Models;

public sealed class PasswordCommitModel
{
    public string Email { get; set; }

    public string Code { get; set; }

    public string Password { get; set; }
}