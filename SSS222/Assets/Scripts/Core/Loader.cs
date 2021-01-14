using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Loader : MonoBehaviour{
    public float timer=1f;
    public AudioMixer audioMixer;
    bool loaded;
    private void Load(){
        //FindObjectOfType<GameSession>().savableData.Load();
        if(Application.platform == RuntimePlatform.Android){SaveSerial.instance.settingsData.pprocessing=false;SaveSerial.instance.settingsData.scbuttons=true;}
        else{SaveSerial.instance.settingsData.pprocessing=true;SaveSerial.instance.settingsData.scbuttons=false;}
        if(!loaded){
        SaveSerial.instance.Load();
        SaveSerial.instance.LoadSettings();
        loaded=true;
        }
        if (Application.platform != RuntimePlatform.Android){Screen.fullScreen = SaveSerial.instance.settingsData.fullscreen;if(SaveSerial.instance.settingsData.fullscreen)Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,true,60);
        QualitySettings.SetQualityLevel(SaveSerial.instance.settingsData.quality);}
        //if (Application.platform == RuntimePlatform.Android)SaveSerial.instance.settingsData.moveByMouse=false;
        audioMixer.SetFloat("MasterVolume", SaveSerial.instance.settingsData.masterVolume);
        audioMixer.SetFloat("SoundVolume", SaveSerial.instance.settingsData.soundVolume);
        audioMixer.SetFloat("MusicVolume", SaveSerial.instance.settingsData.musicVolume);

        #if UNITY_ANDROID
            Application.runInBackground = true;
            //Application.targetFrameRate = 30;
            //QualitySettings.SetQualityLevel(2);
            //QualitySettings.vSyncCount = 0; 
            
            //if (QualitySettings.GetQualityLevel() <= 1){
                //QualitySettings.SetQualityLevel(3);
                //QualitySettings.shadowCascades = 0;
                //QualitySettings.shadowDistance = 15;
            //}
            
            /*else if (QualitySettings.GetQualityLevel() == 5){
                QualitySettings.shadowCascades = 2;
                QualitySettings.shadowDistance = 70;
            }*/
            
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
        #endif

        #if UNITY_STANDALONE_WIN
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 1;
            //QualitySettings.SetQualityLevel(4);
        #endif
    }
    void Update()
    {
        Load();
        timer -= Time.deltaTime;
        if(timer<=0){ if(SceneManager.GetActiveScene().name=="Loading") { SceneManager.LoadScene("Menu"); } Destroy(gameObject); }
    }
}
