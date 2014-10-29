using System;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.Drawers
{
	[CoreDrawer]
	public abstract class BasicDrawer<T> : ObjectDrawer<T>
	{
		protected virtual Func<string, T, T> GetField()
		{
			throw new NotImplementedException();
		}

		public override void OnGUI()
		{
			dmValue = GetField().Invoke(niceName, dmValue);
		}
	}

	public class IntDrawer : BasicDrawer<int>
	{
		protected override Func<string, int, int> GetField()
		{
			return gui.IntField;
		}
	}

	public class FloatDrawer : BasicDrawer<float>
	{
		protected override Func<string, float, float> GetField()
		{
			return gui.FloatField;
		}
	}

	public class StringDrawer : BasicDrawer<string>
	{
		public override void OnGUI()
		{
			dmValue = EditorGUILayout.TextField(niceName, dmValue);
			//dmValue = gui.TextField(niceName, dmValue, new GUILayoutOption[0]);
		}
		//protected override Func<string, string, string> GetField()
		//{
		//	return gui.TextField;
		//}
	}

	public class Vector2Drawer : BasicDrawer<Vector2>
	{
		protected override Func<string, Vector2, Vector2> GetField()
		{
			return gui.Vector2Field;
		}
	}

	public class Vector3Drawer : BasicDrawer<Vector3>
	{
		protected override Func<string, Vector3, Vector3> GetField()
		{
			return gui.Vector3Field;
		}
	}

	public class BoolDrawer : BasicDrawer<bool>
	{
		protected override Func<string, bool, bool> GetField()
		{
			return gui.Toggle;
		}
	}

	public class ColorDrawer : BasicDrawer<Color>
	{
		protected override Func<string, Color, Color> GetField()
		{
			return gui.ColorField;
		}
	}

	public class BoundsDrawer : BasicDrawer<Bounds>
	{
		protected override Func<string, Bounds, Bounds> GetField()
		{
			return gui.BoundsField;
		}
	}

	public class RectDrawer : BasicDrawer<Rect>
	{
		protected override Func<string, Rect, Rect> GetField()
		{
			return gui.RectField;
		}
	}

	public class QuaternionDrawer : BasicDrawer<Quaternion>
	{
		protected override Func<string, Quaternion, Quaternion> GetField()
		{
			return gui.QuaternionField;
		}
	}

	public class UnityObjectDrawer : BasicDrawer<UnityObject>
	{
		public override void OnGUI()
		{
			dmValue = gui.ObjectField(niceName, dataMember.Value, dataMember.Type);
		}
	}

	public class LayerMaskDrawer : BasicDrawer<LayerMask>
	{
		public override void OnGUI()
		{
			dmValue = gui.MaskField(niceName, dataMember.Get(), UnityEditorInternal.InternalEditorUtility.layers);
		}
	}

	public class EnumDrawer : BasicDrawer<Enum>
	{
		protected override Func<string, Enum, Enum> GetField()
		{
			return gui.EnumPopup;
		}
	}
}