using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class SoundManager{
    public enum Sound{
        powerupSFX
    }
    /*public AudioMixer Mixers(AudioMixer mixer){
        public AudioMixer mixer = Resources.Load("MasterMixer") as AudioMixer;
        public string OutputMixer = "SoundMixer";
        AudioMixer outputAudioMixerGroup = mixer.FindMatchingGroups(OutputMixer)[0];
        return outputAudioMixerGroup;
    }*/
    public static void PlaySound(Sound sound){
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = (Resources.Load("MasterMixer") as AudioMixer).FindMatchingGroups("SoundMixer")[0]; ;
        audioSource.PlayOneShot(GetAudioClip(sound));
    }
    private static AudioClip GetAudioClip(Sound sound){
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArr){
            if(soundAudioClip.sound==sound){
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound" + sound + " not found!");
        return null;
    }
}
