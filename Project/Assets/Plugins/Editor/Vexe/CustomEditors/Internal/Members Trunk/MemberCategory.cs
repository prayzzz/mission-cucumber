#define PROFILE

using System.Collections.Generic;
using System.Linq;

using Assets.Plugins.Editor.Vexe.GUI;
using Assets.Plugins.Editor.Vexe.Other;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;

using UnityEditor;

using UnityEngine;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.CustomEditors.Internal
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
        public string Name               { get { return this.name; } }
        public string FullPath           { get { return this.fullPath; } }
        public bool ForceExpand          { get; set; }
        public bool HideHeader           { get; set; }
        public bool IsExpanded           { get; private set; }
        public bool Indent               { get; set; }
        public MembersDisplay Display
        {
            get { return this.display; }
            set
            {
                if (this.display != value)
                {
                    this.display = value;
                    this.Members.OfType<MemberCategory>().Foreach(c => c.Display = this.display);
                }
            }
        }

        public MemberCategory(
            string fullPath, List<ICanBeDrawn> members, float displayOrder, string id, GLWrapper gui, BetterEditorPrefs prefs)
        {
            this.Members       = members;
            this.DisplayOrder  = displayOrder;
            this.prefs    = prefs;
            this.gui      = gui;
            this.fullPath = fullPath;
            this.name     = this.FullPath.Substring(this.FullPath.LastIndexOf('/') + 1);
            this.id       = id + fullPath;
            this.Indent        = true;
        }

        public void AddMember(ICanBeDrawn member)
        {
            this.Members.Add(member);
        }

        // Keys & Foldouts
        #region
        private bool DoHeader()
        {
            bool foldout = false;
            this.gui._beginH(EditorStyles.toolbarButton);
            {
                this.gui.Space(10f);
                foldout = this.gui.Foldout(this.name, this.prefs.GetSafeBool(this.id), GUILayout.ExpandWidth(true));
                this.prefs.SetBool(this.id, foldout);
            }
            this.gui._endH();

            return foldout;
        }
        #endregion


        private bool IsOptionSet(MembersDisplay option)
        {
            return (this.Display & option) > 0;
        }

        public void Draw()
        {
            int count = this.Members.Count;
            if (count == 0)
                return;
            this.IsExpanded = this.HideHeader || this.DoHeader();
            if (!(this.IsExpanded || this.ForceExpand))
                return;

            if (!this.HideHeader) this.gui.Space(-2f);

            bool showGuiBox      = this.IsOptionSet(MembersDisplay.GuiBox);
            bool showLineNumbers = this.IsOptionSet(MembersDisplay.LineNumber);
            bool showSplitter    = this.IsOptionSet(MembersDisplay.Splitter);

            this.gui._beginIndent(showGuiBox ? EditorStyles.textArea : GUIStyle.none, this.Indent ? GLWrapper.NextIndentLevel : 0);
            {
                this.gui.Space(5f);
#if PROFILE
                Profiler.BeginSample(this.name + " Members");
#endif
                for (int i = 0; i < count; i++)
                {
                    var member = this.Members[i];

                    this.gui._beginH();
                    {
                        if (showLineNumbers)
                            this.gui.NumericLabel(i + 1);
                        else member.HeaderSpace();
                        this.gui._beginV();
                        {
                            member.Draw();
                        }
                        this.gui._endV();
                    }
                    this.gui._endH();

                    if (showSplitter && i < count - 1)
                        this.gui.Splitter();
                    else this.gui.Space(2f);
                }
#if PROFILE
                Profiler.EndSample();
#endif
            }
            this.gui._endIndent();
        }

        public void HeaderSpace()
        {
            if (this.IsExpanded)
                this.gui.Space(4f);
        }
    }
}