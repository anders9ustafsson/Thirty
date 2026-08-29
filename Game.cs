namespace Thirty;

public class Game(List<Player> players)
{
    public Player Play(bool log = false, bool verbose = false)
    {
        // Reset the players' scores
        foreach (var player in players) player.NewGame();

        var dice = Die.NewDice();

        var penalty = 0;
        while (players.Count(p => p.IsIn) > 1)
        {
            foreach (var player in players)
            {
                if (player.IsIn)
                {
                    penalty = player.Play(dice, penalty, log && verbose);
                    if (log) Console.WriteLine($"{player.Name} has {player.Points} points remaining.");
                }

                // If only one player is left, break out of the loop
                if (players.Count(p => p.IsIn) == 1) break;
            }
        }

        var winner = players.Single(p => p.IsIn);
        if (log) Console.WriteLine($"{winner.Name} wins!");

        return winner;
    }
}
