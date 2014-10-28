namespace Assets.Scripts.Messages
{
    public class ReceiveHealingMessage
    {
        public ReceiveHealingMessage(int healing)
        {
            this.Healing = healing;
        }

        public int Healing { get; private set; }
    }
}