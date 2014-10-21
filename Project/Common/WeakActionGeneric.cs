using System;

namespace Common
{
    internal class WeakAction<T> : WeakAction
    {
        private readonly Action<T> action;
        
        public WeakAction(object reference, Action<T> action)
            : base(reference, null)
        {
            this.action = action;
        }

        public new Action<T> Action
        {
            get
            {
                return this.action;
            }
        }

        public void Execute(T arg)
        {
            if (this.action != null && this.IsAlive)
            {
                this.action(arg);
            }
        }
    }
}