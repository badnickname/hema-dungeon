namespace HemaDungeon.Calculator.Actives;

internal sealed class Tail : IModificator
{
    public int Priority => 50;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Хвост ящерицы", out var spell)) 
            spell = new Character.Spell("После успешного удара крик идущий от души наносит 100 урона противнику. Не может быть активирован больше 1 раза за поединок. Не может быть активирован не более трех раз за день", 0, "P");
        if (spell.Value > 0 && character.Health > 50)
        {
            character.Health -= 50;
        }
        character.Spells["Хвост ящерицы"] = spell;
    }
}