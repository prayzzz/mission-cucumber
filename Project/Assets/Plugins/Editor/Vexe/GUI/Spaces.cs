using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public void Indent(float indentLevel)
        {
            this.Space(indentLevel *GUIConstants.kIndentAmount);
        }

        public void Indent()
        {
            this.Indent(1f);
        }

        public void Space(float? pixels)
        {
            if (pixels.HasValue)
                this.Space(pixels.Value);
        }

        public void Space(float pixels)
        {
            GUILayout.Space(pixels);
        }

        public void FlexibleSpace()
        {
            GUILayout.FlexibleSpace();
        }
    }
}
