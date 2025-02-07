namespace HemaDungeon.Calculator.Actives;

public sealed class AirShield : IModificator
{
    public int Priority => int.MaxValue - 13;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Щит ветра", out var spell)) 
            spell = new Character.Spell("Жертвует 20% имеющихся хп, и на 1 бой уменьшаяет получаемый по себе урон на 50%. Вслучае победы в бою пожертвованные хп возвращаются", 0, "P");
        if (spell.Value > 0)
        {
            character.Health *= 0.8;
            enemy.Damage *= 0.5;
        }
        character.Spells["Щит ветра"] = spell;
    }
}