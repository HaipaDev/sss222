﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;
public class GameSession : MonoBehaviour{
    [HeaderAttribute("Current Player Values")]
    public int score = 0;
    public float scoreMulti = 1f;
    public int coins = 0;
    public int cores = 0;
    public float coresXp = 0f;
    public float coresXpTotal = 0f;
    public int enemiesCount = 0;
    [HeaderAttribute("EVent Score Values")]
    public int EVscore = 0;
    public int EVscoreMax = 50;
    public int shopScore = 0;
    public int shopScoreMax = 200;
    public int shopScoreMaxS = 200;
    public int shopScoreMaxE = 450;
    [HeaderAttribute("XP Values")]
    public float xp_forCore=100f;
    public float xp_wave=20f;
    public float xp_shop=10f;
    public float xp_powerup=3f;
    public float xp_flying=7f;
    public float flyingTimeReq=25f;
    [HeaderAttribute("Settings")]
    [Range(0.0f, 10.0f)] public float gameSpeed = 1f;
    [HeaderAttribute("Other")]
    [SerializeField] public AudioClip denySFX;
    Player player;
    //public string gameVersion;
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

        if(FindObjectOfType<Player>()!=null){if(FindObjectOfType<Player>().timeFlyingCore>flyingTimeReq){AddXP(xp_flying);FindObjectOfType<Player>().timeFlyingCore=0f;}}

        if(coresXp>=xp_forCore){
            cores+=1;
            coresXp=0;
        }
    }

    public int GetScore(){return score;}
    public int GetCoins(){return coins;}
    public int GetCores(){return cores;}
    public float GetCoresXP(){return coresXp;}
    public int GetEVScore(){return EVscore;}
    public int GetShopScore(){return shopScore; }
    public int GetHighscore(){return FindObjectOfType<SaveSerial>().highscore;}
    public string GetVersion(){return FindObjectOfType<SaveSerial>().gameVersion;}

    public void AddToScore(int scoreValue){
        score += Mathf.RoundToInt(scoreValue*scoreMulti);
        EVscore += scoreValue;
        shopScore += Mathf.RoundToInt(scoreValue*scoreMulti);
    }

    public void MultiplyScore(float multipl)
    {
        int result=Mathf.RoundToInt(score * multipl);
        score = result;
    }

    public void AddToScoreNoEV(int scoreValue){score += scoreValue;}
    public void AddXP(float xpValue){coresXp += xpValue;coresXpTotal+=xpValue;}
    public void AddEnemyCount(){enemiesCount++;FindObjectOfType<DisruptersSpawner>().EnemiesCountHealDrone++;}

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

        FindObjectOfType<SaveSerial>().SaveSettings();
    }
    public void SaveInventory(){
        FindObjectOfType<SaveSerial>().chameleonColor[0] = FindObjectOfType<Inventory>().chameleonColorArr[0];
        FindObjectOfType<SaveSerial>().chameleonColor[1] = FindObjectOfType<Inventory>().chameleonColorArr[1];
        FindObjectOfType<SaveSerial>().chameleonColor[2] = FindObjectOfType<Inventory>().chameleonColorArr[2];
    }
    public void Save(){ FindObjectOfType<SaveSerial>().Save(); FindObjectOfType<SaveSerial>().SaveSettings(); }
    public void Load(){ FindObjectOfType<SaveSerial>().Load(); FindObjectOfType<SaveSerial>().LoadSettings(); }


    public void PlayDenySFX(){AudioSource.PlayClipAtPoint(denySFX,transform.position);}
}
