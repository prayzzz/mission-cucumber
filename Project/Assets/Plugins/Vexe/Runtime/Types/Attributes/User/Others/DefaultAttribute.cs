using System;

using UnityEngine;

using Vexe.Runtime.Helpers;
using Vexe.Runtime.Types;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others
{
    /// <summary>
    /// Annotate ints, floats, bools, strings, enums, Vector2/3s, Colors to give them default values
    /// (this value is assigned only once to your object when it's first created)
    /// Only arrays of simple types are supported (not lists)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DefaultAttribute : Attribute
    {
        public readonly int? intValue;
        public readonly float? floatValue;
        public readonly bool? boolValue;
        public readonly Vector3? vector3Value;
        public readonly Vector2? vector2Value;
        public readonly Color? colorValue;
        public readonly string stringValue;
        public int[] intArray;
        public float[] floatArray;
        public bool[] boolArray;
        public string[] stringArray;
        public int Enum { get; set; }

        public DefaultAttribute(bool[] boolArray)
        {
            this.boolArray = boolArray;
        }

        public DefaultAttribute(string[] stringArray)
        {
            this.stringArray = stringArray;
        }

        public DefaultAttribute(float[] floatArray)
        {
            this.floatArray = floatArray;
        }

        public DefaultAttribute(int[] intArray)
        {
            this.intArray = intArray;
        }

        public DefaultAttribute(int intValue)
        {
            this.intValue = intValue;
        }

        public DefaultAttribute(float floatValue)
        {
            this.floatValue = floatValue;
        }

        public DefaultAttribute(string stringValue)
        {
            this.stringValue = stringValue;
        }

        public DefaultAttribute(bool boolValue)
        {
            this.boolValue = boolValue;
        }

        public DefaultAttribute(float x, float y, float z)
        {
            this.vector3Value = new Vector3(x, y, z);
        }

        public DefaultAttribute(float x, float y)
        {
            this.vector2Value = new Vector2(x, y);
        }

        public DefaultAttribute(float r, float g, float b, float a)
        {
            this.colorValue = new Color(r, g, b, a);
        }

        public DefaultAttribute(Colors color)
        {
            this.colorValue = ColorHelper.FromEnum(color);
        }

        public DefaultAttribute()
        {
            this.Enum = -1;
        }

        public object Value
        {
            get
            {
                if (this.intValue.HasValue)        return this.intValue.Value;
                if (this.floatValue.HasValue)    return this.floatValue.Value;
                if (this.boolValue.HasValue)        return this.boolValue.Value;
                if (this.colorValue.HasValue)    return this.colorValue.Value;
                if (this.vector2Value.HasValue) return this.vector2Value.Value;
                if (this.vector3Value.HasValue) return this.vector3Value.Value;
                if (this.stringValue != null)    return this.stringValue;
                if (this.intArray != null)        return this.intArray;
                if (this.floatArray != null)        return this.floatArray;
                if (this.boolArray != null)        return this.boolArray;
                if (this.stringArray != null)    return this.stringArray;
                if (this.Enum != -1)                return this.Enum;
                throw new NotSupportedException("Value type not supported");
            }
        }
    }
}