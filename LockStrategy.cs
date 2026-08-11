using Dice;

public class LockStrategy
{
    private readonly int _minEyes;

    public LockStrategy(int minEyes)
    {
        _minEyes = minEyes;
    }

    public bool Apply(List<Die> dice)
    {
        var anyLocked = false;
        foreach (var die in dice)
            if (die.LockIf(_minEyes)) anyLocked = true;
        return anyLocked;
    }
}