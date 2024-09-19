using HemaDungeon.Entities;
using HemaDungeon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon.Controllers;

[ApiController]
[Route("api/pages")]
public sealed class PageController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromForm] PageModel model, [FromServices] Context context, [FromServices] UserManager<Character> manager)
    {
        var userId = manager.GetUserId(HttpContext.User)!;
        var user = context.Users.Include(x => x.Pages).First(x => x.Id == userId);
        
        var page = user.Pages.FirstOrDefault(x => x.Id == model.Id) ?? context.Add(CreatePage(user)).Entity;
        page.Description = model.Description ?? string.Empty;
        page.Name = model.Name ?? string.Empty;
        await context.SaveChangesAsync();

        return Redirect("/");
    }

    private Page CreatePage(Character character)
    {
        var count = character.Pages.Count;
        var page = new Page { Number = count };
        character.Pages.Add(page);
        return page;
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Remove([FromForm] PageModel model, [FromServices] Context context, [FromServices] UserManager<Character> manager)
    {
        var userId = manager.GetUserId(HttpContext.User)!;
        var user = context.Users.Include(x => x.Pages).First(x => x.Id == userId);

        var page = user.Pages.First(x => x.Id == model.Id);
        context.Remove(page);
        await context.SaveChangesAsync();

        return Redirect("/");
    }
}