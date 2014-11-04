namespace Assets.Scripts.Messages
{
    public class DealDamageMessage
    {
        public DealDamageMessage(int damage)
        {
            this.Damage = damage;
        }

        public int Damage { get; private set; }
    }
}