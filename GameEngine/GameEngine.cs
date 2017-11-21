using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class GameEngine
    {
        public List<Player> Players { get; set; }

        public List<string> PlayGame()
        {
            var currentGameState = new GameState() { Players = Players };
            var gameLog = new List<string>() { "Starting game." };

            while(MoreThanOnePlayerHasChips(Players))
            {
                Hand.Play(currentGameState, gameLog);
            }

            var winningPlayer = DetermineWinningPlayer(Players);

            gameLog.Add(FormatWinningGameMessage(winningPlayer));

            return gameLog;
        }

        private string FormatWinningGameMessage(Player winningPlayer)
        {
            return string.Format("{0} in seat {1} has won the game.", winningPlayer.Name, winningPlayer.Seat);
        }

        private bool MoreThanOnePlayerHasChips(List<Player> players)
        {
            int numberOfPlayersWithChips = players.Where(x => x.Chips > 0).Count();
            return  numberOfPlayersWithChips > 1;
        }

        private Player DetermineWinningPlayer(List<Player> players)
        {
            return players.Where(x => x.Chips > 0).Single();
        }
    }
}
