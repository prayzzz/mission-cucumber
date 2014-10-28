namespace Assets.Scripts.Messages
{
    public class IsVisibleMessage
    {
        public IsVisibleMessage()
        {
            this.IsVisble = true;
        }

        public bool IsVisble { get; set; }
    }
}