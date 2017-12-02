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
        public void StartActionToPlayerAfterBigBlind()
        {
            var gameState = new GameState();

            var dealer = new MockPlayer("Billy", 1000, 1);
            var smallBlind = new MockPlayer("John", 1000, 2);
            var bigBlind = new MockPlayer("Steve", 1000, 3);
            var firstAction = new MockPlayer("Dave", 1000, 4);

            gameState.Players = new List<Player>() { dealer, smallBlind, bigBlind, firstAction };

            Hand.Play(gameState, new List<string>());

            Assert.Equal(1, firstAction.PlayerActionRequestOrder);
        }

        [Fact]
        public void CollectBlinds()
        {
            var gameState = new GameState();

            var dealer = new MockPlayer("Dealer", 1000, 1);
            var smallBlind = new MockPlayer("Small", 1000, 2);
            var bigBlind = new MockPlayer("Big", 1000, 3);
            var firstAction = new MockPlayer("FirstAction", 1000, 4);

            firstAction.PlayerActionWhenRequested = new PlayerAction("Fold", 0);
            dealer.PlayerActionWhenRequested = new PlayerAction("Raise", 500);
            smallBlind.PlayerActionWhenRequested = new PlayerAction("Fold", 0);
            bigBlind.PlayerActionWhenRequested = new PlayerAction("Fold", 0);

            gameState.Players = new List<Player>() { dealer, smallBlind, bigBlind, firstAction};

            Hand.Play(gameState, new List<string>());

            Assert.Equal(1150, dealer.Chips);
            Assert.Equal(950, smallBlind.Chips);
            Assert.Equal(900, bigBlind.Chips);
        }

        [Fact]
        public void AllowPlayerToFold()
        {
            var gameState = new GameState();

            var dealer = new MockPlayer("Billy", 1000, 1);

            var firstAction = new MockPlayer("John", 1000, 2);

            firstAction.PlayerActionWhenRequested = new PlayerAction("Fold", 0);

            gameState.Players = new List<Player>() { dealer, firstAction };

            Hand.Play(gameState, new List<string>());

            Assert.False(dealer.ActionRequested);
        }

        [Fact]
        public void NotAwardPotToFoldedPlayerWithWinningHand()
        {
            var log = new List<string>();
            var gameState = new GameState();

            var withoutPair = new MockPlayer("NoPair", 1000, 1);
            var withPairWillFold = new MockPlayer("FoldsPair", 1000, 2);
            withPairWillFold.PlayerActionWhenRequested = new PlayerAction("Fold", 0);

            gameState.Players = new List<Player>() { withoutPair, withPairWillFold };

            var mockDeck = new MockDeck();
            mockDeck.PushCardOntoDeck(new Card('C', 11));
            mockDeck.PushCardOntoDeck(new Card('S', 5));
            mockDeck.PushCardOntoDeck(new Card('D', 7));
            mockDeck.PushCardOntoDeck(new Card('H', 7));

            Hand.Play(gameState, log, mockDeck);

            Assert.True(withoutPair.Chips > withPairWillFold.Chips);
        }

        [Fact]
        public void AwardPotToPlayerWithPair()
        {
            var log = new List<string>();
            var gameState = new GameState();

            var withoutPair = new MockPlayer("Billy", 1000, 1);
            var withPair = new MockPlayer("John", 1000, 2);

            gameState.Players = new List<Player>() { withoutPair, withPair };

            var mockDeck = new MockDeck();
            mockDeck.PushCardOntoDeck(new Card('C', 11));
            mockDeck.PushCardOntoDeck(new Card('S', 5));
            mockDeck.PushCardOntoDeck(new Card('D', 7));
            mockDeck.PushCardOntoDeck(new Card('H', 7));

            Hand.Play(gameState, log, mockDeck);

            Assert.True(withPair.Chips > withoutPair.Chips);
        }

        [Fact]
        public void AwardPotToPlayerWithHighCard()
        {
            var log = new List<string>();
            var gameState = new GameState();

            var player1 = new MockPlayer("Billy", 1000, 1);
            var player2 = new MockPlayer("John", 1000, 2);
            var player3 = new MockPlayer("Steve", 1000, 3);

            gameState.Players = new List<Player>() { player1, player2, player3 };

            var mockDeck = new MockDeck();
            mockDeck.PushCardOntoDeck(new Card('S', 14));
            mockDeck.PushCardOntoDeck(new Card('D', 3));
            mockDeck.PushCardOntoDeck(new Card('S', 5));
            mockDeck.PushCardOntoDeck(new Card('D', 7));
            mockDeck.PushCardOntoDeck(new Card('H', 6));
            mockDeck.PushCardOntoDeck(new Card('C', 11));

            Hand.Play(gameState, log, mockDeck);

            Assert.True(player3.Chips > player1.Chips);
            Assert.True(player3.Chips > player2.Chips);
        }

        [Fact]
        public void AwardPotToPlayerWithHighPair()
        {
            var log = new List<string>();
            var gameState = new GameState();

            var player1 = new MockPlayer("Billy", 1000, 1);
            var player2 = new MockPlayer("John", 1000, 2);
            var player3 = new MockPlayer("Steve", 1000, 3);

            gameState.Players = new List<Player>() { player1, player2, player3 };

            var mockDeck = new MockDeck();
            mockDeck.PushCardOntoDeck(new Card('C', 14));
            mockDeck.PushCardOntoDeck(new Card('S', 14));
            mockDeck.PushCardOntoDeck(new Card('C', 5));
            mockDeck.PushCardOntoDeck(new Card('S', 5));
            mockDeck.PushCardOntoDeck(new Card('D', 2));
            mockDeck.PushCardOntoDeck(new Card('H', 2));

            Hand.Play(gameState, log, mockDeck);

            Assert.True(player3.Chips > player1.Chips);
            Assert.True(player3.Chips > player2.Chips);
        }

        [Fact]
        public void DealUniqueCardsFromDeck()
        {
            var log = new List<string>();
            var gameState = new GameState();

            var player1 = new MockPlayer("Billy", 1000, 1);
            var player2 = new MockPlayer("John", 1000, 2);

            gameState.Players = new List<Player>() { player1, player2 };

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
