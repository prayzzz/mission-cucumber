using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;

namespace Assets.Plugins.Editor.Vexe.CustomEditors.Internal
{
    public abstract class GroupResolver
    {
        public Func<MemberGroup> newGroup { get; set; }

        public abstract MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition);
    }

    public abstract class MembersResolver : GroupResolver { }
    public abstract class DefintionResolver : GroupResolver { }
}