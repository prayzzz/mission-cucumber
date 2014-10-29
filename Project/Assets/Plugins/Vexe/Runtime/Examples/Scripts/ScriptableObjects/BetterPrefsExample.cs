using UnityEngine;
using System.Collections;
using System.IO;

namespace Vexe.Runtime.Types.Examples
{
	public class BetterPrefsExample : BetterBehaviour
	{
		public BetterPrefs prefs;

		[Show] void SaveVectorsToPrefs()
		{
			// save position, euler angles and local scale to prefs
			prefs.Vector3s["MyPosition"] = position;
			prefs.Vector3s["MyRotation"] = localEulerAngles;
			prefs.Vector3s["MyScale"]    = localScale;
		}

		[Show] void ReadVectorsFromPrefsAndWriteToFile()
		{
			// read values...
			var pos = prefs.Vector3s["MyPosition"];
			var rot = prefs.Vector3s["MyRotation"];
			var scl = prefs.Vector3s["MyScale"];

			// log them
			Log("Position: {0} - Rotation {1} - Scale {2}", pos, rot, scl);

			// maybe serialize and write them to a file?
			string serializedPos = Serializer.Serialize<Vector3>(pos);
			string serializedRot = Serializer.Serialize<Vector3>(rot);
			string serializedScl = Serializer.Serialize<Vector3>(scl);
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
