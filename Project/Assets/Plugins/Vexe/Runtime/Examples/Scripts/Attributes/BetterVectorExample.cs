using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Vectors;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    public class BetterVectorExample : BetterBehaviour
    {
        [BetterVector]
        public Vector2 vector2Field;

        [BetterVector]
        public Vector2 Vector2Property { get; set; }

        [BetterVector]
        public Vector3 vector3Field;

        [BetterVector]
        public Vector3 Vector3Property { get; set; }
    }
}