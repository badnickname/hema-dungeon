namespace HemaDungeon.Models;

public sealed class CalculatorCompareModel
{
    public User FirstUser { get; set; }

    public User SecondUser { get; set; }
 
    public sealed record User(string Id, int? Health, int? Score, int? Damage, bool? DisableAbility);
}