using System;
using System.Reflection;
using Fasterflect;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Framework.GUIs;
using Vexe.Runtime.Helpers;
using Vexe.Runtime.Types.GUI;
using BF = System.Reflection.BindingFlags;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Helpers
{
	public static class GUIHelper
	{
		/* <<< Misc >>> */
		#region
		public static bool IsProSkin { get { return EditorGUIUtility.isProSkin; } }

		public static GUILayoutOption MultifieldOption = GUILayout.Height(GUIConstants.kDefaultHeight * 3f);

		/// <summary>
		/// Combines two rectangles, returns the result
		/// </summary>
		public static Rect CombineRects(Rect left, Rect right)
		{
			return new Rect(Math.Min(left.x, right.x), Math.Max(left.y, right.y), left.width + right.width, Math.Max(left.height, right.height));
		}

		public static void AddCursorRect(Rect rect, MouseCursor cursor)
		{
			EditorGUIUtility.AddCursorRect(rect, cursor);
		}

		public static Rect GetLastRect()
		{
			return GUILayoutUtility.GetLastRect();
		}
		#endregion

		public static class Folds
		{
			public const string DefaultExpandSymbol = "►";
			public const string DefaultFoldSymbol = "▼";
			public const string AlternateExpandSymbol = ">";
			public const string AlternateFoldSymbol = "˅";
		}

		/* <<< Drag-n-Drop registration >>> */
		/// <summary>
		/// A couple of handy methods to register fields for drag-n-drop operations given what to drag/drop.
		/// </summary>
		#region
		/// <summary>
		/// Registers fieldRect for drag operations. dragObject is what's being dragged out of that field.
		/// </summary>
		public static void RegisterFieldForDrag(Rect fieldRect, UnityObject dragObject)
		{
			if (dragObject == null) return; // can't drag a null wtf!
			Event e = Event.current;
			if (fieldRect.Contains(e.mousePosition) && e.type == EventType.MouseDrag)
			{
				DragAndDrop.PrepareStartDrag();
				DragAndDrop.objectReferences = new[] { dragObject };
				DragAndDrop.StartDrag("drag");
				Event.current.Use();
			}
		}
		public static void RegisterFieldForDrag(UnityObject dragObject)
		{
			RegisterFieldForDrag(GUILayoutUtility.GetLastRect(), dragObject);
		}

		/// <summary>
		/// Registers fieldRect for drop operations.
		/// Returns the dropped value
		/// </summary>
		public static T RegisterFieldForDrop<T>(Rect fieldRect, Func<UnityObject[], UnityObject> getDroppedObject) where T : UnityObject
		{
			Event e = Event.current;
			EventType eventType = e.type;
			T result = null;
			if (fieldRect.Contains(e.mousePosition) && eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
			{
				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				if (eventType == EventType.DragPerform)
				{
					DragAndDrop.AcceptDrag();
					result = getDroppedObject(DragAndDrop.objectReferences) as T;
					Event.current.Use();
				}
			}
			return result;
		}
		public static T RegisterFieldForDrop<T>(Rect fieldRect) where T : UnityObject
		{
			return RegisterFieldForDrop<T>(fieldRect, objects => objects[0]);
		}
		#endregion

		/* <<< Custom action fields >>> */
		#region
		private static Predicate LMB_MouseDown = EventsHelper.MouseEvents.IsLMB_MouseDown;
		public static void CustomActionField(Rect fieldRect, Action action, bool showCursorPredicate, bool clickPredicate, MouseCursor cursor)
		{
			if (showCursorPredicate)
				AddCursorRect(fieldRect, cursor);
			if (fieldRect.Contains(Event.current.mousePosition))
			{
				if (clickPredicate)
					action();
			}
		}
		public static void CustomActionField(Action action, bool showCursorPredicate, bool clickPredicate, MouseCursor cursor)
		{
			CustomActionField(GetLastRect(), action, showCursorPredicate, clickPredicate, cursor);
		}
		public static void SelectField(UnityObject obj, MouseCursor cursor = MouseCursor.Link)
		{
			SelectField(GetLastRect(), obj, cursor);
		}
		public static void SelectField(Rect fieldRect, UnityObject obj, MouseCursor cursor = MouseCursor.Link)
		{
			SelectField(fieldRect, obj, LMB_MouseDown(), cursor);
		}
		public static void SelectField(Rect fieldRect, UnityObject obj, bool clickPredicate, MouseCursor cursor = MouseCursor.Link)
		{
			CustomActionField(fieldRect, () => EditorHelper.SelectObject(obj), obj != null, clickPredicate, cursor);
		}
		public static void SelectField(UnityObject obj, bool clickPredicate, MouseCursor cursor = MouseCursor.Link)
		{
			CustomActionField(() => EditorHelper.SelectObject(obj), obj != null, clickPredicate, cursor);
		}
		public static void PingField(UnityObject obj, MouseCursor cursor = MouseCursor.Zoom)
		{
			PingField(GetLastRect(), obj, cursor);
		}
		public static void PingField(Rect fieldRect, UnityObject obj, MouseCursor cursor = MouseCursor.Zoom)
		{
			PingField(fieldRect, obj, obj != null, LMB_MouseDown(), cursor);
		}
		public static void PingField(Rect fieldRect, UnityObject obj, bool showPredicate, bool clickPredicate, MouseCursor cursor = MouseCursor.Zoom)
		{
			CustomActionField(fieldRect, () => EditorHelper.PingObject(obj), showPredicate, clickPredicate, cursor);
		}
		public static void PingField(Rect fieldRect, UnityObject obj, bool showPredicate, MouseCursor cursor = MouseCursor.Zoom)
		{
			PingField(fieldRect, obj, showPredicate, LMB_MouseDown(), cursor);
		}
		public static void PingField(UnityObject obj, bool showPredicate, bool clickPredicate, MouseCursor cursor = MouseCursor.Zoom)
		{
			PingField(GetLastRect(), obj, showPredicate, clickPredicate, cursor);
		}
		public static void PingField(UnityObject obj, bool showPredicate, MouseCursor cursor = MouseCursor.Zoom)
		{
			PingField(obj, showPredicate, LMB_MouseDown(), cursor);
		}
		#endregion

		/* <<< GUIStyles >>> */
		#region
		private static GUIStyle refreshButtonStyle;
		private static GUIStyle selectedStyle;
		private static GUIStyle selectionButtonStyle;
		public static GUIStyle RefreshButtonStyle
		{
			get
			{
				if (refreshButtonStyle == null)
				{
					refreshButtonStyle = new GUIStyle(EditorStyles.miniButton)
					{
						contentOffset = new Vector2(-1.5f, 0),
						alignment = TextAnchor.MiddleCenter,
						fontSize = 17,
						padding = new RectOffset(0, 0, 0, 0)
					};
				}
				return refreshButtonStyle;
			}
		}
		public static GUIStyle SelectedStyle
		{
			get
			{
				if (selectedStyle == null)
				{
					selectedStyle = new GUIStyle(GUI.skin.textField)
					{
						alignment = TextAnchor.MiddleLeft,
						margin = new RectOffset(0, 0, 0, 0),
						padding = new RectOffset(0, 0, 4, 4),
						normal = new GUIStyleState
						{
							background = RTHelper.GetTexture(61, 128, 223, 255, HideFlags.DontSave),
							textColor = Color.white
						}
					};
				}
				else if (selectedStyle.normal.background == null)
				{
					selectedStyle.normal.background = RTHelper.GetTexture(61, 128, 223, 255, HideFlags.DontSave);
				}
				return selectedStyle;
			}
		}
		public static GUIStyle SelectionButtonStyle
		{
			get
			{
				if (selectionButtonStyle == null)
				{
					selectionButtonStyle = new GUIStyle(EditorStyles.miniButtonRight)
					{
						alignment = TextAnchor.MiddleCenter,
						fontSize = 20,
						padding = new RectOffset(0, 0, 0, 0)
					};
				}
				return selectionButtonStyle;
			}
		}
		public static GUIStyle CreateLabel(int fontSize)
		{
			return CreateLabel(fontSize, Vector2.zero);
		}
		public static GUIStyle CreateLabel(int fontSize, Vector2 contentOffset, TextAnchor alignment = TextAnchor.MiddleLeft, FontStyle fontStyle = FontStyle.Bold)
		{
			return new GUIStyle(GUI.skin.label)
			{
				contentOffset = contentOffset,
				alignment = alignment,
				fontSize = fontSize,
				fontStyle = fontStyle
			};
		}
		public static GUIStyle GetStyle(MiniButtonStyle style)
		{
			switch (style)
			{
				case MiniButtonStyle.Left: return EditorStyles.miniButtonLeft;
				case MiniButtonStyle.Right: return EditorStyles.miniButtonRight;
				case MiniButtonStyle.ModLeft: return ModButtonLeft;
				case MiniButtonStyle.ModRight: return ModButtonRight;
				case MiniButtonStyle.ModMid: return ModButtonMid;
				default: return EditorStyles.miniButtonMid;
			}
		}
		private static GUIStyle GetModButtonStyle(string name, ref GUIStyle style)
		{
			if (style == null)
				style = new GUIStyle(name)
				{
					fontSize = 12,
					contentOffset = new Vector2(-1f, -.8f),
					clipping = TextClipping.Overflow
				};
			return style;
		}
		private static GUIStyle modButtonLeft;
		private static GUIStyle modButtonMid;
		private static GUIStyle modButtonRight;
		private static GUIStyle foldoutStyle;
		public static GUIStyle FoldoutStyle
		{
			get
			{
				if (foldoutStyle == null)
				{
					foldoutStyle = new GUIStyle();
					foldoutStyle.normal = new GUIStyleState
					{
						textColor = EditorStyles.foldout.normal.textColor
					};
				}
				return foldoutStyle;
			}
		}
		public static GUIStyle ModButtonLeft { get { return GetModButtonStyle("miniButtonLeft", ref modButtonLeft); } }
		public static GUIStyle ModButtonMid { get { return GetModButtonStyle("miniButtonMid", ref modButtonMid); } }
		public static GUIStyle ModButtonRight { get { return GetModButtonStyle("miniButtonRight", ref modButtonRight); } }
		public static void DestroyStyleTexture(GUIStyle style)
		{
			UnityObject.DestroyImmediate(style.normal.background);
		}

		/* <<< ColorDuos >>> */
		#region
		private static ColorDuo greenColorDuo;
		private static ColorDuo lightBlueColorDuo;
		private static ColorDuo darkBlueColorDuo;
		private static ColorDuo redColorDuo;
		private static ColorDuo lightGreyColorDuo;
		private static ColorDuo darkGreyColorDuo;
		private static ColorDuo yellowColorDuo;
		private static ColorDuo pinkColorDuo;
		public static ColorDuo GreenColorDuo { get { return GetColorDuo(ref greenColorDuo, "8AFF8E", "7FE36D"); } }
		public static ColorDuo RedColorDuo { get { return GetColorDuo(ref redColorDuo, "FF9696", "FFB8B8"); } }
		public static ColorDuo LightGreyColorDuo { get { return GetColorDuo(ref lightGreyColorDuo, "C4C4C4", "B8B8B8"); } }
		public static ColorDuo DarkGreyColorDuo { get { return GetColorDuo(ref darkGreyColorDuo, "4A4A4A", "656565"); } }
		public static ColorDuo LightBlueColorDuo { get { return GetColorDuo(ref lightBlueColorDuo, "8FFFFD", "BAFFFE"); } }
		public static ColorDuo DarkBlueColorDuo { get { return GetColorDuo(ref darkBlueColorDuo, "2737B8", "202D91"); } }
		public static ColorDuo YellowColorDuo { get { return GetColorDuo(ref yellowColorDuo, "F7FF69", "FBFFAD"); } }
		public static ColorDuo PinkColorDuo { get { return GetColorDuo(ref pinkColorDuo, "FFADFA", "FFC9FB"); } }
		private static ColorDuo GetColorDuo(ref ColorDuo cd, string c1, string c2)
		{
			if (cd == null) cd = new ColorDuo(RTHelper.HexToColor(c1), RTHelper.HexToColor(c2));
			return cd;
		}
		#endregion

		/* <<< StyleDuos >>> */
		#region
		private static StyleDuo greenStyleDuo;
		private static StyleDuo redStyleDuo;
		private static StyleDuo lightBlueStyleDuo;
		private static StyleDuo darkBlueStyleDuo;
		private static StyleDuo lightGreyStyleDuo;
		private static StyleDuo darkGreyStyleDuo;
		public static StyleDuo GreenStyleDuo { get { return GetStyleDuo(ref greenStyleDuo, GreenColorDuo); } }
		public static StyleDuo RedStyleDuo { get { return GetStyleDuo(ref redStyleDuo, RedColorDuo); } }
		public static StyleDuo LightBlueStyleDuo { get { return GetStyleDuo(ref lightBlueStyleDuo, LightBlueColorDuo); } }
		public static StyleDuo DarkBlueStyleDuo { get { return GetStyleDuo(ref darkBlueStyleDuo, DarkBlueColorDuo); } }
		public static StyleDuo LightGreyStyleDuo { get { return GetStyleDuo(ref lightGreyStyleDuo, LightGreyColorDuo); } }
		public static StyleDuo DarkGreyStyleDuo { get { return GetStyleDuo(ref darkGreyStyleDuo, DarkGreyColorDuo); } }
		private static StyleDuo GetStyleDuo(ref StyleDuo style, ColorDuo cd)
		{
			// it seems that re-creating the textures if they've been destroyed will still cause some strange leaks
			// so I just re-create the whole style if the textures are destroyed
			if (style == null || style.TexturesHaveBeenDestroyed) style = new StyleDuo(cd);
			return style;
		}
		#endregion
		#endregion

		// Reflection
		#region
		private static GUIStyle helpBox;
		private static GUIStyle selectionRect;
		private static MethodInfo getHelpIcon;

		public static GUIStyle SelectionRect
		{
			get
			{
				return selectionRect ??
					(selectionRect = typeof(EditorStyles).DelegateForGetPropertyValue("selectionRect", Flags.StaticPrivate).Invoke(null) as GUIStyle);
			}
		}

		public static GUIStyle HelpBox
		{
			get
			{
				return RTHelper.LazyValue(() => helpBox, value => helpBox = value, () =>
					typeof(EditorStyles)
						.GetProperty("helpBox", BF.Static | BF.NonPublic)
						.GetValue(null, null) as GUIStyle
				);
			}
		}

		public static Texture2D GetHelpIcon(MessageType type)
		{
			getHelpIcon = RTHelper.LazyValue(() => getHelpIcon, value => getHelpIcon = value, () =>
				typeof(EditorGUIUtility).GetMethod("GetHelpIcon", BF.Static | BF.NonPublic));
			return getHelpIcon.Invoke(null, new object[] { type }) as Texture2D;
		}
		#endregion
	}
}