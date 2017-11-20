using System.Collections.Generic;

namespace GameEngine
{
    public class GameState
    {
        public List<Player> Players { get; set; }

        public int DealerSeat { get; private set; }

        public GameState()
        {
            DealerSeat = 1;
        }
    }
}
