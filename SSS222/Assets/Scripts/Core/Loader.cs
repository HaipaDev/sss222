using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Loader : MonoBehaviour{
    public float timer=1f;
    public AudioMixer audioMixer;
    private void Load()
    {
        //FindObjectOfType<GameSession>().savableData.Load();
        FindObjectOfType<SaveSerial>().Load();
        FindObjectOfType<SaveSerial>().LoadSettings();
        Screen.fullScreen = FindObjectOfType<SaveSerial>().fullscreen;
        QualitySettings.SetQualityLevel(FindObjectOfType<SaveSerial>().quality);
        audioMixer.SetFloat("MasterVolume", FindObjectOfType<SaveSerial>().masterVolume);
        audioMixer.SetFloat("SoundVolume", FindObjectOfType<SaveSerial>().soundVolume);
        audioMixer.SetFloat("MusicVolume", FindObjectOfType<SaveSerial>().musicVolume);

    }
    // Update is called once per frame
    void Update()
    {
        Load();
        timer -= Time.deltaTime;
        if(timer<=0){ if(SceneManager.GetActiveScene().name=="Loading") { SceneManager.LoadScene("Menu"); } Destroy(gameObject); }
    }
}
