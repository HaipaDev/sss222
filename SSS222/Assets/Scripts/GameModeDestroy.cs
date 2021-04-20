using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeDestroy : MonoBehaviour{
    [SerializeField] int gamemodeID;
    void Start(){
        if(GameSession.instance.gameModeSelected!=gamemodeID){Destroy(gameObject);}
    }
}
