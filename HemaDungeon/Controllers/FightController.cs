using HemaDungeon.Abilities;
using HemaDungeon.Entities;
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
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id;
        var users = await context.FightCharacters.Where(x => x.AuthorId == userId).Include(x => x.Character).ThenInclude(x => x.Visits).ToListAsync();
        return new JsonResult(users);
    }

    [HttpPost("users")]
    [Authorize]
    public async Task<IActionResult> PostUser([FromForm] FightUsersModel model, [FromServices] UserManager<Character> manager, [FromServices] Context context)
    {
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id;

        context.FightCharacters.RemoveRange(context.FightCharacters.Where(x => x.AuthorId == userId).ToList());
        await context.SaveChangesAsync();

        foreach (var user in context.Users.Where(x => model.Ids.Contains(x.Id)))
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

        return Redirect("/");
    }

    [HttpPost("complete")]
    [Authorize]
    public async Task<IActionResult> Complete([FromServices] Context context, [FromServices] UserManager<Character> manager)
    {
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id;

        context.FightStates.RemoveRange(context.FightStates.Where(x => x.AuthorId == userId).ToList());
        context.FightCharacters.RemoveRange(context.FightCharacters.Where(x => x.AuthorId == userId).ToList());
        await context.SaveChangesAsync();
        return Redirect("/");
    }

    [HttpPost("state/complete")]
    [Authorize]
    public async Task<IActionResult> CompleteState([FromForm] FightStateModel model, [FromServices] Context context, [FromServices] UserManager<Character> manager)
    {
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id;
        model.Score ??= [0, 0];

        var states = await context.FightStates.Where(x => x.AuthorId == userId).Include(x => x.Character).ThenInclude(x => x.Character).ToListAsync();
        states[0].Character.Health -= states[1].Damage * (model.Score[1] ?? 0);
        if (states[0].Character.Health < 0) states[0].Character.Health = 0;

        states[1].Character.Health -= states[0].Damage * (model.Score[0] ?? 0);
        if (states[1].Character.Health < 0) states[1].Character.Health = 0;
        await context.SaveChangesAsync();

        context.FightStates.RemoveRange(states);
        await context.SaveChangesAsync();

        return Redirect("/");
    }

    [HttpPost("state")]
    [Authorize]
    public async Task<IActionResult> PostState([FromForm] FightUsersModel model, [FromServices] AbilityService service, [FromServices] UserManager<Character> manager, [FromServices] Context context)
    {
        var userId = (await manager.GetUserAsync(HttpContext.User))?.Id;

        context.FightStates.RemoveRange(context.FightStates.Where(x => x.AuthorId == userId).ToList());
        await context.SaveChangesAsync();

        var states = new List<FightState>();
        foreach (var user in context.FightCharacters.Where(x => x.AuthorId == userId).Include(x => x.Character).Include(x => x.Character).ThenInclude(x => x.Visits).Where(x => model.Ids.Contains(x.Character.Id)))
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

        var buff0 = service.Accept(states[0], states[1]);
        var buff1 = service.Accept(states[1], states[0]);

        states[0].Damage =
            Math.Max(states[0].Character.Character.Agility + buff0.Agility - states[1].Character.Character.Agility - buff1.Agility, 1) +
            Math.Max(states[0].Character.Character.Power - states[1].Character.Character.Power, 1) +
            Math.Max(states[0].Character.Character.Wisdom + buff0.Wisdom - states[1].Character.Character.Wisdom - buff1.Wisdom, 1) +
            Math.Max(states[0].Character.Character.Stamina - states[1].Character.Character.Stamina, 1);
        states[0].Damage *= 5;
        if (states[0].Character.Character.Rang > states[1].Character.Character.Rang)
        {
            states[0].Damage *= (states[0].Character.Character.Rang - states[1].Character.Character.Rang + 1);
        }
        else if (states[0].Character.Character.Rang < states[1].Character.Character.Rang)
        {
            states[0].Damage /= (states[1].Character.Character.Rang - states[0].Character.Character.Rang + 1);
        }

        states[1].Damage =
            Math.Max(states[1].Character.Character.Agility + buff1.Agility - states[0].Character.Character.Agility - buff0.Agility, 1) +
            Math.Max(states[1].Character.Character.Power - states[0].Character.Character.Power, 1) +
            Math.Max(states[1].Character.Character.Wisdom + buff1.Wisdom - states[0].Character.Character.Wisdom - buff0.Wisdom, 1) +
            Math.Max(states[1].Character.Character.Stamina - states[0].Character.Character.Stamina, 1);
        states[1].Damage *= 5;
        if (states[1].Character.Character.Rang > states[0].Character.Character.Rang)
        {
            states[1].Damage *= (states[1].Character.Character.Rang - states[0].Character.Character.Rang);
        }
        else if (states[1].Character.Character.Rang < states[0].Character.Character.Rang)
        {
            states[1].Damage /= (states[0].Character.Character.Rang - states[1].Character.Character.Rang);
        }

        if (buff0.CopyStats || buff1.CopyStats)
        {
            states[0].Damage = 20;
            states[1].Damage = 20;
        }
        states[1].Damage = (int) (states[1].Damage * (buff0.ResistFactor > 0 ? buff0.ResistFactor : 1)) + buff1.Damage;
        states[0].Damage = (int) (states[0].Damage * (buff1.ResistFactor > 0 ? buff1.ResistFactor : 1)) + buff0.Damage;

        states[0].Calculated = buff0.Calculated;
        states[0].Name = buff0.Name ?? string.Empty;
        states[0].Description = buff0.Description ?? string.Empty;
        states[1].Calculated = buff1.Calculated;
        states[1].Name = buff1.Name ?? string.Empty;
        states[1].Description = buff1.Description ?? string.Empty;

        states[0].ScoreHealth = (int) Math.Ceiling(states[0].Character.Health / states[1].Damage);
        if (states[0].ScoreHealth == 0) states[0].ScoreHealth = 1;

        states[1].ScoreHealth = (int) Math.Ceiling(states[1].Character.Health / states[0].Damage);
        if (states[1].ScoreHealth == 0) states[1].ScoreHealth = 1;

        await context.SaveChangesAsync();

        return Redirect("/");
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