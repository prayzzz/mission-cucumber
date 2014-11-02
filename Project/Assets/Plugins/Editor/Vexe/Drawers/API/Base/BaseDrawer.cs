using System;
using System.Reflection;
using System.Text;

using Assets.Plugins.Editor.Vexe.GUI;
using Assets.Plugins.Editor.Vexe.Other;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEditor;

using UnityEngine;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Base
{
    public abstract class BaseDrawer
    {
        public MemberInfo memberInfo       { private set; get; }
        public Type memberType             { private set; get; }
        public object rawTarget            { private set; get; }
        public Object unityTarget     { private set; get; }
        protected Type targetType          { private set; get; }
        protected GLWrapper gui            { private set; get; }
        protected BetterEditorPrefs prefs  { private set; get; }
        protected Attribute[] attributes   { private set; get; }
        protected string memberTypeName    { private set; get; }
        protected string id                { private set; get; }
        protected string niceName          { private set; get; }

        public static bool dbg = false;

        protected bool foldout
        {
            get { return this.prefs.GetSafeBool(this.id); }
            set { this.prefs.SetBool(this.id, value); }
        }

        protected GameObject gameObject
        {
            get
            {
                var component = this.unityTarget as Component;
                return component == null ? null : component.gameObject;
            }
        }

        public BaseDrawer Initialize(MemberInfo memberInfo, Attribute[] attributes, object rawTarget, Object unityTarget, GLWrapper gui, string id)
        {
            if (this.rawTarget == rawTarget && this.unityTarget == unityTarget)
                return this;

            dbgLog("Initializing: " + this);

            this.memberInfo  = memberInfo;
            this.attributes  = attributes;
            this.gui         = gui;
            this.rawTarget   = rawTarget;
            this.unityTarget = unityTarget;
            this.prefs            = BetterEditorPrefs.Instance;
            this.niceName         = memberInfo.GetNiceName();
            this.memberType       = memberInfo.GetDataType(DataInfo.Fallback);
            this.memberTypeName   = this.memberType.GetFriendlyName();
            this.targetType       = rawTarget.GetType();

            // id
            { 
                var builder = new StringBuilder();
                builder.Append(id);
                builder.Append(this.memberTypeName);
                builder.Append(this.niceName);
                builder.Append(this.targetType.Name);
                this.id = builder.ToString();
            }

            this.InternalInitialize();
            this.OnInitialized();
            return this;
        }

        protected virtual void InternalInitialize()
        {
        }

        protected virtual void OnInitialized()
        {
        }

        public static BaseDrawer Create(Type drawerType)
        {
            return drawerType.Instance<BaseDrawer>();
        }

        public void Foldout()
        {
            this.foldout = this.gui.Foldout(this.foldout);
        }

        public abstract void OnGUI();
        public virtual void OnUpperGUI() { }
        public virtual void OnLeftGUI() { }
        public virtual void OnMemberDrawn(Rect rect)
        {
        }
        public virtual void OnRightGUI() { }
        public virtual void OnLowerGUI() { }

        protected void RegisterUndo()
        {
            Undo.RecordObject(this.unityTarget, "set member");
        }

        protected void RegisterDirty()
        {
            EditorUtility.SetDirty(this.unityTarget);
            var savable = this.unityTarget as ISavable;
            if (savable != null)
                savable.Save();
        }

        protected static void dbgLogFormat(string msg, params object[] args)
        {
            if (dbg)
                Debug.Log(string.Format(msg, args));
        }

        protected static void dbgLog(object msg)
        {
            if (dbg)
                Debug.Log(msg);
        }

        protected static void LogFormat(string msg, params object[] args)
        {
            Debug.Log(string.Format(msg, args));
        }

        protected static void Log(object msg)
        {
            Debug.Log(msg);
        }
    }
}