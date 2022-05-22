using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class SettingsMenu : MonoBehaviour{      public static SettingsMenu instance;
    [SerializeField] int panelActive=0;
    [SerializeField] GameObject[] panels;
    [Header("Game")]
    [SceneObjectsOnly][SerializeField]GameObject steeringToggle;
    [SceneObjectsOnly][SerializeField]GameObject vibrationsToggle;
    [SceneObjectsOnly][SerializeField]GameObject horizPlayfieldToggle;
    [SceneObjectsOnly][SerializeField]GameObject dtapMouseShootToggle;
    [SceneObjectsOnly][SerializeField]GameObject lefthandToggle;
    [SceneObjectsOnly][SerializeField]GameObject scbuttonsToggle;
    [SceneObjectsOnly][SerializeField]GameObject discordRPCToggle;
    [SceneObjectsOnly][SerializeField]GameObject autoselectNewItemToggle;
    [SceneObjectsOnly][SerializeField]GameObject alwaysReplaceCurrentSlotToggle;
    [SceneObjectsOnly][SerializeField]GameObject autoUseMedkitsIfLowToggle;
    [SceneObjectsOnly][SerializeField]GameObject allowSelectingEmptySlotsToggle;
    [SceneObjectsOnly][SerializeField]GameObject allowScrollingEmptySlotsToggle;
    [SceneObjectsOnly][SerializeField]GameObject cheatToggle;
    [Header("Sound")]
    public AudioMixer audioMixer;
    [SceneObjectsOnly][SerializeField]GameObject masterSlider;
    [SceneObjectsOnly][SerializeField]GameObject soundSlider;
    [SceneObjectsOnly][SerializeField]GameObject ambienceSlider;
    [SceneObjectsOnly][SerializeField]GameObject musicSlider;
    [SceneObjectsOnly][SerializeField]GameObject musicWinddownToggle;
    
    [Header("Graphics")]
    [SceneObjectsOnly][SerializeField]GameObject qualityDropdopwn;
    [SceneObjectsOnly][SerializeField]GameObject fullscreenToggle;
    [SceneObjectsOnly][SerializeField]GameObject pprocessingToggle;
    [SceneObjectsOnly][SerializeField]GameObject screenshakeToggle;
    [SceneObjectsOnly][SerializeField]GameObject dmgPopupsToggle;
    [SceneObjectsOnly][SerializeField]GameObject particlesToggle;
    [SceneObjectsOnly][SerializeField]GameObject screenflashToggle;
    [SceneObjectsOnly][SerializeField]GameObject playerWeaponsFadeToggle;

    [SceneObjectsOnly][SerializeField]GameObject hudVis_graphicsSlider;
    [SceneObjectsOnly][SerializeField]GameObject hudVis_textSlider;
    [SceneObjectsOnly][SerializeField]GameObject hudVis_barsSlider;
    [SceneObjectsOnly][SerializeField]GameObject hudVis_absorpSlider;
    [SceneObjectsOnly][SerializeField]GameObject hudVis_popupsSlider;
    [SceneObjectsOnly][SerializeField]GameObject hudVis_notifsSlider;

    [AssetsOnly][SerializeField]GameObject pprocessingPrefab;
    [SceneObjectsOnly]public PostProcessVolume postProcessVolume;
    SaveSerial.SettingsData settingsData;
    void Start(){
        instance=this;if(SaveSerial.instance!=null)settingsData=SaveSerial.instance.settingsData;

        scbuttonsToggle.GetComponent<Toggle>().isOn=settingsData.scbuttons;
        dtapMouseShootToggle.GetComponent<Toggle>().isOn=settingsData.dtapMouseShoot;
        lefthandToggle.GetComponent<Toggle>().isOn=settingsData.lefthand;
        vibrationsToggle.GetComponent<Toggle>().isOn=settingsData.vibrations;
        discordRPCToggle.GetComponent<Toggle>().isOn=settingsData.discordRPC;
        autoselectNewItemToggle.GetComponent<Toggle>().isOn=settingsData.autoselectNewItem;
        alwaysReplaceCurrentSlotToggle.GetComponent<Toggle>().isOn=settingsData.alwaysReplaceCurrentSlot;
        autoUseMedkitsIfLowToggle.GetComponent<Toggle>().isOn=settingsData.autoUseMedkitsIfLow;
        allowSelectingEmptySlotsToggle.GetComponent<Toggle>().isOn=settingsData.allowSelectingEmptySlots;
        allowScrollingEmptySlotsToggle.GetComponent<Toggle>().isOn=settingsData.allowScrollingEmptySlots;
        cheatToggle.GetComponent<Toggle>().isOn=GameSession.instance.cheatmode;
        bool h=false;if(settingsData.playfieldRot==PlaneDir.horiz){h=true;}horizPlayfieldToggle.GetComponent<Toggle>().isOn=h;

        foreach(Transform t in steeringToggle.transform.GetChild(0)){t.gameObject.SetActive(false);}
        steeringToggle.transform.GetChild(0).GetChild((int)settingsData.inputType).gameObject.SetActive(true);


        masterSlider.GetComponent<Slider>().value=settingsData.masterVolume;
        soundSlider.GetComponent<Slider>().value=settingsData.soundVolume;
        ambienceSlider.GetComponent<Slider>().value=settingsData.ambienceVolume;
        musicSlider.GetComponent<Slider>().value=settingsData.musicVolume;
        musicWinddownToggle.GetComponent<Toggle>().isOn=settingsData.windDownMusic;


        qualityDropdopwn.GetComponent<Dropdown>().value=settingsData.quality;
        fullscreenToggle.GetComponent<Toggle>().isOn=settingsData.fullscreen;
        pprocessingToggle.GetComponent<Toggle>().isOn=settingsData.pprocessing;
        screenshakeToggle.GetComponent<Toggle>().isOn=settingsData.screenshake;
        dmgPopupsToggle.GetComponent<Toggle>().isOn=settingsData.dmgPopups;
        particlesToggle.GetComponent<Toggle>().isOn=settingsData.particles;
        screenflashToggle.GetComponent<Toggle>().isOn=settingsData.screenflash;
        playerWeaponsFadeToggle.GetComponent<Toggle>().isOn=settingsData.playerWeaponsFade;

        hudVis_graphicsSlider.GetComponent<Slider>().value=settingsData.hudVis_graphics;
        hudVis_textSlider.GetComponent<Slider>().value=settingsData.hudVis_text;
        hudVis_barsSlider.GetComponent<Slider>().value=settingsData.hudVis_barFill;
        hudVis_absorpSlider.GetComponent<Slider>().value=settingsData.hudVis_absorpFill;
        hudVis_popupsSlider.GetComponent<Slider>().value=settingsData.hudVis_popups;
        hudVis_notifsSlider.GetComponent<Slider>().value=settingsData.hudVis_notif;
        if(SceneManager.GetActiveScene().name=="Options")OpenSettings();
        SetPanelActive(0);
    }
    void Update(){postProcessVolume=FindObjectOfType<PostProcessVolume>();if(SaveSerial.instance!=null){
        if(settingsData.pprocessing==true&&postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
        if(settingsData.pprocessing==true&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume.enabled=true;}
        if(settingsData.pprocessing==false&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
        if(settingsData.masterVolume<=-50){settingsData.masterVolume=-80;}
        if(settingsData.soundVolume<=-50){settingsData.soundVolume=-80;}
        if(settingsData.ambienceVolume<=-50){settingsData.ambienceVolume=-80;}
        if(settingsData.musicVolume<=-50){settingsData.musicVolume=-80;}
    }}
    public void SetPanelActive(int i){panelActive=i;foreach(GameObject p in panels){p.SetActive(false);}panels[panelActive].SetActive(true);}
    public void OpenSettings(){transform.GetChild(0).gameObject.SetActive(true);transform.GetChild(1).gameObject.SetActive(false);}
    public void OpenDeleteAll(){transform.GetChild(1).gameObject.SetActive(true);transform.GetChild(0).gameObject.SetActive(false);}
    public void Close(){transform.GetChild(0).gameObject.SetActive(false);transform.GetChild(1).gameObject.SetActive(false);}


    #region//Game
    public void SetOnScreenButtons(bool isOn){if(SaveSerial.instance!=null)settingsData.scbuttons=isOn;}
    public void SetVibrations(bool isOn){if(SaveSerial.instance!=null)settingsData.vibrations=isOn;}
    public void SetDiscordRPC(bool isOn){if(SaveSerial.instance!=null)settingsData.discordRPC=isOn;}
    public void SetAutoselectNewItem(bool isOn){if(SaveSerial.instance!=null)settingsData.autoselectNewItem=isOn;
        if(isOn){
            if(settingsData.alwaysReplaceCurrentSlot){settingsData.alwaysReplaceCurrentSlot=false;alwaysReplaceCurrentSlotToggle.GetComponent<Toggle>().isOn=false;}
        }
    }
    public void SetAlwaysReplaceCurrentSlot(bool isOn){if(SaveSerial.instance!=null)settingsData.alwaysReplaceCurrentSlot=isOn;
        if(isOn){
            if(settingsData.autoselectNewItem){settingsData.autoselectNewItem=false;autoselectNewItemToggle.GetComponent<Toggle>().isOn=false;}
        }
    }   
    public void SetAutouseMedkitsIfLow(bool isOn){if(SaveSerial.instance!=null)settingsData.autoUseMedkitsIfLow=isOn;}
    public void SetAllowSelectingEmptySlots(bool isOn){if(SaveSerial.instance!=null)settingsData.allowSelectingEmptySlots=isOn;
        if(isOn){
            if(settingsData.allowScrollingEmptySlots&&settingsData.alwaysReplaceCurrentSlot){settingsData.alwaysReplaceCurrentSlot=false;alwaysReplaceCurrentSlotToggle.GetComponent<Toggle>().isOn=false;}
        }
    }
    public void SetAllowScrollingEmptySlots(bool isOn){if(SaveSerial.instance!=null)settingsData.allowScrollingEmptySlots=isOn;
        if(isOn){
            if(settingsData.allowSelectingEmptySlots&&settingsData.alwaysReplaceCurrentSlot){settingsData.alwaysReplaceCurrentSlot=false;alwaysReplaceCurrentSlotToggle.GetComponent<Toggle>().isOn=false;}
        }
    }
    public void SetPlayfieldRot(bool horiz){
        PlaneDir h=PlaneDir.vert;if(horiz==true){h=PlaneDir.horiz;}if(SaveSerial.instance!=null)settingsData.playfieldRot=h;
        if(SceneManager.GetActiveScene().name=="Game"&&Application.isPlaying){
            if(h==PlaneDir.horiz){FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,90);FindObjectOfType<Camera>().orthographicSize=GameSession.instance.horizCameraSize;}
            else{FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,0);FindObjectOfType<Camera>().orthographicSize=GameSession.instance.vertCameraSize;}
        }
    }
    public void SetSteering(){  if(SaveSerial.instance!=null){
        var s=settingsData;
        switch(settingsData.inputType){
            case (InputType)0:
                settingsData.inputType=(InputType)1;
                break;
            case (InputType)1:
                settingsData.inputType=(InputType)2;
                break;
            case (InputType)2:
                if(GameSession.instance.cheatmode)settingsData.inputType=(InputType)3;
                else settingsData.inputType=(InputType)0;
                break;
            case (InputType)3:
                settingsData.inputType=(InputType)0;
                break;
        }
    }}
    public void SetJoystick(){
        if(SaveSerial.instance!=null){
        var s=settingsData;
        switch(settingsData.joystickType){
            case (JoystickType)0:
                settingsData.joystickType=(JoystickType)1;
                break;
            case (JoystickType)1:
                settingsData.joystickType=(JoystickType)2;
                break;
            case (JoystickType)2:
                settingsData.joystickType=(JoystickType)0;
                break;
        }
        if(FindObjectOfType<Tag_Joystick>()!=null)FindObjectOfType<Tag_Joystick>().StartCoroutine(FindObjectOfType<Tag_Joystick>().ChangeType());
        }
    }
    public void SetJoystickSize(float size){if(SaveSerial.instance!=null)settingsData.joystickSize=size;}
    public void SetLefthand(bool isOn){
        if(SaveSerial.instance!=null)settingsData.lefthand=isOn;
        if(FindObjectOfType<SwitchPlacesCanvas>()!=null)FindObjectOfType<SwitchPlacesCanvas>().Set();
    }
    public void SetDTapMouse(bool isOn){if(SaveSerial.instance!=null)settingsData.dtapMouseShoot=isOn;}
    public void SetCheatmode(bool isOn){if(GameSession.instance!=null)GameSession.instance.cheatmode=isOn;}
    #endregion


    #region//Sound
    public void SetMasterVolume(float val){if(SaveSerial.instance!=null)System.Math.Round(settingsData.masterVolume=val,2);}
    public void SetSoundVolume(float val){if(SaveSerial.instance!=null)System.Math.Round(settingsData.soundVolume=val,2);}
    public void SetAmbienceVolume(float val){if(SaveSerial.instance!=null)System.Math.Round(settingsData.ambienceVolume=val,2);}
    public void SetMusicVolume(float val){if(SaveSerial.instance!=null)System.Math.Round(settingsData.musicVolume=val,2);}
    public void SetMusicWinddown(bool isOn){if(SaveSerial.instance!=null)settingsData.windDownMusic=isOn;}
    #endregion


    #region//Graphics
    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        if(SaveSerial.instance!=null){
            var s=settingsData;
            settingsData.quality=qualityIndex;
            if(qualityIndex<=1){
                settingsData.pprocessing=false;
                settingsData.dmgPopups=false;
                pprocessingToggle.GetComponent<Toggle>().isOn=false;
                dmgPopupsToggle.GetComponent<Toggle>().isOn=false;
            }if(qualityIndex==0){
                settingsData.screenshake=false;
                settingsData.particles=false;
                settingsData.screenflash=false;
                screenshakeToggle.GetComponent<Toggle>().isOn=false;
                particlesToggle.GetComponent<Toggle>().isOn=false;
                screenflashToggle.GetComponent<Toggle>().isOn=false;
            }if(qualityIndex>1){
                settingsData.particles=true;
                particlesToggle.GetComponent<Toggle>().isOn=true;
            }if(qualityIndex>4){
                settingsData.pprocessing=true;
                pprocessingToggle.GetComponent<Toggle>().isOn=true;
            }
        }
    }
    public void SetScreenshake(bool isOn){if(SaveSerial.instance!=null)settingsData.screenshake=isOn;}
    public void SetDmgPopups(bool isOn){if(SaveSerial.instance!=null)settingsData.dmgPopups=isOn;}
    public void SetParticles(bool isOn){if(SaveSerial.instance!=null)settingsData.particles=isOn;}
    public void SetScreenflash(bool isOn){if(SaveSerial.instance!=null)settingsData.screenflash=isOn;}
    public void SetPlayerWeaponsFade(bool isOn){if(SaveSerial.instance!=null)settingsData.playerWeaponsFade=isOn;}
    public void SetFullscreen(bool isOn){
        Screen.fullScreen=isOn;
        if(SaveSerial.instance!=null)settingsData.fullscreen=isOn;
        Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,isOn,60);
    }
    public void SetPostProcessing(bool isOn){
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance!=null)if(SaveSerial.instance!=null)settingsData.pprocessing=isOn;
        if(isOn==true&&postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}//FindObjectOfType<Level>().RestartScene();}
        if(isOn==true&&postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(isOn==false&&FindObjectOfType<PostProcessVolume>()!=null){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }

    public void SetHudVis_Graphics(float val){if(SaveSerial.instance!=null)settingsData.hudVis_graphics=val;}
    public void SetHudVis_Text(float val){if(SaveSerial.instance!=null)settingsData.hudVis_text=val;}
    public void SetHudVis_BarFill(float val){if(SaveSerial.instance!=null)settingsData.hudVis_barFill=val;}
    public void SetHudVis_AbsorpFill(float val){if(SaveSerial.instance!=null)settingsData.hudVis_absorpFill=val;}
    public void SetHudVis_Popups(float val){if(SaveSerial.instance!=null)settingsData.hudVis_popups=val;}
    public void SetHudVis_Notif(float val){if(SaveSerial.instance!=null)settingsData.hudVis_notif=val;}
    #endregion
    

    public void PlayDing(){if(Application.isPlaying)GetComponent<AudioSource>().Play();}
}
