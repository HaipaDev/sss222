using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class SettingsMenu : MonoBehaviour{
    [SerializeField] int panelActive=0;
    [SerializeField] GameObject[] panels;
    [Header("Game")]
    [SceneObjectsOnly][SerializeField]GameObject steeringToggle;
    [SceneObjectsOnly][SerializeField]GameObject vibrationsToggle;
    [SceneObjectsOnly][SerializeField]GameObject horizPlayfieldToggle;
    [SceneObjectsOnly][SerializeField]GameObject dtapMouseShootToggle;
    [SceneObjectsOnly][SerializeField]GameObject lefthandToggle;
    [SceneObjectsOnly][SerializeField]GameObject scbuttonsToggle;
    [SceneObjectsOnly][SerializeField]GameObject cheatToggle;
    [Header("Sound")]
    public AudioMixer audioMixer;
    [SceneObjectsOnly][SerializeField]GameObject masterSlider;
    [SceneObjectsOnly][SerializeField]GameObject soundSlider;
    [SceneObjectsOnly][SerializeField]GameObject musicSlider;
    
    [Header("Graphics")]
    [SceneObjectsOnly][SerializeField]GameObject qualityDropdopwn;
    [SceneObjectsOnly][SerializeField]GameObject fullscreenToggle;
    [SceneObjectsOnly][SerializeField]GameObject pprocessingToggle;
    [SceneObjectsOnly][SerializeField]GameObject screenshakeToggle;
    [SceneObjectsOnly][SerializeField]GameObject dmgPopupsToggle;
    [SceneObjectsOnly][SerializeField]GameObject particlesToggle;
    [SceneObjectsOnly][SerializeField]GameObject screenflashToggle;

    [SceneObjectsOnly][SerializeField]GameObject hudVis_graphicsSlider;
    [SceneObjectsOnly][SerializeField]GameObject hudVis_textSlider;
    [SceneObjectsOnly][SerializeField]GameObject hudVis_barsSlider;
    [SceneObjectsOnly][SerializeField]GameObject hudVis_absorpSlider;
    [SceneObjectsOnly][SerializeField]GameObject hudVis_popupsSlider;
    [SceneObjectsOnly][SerializeField]GameObject hudVis_notifsSlider;

    [AssetsOnly][SerializeField]GameObject pprocessingPrefab;
    [SceneObjectsOnly]public PostProcessVolume postProcessVolume;
    private void Start(){
        if(SaveSerial.instance!=null){
            scbuttonsToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.scbuttons;
            dtapMouseShootToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.dtapMouseShoot;
            lefthandToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.lefthand;
            vibrationsToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.vibrations;
            cheatToggle.GetComponent<Toggle>().isOn=GameSession.instance.cheatmode;
            bool h=false;if(SaveSerial.instance.settingsData.playfieldRot==PlaneDir.horiz){h=true;}horizPlayfieldToggle.GetComponent<Toggle>().isOn=h;

            masterSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.masterVolume;
            soundSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.soundVolume;
            musicSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.musicVolume;

            qualityDropdopwn.GetComponent<Dropdown>().value=SaveSerial.instance.settingsData.quality;
            fullscreenToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.fullscreen;
            pprocessingToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.pprocessing;
            screenshakeToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.screenshake;
            dmgPopupsToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.dmgPopups;
            particlesToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.particles;
            screenflashToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.screenflash;

            
            hudVis_graphicsSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.hudVis_graphics;
            hudVis_textSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.hudVis_text;
            hudVis_barsSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.hudVis_barFill;
            hudVis_absorpSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.hudVis_absorpFill;
            hudVis_popupsSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.hudVis_popups;
            hudVis_notifsSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.hudVis_notif;
        }
        if(SceneManager.GetActiveScene().name=="Options")OpenSettings();

        if(SaveSerial.instance!=null){
            foreach(Transform t in steeringToggle.transform.GetChild(0)){t.gameObject.SetActive(false);}
            steeringToggle.transform.GetChild(0).GetChild((int)SaveSerial.instance.settingsData.inputType).gameObject.SetActive(true);
        }
    }
    private void Update(){
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
    if(SaveSerial.instance!=null){
        if(SaveSerial.instance.settingsData.pprocessing==true&&postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
        if(SaveSerial.instance.settingsData.pprocessing==true&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume.enabled=true;}
        if(SaveSerial.instance.settingsData.pprocessing==false&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
        if(SaveSerial.instance.settingsData.masterVolume<=-40){SaveSerial.instance.settingsData.masterVolume=-80;}
        if(SaveSerial.instance.settingsData.soundVolume<=-40){SaveSerial.instance.settingsData.soundVolume=-80;}
        if(SaveSerial.instance.settingsData.musicVolume<=-40){SaveSerial.instance.settingsData.musicVolume=-80;}
    }
    }
    public void SetPanelActive(int i){foreach(GameObject p in panels){p.SetActive(false);}panels[i].SetActive(true);panelActive=i;}
    public void OpenSettings(){transform.GetChild(0).gameObject.SetActive(true);transform.GetChild(1).gameObject.SetActive(false);}
    public void OpenDeleteAll(){transform.GetChild(1).gameObject.SetActive(true);transform.GetChild(0).gameObject.SetActive(false);}
    public void Close(){transform.GetChild(0).gameObject.SetActive(false);transform.GetChild(1).gameObject.SetActive(false);}
    public void SetMasterVolume(float volume){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.masterVolume=volume;
    }public void SetSoundVolume(float volume){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.soundVolume=volume;
    }
    public void SetMusicVolume(float volume){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.musicVolume=volume;
    }
    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        if(SaveSerial.instance!=null){
            SaveSerial.instance.settingsData.quality=qualityIndex;
            if(qualityIndex<=1){
                SaveSerial.instance.settingsData.pprocessing=false;
                SaveSerial.instance.settingsData.dmgPopups=false;
                pprocessingToggle.GetComponent<Toggle>().isOn=false;
                dmgPopupsToggle.GetComponent<Toggle>().isOn=false;
            }if(qualityIndex==0){
                SaveSerial.instance.settingsData.screenshake=false;
                SaveSerial.instance.settingsData.particles=false;
                SaveSerial.instance.settingsData.screenflash=false;
                screenshakeToggle.GetComponent<Toggle>().isOn=false;
                particlesToggle.GetComponent<Toggle>().isOn=false;
                screenflashToggle.GetComponent<Toggle>().isOn=false;
            }if(qualityIndex>1){
                SaveSerial.instance.settingsData.particles=true;
                particlesToggle.GetComponent<Toggle>().isOn=true;
            }if(qualityIndex>4){
                SaveSerial.instance.settingsData.pprocessing=true;
                pprocessingToggle.GetComponent<Toggle>().isOn=true;
            }
        }
    }
    public void SetScreenshake(bool isOn){if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.screenshake=isOn;}
    public void SetDmgPopups(bool isOn){if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.dmgPopups=isOn;}
    public void SetParticles(bool isOn){if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.particles=isOn;}
    public void SetScreenflash(bool isOn){if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.screenflash=isOn;}
    public void SetFullscreen(bool isOn){
        Screen.fullScreen=isOn;
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.fullscreen=isOn;
        Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,isOn,60);
    }
    public void SetPostProcessing(bool isOn){
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance!=null)if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.pprocessing=isOn;
        if(isOn==true&&postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}//FindObjectOfType<Level>().RestartScene();}
        if(isOn==true&&postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(isOn==false&&FindObjectOfType<PostProcessVolume>()!=null){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }
    public void SetOnScreenButtons(bool isOn){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.scbuttons=isOn;
    }public void SetVibrations(bool isOn){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.vibrations=isOn;
    }public void SetPlayfieldRot(bool horiz){
        PlaneDir h=PlaneDir.vert;if(horiz==true){h=PlaneDir.horiz;}if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.playfieldRot=h;
        if(SceneManager.GetActiveScene().name=="Game"&&Application.isPlaying){
            if(h==PlaneDir.horiz){FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,90);FindObjectOfType<Camera>().orthographicSize=GameSession.instance.horizCameraSize;}
            else{FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,0);FindObjectOfType<Camera>().orthographicSize=GameSession.instance.vertCameraSize;}
        }
    }
    public void SetSteering(){
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
                if(GameSession.instance.cheatmode)s.settingsData.inputType=(InputType)3;
                else s.settingsData.inputType=(InputType)0;
                break;
            case (InputType)3:
                s.settingsData.inputType=(InputType)0;
                break;
        }
        foreach(Transform t in steeringToggle.transform.GetChild(0)){t.gameObject.SetActive(false);}
        steeringToggle.transform.GetChild(0).GetChild((int)s.settingsData.inputType).gameObject.SetActive(true);
        }
    }
    public void SetJoystick(){
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
    }
    public void SetJoystickSize(float size){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.joystickSize=size;
    }public void SetLefthand(bool isOn){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.lefthand=isOn;
        if(FindObjectOfType<SwitchPlacesCanvas>()!=null)FindObjectOfType<SwitchPlacesCanvas>().Set();
    }public void SetDTapMouse(bool isOn){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.dtapMouseShoot=isOn;
    }public void SetCheatmode(bool isOn){
        if(GameSession.instance!=null)GameSession.instance.cheatmode=isOn;
    }
    public void SetHudVis_Graphics(float amnt){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.hudVis_graphics=amnt;
    }public void SetHudVis_Text(float amnt){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.hudVis_text=amnt;
    }public void SetHudVis_BarFill(float amnt){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.hudVis_barFill=amnt;
    }public void SetHudVis_AbsorpFill(float amnt){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.hudVis_absorpFill=amnt;
    }public void SetHudVis_Popups(float amnt){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.hudVis_popups=amnt;
    }public void SetHudVis_Notif(float amnt){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.hudVis_notif=amnt;
    }

    public void PlayDing(){if(Application.isPlaying)GetComponent<AudioSource>().Play();}
}
