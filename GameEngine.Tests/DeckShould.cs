using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GameEngine.Tests
{
    public class DeckShould
    {
        [Fact]
        public void Draw52Cards()
        {
            int count = 0;
            var deck = new Deck();
            var drawnCards = new List<Card>();

            while (count < 52)
            {
                var card = deck.DrawCard();
                Assert.NotNull(card);
                Assert.False(drawnCards.Contains(card));
                drawnCards.Add(card);
                Assert.True(drawnCards.Contains(card));
                count++;
            }

            Assert.Equal(52, count);
        }
    }
}
