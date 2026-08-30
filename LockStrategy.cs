namespace Thirty;

public class LockStrategy(Dictionary<int, int> minEyesForNumberUnlocked, int stopAt, bool reEvaluate)
{
    public LockStrategy(int minEyes) : this(Enumerable.Range(1, 6).ToDictionary(i => i, _ => minEyes), 30, false)
    {
    }

    public int StopAt { get; } = stopAt;

    public bool Apply(List<Die> dice)
    {
        var anyLocked = false;

        int count;
        do
        {
            // Get min eyes for the number of unlocked dice
            if (!minEyesForNumberUnlocked.TryGetValue(dice.Count(die => !die.Locked), out var minEyes))
                return anyLocked;

            if ((count = dice.Count(die => die.LockIfAtLeast(minEyes))) > 0) anyLocked = true;
        } while (reEvaluate && count > 0);

        return anyLocked;
    }

    public override string ToString()
    {
        return $"{string.Join(",", minEyesForNumberUnlocked.Values)},{StopAt},{reEvaluate}";
    }
}
