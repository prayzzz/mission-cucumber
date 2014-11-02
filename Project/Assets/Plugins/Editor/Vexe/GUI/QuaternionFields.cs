using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public Quaternion QuaternionField(Quaternion value)
        {
            return this.QuaternionField("", value);
        }

        public Quaternion QuaternionField(string label, Quaternion value)
        {
            return this.QuaternionField(label, value, null);
        }

        public Quaternion QuaternionField(string label, string tooltip, Quaternion value)
        {
            return this.QuaternionField(label, tooltip, value, null);
        }

        public Quaternion QuaternionField(string label, Quaternion value, params GUILayoutOption[] option)
        {
            return this.QuaternionField(label, "", value, option);
        }

        public Quaternion QuaternionField(string label, string tooltip, Quaternion value, params GUILayoutOption[] option)
        {
            return this.QuaternionField(GetContent(label, tooltip), value, option);
        }

        public Quaternion QuaternionField(GUIContent content, Quaternion value, params GUILayoutOption[] option)
        {
            return Quaternion.Euler(this.Vector3Field(content, value.eulerAngles, option));
        }
    }
}
