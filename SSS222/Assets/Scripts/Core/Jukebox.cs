using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour{   public static Jukebox instance;
    void Awake(){
        if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
    }
    void Update(){
        var currentMusic=GameAssets.instance.GetMusic(SaveSerial.instance.playerData.musicName);
        if(GetComponent<AudioSource>().clip!=currentMusic.track){GetComponent<AudioSource>().clip=currentMusic.track;GetComponent<AudioSource>().Play();}
    }
}
