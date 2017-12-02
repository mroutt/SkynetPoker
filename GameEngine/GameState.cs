using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class GameState
    {
        public List<Player> Players { get; set; }

        public int DealerPosition { get; private set; }

        public GameState()
        {
            DealerPosition = 1;
        }

        public void AdvanceAfterHand()
        {
            if ((DealerPosition + 1) > Players.Max(x => x.Seat))
                DealerPosition = 1;
            else
                DealerPosition++;
        }
    }
}
