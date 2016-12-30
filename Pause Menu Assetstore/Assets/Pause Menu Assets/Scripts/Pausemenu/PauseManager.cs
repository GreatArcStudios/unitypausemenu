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
        public GameObject MainPanel;
        /// <summary>
        /// This is the video panel holder, which holds all of the controls for the video panel and should be called "vid panel"
        /// </summary>
        public GameObject VidPanel;
        /// <summary>
        /// This is the audio panel holder, which holds all of the silders for the audio panel and should be called "audio panel"
        /// </summary>
        public GameObject AudioPanel;
        /// <summary>
        /// This is the credits panel holder, which holds all of the silders for the audio panel and should be called "credits panel"
        /// </summary>
        public GameObject CreditsPanel;
        /// <summary>
        /// These are the game objects with the title texts like "Pause menu" and "Game Title" 
        /// </summary>
        public GameObject TitleTexts;
        /// <summary>
        /// The mask that makes the scene darker  
        /// </summary>
        public GameObject Mask;
        /// <summary>
        /// Audio Panel animator
        /// </summary>
        public Animator AudioPanelAnimator;
        /// <summary>
        /// Video Panel animator  
        /// </summary>
        public Animator VidPanelAnimator;
        /// <summary>
        /// Quit Panel animator  
        /// </summary>
        public Animator QuitPanelAnimator;
        /// <summary>
        /// Credits Panel animator  
        /// </summary>
        public Animator CreditsPanelAnimator;
        /// <summary>
        /// Pause menu text 
        /// </summary>
        public Text PauseMenu;

        /// <summary>
        /// Main menu level string used for loading the main menu. This means you'll need to type in the editor text box, the name of the main menu level, ie: "mainmenu";
        /// </summary>
        public String MainMenu;
        //DOF script name
        /// <summary>
        /// The Depth of Field script name, ie: "DepthOfField". You can leave this blank in the editor, but will throw a null refrence exception, which is harmless.
        /// </summary>
        public String DofScriptName;

        /// <summary>
        /// The Ambient Occlusion script name, ie: "AmbientOcclusion". You can leave this blank in the editor, but will throw a null refrence exception, which is harmless.
        /// </summary>
        public String AoScriptName;
        /// <summary>
        /// The main camera, assign this through the editor. 
        /// </summary>        
        public Camera MainCam;
        internal static Camera MainCamShared;
        /// <summary>
        /// The main camera game object, assign this through the editor. 
        /// </summary> 
        public GameObject MainCamObj;

        /// <summary>
        /// The terrain detail density float. It's only public because you may want to adjust it in editor
        /// </summary> 
        public float DetailDensity;

        /// <summary>
        /// Timescale value. The defualt is 1 for most games. You may want to change it if you are pausing the game in a slow motion situation 
        /// </summary> 
        public float TimeScale = 1f;
        /// <summary>
        /// One terrain variable used if you have a terrain plugin like rtp. 
        /// </summary>
        public Terrain Terrain;
        /// <summary>
        /// Other terrain variable used if you want to have an option to target low end harware.
        /// </summary>
        public Terrain SimpleTerrain;
        /// <summary>
        /// Inital shadow distance 
        /// </summary>
        internal static float ShadowDistIni;
        /// <summary>
        /// Inital render distance 
        /// </summary>
        internal static float RenderDistIni;
        /// <summary>
        /// Inital AA quality 2, 4, or 8
        /// </summary>
        internal static float AaQualIni;
        /// <summary>
        /// Inital terrain detail density
        /// </summary>
        internal static float DensityIni;
        /// <summary>
        /// Amount of trees that are acutal meshes
        /// </summary>
        internal static float TreeMeshAmtIni;
        /// <summary>
        /// Inital fov 
        /// </summary>
        internal static float FovIni;
        /// <summary>
        /// Inital msaa amount 
        /// </summary>
        internal static int MsaaIni;
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
        internal static int VsyncIni;
        /// <summary>
        /// AA drop down menu.
        /// </summary>
        public Dropdown AaCombo;
        /// <summary>
        /// Aniso drop down menu.
        /// </summary>
        public Dropdown AfCombo;

        public Slider FovSlider;
        public Slider ModelQualSlider;
        public Slider TerrainQualSlider;
        public Slider HighQualTreeSlider;
        public Slider RenderDistSlider;
        public Slider TerrainDensitySlider;
        public Slider ShadowDistSlider;
        public Slider AudioMasterSlider;
        public Slider AudioMusicSlider;
        public Slider AudioEffectsSlider;
        public Slider MasterTexSlider;
        public Slider ShadowCascadesSlider;
        public Toggle VSyncToggle;
        public Toggle AoToggle;
        public Toggle DofToggle;
        public Toggle FullscreenToggle;
        /// <summary>
        /// The preset text label.
        /// </summary>
        public Text PresetLabel;
        /// <summary>
        /// Resolution text label.
        /// </summary>
        public Text ResolutionLabel;
        /// <summary>
        /// Lod bias float array. You should manually assign these based on the quality level.
        /// </summary>
        public float[] LodBias;
        /// <summary>
        /// Shadow distance array. You should manually assign these based on the quality level.
        /// </summary>
        public float[] ShadowDist;
        /// <summary>
        /// An array of music audio sources
        /// </summary>
        public AudioSource[] Music;
        /// <summary>
        /// An array of sound effect audio sources
        /// </summary>
        public AudioSource[] Effects;
        /// <summary>
        /// An array of the other UI elements, which is used for disabling the other elements when the game is paused.
        /// </summary>
        public GameObject[] OtherUiElements;
        /// <summary>
        /// Editor boolean for hardcoding certain video settings. It will allow you to use the values defined in LOD Bias and Shadow Distance
        /// </summary>
        public Boolean HardCodeSomeVideoSettings;
        /// <summary>
        /// Boolean for turning on simple terrain
        /// </summary>
        public Boolean UseSimpleTerrain;
        public static Boolean ReadUseSimpleTerrain;
        /// <summary>
        /// Event system
        /// </summary>
        public EventSystem UiEventSystem;
        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject DefualtSelectedVideo;
        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject DefualtSelectedAudio;
        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject DefualtSelectedMain;
        public GameObject DefualtSelectedCredits;
        //last music multiplier; this should be a value between 0-1
        internal static float LastMusicMult;
        //last audio multiplier; this should be a value between 0-1
        internal static float LastAudioMult;
        //Initial master volume
        internal static float BeforeMaster;
        //last texture limit 
        internal static int LastTexLimit;
        //int for amount of effects
        private int _audioEffectAmt = 0;
        //Inital audio effect volumes
        private float[] _beforeEffectVol;

        //Initial music volume
        private float _beforeMusic;
        //Preset level
        private int _currentLevel;
        //Resoutions
        private Resolution[] _allRes;
        //Camera dof script
        private MonoBehaviour _tempScript;
        //Presets 
        private String[] _presets;
        //Fullscreen Boolean
        private Boolean _isFullscreen;
        //current resoultion
        internal static Resolution CurrentRes;
        //Last resoultion 
        private Resolution _beforeRes;

        //last shadow cascade value
        internal static int LastShadowCascade;

        public static Boolean AoBool;
        public static Boolean DofBool;
        private Boolean _lastAoBool;
        private Boolean _lastDofBool;
        public static Terrain ReadTerrain;
        public static Terrain ReadSimpleTerrain;

        private SaveSettings _saveSettings = new SaveSettings();


        private Terrain CurrentTerrain
        {
            get
            {
                return UseSimpleTerrain ? SimpleTerrain : Terrain;
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

            ReadUseSimpleTerrain = UseSimpleTerrain;
            if (UseSimpleTerrain)
            {
                ReadSimpleTerrain = SimpleTerrain;
            }
            else
            {
                ReadTerrain = Terrain;
            }

            MainCamShared = MainCam;
            //Set the lastmusicmult and last audiomult
            LastMusicMult = AudioMusicSlider.value;
            LastAudioMult = AudioEffectsSlider.value;
            //Set the first selected item
            UiEventSystem.firstSelectedGameObject = DefualtSelectedMain;
            //Get the presets from the quality settings 
            _presets = QualitySettings.names;
            PresetLabel.text = _presets[QualitySettings.GetQualityLevel()].ToString();
            _currentLevel = QualitySettings.GetQualityLevel();
            //Get the current resoultion, if the game is in fullscreen, and set the label to the original resolution
            _allRes = Screen.resolutions;
            CurrentRes.width = Screen.width;
            CurrentRes.height = Screen.height;
            //Debug.Log("ini res" + currentRes);
            ResolutionLabel.text = Screen.width.ToString() + " x " + Screen.height.ToString();
            _isFullscreen = Screen.fullScreen;
            //get initial screen effect bools
            _lastAoBool = AoToggle.isOn;
            _lastDofBool = DofToggle.isOn;
            //get all specified audio source volumes
            _beforeEffectVol = new float[_audioEffectAmt];
            BeforeMaster = AudioListener.volume;
            //get all ini values
            AaQualIni = QualitySettings.antiAliasing;
            RenderDistIni = MainCam.farClipPlane;
            ShadowDistIni = QualitySettings.shadowDistance;
            FovIni = MainCam.fieldOfView;
            MsaaIni = QualitySettings.antiAliasing;
            VsyncIni = QualitySettings.vSyncCount;
            //enable titles
            TitleTexts.SetActive(true);
            //Find terrain
            Terrain = Terrain.activeTerrain;
            //Disable other panels
            MainPanel.SetActive(false);
            VidPanel.SetActive(false);
            AudioPanel.SetActive(false);
            CreditsPanel.SetActive(false);
            //Enable mask
            Mask.SetActive(false);
            //set last texture limit
            LastTexLimit = QualitySettings.masterTextureLimit;
            //set last shadow cascade 
            LastShadowCascade = QualitySettings.shadowCascades;
            try
            {
                _saveSettings.LoadGameSettings();
            }
            catch
            {
                Debug.Log("Game settings not found in: " + Application.persistentDataPath + "/" + _saveSettings.FileName);
                _saveSettings.SaveGameSettings();
            }

            try
            {
                DensityIni = Terrain.activeTerrain.detailObjectDensity;
            }
            catch
            {
                if (Terrain = null)
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
            UiEventSystem.firstSelectedGameObject = DefualtSelectedMain;
	        Time.timeScale = TimeScale; 	
        }
        /// <summary>
        /// Method to resume the game, so disable the pause menu and re-enable all other ui elements
        /// </summary>
        public void Resume()
        {
            Time.timeScale = TimeScale;

            MainPanel.SetActive(false);
            VidPanel.SetActive(false);
            AudioPanel.SetActive(false);
            TitleTexts.SetActive(false);
            Mask.SetActive(false);
            for (int i = 0; i < OtherUiElements.Length; i++)
            {
                OtherUiElements[i].gameObject.SetActive(true);
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
        public void QuitOptions()
        {
            VidPanel.SetActive(false);
            AudioPanel.SetActive(false);
            QuitPanelAnimator.enabled = true;
            QuitPanelAnimator.Play("QuitPanelIn");

        }
        /// <summary>
        /// Method to quit the game. Call methods such as auto saving before qutting here.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        /// <summary>
        /// Cancels quittting by playing an animation.
        /// </summary>
        public void QuitCancel()
        {
            QuitPanelAnimator.Play("QuitPanelOut");
        }
        /// <summary>
        ///Loads the main menu scene.
        /// </summary>
        public void ReturnToMenu()
        {
            Application.LoadLevel(MainMenu);
            UiEventSystem.SetSelectedGameObject(DefualtSelectedMain);
        }

        // Update is called once per frame
        /// <summary>
        /// The update method. This mainly searches for the user pressing the escape key.
        /// </summary>
        public void Update()
        {
            ReadUseSimpleTerrain = UseSimpleTerrain;
            UseSimpleTerrain = ReadUseSimpleTerrain;
            //colorCrossfade();
            if (VidPanel.active == true)
            {
                PauseMenu.text = "Video Menu";
            }
            else if (AudioPanel.active == true)
            {
                PauseMenu.text = "Audio Menu";
            }
            else if (MainPanel.active == true)
            {
                PauseMenu.text = "Pause Menu";
            }

            if (Input.GetKeyDown(KeyCode.Escape) && MainPanel.active == false)
            {

                UiEventSystem.SetSelectedGameObject(DefualtSelectedMain);
                MainPanel.SetActive(true);
                VidPanel.SetActive(false);
                AudioPanel.SetActive(false);
                TitleTexts.SetActive(true);
                Mask.SetActive(true);
                Time.timeScale = 0;
                for (int i = 0; i < OtherUiElements.Length; i++)
                {
                    OtherUiElements[i].gameObject.SetActive(false);
                }
                /* if (blurBool == false)
                  {
                     blurEffect.enabled = true;
                 }  */
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && MainPanel.active == true)
            {

                Time.timeScale = TimeScale;
                MainPanel.SetActive(false);
                VidPanel.SetActive(false);
                AudioPanel.SetActive(false);
                TitleTexts.SetActive(false);
                Mask.SetActive(false);
                for (int i = 0; i < OtherUiElements.Length; i++)
                {
                    OtherUiElements[i].gameObject.SetActive(true);
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
            MainPanel.SetActive(false);
            VidPanel.SetActive(false);
            AudioPanel.SetActive(true);
            AudioPanelAnimator.enabled = true;
            AudioIn();
            PauseMenu.text = "Audio Menu";
        }
        /// <summary>
        /// Play the "audio panel in" animation.
        /// </summary>
        public void AudioIn()
        {
            UiEventSystem.SetSelectedGameObject(DefualtSelectedAudio);
            AudioPanelAnimator.Play("Audio Panel In");
            AudioMasterSlider.value = AudioListener.volume;
            //Perform modulo to find factor f to allow for non uniform music volumes
            float a; float b; float f;
            try
            {
                a = Music[0].volume;
                b = Music[1].volume;
                f = a % b;
                AudioMusicSlider.value = f;
            }
            catch
            {
                Debug.Log("You do not have multiple audio sources");
                AudioMusicSlider.value = LastMusicMult;
            }
            //Do this with the effects
            try
            {
                a = Effects[0].volume;
                b = Effects[1].volume;
                f = a % b;
                AudioEffectsSlider.value = f;
            }
            catch
            {
                Debug.Log("You do not have multiple audio sources");
                AudioEffectsSlider.value = LastAudioMult;
            }

        }
        /// <summary>
        /// Audio Option Methods
        /// </summary>
        /// <param name="f"></param>
        public void UpdateMasterVol(float f)
        {

            //Controls volume of all audio listeners 
            AudioListener.volume = f;
        }
        /// <summary>
        /// Update music effects volume
        /// </summary>
        /// <param name="f"></param>
        public void UpdateMusicVol(float f)
        {
            try
            {
                for (int musicAmt = 0; musicAmt < Music.Length; musicAmt++)
                {
                    Music[musicAmt].volume *= f;
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
        public void UpdateEffectsVol(float f)
        {
            try
            {
                for (_audioEffectAmt = 0; _audioEffectAmt < Effects.Length; _audioEffectAmt++)
                {
                    //get the values for all effects before the change
                    _beforeEffectVol[_audioEffectAmt] = Effects[_audioEffectAmt].volume;

                    //lower it by a factor of f because we don't want every effect to be set to a uniform volume
                    Effects[_audioEffectAmt].volume *= f;
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
        public void ApplyAudio()
        {
            StartCoroutine(ApplyAudioMain());
            UiEventSystem.SetSelectedGameObject(DefualtSelectedMain);

        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the audio settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator ApplyAudioMain()
        {
            AudioPanelAnimator.Play("Audio Panel Out");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)AudioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            MainPanel.SetActive(true);
            VidPanel.SetActive(false);
            AudioPanel.SetActive(false);
            BeforeMaster = AudioListener.volume;
            LastMusicMult = AudioMusicSlider.value;
            LastAudioMult = AudioEffectsSlider.value;
            _saveSettings.SaveGameSettings();
        }
        /// <summary>
        /// Cancel the audio setting changes
        /// </summary>
        public void CancelAudio()
        {
            UiEventSystem.SetSelectedGameObject(DefualtSelectedMain);
            StartCoroutine(CancelAudioMain());
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the audio settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator CancelAudioMain()
        {
            AudioPanelAnimator.Play("Audio Panel Out");
            // Debug.Log(audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length);
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)AudioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            MainPanel.SetActive(true);
            VidPanel.SetActive(false);
            AudioPanel.SetActive(false);
            AudioListener.volume = BeforeMaster;
            //Debug.Log(_beforeMaster + AudioListener.volume);
            try
            {
                for (_audioEffectAmt = 0; _audioEffectAmt < Effects.Length; _audioEffectAmt++)
                {
                    //get the values for all effects before the change
                    Effects[_audioEffectAmt].volume = _beforeEffectVol[_audioEffectAmt];
                }
                for (int musicAmt = 0; musicAmt < Music.Length; musicAmt++)
                {
                    Music[musicAmt].volume = _beforeMusic;
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
            MainPanel.SetActive(false);
            VidPanel.SetActive(true);
            AudioPanel.SetActive(false);
            VidPanelAnimator.enabled = true;
            VideoIn();
            PauseMenu.text = "Video Menu";

        }


        private static readonly Dictionary<int, int> AaDict = new Dictionary<int, int>() { { 0, 0 }, { 2, 1 }, { 4, 2 }, { 8, 3 } };

        /// <summary>
        /// Play the "video panel in" animation
        /// </summary>
        public void VideoIn()
        {
            UiEventSystem.SetSelectedGameObject(DefualtSelectedVideo);
            VidPanelAnimator.Play("Video Panel In");

            AaCombo.value = AaDict[QualitySettings.antiAliasing];

            // --------
            // todo: that stuff is stupid:
            if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable)
            {
                AfCombo.value = 1;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable)
            {
                AfCombo.value = 0;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable)
            {
                AfCombo.value = 2;
            }
            /* 
             * the unity constants already have int values:
                 * AnisotropicFiltering.ForceEnable = 2
                 * AnisotropicFiltering.Enable = 1
             * would be smarter to use them, also this hard coded values just work with the "hardcoded" dropdownlist
             */
            // --------

            PresetLabel.text = _presets[QualitySettings.GetQualityLevel()].ToString();
            FovSlider.value = MainCam.fieldOfView;
            ModelQualSlider.value = QualitySettings.lodBias;
            RenderDistSlider.value = MainCam.farClipPlane;
            ShadowDistSlider.value = QualitySettings.shadowDistance;
            MasterTexSlider.value = QualitySettings.masterTextureLimit;
            ShadowCascadesSlider.value = QualitySettings.shadowCascades;
            FullscreenToggle.isOn = Screen.fullScreen;
            AoToggle.isOn = AoBool;
            DofToggle.isOn = DofBool;
            if (QualitySettings.vSyncCount == 0)
            {
                VSyncToggle.isOn = false;
            }
            else if (QualitySettings.vSyncCount == 1)
            {
                VSyncToggle.isOn = true;
            }
            try
            {
                HighQualTreeSlider.value = CurrentTerrain.treeMaximumFullLODCount;
                TerrainDensitySlider.value = CurrentTerrain.detailObjectDensity;
                TerrainQualSlider.value = CurrentTerrain.heightmapMaximumLOD;
            }
            catch
            {
                return;
            }

        }

        /// <summary>
        /// Cancel the video setting changes 
        /// </summary>
        public void CancelVideo()
        {
            UiEventSystem.SetSelectedGameObject(DefualtSelectedMain);
            StartCoroutine(CancelVideoMain());
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then changethe video settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator CancelVideoMain()
        {
            VidPanelAnimator.Play("Video Panel Out");

            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)VidPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            try
            {
                MainCam.farClipPlane = RenderDistIni;
                Terrain.activeTerrain.detailObjectDensity = DensityIni;
                MainCam.fieldOfView = FovIni;
                MainPanel.SetActive(true);
                VidPanel.SetActive(false);
                AudioPanel.SetActive(false);
                AoBool = _lastAoBool;
                DofBool = _lastDofBool;
                Screen.SetResolution(_beforeRes.width, _beforeRes.height, Screen.fullScreen);
                QualitySettings.shadowDistance = ShadowDistIni;
                QualitySettings.antiAliasing = (int)AaQualIni;
                QualitySettings.antiAliasing = MsaaIni;
                QualitySettings.vSyncCount = VsyncIni;
                QualitySettings.masterTextureLimit = LastTexLimit;
                QualitySettings.shadowCascades = LastShadowCascade;
                Screen.fullScreen = _isFullscreen;
            }
            catch
            {

                Debug.Log("A problem occured (chances are the terrain was not assigned )");
                MainCam.farClipPlane = RenderDistIni;
                MainCam.fieldOfView = FovIni;
                MainPanel.SetActive(true);
                VidPanel.SetActive(false);
                AudioPanel.SetActive(false);
                AoBool = _lastAoBool;
                DofBool = _lastDofBool;
                QualitySettings.shadowDistance = ShadowDistIni;
                Screen.SetResolution(_beforeRes.width, _beforeRes.height, Screen.fullScreen);
                QualitySettings.antiAliasing = (int)AaQualIni;
                QualitySettings.antiAliasing = MsaaIni;
                QualitySettings.vSyncCount = VsyncIni;
                QualitySettings.masterTextureLimit = LastTexLimit;
                QualitySettings.shadowCascades = LastShadowCascade;
                //Screen.fullScreen = isFullscreen;

            }

        }
        //Apply the video prefs
        /// <summary>
        /// Apply the video settings
        /// </summary>
        public void Apply()
        {
            StartCoroutine(ApplyVideo());
            UiEventSystem.SetSelectedGameObject(DefualtSelectedMain);

        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the video settings.
        /// </summary>
        /// <returns></returns>
        internal IEnumerator ApplyVideo()
        {
            VidPanelAnimator.Play("Video Panel Out");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)VidPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            MainPanel.SetActive(true);
            VidPanel.SetActive(false);
            AudioPanel.SetActive(false);
            RenderDistIni = MainCam.farClipPlane;
            ShadowDistIni = QualitySettings.shadowDistance;
            Debug.Log("Shadow dist ini" + ShadowDistIni);
            FovIni = MainCam.fieldOfView;
            AoBool = AoToggle.isOn;
            DofBool = DofToggle.isOn;
            _lastAoBool = AoBool;
            _lastDofBool = DofBool;
            _beforeRes = CurrentRes;
            LastTexLimit = QualitySettings.masterTextureLimit;
            LastShadowCascade = QualitySettings.shadowCascades;
            VsyncIni = QualitySettings.vSyncCount;
            _isFullscreen = Screen.fullScreen;
            try
            {
                DensityIni = CurrentTerrain.detailObjectDensity;
                TreeMeshAmtIni = CurrentTerrain.treeMaximumFullLODCount;
            }
            catch { Debug.Log("Please assign a terrain"); }
            _saveSettings.SaveGameSettings();

        }


        public void TurnOnVSync(bool b)
        {
            VsyncIni = QualitySettings.vSyncCount;
            QualitySettings.vSyncCount = b ? 1 : 0;
        }

        /// <summary>
        /// Update full high quality tree mesh amount.
        /// </summary>
        /// <param name="f"></param>
        public void UpdateTreeMeshAmt(int f)
        {
            CurrentTerrain.treeMaximumFullLODCount = f;
        }

        /// <summary>
        /// Change the lod bias using
        /// <c>
        /// QualitySettings.lodBias = LoDBias / 2.15f;
        /// </c> 
        /// LoDBias is only divided by 2.15 because the max is set to 10 on the slider, and dividing by 2.15 results in 4.65, our desired max. However, deleting or changing 2.15 is compeletly fine.
        /// </summary>
        /// <param name="loDBias"></param>
        public void SetLodBias(float loDBias)
        {
            QualitySettings.lodBias = loDBias / 2.15f;
        }
        /// <summary>
        /// Update the render distance using 
        /// <c>
        /// mainCam.farClipPlane = f;
        /// </c>
        /// </summary>
        /// <param name="f"></param>
        public void UpdateRenderDist(float f)
        {
            try
            {
                MainCam.farClipPlane = f;
            }
            catch
            {
                Debug.Log(" Finding main camera now...it is still suggested that you manually assign this");
                MainCam = Camera.main;
                MainCam.farClipPlane = f;
            }

        }
        /// <summary>
        /// Update the texture quality using  
        /// <c>QualitySettings.masterTextureLimit </c>
        /// </summary>
        /// <param name="qual"></param>
        public void UpdateTex(float qual)
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
        public void UpdateShadowDistance(float dist)
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
        public void TreeMaxLod(float qual)
        {
            CurrentTerrain.treeMaximumFullLODCount = (int)qual;
        }

        /// <summary>
        /// Change the height map max LOD using 
        /// <c>
        /// terrain.heightmapMaximumLOD = (int)qual;
        /// </c>
        /// </summary>
        /// <param name="qual"></param>
        public void UpdateTerrainLod(float qual)
        {
            if (CurrentTerrain == null) return; // fail silently
            CurrentTerrain.heightmapMaximumLOD = (int)qual;
        }
        /// <summary>
        /// Change the fov using a float. The defualt should be 60.
        /// </summary>
        /// <param name="fov"></param>
        public void UpdateFov(float fov)
        {
            MainCam.fieldOfView = fov;
        }
        /// <summary>
        /// Toggle on or off Depth of Field. This is meant to be used with a checkbox.
        /// </summary>
        /// <param name="b"></param>
        public void ToggleDof(bool b)
        {
            try
            {
                _tempScript = (MonoBehaviour)MainCamObj.GetComponent(DofScriptName);
                _tempScript.enabled = b;
                DofBool = b;
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
        public void ToggleAo(bool b)
        {
            try
            {
                _tempScript = (MonoBehaviour)MainCamObj.GetComponent(AoScriptName);
                _tempScript.enabled = b;
                AoBool = b;
            }
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
        public void SetFullScreen(bool b) { Screen.SetResolution(Screen.width, Screen.height, b); }


        private void ChangeRes(int index)
        {
            for (int i = 0; i < _allRes.Length; i++)
            {
                //If the resoultion matches the current resoution height and width then go through the statement.
                if (_allRes[i].height == CurrentRes.height && _allRes[i].width == CurrentRes.width)
                {
                    Screen.SetResolution(_allRes[i + index].width, _allRes[i + index].height, _isFullscreen); _isFullscreen = _isFullscreen; CurrentRes = Screen.resolutions[i + index]; ResolutionLabel.text = CurrentRes.width.ToString() + " x " + CurrentRes.height.ToString();
                }
            }
        }

        /// <summary>
        /// Method for moving to the next resoution in the allRes array. WARNING: This is not finished/buggy.  
        /// </summary>
        //Method for moving to the next resoution in the allRes array. WARNING: This is not finished/buggy.  
        public void NextRes()
        {
            _beforeRes = CurrentRes;
            //Iterate through all of the resoultions. 
            ChangeRes(1);

        }
        /// <summary>
        /// Method for moving to the last resoution in the allRes array. WARNING: This is not finished/buggy.  
        /// </summary>
        //Method for moving to the last resoution in the allRes array. WARNING: This is not finished/buggy.  
        public void LastRes()
        {
            _beforeRes = CurrentRes;
            ChangeRes(-1);
        }

        public void EnableSimpleTerrain(Boolean b)
        {
            UseSimpleTerrain = b;
        }

        /// <summary>
        /// The method for changing aniso settings
        /// </summary>
        /// <param name="anisoSetting"></param>
        public void UpdateAniso(int anisoSetting)
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
        public void UpdateCascades(float cascades)
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
        public void UpdateDensity(float density)
        {
            DetailDensity = density;
            try
            {
                Terrain.detailObjectDensity = DetailDensity;
            }
            catch
            {
                Debug.Log("Please assign a terrain");
            }

        }

        /// <summary>
        /// Sets the MSAA to a specific level (between 0 and 4)
        /// </summary>
        /// <param name="level">
        /// 0 -> 0x MSAA (disabled). 
        /// 1 -> 2x MSAA.
        /// 2 -> 4x MSAA.
        /// 3 -> 8x MSAA.
        /// Left shift works too by getting the log2 of the desired level. 
        /// <c>
        /// QualitySettings.antiAliasing = level == 0 ? 0 : 1 *left shift operator* level ;
        /// </c>
        /// </param>
        public void SetMsaaLevel(int level)
        {
            level = Mathf.Clamp(level, 0, 4);

            QualitySettings.antiAliasing = level == 0 ? 0 : (int)Math.Pow(2.0d, level);

        }

        #region GraphicPresets

        /// <summary>
        /// Set the quality level one level higher. This is done by getting the current quality level, then using 
        /// <c> 
        /// QualitySettings.IncreaseLevel();
        /// </c>
        /// to increase the level. The current level variable is set to the new quality setting, and the label is updated.
        /// </summary>
        public void NextPreset()
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
        public void LastPreset()
        {
            QualitySettings.DecreaseLevel();
            FromPreset();
        }

        private void FromPreset()
        {
            _currentLevel = QualitySettings.GetQualityLevel();
            PresetLabel.text = _presets[_currentLevel];
            if (HardCodeSomeVideoSettings)
            {
                QualitySettings.shadowDistance = ShadowDist[_currentLevel];
                QualitySettings.lodBias = LodBias[_currentLevel];
            }
        }


        /// <summary>
        /// Sets the Graphic to a Preset (from very low to extreme)
        ///     (note: UI buttons in the inspector can carry a parameter, so you wont need 7 methods)
        /// </summary>
        public void SetGraphicsPreset(int preset)
        {
            preset = Mathf.Clamp(preset, 0, 6);

            QualitySettings.SetQualityLevel(preset);
            QualitySettings.shadowDistance = ShadowDist[preset];
            QualitySettings.lodBias = LodBias[preset];

            // in the previous 7 methods were hardcoded values but commented out
            // the logic behind those hardcoded values can be archived by this:
            // QualitySettings.shadowDistance = shadowPreset[preset];
        }
        // private static readonly float[] shadowPreset = {12.6f, 17.4f, 29.7f, 82f, 110f, 338f, 800f};

        #endregion

        /// <summary>
        /// Return to the main menu from the credits panel
        /// </summary>
        public void CreditsReturn()
        {
            StartCoroutine(CreditsReturnMain());
            UiEventSystem.SetSelectedGameObject(DefualtSelectedMain);
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then hide other panels settings
        /// </summary>
        /// <returns></returns>
        internal IEnumerator CreditsReturnMain()
        {
            CreditsPanelAnimator.Play("Credits Panel Out 1");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)CreditsPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            MainPanel.SetActive(true);
            VidPanel.SetActive(false);
            AudioPanel.SetActive(false);
            CreditsPanel.SetActive(false);
        }
        public void CreditsIn()
        {
            MainPanel.SetActive(false);
            VidPanel.SetActive(false);
            AudioPanel.SetActive(false);
            CreditsPanel.SetActive(true);
            CreditsPanelAnimator.enabled = true;
            UiEventSystem.SetSelectedGameObject(DefualtSelectedCredits);
            CreditsPanelAnimator.Play("Credits Panel In");

        }
    }
}
