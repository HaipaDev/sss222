using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Loader : MonoBehaviour{
    public float timer=1f;
    public AudioMixer audioMixer;
    private void Awake() {
        //if(Application.platform == RuntimePlatform.Android)timer=1.2f;
    }
    private void Load()
    {
        //FindObjectOfType<GameSession>().savableData.Load();
        FindObjectOfType<SaveSerial>().Load();
        FindObjectOfType<SaveSerial>().LoadSettings();
        if (Application.platform != RuntimePlatform.Android){Screen.fullScreen = FindObjectOfType<SaveSerial>().fullscreen;
        QualitySettings.SetQualityLevel(FindObjectOfType<SaveSerial>().quality);}
        if (Application.platform == RuntimePlatform.Android)FindObjectOfType<SaveSerial>().moveByMouse=false;
        audioMixer.SetFloat("MasterVolume", FindObjectOfType<SaveSerial>().masterVolume);
        audioMixer.SetFloat("SoundVolume", FindObjectOfType<SaveSerial>().soundVolume);
        audioMixer.SetFloat("MusicVolume", FindObjectOfType<SaveSerial>().musicVolume);

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
    // Update is called once per frame
    void Update()
    {
        Load();
        timer -= Time.deltaTime;
        if(timer<=0){ if(SceneManager.GetActiveScene().name=="Loading") { SceneManager.LoadScene("Menu"); } Destroy(gameObject); }
    }
}
