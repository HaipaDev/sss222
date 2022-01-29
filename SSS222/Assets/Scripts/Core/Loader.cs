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
        if(Application.platform==RuntimePlatform.Android){SaveSerial.instance.settingsData.inputType=/*InputType.touch;*/InputType.mouse;SaveSerial.instance.settingsData.dtapMouseShoot=true;
        SaveSerial.instance.settingsData.pprocessing=false;SaveSerial.instance.settingsData.scbuttons=true;}
        else{SaveSerial.instance.settingsData.inputType=InputType.mouse;SaveSerial.instance.settingsData.dtapMouseShoot=false;
        SaveSerial.instance.settingsData.pprocessing=true;SaveSerial.instance.settingsData.scbuttons=false;}
        if(!loaded){
            SaveSerial.instance.Load();
            SaveSerial.instance.LoadLogin();
            SaveSerial.instance.LoadSettings();
            SaveSerial.instance.LoadStats();
            loaded=true;
        }
        if(Application.platform!=RuntimePlatform.Android){Screen.fullScreen=SaveSerial.instance.settingsData.fullscreen;if(SaveSerial.instance.settingsData.fullscreen)Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,true,60);
        QualitySettings.SetQualityLevel(SaveSerial.instance.settingsData.quality);}
        //if (Application.platform == RuntimePlatform.Android)SaveSerial.instance.settingsData.moveByMouse=false;
        audioMixer.SetFloat("MasterVolume", SaveSerial.instance.settingsData.masterVolume);
        audioMixer.SetFloat("SoundVolume", SaveSerial.instance.settingsData.soundVolume);
        audioMixer.SetFloat("MusicVolume", SaveSerial.instance.settingsData.musicVolume);
    }
    public void ForceLoad(){
        if(loaded)GSceneManager.instance.LoadStartMenuLoader();
    }
    void Update(){
        Load();
        if(forceLoad){ForceLoad();}
    }
}
