using System;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Vexe.Runtime.Types.Delegates
{
    [Serializable]
    public class uAction : uDelegate<Action>
    {
        public override Type[] ParamTypes
        {
            get { return Type.EmptyTypes; }
        }

        public override Type ReturnType
        {
            get { return typeof(void); }
        }

        public void Invoke()
        {
            this.Value.SafeInvoke();
        }

        protected override void DirectAdd(Action handler)
        {
            this.directValue += handler;
        }

        protected override void DirectRemove(Action handler)
        {
            this.directValue -= handler;
        }

        protected override string GetHandlerMethod(Action handler)
        {
            return handler.Method.Name;
        }

        protected override object GetHandlerTarget(Action handler)
        {
            return handler.Target;
        }
    }

    [Serializable]
    public class uAction<T0> : uDelegate<Action<T0>>
    {
        public override Type[] ParamTypes
        {
            get { return new[] { typeof(T0) }; }
        }

        public override Type ReturnType
        {
            get { return typeof(void); }
        }

        public void Invoke(T0 arg0)
        {
            this.Value.SafeInvoke(arg0);
        }

        protected override void DirectAdd(Action<T0> handler)
        {
            this.directValue += handler;
        }

        protected override void DirectRemove(Action<T0> handler)
        {
            this.directValue -= handler;
        }

        protected override string GetHandlerMethod(Action<T0> handler)
        {
            return handler.Method.Name;
        }

        protected override object GetHandlerTarget(Action<T0> handler)
        {
            return handler.Target;
        }
    }

    [Serializable]
    public class uAction<T0, T1> : uDelegate<Action<T0, T1>>
    {
        public override Type[] ParamTypes
        {
            get { return new[] { typeof(T0), typeof(T1) }; }
        }

        public override Type ReturnType
        {
            get { return typeof(void); }
        }

        public void Invoke(T0 arg0, T1 arg1)
        {
            this.Value.SafeInvoke(arg0, arg1);
        }

        protected override void DirectAdd(Action<T0, T1> handler)
        {
            this.directValue += handler;
        }

        protected override void DirectRemove(Action<T0, T1> handler)
        {
            this.directValue -= handler;
        }

        protected override string GetHandlerMethod(Action<T0, T1> handler)
        {
            return handler.Method.Name;
        }

        protected override object GetHandlerTarget(Action<T0, T1> handler)
        {
            return handler.Target;
        }
    }

    [Serializable]
    public class uAction<T0, T1, T2> : uDelegate<Action<T0, T1, T2>>
    {
        public override Type[] ParamTypes
        {
            get { return new[] { typeof(T0), typeof(T1), typeof(T2) }; }
        }

        public override Type ReturnType
        {
            get { return typeof(void); }
        }

        public void Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            this.Value.SafeInvoke(arg0, arg1, arg2);
        }

        protected override void DirectAdd(Action<T0, T1, T2> handler)
        {
            this.directValue += handler;
        }

        protected override void DirectRemove(Action<T0, T1, T2> handler)
        {
            this.directValue -= handler;
        }

        protected override string GetHandlerMethod(Action<T0, T1, T2> handler)
        {
            return handler.Method.Name;
        }

        protected override object GetHandlerTarget(Action<T0, T1, T2> handler)
        {
            return handler.Target;
        }
    }
}