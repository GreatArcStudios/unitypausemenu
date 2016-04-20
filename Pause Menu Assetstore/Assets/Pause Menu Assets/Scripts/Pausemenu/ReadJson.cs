using UnityEngine;
using System.Collections;
using System.IO;
namespace GreatArcStudios
{

    public class ReadJson : MonoBehaviour
    {

        internal static string fileName = "GameSettings.json";
        internal float musicVolume;
        internal float effectsVolume;
        internal float masterVolume;
        internal float shadowDistINI;
        internal float renderDistINI;
        internal float aaQualINI;
        internal float densityINI;
        internal float treeMeshAmtINI;
        internal float fovINI;
        internal int msaaINI;
        internal int vsyncINI;
        internal int textureLimit;
        static string jsonString;
        public void Start()
        {
            createSaveObject();
            LoadGameSettings();
        }
        public static ReadJson createSaveObject()
        {
            jsonString = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
            return JsonUtility.FromJson<ReadJson>(jsonString);
        }
        // Load Settings
        public void LoadGameSettings()
        {
            QualitySettings.antiAliasing = (int)aaQualINI;
            PauseManager.densityINI = densityINI;
            QualitySettings.shadowDistance = shadowDistINI;
            PauseManager.mainCamShared.farClipPlane = renderDistINI;
            PauseManager.treeMeshAmtINI = treeMeshAmtINI;
            PauseManager.mainCamShared.fieldOfView = fovINI;
            QualitySettings.antiAliasing = msaaINI;
            QualitySettings.vSyncCount = vsyncINI;
            PauseManager.lastTexLimit = textureLimit;
            PauseManager.beforeMaster = masterVolume;
            PauseManager.lastAudioMult = effectsVolume;
            PauseManager.lastMusicMult = musicVolume;
        }
    }
}