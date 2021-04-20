using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayLeaderboard : MonoBehaviour{
    public int rank;
    public string username;
    public int score;
    TextMeshProUGUI txtRank;
    TextMeshProUGUI txtScore;
    void Update(){
        txtRank=transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        txtScore=transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        txtRank.text="#"+rank.ToString();
        txtScore.text=username+" : \n"+score.ToString();
    }
}
