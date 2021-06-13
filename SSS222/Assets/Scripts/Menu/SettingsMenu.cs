using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour{
    [SerializeField] int panelActive=0;
    [SerializeField] GameObject[] panels;
    [SerializeField]GameObject qualityDropdopwn;
    [SerializeField]GameObject fullscreenToggle;
    [SerializeField]GameObject pprocessingToggle;
    [SerializeField]GameObject scbuttonsToggle;
    [SerializeField]GameObject steeringToggle;
    [SerializeField]GameObject lefthandToggle;
    [SerializeField]GameObject cheatToggle;
    [SerializeField]GameObject vibrationsToggle;
    [SerializeField]GameObject horizPlayfieldToggle;
    [SerializeField]GameObject masterSlider;
    [SerializeField]GameObject soundSlider;
    [SerializeField]GameObject musicSlider;
    public AudioMixer audioMixer;
    [SerializeField]GameObject pprocessingPrefab;
    public PostProcessVolume postProcessVolume;
    private void Start(){

        if(SaveSerial.instance!=null){
            qualityDropdopwn.GetComponent<Dropdown>().value=SaveSerial.instance.settingsData.quality;
            fullscreenToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.fullscreen;
            pprocessingToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.pprocessing;
            scbuttonsToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.scbuttons;
            //steeringToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.moveByMouse;
            lefthandToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.lefthand;
            cheatToggle.GetComponent<Toggle>().isOn=GameSession.instance.cheatmode;
            vibrationsToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.vibrations;
            bool h=false;if(SaveSerial.instance.settingsData.playfieldRot==PlaneDir.horiz){h=true;}horizPlayfieldToggle.GetComponent<Toggle>().isOn=h;

            masterSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.masterVolume;
            soundSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.soundVolume;
            musicSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.musicVolume;
        }
        if(SceneManager.GetActiveScene().name=="Options")OpenSettings();

        if(SaveSerial.instance!=null){
            foreach(Transform t in steeringToggle.transform.GetChild(0)){t.gameObject.SetActive(false);}
            steeringToggle.transform.GetChild(0).GetChild((int)SaveSerial.instance.settingsData.inputType).gameObject.SetActive(true);
        }
    }
    private void Update(){
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance!=null)if(SaveSerial.instance.settingsData.pprocessing==true&&postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
        if(SaveSerial.instance!=null)if(SaveSerial.instance.settingsData.pprocessing==true&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume.enabled=true;}
        if(SaveSerial.instance!=null)if(SaveSerial.instance.settingsData.pprocessing==false&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
        if(SaveSerial.instance.settingsData.masterVolume<=-40){SaveSerial.instance.settingsData.masterVolume=-80;}
        if(SaveSerial.instance.settingsData.soundVolume<=-40){SaveSerial.instance.settingsData.soundVolume=-80;}
        if(SaveSerial.instance.settingsData.musicVolume<=-40){SaveSerial.instance.settingsData.musicVolume=-80;}
    }
    public void SetPanelActive(int i){
        foreach(GameObject p in panels){p.SetActive(false);}panels[i].SetActive(true);
    }
    public void OpenSettings(){transform.GetChild(0).gameObject.SetActive(true);transform.GetChild(1).gameObject.SetActive(false);}
    public void OpenDeleteAll(){transform.GetChild(1).gameObject.SetActive(true);transform.GetChild(0).gameObject.SetActive(false);}
    public void Close(){transform.GetChild(0).gameObject.SetActive(false);transform.GetChild(1).gameObject.SetActive(false);}
    public void SetMasterVolume(float volume){
    if(GameSession.instance!=null){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.masterVolume=volume;
    }}public void SetSoundVolume(float volume){
    if(GameSession.instance!=null){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.soundVolume=volume;
    }}
    public void SetMusicVolume(float volume){
    if(GameSession.instance!=null){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.musicVolume=volume;
    }}
    public void SetQuality(int qualityIndex){
    if(GameSession.instance!=null){
        QualitySettings.SetQualityLevel(qualityIndex);
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.quality=qualityIndex;
    }}
    public void SetFullscreen (bool isFullscreen){
    if(GameSession.instance!=null){
        Screen.fullScreen = isFullscreen;
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.fullscreen=isFullscreen;
        Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,isFullscreen,60);
    }}
    public void SetPostProcessing (bool isPostprocessed){
    if(GameSession.instance!=null){
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance!=null)if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.pprocessing = isPostprocessed;
        if(isPostprocessed==true && postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}//FindObjectOfType<Level>().RestartScene();}
        if(isPostprocessed==true && postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(isPostprocessed==false && FindObjectOfType<PostProcessVolume>()!=null){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }}
    public void SetOnScreenButtons (bool onscbuttons){
        if(GameSession.instance!=null)if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.scbuttons=onscbuttons;
    }public void SetVibrations (bool vibrations){
        if(GameSession.instance!=null)if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.vibrations=vibrations;
    }public void SetPlayfieldRot (bool horiz){
        PlaneDir h=PlaneDir.vert;if(horiz==true){h=PlaneDir.horiz;}if(GameSession.instance!=null)if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.playfieldRot=h;
        if(SceneManager.GetActiveScene().name=="Game"&&Application.isPlaying){
            if(h==PlaneDir.horiz){FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,90);FindObjectOfType<Camera>().orthographicSize=GameSession.instance.horizCameraSize;}
            else{FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,0);FindObjectOfType<Camera>().orthographicSize=GameSession.instance.vertCameraSize;}
        }
    }
    public void SetSteering(){
    if(GameSession.instance!=null){
        SaveSerial s;
        if(SaveSerial.instance!=null){
        s=SaveSerial.instance;
        switch (s.settingsData.inputType){
            case (InputType)0:
                s.settingsData.inputType=(InputType)1;
                break;
            case (InputType)1:
                s.settingsData.inputType=(InputType)2;
                break;
            case (InputType)2:
                s.settingsData.inputType=(InputType)0;
                break;
        }
        foreach(Transform t in steeringToggle.transform.GetChild(0)){t.gameObject.SetActive(false);}
        steeringToggle.transform.GetChild(0).GetChild((int)s.settingsData.inputType).gameObject.SetActive(true);
        //if(Player.instance!=null){Player.instance.SetInputType(s.settingsData.inputType);}
        }
        //if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.moveByMouse = isMovingByMouse;
        //if(SaveSerial.instance!=null)SaveSerial.instance.moveByMouse = isMovingByMouse;
    }}
    public void SetJoystick(){
    if(GameSession.instance!=null){
        SaveSerial s;
        if(SaveSerial.instance!=null){
        s=SaveSerial.instance;
        switch (s.settingsData.joystickType){
            case (JoystickType)0:
                s.settingsData.joystickType=(JoystickType)1;
                break;
            case (JoystickType)1:
                s.settingsData.joystickType=(JoystickType)2;
                break;
            case (JoystickType)2:
                s.settingsData.joystickType=(JoystickType)0;
                break;
        }
        if(FindObjectOfType<Tag_Joystick>()!=null)FindObjectOfType<Tag_Joystick>().StartCoroutine(FindObjectOfType<Tag_Joystick>().ChangeType());
        }
    }}
    public void SetJoystickSize(float size){
        if(GameSession.instance!=null)if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.joystickSize=size;
    }public void SetLefthand(bool lefthand){
    if(GameSession.instance!=null){
            if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.lefthand=lefthand;
            if(FindObjectOfType<SwitchPlacesCanvas>()!=null)FindObjectOfType<SwitchPlacesCanvas>().Set();
    }}
    public void SetCheatmode(bool isCheatmode){
        if(GameSession.instance!=null)GameSession.instance.cheatmode = isCheatmode;
    }
    public void PlayDing(){
        if(GameSession.instance!=null)GetComponent<AudioSource>().Play();
    }
}
