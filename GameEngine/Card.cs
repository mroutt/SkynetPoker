using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
    public class Card
    {
        public char Suit { get; private set; }
        public int Value { get; private set; }

        public Card(char suit, int value)
        {
            Suit = suit;
            Value = value;
        }
    }
}
