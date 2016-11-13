using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
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
        /// This is the credits panel holder, which holds all of the silders for the audio panel and should be called "credits panel"
        /// </summary>
        public GameObject creditsPanel;
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
        /// Credits Panel animator  
        /// </summary>
        public Animator creditsPanelAnimator;
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
        internal static Camera mainCamShared;
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
        internal static float shadowDistINI;
        /// <summary>
        /// Inital render distance 
        /// </summary>
        internal static float renderDistINI;
        /// <summary>
        /// Inital AA quality 2, 4, or 8
        /// </summary>
        internal static float aaQualINI;
        /// <summary>
        /// Inital terrain detail density
        /// </summary>
        internal static float densityINI;
        /// <summary>
        /// Amount of trees that are acutal meshes
        /// </summary>
        internal static float treeMeshAmtINI;
        /// <summary>
        /// Inital fov 
        /// </summary>
        internal static float fovINI;
        /// <summary>
        /// Inital msaa amount 
        /// </summary>
        internal static int msaaINI;
        /// <summary>
        /// Inital vsync count, the Unity docs say,
        /// <code> 
        /// //This will set the game to have one VSync per frame
        /// QualitySettings.vSyncCount = 1;
        /// </code>
        /// <code>
        /// //This will disable vsync
        /// QualitySettings.vSyncCount = 0;
        /// </code>
        /// </summary>
        internal static int vsyncINI;
        /// <summary>
        /// AA drop down menu.
        /// </summary>
        public Dropdown aaCombo;
        /// <summary>
        /// Aniso drop down menu.
        /// </summary>
        public Dropdown afCombo;

        public Slider fovSlider;
        public Slider modelQualSlider;
        public Slider terrainQualSlider;
        public Slider highQualTreeSlider;
        public Slider renderDistSlider;
        public Slider terrainDensitySlider;
        public Slider shadowDistSlider;
        public Slider audioMasterSlider;
        public Slider audioMusicSlider;
        public Slider audioEffectsSlider;
        public Slider masterTexSlider;
        public Slider shadowCascadesSlider;
        public Toggle vSyncToggle;
        public Toggle aoToggle;
        public Toggle dofToggle;
        public Toggle fullscreenToggle;
        /// <summary>
        /// The preset text label.
        /// </summary>
        public Text presetLabel;
        /// <summary>
        /// Resolution text label.
        /// </summary>
        public Text resolutionLabel;
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
        /// Editor boolean for hardcoding certain video settings. It will allow you to use the values defined in LOD Bias and Shadow Distance
        /// </summary>
        public Boolean hardCodeSomeVideoSettings;
        /// <summary>
        /// Boolean for turning on simple terrain
        /// </summary>
        public Boolean useSimpleTerrain;
        public static Boolean readUseSimpleTerrain;
        /// <summary>
        /// Event system
        /// </summary>
        public EventSystem uiEventSystem;
        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject defualtSelectedVideo;
        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject defualtSelectedAudio;
        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject defualtSelectedMain;
        public GameObject defualtSelectedCredits;
        //last music multiplier; this should be a value between 0-1
        internal static float lastMusicMult;
        //last audio multiplier; this should be a value between 0-1
        internal static float lastAudioMult;
        //Initial master volume
        internal static float beforeMaster;
        //last texture limit 
        internal static int lastTexLimit;
        //int for amount of effects
        private int _audioEffectAmt = 0;
        //Inital audio effect volumes
        private float[] _beforeEffectVol;

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
        internal static Resolution currentRes;
        //Last resoultion 
        private Resolution beforeRes;

        //last shadow cascade value
        internal static int lastShadowCascade;

        public static Boolean aoBool;
        public static Boolean dofBool;
        private Boolean lastAOBool;
        private Boolean lastDOFBool;
        public static Terrain readTerrain;
        public static Terrain readSimpleTerrain;

        private SaveSettings saveSettings = new SaveSettings();


        private Terrain currentTerrain {
            get {
                return useSimpleTerrain ? simpleTerrain : terrain;
            }
        }


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

            readUseSimpleTerrain = useSimpleTerrain;
            if (useSimpleTerrain)
            {
                readSimpleTerrain = simpleTerrain;
            }
            else
            {
                readTerrain = terrain;
            }

            mainCamShared = mainCam;
            //Set the lastmusicmult and last audiomult
            lastMusicMult = audioMusicSlider.value;
            lastAudioMult = audioEffectsSlider.value;
            //Set the first selected item
            uiEventSystem.firstSelectedGameObject = defualtSelectedMain;
            //Get the presets from the quality settings 
            presets = QualitySettings.names;
            presetLabel.text = presets[QualitySettings.GetQualityLevel()].ToString();
            _currentLevel = QualitySettings.GetQualityLevel();
            //Get the current resoultion, if the game is in fullscreen, and set the label to the original resolution
            allRes = Screen.resolutions;
            currentRes.width = Screen.width;
            currentRes.height = Screen.height;
            //Debug.Log("ini res" + currentRes);
            resolutionLabel.text = Screen.width.ToString() + " x " + Screen.height.ToString();
            isFullscreen = Screen.fullScreen;
            //get initial screen effect bools
            lastAOBool = aoToggle.isOn;
            lastDOFBool = dofToggle.isOn;
            //get all specified audio source volumes
            _beforeEffectVol = new float[_audioEffectAmt];
            beforeMaster = AudioListener.volume;
            //get all ini values
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
            creditsPanel.SetActive(false);
            //Enable mask
            mask.SetActive(false);
            //set last texture limit
            lastTexLimit = QualitySettings.masterTextureLimit;
            //set last shadow cascade 
            lastShadowCascade = QualitySettings.shadowCascades;
            try
            {
                saveSettings.LoadGameSettings();
            }
            catch
            {
                Debug.Log("Game settings not found in: " + Application.persistentDataPath + "/" + saveSettings.fileName);
                saveSettings.SaveGameSettings();
            }

            try
            {
                densityINI = Terrain.activeTerrain.detailObjectDensity;
            }
            catch
            {
                if (terrain = null)
                {
                    Debug.Log("Terrain Not Assigned");
                }
            }

            //set the blur boolean to false;
            //blurBool = false;
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
            uiEventSystem.firstSelectedGameObject = defualtSelectedMain;

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
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
        }

        // Update is called once per frame
        /// <summary>
        /// The update method. This mainly searches for the user pressing the escape key.
        /// </summary>
        public void Update()
        {
            readUseSimpleTerrain = useSimpleTerrain;
            useSimpleTerrain = readUseSimpleTerrain;
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

            if (Input.GetKeyDown(KeyCode.Escape) && mainPanel.active == false)
            {

                uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
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
            else if (Input.GetKeyDown(KeyCode.Escape) && mainPanel.active == true)
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
            uiEventSystem.SetSelectedGameObject(defualtSelectedAudio);
            audioPanelAnimator.Play("Audio Panel In");
            audioMasterSlider.value = AudioListener.volume;
            //Perform modulo to find factor f to allow for non uniform music volumes
            float a; float b; float f;
            try
            {
                a = music[0].volume;
                b = music[1].volume;
                f = a % b;
                audioMusicSlider.value = f;
            }
            catch
            {
                Debug.Log("You do not have multiple audio sources");
                audioMusicSlider.value = lastMusicMult;
            }
            //Do this with the effects
            try
            {
                a = effects[0].volume;
                b = effects[1].volume;
                f = a % b;
                audioEffectsSlider.value = f;
            }
            catch
            {
                Debug.Log("You do not have multiple audio sources");
                audioEffectsSlider.value = lastAudioMult;
            }

        }
        /// <summary>
        /// Audio Option Methods
        /// </summary>
        /// <param name="f"></param>
        public void updateMasterVol(float f)
        {

            //Controls volume of all audio listeners 
            AudioListener.volume = f;
        }
        /// <summary>
        /// Update music effects volume
        /// </summary>
        /// <param name="f"></param>
        public void updateMusicVol(float f)
        {
            try
            {
                for (int _musicAmt = 0; _musicAmt < music.Length; _musicAmt++)
                {
                    music[_musicAmt].volume *= f;
                }
            }
            catch
            {
                Debug.Log("Please assign music sources in the manager");
            }
            //_beforeMusic = music.volume;
        }
        /// <summary>
        /// Update the audio effects volume
        /// </summary>
        /// <param name="f"></param>
        public void updateEffectsVol(float f)
        {
            try
            {
                for (_audioEffectAmt = 0; _audioEffectAmt < effects.Length; _audioEffectAmt++)
                {
                    //get the values for all effects before the change
                    _beforeEffectVol[_audioEffectAmt] = effects[_audioEffectAmt].volume;

                    //lower it by a factor of f because we don't want every effect to be set to a uniform volume
                    effects[_audioEffectAmt].volume *= f;
                }
            }
            catch
            {
                Debug.Log("Please assign audio effects sources in the manager.");
            }

        }
        /// <summary> 
        /// The method for changing the applying new audio settings
        /// </summary>
        public void applyAudio()
        {
            StartCoroutine(applyAudioMain());
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);

        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the audio settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator applyAudioMain()
        {
            audioPanelAnimator.Play("Audio Panel Out");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            mainPanel.SetActive(true);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            beforeMaster = AudioListener.volume;
            lastMusicMult = audioMusicSlider.value;
            lastAudioMult = audioEffectsSlider.value;
            saveSettings.SaveGameSettings();
        }
        /// <summary>
        /// Cancel the audio setting changes
        /// </summary>
        public void cancelAudio()
        {
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
            StartCoroutine(cancelAudioMain());
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the audio settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator cancelAudioMain()
        {
            audioPanelAnimator.Play("Audio Panel Out");
            // Debug.Log(audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length);
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            mainPanel.SetActive(true);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            AudioListener.volume = beforeMaster;
            //Debug.Log(_beforeMaster + AudioListener.volume);
            try
            {
                for (_audioEffectAmt = 0; _audioEffectAmt < effects.Length; _audioEffectAmt++)
                {
                    //get the values for all effects before the change
                    effects[_audioEffectAmt].volume = _beforeEffectVol[_audioEffectAmt];
                }
                for (int _musicAmt = 0; _musicAmt < music.Length; _musicAmt++)
                {
                    music[_musicAmt].volume = _beforeMusic;
                }
            }
            catch
            {
                Debug.Log("please assign the audio sources in the manager");
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


        private static readonly Dictionary<int, int> aaDict = new Dictionary<int, int>() { {0, 0}, {2, 1}, {4, 2}, {8, 3} };

        /// <summary>
        /// Play the "video panel in" animation
        /// </summary>
        public void videoIn()
        {
            uiEventSystem.SetSelectedGameObject(defualtSelectedVideo);
            vidPanelAnimator.Play("Video Panel In");

            aaCombo.value = aaDict[QualitySettings.antiAliasing];
            
            // --------
            // todo: that stuff is stupid:
            if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable)
            {
                afCombo.value = 1;  
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable)
            {
                afCombo.value = 0;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable)
            {
                afCombo.value = 2;
            }
            /* 
             * the unity constants already have int values:
                 * AnisotropicFiltering.ForceEnable = 2
                 * AnisotropicFiltering.Enable = 1
             * would be smarter to use them, also this hard coded values just work with the "hardcoded" dropdownlist
             */
            // --------

            presetLabel.text = presets[QualitySettings.GetQualityLevel()].ToString();
            fovSlider.value = mainCam.fieldOfView;
            modelQualSlider.value = QualitySettings.lodBias;
            renderDistSlider.value = mainCam.farClipPlane;
            shadowDistSlider.value = QualitySettings.shadowDistance;
            masterTexSlider.value = QualitySettings.masterTextureLimit;
            shadowCascadesSlider.value = QualitySettings.shadowCascades;
            fullscreenToggle.isOn = Screen.fullScreen;
            aoToggle.isOn = aoBool;
            dofToggle.isOn = dofBool;
            if (QualitySettings.vSyncCount == 0)
            {
                vSyncToggle.isOn = false;
            }
            else if (QualitySettings.vSyncCount == 1)
            {
                vSyncToggle.isOn = true;
            }
            try
            {
                highQualTreeSlider.value = currentTerrain.treeMaximumFullLODCount;
                terrainDensitySlider.value = currentTerrain.detailObjectDensity;
                terrainQualSlider.value = currentTerrain.heightmapMaximumLOD;
            }
            catch
            {
                return;
            }

        }

        /// <summary>
        /// Cancel the video setting changes 
        /// </summary>
        public void cancelVideo()
        {
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
            StartCoroutine(cancelVideoMain());
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then changethe video settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator cancelVideoMain()
        {
            vidPanelAnimator.Play("Video Panel Out");

            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)vidPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            try
            {
                mainCam.farClipPlane = renderDistINI;
                Terrain.activeTerrain.detailObjectDensity = densityINI;
                mainCam.fieldOfView = fovINI;
                mainPanel.SetActive(true);
                vidPanel.SetActive(false);
                audioPanel.SetActive(false);
                aoBool = lastAOBool;
                dofBool = lastDOFBool;
                Screen.SetResolution(beforeRes.width, beforeRes.height, Screen.fullScreen);
                QualitySettings.shadowDistance = shadowDistINI;
                QualitySettings.antiAliasing = (int)aaQualINI;
                QualitySettings.antiAliasing = msaaINI;
                QualitySettings.vSyncCount = vsyncINI;
                QualitySettings.masterTextureLimit = lastTexLimit;
                QualitySettings.shadowCascades = lastShadowCascade;
                Screen.fullScreen = isFullscreen;
            }
            catch
            {

                Debug.Log("A problem occured (chances are the terrain was not assigned )");
                mainCam.farClipPlane = renderDistINI;
                mainCam.fieldOfView = fovINI;
                mainPanel.SetActive(true);
                vidPanel.SetActive(false);
                audioPanel.SetActive(false);
                aoBool = lastAOBool;
                dofBool = lastDOFBool;
                QualitySettings.shadowDistance = shadowDistINI;
                Screen.SetResolution(beforeRes.width, beforeRes.height, Screen.fullScreen);
                QualitySettings.antiAliasing = (int)aaQualINI;
                QualitySettings.antiAliasing = msaaINI;
                QualitySettings.vSyncCount = vsyncINI;
                QualitySettings.masterTextureLimit = lastTexLimit;
                QualitySettings.shadowCascades = lastShadowCascade;
                //Screen.fullScreen = isFullscreen;

            }

        }
        //Apply the video prefs
        /// <summary>
        /// Apply the video settings
        /// </summary>
        public void apply()
        {
            StartCoroutine(applyVideo());
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);

        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the video settings.
        /// </summary>
        /// <returns></returns>
        internal IEnumerator applyVideo()
        {
            vidPanelAnimator.Play("Video Panel Out");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)vidPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            mainPanel.SetActive(true);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            renderDistINI = mainCam.farClipPlane;
            shadowDistINI = QualitySettings.shadowDistance;
            Debug.Log("Shadow dist ini" + shadowDistINI);
            fovINI = mainCam.fieldOfView;
            aoBool = aoToggle.isOn;
            dofBool = dofToggle.isOn;
            lastAOBool = aoBool;
            lastDOFBool = dofBool;
            beforeRes = currentRes;
            lastTexLimit = QualitySettings.masterTextureLimit;
            lastShadowCascade = QualitySettings.shadowCascades;
            vsyncINI = QualitySettings.vSyncCount;
            isFullscreen = Screen.fullScreen;
            try
            {
                densityINI = currentTerrain.detailObjectDensity;
                treeMeshAmtINI = currentTerrain.treeMaximumFullLODCount;
            }
            catch { Debug.Log("Please assign a terrain"); }
            saveSettings.SaveGameSettings();

        }


        public void TurnOnVSync(bool b){
            vsyncINI = QualitySettings.vSyncCount;
            QualitySettings.vSyncCount = b ? 1 : 0;
        }

        /// <summary>
        /// Update full high quality tree mesh amount.
        /// </summary>
        /// <param name="f"></param>
        public void updateTreeMeshAmt(int f) {
            currentTerrain.treeMaximumFullLODCount = f;
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
            }
            catch
            {
                Debug.Log(" Finding main camera now...it is still suggested that you manually assign this");
                mainCam = Camera.main;
                mainCam.farClipPlane = f;
            }

        }
        /// <summary>
        /// Update the texture quality using  
        /// <c>QualitySettings.masterTextureLimit </c>
        /// </summary>
        /// <param name="qual"></param>
        public void updateTex(float qual)
        {
            QualitySettings.masterTextureLimit = (int)qual;
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
            currentTerrain.treeMaximumFullLODCount = (int)qual;
        }

        /// <summary>
        /// Change the height map max LOD using 
        /// <c>
        /// terrain.heightmapMaximumLOD = (int)qual;
        /// </c>
        /// </summary>
        /// <param name="qual"></param>
        public void updateTerrainLod(float qual) {
            if (currentTerrain == null) return; // fail silently
            currentTerrain.heightmapMaximumLOD = (int) qual;
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
        public void toggleDOF(bool b)
        {
            try
            {
                tempScript = (MonoBehaviour)mainCamObj.GetComponent(DOFScriptName);
                tempScript.enabled = b;
                dofBool = b;
            }
            catch
            {
                Debug.Log("No AO post processing found");
            }
        }
        /// <summary>
        /// Toggle on or off Ambient Occulusion. This is meant to be used with a checkbox.
        /// </summary>
        /// <param name="b"></param>
        public void toggleAO(bool b)
        {
            try
            {
                tempScript = (MonoBehaviour)mainCamObj.GetComponent(AOScriptName);
                tempScript.enabled = b;
                aoBool = b;            }
            catch
            {
                Debug.Log("No AO post processing found");
                return;
            }
        }
        /// <summary>
        /// Set the game to windowed or full screen. This is meant to be used with a checkbox
        /// </summary>
        /// <param name="b"></param>
        public void setFullScreen(bool b){ Screen.SetResolution(Screen.width, Screen.height, b); }


        private void changeRes(int index){
            for (int i = 0; i < allRes.Length; i++)
            {
                //If the resoultion matches the current resoution height and width then go through the statement.
                if (allRes[i].height == currentRes.height && allRes[i].width == currentRes.width)
                {
                    Screen.SetResolution(allRes[i+index].width, allRes[i+index].height, isFullscreen); isFullscreen = isFullscreen; currentRes = Screen.resolutions[i+index]; resolutionLabel.text = currentRes.width.ToString() + " x " + currentRes.height.ToString();
                }
            }
        }

        /// <summary>
        /// Method for moving to the next resoution in the allRes array. WARNING: This is not finished/buggy.  
        /// </summary>
        //Method for moving to the next resoution in the allRes array. WARNING: This is not finished/buggy.  
        public void nextRes()
        {
            beforeRes = currentRes;
            //Iterate through all of the resoultions. 
            changeRes(1);

        }
        /// <summary>
        /// Method for moving to the last resoution in the allRes array. WARNING: This is not finished/buggy.  
        /// </summary>
        //Method for moving to the last resoution in the allRes array. WARNING: This is not finished/buggy.  
        public void lastRes()
        {
            beforeRes = currentRes;
            changeRes(-1);
        }
        
        public void enableSimpleTerrain(Boolean b)
        {
            useSimpleTerrain = b;
        }

        /// <summary>
        /// The method for changing aniso settings
        /// </summary>
        /// <param name="anisoSetting"></param>
        public void updateANISO(int anisoSetting)
        {
            if (anisoSetting == 0)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            }
            else if (anisoSetting == 1)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
            }
            else if (anisoSetting == 2)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
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
            try
            {
                terrain.detailObjectDensity = detailDensity;
            }
            catch
            {
                Debug.Log("Please assign a terrain");
            }

        }

        /// <summary>
        /// Sets the MSAA to a specific level (between 0 and 4)
        /// </summary>
        /// <param name="level">log2 of the desired level. 
        /// 0 -> 0x MSAA (disabled). 
        /// 1 -> 2x MSAA.
        /// 2 -> 4x MSAA.
        /// 3 -> 8x MSAA.
        /// </param>
        public void SetMSAALevel(int level) {
            level = Mathf.Clamp(level, 0, 4);

            QualitySettings.antiAliasing = level == 0 ? 0 : 1 << level;

        }

#region GraphicPresets

        /// <summary>
        /// Set the quality level one level higher. This is done by getting the current quality level, then using 
        /// <c> 
        /// QualitySettings.IncreaseLevel();
        /// </c>
        /// to increase the level. The current level variable is set to the new quality setting, and the label is updated.
        /// </summary>
        public void nextPreset()
        {
            QualitySettings.IncreaseLevel();
            FromPreset();
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
            QualitySettings.DecreaseLevel();
            FromPreset();
        }

        private void FromPreset() {
             _currentLevel = QualitySettings.GetQualityLevel();
            presetLabel.text = presets[_currentLevel];
            if (hardCodeSomeVideoSettings)
            {
                QualitySettings.shadowDistance = shadowDist[_currentLevel];
                QualitySettings.lodBias = LODBias[_currentLevel];
            }
        }


        /// <summary>
        /// Sets the Graphic to a Preset (from very low to extreme)
        ///     (note: UI buttons in the inspector can carry a parameter, so you wont need 7 methods)
        /// </summary>
        public void SetGraphicsPreset(int preset) {
            preset = Mathf.Clamp(preset, 0, 6);

            QualitySettings.SetQualityLevel(preset);
            QualitySettings.shadowDistance = shadowDist[preset];
            QualitySettings.lodBias = LODBias[preset];
            
            // in the previous 7 methods were hardcoded values but commented out
            // the logic behind those hardcoded values can be archived by this:
            // QualitySettings.shadowDistance = shadowPreset[preset];
        }
        // private static readonly float[] shadowPreset = {12.6f, 17.4f, 29.7f, 82f, 110f, 338f, 800f};

#endregion

        /// <summary>
        /// Return to the main menu from the credits panel
        /// </summary>
        public void creditsReturn()
        {
            StartCoroutine(creditsReturnMain());
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then hide other panels settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator creditsReturnMain()
        {
            creditsPanelAnimator.Play("Credits Panel Out 1");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)creditsPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            mainPanel.SetActive(true);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            creditsPanel.SetActive(false);
        }
        public void creditsIn()
        {
            mainPanel.SetActive(false);
            vidPanel.SetActive(false);
            audioPanel.SetActive(false);
            creditsPanel.SetActive(true);
            creditsPanelAnimator.enabled = true;
            uiEventSystem.SetSelectedGameObject(defualtSelectedCredits);
            creditsPanelAnimator.Play("Credits Panel In");

        }
    }
}
