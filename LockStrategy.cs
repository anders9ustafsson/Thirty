namespace Thirty;

public class LockStrategy
{
    private readonly Dictionary<int, int> _minEyesForNumberUnlocked;

    public LockStrategy(int minEyes)
    {
        _minEyesForNumberUnlocked = Enumerable.Range(1, 6).ToDictionary(i => i, _ => minEyes);
    }

    public LockStrategy(Dictionary<int, int> minEyesForNumberUnlocked)
    {
        _minEyesForNumberUnlocked = minEyesForNumberUnlocked;
    }

    public bool Apply(List<Die> dice)
    {
        if (!_minEyesForNumberUnlocked.TryGetValue(dice.Count(die => !die.Locked), out var minEyes)) return false;

        var anyLocked = false;
        foreach (var die in dice)
            if (die.LockIf(minEyes))
                anyLocked = true;
        return anyLocked;
    }
}
