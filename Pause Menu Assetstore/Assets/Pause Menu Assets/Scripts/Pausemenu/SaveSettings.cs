using System;
using System.IO;
using UnityEngine;
/// <summary>
///  Copyright (c) 2016 Eric Zhu 
/// </summary>
namespace GreatArcStudios
{
    [System.Serializable]
    public class SaveSettings
    {
        /// <summary>
        /// Change the file name if something else floats your boat
        /// </summary>
        public string fileName = "GameSettings.json";
        /// <summary>
        /// Music volume
        /// </summary>
        public float musicVolume;
        /// <summary>
        /// Effects volume
        /// </summary>
        public float effectsVolume;
        /// <summary>
        /// Master volume
        /// </summary>
        public float masterVolume;
        /// <summary>
        /// Shadow Distance
        /// </summary>
        public float shadowDistINI;
        /// <summary>
        /// Render distance
        /// </summary>
        public float renderDistINI;
        /// <summary>
        /// MSAA quality
        /// </summary>
        public float aaQualINI;
        /// <summary>
        /// Density
        /// </summary>
        public float densityINI;
        /// <summary>
        /// Terrain trees rendered as meshes amount
        /// </summary>
        public float treeMeshAmtINI;
        /// <summary>
        /// Camera FOV
        /// </summary>
        public float fovINI;
        /// <summary>
        /// Terrain heightmap quality
        /// </summary>
        public float terrainHeightMapLOD;
        public int msaaINI;
        /// <summary>
        /// VSync settings
        /// </summary>
        public int vsyncINI;
        /// <summary>
        /// Texture quality
        /// </summary>
        public int textureLimit;
        /// <summary>
        /// Quality preset
        /// </summary>
        public int curQualityLevel;
        /// <summary>
        /// Shadwo Cascade
        /// </summary>
        public int lastShadowCascade;
        /// <summary>
        /// Aniso texture level
        /// </summary>
        public int anisoLevel;
        /// <summary>
        /// AO on or off
        /// </summary>
        public bool aoBool;
        /// <summary>
        /// DOF on or off
        /// </summary>
        public bool dofBool;
        /// <summary>
        /// Use simple terrain or high quality terrain ie: RTP. 
        /// </summary>
        public bool useSimpleTerrain;
        /// <summary>
        /// Is the game in fullscreen
        /// </summary>
        public bool fullscreenBool;
        /// <summary>
        /// Resolution heigh
        /// </summary>
        public int resHeight;
        /// <summary>
        /// Resolution Width
        /// </summary>
        public int resWidth;

        /// <summary>
        /// Read the game settings from the file
        /// </summary>
        /// <param name="readString"></param>
        public void LoadGameSettings(String readString)
        {
            SaveSettings read = JsonUtility.FromJson<SaveSettings>(readString);
            QualitySettings.antiAliasing = (int)read.aaQualINI;
            PauseManager.densityINI = read.densityINI;
            QualitySettings.shadowDistance = read.shadowDistINI;
            PauseManager.mainCamShared.farClipPlane = read.renderDistINI;
            PauseManager.treeMeshAmtINI = read.treeMeshAmtINI;
            PauseManager.mainCamShared.fieldOfView = read.fovINI;
            QualitySettings.antiAliasing = read.msaaINI;
            QualitySettings.vSyncCount = read.vsyncINI;
            PauseManager.lastTexLimit = read.textureLimit;
            QualitySettings.masterTextureLimit = read.textureLimit;
            AudioListener.volume = read.masterVolume;
            PauseManager.lastAudioMult = read.effectsVolume;
            PauseManager.lastMusicMult = read.musicVolume;
            PauseManager.dofBool = read.dofBool;
            PauseManager.aoBool = read.aoBool;
            QualitySettings.SetQualityLevel(read.curQualityLevel);
            QualitySettings.shadowCascades = read.lastShadowCascade;
            Screen.SetResolution(read.resWidth, read.resHeight, read.fullscreenBool);
            if (read.anisoLevel == 0)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            }
            else if (read.anisoLevel == 1)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
            }
            else if (read.anisoLevel == 2)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
            }
            try
            {
                if (read.useSimpleTerrain)
                {
                    PauseManager.readTerrain.heightmapMaximumLOD = (int)read.terrainHeightMapLOD;
                }
                else
                {
                    PauseManager.readSimpleTerrain.heightmapMaximumLOD = (int)read.terrainHeightMapLOD;
                }
                PauseManager.readUseSimpleTerrain = read.useSimpleTerrain;
            }
            catch
            {
                Debug.Log("Cannot read terain heightmap LOD because the terrain was not assigned.");
            }
        }



        /// <summary>
        /// Get the quality/music settings before saving 
        /// </summary>
        public void SaveGameSettings()
        {
            string jsonString;
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
            msaaINI = QualitySettings.antiAliasing;
            vsyncINI = PauseManager.vsyncINI;
            textureLimit = PauseManager.lastTexLimit;
            masterVolume = PauseManager.beforeMaster;
            effectsVolume = PauseManager.lastAudioMult;
            musicVolume = PauseManager.lastMusicMult;
            aoBool = PauseManager.aoBool;
            dofBool = PauseManager.dofBool;
            curQualityLevel = QualitySettings.GetQualityLevel();
            lastShadowCascade = PauseManager.lastShadowCascade;
            resHeight = Screen.currentResolution.height;
            resWidth = Screen.currentResolution.width;
            fullscreenBool = Screen.fullScreen;
            if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable)
            {
                anisoLevel = 0;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable)
            {
                anisoLevel = 1;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable)
            {
                anisoLevel = 2;
            }
            try
            {
                if (PauseManager.readUseSimpleTerrain)
                {
                    terrainHeightMapLOD = PauseManager.readTerrain.heightmapMaximumLOD;
                }
                else
                {
                    terrainHeightMapLOD = PauseManager.readSimpleTerrain.heightmapMaximumLOD;
                }
            }
            catch
            {
                Debug.Log("Cannot save terain heightmap LOD because the terrain was not assigned.");
            }
            useSimpleTerrain = PauseManager.readUseSimpleTerrain;
            jsonString = JsonUtility.ToJson(this);
            Debug.Log(jsonString);
            File.WriteAllText(Application.persistentDataPath + "/" + fileName, jsonString);
        }


    }
}