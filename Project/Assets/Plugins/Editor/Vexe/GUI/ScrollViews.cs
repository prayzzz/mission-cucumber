using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public void _beginScroll(ref Vector2 pos)
        {
            this._beginScroll(ref pos, (GUILayoutOption[])null);
        }
        public void _beginScroll(ref Vector2 pos, params GUILayoutOption[] option)
        {
            this._beginScroll(ref pos, UnityEngine.GUI.skin.scrollView, option);
        }
        public void _beginScroll(ref Vector2 pos, GUIStyle background, params GUILayoutOption[] option)
        {
            this._beginScroll(ref pos, false, false, UnityEngine.GUI.skin.horizontalScrollbar, UnityEngine.GUI.skin.verticalScrollbar, background, option);
        }
        public void _beginScroll(ref Vector2 pos, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] option)
        {
            this._beginScroll(ref pos, alwaysShowHorizontal, alwaysShowVertical, UnityEngine.GUI.skin.horizontalScrollbar, UnityEngine.GUI.skin.verticalScrollbar, UnityEngine.GUI.skin.scrollView, option);
        }
        public void _beginScroll(ref Vector2 pos, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] option)
        {
            this._beginScroll(ref pos, false, false, horizontalScrollbar, verticalScrollbar, UnityEngine.GUI.skin.scrollView, option);
        }
        public void _beginScroll(ref Vector2 pos, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] option)
        {
            pos = GUILayout.BeginScrollView(pos, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, option);
        }
        public void _endScroll()
        {
            GUILayout.EndScrollView();
        }
    }
}