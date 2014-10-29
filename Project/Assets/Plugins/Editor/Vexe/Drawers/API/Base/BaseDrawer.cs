using System;
using System.Reflection;
using Vexe.Editor.Framework.GUIs;
using UnityEngine;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;
using System.Text;
using UnityEditor;

namespace Vexe.Editor.Framework.Drawers
{
	public abstract class BaseDrawer
	{
		public MemberInfo memberInfo       { private set; get; }
		public Type memberType             { private set; get; }
		public object rawTarget            { private set; get; }
		public UnityObject unityTarget     { private set; get; }
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
			get { return prefs.GetSafeBool(id); }
			set { prefs.SetBool(id, value); }
		}

		protected GameObject gameObject
		{
			get
			{
				var component = unityTarget as Component;
				return component == null ? null : component.gameObject;
			}
		}

		public BaseDrawer Initialize(MemberInfo memberInfo, Attribute[] attributes, object rawTarget, UnityObject unityTarget, GLWrapper gui, string id)
		{
			if (this.rawTarget == rawTarget && this.unityTarget == unityTarget)
				return this;

			dbgLog("Initializing: " + this);

			this.memberInfo  = memberInfo;
			this.attributes  = attributes;
			this.gui         = gui;
			this.rawTarget   = rawTarget;
			this.unityTarget = unityTarget;
			prefs            = BetterEditorPrefs.Instance;
			niceName         = memberInfo.GetNiceName();
			memberType       = memberInfo.GetDataType(DataInfo.Fallback);
			memberTypeName   = memberType.GetFriendlyName();
			targetType       = rawTarget.GetType();

			// id
			{ 
				var builder = new StringBuilder();
				builder.Append(id);
				builder.Append(memberTypeName);
				builder.Append(niceName);
				builder.Append(targetType.Name);
				this.id = builder.ToString();
			}

			InternalInitialize();
			OnInitialized();
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
			foldout = gui.Foldout(foldout);
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
			Undo.RecordObject(unityTarget, "set member");
		}

		protected void RegisterDirty()
		{
			EditorUtility.SetDirty(unityTarget);
			var savable = unityTarget as ISavable;
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