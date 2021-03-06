﻿namespace GameEngine
{
    public class Player
    {
        public string Name { get; private set; }
        public int Chips { get; set; }
        public int Seat { get; private set; }

        public Player(string name, int chips, int seat)
        {
            Name = name;
            Chips = chips;
            Seat = seat;
        }

        public virtual PlayerAction GetPlayerAction()
        {
            return new PlayerAction("Raise", Chips);
        }

        public virtual void NotifyOfPlayerAction(PlayerAction action)
        {

        }

        public virtual void NotifyOfGameEvent(GameState newGameState)
        {

        }

        public virtual void DealCards(Card card1, Card card2)
        {
        }
    }
}
