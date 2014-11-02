using System.IO;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Core;
using Assets.Plugins.Vexe.Runtime.Types.Other;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.ScriptableObjects
{
    public class BetterPrefsExample : BetterBehaviour
    {
        public BetterPrefs prefs;

        [Show] void SaveVectorsToPrefs()
        {
            // save position, euler angles and local scale to prefs
            this.prefs.Vector3s["MyPosition"] = this.position;
            this.prefs.Vector3s["MyRotation"] = this.localEulerAngles;
            this.prefs.Vector3s["MyScale"]    = this.localScale;
        }

        [Show] void ReadVectorsFromPrefsAndWriteToFile()
        {
            // read values...
            var pos = this.prefs.Vector3s["MyPosition"];
            var rot = this.prefs.Vector3s["MyRotation"];
            var scl = this.prefs.Vector3s["MyScale"];

            // log them
            Log("Position: {0} - Rotation {1} - Scale {2}", pos, rot, scl);

            // maybe serialize and write them to a file?
            string serializedPos = this.Serializer.Serialize<Vector3>(pos);
            string serializedRot = this.Serializer.Serialize<Vector3>(rot);
            string serializedScl = this.Serializer.Serialize<Vector3>(scl);
            using (var file = File.Open("Assets/Vexe/Runtime/Examples/Assets/Sample.data", FileMode.OpenOrCreate))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.WriteLine(serializedPos);
                    writer.WriteLine(serializedRot);
                    writer.WriteLine(serializedScl);
                }
            }
        }
    }
}
