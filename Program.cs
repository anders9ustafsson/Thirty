using Thirty;

const int numberOfRounds = 10000000;
const int stopAt = 30;
const int strategyMinEyes = 4;

// Loop over relevant lock strategies
for (var d6 = 6; d6 >= strategyMinEyes; --d6)
for (var d5 = d6; d5 >= strategyMinEyes; --d5)
for (var d4 = d5; d4 >= strategyMinEyes; --d4)
for (var d3 = d4; d3 >= strategyMinEyes; --d3)
for (var d2 = d3; d2 >= strategyMinEyes; --d2)
{
    var lockStrategy = new LockStrategy(new Dictionary<int, int>
        { { 6, d6 }, { 5, d5 }, { 4, d4 }, { 3, d3 }, { 2, d2 } }, stopAt);
    var player = new Player(lockStrategy);

    var win = 0;
    var points = 0.0;

    // Create list of six die
    var dice = Enumerable.Range(0, 6).Select(_ => new Die()).ToList();

    for (var i = 0; i < numberOfRounds; i++)
    {
        var sum = player.Play(dice);
        if (sum >= stopAt) win++;
        
        // Minus points if below 30, 10 bonus if all dice have the same eyes
        points += Math.Min(0, sum - 30) + (dice.All(die => die == dice[0]) ? 10 : 0);
    }

    Console.WriteLine($"Strategy {d6},{d5},{d4},{d3},{d2} wins {(double)win / numberOfRounds * 100.0:F1}%, average points {points / numberOfRounds:F3}");
}
