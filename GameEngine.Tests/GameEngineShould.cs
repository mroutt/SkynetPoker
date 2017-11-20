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
        public void ReturnLogOfGameWithInitialState()
        {
            var gameLog = _engine.PlayGame();
            var initialLogEvent = gameLog.First();
            var initialGameState = initialLogEvent.NewGameState;
            string initialEventDescription = initialLogEvent.EventDescription;

            Assert.Equal("Starting game.", initialEventDescription);
            Assert.NotNull(initialGameState);
            Assert.NotEmpty(initialGameState.Players);

            foreach (var player in initialGameState.Players)
            {
                Assert.NotNull(player.Name);
                Assert.NotNull(player.Chips);
                Assert.NotNull(player.Seat);
            }

            Assert.Equal(1, initialGameState.DealerSeat);
        }

        [Fact]
        public void ReturnMoreThanOneLogEventInGame()
        {
            var gameLog = _engine.PlayGame();
            Assert.True(gameLog.Count() > 1);
        }

        [Fact]
        public void ReturnAWinnerAfterGame()
        {
            var gameLog = _engine.PlayGame();

            var endGameState = gameLog.Last().NewGameState;

            var players = endGameState.Players;

            Assert.Equal(1, players.Where(x => x.Chips > 0).Count());
            Assert.Contains("has won the game.", gameLog.Last().EventDescription);
        }
        
        [Fact]
        public void PlayGameDealsCards()
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

            Assert.True(player1.Card1 > 0);
            Assert.True(player1.Card2 > 0);
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
            Assert.Equal(0, player1.Chips);
        }

        [Fact]
        public void AwardPotsAfterGame()
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
            Assert.Equal(0, player1.Chips);
            Assert.Equal(2000, player2.Chips);
        }
    }
}
