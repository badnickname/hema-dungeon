namespace HemaDungeon.Calculator.Passives;

internal sealed class GodDefence : IModificator
{
    public int Priority => int.MaxValue - 10;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Защита богов", out var spell)) 
            spell = new Character.Spell("Снижает получаемый урон на 80% с участниками не достигшими 3 ранга", 1, "P");
        if (spell.Value > 0 && enemy.Rang > 3)
        {
            enemy.Damage *= 0.2f;
        }
        character.Spells["Защита богов"] = spell;
    }
}