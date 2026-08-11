using Dice;

const int numberOfRounds = 10000000;
var lockStrategy1 = new LockStrategy(minEyes: 5);
var lockStragegy2 = new LockStrategy(minEyes: 6);
//var counts = new int[37];
var win1 = 0;
var win2 = 0;

var dice = Enumerable.Range(0, 6).Select(_ => new Die()).ToList();

for (int i = 0; i < numberOfRounds; i++)
{
    var sum1 = Round(dice, lockStrategy1);
    var sum2 = Round(dice, lockStragegy2);
    if (sum1 > sum2) win1++;
    else if (sum2 > sum1) win2++;
    //counts[sum]++;
}

Console.WriteLine($"Strategy 1 wins: {win1} ({(double)win1 / numberOfRounds * 100.0}%)");
Console.WriteLine($"Strategy 2 wins: {win2} ({(double)win2 / numberOfRounds * 100.0}%)");
/*for (var i = 6; i < counts.Length; ++i)
    Console.WriteLine($"{i},{counts[i] / (double)numberOfRounds * 100.0}");

var average = counts.Select((count, sum) => (double)count * sum).Sum() / (double)numberOfRounds;
Console.WriteLine($"Average: {average}");*/

return;

static int Round(List<Die> dice, LockStrategy lockStrategy)
{
    UnlockAll(dice);
    while (dice.Any(die => !die.Locked))
    {
        RollUnlocked(dice);
        var anyLocked = lockStrategy.Apply(dice);
        if (!anyLocked)
            dice.Where(die => !die.Locked).MaxBy(die => die.Eyes)?.Locked = true;
        //Console.WriteLine(string.Join(", ", dice.Select(die => $"{die.Eyes}{(die.Locked ? "L" : "")}")));
    }

    var sum = dice.Sum(die => die.Eyes);
    return sum;
}

static void UnlockAll(List<Die> dice)
{
    foreach (var die in dice) die.Locked = false;
}

static void RollUnlocked(List<Die> dice)
{
    foreach (var die in dice) die.RollIfUnlocked();
}
