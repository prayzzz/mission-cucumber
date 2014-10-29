using System;
using Vexe.Runtime.Types;

namespace Vexe.Editor.BetterBehaviourInternal
{
	public abstract class GroupResolver
	{
		public Func<MemberGroup> newGroup { get; set; }

		public abstract MemberGroup Resolve(MemberGroup input, DefineCategoryAttribute definition);
	}

	public abstract class MembersResolver : GroupResolver { }
	public abstract class DefintionResolver : GroupResolver { }
}