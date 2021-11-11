using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubmitScore : MonoBehaviour{
    //[SerializeField]TextMeshProUGUI txtInput;
    public void SubmitScoreFunc(){
        var db=FindObjectOfType<DBAccess>();
        if(SaveSerial.instance.hyperGamerLoginData.username!=""){
            db.SaveScoreToDB(SaveSerial.instance.hyperGamerLoginData.username,GameSession.instance.GetHighscore(GameSession.instance.gameModeSelected));
            if(FindObjectOfType<DisplayLeaderboard>().currentUser)FindObjectOfType<DisplayLeaderboard>().DisplayCurrentUserHighscore();
        }
        else{GSceneManager.instance.LoadLoginScene();}
    }
    public void ReturnToGMInfo(){StartCoroutine(ReturnToGMInfoI());}
    IEnumerator ReturnToGMInfoI(){
        yield return new WaitForSecondsRealtime(3.5f);
        GSceneManager.instance.LoadGameModeInfoScene();
    }
}
