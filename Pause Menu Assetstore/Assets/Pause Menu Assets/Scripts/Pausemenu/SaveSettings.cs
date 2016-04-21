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
        public static string fileName = "GameSettings.json";
        public float musicVolume;
        public float effectsVolume;
        public float masterVolume;
        public float shadowDistINI;
        public float renderDistINI;
        public float aaQualINI;
        public float densityINI;
        public float treeMeshAmtINI;
        public float fovINI;
        public int msaaINI;
        public int vsyncINI;
        public int textureLimit;
        static string jsonString;
        public static string readString;
        public void Start()
        {
            try
            {
                LoadGameSettings();
            }
            catch (FileNotFoundException)
            {
                Debug.Log("Game settings not found in: " + Application.persistentDataPath + "/" + fileName);
            }
           
        }

        /// <summary>
        /// Load the same settings
        /// </summary>
        public void LoadGameSettings()
        {
            readString = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
            ReadJson read =  ReadJson.createJSONOBJ(readString);
            QualitySettings.antiAliasing = (int)read.aaQualINI;
            PauseManager.densityINI = read.densityINI;
            QualitySettings.shadowDistance = read.shadowDistINI;
            PauseManager.mainCamShared.farClipPlane = read.renderDistINI;
            PauseManager.treeMeshAmtINI = read.treeMeshAmtINI;
            PauseManager.mainCamShared.fieldOfView = read.fovINI;
            QualitySettings.antiAliasing = read.msaaINI;
            QualitySettings.vSyncCount = read.vsyncINI;
            PauseManager.lastTexLimit = read.textureLimit;
            PauseManager.beforeMaster = read.masterVolume;
            PauseManager.lastAudioMult = read.effectsVolume;
            PauseManager.lastMusicMult = read.musicVolume;
        }
        /// <summary>
        /// Get the quality/music settings before saving 
        /// </summary>
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