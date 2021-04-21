using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubmitScore : MonoBehaviour{
    [SerializeField]TextMeshProUGUI txtInput;
    public void SubmitScoreFunc(int gamemodeID){
        var db=FindObjectOfType<DBAccess>();
        db.SaveScoreToDB(txtInput.text,GameSession.instance.GetHighscore(gamemodeID));
    }
}
