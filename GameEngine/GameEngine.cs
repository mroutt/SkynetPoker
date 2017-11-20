using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class GameEngine
    {
        public List<Player> Players { get; set; }
        private GameState _currentGameState;
        private List<GameLogEvent> _gameLog;

        public GameEngine()
        {
            Players = CreatePlayers();
        }

        public List<GameLogEvent> PlayGame()
        {
            _currentGameState = new GameState() { Players = Players };
            _gameLog = new List<GameLogEvent>() { new GameLogEvent("Starting game.", _currentGameState) };

            while(MoreThanOnePlayerHasChips())
            {
                PlayHand(_currentGameState.Players);
            }

            var winningPlayer = Players.Where(x => x.Chips > 0).Single();

            string finalGameMessage = string.Format("{0} in seat {1} has won the game.", winningPlayer.Name, winningPlayer.Seat);

            _gameLog.Add(new GameLogEvent(finalGameMessage, _currentGameState));

            return _gameLog;
        }

        private void PlayHand(List<Player> players)
        {
            var cards = DealCards(players);
            int currentPot = 0;

            foreach(var player in players)
            {
                var action = player.GetPlayerAction();
                currentPot = ApplyPlayerAction(action, player, currentPot);
                NotifyPlayersOfAction(action, players);
                LogPlayerAction(action, player);
            }

            int winningSeat = DetermineWinningSeat(cards);

            AwardPotToPlayerAtWinningSeat(winningSeat, players, currentPot);
        }

        private void LogPlayerAction(PlayerAction action, Player player)
        {
            string actionDescription = "";
            switch(action.Action)
            {
                case "Raise":
                    actionDescription = "raises " + action.ChipAmount;
                    break;

                case "Call":
                    actionDescription = "calls " + action.ChipAmount;
                    break;
            }

            string logMessage = string.Format("{0} in seat {1} {2}", player.Name, player.Seat, actionDescription);

            _gameLog.Add(new GameLogEvent(logMessage, _currentGameState));
        }

        private void AwardPotToPlayerAtWinningSeat(int winningSeat, List<Player> players, int currentPot)
        {
            var player = players.Where(x => x.Seat == winningSeat).Single();
            player.Chips += currentPot;
        }

        private int DetermineWinningSeat(List<PlayerCards> cards)
        {
            int maxScore = 0;
            int maxScoreSeat = 0;
            foreach(var card in cards)
            {
                int score = card.Card1 + card.Card2;

                if(score > maxScore)
                {
                    maxScore = score;
                    maxScoreSeat = card.Seat;
                }
            }

            return maxScoreSeat;
        }

        private List<PlayerCards> DealCards(List<Player> players)
        {
            var cards = new List<PlayerCards>();
            int cardToDeal = 1;

            foreach (var player in players)
            {
                cards.Add(new PlayerCards(player.Seat, cardToDeal, cardToDeal));
                player.DealCards(cardToDeal, cardToDeal);
                cardToDeal += 1;
            }

            return cards;
        }

        private int ApplyPlayerAction(PlayerAction action, Player player, int currentPot)
        {
            if (action.Action == "Raise" || action.Action == "Call")
            {
                player.Chips -= action.ChipAmount;
                currentPot += action.ChipAmount;
            }

            return currentPot;
        }

        private void NotifyPlayersOfAction(PlayerAction action, List<Player> players)
        {
            foreach(var player in players)
            {
                player.NotifyOfPlayerAction(action);
            }
        }

        private List<Player> CreatePlayers()
        {
            return new List<Player>()
            {
                new Player("Joe Bob", 1000, 1),
                new Player("Tommy", 1000, 2),
                new Player("John", 1000, 3)
            };
        }

        private bool MoreThanOnePlayerHasChips()
        {
            int numberOfPlayersWithChips = _currentGameState.Players.Where(x => x.Chips > 0).Count();
            return  numberOfPlayersWithChips > 1;
        }
    }
}
