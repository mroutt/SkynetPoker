namespace GameEngine
{
    public class PlayerCards
    {
        public int Seat { get; set; }
        public int Card1 { get; set; }
        public int Card2 { get; set; }

        public PlayerCards(int seat, int card1, int card2)
        {
            Seat = seat;
            Card1 = card1;
            Card2 = card2;
        }

    }
}
