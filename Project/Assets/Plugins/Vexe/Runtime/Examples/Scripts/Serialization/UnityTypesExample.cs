using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Serialization
{
    [BasicView]
    public class UnityTypesExample : BetterBehaviour
    {
        public Vector3 vector3;
        public Color color { get; set; }
        public Quaternion quaternion;

        [Save]
        private Vector2 vector2 { get; set; }

        [Serialize]
        protected Bounds bounds;

        [SerializeField]
        private LayerMask mask;
    }
}