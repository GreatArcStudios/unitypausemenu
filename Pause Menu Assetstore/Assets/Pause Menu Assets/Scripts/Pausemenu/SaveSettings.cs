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
        public static  float musicVolume;
        public static  float effectsVolume;
        public static  float masterVolume;
        public static  float shadowDistINI;
        public static  float renderDistINI;
        public static  float aaQualINI;
        public static  float densityINI;
        public static  float treeMeshAmtINI;
        public static  float fovINI;
        public static  int msaaINI;
        public static  int vsyncINI;
        public static  int textureLimit;
        static string jsonString;
        // Load Settings
        public static void LoadGameSettings()
        {
            jsonString = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
            JsonUtility.FromJson<SaveSettings>(jsonString);
            QualitySettings.antiAliasing = (int)aaQualINI;
            PauseManager.densityINI = densityINI;
           QualitySettings.shadowDistance = shadowDistINI;
            //PauseManager.mainCamShared.farClipPlane = renderDistINI;
            PauseManager.treeMeshAmtINI = treeMeshAmtINI ;
           // PauseManager.mainCamShared.fieldOfView = fovINI;
            QualitySettings.antiAliasing = msaaINI ;
            QualitySettings.vSyncCount =vsyncINI;
            PauseManager.lastTexLimit = textureLimit;
            PauseManager.beforeMaster = masterVolume;
            PauseManager.lastAudioMult =effectsVolume;
            PauseManager.lastMusicMult =musicVolume;
        }
        //Save Settings
        public  void SaveGameSettings()
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
            jsonString = JsonUtility.ToJson(this, true);
            File.WriteAllText(Application.persistentDataPath + "/" + fileName, jsonString); 
            
        }
       
    }
}