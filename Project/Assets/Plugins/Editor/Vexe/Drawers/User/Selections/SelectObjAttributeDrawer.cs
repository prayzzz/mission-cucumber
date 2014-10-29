using System;
using System.Linq;
using UnityEngine;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;
using UnityObject = UnityEngine.Object;

namespace Vexe.Editor.Framework.Drawers
{
	public class SelectObjAttributeDrawer : AttributeDrawer<UnityObject, SelectObjAttribute>
	{
		public override void OnGUI()
		{
			gui._beginH();
			{
				bool isNull = dataMember.IsNull();
				bool isObjectField = BetterEditorPrefs.sGetSafeBool(id);

				gui.TextField(niceName, isNull ? "null" : dmValue.name + " (" + memberTypeName + ")");

				var fieldRect = gui.GetLastRect();
				{
					GUIHelper.PingField(fieldRect, dmValue, !isNull && isObjectField);
				}

				if (gui.SelectionButton("object"))
				{
					Func<UnityObject[], string, Tab> newTab = (values, title) =>
							new Tab<UnityObject>(
								@getValues    : () => values,
								@getCurrent   : dataMember.Get,
								@setTarget    : dataMember.Set,
								@getValueName : obj => obj.name,
								@title        : title
							);

					bool isGo =  memberType == typeof(GameObject);
					SelectionWindow.Show("Select a " + memberTypeName,
						newTab(UnityObject.FindObjectsOfType(memberType), "All"),
						newTab(isGo ?  (UnityObject[])gameObject.GetChildren() :
							gameObject.GetComponentsInChildren(memberType), "Children"),
						newTab(isGo ?  (UnityObject[])gameObject.GetParents() :
							gameObject.GetComponentsInParent(memberType), "Parents"),
						newTab(isGo ? PrefabHelper.GetGameObjectPrefabs().ToArray() :
								PrefabHelper.GetComponentPrefabs(memberType).Cast<UnityObject>().ToArray(), "Prefabs")
					);
				}
			}
			gui._endH();
		}
	}
}