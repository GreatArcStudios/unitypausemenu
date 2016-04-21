using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
///  Copyright (c) 2016 Eric Zhu 
/// </summary>
namespace GreatArcStudios
{
    [System.Serializable]
    public class SaveSettings : MonoBehaviour
    {
        public string fileName = "GameSettings.json";
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
        string jsonString;

        public void Start()
        {
            LoadGameSettings();

        }

        // Load Settings
        public void LoadGameSettings()
        {
            jsonString = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
           
            ReadJson.createJSONOBJ(jsonString);
            QualitySettings.antiAliasing = (int)aaQualINI;
            PauseManager.densityINI = densityINI;
            QualitySettings.shadowDistance = shadowDistINI;
            Debug.Log(ReadJson.renderDistINI);
            //PauseManager.mainCamShared.farClipPlane = renderDistINI;
            PauseManager.treeMeshAmtINI = treeMeshAmtINI;
            // PauseManager.mainCamShared.fieldOfView = fovINI;
            QualitySettings.antiAliasing = msaaINI;
            QualitySettings.vSyncCount = vsyncINI;
            PauseManager.lastTexLimit = textureLimit;
            PauseManager.beforeMaster = masterVolume;
            PauseManager.lastAudioMult = effectsVolume;
            PauseManager.lastMusicMult = musicVolume;
        }
        //Save Settings
        public void SaveGameSettings()
        {
            if (File.Exists(Application.persistentDataPath + "/" + fileName))
            {
                File.Delete(Application.persistentDataPath + "/" + fileName);
            }
            aaQualINI = QualitySettings.antiAliasing;
            densityINI = PauseManager.densityINI;
            shadowDistINI = PauseManager.shadowDistINI;
            renderDistINI = PauseManager.mainCamShared.farClipPlane;
            treeMeshAmtINI = PauseManager.treeMeshAmtINI;
            fovINI = PauseManager.mainCamShared.fieldOfView;
            msaaINI = PauseManager.msaaINI;
            vsyncINI = PauseManager.vsyncINI;
            textureLimit = PauseManager.lastTexLimit;
            masterVolume = PauseManager.beforeMaster;
            effectsVolume = PauseManager.lastAudioMult;
            musicVolume = PauseManager.lastMusicMult;
            jsonString = JsonUtility.ToJson(this);
            File.WriteAllText(Application.persistentDataPath + "/" + fileName, jsonString);

        }

    }
}