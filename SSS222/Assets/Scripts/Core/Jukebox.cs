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
    [HideInInspector]public bool inverted=false;
    AudioClip currentMusic;
    void Awake(){
        if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
    }
    void Start(){if(currentMusic==null)SetMusicToCstmzMusic();}
    void Update(){
        if(GetComponent<AudioSource>().clip!=currentMusic){GetComponent<AudioSource>().clip=currentMusic;GetComponent<AudioSource>().Play();}

        if(GameRules.instance!=null&&SceneManager.GetActiveScene().name=="Game"&&SaveSerial.instance.settingsData.windDownMusic){
            float _curMusicSpeed=GetComponent<AudioSource>().pitch;
            float _musicSpeed=1f;
            if(GameRules.instance.musicSlowdownOnPause&&GameSession.GlobalTimeIsPausedNotSlowed){_musicSpeed=pauseSpeed;}
            if(GameRules.instance.musicSlowdownOnPaceChange&&!GameSession.GlobalTimeIsPausedNotSlowed/*&&GameSession.instance.speedChanged*/){_musicSpeed=1-(GameSession.instance.defaultGameSpeed-GameSession.instance.gameSpeed);}
            if(GameRules.instance.musicSlowdownOnPaceChange&&!GameSession.GlobalTimeIsPausedNotSlowed&&Player.instance!=null){_musicSpeed=1-(GameSession.instance.defaultGameSpeed-GameSession.instance.gameSpeed);}
            if(Player.instance==null){_musicSpeed=deadSpeed;}

            if(GameRules.instance.musicSlowdownOnPause&&!GameSession.GlobalTimeIsPausedNotSlowed&&!GameRules.instance.musicSlowdownOnPaceChange){_musicSpeed=1f;}
            
            else if(_curMusicSpeed>_musicSpeed)_curMusicSpeed=Mathf.Clamp(_curMusicSpeed-=windUpDownSpeed,_musicSpeed,upperPitchLimit);
            if(_curMusicSpeed<_musicSpeed)_curMusicSpeed=Mathf.Clamp(_curMusicSpeed+=windUpDownSpeed,lowerPitchLimit,_musicSpeed);
            int _mult=1;if(inverted){_mult=-1;}else{_mult=1;}
            GetComponent<AudioSource>().pitch=_curMusicSpeed*_mult;//*(inverted?  1 : 0);
        }else{GetComponent<AudioSource>().pitch=1;}
    }
    public void SetMusic(AudioClip clip,bool force=false){if(currentMusic!=clip||force)currentMusic=clip;}
    public void SetMusicToCstmzMusic(bool force=false){SetMusic(GameAssets.instance.GetMusic(SaveSerial.instance.playerData.musicName).track,force);}
    public void PauseFor(float delay){StartCoroutine(PauseForI(delay));}
    IEnumerator PauseForI(float delay){
        GetComponent<AudioSource>().Pause();
        yield return new WaitForSeconds(delay);
        GetComponent<AudioSource>().UnPause();
    }
}
