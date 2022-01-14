using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubmitScore : MonoBehaviour{
    //[SerializeField]TextMeshProUGUI txtInput;
    public void SubmitScoreFunc(){
        if(SaveSerial.instance.hyperGamerLoginData.username!=""&&SaveSerial.instance.hyperGamerLoginData.loggedIn){
            DBAccess.instance.SaveScoreToDB(SaveSerial.instance.hyperGamerLoginData.username,GameSession.instance.GetHighscoreCurrent());
            if(FindObjectOfType<DisplayLeaderboard>().currentUser)FindObjectOfType<DisplayLeaderboard>().DisplayCurrentUserHighscore();
        }
        else{GSceneManager.instance.LoadLoginScene();}
    }
    public void ReturnToGMInfo(){StartCoroutine(ReturnToGMInfoI());}
    IEnumerator ReturnToGMInfoI(){
        yield return new WaitForSecondsRealtime(4f);
        GSceneManager.instance.LoadGameModeInfoScene();
    }
}
