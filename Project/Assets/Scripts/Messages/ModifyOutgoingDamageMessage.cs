namespace Assets.Scripts.Messages
{
    public class ModifyOutgoingDamageMessage
    {
        public ModifyOutgoingDamageMessage(int damage)
        {
            this.Damage = damage;
        }

        public int Damage { get; set; }
    }
}