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
        public string FileName = "GameSettings.json";
        /// <summary>
        /// Music volume
        /// </summary>
        public float MusicVolume;
        /// <summary>
        /// Effects volume
        /// </summary>
        public float EffectsVolume;
        /// <summary>
        /// Master volume
        /// </summary>
        public float MasterVolume;
        /// <summary>
        /// Shadow Distance
        /// </summary>
        public float ShadowDistIni;
        /// <summary>
        /// Render distance
        /// </summary>
        public float RenderDistIni;
        /// <summary>
        /// MSAA quality
        /// </summary>
        public float AaQualIni;
        /// <summary>
        /// Density
        /// </summary>
        public float DensityIni;
        /// <summary>
        /// Terrain trees rendered as meshes amount
        /// </summary>
        public float TreeMeshAmtIni;
        /// <summary>
        /// Camera FOV
        /// </summary>
        public float FovIni;
        /// <summary>
        /// Terrain heightmap quality
        /// </summary>
        public float TerrainHeightMapLod;
        public int MsaaIni;
        /// <summary>
        /// VSync settings
        /// </summary>
        public int VsyncIni;
        /// <summary>
        /// Texture quality
        /// </summary>
        public int TextureLimit;
        /// <summary>
        /// Quality preset
        /// </summary>
        public int CurQualityLevel;
        /// <summary>
        /// Shadwo Cascade
        /// </summary>
        public int LastShadowCascade;
        /// <summary>
        /// Aniso texture level
        /// </summary>
        public int AnisoLevel;
        /// <summary>
        /// AO on or off
        /// </summary>
        public bool AoBool;
        /// <summary>
        /// DOF on or off
        /// </summary>
        public bool DofBool;
        /// <summary>
        /// Use simple terrain or high quality terrain ie: RTP. 
        /// </summary>
        public bool UseSimpleTerrain;
        /// <summary>
        /// Is the game in fullscreen
        /// </summary>
        public bool FullscreenBool;
        /// <summary>
        /// Resolution heigh
        /// </summary>
        public int ResHeight;
        /// <summary>
        /// Resolution Width
        /// </summary>
        public int ResWidth;

        /// <summary>
        /// Read the game settings from the file
        /// </summary>
        public void LoadGameSettings()
        {
            String readString = File.ReadAllText(Application.persistentDataPath + "/" + FileName);
            JsonUtility.FromJsonOverwrite(readString, this);
            QualitySettings.antiAliasing = (int)AaQualIni;
            PauseManager.DensityIni = DensityIni;
            QualitySettings.shadowDistance = ShadowDistIni;
            PauseManager.MainCamShared.farClipPlane = RenderDistIni;
            PauseManager.TreeMeshAmtIni = TreeMeshAmtIni;
            PauseManager.MainCamShared.fieldOfView = FovIni;
            QualitySettings.antiAliasing = MsaaIni;
            QualitySettings.vSyncCount = VsyncIni;
            PauseManager.LastTexLimit = TextureLimit;
            QualitySettings.masterTextureLimit = TextureLimit;
            AudioListener.volume = MasterVolume;
            PauseManager.LastAudioMult = EffectsVolume;
            PauseManager.LastMusicMult = MusicVolume;
            PauseManager.DofBool = DofBool;
            PauseManager.AoBool = AoBool;
            QualitySettings.SetQualityLevel(CurQualityLevel);
            QualitySettings.shadowCascades = LastShadowCascade;
            //Temp solution for recent resoultion problems 
            //Screen.SetResolution(resWidth, resHeight, fullscreenBool);
            if (AnisoLevel == 0)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            }
            else if (AnisoLevel == 1)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
            }
            else if (AnisoLevel == 2)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
            }
            try
            {
                if (UseSimpleTerrain)
                {
                    PauseManager.ReadTerrain.heightmapMaximumLOD = (int)TerrainHeightMapLod;
                }
                else
                {
                    PauseManager.ReadSimpleTerrain.heightmapMaximumLOD = (int)TerrainHeightMapLod;
                }
                PauseManager.ReadUseSimpleTerrain = UseSimpleTerrain;
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
            if (File.Exists(Application.persistentDataPath + "/" + FileName))
            {
                File.Delete(Application.persistentDataPath + "/" + FileName);
            }
            AaQualIni = QualitySettings.antiAliasing;
            DensityIni = PauseManager.DensityIni;
            ShadowDistIni = PauseManager.ShadowDistIni;
            RenderDistIni = PauseManager.MainCamShared.farClipPlane;
            TreeMeshAmtIni = PauseManager.TreeMeshAmtIni;
            FovIni = PauseManager.MainCamShared.fieldOfView;
            MsaaIni = QualitySettings.antiAliasing;
            VsyncIni = PauseManager.VsyncIni;
            TextureLimit = PauseManager.LastTexLimit;
            MasterVolume = PauseManager.BeforeMaster;
            EffectsVolume = PauseManager.LastAudioMult;
            MusicVolume = PauseManager.LastMusicMult;
            AoBool = PauseManager.AoBool;
            DofBool = PauseManager.DofBool;
            CurQualityLevel = QualitySettings.GetQualityLevel();
            LastShadowCascade = PauseManager.LastShadowCascade;
             //Temp solution for recent resoultion problems 
            //resHeight = Screen.height;
            //resWidth = Screen.width;
            //fullscreenBool = Screen.fullScreen;
            if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable)
            {
                AnisoLevel = 0;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable)
            {
                AnisoLevel = 1;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable)
            {
                AnisoLevel = 2;
            }
            try
            {
                if (PauseManager.ReadUseSimpleTerrain)
                {
                    TerrainHeightMapLod = PauseManager.ReadTerrain.heightmapMaximumLOD;
                }
                else
                {
                    TerrainHeightMapLod = PauseManager.ReadSimpleTerrain.heightmapMaximumLOD;
                }
            }
            catch
            {
                Debug.Log("Cannot save terain heightmap LOD because the terrain was not assigned.");
            }
            UseSimpleTerrain = PauseManager.ReadUseSimpleTerrain;
            jsonString = JsonUtility.ToJson(this);
            Debug.Log(jsonString);
            File.WriteAllText(Application.persistentDataPath + "/" + FileName, jsonString);
        }


    }
}