using System;
using System.Linq;
using Vexe.Runtime.Extensions;
using UnityObject = UnityEngine.Object;

namespace Vexe.Runtime.Types
{
	[Serializable]
	public abstract class uDelegate<T> : BaseDelegate where T : class
	{
		protected T directValue;

		protected T Value
		{
			get { if (directValue == null) Rebuild(); return directValue; }
			set { directValue = value; }
		}

		public void Add(T handler)
		{
			AssertHandlerValidity(handler);
			handlers.Add(new Handler
			{
				target = GetHandlerTarget(handler) as UnityObject,
				method = GetHandlerMethod(handler)
			});
			DirectAdd(handler);
		}

		public void Remove(T handler)
		{
			AssertHandlerValidity(handler);
			int index = handlers.IndexOf(t => t.target == GetUnityTarget(handler));
			if (index == -1) return;
			handlers.RemoveAt(index);
			DirectRemove(handler);
		}

		public bool Contains(T handler)
		{
			AssertHandlerValidity(handler);
			return handlers.Any(t => t.target == GetUnityTarget(handler) &&
											 t.method == GetHandlerMethod(handler));
		}

		public void Clear()
		{
			directValue = null;
			handlers.Clear();
		}

		public void Rebuild()
		{
			directValue = null;
			for (int i = 0; i < handlers.Count; i++)
			{
				var handler = handlers[i];
				var del     = Delegate.CreateDelegate(typeof(T), handler.target, handler.method) as T;
				DirectAdd(del);
			}
		}

		protected abstract string GetHandlerMethod(T handler);
		protected abstract object GetHandlerTarget(T handler);
		protected abstract void DirectAdd(T handler);
		protected abstract void DirectRemove(T handler);

		private UnityObject GetUnityTarget(T handler)
		{
			return GetHandlerTarget(handler) as UnityObject;
		}

		private void AssertHandlerValidity(T handler)
		{
			if (GetUnityTarget(handler) == null)
				throw new InvalidOperationException("handler's target must be a unity object");
		}
	}
}