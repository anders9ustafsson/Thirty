namespace Thirty;

public class LockStrategy(Dictionary<int, int> minEyesForNumberUnlocked, int stopAt)
{
    public LockStrategy(int minEyes) : this(Enumerable.Range(1, 6).ToDictionary(i => i, _ => minEyes), 30)
    {
    }

    public int StopAt { get; } = stopAt;

    public bool Apply(List<Die> dice)
    {
        // Get min eyes for the number of unlocked dice
        if (!minEyesForNumberUnlocked.TryGetValue(dice.Count(die => !die.Locked), out var minEyes)) return false;

        var anyLocked = false;
        foreach (var die in dice)
            if (die.LockIfAtLeast(minEyes))
                anyLocked = true;
        return anyLocked;
    }

    public override string ToString()
    {
        return $"{string.Join(",", minEyesForNumberUnlocked.Values)},{StopAt}";
    }
}
