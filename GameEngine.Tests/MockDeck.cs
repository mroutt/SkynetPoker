using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.Tests
{
    public class MockDeck : Deck
    {
        private Stack<Card> _cards;

        public MockDeck() : base()
        {
            _cards = new Stack<Card>();
        }

        public override Card DrawCard()
        {
            return _cards.Pop();
        }

        public void PushCardOntoDeck(Card card)
        {
            _cards.Push(card);
        }
    }
}
