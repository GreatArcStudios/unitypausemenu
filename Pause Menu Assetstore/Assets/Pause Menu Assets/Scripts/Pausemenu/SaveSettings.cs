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
        public  string fileName = "GameSettings.json";
        public  float musicVolume;
        public  float effectsVolume;
        public  float masterVolume;
        public  float shadowDistINI;
        public  float renderDistINI;
        public  float aaQualINI;
        public  float densityINI;
        public  float treeMeshAmtINI;
        public  float fovINI;
        public  int msaaINI;
        public  int vsyncINI;
        public  int textureLimit;
        string jsonString;
        // Load Settings
        public  void LoadGameSettings()
        {
            JsonUtility.FromJson<SaveSettings>(jsonString);
            PauseManager.aaQualINI = aaQualINI;
            PauseManager.densityINI = densityINI;
            PauseManager.shadowDistINI = shadowDistINI;
            PauseManager.renderDistINI = renderDistINI;
            PauseManager.treeMeshAmtINI = treeMeshAmtINI ;
            PauseManager.fovINI = fovINI;
            PauseManager.msaaINI = msaaINI ;
            PauseManager.vsyncINI =vsyncINI;
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
            renderDistINI = PauseManager.renderDistINI;
            treeMeshAmtINI = PauseManager.treeMeshAmtINI;
            fovINI = PauseManager.fovINI;
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