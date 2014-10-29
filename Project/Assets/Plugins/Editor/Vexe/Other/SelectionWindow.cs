using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Framework.GUIs;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types.GUI;

#pragma warning disable 0414

namespace Vexe.Editor.Helpers
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
			gui = new GLWrapper();
			this.onClose = onClose;
			this.tabs = tabs;

			for (int i = 0; i < tabs.Length; i++)
			{
				var t = tabs[i];
				t.gui = gui;
				t.selectionStyle = GUIHelper.SelectionRect;
			}
			currentTab = lastTabIndex >= tabs.Length ? tabs[0] : tabs[lastTabIndex];
			search = "";
		}

		public void OnGUI()
		{
			gui._beginH(EditorStyles.toolbar);
			{
				for (int i = 0; i < tabs.Length; i++)
				{
					var tab = tabs[i];
					gui._beginColor(tab == currentTab ? TabColor : (Color?)null);
					{
						if (gui.Button(tab.title, EditorStyles.toolbarButton))
						{
							currentTab = tab;
							currentTab.Refresh();
							lastTabIndex = i;
						}
					}
					gui._endColor();
				}
				gui.FlexibleSpace();
			}
			gui._endH();

			gui.Space(3f);
			GUI.SetNextControlName("SearchBox");
			search = gui.ToolbarSearchField(search);
			if (Event.current != null && Event.current.isKey && Event.current.keyCode == KeyCode.Tab)
			{
				Debug.Log(GUI.GetNameOfFocusedControl());
				GUI.FocusControl("SearchBox");
				Debug.Log(GUI.GetNameOfFocusedControl());
			}
			gui.Splitter();
			gui._beginScroll(ref scroll, BackgroundStyle);
			{
				currentTab.OnGUI(search, maxSize.x - minSize.x);
			}
			gui._endScroll();
		}

		public void OnFocus()
		{
			if (currentTab != null)
				currentTab.Refresh();
		}

		private void Update()
		{
			if (close) Close(); // Can't close out immediately in OnLostFocus
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
			CleanUp();
			close = true;
			onClose.SafeInvoke();
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
			defaultValue      = (T)typeof(T).GetDefaultValue();
		}

		public Tab(Func<T[]> getValues, Func<T> getCurrent, Action<T> setTarget, Func<T, string> getValueName, string title)
			: this(getValues, getCurrent, setTarget, getValueName, null, title)
		{
		}

		public override void OnGUI(string search, float width)
		{
			// Default value
			{
				var isDefault = defaultValue.GenericEqual(getCurrent());
				gui._beginColor(GUIHelper.RedColorDuo.FirstColor);
				{
					OnValueGUI(defaultValue,
						string.Format("Default ({0})", typeof(T).IsClass ? "Null" : defaultValue.ToString()),
						width, isDefault,
						isDefault ? selectionStyle : getStyleDuo(defaultValue).NextStyle,
						isDefault ? EditorStyles.whiteLargeLabel : EditorStyles.whiteLargeLabel);
				}
				gui._endColor();
			}

			if (values == null) Refresh();
			if (values == null) return;

			if (previousSearch != search)
			{
				previousSearch = search;
				filteredValues = values.Where(x => Regex.IsMatch(getValueName(x), search, RegexOptions.IgnoreCase))
											  .ToList();
			}

			for (int i = 0; i < filteredValues.Count; i++)
			{
				var value      = filteredValues[i];
				var isSelected = value.GenericEqual(getCurrent());
				var nextStyle  = getStyleDuo(value).NextStyle;

				OnValueGUI(value, getValueName(value), width, isSelected, nextStyle,
					isSelected ? EditorStyles.whiteBoldLabel : EditorStyles.whiteLabel);
			}
		}

		private void OnValueGUI(T value, string itemName, float width, bool isSelected, GUIStyle nextStyle, GUIStyle labelStyle)
		{
			gui._beginH(isSelected ? selectionStyle : nextStyle);
			{
				gui.Space(SelectionWindow.IndentWidth);
				gui.Label(itemName, labelStyle);
				var rect = gui.GetLastRect();
				{
					rect.width = width;
					if (!isSelected)
					{
						GUIHelper.AddCursorRect(rect, MouseCursor.Link);
						if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
						{
							setTarget(value);
							Refresh();
						}
					}
				}
			}
			gui._endH();
		}

		public override void Refresh()
		{
			values = getValues();
		}
	}
}