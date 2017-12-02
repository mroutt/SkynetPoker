namespace GameEngine.Tests
{
    public class MockPlayer : Player
    {
        public bool PlayerNotified { get; set; } = false;
        public bool ActionRequested { get; set; } = false;
        public Card Card1 { get; set; }
        public Card Card2 { get; set; }
        public PlayerAction PlayerActionWhenRequested { get; set; }
        public int NumberOfDealerSeatAdvances { get; set; }
        public int PlayerActionRequestOrder { get; set; }

        private int LastKnownDealerSeat { get; set; }
        private int PlayerActionCount { get; set; }

        public MockPlayer(string name, int chips, int seat) : base(name, chips, seat)
        {
            PlayerActionWhenRequested = new PlayerAction("Raise", 100);
            NumberOfDealerSeatAdvances = 0;
            LastKnownDealerSeat = 0;
            PlayerActionCount = 0;
        }

        public override void NotifyOfPlayerAction(PlayerAction action)
        {
            PlayerNotified = true;
            PlayerActionCount++;
        }

        public override void NotifyOfGameEvent(GameState newGameState)
        {
            if (newGameState.DealerPosition != LastKnownDealerSeat)
            {
                NumberOfDealerSeatAdvances++;
                LastKnownDealerSeat = newGameState.DealerPosition;
            }
        }

        public override void DealCards(Card card1, Card card2)
        {
            Card1 = card1;
            Card2 = card2;
        }

        public override PlayerAction GetPlayerAction()
        {
            ActionRequested = true;
            PlayerActionRequestOrder = PlayerActionCount + 1;
            return PlayerActionWhenRequested;
        }
    }
}
