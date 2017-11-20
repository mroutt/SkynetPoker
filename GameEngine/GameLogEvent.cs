using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
    public class GameLogEvent
    {
        public string EventDescription { get; private set; }
        public GameState NewGameState { get; private set; }

        public GameLogEvent(string eventDescription, GameState newGameState)
        {
            EventDescription = eventDescription;
            NewGameState = newGameState;
        }
    }
}
