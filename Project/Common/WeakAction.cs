using System;

namespace Common
{
    public class WeakAction
    {
        private readonly Action action;

        private WeakReference reference;

        public WeakAction(object reference, Action action)
        {
            this.action = action;
            this.reference = new WeakReference(reference);
        }
        
        public bool IsAlive
        {
            get
            {
                return this.reference != null && this.reference.IsAlive;
            }
        }

        public Action Action
        {
            get
            {
                return this.action;
            }
        }

        public void Execute()
        {
            if (this.action != null && this.IsAlive)
            {
                this.action();
            }
        }

        public void MarkForDeletion()
        {
            this.reference = null;
        }
    }
}