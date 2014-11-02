using System;
using System.Collections.Generic;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Vexe.Runtime.Types.Delegates
{
    [Serializable]
    public abstract class BaseDelegate
    {
        [SerializeField]
        public List<Handler> handlers = new List<Handler>();

        public abstract Type[] ParamTypes { get; }
        public abstract Type ReturnType { get; }

        [Serializable]
        public class Handler
        {
            public Object target;
            public string method;
        }
    }
}