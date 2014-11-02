using System;
using System.Collections.Generic;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Serialization
{
    [BasicView]
    public class StructsExample : BetterBehaviour
    {
        public MyStruct0 structField;
        public MyStruct1[] StructArrayProperty { get; set; }
        public Dictionary<MyStruct0, MyStruct1> dict;

        [Show]void set()
        {
            this.structField.struct1.vector = this.position;
        }
    }

    [Serializable]
    public struct MyStruct0
    {
        [Serialize]
        private GameObject Go;
        public MyStruct1 struct1;
    }

    [Serializable]
    public struct MyStruct1
    {
        [Save]
        private float value;
        public Vector3 vector;
    }
}