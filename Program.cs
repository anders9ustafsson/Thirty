using Thirty;

SimplePlay();
return;

static IEnumerable<LockStrategy> EnumerateLockStrategies(int minEyes, int stopAt)
{
    for (var d6 = 6; d6 >= minEyes; --d6)
    for (var d5 = d6; d5 >= minEyes; --d5)
    for (var d4 = d5; d4 >= minEyes; --d4)
    for (var d3 = d4; d3 >= minEyes; --d3)
    for (var d2 = d3; d2 >= minEyes; --d2)
    {
        yield return new LockStrategy(new Dictionary<int, int>
            { { 6, d6 }, { 5, d5 }, { 4, d4 }, { 3, d3 }, { 2, d2 } }, stopAt);
    }
}

static void CreateStatistics()
{
    const int stopAt = 30;
    const int strategyMinEyes = 4;
    const int numberOfRounds = 100000;

    // Loop over relevant lock strategies
    foreach (var lockStrategy in EnumerateLockStrategies(strategyMinEyes, stopAt))
    {
        var player = new Player(lockStrategy);

        var win = 0;
        var totalPoints = 0.0;

        // Create list of six die
        var dice = Enumerable.Range(0, 6).Select(_ => new Die()).ToList();

        for (var i = 0; i < numberOfRounds; i++)
        {
            var (sum, points) = player.PlayQualification(dice);
            if (sum >= 30) win++;

            // Minus points if below 30, 10 bonus if all dice have the same eyes
            totalPoints += points;
        }

        Console.WriteLine(
            $"Strategy {lockStrategy} wins {(double)win / numberOfRounds * 100.0:F1}%, average points {totalPoints / numberOfRounds:F3}");
    }
}

static void SimplePlay()
{
    const int stopAt = 30;
    const int strategyMinEyes = 4;

    var player = new Player(EnumerateLockStrategies(strategyMinEyes, stopAt).First());
    var dice = Enumerable.Range(0, 6).Select(_ => new Die()).ToList();
    player.Play(dice, 0, true);
}
