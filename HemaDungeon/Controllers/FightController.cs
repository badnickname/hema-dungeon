using HemaDungeon.Entities;
using HemaDungeon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon.Controllers;

[ApiController]
[Route("api/fight")]
public sealed class FightController : ControllerBase
{
    [HttpGet("users")]
    [Authorize]
    public async Task<IActionResult> GetUser([FromServices] Context context)
    {
        var users = await context.FightCharacters.Include(x => x.Character).ToListAsync();
        return new JsonResult(users);
    }

    [HttpPost("users")]
    [Authorize]
    public async Task<IActionResult> PostUser([FromForm] FightUsersModel model, [FromServices] Context context)
    {
        context.FightCharacters.RemoveRange(context.FightCharacters.ToList());
        await context.SaveChangesAsync();

        foreach (var user in context.Users.Where(x => model.Names.Contains(x.Name)))
        {
            context.FightCharacters.Add(new FightCharacter
            {
                Id = Guid.NewGuid().ToString(),
                Character = user,
                Health = user.Vitality
            });
        }
        await context.SaveChangesAsync();

        return Redirect("/");
    }

    [HttpPost("complete")]
    [Authorize]
    public async Task<IActionResult> Complete([FromServices] Context context)
    {
        context.FightStates.RemoveRange(context.FightStates.ToList());
        context.FightCharacters.RemoveRange(context.FightCharacters.ToList());
        await context.SaveChangesAsync();
        return Redirect("/");
    }

    [HttpPost("state/complete")]
    [Authorize]
    public async Task<IActionResult> CompleteState([FromForm] FightStateModel model, [FromServices] Context context)
    {
        var states = await context.FightStates.Include(x => x.Character).ThenInclude(x => x.Character).ToListAsync();
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
    public async Task<IActionResult> PostState([FromForm] FightUsersModel model, [FromServices] Context context)
    {
        context.FightStates.RemoveRange(context.FightStates.ToList());
        await context.SaveChangesAsync();

        var states = new List<FightState>();
        foreach (var user in context.FightCharacters.Include(x => x.Character).Include(x => x.Character).Where(x => model.Names.Contains(x.Character.Name)))
        {
            var state = new FightState
            {
                Id = Guid.NewGuid().ToString(),
                Character = user,
            };
            states.Add(state);

            context.FightStates.Add(state);
        }

        states[0].Damage =
            Math.Max(states[0].Character.Character.Agility - states[1].Character.Character.Agility, 1) +
            Math.Max(states[0].Character.Character.Power - states[1].Character.Character.Power, 1) +
            Math.Max(states[0].Character.Character.Wisdom - states[1].Character.Character.Wisdom, 1) +
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
        states[0].Damage = (int) states[0].Damage;

        states[1].Damage =
            Math.Max(states[1].Character.Character.Agility - states[0].Character.Character.Agility, 1) +
            Math.Max(states[1].Character.Character.Power - states[0].Character.Character.Power, 1) +
            Math.Max(states[1].Character.Character.Wisdom - states[0].Character.Character.Wisdom, 1) +
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
        states[1].Damage = (int) states[1].Damage;

        states[0].ScoreHealth = (int) Math.Ceiling(states[0].Character.Health / states[1].Damage);
        if (states[0].ScoreHealth == 0) states[0].ScoreHealth = 1;

        states[1].ScoreHealth = (int) Math.Ceiling(states[1].Character.Health / states[0].Damage);
        if (states[1].ScoreHealth == 0) states[1].ScoreHealth = 1;

        await context.SaveChangesAsync();

        return Redirect("/");
    }

    [HttpGet("state")]
    [Authorize]
    public async Task<IActionResult> GetState([FromServices] Context context)
    {
        var states = await context.FightStates.Include(x => x.Character).ThenInclude(x => x.Character).ToListAsync();
        return new JsonResult(states);
    }
}