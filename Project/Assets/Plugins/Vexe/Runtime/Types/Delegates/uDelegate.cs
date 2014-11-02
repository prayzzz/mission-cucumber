using System;
using System.Linq;

using Vexe.Runtime.Extensions;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Vexe.Runtime.Types.Delegates
{
    [Serializable]
    public abstract class uDelegate<T> : BaseDelegate where T : class
    {
        protected T directValue;

        protected T Value
        {
            get { if (this.directValue == null) this.Rebuild(); return this.directValue; }
            set { this.directValue = value; }
        }

        public void Add(T handler)
        {
            this.AssertHandlerValidity(handler);
            this.handlers.Add(new Handler
            {
                target = this.GetHandlerTarget(handler) as Object,
                method = this.GetHandlerMethod(handler)
            });
            this.DirectAdd(handler);
        }

        public void Remove(T handler)
        {
            this.AssertHandlerValidity(handler);
            int index = this.handlers.IndexOf(t => t.target == this.GetUnityTarget(handler));
            if (index == -1) return;
            this.handlers.RemoveAt(index);
            this.DirectRemove(handler);
        }

        public bool Contains(T handler)
        {
            this.AssertHandlerValidity(handler);
            return this.handlers.Any(t => t.target == this.GetUnityTarget(handler) &&
                                             t.method == this.GetHandlerMethod(handler));
        }

        public void Clear()
        {
            this.directValue = null;
            this.handlers.Clear();
        }

        public void Rebuild()
        {
            this.directValue = null;
            for (int i = 0; i < this.handlers.Count; i++)
            {
                var handler = this.handlers[i];
                var del     = Delegate.CreateDelegate(typeof(T), handler.target, handler.method) as T;
                this.DirectAdd(del);
            }
        }

        protected abstract string GetHandlerMethod(T handler);
        protected abstract object GetHandlerTarget(T handler);
        protected abstract void DirectAdd(T handler);
        protected abstract void DirectRemove(T handler);

        private Object GetUnityTarget(T handler)
        {
            return this.GetHandlerTarget(handler) as Object;
        }

        private void AssertHandlerValidity(T handler)
        {
            if (this.GetUnityTarget(handler) == null)
                throw new InvalidOperationException("handler's target must be a unity object");
        }
    }
}