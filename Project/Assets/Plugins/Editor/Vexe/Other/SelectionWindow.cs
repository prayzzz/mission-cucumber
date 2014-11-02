using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Assets.Plugins.Editor.Vexe.GUI;

using UnityEditor;

using UnityEngine;

using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types.GUI;

#pragma warning disable 0414

namespace Assets.Plugins.Editor.Vexe.Other
{
    public class SelectionWindow : EditorWindow
    {
        public static float IndentWidth        { get; set; }
        public static GUIStyle BackgroundStyle { get; set; }
        public static Color TabColor           { get; set; }

        static SelectionWindow()
        {
            IndentWidth     = 15f;
            BackgroundStyle = GUIHelper.DarkGreyStyleDuo.SecondStyle;
            TabColor        = GUIHelper.LightBlueColorDuo.SecondColor;
        }

        private bool close;
        private Vector2 scroll;
        private string search;
        private Tab[] tabs;
        private Action onClose;
        private Tab currentTab;
        private GLWrapper gui;
        private static int lastTabIndex;
        private bool firstTime;

        private void Initialize(Tab[] tabs, Action onClose)
        {
            this.gui = new GLWrapper();
            this.onClose = onClose;
            this.tabs = tabs;

            for (int i = 0; i < tabs.Length; i++)
            {
                var t = tabs[i];
                t.gui = this.gui;
                t.selectionStyle = GUIHelper.SelectionRect;
            }
            this.currentTab = lastTabIndex >= tabs.Length ? tabs[0] : tabs[lastTabIndex];
            this.search = "";
        }

        public void OnGUI()
        {
            this.gui._beginH(EditorStyles.toolbar);
            {
                for (int i = 0; i < this.tabs.Length; i++)
                {
                    var tab = this.tabs[i];
                    this.gui._beginColor(tab == this.currentTab ? TabColor : (Color?)null);
                    {
                        if (this.gui.Button(tab.title, EditorStyles.toolbarButton))
                        {
                            this.currentTab = tab;
                            this.currentTab.Refresh();
                            lastTabIndex = i;
                        }
                    }
                    this.gui._endColor();
                }
                this.gui.FlexibleSpace();
            }
            this.gui._endH();

            this.gui.Space(3f);
            UnityEngine.GUI.SetNextControlName("SearchBox");
            this.search = this.gui.ToolbarSearchField(this.search);
            if (Event.current != null && Event.current.isKey && Event.current.keyCode == KeyCode.Tab)
            {
                Debug.Log(UnityEngine.GUI.GetNameOfFocusedControl());
                UnityEngine.GUI.FocusControl("SearchBox");
                Debug.Log(UnityEngine.GUI.GetNameOfFocusedControl());
            }
            this.gui.Splitter();
            this.gui._beginScroll(ref this.scroll, BackgroundStyle);
            {
                this.currentTab.OnGUI(this.search, this.maxSize.x - this.minSize.x);
            }
            this.gui._endScroll();
        }

        public void OnFocus()
        {
            if (this.currentTab != null)
                this.currentTab.Refresh();
        }

        private void Update()
        {
            if (this.close) this.Close(); // Can't close out immediately in OnLostFocus
        }

        public static void Show(string title, Action onClose, params Tab[] tabs)
        {
            GetWindow<SelectionWindow>(true, title).Initialize(tabs, onClose);
        }

        public static void Show(string title, params Tab[] tabs)
        {
            Show(title, null, tabs);
        }

        public static void Show(Action onClose, params Tab[] tabs)
        {
            Show(string.Empty, onClose, tabs);
        }

        public static void Show(params Tab[] tabs)
        {
            Show(string.Empty, null, tabs);
        }

        public void CleanUp()
        {
            GUIHelper.DarkGreyStyleDuo.DestroyTextures();
        }

        private void OnLostFocus()
        {
            this.CleanUp();
            this.close = true;
            this.onClose.SafeInvoke();
        }
    }

    public abstract class Tab
    {
        public string title { set; get; }
        public GLWrapper gui { get; set; }
        public GUIStyle selectionStyle { get; set; }

        public virtual void Refresh()
        {
        }

        public abstract void OnGUI(string search, float width);
    }

    public class Tab<T> : Tab
    {
        private readonly Func<T[]> getValues;
        private readonly Action<T> setTarget;
        private readonly Func<T, string> getValueName;
        private readonly Func<T> getCurrent;
        private readonly Func<T, StyleDuo> getStyleDuo;
        private readonly T defaultValue;
        private string previousSearch;
        private T[] values;
        private List<T> filteredValues;

        public Tab(Func<T[]> getValues, Func<T> getCurrent, Action<T> setTarget, Func<T, string> getValueName, Func<T, StyleDuo> getStyleDuo, string title)
        {
            this.getValues    = getValues;
            this.setTarget    = setTarget;
            this.getValueName = getValueName;
            this.getCurrent   = getCurrent;
            this.getStyleDuo  = getStyleDuo ?? (x => GUIHelper.DarkGreyStyleDuo);
            this.title        = title;
            this.defaultValue      = (T)typeof(T).GetDefaultValue();
        }

        public Tab(Func<T[]> getValues, Func<T> getCurrent, Action<T> setTarget, Func<T, string> getValueName, string title)
            : this(getValues, getCurrent, setTarget, getValueName, null, title)
        {
        }

        public override void OnGUI(string search, float width)
        {
            // Default value
            {
                var isDefault = this.defaultValue.GenericEqual(this.getCurrent());
                this.gui._beginColor(GUIHelper.RedColorDuo.FirstColor);
                {
                    this.OnValueGUI(this.defaultValue,
                        string.Format("Default ({0})", typeof(T).IsClass ? "Null" : this.defaultValue.ToString()),
                        width, isDefault,
                        isDefault ? this.selectionStyle : this.getStyleDuo(this.defaultValue).NextStyle,
                        isDefault ? EditorStyles.whiteLargeLabel : EditorStyles.whiteLargeLabel);
                }
                this.gui._endColor();
            }

            if (this.values == null) this.Refresh();
            if (this.values == null) return;

            if (this.previousSearch != search)
            {
                this.previousSearch = search;
                this.filteredValues = this.values.Where(x => Regex.IsMatch(this.getValueName(x), search, RegexOptions.IgnoreCase))
                                              .ToList();
            }

            for (int i = 0; i < this.filteredValues.Count; i++)
            {
                var value      = this.filteredValues[i];
                var isSelected = value.GenericEqual(this.getCurrent());
                var nextStyle  = this.getStyleDuo(value).NextStyle;

                this.OnValueGUI(value, this.getValueName(value), width, isSelected, nextStyle,
                    isSelected ? EditorStyles.whiteBoldLabel : EditorStyles.whiteLabel);
            }
        }

        private void OnValueGUI(T value, string itemName, float width, bool isSelected, GUIStyle nextStyle, GUIStyle labelStyle)
        {
            this.gui._beginH(isSelected ? this.selectionStyle : nextStyle);
            {
                this.gui.Space(SelectionWindow.IndentWidth);
                this.gui.Label(itemName, labelStyle);
                var rect = this.gui.GetLastRect();
                {
                    rect.width = width;
                    if (!isSelected)
                    {
                        GUIHelper.AddCursorRect(rect, MouseCursor.Link);
                        if (UnityEngine.GUI.Button(rect, GUIContent.none, GUIStyle.none))
                        {
                            this.setTarget(value);
                            this.Refresh();
                        }
                    }
                }
            }
            this.gui._endH();
        }

        public override void Refresh()
        {
            this.values = this.getValues();
        }
    }
}