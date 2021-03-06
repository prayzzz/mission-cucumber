﻿using System;

using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public Enum EnumPopup(Enum selected, params GUILayoutOption[] option)
        {
            return this.EnumPopup(selected, EditorStyles.popup, option);
        }
        public Enum EnumPopup(Enum selected, GUIStyle style, params GUILayoutOption[] option)
        {
            return this.EnumPopup(GUIContent.none, selected, style, option);
        }
        public Enum EnumPopup(GUIContent label, Enum selected, params GUILayoutOption[] option)
        {
            return this.EnumPopup(label, selected, EditorStyles.popup, option);
        }
        public Enum EnumPopup(string label, Enum selected)
        {
            return this.EnumPopup(label, selected, (GUILayoutOption[])null);
        }
        public Enum EnumPopup(string label, Enum selected, GUILayoutOption[] option)
        {
            return this.EnumPopup(label, selected, EditorStyles.popup, option);
        }
        public Enum EnumPopup(string label, Enum selected, GUIStyle style, params GUILayoutOption[] option)
        {
            return this.EnumPopup(GetContent(label), selected, style, option);
        }
        public Enum EnumPopup(GUIContent label, Enum selected, GUIStyle style, params GUILayoutOption[] option)
        {
            return EditorGUILayout.EnumPopup(label, selected, style, option);
        }
    }
}
