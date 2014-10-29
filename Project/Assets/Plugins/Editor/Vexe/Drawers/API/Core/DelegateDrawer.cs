using System;
using System.Linq;
using System.Reflection;
using Fasterflect;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Framework.GUIs;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	[CoreDrawer]
	public class DelegateDrawer : ObjectDrawer<BaseDelegate>
	{
		private AddingData adding;
		private DataMember[] argMembers;
		private object[] argValues;
		private string[] ignored;
		private string headerString;

		protected override void OnInitialized()
		{
			ignored = DelegateSettings.IgnoredMethods;

			// initialize adding area and foldout keys
			adding       = new AddingData();
			kAdvancedKey = id + "advanced";
			kAddKey      = id + "add";
			kInvokeKey   = id + "invoke";
			headerString = string.Format("{0} ({1})", niceName, memberTypeName);

			// make sure delegate is not null
			if (dmValue == null)
				 dmValue = memberType.Instance<BaseDelegate>();

			// initialize arguments data (used when invoking the delegate from the editor)
			int len      = dmValue.ParamTypes.Length;
			argValues    = new object[len];
			argMembers   = new DataMember[len];

			for (int iLoop = 0; iLoop < len; iLoop++)
			{
				int i = iLoop;
				var paramType = dmValue.ParamTypes[i];

				argValues[i] = paramType.GetDefaultValueEmptyIfString();

				var elementInfo = new DataInfo(
						@id            : id + paramType.FullName + i,
						@getter        : () => argValues[i],
						@setter        : x => argValues[i] = x,
						@attributes    : new Attribute[0],
						@name          : string.Format("({0})", paramType.GetFriendlyName()),
						@elementType   : paramType,
						@declaringType : targetType,
						@reflectedType : GetType()
					);

				argMembers[i] = new DataMember(elementInfo, rawTarget);
			}

			dbgLog("Delegate drawer init");
		}

		public override void OnGUI()
		{
			foldout = gui.Foldout(headerString, foldout, GUILayout.ExpandWidth(true));
			if (!foldout) return;

			// read
			var handlers = dmValue.handlers;

			gui._beginV(GUI.skin.box);
			{

				// header
				{
					gui._beginH();
					{
						gui.BoldLabel("Target :: Handler");
						gui._beginState(foldout && handlers.Count > 0);
						{
							chkAdvanced = gui.CheckButton(chkAdvanced, "advanced mode", MiniButtonStyle.Right);
						}
						gui._endState();
					}
					gui._endH();
				}

				gui.Splitter();

				// body
				{
					// target : handler
					if (handlers.Count == 0)
					{
						gui.HelpBox("There are no handlers for this delegate");
					}
					else
					{
						for (int iLoop = 0; iLoop < handlers.Count; )
						{
							var i = iLoop;
							var handler = handlers[i];
							var removed = false;
							gui._beginH();
							{
								gui.ObjectField(handler.target);
								gui.TextField(handler.method);
								if (chkAdvanced)
								{
									if (gui.MoveDownButton())
										handlers.MoveElementDown(i);
									if (gui.MoveUpButton())
										handlers.MoveElementUp(i);
									if (gui.RemoveButton("handler", MiniButtonStyle.ModRight))
									{
										handlers.RemoveAt(i);
										removed = true;
									}
								}
							}
							gui._endH();
							if (!removed) iLoop++;
						}
					}
				}

				gui.Splitter();

				// footer
				{
					// add> gameObject :: invoke>
					gui._beginH();
					{
						gui._beginState(foldoutAdd && adding.gameObject != null && adding.component != null && adding.method != null);
						{
							if (gui.Button("Add", "Add new handler", EditorStyles.miniButton, GUILayout.Width(40f)))
							{
								handlers.Add(new BaseDelegate.Handler
								{
									target = adding.component,
									method = adding.method.Name
								});
							}
						}
						gui._endState();

						gui.Space(10f);
						foldoutAdd = gui.Foldout(foldoutAdd);
						// gameObject
						gui.FlexibleSpace();
						var newGo = gui.ObjectField(adding.gameObject);
						{
							if (adding.gameObject != newGo)
							{
								adding.gameObject = newGo;
								foldoutAdd = true;
							}
						}
						gui.FlexibleSpace();
						bool paramless = argMembers.IsEmpty();
						gui._beginState(foldoutInvoke || paramless);
						{
							if (gui.Button("Invoke", EditorStyles.miniButtonRight, GUILayout.Width(50f)))
							{
								for (int i = 0; i < handlers.Count; i++)
								{
									var handler = handlers[i];
									var method = handler.target.GetType().DelegateForCallMethod(handler.method, dmValue.ParamTypes);
									method.Invoke(handler.target, argValues);
								}
							}
						}
						gui._endState();
						gui.Space(8f);
						if (!paramless && handlers.Count > 0)
							foldoutInvoke = gui.Foldout(foldoutInvoke);
					}
					gui._endH();

					// adding area: component -- method
					if (foldoutAdd && adding.gameObject != null)
					{
						gui.Space(5f);
						gui.Label("Add handler:");
						gui._beginIndent(GUI.skin.textArea);
						{
							// component
							if (adding.gameObject != null)
							{
								var components   = adding.gameObject.GetAllComponents();
								int cIndex       = components.IndexOfZeroIfNotFound(adding.component);
								var uniqueNames  = components.Select(c => c.GetType().Name).ToList().Uniqify();
								var targetSelection = gui.Popup("Target", cIndex, uniqueNames.ToArray());
								{
									if (cIndex == targetSelection && adding.component == components[targetSelection]) return;
									adding.component = components[targetSelection];
								}

								// method
								if (adding.component != null)
								{
									var methods = adding.component.GetType()
																			.GetMethods(dmValue.ReturnType, dmValue.ParamTypes, Flags.InstancePublic, false)
																			.Where(m => !m.IsDefined<HideAttribute>())
																			.Where(m => !ignored.Contains(m.Name))
																			.ToList();
									int mIndex  = methods.IndexOfZeroIfNotFound(adding.method);
									int methodSelection = gui.Popup("Handler", mIndex, methods.Select(m => m.GetFullName()).ToArray());
									{
										if (methods.IsEmpty() || (mIndex == methodSelection && adding.method == methods[methodSelection])) return;
										adding.method = methods[methodSelection];
									}
								}
							}
						}
						gui._endIndent();
					}

					// invocation args
					if (foldoutInvoke)
					{
						gui.Label("Invoke with arguments:");
						gui._beginIndent();
						{
							for (int i = 0; i < argMembers.Length; i++)
								gui.MemberField(argMembers[i], unityTarget, id, false);
						}
						gui._endIndent();
					}
				}
			}
			gui._endV();

			// write
			dmValue.handlers = handlers;
		}

		private struct AddingData
		{
			public GameObject gameObject;
			public Component component;
			public MethodInfo method;
		}

		// Keys & foldouts
		#region
		private string kAdvancedKey;
		private string kAddKey;
		private string kInvokeKey;

		private bool foldoutAdd
		{
			get { return prefs.GetSafeBool(kAddKey);}
			set { prefs.SetBool(kAddKey, value); }
		}
		private bool chkAdvanced
		{
			get { return prefs.GetSafeBool(kAdvancedKey); }
			set { prefs.SetBool(kAdvancedKey, value); }
		}
		private bool foldoutInvoke
		{
			get { return prefs.GetSafeBool(kInvokeKey); }
			set { prefs.SetBool(kInvokeKey, value); }
		}
		#endregion
	}
}