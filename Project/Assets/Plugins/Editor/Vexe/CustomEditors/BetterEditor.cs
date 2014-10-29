#define PROFILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.BetterBehaviourInternal;
using Vexe.Editor.Framework.Editors;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Serialization;
using Vexe.Runtime.Serialization.Serializers;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.BetterEditors
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

	public abstract class BetterEditor : BaseEditor<UnityObject>
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
			OnAwakeAssertion();

			// Fetch visible members
			{
				Func <MemberInfo, VisibleMember> newMember = m => new VisibleMember(m, gui, target, target, id);
				var allMembers = targetType.GetMembersBeneath(target is BetterBehaviour ? typeof(BetterBehaviour) : typeof(BetterScriptableObject));
				visibleMembers = allMembers.Where(SerializationLogic.Default.IsVisibleMember)
					                        .Select(newMember)
					                        .ToList();
				// Initialize view
				{
					minimal = targetType.IsDefined<MinimalViewAttribute>(true);
					if (!minimal)
					{
						baseCategories = new List<MemberCategory>();

						Func<string, float,  MemberCategory> newCategory = (path, order) =>
							new MemberCategory(path, new List<ICanBeDrawn>(), order, id, gui, prefs);

						var basic = targetType.GetCustomAttribute<BasicViewAttribute>(true);
						if (basic != null)
						{
							var c = newCategory("", 0f);
							c.Indent = false;
							c.HideHeader = true;
							visibleMembers.Foreach(c.AddMember);
							baseCategories.Add(c);
						}
						else
						{
							// Create the intial input from the target's visible members
							var input = new MemberGroup(visibleMembers, newMember);

							// Create resolvers
							var resolvers = new GroupResolver[]
							{
									new PatternResolver(),		new MemberTypesResolver(),
									new ReturnTypeResolver(),	new ExplicitMemberAddResolver(),
									new CategoryMembersResolver()
							};
							var core = new CoreResolver(resolvers, () => new MemberGroup(newMember));

							Action<DefineCategoryAttribute, MemberCategory> resolve = (def, category) =>
								core.Resolve(input, def).Members
																.OrderBy(m => m.DisplayOrder)
																.ThenBy(m => m.Info.GetDataType().Name)
																.ThenBy(m => m.Info.Name)
															   .Foreach(category.AddMember);

							var multiple	 = targetType.GetCustomAttribute<DefineCategoriesAttribute>(true);
							var ignored		 = targetType.GetCustomAttributes<IgnoreCategoriesAttribute>(true);
							var definitions = targetType.GetCustomAttributes<DefineCategoryAttribute>(true);
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
								parent == null ? baseCategories :
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
											baseCategories.Add(current);
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
							baseCategories = baseCategories.OrderBy(x => x.DisplayOrder).ToList();
						}
					}

					// Set script reference
					script = serializedObject.FindProperty("m_Script");

					// Initialize settings
					{
						settings                 = new Settings();
						var display              = prefs.GetInt(id);
						settings.display         = display.HasValue ? (MembersDisplay)display.Value : Settings.DefaultDisplay;
						settings.serializerTypes = typeof(BaseSerializer).GetConcreteChildren().ToArray();
						settings.serializerNames = settings.serializerTypes.Select(t => t.GetCustomAttribute<DisplayNameAttribute>().name).ToArray();
						settings.serializerIndex = settings.serializerTypes.IndexOf((target as ISavable).SerializerType);
					}

					serializationData = targetType.Field("serializationData", Flags.InstancePrivate);

					OnInitialized();
				}
			}
		}

		public override void OnInspectorGUI()
		{
#if PROFILE
			Profiler.BeginSample(targetType.Name + " OnInspectorGUI");

			Profiler.BeginSample(targetType.Name + " Header");
#endif

			// Header
			{
				gui.Space(3f);
				gui._beginH(EditorStyles.toolbarButton);
				{
					gui.Space(10f);
					foldout = gui.Foldout(foldout);
					gui.Space(-11f);
					gui.ObjectField("Script", script.objectReferenceValue);
				}
				gui._endH();

				if (foldout)
				{
					gui.Space(-2f);
					gui._beginIndent(GUI.skin.textField);
					{
						gui.Space(2f);
						dbg = gui.Toggle("Debug", dbg);
						var display = settings.display;
						var mask = gui.EnumMaskFieldThatWorks(display, "Display");
						{
							var newValue = (MembersDisplay)mask;
							if (display != newValue)
							{
								settings.display = newValue;
								prefs.SetInt(id, mask);
							}
						}
						var selection = gui.Popup("Serializer", settings.serializerIndex,
							settings.serializerNames);
						{
							if (selection != settings.serializerIndex)
							{
								(target as ISavable).SerializerType = settings.serializerTypes[selection];
								settings.serializerIndex = selection;
							}
						}
						gui._beginH();
						{
							gui.Space(10f);
							gui._beginV();
							{
								gui.MemberField(serializationData, target, target, id);
							}
							gui._endV();
						}
						gui._endH();
					}
					gui._endIndent();
				}
			}

#if PROFILE
			Profiler.EndSample();
#endif

			EditorGUI.BeginChangeCheck();

#if PROFILE
			Profiler.BeginSample(targetType.Name + " Members");
#endif

			if (minimal)
			{
				for (int i = 0; i < visibleMembers.Count; i++)
					visibleMembers[i].Draw();
			}
			else
			{
				for (int i = 0; i < baseCategories.Count; i++)
				{
					var c = baseCategories[i];
					c.Display = settings.display;
					c.Draw();
				}
			}

			OnGUI();

#if PROFILE
			Profiler.EndSample();
#endif

			if (EditorGUI.EndChangeCheck())
			{ 
				EditorUtility.SetDirty(target);
				var savable = target as ISavable;
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
