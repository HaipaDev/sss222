using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class Jukebox : MonoBehaviour{   public static Jukebox instance;
    [SerializeField]float pauseSpeed=0.5f;
    [SerializeField]float deadSpeed=0.4f;
    [SerializeField]float windUpDownSpeed=0.05f;
    [SerializeField]float lowerPitchLimit=0.1f;
    [SerializeField]float upperPitchLimit=2f;
    [DisableInEditorMode]public bool inverted=false;
    [SerializeField]AudioClip currentMusic;

    [SerializeField] float fadeDuration=1f;
    bool fadingIn=false;
    bool fadingOut=false;
    float startVolume=0f;
    float endVolume=1f;
    float fadeStartTime;
    [SerializeField] public float crossfadeDuration=1.0f;
    bool isCrossFading;

    AudioSource bgmusicSource;
    AudioSource bossMusicSource;
    void Awake(){
        if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);gameObject.name=gameObject.name.Split('(')[0];}
    }
    IEnumerator Start(){yield return new WaitForSecondsRealtime(0.1f);
        if(bgmusicSource==null){
            bgmusicSource=GetComponent<AudioSource>();
            bgmusicSource.loop=true;
        }

        if(bossMusicSource==null){
            bossMusicSource=gameObject.AddComponent<AudioSource>();
            bossMusicSource.outputAudioMixerGroup=bgmusicSource.outputAudioMixerGroup;
            bossMusicSource.priority=bgmusicSource.priority;
            bossMusicSource.volume=bgmusicSource.volume;
            bossMusicSource.loop=true;
        }

        SetMusicToCstmzMusic();
    }
    void Update(){
        if(currentMusic!=null)if(bgmusicSource.clip!=currentMusic){bgmusicSource.clip=currentMusic;bgmusicSource.Play();}

        if(fadingIn){
            float progress=(Time.time - fadeStartTime) / fadeDuration;
            bgmusicSource.volume=Mathf.Lerp(startVolume, endVolume, progress);

            if(bgmusicSource.volume==endVolume){
                fadingIn=false;
            }
        }

        if(fadingOut){
            float progress=(Time.time - fadeStartTime) / fadeDuration;
            bgmusicSource.volume=Mathf.Lerp(startVolume, 0f, progress);

            if(bgmusicSource.volume==0f){
                fadingOut=false;
                bgmusicSource.Stop();
            }
        }

        //Change pitch on Pause
        if(bgmusicSource!=null&&bossMusicSource!=null){
            if(GameRules.instance!=null&&SceneManager.GetActiveScene().name=="Game"&&SaveSerial.instance.settingsData.windDownMusic){
                float _curMusicSpeed=bgmusicSource.pitch;float _curBossMusicSpeed=bossMusicSource.pitch;
                float _musicSpeed=1f;
                if(GameRules.instance.musicSlowdownOnPause&&GameManager.GlobalTimeIsPausedNotSlowed){_musicSpeed=pauseSpeed;}
                if(GameRules.instance.musicSlowdownOnPaceChange&&!GameManager.GlobalTimeIsPausedNotSlowed){_musicSpeed=1-(GameManager.instance.defaultGameSpeed-GameManager.instance.gameSpeed);}
                if(GameRules.instance.musicSlowdownOnPaceChange&&!GameManager.GlobalTimeIsPausedNotSlowed&&Player.instance!=null){_musicSpeed=1-(GameManager.instance.defaultGameSpeed-GameManager.instance.gameSpeed);}
                if(Player.instance==null){_musicSpeed=deadSpeed;inverted=false;}

                if(GameRules.instance.musicSlowdownOnPause&&!GameManager.GlobalTimeIsPausedNotSlowed&&!GameRules.instance.musicSlowdownOnPaceChange){_musicSpeed=1f;}
                
                int _mult=1;if(inverted){_mult=-1;}else{_mult=1;}
                if(_curMusicSpeed>_musicSpeed*_mult)_curMusicSpeed=Mathf.Clamp(_curMusicSpeed-=windUpDownSpeed,_musicSpeed,upperPitchLimit)*_mult;
                if(_curMusicSpeed<_musicSpeed*_mult)_curMusicSpeed=Mathf.Clamp(_curMusicSpeed+=windUpDownSpeed,lowerPitchLimit,_musicSpeed)*_mult;
                bgmusicSource.pitch=_curMusicSpeed;
                if(_curBossMusicSpeed>_musicSpeed*_mult)_curBossMusicSpeed=Mathf.Clamp(_curBossMusicSpeed-=windUpDownSpeed,_musicSpeed,upperPitchLimit)*_mult;
                if(_curBossMusicSpeed<_musicSpeed*_mult)_curBossMusicSpeed=Mathf.Clamp(_curBossMusicSpeed+=windUpDownSpeed,lowerPitchLimit,_musicSpeed)*_mult;
                bossMusicSource.pitch=_curBossMusicSpeed;
            }else{bgmusicSource.pitch=1;bossMusicSource.pitch=1;}
        }
    }
    public void SetMusic(AudioClip clip,bool force=false){
        if(currentMusic!=clip||force){
            currentMusic=clip;
            bgmusicSource.loop=true;
        }
    }
    public void SetBossMusic(AudioClip clip){
        if (bossMusicSource.clip != null && bossMusicSource.isPlaying){// Already playing boss music
            return;
        }

        bossMusicSource.clip=clip;
        bossMusicSource.loop=true;

        if(bgmusicSource.isPlaying){
            // Fade out regular music
            FadeOut();
        }
        else{
            // Play boss music immediately
            bossMusicSource.Play();
            bossMusicSource.volume=1;
        }
    }
    void FadeOut(){
        if(!fadingOut){
            fadingOut=true;
            startVolume=bgmusicSource.volume;
            fadeStartTime=Time.time;
        }
    }

    void FadeIn(AudioSource source=null){
        if(source==null){
            source=bgmusicSource;
        }

        if(!fadingIn){
            fadingIn=true;
            startVolume=0f;
            endVolume=1f;
            fadeStartTime=Time.time;
            source.volume=0f;
        }
    }

    public void CrossfadeBossToBG(){Crossfade(bossMusicSource,bgmusicSource);}
    public void CrossfadeBGToBoss(){Crossfade(bgmusicSource,bossMusicSource);}
    public void Crossfade(AudioSource audioSource1, AudioSource audioSource2,bool pauseInsteadOfStop=false){
        if(!isCrossFading){StartCoroutine(CrossfadeCoroutine(audioSource1, audioSource2,pauseInsteadOfStop));}
    }
    IEnumerator CrossfadeCoroutine(AudioSource audioSource1, AudioSource audioSource2,bool pauseInsteadOfStop=false){
        isCrossFading=true;
        float t=0.0f;
        float startVolume=audioSource1.volume;
        float endVolume=0.0f;

        audioSource2.volume=endVolume;
        audioSource2.Play();
        while (t < crossfadeDuration) {
            t += Time.deltaTime;
            audioSource1.volume = Mathf.Lerp(startVolume, endVolume, t / crossfadeDuration);
            audioSource2.volume = Mathf.Lerp(endVolume, startVolume, t / crossfadeDuration);
            yield return null;
        }

        if(!pauseInsteadOfStop)audioSource1.Stop();
        else audioSource1.Pause();
        audioSource1.volume=startVolume;
        //audioSource2.Play();
        isCrossFading=false;
    }

    public void SetMusicToCstmzMusic(bool force=false){SetMusic(AssetsManager.instance.GetMusic(SaveSerial.instance.playerData.musicName).track,force);}
    public void PauseBGMusic(){bgmusicSource.Pause();}
    public void UnPauseBGMusic(){bgmusicSource.UnPause();}
    public void PauseBGMusicFor(float delay){StartCoroutine(PauseBGMusicForI(delay));}
    IEnumerator PauseBGMusicForI(float delay){
        PauseBGMusic();
        yield return new WaitForSeconds(delay);
        UnPauseBGMusic();
    }
    public void PauseBossMusic(){bossMusicSource.Pause();}
    public void UnPauseBossMusic(){bossMusicSource.UnPause();}
    public void PauseBossMusicFor(float delay){StartCoroutine(PauseBossMusicForI(delay));}
    IEnumerator PauseBossMusicForI(float delay){
        PauseBossMusic();
        yield return new WaitForSeconds(delay);
        UnPauseBossMusic();
    }
}