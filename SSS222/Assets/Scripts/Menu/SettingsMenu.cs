using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class SettingsMenu : MonoBehaviour{
    //Settings settings;
    GameSession gameSession;
    SaveSerial saveSerial;
    public int quality;
    public bool fullscreen;
    public bool pprocessing;
    public bool scbuttons;
    public bool moveByMouse;
    public float masterVolume;
    public float soundVolume;
    public float musicVolume;
    [SerializeField]GameObject qualityDropdopwn;
    [SerializeField]GameObject fullscreenToggle;
    [SerializeField]GameObject pprocessingToggle;
    [SerializeField]GameObject scbuttonsToggle;
    [SerializeField]GameObject steeringToggle;
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
        saveSerial = FindObjectOfType<SaveSerial>();
        audioSource = GetComponent<AudioSource>();
        
        /*quality = qualityDropdopwn.GetComponent<Dropdown>().value;
        fullscreen = fullscreenToggle.GetComponent<Toggle>().isOn;
        moveByMouse = steeringToggle.GetComponent<Toggle>().isOn;*/

        qualityDropdopwn.GetComponent<Dropdown>().value = saveSerial.quality;
        fullscreenToggle.GetComponent<Toggle>().isOn = saveSerial.fullscreen;
        pprocessingToggle.GetComponent<Toggle>().isOn = saveSerial.pprocessing;
        scbuttonsToggle.GetComponent<Toggle>().isOn = saveSerial.scbuttons;
        steeringToggle.GetComponent<Toggle>().isOn = saveSerial.moveByMouse;

        masterSlider.GetComponent<Slider>().value = saveSerial.masterVolume;
        soundSlider.GetComponent<Slider>().value = saveSerial.soundVolume;
        musicSlider.GetComponent<Slider>().value = saveSerial.musicVolume;
    }
    private void Update() {
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(pprocessing==true && postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
        if(pprocessing==true && FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume.enabled=true;}
        if(pprocessing==false && FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }
    public void SetMasterVolume(float volume){
        audioMixer.SetFloat("MasterVolume", volume);
        masterVolume = volume;
    }public void SetSoundVolume(float volume){
        audioMixer.SetFloat("SoundVolume", volume);
        soundVolume = volume;
    }
    public void SetMusicVolume(float volume){
        audioMixer.SetFloat("MusicVolume", volume);
        musicVolume = volume;
    }
    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        quality = qualityIndex;
    }
    public void SetFullscreen (bool isFullscreen){
        Screen.fullScreen = isFullscreen;
        fullscreen = isFullscreen;
        Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,isFullscreen,60);
    }public void SetPostProcessing (bool isPostprocessed){
        //gameSession.pprocessing = isPostprocessed;
        pprocessing = isPostprocessed;
        if(isPostprocessed==true && postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}//FindObjectOfType<Level>().RestartScene();}
        if(isPostprocessed==true && postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(isPostprocessed==false && FindObjectOfType<PostProcessVolume>()!=null){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }public void SetOnScreenButtons (bool onscbuttons){
        scbuttons = onscbuttons;
        //if(onscbuttons){Debug.Log(scbuttons);scbuttons=true;}
    }
    public void SetSteering(bool isMovingByMouse){
        moveByMouse = isMovingByMouse;
        //saveSerial.moveByMouse = isMovingByMouse;
    }public void SetCheatmode(bool isCheatmode){
        GameSession.instance.cheatmode = isCheatmode;
    }
    public void PlayDing(){
        audioSource.Play();
    }
}
