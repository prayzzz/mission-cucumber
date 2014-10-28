namespace Assets.Scripts.Messages
{
    public class ModifyIncomingDamageMessage
    {
        public ModifyIncomingDamageMessage(int damage)
        {
            this.Damage = damage;
        }

        public int Damage { get; set; }
    }
}