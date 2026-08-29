namespace Thirty;

public class Player(string name, LockStrategy lockStrategy)
{
    public string Name => name;

    public LockStrategy LockStrategy => lockStrategy;

    public int Points { get; private set; } = 30;

    public bool IsIn => Points > 0;

    public void NewGame()
    {
        Points = 30;
    }

    public (int, int) PlayQualification(List<Die> dice, bool log = false)
    {
        if (log) Console.WriteLine("Qualification");

        UnlockAll(dice);

        while (dice.Any(die => !die.Locked))
        {
            // Roll if unlocked
            foreach (var die in dice) die.RollIfUnlocked();

            // Lock according to lock strategy; if no die locked, lock one with largest number of eyes
            var anyLocked = lockStrategy.Apply(dice);
            if (!anyLocked)
                dice.Where(die => !die.Locked).MaxBy(die => die.Eyes)?.Locked = true;

            if (log)
                Console.WriteLine(
                    string.Join(", ", dice.Select(die => $"{die.Eyes}{(die.Locked ? "L" : "")}")));

            // No need to continue past stop result
            if (dice.Sum(die => die.Eyes) >= lockStrategy.StopAt) break;
        }

        var sum = dice.Sum(die => die.Eyes);

        // Minus points if below 30, 10 bonus if all dice have the same eyes
        var points = Math.Min(0, sum - 30) + (dice.All(die => die == dice[0]) ? 10 : 0);

        return (sum, points);
    }

    public int PlayPenalty(List<Die> dice, int eyes, bool log = false)
    {
        if (log) Console.WriteLine("Penalty");

        UnlockAll(dice);

        while (dice.Any(die => !die.Locked))
        {
            // Roll if unlocked
            foreach (var die in dice) die.RollIfUnlocked();

            // Lock those with the requested number of eyes
            var anyLocked = dice.Count(die => die.LockIfExactly(eyes)) > 0;

            if (log)
                Console.WriteLine(
                    string.Join(", ", dice.Select(die => $"{die.Eyes}{(die.Locked ? "L" : "")}")));

            // If no new die locked, exit the loop
            if (!anyLocked) break;
        }

        // Count sum of dice with the requested number of eyes; if all dice have the requested number of eyes, double the count
        var count = dice.Count(die => die.Eyes == eyes);
        return eyes * count * (count == dice.Count ? 2 : 1);
    }

    public int Play(List<Die> dice, int penalty, bool log = false)
    {
        Points -= penalty;
        if (Points <= 0)
        {
            // Return remaining points to be deducted from the next player
            var remaining = -Points;
            Points = 0;
            return remaining;
        }

        var (sum, points) = PlayQualification(dice, log);
        Points = Math.Max(Points + points, 0);

        if (log) Console.WriteLine($"Sum: {sum}, Points: {Points}");

        return Points > 0 && sum > 30 ? PlayPenalty(dice, sum - 30, log) : 0;
    }

    private static void UnlockAll(List<Die> dice)
    {
        // Unlock all
        foreach (var die in dice) die.Locked = false;
    }
}