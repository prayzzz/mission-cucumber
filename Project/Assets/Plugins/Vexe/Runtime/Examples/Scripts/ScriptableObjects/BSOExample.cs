using UnityEngine;
using Vexe.Runtime.Types;

namespace Vexe.Runtime.Types.Examples
{
#if UNITY_EDITOR
	public static class BSOMenuItem
	{
		[UnityEditor.MenuItem("Tools/Vexe/Create BSO Asset")]
		public static void CreateAsset()
		{
			var ex = ScriptableObject.CreateInstance<BSOExample>();
			UnityEditor.AssetDatabase.CreateAsset(ex, "Assets/Vexe/Runtime/Examples/BSO.asset");
		}
	}
#endif

	public class BSOExample : BetterScriptableObject
	{
		[Save, Tags, PerItem]
		private string[] tags;

		[SelectEnum]
		public KeyCode Jump { get; set; }

		[BetterVector]
		public Vector3 Target { get; set; }

		[Show]
		private void method()
		{
			Debug.Log("method");
		}
	}
}