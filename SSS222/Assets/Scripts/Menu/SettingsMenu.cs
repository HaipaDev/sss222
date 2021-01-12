using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class SettingsMenu : MonoBehaviour{
    [SerializeField] int panelActive=0;
    [SerializeField] GameObject[] panels;
    //Settings settings;
    GameSession gameSession;
    //SaveSerial saveSerial;
    /*public int quality;
    public bool fullscreen;
    public bool pprocessing;
    public bool scbuttons;
    public bool lefthand;
    public bool moveByMouse;
    public float masterVolume;
    public float soundVolume;
    public float musicVolume;*/
    [SerializeField]GameObject qualityDropdopwn;
    [SerializeField]GameObject fullscreenToggle;
    [SerializeField]GameObject pprocessingToggle;
    [SerializeField]GameObject scbuttonsToggle;
    [SerializeField]GameObject steeringToggle;
    [SerializeField]GameObject lefthandToggle;
    [SerializeField]GameObject masterSlider;
    [SerializeField]GameObject soundSlider;
    [SerializeField]GameObject musicSlider;
    [SerializeField]AudioSource audioSource;
    public AudioMixer audioMixer;
    [SerializeField]GameObject pprocessingPrefab;
    public PostProcessVolume postProcessVolume;
    private void Start(){
        //settings = FindObjectOfType<Settings>();
        gameSession = FindObjectOfType<GameSession>();
        SaveSerial.instance = FindObjectOfType<SaveSerial>();
        audioSource = GetComponent<AudioSource>();
        
        /*quality = qualityDropdopwn.GetComponent<Dropdown>().value;
        fullscreen = fullscreenToggle.GetComponent<Toggle>().isOn;
        moveByMouse = steeringToggle.GetComponent<Toggle>().isOn;*/

        qualityDropdopwn.GetComponent<Dropdown>().value = SaveSerial.instance.settingsData.quality;
        fullscreenToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.settingsData.fullscreen;
        pprocessingToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.settingsData.pprocessing;
        scbuttonsToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.settingsData.scbuttons;
        steeringToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.settingsData.moveByMouse;
        lefthandToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.settingsData.lefthand;

        masterSlider.GetComponent<Slider>().value = SaveSerial.instance.settingsData.masterVolume;
        soundSlider.GetComponent<Slider>().value = SaveSerial.instance.settingsData.soundVolume;
        musicSlider.GetComponent<Slider>().value = SaveSerial.instance.settingsData.musicVolume;
    }
    private void Update() {
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance.settingsData.pprocessing==true && postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
        if(SaveSerial.instance.settingsData.pprocessing==true && FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume.enabled=true;}
        if(SaveSerial.instance.settingsData.pprocessing==false && FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }
    public void SetPanelActive(int i){
        foreach(GameObject p in panels){p.SetActive(false);}panels[i].SetActive(true);
    }
    public void SetMasterVolume(float volume){
        audioMixer.SetFloat("MasterVolume", volume);
        SaveSerial.instance.settingsData.masterVolume = volume;
    }public void SetSoundVolume(float volume){
        audioMixer.SetFloat("SoundVolume", volume);
        SaveSerial.instance.settingsData.soundVolume = volume;
    }
    public void SetMusicVolume(float volume){
        audioMixer.SetFloat("MusicVolume", volume);
        SaveSerial.instance.settingsData.musicVolume = volume;
    }
    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        SaveSerial.instance.settingsData.quality = qualityIndex;
    }
    public void SetFullscreen (bool isFullscreen){
        Screen.fullScreen = isFullscreen;
        SaveSerial.instance.settingsData.fullscreen = isFullscreen;
        Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,isFullscreen,60);
    }public void SetPostProcessing (bool isPostprocessed){
        //gameSession.pprocessing = isPostprocessed;
        SaveSerial.instance.settingsData.pprocessing = isPostprocessed;
        if(isPostprocessed==true && postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}//FindObjectOfType<Level>().RestartScene();}
        if(isPostprocessed==true && postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(isPostprocessed==false && FindObjectOfType<PostProcessVolume>()!=null){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }public void SetOnScreenButtons (bool onscbuttons){
        SaveSerial.instance.settingsData.scbuttons = onscbuttons;
        //if(onscbuttons){Debug.Log(scbuttons);scbuttons=true;}
    }
    public void SetSteering(bool isMovingByMouse){
        SaveSerial.instance.settingsData.moveByMouse = isMovingByMouse;
        //SaveSerial.instance.moveByMouse = isMovingByMouse;
    }public void SetJoystick(){
        var s=SaveSerial.instance;
        if(s.settingsData.joystickType==(JoystickType)0){s.settingsData.joystickType=(JoystickType)1;return;}
        if(s.settingsData.joystickType==(JoystickType)1){s.settingsData.joystickType=(JoystickType)2;return;}
        if(s.settingsData.joystickType==(JoystickType)2){s.settingsData.joystickType=(JoystickType)0;return;}
    }public void SetJoystickSize(float size){
        SaveSerial.instance.settingsData.joystickSize=size;
    }public void SetLefthand(bool lefthand){
        SaveSerial.instance.settingsData.lefthand=lefthand;
    }public void SetCheatmode(bool isCheatmode){
        GameSession.instance.cheatmode = isCheatmode;
    }
    public void PlayDing(){
        audioSource.Play();
    }
}
