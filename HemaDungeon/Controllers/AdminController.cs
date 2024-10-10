using HemaDungeon.Entities;
using HemaDungeon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon.Controllers;

[ApiController]
[Route("api/admin")]
public sealed class AdminController : ControllerBase
{
    [HttpGet("visits")]
    public async Task<IActionResult> GetVisits([FromServices] Context context)
    {
        var visits = await context.Visits.OrderBy(x =>x.Date).Include(x => x.Character).ToListAsync();
        var result = visits
            .GroupBy(x => x.Date)
            .ToDictionary(x => $"{x.Key!.Value.Year}-{x.Key!.Value.Month}-{x.Key!.Value.Day}", x => x.ToList());
        return new JsonResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("visits")]
    public async Task<IActionResult> SaveVisits([FromForm] VisitModel model, [FromServices] UserManager<Character> manager, [FromServices] Context context)
    {
        var date = model.DateTime.Date;
        model.SkipIds ??= [];
        model.Ids ??= [];

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
                WasHere = model.Ids.Contains(user.Id),
                CanSkip = model.SkipIds.Contains(user.Id)
            });
        }
        await context.SaveChangesAsync();
        
        // Сражения
        if (context.FightCharacters.Any()) return Redirect("/");
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id!;

        context.FightCharacters.RemoveRange(context.FightCharacters.Where(x => x.AuthorId == userId).ToList());
        await context.SaveChangesAsync();

        foreach (var user in context.Users.Include(x => x.Visits).AsEnumerable().Where(x => model.Ids.Contains(x.Id)))
        {
            context.FightCharacters.Add(new FightCharacter
            {
                Id = Guid.NewGuid().ToString(),
                Character = user,
                AuthorId = userId,
                Health = user.Vitality,
            });
        }
        await context.SaveChangesAsync();

        return Redirect("/");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("visits")]
    public async Task<IActionResult> DeleteVisit([FromForm] RemoveVisitModel model, [FromServices] Context context)
    {
        var date = model.DateTime.Date;

        var visits = await context.Visits.Where(x => x.Date == date).ToListAsync();
        context.Visits.RemoveRange(visits);
        await context.SaveChangesAsync();

        return Redirect("/?dashboard=true");
    }

    [Authorize]
    [HttpGet("role")]
    public async Task<IActionResult> CheckIsAdmin([FromServices] UserManager<Character> manager)
    {
        var user = await manager.GetUserAsync(HttpContext.User);
        return new JsonResult(await manager.IsInRoleAsync(user!, "Admin"));
    }
}