using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour{
    //Settings settings;
    GameSession gameSession;
    private void Start(){
        //settings = FindObjectOfType<Settings>();
        gameSession = FindObjectOfType<GameSession>();
    }
    public AudioMixer audioMixer;
    public void SetMasterVolume(float volume){
        audioMixer.SetFloat("MasterVolume", volume);
    }public void SetSoundVolume(float volume){
        audioMixer.SetFloat("SoundVolume", volume);
    }public void SetMusicVolume(float volume){
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullscreen (bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }
    public void SetSteering(bool isMovingByMouse)
    {
        gameSession.moveByMouse = isMovingByMouse;
    }
}
