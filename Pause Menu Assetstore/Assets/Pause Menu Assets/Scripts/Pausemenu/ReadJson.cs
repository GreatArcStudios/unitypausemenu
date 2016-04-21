using UnityEngine;
using System.Collections;
using System.IO;
namespace GreatArcStudios
{
    [System.Serializable]
    public class ReadJson
    {

        internal static string fileName = "GameSettings.json";
        internal static float musicVolume;
        internal static float effectsVolume;
        internal static float masterVolume;
        internal static float shadowDistINI;
        internal static float renderDistINI;
        internal static float aaQualINI;
        internal static float densityINI;
        internal static float treeMeshAmtINI;
        internal static float fovINI;
        internal static int msaaINI;
        internal static int vsyncINI;
        internal static int textureLimit;

        public static ReadJson createJSONOBJ(string jsonString)
        {
            return JsonUtility.FromJson<ReadJson>(jsonString);

        }


   }
}