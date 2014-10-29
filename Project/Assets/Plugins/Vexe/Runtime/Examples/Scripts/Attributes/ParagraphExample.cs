namespace Vexe.Runtime.Types.Examples
{
	[MinimalView]
	public class ParagraphExample : BetterBehaviour
	{
		[Paragraph]
		public string p1;

		[Paragraph]
		public string P2 { get; set; }
	}
}