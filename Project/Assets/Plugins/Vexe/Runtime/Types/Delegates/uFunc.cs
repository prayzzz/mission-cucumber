using System;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Vexe.Runtime.Types.Delegates
{
    [Serializable]
    public class uFunc<TReturn> : uDelegate<Func<TReturn>>
    {
        public override Type[] ParamTypes
        {
            get { return Type.EmptyTypes; }
        }

        public override Type ReturnType
        {
            get { return typeof(TReturn); }
        }

        public TReturn Invoke()
        {
            return this.Value.SafeInvoke();
        }

        protected override void DirectAdd(Func<TReturn> handler)
        {
            this.directValue += handler;
        }

        protected override void DirectRemove(Func<TReturn> handler)
        {
            this.directValue -= handler;
        }

        protected override string GetHandlerMethod(Func<TReturn> handler)
        {
            return handler.Method.Name;
        }

        protected override object GetHandlerTarget(Func<TReturn> handler)
        {
            return handler.Target;
        }
    }

    [Serializable]
    public class uFunc<T0, TReturn> : uDelegate<Func<T0, TReturn>>
    {
        public override Type[] ParamTypes
        {
            get { return new[] { typeof(T0) }; }
        }

        public override Type ReturnType
        {
            get { return typeof(TReturn); }
        }

        public TReturn Invoke(T0 arg0)
        {
            return this.Value.SafeInvoke(arg0);
        }

        protected override void DirectAdd(Func<T0, TReturn> handler)
        {
            this.directValue += handler;
        }

        protected override void DirectRemove(Func<T0, TReturn> handler)
        {
            this.directValue -= handler;
        }

        protected override string GetHandlerMethod(Func<T0, TReturn> handler)
        {
            return handler.Method.Name;
        }

        protected override object GetHandlerTarget(Func<T0, TReturn> handler)
        {
            return handler.Target;
        }
    }

    [Serializable]
    public class uFunc<T0, T1, TReturn> : uDelegate<Func<T0, T1, TReturn>>
    {
        public override Type[] ParamTypes
        {
            get { return new[] { typeof(T0), typeof(T1) }; }
        }

        public override Type ReturnType
        {
            get { return typeof(TReturn); }
        }

        public TReturn Invoke(T0 arg0, T1 arg1)
        {
            return this.Value.SafeInvoke(arg0, arg1);
        }

        protected override void DirectAdd(Func<T0, T1, TReturn> handler)
        {
            this.directValue += handler;
        }

        protected override void DirectRemove(Func<T0, T1, TReturn> handler)
        {
            this.directValue -= handler;
        }

        protected override string GetHandlerMethod(Func<T0, T1, TReturn> handler)
        {
            return handler.Method.Name;
        }

        protected override object GetHandlerTarget(Func<T0, T1, TReturn> handler)
        {
            return handler.Target;
        }
    }

    [Serializable]
    public class uFunc<T0, T1, T2, TReturn> : uDelegate<Func<T0, T1, T2, TReturn>>
    {
        public override Type[] ParamTypes
        {
            get { return new[] { typeof(T0), typeof(T1), typeof(T2) }; }
        }

        public override Type ReturnType
        {
            get { return typeof(TReturn); }
        }

        public TReturn Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            return this.Value.SafeInvoke(arg0, arg1, arg2);
        }

        protected override void DirectAdd(Func<T0, T1, T2, TReturn> handler)
        {
            this.directValue += handler;
        }

        protected override void DirectRemove(Func<T0, T1, T2, TReturn> handler)
        {
            this.directValue -= handler;
        }

        protected override string GetHandlerMethod(Func<T0, T1, T2, TReturn> handler)
        {
            return handler.Method.Name;
        }

        protected override object GetHandlerTarget(Func<T0, T1, T2, TReturn> handler)
        {
            return handler.Target;
        }
    }
}