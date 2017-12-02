namespace GameEngine
{
    public class Seat
    {
        public Card Card1 { get; set; }
        public Card Card2 { get; set; }
        public bool StillInHand { get; set; }
        public bool BigBlind { get; set; }
        public bool SmallBlind { get; set; }
        public Player Player { get; set; }
        
        public Seat(Player player)
        {
            StillInHand = true;
            Player = player;
        }

        public void DealCards(Card card1, Card card2)
        {
            Card1 = card1;
            Card2 = card2;
            Player.DealCards(card1, card2);
        }

        public PlayerAction RequestAction()
        {
            return Player.GetPlayerAction();
        }

        public void TakeChipsFromPlayer(int chips)
        {
            Player.Chips -= chips;
        }
    }
}
