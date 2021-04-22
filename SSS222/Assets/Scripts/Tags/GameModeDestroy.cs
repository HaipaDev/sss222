using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeDestroy : MonoBehaviour{
    [SerializeField] bool reverse;
    [SerializeField] int gamemodeID;
    void Start(){
        if(!reverse&&GameSession.instance.gameModeSelected!=gamemodeID){Destroy(gameObject);}
        else if(reverse&&GameSession.instance.gameModeSelected==gamemodeID){Destroy(gameObject);}
    }
}
