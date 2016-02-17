using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using UnityStandardAssets.ImageEffects;
/// <summary>
///  Copyright (c) 2016 Eric Zhu 
/// </summary>
namespace GreatArcStudios
{
    /// <summary>
    /// The pause menu manager. You can extend this to make your own. Everything is pretty modular, so creating you own based off of this should be easy. Thanks for downloading and good luck! 
    /// </summary>
    public class PauseManager : MonoBehaviour
    {
        /// <summary>
        /// This is the main panel holder, which holds the main panel and should be called "main panel"
        /// </summary> 
        public GameObject mainPanel;
        /// <summary>
        /// This is the video panel holder, which holds all of the controls for the video panel and should be called "vid panel"
        /// </summary>
        public GameObject vidPanel;
        /// <summary>
        /// This is the audio panel holder, which holds all of the silders for the audio panel and should be called "audio panel"
        /// </summary>
        public GameObject audioPanel;
        /// <summary>
        /// These are the game objects with the title texts like "Pause menu" and "Game Title" 
        /// </summary>
        public GameObject TitleTexts;
        /// <summary>
        /// The mask that makes the scene darker  
        /// </summary>
        public GameObject mask;
        /// <summary>
        /// Audio Panel animator
        /// </summary>
        public Animator audioPanelAnimator;
        /// <summary>
        /// Video Panel animator  
        /// </summary>
        public Animator vidPanelAnimator;
        /// <summary>
        /// Quit Panel animator  
        /// </summary>
        public Animator quitPanelAnimator;
        /// <summary>
        /// Pause menu text 
        /// </summary>
        public Text pauseMenu;

        /// <summary>
        /// Main menu level string used for loading the main menu. This means you'll need to type in the editor text box, the name of the main menu level, ie: "mainmenu";
        /// </summary>
        public String mainMenu;
        //DOF script name
        /// <summary>
        /// The Depth of Field script name, ie: "DepthOfField". You can leave this blank in the editor, but will throw a null refrence exception, which is harmless.
        /// </summary>
        public String DOFScriptName;

        /// <summary>
        /// The Ambient Occlusion script name, ie: "AmbientOcclusion". You can leave this blank in the editor, but will throw a null refrence exception, which is harmless.
        /// </summary>
        public String AOScriptName;
        /// <summary>
        /// The main camera, assign this through the editor. 
        /// </summary>        
        public Camera mainCam;

        /// <summary>
        /// The main camera game object, assign this through the editor. 
        /// </summary> 
        public GameObject mainCamObj;

        /// <summary>
        /// The terrain detail density float. It's only public because you may want to adjust it in editor
        /// </summary> 
        public float detailDensity;

        /// <summary>
        /// Timescale value. The defualt is 1 for most games. You may want to change it if you are pausing the game in a slow motion situation 
        /// </summary> 
        public float timeScale = 1f;
        /// <summary>
        /// One terrain variable used if you have a terrain plugin like rtp. 
        /// </summary>
        public Terrain terrain;
        /// <summary>
        /// Other terrain variable used if you want to have an option to target low end harware.
        /// </summary>
        public Terrain simpleTerrain;
        /// <summary>
        /// Inital shadow distance 
        /// </summary>
        protected float shadowDistINI;
        /// <summary>
        /// Inital render distance 
        /// </summary>
        protected float renderDistINI;
        /// <summary>
        /// Inital AA quality 2, 4, or 8
        /// </summary>
        protected float aaQualINI;
        /// <summary>
        /// Inital terrain detail density
        /// </summary>
        protected float densityINI;
        /// <summary>
        /// Amount of trees that are acutal meshes
        /// </summary>
        protected float treeMeshAmtINI;
        /// <summary>
        /// Inital fov 
        /// </summary>
        protected float fovINI;
        /// <summary>
        /// Inital msaa amount 
        /// </summary>
        protected int msaaINI;
        /// <summary>
        /// Inital vsync count, the Unity docs say,
        /// <code> 
        /// //This will set the game to have one VSync per frame
        /// QualitySettings.vSyncCount = 1;
        /// </code>
        /// <code>
        /// //This will set the game to have two VSync per frame
        /// QualitySettings.vSyncCount = 2;
        /// </code>
        /// <code>
        /// //This will disable vsync
        /// QualitySettings.vSyncCount = 0;
        /// </code>
        /// </summary>
        protected int vsyncINI;
        /// <summary>
        /// AA drop down menu.
        /// </summary>
        public UnityEngine.UI.Dropdown aaCombo;
        /// <summary>
        /// Aniso drop down menu.
        /// </summary>
        public UnityEngine.UI.Dropdown afCombo;
        /// <summary>
        /// Lod bias float array. You should manually assign these based on the quality level.
        /// </summary>
        public float[] LODBias;
        /// <summary>
        /// Shadow distance array. You should manually assign these based on the quality level.
        /// </summary>
        public float[] shadowDist;
        /// <summary>
        /// An array of music audio sources
        /// </summary>
        public AudioSource[] music;
        /// <summary>
        /// An array of sound effect audio sources
        /// </summary>
        public AudioSource[] effects;
        /// <summary>
        /// An array of the other UI elements, which is used for disabling the other elements when the game is paused.
        /// </summary>
        public GameObject[] otherUIElements;
        /// <summary>
        /// The preset text label.
        /// </summary>
        public Text presetLabel;
        /// <summary>
        /// Resolution text label.
        /// </summary>
        public Text resolutionLabel;
        //int for amount of effects
        private int _audioEffectAmt = 0;
        //Inital audio effect volumes
        private float[] _beforeEffectVol;
        //Initial master volume
        private float _beforeMaster;
        //Initial music volume
        private float _beforeMusic;
        //Preset level
        private int _currentLevel;
        //Resoutions
        private Resolution[] allRes;
        //Camera dof script
        private MonoBehaviour tempScript;
        //Presets 
        private String[] presets;
        //Fullscreen Boolean
        private Boolean isFullscreen;
        //current resoultion
        private Resolution currentRes;


        /*
        //Color fade duration value
        //public float crossFadeDuration;
        //custom color
        //public Color _customColor;
        
         //Animation clips
         private AnimationClip audioIn;
         private AnimationClip audioOut;
         public AnimationClip vidIn;
         public AnimationClip vidOut;
         public AnimationClip mainIn;
         public AnimationClip mainOut; 
          */
        //Blur Variables
        //Blur Effect Script (using the standard image effects package) 
        //public Blur blurEffect;
        //Blur Effect Shader (should be the one that came with the package)
        //public Shader blurEffectShader;
        //Boolean for if the blur effect was originally enabled
        //public Boolean blurBool;



        /// <summary>
        /// The start method; you will need to place all of your inital value getting/setting here. 
        /// </summary>
        public void Start()
        {
            //Get the presets from the quality settings 
            presets = QualitySettings.names;
            presetLabel.text = presets[QualitySettings.GetQualityLevel()].ToString();
            _currentLevel = QualitySettings.GetQualityLevel();
            //Get the current resoultion, if the game is in fullscreen, and set the label to the original resolution
            allRes = Screen.resolutions;
            currentRes = Screen.currentResolution;
            Debug.Log("ini res" + currentRes);
            resolutionLabel.text = Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString();
            isFullscreen = Screen.fullScreen;
            //get all ini values
            _beforeEffectVol = new float[_audioEffectAmt];
            aaQualINI = QualitySettings.antiAliasing;
            renderDistINI = mainCam.farClipPlane;
            shadowDistINI = QualitySettings.shadowDistance;
            fovINI = mainCam.fieldOfView;
            msaaINI = QualitySettings.antiAliasing;
            vsyncINI = QualitySettings.vSyncCount;
            //enable titles
            TitleTexts.SetActive(true);
            //Find terrain
            terrain = Terrain.activeTerrain;
            //Disable other panels
            mainPanel.SetActive(false);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            //Enable mask
            mask.SetActive(false);
            //set the blur boolean to false;
            //blurBool = false;
            try
            {
                densityINI = Terrain.activeTerrain.detailObjectDensity;
            }
            catch (Exception e)
            {
                if (terrain = null)
                {
                    Debug.Log("Terrain Not Assigned");
                }
                else
                {
                    Debug.Log(e);
                }
            }
            //Add the blur effect
            /*mainCamObj.AddComponent(typeof(Blur));
            blurEffect = (Blur)mainCamObj.GetComponent(typeof(Blur));
            blurEffect.blurShader = blurEffectShader;
            blurEffect.enabled = false;  */

        }
        /// <summary>
        /// Restart the level by loading the loaded level.
        /// </summary>
        public void Restart()
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        /// <summary>
        /// Method to resume the game, so disable the pause menu and re-enable all other ui elements
        /// </summary>
        public void Resume()
        {
            Time.timeScale = timeScale;

            mainPanel.SetActive(false);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            TitleTexts.SetActive(false);
            mask.SetActive(false);
            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(true);
            }
            /* if (blurBool == false)
             {
                 blurEffect.enabled = false;
             }
             else
             {
                 //if you want to add in your own stuff do so here
                 return;
             } */
        }
        /// <summary>
        /// All the methods relating to qutting should be called here.
        /// </summary>
        public void quitOptions()
        {
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            quitPanelAnimator.enabled = true;
            quitPanelAnimator.Play("QuitPanelIn");

        }
        /// <summary>
        /// Method to quit the game. Call methods such as auto saving before qutting here.
        /// </summary>
        public void quitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        /// <summary>
        /// Cancels quittting by playing an animation.
        /// </summary>
        public void quitCancel()
        {
            quitPanelAnimator.Play("QuitPanelOut");
        }
        /// <summary>
        ///Loads the main menu scene.
        /// </summary>
        public void returnToMenu()
        {
            Application.LoadLevel(mainMenu);
        }

        // Update is called once per frame
        /// <summary>
        /// The update method. This mainly searches for the user pressing the escape key.
        /// </summary>
        public void Update()
        {

            //colorCrossfade();
            if (vidPanel.active == true)
            {
                pauseMenu.text = "Video Menu";
            }
            else if (audioPanel.active == true)
            {
                pauseMenu.text = "Audio Menu";
            }
            else if (mainPanel.active == true)
            {
                pauseMenu.text = "Pause Menu";
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainPanel.SetActive(true);
                vidPanel.SetActive(false);
                audioPanel.SetActive(false);
                TitleTexts.SetActive(true);
                mask.SetActive(true);
                Time.timeScale = 0;
                for (int i = 0; i < otherUIElements.Length; i++)
                {
                    otherUIElements[i].gameObject.SetActive(false);
                }
                /* if (blurBool == false)
                  {
                     blurEffect.enabled = true;
                 }  */
            }


        }
        /*
        void colorCrossfade()
        {
            Debug.Log(pauseMenu.color);

            if (pauseMenu.color == Color.white)
            {
                pauseMenu.CrossFadeColor(_customColor, crossFadeDuration, true, false);
            }
            else { 
                pauseMenu.CrossFadeColor(Color.white, crossFadeDuration, true, false);
            }
        }  */
        /////Audio Options

        /// <summary>
        /// Show the audio panel 
        /// </summary>
        public void Audio()
        {
            mainPanel.SetActive(false);
            vidPanel.SetActive(false);
            audioPanel.SetActive(true);
            audioPanelAnimator.enabled = true;
            audioIn();
            pauseMenu.text = "Audio Menu";
        }
        /// <summary>
        /// Play the "audio panel in" animation.
        /// </summary>
        public void audioIn()
        {
            audioPanelAnimator.Play("Audio Panel In");
        }
        /// <summary>
        /// Audio Option Methods
        /// </summary>
        /// <param name="f"></param>
        public void updateMasterVol(float f)
        {
            _beforeMaster = AudioListener.volume;
            //Controls volume of all audio listeners 
            AudioListener.volume = f;
        }
        /// <summary>
        /// Update music effects volume
        /// </summary>
        /// <param name="f"></param>
        public void updateMusicVol(float f)
        {
            for (int _musicAmt = 0; _musicAmt < music.Length; _musicAmt++)
            {
                music[_musicAmt].volume = f;
            }
            //_beforeMusic = music.volume;
        }
        /// <summary>
        /// Update the audio effects volume
        /// </summary>
        /// <param name="f"></param>
        public void updateEffectsVol(float f)
        {
            for (_audioEffectAmt = 0; _audioEffectAmt < effects.Length; _audioEffectAmt++)
            {
                //get the values for all effects before the change
                _beforeEffectVol[_audioEffectAmt] = effects[_audioEffectAmt].volume;

                //lower it by a factor of f because we don't want every effect to be set to a uniform volume
                effects[_audioEffectAmt].volume *= f;
            }

        }
        /// <summary>
        /// The method for changing the applying new audio settings
        /// </summary>
        public void applyAudio()
        {
            StartCoroutine(applyAudioMain());

        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the audio settings
        /// </summary>
        /// <returns></returns>
        protected IEnumerator applyAudioMain()
        {
            audioPanelAnimator.Play("Audio Panel Out");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            mainPanel.SetActive(true);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
        }
        /// <summary>
        /// Cancel the audio setting changes
        /// </summary>
        public void cancelAudio()
        {

            StartCoroutine(cancelAudioMain());
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the audio settings
        /// </summary>
        /// <returns></returns>
        protected IEnumerator cancelAudioMain()
        {
            audioPanelAnimator.Play("Audio Panel Out");
            // Debug.Log(audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length);
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            Debug.Log("Passed");
            mainPanel.SetActive(true);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            for (_audioEffectAmt = 0; _audioEffectAmt < effects.Length; _audioEffectAmt++)
            {
                //get the values for all effects before the change
                effects[_audioEffectAmt].volume = _beforeEffectVol[_audioEffectAmt];

            }
            AudioListener.volume = _beforeMaster;
            for (int _musicAmt = 0; _musicAmt < music.Length; _musicAmt++)
            {
                music[_musicAmt].volume = _beforeMusic;
            }

        }
        /////Video Options
        /// <summary>
        /// Show video
        /// </summary>
        public void Video()
        {

            mainPanel.SetActive(false);
            vidPanel.SetActive(true);
            audioPanel.SetActive(false);
            vidPanelAnimator.enabled = true;
            videoIn();
            pauseMenu.text = "Video Menu";

        }
        /// <summary>
        /// Play the "video panel in" animation
        /// </summary>
        public void videoIn()
        {
            vidPanelAnimator.Play("Video Panel In");
        }

        /// <summary>
        /// Cancel the video setting changes 
        /// </summary>
        public void cancelVideo()
        {
            StartCoroutine(cancelVideoMain());
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then changethe video settings
        /// </summary>
        /// <returns></returns>
        protected IEnumerator cancelVideoMain()
        {
            vidPanelAnimator.Play("Video Panel Out");
            // Debug.Log(audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length);
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)vidPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            try
            {
                QualitySettings.shadowDistance = shadowDistINI;
                mainCam.farClipPlane = renderDistINI;
                QualitySettings.antiAliasing = (int)aaQualINI;
                Terrain.activeTerrain.detailObjectDensity = densityINI;
                mainCam.fieldOfView = fovINI;
                QualitySettings.antiAliasing = msaaINI;
                QualitySettings.vSyncCount = vsyncINI;
                mainPanel.SetActive(true);
                vidPanel.SetActive(false);
                audioPanel.SetActive(false);
            }
            catch (Exception e)
            {

                Debug.Log("A problem occured (chances are the terrain was not assigned ): " + e);
                QualitySettings.shadowDistance = shadowDistINI;
                mainCam.farClipPlane = renderDistINI;
                QualitySettings.antiAliasing = (int)aaQualINI;
                mainCam.fieldOfView = fovINI;
                QualitySettings.antiAliasing = msaaINI;
                QualitySettings.vSyncCount = vsyncINI;
                mainPanel.SetActive(true);
                vidPanel.SetActive(false);
                audioPanel.SetActive(false);

            }

        }
        //Apply the video prefs
        /// <summary>
        /// Apply the video settings
        /// </summary>
        public void apply()
        {
            StartCoroutine(applyVideo());

        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the video settings
        /// </summary>
        /// <returns></returns>
        protected IEnumerator applyVideo()
        {
            vidPanelAnimator.Play("Video Panel Out");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)vidPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            mainPanel.SetActive(true);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
        }
        /// <summary>
        /// Video Options
        /// </summary>
        /// <param name="B"></param>
        public void toggleVSync(Boolean B)
        {
            if (B == true)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }

        }
        /// <summary>
        /// Update full high quality tree mesh amount.
        /// </summary>
        /// <param name="f"></param>
        public void updateTreeMeshAmt(int f)
        {
            treeMeshAmtINI = terrain.treeMaximumFullLODCount;
            terrain.treeMaximumFullLODCount = (int)f;
        }
        /// <summary>
        /// Change the lod bias using
        /// <c>
        /// QualitySettings.lodBias = LoDBias / 2.15f;
        /// </c> 
        /// LoDBias is only divided by 2.15 because the max is set to 10 on the slider, and dividing by 2.15 results in 4.65, our desired max. However, deleting or changing 2.15 is compeletly fine.
        /// </summary>
        /// <param name="LoDBias"></param>
        public void lodBias(float LoDBias)
        {
            QualitySettings.lodBias = LoDBias / 2.15f;
        }
        /// <summary>
        /// Update the render distance using 
        /// <c>
        /// mainCam.farClipPlane = f;
        /// </c>
        /// </summary>
        /// <param name="f"></param>
        public void updateRenderDist(float f)
        {
            try
            {
                mainCam.farClipPlane = f;
                renderDistINI = f;
            }
            catch
            {
                Debug.Log(" Finding main camera now...it is still suggested that you manually assign this");
                Camera.main.farClipPlane = f;
                renderDistINI = f;
            }

        }
        /// <summary>
        /// Update the texture quality using  
        /// <c>QualitySettings.masterTextureLimit </c>
        /// </summary>
        /// <param name="qual"></param>
        public void updateTex(float qual)
        {
            int f = Mathf.RoundToInt(qual);
            QualitySettings.masterTextureLimit = f;
        }
        /// <summary>
        /// Update the shadow distance using 
        /// <c>
        /// QualitySettings.shadowDistance = dist;
        /// </c>
        /// </summary>
        /// <param name="dist"></param>
        public void updateShadowDistance(float dist)
        {
            QualitySettings.shadowDistance = dist;
            shadowDistINI = dist;
        }
        /// <summary>
        /// Change the max amount of high quality trees using 
        /// <c>
        /// terrain.treeMaximumFullLODCount = (int)qual;
        /// </c>
        /// </summary>
        /// <param name="qual"></param>
        public void treeMaxLod(float qual)
        {
            terrain.treeMaximumFullLODCount = (int)qual;
        }
        /// <summary>
        /// Change the height map max LOD using 
        /// <c>
        /// terrain.heightmapMaximumLOD = (int)qual;
        /// </c>
        /// </summary>
        /// <param name="qual"></param>
        public void updateTerrainLod(float qual)
        {
            try { terrain.heightmapMaximumLOD = (int)qual; }
            catch { Debug.Log("Terrain not assigned"); return; }

        }
        /// <summary>
        /// Change the fov using a float. The defualt should be 60.
        /// </summary>
        /// <param name="fov"></param>
        public void updateFOV(float fov)
        {
            mainCam.fieldOfView = fov;

        }
        /// <summary>
        /// Toggle on or off Depth of Field. This is meant to be used with a checkbox.
        /// </summary>
        /// <param name="b"></param>
        public void toggleDOF(Boolean b)
        {
            tempScript = (MonoBehaviour)mainCamObj.GetComponent(DOFScriptName);
            if (b == true)
            {
                tempScript.enabled = true;
            }
            else
            {
                tempScript.enabled = false;
            }

        }
        /// <summary>
        /// Toggle on or off Ambient Occulusion. This is meant to be used with a checkbox.
        /// </summary>
        /// <param name="b"></param>
        public void toggleAO(Boolean b)
        {
            tempScript = (MonoBehaviour)mainCamObj.GetComponent(AOScriptName);
            if (b == true)
            {
                tempScript.enabled = true;
            }
            else
            {
                tempScript.enabled = false;
            }

        }
        /// <summary>
        /// Set the game to windowed or full screen. This is meant to be used with a checkbox
        /// </summary>
        /// <param name="b"></param>
        public void setFullScreen(Boolean b)
        {
            if (b == true)
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                isFullscreen = true;
            }
            else
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
                isFullscreen = false;
            }
        }
        /// <summary>
        /// Method for moving to the next resoution in the allRes array. WARNING: This is not finished/buggy.  
        /// </summary>
        //Method for moving to the next resoution in the allRes array. WARNING: This is not finished/buggy.  
        public void nextRes()
        {
            //Iterate through all of the resoultions. 
            for (int i = 0; i < allRes.Length; i++)
            {
                //If the resoultion matches the current resoution height and width then go through the statement.
                if (allRes[i].height == currentRes.height && allRes[i].width == currentRes.width)
                {
                    Debug.Log("found " + i);
                    //If the user is playing fullscreen. Then set the resoution to one element higher in the array, set the full screen boolean to true, reset the current resolution, and then update the resolution label.
                    if (isFullscreen == true) { Screen.SetResolution(allRes[i + 1].width, allRes[i + 1].height, true); isFullscreen = true; currentRes = Screen.currentResolution; resolutionLabel.text = Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString(); }
                    //If the user is playing in a window. Then set the resoution to one element higher in the array, set the full screen boolean to false, reset the current resolution, and then update the resolution label.
                    if (isFullscreen == false) { Screen.SetResolution(allRes[i + 1].width, allRes[i + 1].height, false); isFullscreen = false; currentRes = Screen.currentResolution; resolutionLabel.text = Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString(); }

                    Debug.Log("Res after: " + currentRes);
                }
            }

        }
        /// <summary>
        /// Method for moving to the last resoution in the allRes array. WARNING: This is not finished/buggy.  
        /// </summary>
        //Method for moving to the last resoution in the allRes array. WARNING: This is not finished/buggy.  
        public void lastRes()
        {
            //Iterate through all of the resoultions. 
            for (int i = 0; i < allRes.Length; i++)
            {
                if (allRes[i].height == currentRes.height && allRes[i].width == currentRes.width)
                {

                    Debug.Log("found " + i);
                    //If the user is playing fullscreen. Then set the resoution to one element lower in the array, set the full screen boolean to true, reset the current resolution, and then update the resolution label.
                    if (isFullscreen == true) { Screen.SetResolution(allRes[i - 1].width, allRes[i - 1].height, true); isFullscreen = true; currentRes = Screen.currentResolution; resolutionLabel.text = Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString(); }
                    //If the user is playing in a window. Then set the resoution to one element lower in the array, set the full screen boolean to false, reset the current resolution, and then update the resolution label.
                    if (isFullscreen == false) { Screen.SetResolution(allRes[i - 1].width, allRes[i - 1].height, false); isFullscreen = false; currentRes = Screen.currentResolution; resolutionLabel.text = Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString(); }

                    Debug.Log("Res after: " + currentRes);
                }
            }

        }

        /// <summary>
        /// Force aniso on using quality settings
        /// </summary>
        //Force the aniso on.
        public void forceOnANISO()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        }
        /// <summary>
        /// Per texture aniso using quality settings
        /// </summary>
        //Use per texture aniso settings.
        public void perTexANISO()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        }
        /// <summary>
        /// Disable aniso using quality setttings
        /// </summary>
        //Disable aniso all together.
        public void disableANISO()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        }
        /// <summary>
        /// The method for changing aniso settings
        /// </summary>
        /// <param name="anisoSetting"></param>
        public void updateANISO(int anisoSetting)
        {
            if (anisoSetting == 0)
            {
                disableANISO();
            }
            else if (anisoSetting == 1)
            {
                forceOnANISO();
            }
            else if (anisoSetting == 2)
            {
                perTexANISO();
            }
        }

        /// <summary>
        /// The method for setting the amount of shadow cascades
        /// </summary>
        /// <param name="cascades"></param>
        public void updateCascades(float cascades)
        {
            int c = Mathf.RoundToInt(cascades);
            if (c == 1)
            {
                c = 2;
            }
            else if (c == 3)
            {
                c = 2;
            }
            QualitySettings.shadowCascades = c;
        }
        /// <summary>
        /// Update terrain density
        /// </summary>
        /// <param name="density"></param>
        public void updateDensity(float density)
        {
            detailDensity = density;
            terrain.detailObjectDensity = detailDensity;
        }
        /// <summary>
        /// Update MSAA quality using quality settings
        /// </summary>
        /// <param name="msaaAmount"></param>
        public void updateMSAA(int msaaAmount)
        {
            if (msaaAmount == 0)
            {
                disMSAA();
            }
            else if (msaaAmount == 1)
            {
                twoMSAA();
            }
            else if (msaaAmount == 2)
            {
                fourMSAA();
            }
            else if (msaaAmount == 3)
            {
                eightMSAA();
            }

        }
        /// <summary>
        /// Set MSAA to 0x (disabling it) using quality settings
        /// </summary>
        public void disMSAA()
        {

            QualitySettings.antiAliasing = 0;


            Debug.Log(QualitySettings.antiAliasing);
            // aaOption.text = "MSAA: " + QualitySettings.antiAliasing.ToString();
        }
        /// <summary>
        /// Set MSAA to 2x using quality settings
        /// </summary>
        public void twoMSAA()
        {

            QualitySettings.antiAliasing = 2;

            Debug.Log(QualitySettings.antiAliasing);
            // aaOption.text = "MSAA: " + QualitySettings.antiAliasing.ToString();
        }
        /// <summary>
        /// Set MSAA to 4x using quality settings
        /// </summary>
        public void fourMSAA()
        {

            QualitySettings.antiAliasing = 4;


            Debug.Log(QualitySettings.antiAliasing);
            // aaOption.text = "MSAA: " + QualitySettings.antiAliasing.ToString();
        }
        /// <summary>
        /// Set MSAA to 8x using quality settings
        /// </summary>
        public void eightMSAA()
        {

            QualitySettings.antiAliasing = 8;


            Debug.Log(QualitySettings.antiAliasing);
            // aaOption.text = "MSAA: " + QualitySettings.antiAliasing.ToString();
        }
        /// <summary>
        /// Set the quality level one level higher. This is done by getting the current quality level, then using 
        /// <c> 
        /// QualitySettings.IncreaseLevel();
        /// </c>
        /// to increase the level. The current level variable is set to the new quality setting, and the label is updated.
        /// </summary>
        public void nextPreset()
        {
            _currentLevel = QualitySettings.GetQualityLevel();
            QualitySettings.IncreaseLevel();
            _currentLevel = QualitySettings.GetQualityLevel();
            presetLabel.text = presets[_currentLevel].ToString();
        }
        /// <summary>
        /// Set the quality level one level lower. This is done by getting the current quality level, then using 
        /// <c> 
        /// QualitySettings.DecreaseLevel();
        /// </c>
        /// to decrease the level. The current level variable is set to the new quality setting, and the label is updated.
        /// </summary>
        public void lastPreset()
        {
            _currentLevel = QualitySettings.GetQualityLevel();
            QualitySettings.DecreaseLevel();
            _currentLevel = QualitySettings.GetQualityLevel();
            presetLabel.text = presets[_currentLevel].ToString();
        }
        /// <summary>
        /// Hard code the minimal settings
        /// </summary>
        public void setMinimal()
        {
            QualitySettings.SetQualityLevel(0);
            //QualitySettings.shadowDistance = 12.6f;
            QualitySettings.shadowDistance = shadowDist[0];
            //QualitySettings.lodBias = 0.3f;
            QualitySettings.lodBias = LODBias[0];
        }
        /// <summary>
        /// Hard code the very low settings
        /// </summary>
        public void setVeryLow()
        {
            QualitySettings.SetQualityLevel(1);
            //QualitySettings.shadowDistance = 17.4f;
            QualitySettings.shadowDistance = shadowDist[1];
            //QualitySettings.lodBias = 0.55f;
            QualitySettings.lodBias = LODBias[1];
        }
        /// <summary>
        /// Hard code the low settings
        /// </summary>
        public void setLow()
        {
            QualitySettings.SetQualityLevel(2);
            //QualitySettings.shadowDistance = 29.7f;
            //QualitySettings.lodBias = 0.68f;
            QualitySettings.lodBias = LODBias[2];
            QualitySettings.shadowDistance = shadowDist[2];
        }
        /// <summary>
        /// Hard code the normal settings
        /// </summary>
        public void setNormal()
        {
            QualitySettings.SetQualityLevel(3);
            //QualitySettings.shadowDistance = 82f;
            //QualitySettings.lodBias = 1.09f;
            QualitySettings.shadowDistance = shadowDist[3];
            QualitySettings.lodBias = LODBias[3];
        }
        /// <summary>
        /// Hard code the very high settings
        /// </summary>
        public void setVeryHigh()
        {
            QualitySettings.SetQualityLevel(4);
            //QualitySettings.shadowDistance = 110f;
            //QualitySettings.lodBias = 1.22f;
            QualitySettings.shadowDistance = shadowDist[4];
            QualitySettings.lodBias = LODBias[4];
        }
        /// <summary>
        /// Hard code the ultra settings
        /// </summary>
        public void setUltra()
        {
            QualitySettings.SetQualityLevel(5);
            //QualitySettings.shadowDistance = 338f;
            //QualitySettings.lodBias = 1.59f;
            QualitySettings.shadowDistance = shadowDist[5];
            QualitySettings.lodBias = LODBias[5];
        }
        /// <summary>
        /// Hard code the extreme settings
        /// </summary>
        public void setExtreme()
        {
            QualitySettings.SetQualityLevel(6);
            //QualitySettings.shadowDistance = 800f;
            //QualitySettings.lodBias = 4.37f;
            QualitySettings.shadowDistance = shadowDist[6];
            QualitySettings.lodBias = LODBias[6];
        }

    }
}
