using Assets.Scripts.BaseClasses;
using Assets.Scripts.Messages;

namespace Assets.Scripts.Components.TargetFilter
{
    public class DeadFilter : BaseTargetFilter
    {
        public override bool IsTargetValid(BaseUnit target)
        {
            var aliveMessage = new IsAliveMessage();
            target.Messenger.Send(aliveMessage);

            return !aliveMessage.IsAlive;
        }
    }
}