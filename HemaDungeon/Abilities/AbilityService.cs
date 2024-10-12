using HemaDungeon.Entities;

namespace HemaDungeon.Abilities;

public sealed class AbilityService
{
    public Buff Accept(FightState state, FightState second)
    {
        switch (state.Character.Character.Ability)
        {
            case AbilityType.GodDefence:
            {
                if (second.Character.Character.Rang > 3) return new Buff
                {
                    Name = "Защита богов",
                    Description = "Снижает получаемый урон на 80% с участниками не достигшими 3 ранга",
                    ResistFactor = 0.2f,
                    Calculated = true
                };
                return new Buff
                {
                    Name = "Защита богов",
                    Description = "Снижает получаемый урон на 80% с участниками не достигшими 3 ранга",
                    Calculated = true
                };
            }
            case AbilityType.Food:
            {
                if (second.Character.Character.Stamina < state.Character.Character.Stamina) 
                    return new Buff
                    {
                        Name = "Корм энергичных кошек",
                        Description = "Если выносливость Кота Бориса больше, чем выносливость противника, добавь +10 к урону",
                        Damage = 10, 
                        Calculated = true
                    };
                return new Buff
                    {
                        Name = "Корм энергичных кошек",
                        Description =
                            "Если выносливость Кота Бориса больше, чем выносливость противника, добавь +10 к урону",
                        Calculated = true
                    };
            }
            case AbilityType.Air:
            {
                return new Buff
                {
                    Name = "Маг воздуха",
                    Description = "Ловкость увеличена на 10",
                    Agility = 10,
                    Calculated = true
                };
            }
            case AbilityType.Armor:
            {
                return new Buff
                {
                    Name = "Стальная броня",
                    Description = "Снижает получаемый урон на 20%. На механизмы не действуют яды",
                    ResistFactor = 0.8f, 
                    Calculated = true
                };
            }
            case AbilityType.Mimic:
            {
                return new Buff
                {
                    Name = "Мимик",
                    Description = "Копирует статы противника, все кроме лиги",
                    CopyStats = true, 
                    Calculated = true
                };
            }
            case AbilityType.Wisdom:
            {
                return new Buff
                {
                    Name = "Мудрость предков",
                    Description = "Мудрость увеличена на 10",
                    Wisdom = 10, 
                    Calculated = true
                };
            }

            case AbilityType.Joke:
                return new Buff
                {
                    Name = "Держите анек",
                    Description = "Во время тренировки может рассказать анекдот, пока другие участники стоят в планке. Добавляет 50 хп на 1 день. Можно применять не более 1 раза в день. Не срабатывает, если анекдот не смешной."
                };
            case AbilityType.Tail:
                return new Buff
                {
                    Name = "Хвост ящерицы",
                    Description = "Отбрасывает хвост, теряет 50 хп, но не умирает в бою (если есть жизни). Действует не более раза в неделю."
                };
            case AbilityType.Roar:
                return new Buff
                {
                    Name = "Львиный рык",
                    Description = "После успешного удара крик идущий от души наносит 100 урона противнику. Не может быть активирован больше 1 раза за поединок. Не может быть активирован не более трех раз за день"
                };
            case AbilityType.Klukalo:
                return new Buff
                {
                    Name = "Клюкало",
                    Description = "Может достать клюкало и бросить в воду. Пока противники находятся в стане может нанести урон (на 1 балл) противнику ниже или равному ему по рангу или сбежать от высокорангового противника. На изготовление клюкало требуется неделя"
                };
            case AbilityType.Worm:
                return new Buff
                {
                    Name = "Книжный червь",
                    Description = "Не наносит урон, если не показал заполненный спортивный дневник (на этой неделе)"
                };
            case AbilityType.Poison:
                return new Buff
                {
                    Name = "Ядовитое касание",
                    Description = "Три любых попадания за бой убивают противника"
                };
            case AbilityType.Calm:
                return new Buff
                {
                    Name = "Спокойствие самурая",
                    Description = "Начинает поединок с 30 секундной медитации. Медитация выполняется сидя на коленях с закрытыми глазами. После чего выполняет поклон и выходит на площадку"
                };
            case AbilityType.Gnome:
                return new Buff
                {
                    Name = "Гномьи технологии",
                    Description = "Оружие, на 10 см ниже максимальной допустимой длины наносит дополнительно 10 ед. урона при попадании"
                };
            case AbilityType.DungeonMaster:
            {
                if (second.Character.Character.Rang > 3) return new Buff
                {
                    Name = "Мастер",
                    Description = "Снижает получаемый урон на 80% с участниками не достигшими 3 ранга. Увеличивает статы в 2 раза",
                    ResistFactor = 0.2f,
                    StatesFactor = 2,
                    Calculated = true
                };
                return new Buff
                {
                    Name = "Мастер",
                    Description = "Снижает получаемый урон на 80% с участниками не достигшими 3 ранга. Увеличивает статы в 2 раза",
                    StatesFactor = 2,
                    Calculated = true
                };
            }
        }

        return new Buff();
    }
}