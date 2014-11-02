using System.Collections.Generic;

using Assets.Plugins.Editor.Vexe.Other;

using UnityEngine;

using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public enum MiniButtonStyle { Left, Mid, Right, ModLeft, ModMid, ModRight }

    public partial class GLWrapper
    {
        public const MiniButtonStyle DefaultMiniStyle = MiniButtonStyle.Mid;
        public const MiniButtonStyle DefaultModStyle = MiniButtonStyle.ModMid;
        public static GUILayoutOption[] DefaultMiniOption;
        private static GUIStyle DefaultButtonStyle { get { return UnityEngine.GUI.skin.button; } }

        static GLWrapper()
        {
            DefaultMiniOption = new GUILayoutOption[2]
            {
                GUILayout.Width(GUIConstants.kDefaultMiniWidth),
                GUILayout.Height(GUIConstants.kDefaultMiniHeight)
            };
        }

        public bool Button(string text)
        {
            return this.Button(text, "");
        }
        public bool Button(string text, GUIStyle style)
        {
            return this.Button(text, "", style);
        }

        public bool Button(string text, GUIStyle style, params GUILayoutOption[] option)
        {
            return this.Button(GetContent(text), style, option);
        }

        public bool Button(string text, params GUILayoutOption[] option)
        {
            return this.Button(text, "", option);
        }

        public bool Button(string text, string tooltip)
        {
            return this.Button(text, tooltip, (GUILayoutOption[])null);
        }

        public bool Button(string text, string tooltip, params GUILayoutOption[] option)
        {
            return this.Button(text, tooltip, DefaultButtonStyle, option);
        }

        public bool Button(string text, string tooltip, GUIStyle style)
        {
            return this.Button(text, tooltip, style, null);
        }

        public bool Button(string text, string tooltip, GUIStyle style, params GUILayoutOption[] option)
        {
            return this.Button(GetContent(text, tooltip), style, option);
        }

        public bool Button(GUIContent content)
        {
            return this.Button(content, DefaultButtonStyle, null);
        }

        public bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] option)
        {
            return GUILayout.Button(content, style, option);
        }

        public bool InvisibleButton(params GUILayoutOption[] option)
        {
            return this.Button(GUIContent.none, GUIStyle.none, option);
        }

        public bool InvisibleButton()
        {
            return this.InvisibleButton(null);
        }

        public bool MiniButton(string text)
        {
            return this.MiniButton(text, "");
        }
        public bool MiniButton(string text, string tooltip)
        {
            return this.MiniButton(text, tooltip, DefaultMiniStyle);
        }
        public bool MiniButton(string text, MiniButtonStyle style)
        {
            return this.MiniButton(text, style, DefaultMiniOption);
        }
        public bool MiniButton(string text, params GUILayoutOption[] option)
        {
            return this.MiniButton(text, DefaultMiniStyle, option);
        }

        public bool MiniButton(string text, MiniButtonStyle style, params GUILayoutOption[] option)
        {
            return this.MiniButton(text, "", style, option);
        }
        public bool MiniButton(string text, string tooltip, MiniButtonStyle style)
        {
            return this.MiniButton(text, tooltip, style, DefaultMiniOption);
        }
        public bool MiniButton(string text, string tooltip, MiniButtonStyle style, params GUILayoutOption[] option)
        {
            return this.MiniButton(GetContent(text, tooltip), style, option);
        }
        public bool MiniButton(GUIContent content, MiniButtonStyle style)
        {
            return this.MiniButton(content, style, DefaultMiniOption);
        }
        public bool MiniButton(GUIContent content, MiniButtonStyle style, params GUILayoutOption[] option)
        {
            return this.MiniButton(content, GUIHelper.GetStyle(style), option);
        }
        public bool MiniButton(string text, string tooltip, GUIStyle style, params GUILayoutOption[] option)
        {
            return this.MiniButton(GetContent(text, tooltip), style, option);
        }
        public bool MiniButton(string text, string tooltip, GUIStyle style)
        {
            return this.MiniButton(GetContent(text, tooltip), style, DefaultMiniOption);
        }
        public bool MiniButton(string text, string tooltip, params GUILayoutOption[] option)
        {
            if (option == null)
                return this.MiniButton(GetContent(text, tooltip), GUIHelper.GetStyle(DefaultMiniStyle), GUILayout.Height(GUIConstants.kDefaultMiniHeight));
            return this.MiniButton(GetContent(text, tooltip), GUIHelper.GetStyle(DefaultMiniStyle), option);
        }
        public bool MiniButton(GUIContent content, GUIStyle style, params GUILayoutOption[] option)
        {
            return this.Button(content, style, option);
        }

        public bool AddButton(string target)
        {
            return this.AddButton(target, DefaultModStyle);
        }
        public bool AddButton(string target, MiniButtonStyle style)
        {
            return this.MiniButton("+", "Add a new " + target, style);
        }

        public bool ClearButton(string target)
        {
            return this.ClearButton(target, DefaultModStyle);
        }
        public bool ClearButton(string target, MiniButtonStyle style)
        {
            return this.MiniButton("x", "Clear " + target, style);
        }

        public bool RemoveButton(string target)
        {
            return this.RemoveButton(target, DefaultModStyle);
        }
        public bool RemoveButton(string target, MiniButtonStyle style)
        {
            return this.MiniButton("-", "Remove " + target, style);
        }

        public bool MoveUpButton(MiniButtonStyle style)
        {
            return this.MiniButton("▲", "Move element up", style);
        }
        public bool MoveUpButton()
        {
            return this.MoveUpButton(DefaultMiniStyle);
        }
        public bool MoveUpButton<T>(List<T> list, int atIndex, MiniButtonStyle style)
        {
            var move = this.MoveUpButton(style);
            if (move)
                list.MoveElementUp(atIndex);
            return move;
        }
        public bool MoveUpButton<T>(List<T> list, int atIndex)
        {
            return this.MoveUpButton(list, atIndex, DefaultMiniStyle);
        }

        public bool MoveDownButton(MiniButtonStyle style)
        {
            return this.MiniButton("▼", "Move element down", style);
        }
        public bool MoveDownButton()
        {
            return this.MoveDownButton(DefaultMiniStyle);
        }
        public bool MoveDownButton<T>(List<T> list, int atIndex, MiniButtonStyle style)
        {
            var move = this.MoveDownButton(style);
            if (move)
                list.MoveElementDown(atIndex);
            return move;
        }

        public bool MoveDownButton<T>(List<T> list, int atIndex)
        {
            return this.MoveDownButton(list, atIndex, DefaultMiniStyle);
        }

        public bool ToggleButton(bool value, string on, string off, string whatToToggle, MiniButtonStyle style = MiniButtonStyle.Mid)
        {
            var btn = this.MiniButton(value ? on : off, "Toggle " + whatToToggle, style);
            return btn ? !value : value;
        }

        public bool FoldoutButton(bool value, string whatToFoldout, MiniButtonStyle style = MiniButtonStyle.Mid)
        {
            return this.ToggleButton(value, GUIHelper.Folds.DefaultFoldSymbol, GUIHelper.Folds.DefaultExpandSymbol, whatToFoldout, style);
        }

        public bool CheckButton(bool value, string whatToToggle)
        {
            return this.CheckButton(value, whatToToggle, DefaultMiniStyle);
        }

        public bool CheckButton(bool value, string whatToToggle, MiniButtonStyle style)
        {
            return this.ToggleButton(value, "✔", "", whatToToggle, style);
        }

        public bool RefreshButton(params GUILayoutOption[] option)
        {
            return this.Button("↶", "Refresh", GUIHelper.RefreshButtonStyle, option);
        }

        public bool RefreshButton()
        {
            return this.RefreshButton(DefaultMiniOption);
        }

        public bool SelectionButton(string whatToSelect, params GUILayoutOption[] option)
        {
            return this.Button("◉", "Select " + whatToSelect, GUIHelper.SelectionButtonStyle, option);
        }

        public bool SelectionButton(string whatToSelect)
        {
            return this.SelectionButton(whatToSelect, DefaultMiniOption);
        }

        public bool SelectionButton()
        {
            return this.SelectionButton("");
        }

        public bool InspectButton(GameObject go, MiniButtonStyle style, params GUILayoutOption[] option)
        {
            var inspect = this.MiniButton("⏎", "Inspect", style, option);
            if (inspect) EditorHelper.InspectTarget(go);
            return inspect;
        }

        public bool InspectButton(GameObject go, MiniButtonStyle style)
        {
            return this.InspectButton(go, style, DefaultMiniOption);
        }

        public bool InspectButton(GameObject go, params GUILayoutOption[] option)
        {
            return this.InspectButton(go, DefaultMiniStyle, option);
        }

        public bool InspectButton(GameObject go)
        {
            return this.InspectButton(go, DefaultMiniStyle, DefaultMiniOption);
        }
    }
}
