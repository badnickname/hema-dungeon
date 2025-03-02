using HemaDungeon.Adapters;
using HemaDungeon.Core.Entities;
using HemaDungeon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon.Controllers;

[ApiController]
[Route("calc/api")]
public sealed class CalculatorController : ControllerBase
{
    [HttpGet("image/{name}")]
    [ResponseCache(Duration = 3600)]
    public async Task<IActionResult> GetImage(string name)
    {
        var file = await System.IO.File.ReadAllBytesAsync($"wwwroot/images/{name}");
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType($"wwwroot/images/{name}", out var contentType)) contentType = "image";
        return File(file, contentType);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetCharacters([FromServices] Context context, string? region)
    {
        if (string.IsNullOrEmpty(region)) region = "NOVOSIBIRSK";
        var result = await context.Users.Include(x => x.Region).Include(x => x.Visits).Include(x => x.Cataclysms).Where(x => x.Region.Id == region && x.VisitToday == true).ToListAsync();
        return new JsonResult(result.Where(x => x.IsDead != true).ToList());
    }
 
    [HttpPost("users/compare")]
    public async Task<CompareResult> Compare(CalculatorCompareModel model, [FromServices] Calculator.Calculator service, [FromServices] FightStateAdapter adapter, [FromServices] Context context)
    {
        var firstState = await GetState(model.FirstUser.Id, context);
        var secondState = await GetState(model.SecondUser.Id, context);
        
        var first = adapter.ToCharacter(firstState, model.FirstUser.Health, model.FirstUser.Spells ?? []);
        var second = adapter.ToCharacter(secondState, model.SecondUser.Health, model.SecondUser.Spells ?? []);

        service.Accept(first, second);

        var result = new CompareResult(adapter.EnrichFromCharacter(firstState, first), adapter.EnrichFromCharacter(secondState, second));
        return result;
    }

    private static async Task<FightState> GetState(string id, Context context)
    {
        return new FightState
        {
            Character = new FightCharacter
            {
                Character = await context.Users
                    .Include(x => x.Visits)
                    .Include(x => x.Cataclysms)
                    .Include(x => x.Tournaments)
                    .FirstAsync(x => x.Id == id)
            }
        };
    }

    [HttpPost("users/calculate")]
    public async Task<CalculateResult> Calculate(CalculatorCompareModel model, [FromServices] Context context, [FromServices] FightStateAdapter adapter, [FromServices] Calculator.Calculator service)
    {
        var firstState = await GetState(model.FirstUser.Id, context);
        var secondState = await GetState(model.SecondUser.Id, context);
        
        var first = adapter.ToCharacter(firstState, model.FirstUser.Health, model.FirstUser.Spells ?? []);
        first.Hits = model.FirstUser.Score ?? 0;
        var second = adapter.ToCharacter(secondState, model.SecondUser.Health, model.SecondUser.Spells ?? []);
        second.Hits = model.SecondUser.Score ?? 0;

        service.Accept(first, second);

        var compare = new CompareResult(adapter.EnrichFromCharacter(firstState, first), adapter.EnrichFromCharacter(secondState, second))
        {
            FirstUser =
            {
                Health = first.HealthAfter,
            },
            SecondUser =
            {
                Health = second.HealthAfter
            }
        };

        return new CalculateResult(compare.FirstUser, compare.SecondUser);
    }

    [HttpGet("regions")]
    public async Task<Region[]> GetRegions([FromServices] Context context, CancellationToken token) => await context.Regions.ToArrayAsync(token);

    public sealed record CompareResult(CalculatorCompareResult FirstUser, CalculatorCompareResult SecondUser);

    public sealed record CalculateResult(CalculatorCompareResult FirstUser, CalculatorCompareResult SecondUser);
}