namespace Assets.Scripts.Messages
{
    public class HealingReceivedMessage
    {
        public HealingReceivedMessage(int healing)
        {
            this.Healing = healing;
        }

        public int Healing { get; private set; }
    }
}