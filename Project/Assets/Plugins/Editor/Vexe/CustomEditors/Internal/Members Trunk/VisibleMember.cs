using System;
using System.Reflection;
using Vexe.Editor.Framework.GUIs;
using Vexe.Runtime.Extensions;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.BetterBehaviourInternal
{
	public class VisibleMember : ICanBeDrawn, IEquatable<VisibleMember>
	{
		private readonly MemberInfo info;
		private readonly GLWrapper gui;
		private readonly object rawTarget;
		private readonly UnityObject unityTarget;
		private readonly Type dataType;
		private readonly string key;

		public MemberInfo Info    { get { return info; } }
		public Type DataType      { get { return dataType; } }
		public string Name        { get { return info.Name; } }
		public float DisplayOrder { get; set; }

		public VisibleMember(MemberInfo info, GLWrapper gui, object rawTarget, UnityObject unityTarget, string key)
		{
			this.info        = info;
			this.gui         = gui;
			this.rawTarget   = rawTarget;
			this.unityTarget = unityTarget;
			this.key         = key;
			dataType         = info.GetDataType();
		}

		public void Draw()
		{
			gui.MemberField(info, rawTarget, unityTarget, key, false);
		}

		public void HeaderSpace()
		{
			gui.Space(10f);
		}

		public bool Equals(VisibleMember other)
		{
			return info.Equals(other.info);
		}
	}
}