using HemaDungeon.Adapters;
using HemaDungeon.Core.Entities;
using HemaDungeon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon.Controllers;

[ApiController]
[Route("api/fight")]
public sealed class FightController : ControllerBase
{
    [HttpGet("users")]
    [Authorize]
    public async Task<IActionResult> GetUser([FromServices] Context context, [FromServices] UserManager<Character> manager)
    {
        var users = await context.FightCharacters.Include(x => x.Character).ThenInclude(x => x.Visits).ToListAsync();
        return new JsonResult(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("users")]
    [Authorize]
    public async Task<IActionResult> PostUser([FromForm] FightUsersModel model, [FromServices] UserManager<Character> manager, [FromServices] Context context)
    {
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id;

        context.FightCharacters.RemoveRange(context.FightCharacters.Where(x => x.AuthorId == userId).ToList());
        await context.SaveChangesAsync();

        foreach (var user in context.Users.Include(x => x.Visits).Include(x => x.Tournaments).Include(x => x.Cataclysms).Where(x => model.Ids.Contains(x.Id)))
        {
            context.FightCharacters.Add(new FightCharacter
            {
                Id = Guid.NewGuid().ToString(),
                Character = user,
                AuthorId = userId,
                Health = user.Vitality
            });
        }
        await context.SaveChangesAsync();

        return Redirect("/?dashboard=true");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("complete")]
    [Authorize]
    public async Task<IActionResult> Complete([FromServices] Context context, [FromServices] UserManager<Character> manager)
    {
        context.FightStates.RemoveRange(context.FightStates.ToList());
        context.FightCharacters.RemoveRange(context.FightCharacters.ToList());
        await context.SaveChangesAsync();
        return Redirect("/");
    }

    [HttpPost("state/complete")]
    [Authorize]
    public async Task<IActionResult> CompleteState([FromForm] FightStateModel model, [FromServices] Context context, [FromServices] UserManager<Character> manager)
    {
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id;
        model.Score ??= [0, 0];
        model.Result ??= [0, 0];
        model.Damage ??= [0, 0];

        var states = await context.FightStates.Where(x => x.AuthorId == userId).Include(x => x.Character).ThenInclude(x => x.Character).ToListAsync();
        states[0].Character.Health -= states[1].Damage * ((model.Score[1] ?? 0) + (model.Result[1] ?? 0)) + (model.Damage?[1] ?? 0);
        if (states[0].Character.Health < 0) states[0].Character.Health = 0;

        states[1].Character.Health -= states[0].Damage * ((model.Score[0] ?? 0) + (model.Result[0] ?? 0)) + (model.Damage?[0] ?? 0);
        if (states[1].Character.Health < 0) states[1].Character.Health = 0;
        await context.SaveChangesAsync();

        context.FightStates.RemoveRange(states);

        context.Results.Add(new Result
        {
            Id = Guid.NewGuid().ToString(),
            CreateDate = DateTime.UtcNow,
            DateTime = DateTime.UtcNow.Date,
            First = states[0].Character.Character,
            Second = states[1].Character.Character,
            FirstScore = model.Result[0].Value + model.Score[0].Value * 2,
            SecondScore = model.Result[1].Value + model.Score[1].Value * 2
        });
        await context.SaveChangesAsync();

        return Redirect("/?dashboard=true");
    }

    [HttpGet("results")]
    [Authorize]
    public async Task<IActionResult> GetResultsToday([FromServices] Context context)
    {
        var date = DateTime.UtcNow.Date;
        var results = context.Results.OrderBy(x => x.CreateDate).Include(x => x.First).Include(x => x.Second).Where(x => x.DateTime == date).ToList();
        return new JsonResult(results);
    }

    [HttpPost("state")]
    [Authorize]
    public async Task<IActionResult> PostState([FromForm] FightUsersModel model, [FromServices] Calculator.Calculator service, [FromServices] FightStateAdapter adapter, [FromServices] UserManager<Character> manager, [FromServices] Context context)
    {
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id;

        context.FightStates.RemoveRange(context.FightStates.ToList());
        await context.SaveChangesAsync();

        var states = new List<FightState>();
        foreach (var user in context.FightCharacters
                     .Include(x => x.Character)
                     .Include(x => x.Character)
                     .ThenInclude(x => x.Visits)
                     .Include(x => x.Character)
                     .Include(x => x.Character)
                     .ThenInclude(x => x.Tournaments)
                     .Include(x => x.Character)
                     .Include(x => x.Character)
                     .ThenInclude(x => x.Cataclysms)
                     .Where(x => model.Ids.Contains(x.Character.Id)))
        {
            var state = new FightState
            {
                Id = Guid.NewGuid().ToString(),
                Character = user,
                AuthorId = userId,
            };
            states.Add(state);

            context.FightStates.Add(state);
        }

        var first = adapter.ToCharacter(states[0], null, null);
        var second = adapter.ToCharacter(states[1], null, null);

        service.Accept(first, second);

        adapter.EnrichFromCharacter(states[0], first);
        adapter.EnrichFromCharacter(states[1], second);

        await context.SaveChangesAsync();

        return Redirect("/?dashboard=true");
    }

    [HttpGet("state")]
    [Authorize]
    public async Task<IActionResult> GetState([FromServices] Context context, [FromServices] UserManager<Character> manager)
    {
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id;

        var states = await context.FightStates.Where(x => x.AuthorId == userId).Include(x => x.Character).ThenInclude(x => x.Character).ThenInclude(x => x.Visits).ToListAsync();
        return new JsonResult(states);
    }
}