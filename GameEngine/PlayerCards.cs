namespace GameEngine
{
    public class PlayerCards
    {
        public int Seat { get; set; }
        public Card Card1 { get; set; }
        public Card Card2 { get; set; }

        public PlayerCards(int seat, Card card1, Card card2)
        {
            Seat = seat;
            Card1 = card1;
            Card2 = card2;
        }

    }
}
