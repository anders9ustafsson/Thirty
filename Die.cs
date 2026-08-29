namespace Thirty;

public class Die
{
    public Die()
    {
        Roll();
    }

    public int Eyes { get; private set; }

    public bool Locked { get; set;}
    
    public void Roll()
    {
        Eyes = Random.Shared.Next(1, 7);
    }

    public void RollIfUnlocked()
    {
        if (Locked) return;
        Roll();
    }

    public bool LockIfAtLeast(int minEyes)
    {
        if (Locked || Eyes < minEyes) return false;
        Locked = true;
        return true;
    }

    public bool LockIfExactly(int eyes)
    {
        if (Locked || Eyes != eyes) return false;
        Locked = true;
        return true;
    }

    public static List<Die> NewDice()
    {
        return [.. Enumerable.Range(0, 6).Select(_ => new Die())];
    }
}
