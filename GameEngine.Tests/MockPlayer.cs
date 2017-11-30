namespace GameEngine.Tests
{
    public class MockPlayer : Player
    {
        public bool PlayerNotified { get; set; } = false;
        public bool ActionRequested { get; set; } = false;
        public Card Card1 { get; set; }
        public Card Card2 { get; set; }
        public PlayerAction PlayerActionWhenRequested { get; set; }

        public MockPlayer(string name, int chips, int seat) : base(name, chips, seat)
        {
            PlayerActionWhenRequested = new PlayerAction("Raise", 100);
        }

        public override void NotifyOfPlayerAction(PlayerAction action)
        {
            PlayerNotified = true;
        }

        public override void DealCards(Card card1, Card card2)
        {
            Card1 = card1;
            Card2 = card2;
        }

        public override PlayerAction GetPlayerAction()
        {
            ActionRequested = true;
            return PlayerActionWhenRequested;
        }
    }
}
