using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public void _beginScroll(ref Vector2 pos)
		{
			_beginScroll(ref pos, (GUILayoutOption[])null);
		}
		public void _beginScroll(ref Vector2 pos, params GUILayoutOption[] option)
		{
			_beginScroll(ref pos, GUI.skin.scrollView, option);
		}
		public void _beginScroll(ref Vector2 pos, GUIStyle background, params GUILayoutOption[] option)
		{
			_beginScroll(ref pos, false, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, background, option);
		}
		public void _beginScroll(ref Vector2 pos, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] option)
		{
			_beginScroll(ref pos, alwaysShowHorizontal, alwaysShowVertical, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, option);
		}
		public void _beginScroll(ref Vector2 pos, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] option)
		{
			_beginScroll(ref pos, false, false, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, option);
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