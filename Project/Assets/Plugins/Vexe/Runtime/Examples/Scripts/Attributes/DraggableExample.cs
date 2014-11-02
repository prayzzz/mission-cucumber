using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Decorates;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    [BasicView]
    public class DraggableExample : BetterBehaviour
    {
        [Comment("Try and drag these two fields around")]
        [Draggable]
        public GameObject dragMe1;

        [Draggable]
        public GameObject DragMe2 { get; set; }
    }
}