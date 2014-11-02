using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Requirements;

using UnityEditor;

using UnityEngine;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.Drawers.User.Requirements
{
    public abstract class RequiredFromRelativeAttributeDrawer<T> : BaseRequirementAttributeDrawer<T> where T : RequiredFromRelativeAttribute
    {
        protected abstract string relative { get; }
        protected abstract string plural { get; }

        protected abstract Component GetComponentInRelative(GameObject from, Type componentType);
        protected abstract GameObject GetRelativeAtPath(GameObject from, string path, bool throwIfNotFound);
        protected abstract GameObject GetOrAddRelativeAtPath(GameObject from, string path);

        protected override Component GetComponent(GameObject from, Type componentType)
        {
            Component c = null;
            var path = this.attribute.Path;
            if (path.IsNullOrEmpty())
            {
                c = this.GetComponentInRelative(from, componentType);
                if (c == null)
                    this.gui.HelpBox(string.Format("Couldn't find component `{0}` in {1}", componentType.Name, this.plural), MessageType.Warning);
            }
            else
            {
                Action<GameObject> handleRelative = relative =>
                {
                    if (relative == null) return;
                    c = relative.GetComponent(componentType);
                    if (c == null && this.attribute.Add)
                    {
                        if (componentType.IsAbstract)
                            this.gui.HelpBox("Can't add component `" + componentType.Name + "` because it's abstract", MessageType.Warning);
                        else
                            c = relative.AddComponent(componentType);
                    }
                };

                handleRelative(this.attribute.Create ?
                    this.GetOrAddRelativeAtPath(from, path) :
                    this.GetRelativeAtPath(from, path, false)
                );
            }
            return c;
        }

        protected override GameObject GetGO(GameObject from)
        {
            string path = this.ProcessPath();
            if (path.IsNullOrEmpty())
            {
                this.gui.HelpBox(string.Format("No {0} path specified - Can't get {0} GameObject", this.relative), MessageType.Warning);
                return null;
            }
            try
            {
                return this.attribute.Create ?
                    this.GetOrAddRelativeAtPath(from, path) :
                    this.GetRelativeAtPath(from, path, true);
            }
            catch
            {
                this.gui.HelpBox(this.relative + " not found at the specified path `" + path + "`", MessageType.Warning);
                return null;
            }
        }

        private string ProcessPath()
        {
            string p = this.attribute.Path;
            if (p == "$Name")
            {
                return this.memberInfo.Name.SplitPascalCase().Split(' ')[0];
            }
            if (p == "$name")
            {
                return this.memberInfo.Name.SplitCamelCase().Split(' ')[0];
            }
            if (p == "$fullName")
            {
                return this.memberInfo.Name;
            }
            if (p == "$FullName")
            {
                return this.memberInfo.Name.ToUpperAt(0);
            }
            if (p == "$Full Name")
            {
                return this.memberInfo.Name.SplitPascalCase();
            }
            return p;
        }
    }
}