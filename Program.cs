using Thirty;

const int numberOfRounds = 1000000;
const int stopAt = 30;
const int strategyMinEyes = 4;

// Loop over relevant lock strategies
for (var d6 = 6; d6 >= strategyMinEyes; --d6)
for (var d5 = d6; d5 >= strategyMinEyes; --d5)
for (var d4 = d5; d4 >= strategyMinEyes; --d4)
for (var d3 = d4; d3 >= strategyMinEyes; --d3)
for (var d2 = d3; d2 >= strategyMinEyes; --d2)
for (var d1 = d2; d1 >= strategyMinEyes; --d1)
{
    var lockStrategy = new LockStrategy(new Dictionary<int, int>
        { { 6, d6 }, { 5, d5 }, { 4, d4 }, { 3, d3 }, { 2, d2 }, { 1, d1 } });
    var win = 0;

    // Create list of six die
    var dice = Enumerable.Range(0, 6).Select(_ => new Die()).ToList();

    for (var i = 0; i < numberOfRounds; i++)
    {
        var sum = Round(dice, lockStrategy, stopAt);
        if (sum >= stopAt) win++;
    }

    Console.WriteLine($"Strategy {d6},{d5},{d4},{d3},{d2},{d1} wins {(double)win / numberOfRounds * 100.0:F2}%");
}

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
