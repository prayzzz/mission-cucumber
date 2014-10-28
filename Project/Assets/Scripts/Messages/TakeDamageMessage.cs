namespace Assets.Scripts.Messages
{
    public class TakeDamageMessage
    {
        public TakeDamageMessage(int damage)
        {
            this.Damage = damage;
        }

        public int Damage { get; private set; }
    }
}