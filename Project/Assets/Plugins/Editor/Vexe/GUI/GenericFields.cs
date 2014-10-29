using System;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Helpers;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		[Obsolete("Use MemberField instead")]
		public T GenericField<T>(T value)
		{
			return GenericField("", value);
		}

		[Obsolete("Use MemberField instead")]
		public T GenericField<T>(string label, T value)
		{
			return (T)GenericField(label, value, value == null ? typeof(T) : value.GetType());
		}

		[Obsolete("Use MemberField instead")]
		public object GenericField(string label, object value, Type valueType)
		{
			if (valueType == typeof(int))
			{
				return IntField(label, (int)value);
			}
			else if (valueType == typeof(float))
			{
				return FloatField(label, (float)value);
			}
			else if (valueType == typeof(bool))
			{
				return Toggle(label, (bool)value);
			}
			else if (valueType == typeof(string))
			{
				return TextField(label, (string)value);
			}
			else if (valueType == typeof(Vector3))
			{
				return Vector3Field(label, (Vector3)value);
			}
			else if (valueType == typeof(Vector2))
			{
				return Vector2Field(label, (Vector2)value);
			}
			else if (valueType == typeof(Bounds))
			{
				return BoundsField(label, (Bounds)value);
			}
			else if (valueType == typeof(Rect))
			{
				return RectField(label, (Rect)value);
			}
			else if (valueType == typeof(Color))
			{
				return ColorField(label, (Color)value);
			}
			else if (typeof(UnityObject).IsAssignableFrom(valueType))
			{
				return ObjectField(label, value as UnityObject, valueType);
			}
			else if (valueType == typeof(Quaternion))
			{
				return Quaternion.Euler(Vector3Field(label, ((Quaternion)value).eulerAngles));
			}
			else
			{
				_beginColor(GUIHelper.RedColorDuo.FirstColor);
				{
					HelpBox("Type `" + valueType.Name + "` is not supported", MessageType.Error);
				}
				_endColor();
			}
			return null;
		}
	}
}
