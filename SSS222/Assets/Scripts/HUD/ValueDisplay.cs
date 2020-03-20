using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI scoreText;
    GameSession gameSession;
    Player player;
    [SerializeField] string value = "score";
    [SerializeField] float valueLimitD=-1;

    // Start is called before the first frame update
    void Start(){
        scoreText = GetComponent<TMPro.TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update(){
        if (value == "score") scoreText.text = gameSession.GetScore().ToString();
        else if (value == "evscore") scoreText.text = gameSession.GetEVScore().ToString();
        else if (value == "coins") scoreText.text = gameSession.GetCoins().ToString();
        /*else if (value == "state"){
            var value = System.Math.Round(player.GetGCloverTimer(),1);

            if (value <= valueLimitD){ value = 0; }
            else { scoreText.text = value.ToString(); }
        }*/
    }
}
