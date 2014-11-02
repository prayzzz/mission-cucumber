namespace Assets.Scripts.Messages
{
    public class DamageDoneMessage
    {
        public DamageDoneMessage(int damage)
        {
            this.Damage = damage;
        }

        public int Damage { get; private set; }
    }
}