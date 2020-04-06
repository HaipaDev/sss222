using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour{
    //Settings settings;
    GameSession gameSession;
    SaveSerial saveSerial;
    public int quality;
    public bool fullscreen;
    public bool moveByMouse;
    public float masterVolume;
    public float soundVolume;
    public float musicVolume;
    [SerializeField]GameObject qualityDropdopwn;
    [SerializeField]GameObject fullscreenToggle;
    [SerializeField]GameObject steeringToggle;
    [SerializeField]GameObject masterSlider;
    [SerializeField]GameObject soundSlider;
    [SerializeField]GameObject musicSlider;
    [SerializeField]AudioSource audioSource;
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
        steeringToggle.GetComponent<Toggle>().isOn = saveSerial.moveByMouse;

        masterSlider.GetComponent<Slider>().value = saveSerial.masterVolume;
        soundSlider.GetComponent<Slider>().value = saveSerial.soundVolume;
        musicSlider.GetComponent<Slider>().value = saveSerial.musicVolume;
    }
    public AudioMixer audioMixer;
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
    }
    public void SetSteering(bool isMovingByMouse)
    {
        moveByMouse = isMovingByMouse;
        //saveSerial.moveByMouse = isMovingByMouse;
    }
    public void PlayDing(){
        audioSource.Play();
    }
}
