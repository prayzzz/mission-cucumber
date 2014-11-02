using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

using Vexe.Runtime.Types;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    public class DefaultValueExample : BetterBehaviour
    {
        [Default("Hello")]
        public string stringField;

        [Default("World")]
        public string StringProperty { get; set; }

        [Default(10)]
        public int intField;

        [Default(15)]
        public int IntProperty { get; set; }

        [Default(10.75f)]
        public float floatField;

        [Default(15.25f)]
        public float FloatProperty { get; set; }

        [Default(true)]
        public bool BoolProperty { get; set; }

        [Default(Colors.Red)] // defined in Vexe.Runtime.Types
        public Color colorField;

        [Default(1, 0.5f, 0.2f, 0.3f)] // r, g, b, a
        public Color ColorProperty { get; set; }

        [Default(1f, 2f)]
        public Vector2 vector2;

        [Default(0f, 2f, 4f)]
        public Vector3 vector3 { get; set; }

        [Default(Enum = (int)KeyCode.Space)]
        public KeyCode keyJump;

        [Default(new[] { 1, 2, 3 })]
        public int[] intArray;

        [Default(new[] { 1.5f, 2.5f, 3.2f })]
        public float[] FloatArray { get; set; }

        [Default(new[] { true, true, false, true })]
        public bool[] boolArray;

        [Default(new[] { "hello", "world" })]
        public string[] StringArray { get; set; }
    }
}