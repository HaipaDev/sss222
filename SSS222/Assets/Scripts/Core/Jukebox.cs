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

    [SerializeField] float fadeDuration=1f;
    bool fadingIn=false;
    bool fadingOut=false;
    float startVolume=0f;
    float endVolumeDef=0.4f;
    float endVolume=1f;
    float fadeStartTime;
    AudioSource fadingInSource;
    AudioSource fadingOutSource;
    bool fadeOutPauseInsteadOfStop;
    [SerializeField] public float crossfadeDuration=1.0f;
    bool isCrossFading;

    AudioSource bgmusicSource;
    AudioSource bossMusicSource;
    void Awake(){
        if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);gameObject.name=gameObject.name.Split('(')[0];}
    }
    void Start(){
        if(bgmusicSource==null){
            bgmusicSource=GetComponent<AudioSource>();
            bgmusicSource.loop=true;
            //endVolumeDef=bgmusicSource.volume;
            endVolume=endVolumeDef;
        }
        if(bossMusicSource==null){SetupBossMusicSource();}

        StartCoroutine(SetMusicToCstmzI());
    }
    IEnumerator SetMusicToCstmzI(){
        yield return new WaitForSecondsRealtime(0.1f);
        SetMusicToCstmzMusic();
    }
    void SetupBossMusicSource(){
        if(bgmusicSource==null){bgmusicSource=GetComponent<AudioSource>();bgmusicSource.Stop();}
        bossMusicSource=gameObject.AddComponent<AudioSource>();
        if(bgmusicSource!=null){
            bossMusicSource.outputAudioMixerGroup=bgmusicSource.outputAudioMixerGroup;
            bossMusicSource.priority=bgmusicSource.priority;
            bossMusicSource.volume=bgmusicSource.volume;
        }
        bossMusicSource.loop=true;
    }
    void Update(){
        if(fadingIn&&fadingInSource!=null){
            if(!fadingInSource.isPlaying){fadingInSource.UnPause();fadingInSource.Play();}
            float progress=(Time.time - fadeStartTime) / fadeDuration;
            fadingInSource.volume=Mathf.Lerp(startVolume, endVolume, progress);

            if(fadingInSource.volume==endVolume){
                fadingIn=false;
            }
        }

        if(fadingOut&&fadingOutSource!=null){
            float progress=(Time.time - fadeStartTime) / fadeDuration;
            fadingOutSource.volume=Mathf.Lerp(startVolume, 0f, progress);

            if(fadingOutSource.volume==0f){
                fadingOut=false;
                if(!fadeOutPauseInsteadOfStop){
                    if(fadingOutSource==bgmusicSource){StopBGMusic();}
                    else if(fadingOutSource==bgmusicSource){StopBossMusic();}
                }else{
                    if(fadingOutSource==bgmusicSource){StopBGMusic();}
                    else if(fadingOutSource==bgmusicSource){StopBossMusic();}
                }
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
        if(bgmusicSource.clip!=clip||force){
            bgmusicSource.clip=clip;
            bgmusicSource.loop=true;
            bgmusicSource.Play();
        }
    }
    public void SetBossMusic(AudioClip clip){
        if(bossMusicSource!=null){
            if(bossMusicSource.clip != null && bossMusicSource.isPlaying){// Already playing boss music
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
                bossMusicSource.volume=endVolumeDef;
            }
        }else{Debug.LogWarning("Boss music source not present, adding manually");SetupBossMusicSource();SetBossMusic(clip);}
    }

    public void FadeIn(AudioSource source=null){
        if(source==null){
            fadingInSource=bgmusicSource;
        }else{fadingInSource=source;}

        if(!fadingIn){
            fadingIn=true;
            startVolume=0f;
            endVolume=endVolumeDef;
            fadeStartTime=Time.time;
            fadingInSource.volume=0f;
        }
    }
    public void FadeOut(AudioSource source=null,bool pauseInsteadOfStop=false){
        if(source==null){
            fadingOutSource=bgmusicSource;
        }else{fadingOutSource=source;}

        if(!fadingOut){
            fadingOut=true;
            startVolume=fadingOutSource.volume;
            fadeStartTime=Time.time;
            fadeOutPauseInsteadOfStop=pauseInsteadOfStop;
        }
    }
    public void FadeInBGMusic(){FadeIn(bgmusicSource);}
    public void FadeOutBGMusic(bool pauseInsteadOfStop=false){FadeOut(bgmusicSource,pauseInsteadOfStop);}
    public void FadeInBossMusic(){FadeIn(bossMusicSource);}
    public void FadeOutBossMusic(bool pauseInsteadOfStop=false){FadeOut(bossMusicSource,pauseInsteadOfStop);}

    public void CrossfadeBossToBG(){if(bossMusicSource.isPlaying)Crossfade(bossMusicSource,bgmusicSource);}
    public void CrossfadeBGToBoss(){if(bgmusicSource.isPlaying)Crossfade(bgmusicSource,bossMusicSource,true);}
    public void Crossfade(AudioSource audioSource1, AudioSource audioSource2,bool pauseInsteadOfStop=false){
        if(!isCrossFading){StartCoroutine(CrossfadeCoroutine(audioSource1, audioSource2, pauseInsteadOfStop));}
    }
    IEnumerator CrossfadeCoroutine(AudioSource audioSource1, AudioSource audioSource2,bool pauseInsteadOfStop=false){
        isCrossFading=true;
        float t=0.0f;
        float _startVolume=audioSource1.volume;
        float _endVolume=0.0f;

        audioSource2.volume=_endVolume;
        if(!audioSource2.isPlaying)audioSource2.Play();
        else audioSource2.UnPause();
        while(t<crossfadeDuration){
            t+=Time.deltaTime;
            audioSource1.volume=Mathf.Lerp(_startVolume, _endVolume, t / crossfadeDuration);
            audioSource2.volume=Mathf.Lerp(_endVolume, _startVolume, t / crossfadeDuration);
            yield return null;
        }

        if(!pauseInsteadOfStop)audioSource1.Stop();
        else audioSource1.Pause();
        audioSource1.volume=_startVolume;
        isCrossFading=false;
    }

    public void SetMusicToCstmzMusic(bool force=false){SetMusic(AssetsManager.instance.GetMusic(SaveSerial.instance.playerData.musicName).track,force);}
    public void PlayBGMusic(bool fadeIn=false){bgmusicSource.Play();if(fadeIn){FadeInBGMusic();}}
    public void StopBGMusic(){bgmusicSource.Stop();}
    public void PauseBGMusic(){bgmusicSource.Pause();}
    public void UnPauseBGMusic(bool fadeIn=false){bgmusicSource.UnPause();if(fadeIn){FadeInBGMusic();}}
    public void PauseBGMusicFor(float delay){StartCoroutine(PauseBGMusicForI(delay));}
    IEnumerator PauseBGMusicForI(float delay){
        PauseBGMusic();
        yield return new WaitForSeconds(delay);
        UnPauseBGMusic();
    }
    public bool BGMusicSilenced(){return bgmusicSource.volume==0;}
    public void PlayBossMusic(bool fadeIn=false){bossMusicSource.Play();if(fadeIn){FadeInBossMusic();}}
    public void StopBossMusic(){bossMusicSource.Stop();}
    public void PauseBossMusic(){bossMusicSource.Pause();}
    public void UnPauseBossMusic(bool fadeIn=false){bossMusicSource.UnPause();if(fadeIn){FadeInBossMusic();}}
    public void PauseBossMusicFor(float delay){StartCoroutine(PauseBossMusicForI(delay));}
    IEnumerator PauseBossMusicForI(float delay){
        PauseBossMusic();
        yield return new WaitForSeconds(delay);
        UnPauseBossMusic();
    }
    public bool BossMusicSilenced(){return bossMusicSource.volume==0;}
}