using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;

#pragma warning disable 0168

namespace Vexe.Editor.Framework.Drawers
{
	using Editor = UnityEditor.Editor;

	public class InlineDrawer : CompositeDrawer<UnityObject, InlineAttribute>
	{
		private Editor editor;
		private bool hideTarget;
		private Func<GameObject, IEnumerable<Editor>> mGetEditors;
		private List<bool> expandValues;
		private UnityObject current;

		public bool GuiBox { get; set; }

		public bool HideTarget
		{
			get { return hideTarget; }
			set
			{
				if (hideTarget != value)
				{
					dmValue.SetVisibility(!value);
					hideTarget = value;
				}
			}
		}

		private Func<GameObject, IEnumerable<Editor>> getEditors
		{
			get
			{
				if (mGetEditors == null)
				{
					dbgLog("assigning get editors");
					mGetEditors = new Func<GameObject, IEnumerable<Editor>>(go =>
					{
						dbgLog("created new editors");
						return go.GetAllComponents().Select<Component, Editor>(Editor.CreateEditor);
					}).Memoize();
				}
				return mGetEditors;
			}
		}

		protected override void OnInitialized()
		{
			expandValues = new List<bool>();
			GuiBox       = attribute.GuiBox;
			HideTarget   = attribute.HideTarget;
			dbgLog("Initialized InlineDrawer");
		}

		public override void OnLeftGUI()
		{
			Foldout();
			gui.Space(-10f);
		}

		public override void OnLowerGUI()
		{
			var objRef = dmValue;
			if (foldout)
			{
				if (objRef == null)
				{
					gui.HelpBox("Please assign a target to inline");
					return;
				}

				Action<UnityObject> update = obj =>
				{
					current = obj;
					editor = Editor.CreateEditor(current);
				};

				if (editor == null || objRef != current)
					update(objRef);

				gui._beginV(GuiBox ? GUI.skin.box : GUIStyle.none);
				{
					var go = objRef as GameObject;
					if (go != null)
					{
						getEditors(go).Foreach(DrawEditor);
					}
					else
					{
						if (editor.target == null)
							update(objRef);

						editor.OnInspectorGUI();
					}
				}
				gui._endV();
			}
		}

		private void DrawEditor(Editor e, int index)
		{
			DrawExpandableHeader(e.target, index, e.OnInspectorGUI);
		}

		private void DrawExpandableHeader(UnityObject target, int index, Action body)
		{
			bool expanded = EditorGUILayout.InspectorTitlebar(InternalEditorUtility.GetIsInspectorExpanded(target), target);
			bool previous;
			if (index >= expandValues.Count)
			{
				expandValues.Add(expanded);
				previous = !expanded;
			}
			else
			{
				previous = expandValues[index];
			}
			if (expanded != previous)
			{
				expandValues[index] = expanded;
				InternalEditorUtility.SetIsInspectorExpanded(target, expanded);
			}
			if (expanded)
			{
				gui.Space(5f);
				gui._beginIndent();
				{
					body();
				}
				gui._endIndent();

			}
		}
	}
}