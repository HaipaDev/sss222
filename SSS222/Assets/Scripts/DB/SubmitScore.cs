using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubmitScore : MonoBehaviour{
    //[SerializeField]TextMeshProUGUI txtInput;
    public void SubmitScoreFunc(int gamemodeID){
        var db=FindObjectOfType<DBAccess>();
        if(SaveSerial.instance.hyperGamerLoginData.username!="")db.SaveScoreToDB(SaveSerial.instance.hyperGamerLoginData.username,GameSession.instance.GetHighscore(gamemodeID));
    }
}
