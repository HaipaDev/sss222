using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;
public class GameSession : MonoBehaviour{
    public int score = 0;
    public int EVscore = 0;
    public int shopScore = 0;
    public int shopScoreMax = 200;
    public int shopScoreMaxS = 200;
    public int shopScoreMaxE = 450;
    public int coins = 0;
    [Range(0.0f, 10.0f)] public float gameSpeed = 1f;
    //public bool moveByMouse = true;

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
        if(shopScore>=shopScoreMax)
        {
            Shop.shopOpen = true;
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach(Enemy enemy in enemies){
                enemy.givePts = false;
                enemy.health = -1;
                enemy.Die();
            }
            gameSpeed = 0f;
            shopScoreMax = Random.Range(shopScoreMaxS,shopScoreMaxE);
            shopScore = 0;
        }
    }

    public int GetScore(){return score;}
    public int GetCoins(){return coins;}
    public int GetEVScore(){return EVscore;}
    public int GetShopScore(){return shopScore; }
    public int GetHighscore(){return FindObjectOfType<SaveSerial>().highscore;}

    public void AddToScore(int scoreValue){
        score += scoreValue;
        EVscore += scoreValue;
        shopScore += scoreValue;
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
        shopScore = 0;
        coins = 0;
    }
    public void SaveHighscore()
    {
        if (score > FindObjectOfType<SaveSerial>().highscore) FindObjectOfType<SaveSerial>().highscore = score;
        //FindObjectOfType<DataSavable>().highscore = highscore;
    }
    public void SaveSettings(){
        FindObjectOfType<SaveSerial>().moveByMouse = FindObjectOfType<SettingsMenu>().moveByMouse;
        FindObjectOfType<SaveSerial>().quality = FindObjectOfType<SettingsMenu>().quality;
        FindObjectOfType<SaveSerial>().fullscreen = FindObjectOfType<SettingsMenu>().fullscreen;
        FindObjectOfType<SaveSerial>().masterVolume = FindObjectOfType<SettingsMenu>().masterVolume;
        FindObjectOfType<SaveSerial>().soundVolume = FindObjectOfType<SettingsMenu>().soundVolume;
        FindObjectOfType<SaveSerial>().musicVolume = FindObjectOfType<SettingsMenu>().musicVolume;
    }
    public void SaveInventory(){
        FindObjectOfType<SaveSerial>().chameleonColor[0] = FindObjectOfType<Inventory>().chameleonColorArr[0];
        FindObjectOfType<SaveSerial>().chameleonColor[1] = FindObjectOfType<Inventory>().chameleonColorArr[1];
        FindObjectOfType<SaveSerial>().chameleonColor[2] = FindObjectOfType<Inventory>().chameleonColorArr[2];
    }
    public void Save(){ FindObjectOfType<SaveSerial>().Save(); FindObjectOfType<SaveSerial>().SaveSettings(); }
    public void Load(){ FindObjectOfType<SaveSerial>().Load(); FindObjectOfType<SaveSerial>().LoadSettings(); }
}
