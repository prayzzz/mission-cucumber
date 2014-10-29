#define PROFILE

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Framework;
using Vexe.Editor.Framework.GUIs;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.BetterBehaviourInternal
{
	public class MemberCategory : ICanBeDrawn
	{
		private readonly string fullPath;
		private readonly string name;
		private readonly string id;
		private readonly GLWrapper gui;
		private readonly BetterEditorPrefs prefs;
		private MembersDisplay display;

		public List<ICanBeDrawn> Members { get; set; }
		public float DisplayOrder        { get; set; }
		public string Name               { get { return name; } }
		public string FullPath           { get { return fullPath; } }
		public bool ForceExpand          { get; set; }
		public bool HideHeader           { get; set; }
		public bool IsExpanded           { get; private set; }
		public bool Indent               { get; set; }
		public MembersDisplay Display
		{
			get { return display; }
			set
			{
				if (display != value)
				{
					display = value;
					Members.OfType<MemberCategory>().Foreach(c => c.Display = display);
				}
			}
		}

		public MemberCategory(
			string fullPath, List<ICanBeDrawn> members, float displayOrder, string id, GLWrapper gui, BetterEditorPrefs prefs)
		{
			Members       = members;
			DisplayOrder  = displayOrder;
			this.prefs    = prefs;
			this.gui      = gui;
			this.fullPath = fullPath;
			this.name     = FullPath.Substring(FullPath.LastIndexOf('/') + 1);
			this.id       = id + fullPath;
			Indent        = true;
		}

		public void AddMember(ICanBeDrawn member)
		{
			Members.Add(member);
		}

		// Keys & Foldouts
		#region
		private bool DoHeader()
		{
			bool foldout = false;
			gui._beginH(EditorStyles.toolbarButton);
			{
				gui.Space(10f);
				foldout = gui.Foldout(name, prefs.GetSafeBool(id), GUILayout.ExpandWidth(true));
				prefs.SetBool(id, foldout);
			}
			gui._endH();

			return foldout;
		}
		#endregion


		private bool IsOptionSet(MembersDisplay option)
		{
			return (Display & option) > 0;
		}

		public void Draw()
		{
			int count = Members.Count;
			if (count == 0)
				return;
			IsExpanded = HideHeader || DoHeader();
			if (!(IsExpanded || ForceExpand))
				return;

			if (!HideHeader) gui.Space(-2f);

			bool showGuiBox      = IsOptionSet(MembersDisplay.GuiBox);
			bool showLineNumbers = IsOptionSet(MembersDisplay.LineNumber);
			bool showSplitter    = IsOptionSet(MembersDisplay.Splitter);

			gui._beginIndent(showGuiBox ? EditorStyles.textArea : GUIStyle.none, Indent ? GLWrapper.NextIndentLevel : 0);
			{
				gui.Space(5f);
#if PROFILE
				Profiler.BeginSample(name + " Members");
#endif
				for (int i = 0; i < count; i++)
				{
					var member = Members[i];

					gui._beginH();
					{
						if (showLineNumbers)
							gui.NumericLabel(i + 1);
						else member.HeaderSpace();
						gui._beginV();
						{
							member.Draw();
						}
						gui._endV();
					}
					gui._endH();

					if (showSplitter && i < count - 1)
						gui.Splitter();
					else gui.Space(2f);
				}
#if PROFILE
				Profiler.EndSample();
#endif
			}
			gui._endIndent();
		}

		public void HeaderSpace()
		{
			if (IsExpanded)
				gui.Space(4f);
		}
	}
}