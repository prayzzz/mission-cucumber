using System;
using System.Collections.Generic;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Decorates;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Serialization
{
    [BasicView]
    public class GenericsExample : BetterBehaviour
    {
        [Comment("This might not serialize, depending on your serializer", 2)]
        public Tuple<Transform, Tuple<string, GameObject>> Tuple { get; set; }

        [Comment("This might not serialize, depending on your serializer", 2)]
        public List<List<List<Dictionary<string, IOwner<Vector3>>>>> nasty;

        public IOwner<GameObject> GoOwner { get; set; }
        public IOwner<Vector3> VecOwner { get; set; }
    }

    public interface IOwner<T>
    {
        T Value { get; set; }
    }

    [Serializable]
    public class Vector3Owner : IOwner<Vector3>
    {
        public Vector3 Value { get; set; }
    }

    [Serializable]
    public class GameObjectOwner : IOwner<GameObject>
    {
        public GameObject Value { get; set; }
    }

    [Serializable]
    public class Tuple<TKey, TValue>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", this.Key, this.Value);
        }
    }
}