using System.Collections.Generic;
using UnityEngine;

namespace Vexe.Runtime.Types.Examples
{
	[BasicView]
	public class UnityTypesExample : BetterBehaviour
	{
		public Vector3 vector3;
		public Color color { get; set; }
		public Quaternion quaternion;

		[Save]
		private Vector2 vector2 { get; set; }

		[Serialize]
		protected Bounds bounds;

		[SerializeField]
		private LayerMask mask;
	}
}