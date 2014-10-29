using System;
using UnityEditor;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public abstract class RequiredFromRelativeAttributeDrawer<T> : BaseRequirementAttributeDrawer<T> where T : RequiredFromRelativeAttribute
	{
		protected abstract string relative { get; }
		protected abstract string plural { get; }

		protected abstract Component GetComponentInRelative(GameObject from, Type componentType);
		protected abstract GameObject GetRelativeAtPath(GameObject from, string path, bool throwIfNotFound);
		protected abstract GameObject GetOrAddRelativeAtPath(GameObject from, string path);

		protected override Component GetComponent(GameObject from, Type componentType)
		{
			Component c = null;
			var path = attribute.Path;
			if (path.IsNullOrEmpty())
			{
				c = GetComponentInRelative(from, componentType);
				if (c == null)
					gui.HelpBox(string.Format("Couldn't find component `{0}` in {1}", componentType.Name, plural), MessageType.Warning);
			}
			else
			{
				Action<GameObject> handleRelative = relative =>
				{
					if (relative == null) return;
					c = relative.GetComponent(componentType);
					if (c == null && attribute.Add)
					{
						if (componentType.IsAbstract)
							gui.HelpBox("Can't add component `" + componentType.Name + "` because it's abstract", MessageType.Warning);
						else
							c = relative.AddComponent(componentType);
					}
				};

				handleRelative(attribute.Create ?
					GetOrAddRelativeAtPath(from, path) :
					GetRelativeAtPath(from, path, false)
				);
			}
			return c;
		}

		protected override GameObject GetGO(GameObject from)
		{
			string path = ProcessPath();
			if (path.IsNullOrEmpty())
			{
				gui.HelpBox(string.Format("No {0} path specified - Can't get {0} GameObject", relative), MessageType.Warning);
				return null;
			}
			try
			{
				return attribute.Create ?
					GetOrAddRelativeAtPath(from, path) :
					GetRelativeAtPath(from, path, true);
			}
			catch
			{
				gui.HelpBox(relative + " not found at the specified path `" + path + "`", MessageType.Warning);
				return null;
			}
		}

		private string ProcessPath()
		{
			string p = attribute.Path;
			if (p == "$Name")
			{
				return memberInfo.Name.SplitPascalCase().Split(' ')[0];
			}
			if (p == "$name")
			{
				return memberInfo.Name.SplitCamelCase().Split(' ')[0];
			}
			if (p == "$fullName")
			{
				return memberInfo.Name;
			}
			if (p == "$FullName")
			{
				return memberInfo.Name.ToUpperAt(0);
			}
			if (p == "$Full Name")
			{
				return memberInfo.Name.SplitPascalCase();
			}
			return p;
		}
	}
}