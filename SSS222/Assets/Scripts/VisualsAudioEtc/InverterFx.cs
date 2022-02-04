using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class InverterFx : MonoBehaviour{
    public static InverterFx instance;
    [SerializeField] public bool on;
    [SerializeField] public bool invertSprite=true;
    [SerializeField] public bool invertSounds=true;
    [SerializeField] public bool invertMusic=true;
    [HideIf("@this.invertParticlesInGame==true")][SerializeField] public bool invertSpritesInGame=true;
    [DisableIf("@this.invertSpritesInGame==false")][SerializeField] public bool invertParticlesInGame=true;

    [HideInInspector] public bool reverted=true;
    [HideInInspector] List<AudioSource> loopedSounds;
    //float offTimer;
    [HideInInspector] public SpriteRenderer sprRend;
    void Start(){
        if(instance!=null){Destroy(gameObject);}else{instance=this;}
        sprRend=GetComponent<SpriteRenderer>();
        loopedSounds=new List<AudioSource>();
    }

    void Update(){
        if(invertSpritesInGame){sprRend.sortingOrder=-1;}
        else{sprRend.sortingOrder=0;}
        if(invertParticlesInGame){sprRend.sortingOrder=-2;}

        if(on){if(invertSprite)sprRend.enabled=true;}
        if(invertSounds||invertMusic){
            foreach(AudioSource snd in FindObjectsOfType<AudioSource>()){if(snd!=null){
                GameObject sndGo=snd.gameObject;
                if(on){
                    if(invertSounds&&sndGo!=Jukebox.instance){//If not Jukebox
                        if(snd.loop){loopedSounds.Add(snd);}
                        SetSoundReverse(snd,snd.loop);
                    }else if(invertMusic&&sndGo==Jukebox.instance){
                        if(Jukebox.instance.GetComponent<AudioSource>().pitch!=-1)Jukebox.instance.GetComponent<AudioSource>().pitch=-1;
                    }
                }else{
                    if(!reverted){
                        if(invertSprite)sprRend.enabled=false;
                        if(invertSounds){
                            if(!loopedSounds.Contains(snd)){snd.loop=false;}//snd.Stop();}
                            if(loopedSounds.Count>0){for(int i=0;i<loopedSounds.Count;i++){
                                if(loopedSounds[i]!=null){loopedSounds[i].pitch=1;loopedSounds[i].loop=true;}
                                loopedSounds.Remove(loopedSounds[i]);
                            }}
                        }
                        if(invertMusic)if(Jukebox.instance!=null){Jukebox.instance.GetComponent<AudioSource>().pitch=1;}//offTimer=1f;}
                        reverted=true;
                    }
                }
            }
        }}
        if(Player.instance!=null){
            //if(Player.instance.inverter!=true){reverted=true;on=false;}
        }else{
            on=false;
            //offTimer=0;
        }
        //if(offTimer>0)offTimer-=Time.deltaTime;
        //if(offTimer<=0&&reverted==true)on=false;
    }
    public void SetSoundReverse(AudioSource snd,bool looping){InverterFx.instance.StartCoroutine(InverterFx.instance.SetSoundReverseI(snd,looping));}
    IEnumerator SetSoundReverseI(AudioSource snd,bool looping){if(snd!=null){snd.loop=true;snd.pitch=-1;yield return new WaitForSeconds(0.02f);if(snd!=null)if(!looping)snd.loop=false;}}
    void OnValidate(){
        if(!on){reverted=false;}
        
        if(invertSpritesInGame){GetComponent<SpriteRenderer>().sortingOrder=-1;}
        else{GetComponent<SpriteRenderer>().sortingOrder=0;}
        if(invertParticlesInGame){GetComponent<SpriteRenderer>().sortingOrder=-2;}
    }
}
