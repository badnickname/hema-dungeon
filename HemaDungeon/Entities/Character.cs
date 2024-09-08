using Microsoft.AspNetCore.Identity;

namespace HemaDungeon.Entities;

public sealed class Character : IdentityUser
{
    public string Name { get; set; }

    public string Avatar { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public string Story { get; set; }
}