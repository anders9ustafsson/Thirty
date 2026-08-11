namespace Dice;

public class Die
{
    private int _eyes;

    public Die()
    {
        Roll();
    }

    public int Eyes => _eyes;
    
    public bool Locked { get; set;}
    
    public void Roll()
    {
        _eyes = Random.Shared.Next(1, 7);
    }

    public void RollIfUnlocked()
    {
        if (Locked) return;
        Roll();
    }

    public bool LockIf(int minEyes)
    {
        if (Locked || _eyes < minEyes) return false;
        Locked = true;
        return true;
    }
}
