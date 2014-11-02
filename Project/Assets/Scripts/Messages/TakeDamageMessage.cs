using Assets.Scripts.BaseClasses;

namespace Assets.Scripts.Messages
{
    public class TakeDamageMessage
    {
        public TakeDamageMessage(BaseUnit sender, int damage)
        {
            this.Sender = sender;
            this.Damage = damage;
        }

        public BaseUnit Sender { get; private set; }

        public int Damage { get; private set; }
    }
}