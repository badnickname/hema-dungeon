namespace HemaDungeon.Calculator.Actives;

internal sealed class Roar : IModificator
{
    public int Priority => int.MaxValue - 22;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Львиный рык", out var spell)) 
            spell = new Character.Spell("После успешного удара крик идущий от души наносит 100 урона противнику. Не может быть активирован больше 1 раза за поединок. Не может быть активирован не более трех раз за день", 0, "P");
        if (spell.Value > 0)
        {
            var damage = 100f;
            for (var i = 0; i < character.TournamentsCount; i++)
                damage *= 1.5f;
            enemy.Health -= damage;
            if (enemy.Health < 0) enemy.Health = 0;
        }
        character.Spells["Львиный рык"] = spell;
    }
}