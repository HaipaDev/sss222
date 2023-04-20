using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Loader : MonoBehaviour{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] bool loaded;
    [SerializeField] bool forceLoad;
    void Load(){
        var ss=SaveSerial.instance.settingsData;
        //DTapToShoot and OnScreenButtons if Touch Supported and No Mouse nor Keyboard or Gamepad detected
        if(Input.touchSupported&&!Input.mousePresent&&Input.GetJoystickNames().Length==0){
            ss.inputType=InputType.mouse;
            ss.dtapMouseShoot=true;
            ss.scbuttons=true;
        }
        //Keyboard Input auto if only Keyboard or Gamepad plugged in
        else if(!Input.touchSupported&&!Input.mousePresent&&(SystemInfo.deviceType==DeviceType.Desktop||Input.GetJoystickNames().Length>0)){
            ss.inputType=InputType.keyboard;
            ss.dtapMouseShoot=false;
            ss.scbuttons=false;
        }
        //If a Mouse is present on a Touch Device
        else if(Input.touchSupported&&Input.mousePresent){
            ss.inputType=InputType.mouse;
            ss.dtapMouseShoot=false;
            ss.scbuttons=true;
        }
        else{
            ss.inputType=InputType.mouse;
            ss.dtapMouseShoot=false;
            ss.scbuttons=false;
        }
        if(Application.platform==RuntimePlatform.Android){ss.pprocessing=false;ss.pauseWhenOOF=false;}
        else{ss.pprocessing=true;}

        if(!loaded){
            SaveSerial.instance.Load();
            SaveSerial.instance.LoadLogin();
            SaveSerial.instance.LoadSettings();
            SaveSerial.instance.LoadStats();
            loaded=true;
        }
        if(Application.platform!=RuntimePlatform.Android){Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,FullScreenMode.FullScreenWindow);//Screen.fullScreen=ss.fullscreen;if(ss.fullscreen)Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,true,60);
        if(Application.platform!=RuntimePlatform.Android){
            Screen.SetResolution(ss.resolution.x,ss.resolution.y,SettingsMenu.GetFullScreenMode(ss.windowMode));
        }
        QualitySettings.SetQualityLevel(ss.quality);}
        //if (Application.platform == RuntimePlatform.Android)ss.moveByMouse=false;
        audioMixer.SetFloat("MasterVolume", ss.masterVolume);
        audioMixer.SetFloat("SoundVolume", ss.soundVolume);
        audioMixer.SetFloat("MusicVolume", ss.musicVolume);
        //Jukebox.instance.SetMusicToCstmzMusic();
    }
    public void ForceLoad(){
        if(GameObject.Find("IntroLong").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("IntroLongStart")){GameObject.Find("IntroLong").GetComponent<Animator>().SetBool("force",true);}
        else{if(loaded)GSceneManager.instance.LoadStartMenuLoader();}
    }
    void Update(){
        Load();
        if(forceLoad){ForceLoad();}
    }
}
