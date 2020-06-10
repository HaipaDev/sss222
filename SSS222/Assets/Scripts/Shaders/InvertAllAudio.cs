using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InvertAllAudio : MonoBehaviour{
    public bool revertMusic;
    public MusicPlayer musicPlayer;
    float offTimer;
    void Start(){
        musicPlayer=FindObjectOfType<MusicPlayer>();
    }

    void Update(){
        //if(invert){
            foreach(AudioSource sound in FindObjectsOfType<AudioSource>()){
            if(sound!=null){
                GameObject snd=sound.gameObject;
                //if(sound!=musicPlayer){
                if(snd.GetComponent<MusicPlayer>()==null){//If not MusicPlayer
                    //if(sound.GetComponent<AudioSource>()!=null){
                    //var tempAudioTime=snd.GetComponent<AudioSource>().clip.length-0.025f;
                    if(revertMusic!=true){snd.GetComponent<AudioSource>().loop=true;snd.GetComponent<AudioSource>().pitch=-1;}
                    else{snd.GetComponent<AudioSource>().loop=false;}
                    //snd.GetComponent<AudioSource>().time=tempAudioTime;
                    //}
                }else{
                    if(revertMusic!=true){
                        musicPlayer=FindObjectOfType<MusicPlayer>();
                        if(musicPlayer.GetComponent<AudioSource>().pitch!=-1)musicPlayer.GetComponent<AudioSource>().pitch=-1;}
                    //else{musicPlayer=FindObjectOfType<MusicPlayer>();musicPlayer.GetComponent<AudioSource>().pitch=1;}}
                if(revertMusic==true){if(sound!=musicPlayer){sound.loop=false;sound.Stop();}if(FindObjectOfType<MusicPlayer>()!=null){musicPlayer=FindObjectOfType<MusicPlayer>();musicPlayer.GetComponent<AudioSource>().pitch=1;offTimer=1f;}}
                }
            }
        }
        //else{musicPlayer.GetComponent<AudioSource>().pitch=1;}
        if(FindObjectOfType<Player>()!=null){
            if(FindObjectOfType<Player>().inverter!=true){revertMusic=true;}//this.enabled=false;}
        }else{
            revertMusic=true;
            offTimer=0;
            GetComponent<SpriteRenderer>().enabled=false;
            this.enabled=false;
        }
        if(offTimer>0)offTimer-=Time.deltaTime;
        if(offTimer<=0&&revertMusic==true)this.enabled=false;
    }
}
