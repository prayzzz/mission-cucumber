namespace Assets.Scripts.Messages
{
    public class DamageTakenMessage
    {
        public DamageTakenMessage(int damage)
        {
            this.Damage = damage;
        }

        public int Damage { get; private set; }
    }
}