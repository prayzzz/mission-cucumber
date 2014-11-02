using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Popups;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Selections;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Vectors;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.ScriptableObjects
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