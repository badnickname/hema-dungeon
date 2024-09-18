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
    [Authorize(Roles = "Admin")]
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
    public async Task<IActionResult> SaveVisits([FromForm] VisitModel model, [FromServices] Context context)
    {
        var date = model.DateTime.Date;
        model.SkipNames ??= [];
        model.Names ??= [];

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
                WasHere = model.Names.Contains(user.Name),
                CanSkip = model.SkipNames.Contains(user.Name)
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

        return Redirect("/");
    }

    [Authorize]
    [HttpGet("role")]
    public async Task<IActionResult> CheckIsAdmin([FromServices] UserManager<Character> manager)
    {
        var user = await manager.GetUserAsync(HttpContext.User);
        return new JsonResult(await manager.IsInRoleAsync(user!, "Admin"));
    }
}