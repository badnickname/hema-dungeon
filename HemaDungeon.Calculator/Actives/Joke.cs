namespace HemaDungeon.Calculator.Actives;

internal sealed class Joke : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Держите анек", out var spell)) 
            spell = new Character.Spell("Во время тренировки может рассказать анекдот, пока другие участники стоят в планке. Добавляет 50 хп на 1 день. Можно применять не более 1 раза в день. Не срабатывает, если анекдот не смешной", 0, "P");
        if (spell.Value > 0)
        {
            var health = 50f;
            for (var i = 0; i < character.TournamentsCount; i++)
                health *= 1.5f;
            character.Health += health;
        }
        character.Spells["Держите анек"] = spell;
    }
}