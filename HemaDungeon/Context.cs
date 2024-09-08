using HemaDungeon.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon;

public sealed class Context : IdentityDbContext<Character>
{
    public Context(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}