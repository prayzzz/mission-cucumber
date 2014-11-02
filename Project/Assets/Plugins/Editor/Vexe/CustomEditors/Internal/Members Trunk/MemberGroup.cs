using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Editor.Vexe.CustomEditors.Internal
{
    public class MemberGroup
    {
        public List<VisibleMember> Members { get; private set; }
        private readonly Func<MemberInfo, VisibleMember> newMember;

        public MemberGroup(List<VisibleMember> members, Func<MemberInfo, VisibleMember> newMember)
        {
            this.Members = members;
            this.newMember = newMember;
        }

        public MemberGroup(Func<MemberInfo, VisibleMember> newMember)
            : this(new List<VisibleMember>(), newMember)
        {
        }

        public void AddMember(MemberInfo m)
        {
            this.Members.Add(this.newMember(m) as VisibleMember);
        }

        public void UnionMembers(IEnumerable<VisibleMember> members)
        {
            this.Members = this.Members.Union(members).ToList();
        }

        public void AddMembers(IEnumerable<VisibleMember> members)
        {
            members.Foreach(m => this.AddMember(m.Info));
        }

        public void AddMembers(MemberGroup input, Func<VisibleMember, bool> predicate)
        {
            input.Members.Where(predicate).Foreach(m => this.AddMember(m.Info));
        }
    }
}