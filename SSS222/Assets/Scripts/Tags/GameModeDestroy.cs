using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameModeDestroy : MonoBehaviour{
    [SerializeField] bool reverse;
    [SerializeField] string gamemodeName;
    //[SerializeField] int gamemodeID;
    IEnumerator Start(){
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0,0.25f));//Prevent overload crash
        if(!reverse&&GameSession.instance.gameModeSelected!=Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e => e.cfgName.Contains(gamemodeName))){Destroy(gameObject);}
        else if(reverse&&GameSession.instance.gameModeSelected==Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e => e.cfgName.Contains(gamemodeName))){Destroy(gameObject);}
    }
}
