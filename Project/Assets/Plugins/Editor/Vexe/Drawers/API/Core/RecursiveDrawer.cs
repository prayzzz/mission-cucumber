using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using Smooth.Slinq;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Helpers;
using Vexe.Runtime.Serialization;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.Drawers
{
	[CoreDrawer]
	public class RecursiveDrawer<T> : ObjectDrawer<T>
	{
		private int childCount;
		private string nullString;

		protected override void OnInitialized()
		{
			childCount = memberType.GetChildren().Count();
			nullString = string.Format("null ({0})", memberTypeName);
		}

		public override void OnGUI()
		{
			if (!DrawField()) return;

			if (dataMember.IsNull())
			{
				gui.HelpBox("Member value is null");
				return;
			}

			var members = GetMembers().Invoke(dmValue.GetType());
			if (members.IsEmpty())
			{
				gui.HelpBox("Object doesn't have any visible members");
				return;
			}

			gui._beginIndent();
			{
				for (int i = 0; i < members.Count; i++)
					gui.MemberField(members[i], dmValue, unityTarget, id, false);
			}
			gui._endIndent();
		}

		Func<T, string> _getFieldString;
		Func<T, string> GetFieldString()
		{
			return _getFieldString ?? (_getFieldString = new Func<T, string>(value =>
			{
				var field = string.Format("{0} ({1})", value.GetType().GetFriendlyName(), memberTypeName);
				var obj = value as UnityObject;
				if (obj != null)
					field = field.Insert(0, obj.name + " - ");
				return field;
			}).Memoize());
		}

		Func<Type, List<MemberInfo>> _getMembers;
		Func<Type, List<MemberInfo>> GetMembers()
		{
			return _getMembers ?? (_getMembers = new Func<Type, List<MemberInfo>>(type =>
						type.GetMembers(Flags.InstanceAnyVisibility).Slinq()
							 .Where(SerializationLogic.Default.IsVisibleMember)
							 .OrderBy(m => m.GetDataType().Name)
							 .OrderBy(m => m.Name)
							 .ToList()
						).Memoize());
		}

		private bool DrawField()
		{
			gui._beginH();
			{
				Foldout();
				gui.Space(-10.5f);
				Field();
				Buttons();
			}
			gui._endH();

			return foldout;
		}

		private void Field()
		{
			var value = dmValue;
			if (memberType.IsA<UnityObject>())
			{
				var obj  = value as UnityObject;
				dmValue  = (T)(object)gui.ObjectField(niceName, obj);
				var rect = gui.GetLastRect();
				{
					GUIHelper.PingField(rect, obj, MouseCursor.Link);
					GUIHelper.SelectField(rect, obj, EventsHelper.MouseEvents.IsRMB_MouseDown(), MouseCursor.Link);
				}
			}
			else
			{
				string field = value == null ? nullString : GetFieldString().Invoke(value);
				gui.TextField(niceName, field);
				if (value == null && !memberType.IsAbstract)
					TryCreateInstance(memberType);
			}
		}

		protected virtual void Buttons()
		{
			if (childCount <= 1)
			{
				gui._beginState(false);
				{
					gui.SelectionButton("Object doesn't have any children/implementers");
				}
				gui._endState();
				return;
			}

			var tabs = new List<Tab>();

			Action<Func<Type[]>, Action<Type>, string> newTypeTab = (getValues, create, title) =>
				tabs.Add(new Tab<Type>(
					@getValues    : getValues,
					@getCurrent   : () => { var x = dmValue; return x == null ? null : x.GetType(); },
					@setTarget    : newType => { if (newType == null) dmValue = default(T); else create(newType); },
					@getValueName : type => type.Name,
					@title        : title
				));

			if (memberType.IsInterface)
			{
				Action<Func<UnityObject[]>, string> newUnityTab = (getValues, title) =>
					tabs.Add(new Tab<UnityObject>(
						@getValues    : getValues,
						@getCurrent   : dataMember.As<UnityObject>,
						@setTarget    : dataMember.SetRaw,
						@getValueName : obj => obj.name + " (" + obj.GetType().Name + ")",
						@title        : title
					));

				newUnityTab(() => UnityObject.FindObjectsOfType<UnityObject>()
													  .OfType(memberType)
													  .ToArray(), "Scene");

				newUnityTab(() => PrefabHelper.GetComponentPrefabs(memberType)
														.ToArray(), "Prefabs");

				newTypeTab(() => ReflectionHelper.GetAllUserTypesOf(memberType)
														   .Where(t => t.IsA<UnityObject>())
														   .Where(t => !t.IsAbstract)
														   .ToArray(), TryCreateInstanceInGO, "Unity types");
			}

			newTypeTab(() => ReflectionHelper.GetAllUserTypesOf(memberType)
														.Disinclude(memberType.IsAbstract ? memberType : null)
														.Where(t => !t.IsA<UnityObject>())
														.ToArray(), TryCreateInstance, "System types");

			if (gui.SelectionButton())
				SelectionWindow.Show("Select a `" + memberTypeName + "` object", tabs.ToArray());
		}

		private void TryCreateInstanceInGO(Type newType)
		{
			TryCreateInstance(() => new GameObject("(new) " + newType.Name).AddComponent(newType));
		}

		private void TryCreateInstance(Type newType)
		{
			TryCreateInstance(() => newType.Instance<T>(rawTarget));
		}

		private void TryCreateInstance(Func<object> create)
		{
			try
			{
				dataMember.SetRaw(create());
				EditorHelper.RepaintAllInspectors();
				foldout = true;
			}
			catch (TargetInvocationException e)
			{
				Debug.LogError(string.Format("Couldn't create instance of type {0}. Make sure the type has an empty constructor. Message: {1}, Stacktrace: {2}", memberTypeName, e.Message, e.StackTrace));
			}
		}
	}
}
