using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour{
    int score = 0;
    public int EVscore = 0;
    public int coins = 0;
    [Range(0.0f, 10.0f)] public float gameSpeed = 1f;
    // Start is called before the first frame update
    private void Awake(){
        SetUpSingleton();
    }

    private void SetUpSingleton(){
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numberGameSessions>1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore(){return score;}
    public int GetCoins(){return coins;}
    public int GetEVScore(){return EVscore;}

    public void AddToScore(int scoreValue){
        score += scoreValue;
        EVscore += scoreValue;
    }

    public void AddToScoreNoEV(int scoreValue){score += scoreValue;}

    public void ResetScore(){
        Destroy(gameObject);
    }

    private void Update()
    {
        Time.timeScale = gameSpeed;
    }
}
