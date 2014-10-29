using System;

namespace Vexe.Editor.BetterBehaviourInternal
{
	public interface ICanBeDrawn
	{
		float DisplayOrder { get; set; }
		string Name { get; }
		void Draw();
		void HeaderSpace();
	}
}