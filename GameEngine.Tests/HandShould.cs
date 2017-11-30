using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GameEngine.Tests
{
    public class HandShould
    {
        [Fact]
        public void DealUniqueCardsFromDeck()
        {
            var log = new List<string>();
            var gameState = new GameState();

            var player1 = new MockPlayer("Billy", 1000, 1);
            var player2 = new MockPlayer("John", 1000, 2);

            var players = new List<Player>()
            {
                player1,
                player2
            };

            gameState.Players = players;

            Hand.Play(gameState, log);

            var allCardsDealt = new List<Card>();
            allCardsDealt.Add(player1.Card1);
            allCardsDealt.Add(player1.Card2);
            allCardsDealt.Add(player2.Card1);
            allCardsDealt.Add(player2.Card2);

            Assert.Equal(allCardsDealt.Count, allCardsDealt.Distinct().ToList().Count);
        }
    }
}
