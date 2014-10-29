using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System;
using Vexe.Runtime.Extensions;

namespace Vexe.Editor.BetterBehaviourInternal
{
	public class MemberGroup
	{
		public List<VisibleMember> Members { get; private set; }
		private readonly Func<MemberInfo, VisibleMember> newMember;

		public MemberGroup(List<VisibleMember> members, Func<MemberInfo, VisibleMember> newMember)
		{
			Members = members;
			this.newMember = newMember;
		}

		public MemberGroup(Func<MemberInfo, VisibleMember> newMember)
			: this(new List<VisibleMember>(), newMember)
		{
		}

		public void AddMember(MemberInfo m)
		{
			Members.Add(newMember(m) as VisibleMember);
		}

		public void UnionMembers(IEnumerable<VisibleMember> members)
		{
			Members = Members.Union(members).ToList();
		}

		public void AddMembers(IEnumerable<VisibleMember> members)
		{
			members.Foreach(m => AddMember(m.Info));
		}

		public void AddMembers(MemberGroup input, Func<VisibleMember, bool> predicate)
		{
			input.Members.Where(predicate).Foreach(m => AddMember(m.Info));
		}
	}
}