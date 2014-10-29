using UnityEngine;
using ProtoBuf;

namespace Vexe.Runtime.Types.Examples
{
	[DefineCategory("Serializable")]
	[DefineCategory("NotSerializable")]
	public class Rules : BetterBehaviour
	{
		private const string Serializable = "Serializable";
		private const string NotSerializable = "NotSerializable";

		// Not serializable nor visible
		#region
		private int notSerializableNorVisible1;
		protected int notSerializableNorVisible2;
		private int notSerializableNorVisible3 { get; set; }
		protected int notSerializableNorVisible4 { get; set; }
		#endregion

		// Not serializable but visible
		#region
		[Show, Category(NotSerializable), Comment("This is a cool int")]
		private int notSerializableVisible1;

		[Show, Category(NotSerializable)]
		protected int notSerializableVisible2;

		[Show, Category(NotSerializable)]
		private int notSerializableVisible3 { get; set; }

		[Show, Category(NotSerializable)]
		protected int notSerializableVisible4 { get; set; }
		#endregion 

		// Serializable and visible
		#region
		[Save, Category(Serializable)]
		private int serializableVisible1;

		[Serialize, Category(Serializable)]
		private int serializableVisible2;

		[SerializeField, Category(Serializable)]
		private int serializableVisible3;

		[Save, Category(Serializable)]
		private int serializableVisible4 { get; set; }

		[Serialize, Category(Serializable)]
		private int serializableVisible5 { get; set; }

		[SerializeField, Category(Serializable)]
		private int serializableVisible6 { get; set; }
		#endregion

		// Serializble but not visible
		#region
		[Hide]
		public int serializableNotVisible1;
		[Save, Hide]
		private int serializableNotVisible2;
		[SerializeField, HideInInspector]
		protected int serializableNotVisible3;
		[Serialize, Hide]
		private int serializableNotVisible4 { get; set; }

		[Show] private void SetHiddenSerializableValues()
		{
			serializableNotVisible1 = 10;
			serializableNotVisible2 = 15;
			serializableNotVisible3 = 20;
			serializableNotVisible4 = 25;
		}
		[Show] private void PrintHiddenSerializableValues()
		{
			Log(serializableNotVisible1);
			Log(serializableNotVisible2);
			Log(serializableNotVisible3);
			Log(serializableNotVisible4);
		}
		#endregion
	}
}