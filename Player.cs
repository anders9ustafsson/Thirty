namespace Thirty;

public class Player(LockStrategy lockStrategy)
{
    public int Play(List<Die> dice)
    {
        // Unlock all
        foreach (var die in dice) die.Locked = false;

        while (dice.Any(die => !die.Locked))
        {
            // Roll if unlocked
            foreach (var die in dice) die.RollIfUnlocked();

            // Lock according to lock strategy; if no die locked, lock one with largest number of eyes
            var anyLocked = lockStrategy.Apply(dice);
            if (!anyLocked)
                dice.Where(die => !die.Locked).MaxBy(die => die.Eyes)?.Locked = true;
            /*Console.WriteLine(
                string.Join(", ", dice.Select(die => $"{die.Eyes}{(die.Locked ? "L" : "")}")));*/

            // No need to continue past stop result
            if (dice.Sum(die => die.Eyes) >= lockStrategy.StopAt) break;
        }

        var sum = dice.Sum(die => die.Eyes);
        return sum;
    }

}