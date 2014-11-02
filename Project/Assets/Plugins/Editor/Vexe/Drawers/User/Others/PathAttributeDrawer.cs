using Assets.Plugins.Editor.Vexe.Drawers.API.Base;
using Assets.Plugins.Editor.Vexe.Other;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;

using UnityEditor;

using UnityEngine;

using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Helpers;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Others
{
    public class PathAttributeDrawer : CompositeDrawer<string, PathAttribute>
    {
        public override void OnMemberDrawn(Rect rect)
        {
            var drop = GUIHelper.RegisterFieldForDrop<Object>(rect);

            if (drop != null)
            {
                //Debug.Log("drop: " + drop.name + " value: " + dmValue + " rect " + rect);
                this.dmValue = this.GetPath(drop);
            }

            var e = Event.current;
            if (e != null && rect.Contains(e.mousePosition))
            {
                if (Event.current.control && EventsHelper.MouseEvents.IsMMB_MouseDown())
                {
                    SelectionWindow.Show(new Tab<GameObject>(
                        getValues: Resources.FindObjectsOfTypeAll<GameObject>,
                        getCurrent: () => null,
                        setTarget: input => this.dmValue = this.GetPath(input),
                        getValueName: target => target.name,
                        title: "Objects"
                    ));
                }
            }
        }

        private string GetPath(Object input)
        {
            if (this.attribute.UseFullPath)
            {
                if (EditorHelper.IsSceneObject(input))
                {
                    var comp = input as Component;
                    var go = comp != null ? comp.gameObject : input as GameObject;
                    if (go != null)
                    {
                        var parents = go.GetParents();
                        return parents.IsNullOrEmpty() ? go.name :
                            parents.JoinString("/", p => p.name) + "/" + go.name;
                    }
                }
                else
                {
                    string fullPath = AssetDatabase.GetAssetPath(input);
                    return this.attribute.AbsoluteAssetPath ? fullPath :
                        fullPath.Remove(0, fullPath.IndexOf('/') + 1);
                }
            }
            return input.name;
        }
    }
}