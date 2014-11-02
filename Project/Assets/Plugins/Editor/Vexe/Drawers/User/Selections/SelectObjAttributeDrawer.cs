using System;
using System.Linq;

using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.Other;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Selections;

using UnityEngine;

using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

using Object = UnityEngine.Object;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Selections
{
    public class SelectObjAttributeDrawer : AttributeDrawer<Object, SelectObjAttribute>
    {
        public override void OnGUI()
        {
            this.gui._beginH();
            {
                bool isNull = this.dataMember.IsNull();
                bool isObjectField = BetterEditorPrefs.sGetSafeBool(this.id);

                this.gui.TextField(this.niceName, isNull ? "null" : this.dmValue.name + " (" + this.memberTypeName + ")");

                var fieldRect = this.gui.GetLastRect();
                {
                    GUIHelper.PingField(fieldRect, this.dmValue, !isNull && isObjectField);
                }

                if (this.gui.SelectionButton("object"))
                {
                    Func<Object[], string, Tab> newTab = (values, title) =>
                            new Tab<Object>(
                                getValues    : () => values,
                                getCurrent   : this.dataMember.Get,
                                setTarget    : this.dataMember.Set,
                                getValueName : obj => obj.name,
                                title        : title
                            );

                    bool isGo =  this.memberType == typeof(GameObject);
                    SelectionWindow.Show("Select a " + this.memberTypeName,
                        newTab(Object.FindObjectsOfType(this.memberType), "All"),
                        newTab(isGo ?  (Object[])this.gameObject.GetChildren() :
                            this.gameObject.GetComponentsInChildren(this.memberType), "Children"),
                        newTab(isGo ?  (Object[])this.gameObject.GetParents() :
                            this.gameObject.GetComponentsInParent(this.memberType), "Parents"),
                        newTab(isGo ? PrefabHelper.GetGameObjectPrefabs().ToArray() :
                                PrefabHelper.GetComponentPrefabs(this.memberType).Cast<Object>().ToArray(), "Prefabs")
                    );
                }
            }
            this.gui._endH();
        }
    }
}