using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GameEngine.Tests
{
    public class GameEngineShould
    {
        private readonly GameEngine _engine;

        public GameEngineShould()
        {
            _engine = new GameEngine();
        }

        [Fact]
        public void ReturnFullGameLogs()
        {
            var player1 = new MockPlayer("Billy", 1000, 1) { PlayerActionWhenRequested = new PlayerAction("Raise", 100) };
            var player2 = new MockPlayer("John", 1000, 2) { PlayerActionWhenRequested = new PlayerAction("Call", 100) };

            var players = new List<Player>()
            {
                player1,
                player2
            };

            _engine.Players = players;

            var gameLog = _engine.PlayGame();

            var startGameMessage = gameLog.First();
            var endGameMessage = gameLog.Last();

            Assert.Equal("Starting game.", startGameMessage);
            Assert.Contains("has won the game.", endGameMessage);
            Assert.True(gameLog.Count() > 41);
        }
        
        [Fact]
        public void DealCards()
        {
            var player1 = new MockPlayer("Billy", 1000, 1);
            var player2 = new MockPlayer("John", 1000, 2);

            var players = new List<Player>()
            {
                player1,
                player2
            };

            _engine.Players = players;

            _engine.PlayGame();

            Assert.NotNull(player1.Card1);
            Assert.NotNull(player1.Card2);
        }

        [Fact]
        public void NotifyPlayersOfActions()
        {
            var player1 = new MockPlayer("Billy", 1000, 1);
            var player2 = new MockPlayer("John", 1000, 2);

            var players = new List<Player>()
            {
                player1,
                player2
            };

            _engine.Players = players;

            _engine.PlayGame();

            Assert.True(player1.PlayerNotified);
        }

        [Fact]
        public void ProcessPlayerAction()
        {
            var player1 = new MockPlayer("Billy", 1000, 1) { PlayerActionWhenRequested = new PlayerAction("Raise", 1000) };
            var player2 = new MockPlayer("John", 1000, 2);

            var players = new List<Player>()
            {
                player1,
                player2
            };

            _engine.Players = players;

            _engine.PlayGame();

            Assert.True(player1.ActionRequested);
        }

        [Fact]
        public void EndGameWithAccurateNumberOfChips()
        {
            var player1 = new MockPlayer("Billy", 1000, 1) { PlayerActionWhenRequested = new PlayerAction("Raise", 100) };
            var player2 = new MockPlayer("John", 1000, 2) { PlayerActionWhenRequested = new PlayerAction("Call", 100) };

            var players = new List<Player>()
            {
                player1,
                player2
            };

            _engine.Players = players;

            _engine.PlayGame();

            Assert.True(player1.ActionRequested);
            Assert.Equal(1, players.Where(x => x.Chips == 0).Count());
            Assert.Equal(1, players.Where(x => x.Chips == 2000).Count());
        }
    }
}
