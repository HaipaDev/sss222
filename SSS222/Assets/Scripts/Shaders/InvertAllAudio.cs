using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InvertAllAudio : MonoBehaviour{
    public bool revertMusic;
    public MusicPlayer musicPlayer;
    void Start(){
        musicPlayer=FindObjectOfType<MusicPlayer>();
    }

    void Update(){
        //if(invert){
            AudioSource[] sounds=FindObjectsOfType<AudioSource>();
            foreach(AudioSource sound in sounds){
                //if(sound!=musicPlayer){
                    if(sound.gameObject.name!="MusicPlayer"){
                    var tempAudioTime=sound.GetComponent<AudioSource>().clip.length-0.025f;
                    sound.GetComponent<AudioSource>().pitch=-1;
                    sound.GetComponent<AudioSource>().time=tempAudioTime;
                //}else{
                    if(revertMusic!=true){
                        musicPlayer=FindObjectOfType<MusicPlayer>();
                        if(musicPlayer.GetComponent<AudioSource>().pitch!=-1)musicPlayer.GetComponent<AudioSource>().pitch=-1;}
                    //else{musicPlayer=FindObjectOfType<MusicPlayer>();musicPlayer.GetComponent<AudioSource>().pitch=1;}}
                if(revertMusic==true){musicPlayer=FindObjectOfType<MusicPlayer>();musicPlayer.GetComponent<AudioSource>().pitch=1;this.enabled=false;}
            }
        }
        //else{musicPlayer.GetComponent<AudioSource>().pitch=1;}
        if(FindObjectOfType<Player>().inverted!=true){this.enabled=false;}
    }
}
