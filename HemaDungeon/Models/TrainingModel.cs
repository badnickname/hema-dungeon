namespace HemaDungeon.Models;

public sealed class TrainingModel
{
    public string DateTime { get; set; }

    public ICollection<UserData>? Users { get; set; }

    [Serializable]
    public sealed record UserData(string Id, bool? WasHere, bool? Skip, int Damage);
}