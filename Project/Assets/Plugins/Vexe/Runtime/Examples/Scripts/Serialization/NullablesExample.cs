using System.Collections.Generic;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Decorates;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Serialization
{
    [BasicView]
    public class NullablesExample : BetterBehaviour
    {
        [Whitespace(Top = 20f)]
        public int? nullableIntField;

        [Whitespace(Left = 40f)]
        public bool? nullableBoolProperty { get; set; }

        [Comment("this is an array with nullable floats")]
        public float?[] nullableFloatArray;

        [PerValue, IgnoreAddArea, OnChanged(Set = "position")]
        public Dictionary<string, Vector3?> nullableDictionary;
    }
}