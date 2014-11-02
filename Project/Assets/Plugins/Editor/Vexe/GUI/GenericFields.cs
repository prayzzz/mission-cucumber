using System;

using Assets.Plugins.Editor.Vexe.Other;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        [Obsolete("Use MemberField instead")]
        public T GenericField<T>(T value)
        {
            return this.GenericField("", value);
        }

        [Obsolete("Use MemberField instead")]
        public T GenericField<T>(string label, T value)
        {
            return (T)this.GenericField(label, value, value == null ? typeof(T) : value.GetType());
        }

        [Obsolete("Use MemberField instead")]
        public object GenericField(string label, object value, Type valueType)
        {
            if (valueType == typeof(int))
            {
                return this.IntField(label, (int)value);
            }
            else if (valueType == typeof(float))
            {
                return this.FloatField(label, (float)value);
            }
            else if (valueType == typeof(bool))
            {
                return this.Toggle(label, (bool)value);
            }
            else if (valueType == typeof(string))
            {
                return this.TextField(label, (string)value);
            }
            else if (valueType == typeof(Vector3))
            {
                return this.Vector3Field(label, (Vector3)value);
            }
            else if (valueType == typeof(Vector2))
            {
                return this.Vector2Field(label, (Vector2)value);
            }
            else if (valueType == typeof(Bounds))
            {
                return this.BoundsField(label, (Bounds)value);
            }
            else if (valueType == typeof(Rect))
            {
                return this.RectField(label, (Rect)value);
            }
            else if (valueType == typeof(Color))
            {
                return this.ColorField(label, (Color)value);
            }
            else if (typeof(Object).IsAssignableFrom(valueType))
            {
                return this.ObjectField(label, value as Object, valueType);
            }
            else if (valueType == typeof(Quaternion))
            {
                return Quaternion.Euler(this.Vector3Field(label, ((Quaternion)value).eulerAngles));
            }
            else
            {
                this._beginColor(GUIHelper.RedColorDuo.FirstColor);
                {
                    this.HelpBox("Type `" + valueType.Name + "` is not supported", MessageType.Error);
                }
                this._endColor();
            }
            return null;
        }
    }
}
