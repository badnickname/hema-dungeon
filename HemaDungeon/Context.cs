using HemaDungeon.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon;

public sealed class Context : IdentityDbContext<Character>
{
    public DbSet<FightCharacter> FightCharacters { get; set; }

    public DbSet<FightState> FightStates { get; set; }

    public DbSet<Visit> Visits { get; set; }

    public DbSet<Result> Results { get; set; }

    public DbSet<DeadCharacter> DeadCharacters { get; set; }

    public Context(DbContextOptions options) : base(options)
    {
#if RELEASE
        Database.Migrate();
#endif
    }
}