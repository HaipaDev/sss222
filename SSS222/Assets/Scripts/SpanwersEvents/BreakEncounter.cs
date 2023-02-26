using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEncounter : MonoBehaviour{    public static BreakEncounter instance;
    [SerializeField] bool ascended;
    [SerializeField] int waveCount;
    public bool calledBreak;
    public bool waitForCargoSpawn;
    void Awake(){instance=this;}
    void Update(){
        if(((ascended&&GameRules.instance.breakEncounterAscendReq)||!GameRules.instance.breakEncounterAscendReq)&&waveCount>=GameRules.instance.breakEncounterWavesReq){CallBreak();ascended=false;waveCount=0;}

        if(calledBreak){
            if(Player.instance!=null){if(Player.instance.transform.position.y>5f&&GameRules.instance.breakEncounterQuitWhenPlayerUp&&!waitForCargoSpawn){QuitBreak();}}
            if(FindObjectOfType<CargoShip>()!=null){if(FindObjectOfType<CargoShip>().transform.position.y<1f){FindObjectOfType<CargoShip>().StopCargo();}}
            if(FindObjectsOfType<Enemy>().Length<=0&&waitForCargoSpawn){Shop.instance.SpawnCargo(dir.up);waitForCargoSpawn=false;}
        }
    }
    public void CallBreak(){
        Debug.Log("Calling Break");
        calledBreak=true;
        waitForCargoSpawn=true;
        FindObjectOfType<Waves>().timeSpawns=-4;
        if(GameRules.instance.breakEncounterPauseMusic){Jukebox.instance.FadeOutBGMusic(true);}
        if(GameManager.instance.CheckGamemodeSelected("Adventure"))GameManager.instance.SaveAdventure();
    }
    public void QuitBreak(){
        Debug.Log("Quitting Break");
        calledBreak=false;
        waitForCargoSpawn=false;
        if(FindObjectOfType<CargoShip>()!=null)FindObjectOfType<CargoShip>().SetCargoSpawnDir(dir.up);
        FindObjectOfType<Waves>().RandomizeWave();
        FindObjectOfType<Waves>().timeSpawns=FindObjectOfType<Waves>().currentWave.timeSpawnWave;
        if(GameRules.instance.breakEncounterPauseMusic){Jukebox.instance.UnPauseBGMusic(true);}
        if(GameManager.instance.CheckGamemodeSelected("Adventure"))GameManager.instance.SaveAdventure();
    }
    public void Ascended(){ascended=true;}
    public void AddWaves(){
        if((GameRules.instance.breakEncounterCountWavesPostAscend&&GameRules.instance.breakEncounterAscendReq&&ascended)
        ||!GameRules.instance.breakEncounterCountWavesPostAscend)waveCount++;
    }
}
