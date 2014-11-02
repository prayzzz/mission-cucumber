using System;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.Drawers.API.Core
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
            this.dmValue = this.GetField().Invoke(this.niceName, this.dmValue);
        }
    }

    public class IntDrawer : BasicDrawer<int>
    {
        protected override Func<string, int, int> GetField()
        {
            return this.gui.IntField;
        }
    }

    public class FloatDrawer : BasicDrawer<float>
    {
        protected override Func<string, float, float> GetField()
        {
            return this.gui.FloatField;
        }
    }

    public class StringDrawer : BasicDrawer<string>
    {
        public override void OnGUI()
        {
            this.dmValue = EditorGUILayout.TextField(this.niceName, this.dmValue);
            //dmValue = gui.TextField(niceName, dmValue, new GUILayoutOption[0]);
        }
        //protected override Func<string, string, string> GetField()
        //{
        //    return gui.TextField;
        //}
    }

    public class Vector2Drawer : BasicDrawer<Vector2>
    {
        protected override Func<string, Vector2, Vector2> GetField()
        {
            return this.gui.Vector2Field;
        }
    }

    public class Vector3Drawer : BasicDrawer<Vector3>
    {
        protected override Func<string, Vector3, Vector3> GetField()
        {
            return this.gui.Vector3Field;
        }
    }

    public class BoolDrawer : BasicDrawer<bool>
    {
        protected override Func<string, bool, bool> GetField()
        {
            return this.gui.Toggle;
        }
    }

    public class ColorDrawer : BasicDrawer<Color>
    {
        protected override Func<string, Color, Color> GetField()
        {
            return this.gui.ColorField;
        }
    }

    public class BoundsDrawer : BasicDrawer<Bounds>
    {
        protected override Func<string, Bounds, Bounds> GetField()
        {
            return this.gui.BoundsField;
        }
    }

    public class RectDrawer : BasicDrawer<Rect>
    {
        protected override Func<string, Rect, Rect> GetField()
        {
            return this.gui.RectField;
        }
    }

    public class QuaternionDrawer : BasicDrawer<Quaternion>
    {
        protected override Func<string, Quaternion, Quaternion> GetField()
        {
            return this.gui.QuaternionField;
        }
    }

    public class UnityObjectDrawer : BasicDrawer<Object>
    {
        public override void OnGUI()
        {
            this.dmValue = this.gui.ObjectField(this.niceName, this.dataMember.Value, this.dataMember.Type);
        }
    }

    public class LayerMaskDrawer : BasicDrawer<LayerMask>
    {
        public override void OnGUI()
        {
            this.dmValue = this.gui.MaskField(this.niceName, this.dataMember.Get(), UnityEditorInternal.InternalEditorUtility.layers);
        }
    }

    public class EnumDrawer : BasicDrawer<Enum>
    {
        protected override Func<string, Enum, Enum> GetField()
        {
            return this.gui.EnumPopup;
        }
    }
}