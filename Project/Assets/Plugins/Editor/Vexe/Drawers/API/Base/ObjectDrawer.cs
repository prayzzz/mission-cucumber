using UnityEditor;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public abstract class ObjectDrawer<T> : BaseDrawer
	{
		protected DataMember<T> dataMember { private set; get; }

		protected T dmValue
		{
			get { return dataMember.Value; }
			set { dataMember.Value = value; }
		}

		protected override void InternalInitialize()
		{
			dbgLog("Undo target: " + unityTarget);
			dataMember = DataMember.Create<T>(memberInfo, rawTarget, RegisterDirty, RegisterUndo);
		}

		public void MemberField()
		{
			MemberField(dataMember);
		}

		public void MemberField<TMemberType>(DataMember<TMemberType> dm)
		{
			gui.MemberField(dm, unityTarget, id);
		}

		public sealed override void OnLeftGUI()
		{
		}
		public sealed override void OnRightGUI()
		{
		}
		public sealed override void OnUpperGUI()
		{
		}
		public sealed override void OnLowerGUI()
		{
		}
		public sealed override void OnMemberDrawn(Rect area)
		{
		}
	}
}