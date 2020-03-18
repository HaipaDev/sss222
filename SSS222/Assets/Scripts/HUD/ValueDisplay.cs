using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI scoreText;
    GameSession gameSession;
    [SerializeField] string value = "score";

    // Start is called before the first frame update
    void Start(){
        scoreText = GetComponent<TMPro.TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update(){
        if (value == "score") scoreText.text = gameSession.GetScore().ToString();
        else if (value == "evscore") scoreText.text = gameSession.GetEVScore().ToString();
        else if (value == "coins") scoreText.text = gameSession.GetCoins().ToString();
    }
}
