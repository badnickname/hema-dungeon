using HemaDungeon.Core.Entities;
using HemaDungeon.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HemaDungeon.Workers;

public sealed class AdminWorker(IServiceProvider provider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = provider.CreateScope();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<AdminOptions>>();
        var manager = scope.ServiceProvider.GetRequiredService<UserManager<Character>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!roleManager.Roles.Any(x => x.Name == "Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        var admins = await manager.GetUsersInRoleAsync("Admin");
        foreach (var admin in admins)
        {
            await manager.RemoveFromRoleAsync(admin, "Admin");
        }

        foreach (var email in options.Value.Emails)
        {
            var user = await manager.FindByEmailAsync(email);
            if (user is null) continue;
            var roles = await manager.GetRolesAsync(user);
            if (roles.Contains("Admin")) continue;
            await manager.AddToRoleAsync(user, "Admin");
        }
    }
}