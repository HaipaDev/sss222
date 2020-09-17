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

        qualityDropdopwn.GetComponent<Dropdown>().value = SaveSerial.instance.quality;
        fullscreenToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.fullscreen;
        pprocessingToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.pprocessing;
        scbuttonsToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.scbuttons;
        steeringToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.moveByMouse;
        lefthandToggle.GetComponent<Toggle>().isOn = SaveSerial.instance.lefthand;

        masterSlider.GetComponent<Slider>().value = SaveSerial.instance.masterVolume;
        soundSlider.GetComponent<Slider>().value = SaveSerial.instance.soundVolume;
        musicSlider.GetComponent<Slider>().value = SaveSerial.instance.musicVolume;
    }
    private void Update() {
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance.pprocessing==true && postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
        if(SaveSerial.instance.pprocessing==true && FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume.enabled=true;}
        if(SaveSerial.instance.pprocessing==false && FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }
    public void SetPanelActive(int i){
        foreach(GameObject p in panels){p.SetActive(false);}panels[i].SetActive(true);
    }
    public void SetMasterVolume(float volume){
        audioMixer.SetFloat("MasterVolume", volume);
        SaveSerial.instance.masterVolume = volume;
    }public void SetSoundVolume(float volume){
        audioMixer.SetFloat("SoundVolume", volume);
        SaveSerial.instance.soundVolume = volume;
    }
    public void SetMusicVolume(float volume){
        audioMixer.SetFloat("MusicVolume", volume);
        SaveSerial.instance.musicVolume = volume;
    }
    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        SaveSerial.instance.quality = qualityIndex;
    }
    public void SetFullscreen (bool isFullscreen){
        Screen.fullScreen = isFullscreen;
        SaveSerial.instance.fullscreen = isFullscreen;
        Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,isFullscreen,60);
    }public void SetPostProcessing (bool isPostprocessed){
        //gameSession.pprocessing = isPostprocessed;
        SaveSerial.instance.pprocessing = isPostprocessed;
        if(isPostprocessed==true && postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}//FindObjectOfType<Level>().RestartScene();}
        if(isPostprocessed==true && postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(isPostprocessed==false && FindObjectOfType<PostProcessVolume>()!=null){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }public void SetOnScreenButtons (bool onscbuttons){
        SaveSerial.instance.scbuttons = onscbuttons;
        //if(onscbuttons){Debug.Log(scbuttons);scbuttons=true;}
    }
    public void SetSteering(bool isMovingByMouse){
        SaveSerial.instance.moveByMouse = isMovingByMouse;
        //SaveSerial.instance.moveByMouse = isMovingByMouse;
    }public void SetJoystick(){
        var s=SaveSerial.instance;
        if(s.joystickType==(JoystickType)0){s.joystickType=(JoystickType)1;return;}
        if(s.joystickType==(JoystickType)1){s.joystickType=(JoystickType)2;return;}
        if(s.joystickType==(JoystickType)2){s.joystickType=(JoystickType)0;return;}
    }public void SetJoystickSize(float size){
        SaveSerial.instance.joystickSize=size;
    }public void SetLefthand(bool lefthand){
        SaveSerial.instance.lefthand=lefthand;
    }public void SetCheatmode(bool isCheatmode){
        GameSession.instance.cheatmode = isCheatmode;
    }
    public void PlayDing(){
        audioSource.Play();
    }
}
