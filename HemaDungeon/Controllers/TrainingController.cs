using System.Globalization;
using HemaDungeon.Entities;
using HemaDungeon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon.Controllers;

[ApiController]
[Route("api/trainings")]
public sealed class TrainingController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Start([FromForm] TrainingModel model, [FromServices] UserManager<Character> manager, [FromServices] Context context)
    {
        // Посещения
        var date = DateTime.ParseExact(model.DateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToUniversalTime().Date;

        var visits = await context.Visits.Where(x => x.Date == date).ToListAsync();
        context.Visits.RemoveRange(visits);
        await context.SaveChangesAsync();

        var users = await context.Users.ToListAsync();
        foreach (var user in users)
        {
            context.Visits.Add(new Visit
            {
                Character = user,
                Date = date,
                Id = Guid.NewGuid().ToString(),
                WasHere = model.Users?.Any(x => x.Id == user.Id && x.WasHere == true) ?? false,
                CanSkip = model.Users?.Any(x => x.Id == user.Id && x.Skip == true) ?? false
            });
        }
        await context.SaveChangesAsync();

        // Сражения
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id!;

        context.FightCharacters.RemoveRange(context.FightCharacters.Where(x => x.AuthorId == userId).ToList());
        await context.SaveChangesAsync();

        foreach (var user in context.Users.Include(x => x.Visits).AsEnumerable().Where(x => model.Users.Any(y => y.WasHere == true && x.Id == y.Id)))
        {
            context.FightCharacters.Add(new FightCharacter
            {
                Id = Guid.NewGuid().ToString(),
                Character = user,
                AuthorId = userId,
                Health = user.Vitality - (model.Users?.FirstOrDefault(x => x.Id == user.Id)?.Damage ?? 0) 
            });
        }
        await context.SaveChangesAsync();

        return Redirect("/");
    }
}