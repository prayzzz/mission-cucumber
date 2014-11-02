using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Selections;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    [BasicView]
    public class SelectionsExample : BetterBehaviour
    {
        [SelectObj]
        public BoxCollider boxCollider;

        [SelectObj]
        public GameObject GO { get; set; }

        [SelectEnum]
        public KeyCode jumpKey;
    }
}