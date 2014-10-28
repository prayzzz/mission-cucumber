namespace Assets.Scripts.Messages
{
    public class ModifyIncomingHealingMessage
    {
        public ModifyIncomingHealingMessage(int healing)
        {
            this.Healing = healing;
        }

        public int Healing { get; set; }
    }
}