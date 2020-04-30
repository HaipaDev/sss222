using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InvertAllAudio : MonoBehaviour{
    void Start(){
        
    }

    void Update(){
        AudioSource[] sounds=FindObjectsOfType<AudioSource>();
        foreach(AudioSource sound in sounds){
            var tempAudioTime=sound.GetComponent<AudioSource>().clip.length-0.025f;
            sound.GetComponent<AudioSource>().pitch=-1;
            sound.GetComponent<AudioSource>().time=tempAudioTime;
            
        }
    }
}
