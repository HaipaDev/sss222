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
    void Awake(){
        if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);gameObject.name=gameObject.name.Split('(')[0];}
    }
    IEnumerator Start(){yield return new WaitForSecondsRealtime(0.1f);if(currentMusic==null)SetMusicToCstmzMusic();}
    void Update(){
        if(GetComponent<AudioSource>().clip!=currentMusic){GetComponent<AudioSource>().clip=currentMusic;GetComponent<AudioSource>().Play();}

        if(GameRules.instance!=null&&SceneManager.GetActiveScene().name=="Game"&&SaveSerial.instance.settingsData.windDownMusic){
            float _curMusicSpeed=GetComponent<AudioSource>().pitch;
            float _musicSpeed=1f;
            if(GameRules.instance.musicSlowdownOnPause&&GameManager.GlobalTimeIsPausedNotSlowed){_musicSpeed=pauseSpeed;}
            if(GameRules.instance.musicSlowdownOnPaceChange&&!GameManager.GlobalTimeIsPausedNotSlowed/*&&GameManager.instance.speedChanged*/){_musicSpeed=1-(GameManager.instance.defaultGameSpeed-GameManager.instance.gameSpeed);}
            if(GameRules.instance.musicSlowdownOnPaceChange&&!GameManager.GlobalTimeIsPausedNotSlowed&&Player.instance!=null){_musicSpeed=1-(GameManager.instance.defaultGameSpeed-GameManager.instance.gameSpeed);}
            if(Player.instance==null){_musicSpeed=deadSpeed;inverted=false;}

            if(GameRules.instance.musicSlowdownOnPause&&!GameManager.GlobalTimeIsPausedNotSlowed&&!GameRules.instance.musicSlowdownOnPaceChange){_musicSpeed=1f;}
            
            int _mult=1;if(inverted){_mult=-1;}else{_mult=1;}
            if(_curMusicSpeed>_musicSpeed*_mult)_curMusicSpeed=Mathf.Clamp(_curMusicSpeed-=windUpDownSpeed,_musicSpeed,upperPitchLimit)*_mult;
            if(_curMusicSpeed<_musicSpeed*_mult)_curMusicSpeed=Mathf.Clamp(_curMusicSpeed+=windUpDownSpeed,lowerPitchLimit,_musicSpeed)*_mult;
            //int _mult=1;if(inverted){_mult=-1;GetComponent<AudioSource>().pitch=-1;}else{GetComponent<AudioSource>().pitch=_curMusicSpeed;}
            GetComponent<AudioSource>().pitch=_curMusicSpeed;
        }else{GetComponent<AudioSource>().pitch=1;}
    }
    public void SetMusic(AudioClip clip,bool force=false){if(currentMusic!=clip||force)currentMusic=clip;GetComponent<AudioSource>().loop=true;}
    public void SetMusicToCstmzMusic(bool force=false){SetMusic(AssetsManager.instance.GetMusic(SaveSerial.instance.playerData.musicName).track,force);}
    public void Pause(){GetComponent<AudioSource>().Pause();}
    public void UnPause(){GetComponent<AudioSource>().UnPause();}
    public void PauseFor(float delay){StartCoroutine(PauseForI(delay));}
    IEnumerator PauseForI(float delay){
        Pause();
        yield return new WaitForSeconds(delay);
        UnPause();
    }
}
