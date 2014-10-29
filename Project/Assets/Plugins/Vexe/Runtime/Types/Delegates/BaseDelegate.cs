using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Vexe.Runtime.Types
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
			public UnityObject target;
			public string method;
		}
	}
}