using Assets.Scripts.Common;

namespace Assets.Scripts.Messages
{
    public class TeamRequestMessage
    {
        public TeamRequestMessage()
        {
            this.Team = TeamEnum.Neutral;
        }

        public TeamEnum Team { get; set; }
    }
}