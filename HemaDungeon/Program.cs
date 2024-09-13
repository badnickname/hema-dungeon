using HemaDungeon;
using HemaDungeon.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<Context>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("default")));
builder.Services.AddIdentity<Character, IdentityRole>(options =>
{
    options.Password = new PasswordOptions
    {
        RequireDigit = false,
        RequireLowercase = false,
        RequireUppercase = false,
        RequiredLength = 3,
        RequireNonAlphanumeric = false,
        RequiredUniqueChars = 1,
    };
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = null;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<Context>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("login", (HttpContext context) => context.Response.Redirect("/"));
app.MapGet("dashboard", (HttpContext context) => context.Response.Redirect("/"));
app.MapGet("register", (HttpContext context) => context.Response.Redirect("/"));
app.MapGet("fight", (HttpContext context) => context.Response.Redirect("/"));

app.Run();
