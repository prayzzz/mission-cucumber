#define PROFILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Assets.Plugins.Editor.Vexe.CustomEditors.Internal;
using Assets.Plugins.Vexe.Runtime.Serialization;
using Assets.Plugins.Vexe.Runtime.Serialization.Serializers;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using Fasterflect;

using UnityEditor;

using UnityEngine;

using Vexe.Runtime.Extensions;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.CustomEditors
{
    public static class MenuItems
    {
        [MenuItem("Tools/Vexe/Other/ToggleProfilerRecord &#r")]
        public static void ToggleProfilerRecord()
        {
            var profilerWndType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ProfilerWindow");
            var profilerWnd = EditorWindow.GetWindow(profilerWndType);
            var record = profilerWnd.GetType().GetField("m_Recording", BindingFlags.Instance | BindingFlags.NonPublic);
            record.SetValue(profilerWnd, !(bool)record.GetValue(profilerWnd));
        }
    }

    public abstract class BetterEditor : BaseEditor<Object>
    {
        private List<MemberCategory> baseCategories;
        private List<VisibleMember> visibleMembers;
        private SerializedProperty script;
        private bool minimal;
        private Settings settings;
        private FieldInfo serializationData;

        protected abstract bool dbg { get; set; }
        protected abstract void OnAwakeAssertion();

        protected override void OnAwake()
        {
            this.OnAwakeAssertion();

            // Fetch visible members
            {
                Func <MemberInfo, VisibleMember> newMember = m => new VisibleMember(m, this.gui, this.target, this.target, this.id);
                var allMembers = this.targetType.GetMembersBeneath(this.target is BetterBehaviour ? typeof(BetterBehaviour) : typeof(BetterScriptableObject));
                this.visibleMembers = allMembers.Where(SerializationLogic.Default.IsVisibleMember)
                                            .Select(newMember)
                                            .ToList();
                // Initialize view
                {
                    this.minimal = this.targetType.IsDefined<MinimalViewAttribute>(true);
                    if (!this.minimal)
                    {
                        this.baseCategories = new List<MemberCategory>();

                        Func<string, float,  MemberCategory> newCategory = (path, order) =>
                            new MemberCategory(path, new List<ICanBeDrawn>(), order, this.id, this.gui, this.prefs);

                        var basic = this.targetType.GetCustomAttribute<BasicViewAttribute>(true);
                        if (basic != null)
                        {
                            var c = newCategory("", 0f);
                            c.Indent = false;
                            c.HideHeader = true;
                            this.visibleMembers.Foreach(c.AddMember);
                            this.baseCategories.Add(c);
                        }
                        else
                        {
                            // Create the intial input from the target's visible members
                            var input = new MemberGroup(this.visibleMembers, newMember);

                            // Create resolvers
                            var resolvers = new GroupResolver[]
                            {
                                    new PatternResolver(),        new MemberTypesResolver(),
                                    new ReturnTypeResolver(),    new ExplicitMemberAddResolver(),
                                    new CategoryMembersResolver()
                            };
                            var core = new CoreResolver(resolvers, () => new MemberGroup(newMember));

                            Action<DefineCategoryAttribute, MemberCategory> resolve = (def, category) =>
                                core.Resolve(input, def).Members
                                                                .OrderBy(m => m.DisplayOrder)
                                                                .ThenBy(m => m.Info.GetDataType().Name)
                                                                .ThenBy(m => m.Info.Name)
                                                               .Foreach(category.AddMember);

                            var multiple     = this.targetType.GetCustomAttribute<DefineCategoriesAttribute>(true);
                            var ignored         = this.targetType.GetCustomAttributes<IgnoreCategoriesAttribute>(true);
                            var definitions = this.targetType.GetCustomAttributes<DefineCategoryAttribute>(true);
                            if (multiple != null)
                                definitions = definitions.Concat(multiple.names.Select(n => new DefineCategoryAttribute(n, 1000)));

                            definitions = definitions.Where(d => !ignored.Any(ig => ig.Paths.Contains(d.FullPath)))
                                                      .ToList();

                            Func<string, string[]> ParseCategoryPath = fullPath =>
                            {
                                int nPaths = fullPath.Split('/').Length;
                                string[] result = new string[nPaths];
                                for (int i = 0, index = -1; i < nPaths - 1; i++)
                                {
                                    index = fullPath.IndexOf('/', index + 1);
                                    result[i] = fullPath.Substring(0, index);
                                }
                                result[nPaths - 1] = fullPath;
                                return result;
                            };

                            // Order by exclusivity then path lengths
                            var defs = from d in definitions
                                          let paths = ParseCategoryPath(d.FullPath)
                                          orderby !d.Exclusive//, paths.Length
                                          select new { def = d, paths };

                            Func<MemberCategory, List<MemberCategory>> getParentCats = parent =>
                                parent == null ? this.baseCategories :
                                (from m in parent.Members
                                 let cat = m as MemberCategory
                                 where cat != null
                                 select cat).ToList();

                            // Parse paths and resolve definitions
                            var categories = new Dictionary<string, MemberCategory>();
                            foreach (var x in defs)
                            {
                                var paths = x.paths;
                                var d = x.def;

                                MemberCategory parent = null;

                                for (int i = 0; i < paths.Length; i++)
                                {
                                    var p = paths[i];
                                    var current = getParentCats(parent).FirstOrDefault(c => c.FullPath == p);
                                    if (current == null)
                                    {
                                        current = newCategory(p, d.DisplayOrder);
                                        if (i == 0)
                                            this.baseCategories.Add(current);
                                        if (parent != null)
                                            parent.AddMember(current);
                                    }
                                    categories[p] = current;
                                    parent = current;
                                }

                                categories[paths.Last()].ForceExpand = d.ForceExpand;
                                resolve(d, categories[paths.Last()]);
                                categories.Clear();
                                parent.Members = parent.Members.OrderBy(m => m.DisplayOrder).ToList();
                            }
                            this.baseCategories = this.baseCategories.OrderBy(x => x.DisplayOrder).ToList();
                        }
                    }

                    // Set script reference
                    this.script = this.serializedObject.FindProperty("m_Script");

                    // Initialize settings
                    {
                        this.settings                 = new Settings();
                        var display              = this.prefs.GetInt(this.id);
                        this.settings.display         = display.HasValue ? (MembersDisplay)display.Value : Settings.DefaultDisplay;
                        this.settings.serializerTypes = typeof(BaseSerializer).GetConcreteChildren().ToArray();
                        this.settings.serializerNames = this.settings.serializerTypes.Select(t => t.GetCustomAttribute<DisplayNameAttribute>().name).ToArray();
                        this.settings.serializerIndex = this.settings.serializerTypes.IndexOf((this.target as ISavable).SerializerType);
                    }

                    this.serializationData = this.targetType.Field("serializationData", Flags.InstancePrivate);

                    this.OnInitialized();
                }
            }
        }

        public override void OnInspectorGUI()
        {
#if PROFILE
            Profiler.BeginSample(this.targetType.Name + " OnInspectorGUI");

            Profiler.BeginSample(this.targetType.Name + " Header");
#endif

            // Header
            {
                this.gui.Space(3f);
                this.gui._beginH(EditorStyles.toolbarButton);
                {
                    this.gui.Space(10f);
                    this.foldout = this.gui.Foldout(this.foldout);
                    this.gui.Space(-11f);
                    this.gui.ObjectField("Script", this.script.objectReferenceValue);
                }
                this.gui._endH();

                if (this.foldout)
                {
                    this.gui.Space(-2f);
                    this.gui._beginIndent(UnityEngine.GUI.skin.textField);
                    {
                        this.gui.Space(2f);
                        this.dbg = this.gui.Toggle("Debug", this.dbg);
                        var display = this.settings.display;
                        var mask = this.gui.EnumMaskFieldThatWorks(display, "Display");
                        {
                            var newValue = (MembersDisplay)mask;
                            if (display != newValue)
                            {
                                this.settings.display = newValue;
                                this.prefs.SetInt(this.id, mask);
                            }
                        }
                        var selection = this.gui.Popup("Serializer", this.settings.serializerIndex,
                            this.settings.serializerNames);
                        {
                            if (selection != this.settings.serializerIndex)
                            {
                                (this.target as ISavable).SerializerType = this.settings.serializerTypes[selection];
                                this.settings.serializerIndex = selection;
                            }
                        }
                        this.gui._beginH();
                        {
                            this.gui.Space(10f);
                            this.gui._beginV();
                            {
                                this.gui.MemberField(this.serializationData, this.target, this.target, this.id);
                            }
                            this.gui._endV();
                        }
                        this.gui._endH();
                    }
                    this.gui._endIndent();
                }
            }

#if PROFILE
            Profiler.EndSample();
#endif

            EditorGUI.BeginChangeCheck();

#if PROFILE
            Profiler.BeginSample(this.targetType.Name + " Members");
#endif

            if (this.minimal)
            {
                for (int i = 0; i < this.visibleMembers.Count; i++)
                    this.visibleMembers[i].Draw();
            }
            else
            {
                for (int i = 0; i < this.baseCategories.Count; i++)
                {
                    var c = this.baseCategories[i];
                    c.Display = this.settings.display;
                    c.Draw();
                }
            }

            this.OnGUI();

#if PROFILE
            Profiler.EndSample();
#endif

            if (EditorGUI.EndChangeCheck())
            { 
                EditorUtility.SetDirty(this.target);
                var savable = this.target as ISavable;
                if (savable != null)
                    savable.Save();
            }

#if PROFILE
            Profiler.EndSample();
#endif
        }

        /// <summary>
        /// override this when writing GUI for your custom BetterEditors instead of using OnInspectorGUI
        /// </summary>
        protected virtual void OnGUI()
        {
        }

        /// <summary>
        /// override this when initializing your custom BetterEditors instead of using OnEnable or OnAwake
        /// </summary>
        protected virtual void OnInitialized()
        {
        }

        private struct Settings
        {
            public int serializerIndex;
            public Type[] serializerTypes;
            public string[] serializerNames;
            public MembersDisplay display;

            public static MembersDisplay DefaultDisplay
            {
                get { return MembersDisplay.GuiBox; }
            }
        }
    }
}
