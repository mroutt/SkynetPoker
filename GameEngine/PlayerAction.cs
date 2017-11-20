namespace GameEngine
{
    public class PlayerAction
    {
        public string Action { get; private set; }
        public int ChipAmount { get; private set; }

        public PlayerAction(string action, int chipAmount)
        {
            Action = action;
            ChipAmount = chipAmount;
        }
    }
}
