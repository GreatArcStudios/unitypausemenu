using UnityEngine;
using System.Collections;
using System.IO;
namespace GreatArcStudios
{
    [System.Serializable]
    public class ReadJson
    {

        public  string fileName = "GameSettings.json";
        public float musicVolume;
        public float effectsVolume;
        public float masterVolume;
        public float shadowDistINI;
        public float renderDistINI;
        public float aaQualINI;
        public float densityINI;
        public float treeMeshAmtINI;
        public float fovINI;
        public float terrainHeightMapLOD;
        public int msaaINI;
        public int vsyncINI;
        public int textureLimit;
        public int curQualityLevel;
        public bool aoBool;
        public bool dofBool;
        public bool useSimpleTerrain;
        /// <summary>
        /// Method to create JSON Object by passing through a json string. 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static ReadJson createJSONOBJ(string jsonString)
        {
            return JsonUtility.FromJson<ReadJson>(jsonString);

        }


   }
}