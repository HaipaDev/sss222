using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;
public class GameSession : MonoBehaviour{
    public int score = 0;
    public int EVscore = 0;
    public int coins = 0;
    [Range(0.0f, 10.0f)] public float gameSpeed = 1f;
    public bool moveByMouse = true;

    /*public SavableData savableData;
    [System.Serializable]
    public class SavableData{
        public int highscore;
        public SavableData(SavableData data)
        {
            highscore = data.highscore;
        }
        public void Save()
        {
            SaveSystem.SaveData(this);
        }
        public void Load(){
            SavableData data = SaveSystem.LoadData();
            highscore = data.highscore;
        }
    }*/

    private void Awake(){
        SetUpSingleton();
    }
    private void SetUpSingleton(){
        int numberOfObj = FindObjectsOfType<GameSession>().Length;
        if(numberOfObj > 1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        FindObjectOfType<SaveSerial>().highscore = 0;
    }
    private void Update()
    {
        Time.timeScale = gameSpeed;
    }

    public int GetScore(){return score;}
    public int GetCoins(){return coins;}
    public int GetEVScore(){return EVscore;}
    public int GetHighscore(){return FindObjectOfType<SaveSerial>().highscore;}

    public void AddToScore(int scoreValue){
        score += scoreValue;
        EVscore += scoreValue;
    }

    public void MultiplyScore(float multipl)
    {
        int result=Mathf.RoundToInt(score * multipl);
        score = result;
    }

    public void AddToScoreNoEV(int scoreValue){score += scoreValue;}

    public void ResetScore(){
        score=0;
        EVscore = 0;
        coins = 0;
    }
    public void SaveHighscore()
    {
        if (score > FindObjectOfType<SaveSerial>().highscore) FindObjectOfType<SaveSerial>().highscore = score;
        //FindObjectOfType<DataSavable>().highscore = highscore;
    }
    public void SaveSettings(){
        FindObjectOfType<SaveSerial>().moveByMouse = this.moveByMouse;
    }
    public void Save(){ FindObjectOfType<SaveSerial>().Save();}
    public void Load(){ FindObjectOfType<SaveSerial>().Load();}
}
