using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameModeDestroy : MonoBehaviour{
    [SerializeField] bool reverse;
    [SerializeField] string gamemodeName;
    //[SerializeField] int gamemodeID;
    void Start(){
        if(!reverse&&GameSession.instance.gameModeSelected!=Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e => e.cfgName.Contains(gamemodeName))){Destroy(gameObject);}
        else if(reverse&&GameSession.instance.gameModeSelected==Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e => e.cfgName.Contains(gamemodeName))){Destroy(gameObject);}
    }
}
