using System;
using Xunit;

namespace GameEngine.Tests
{
    public class GameEngineShould
    {
        [Fact]
        public void Start()
        {
            var engine = new GameEngine();

            engine.StartGame();
        }
    }
}
