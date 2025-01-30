namespace HemaDungeon.Core.Entities;

public sealed class DeadCharacter
{
    public string Name { get; set; }

    public string Avatar { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public string Story { get; set; }

    public string Author { get; set; }

    // Physics
    public int PushUp { get; set; }

    public int PullUp { get; set; }

    public int Abdominal { get; set; }

    public float RunTwenty { get; set; }

    public float RunFifteen { get; set; }

    public int Rang { get; set; }

    public int Score { get; set; }

    public int Rope { get; set; }

    public bool? IsDead { get; set; }
}