using Thirty;

const int numberOfRounds = 100000;
const int stopAt = 30;

var lockStrategy = new LockStrategy(new Dictionary<int, int>{{6, 6}, {5, 6}, {4, 6}, {3, 6}, {2, 5}, {1, 4}});
var win = 0;

// Create list of six die
var dice = Enumerable.Range(0, 6).Select(_ => new Die()).ToList();

for (var i = 0; i < numberOfRounds; i++)
{
    var sum = Round(dice, lockStrategy, stopAt);
    if (sum >= stopAt) win++;
}

Console.WriteLine($"Strategy wins: {win} ({(double)win / numberOfRounds * 100.0}%)");

return;

static int Round(List<Die> dice, LockStrategy lockStrategy, int stopAt)
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
        if (dice.Sum(die => die.Eyes) >= stopAt) break;
    }

    var sum = dice.Sum(die => die.Eyes);
    return sum;
}
