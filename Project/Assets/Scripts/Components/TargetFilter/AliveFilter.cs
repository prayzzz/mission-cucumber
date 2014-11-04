using Assets.Scripts.BaseClasses;
using Assets.Scripts.Messages;

namespace Assets.Scripts.Components.TargetFilter
{
    public class AliveFilter : BaseTargetFilter
    {
        public override bool IsTargetValid(BaseUnit target)
        {
            var aliveMessage = new IsAliveMessage();
            target.Messenger.Send(aliveMessage);

            return aliveMessage.IsAlive;
        }
    }
}