using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using BayatGames.SaveGameFree;
using UnityEngine.Rendering.PostProcessing;
//using UnityEngine.InputSystem;
public class GameSession : MonoBehaviour{
    public static GameSession instance;
    [Header("Global")]
    public bool shopOn=true;
    public bool shopCargoOn=true;
    public bool upgradesOn=true;
    public bool xpOn=true;
    [Header("Current Player Values")]
    public int score = 0;
    public float scoreMulti = 1f;
    public float luckMulti = 1f;
    public int coins = 0;
    public int cores = 0;
    public float coresXp = 0f;
    public float coresXpTotal = 0f;
    public int enemiesCount = 0;
    [Header("EVent Score Values")]
    public int EVscore = 0;
    public int EVscoreMax = 50;
    public int shopScore = 0;
    public int shopScoreMax = 200;
    public int shopScoreMaxS = 200;
    public int shopScoreMaxE = 450;
    [Header("XP Values")]
    public float xp_forCore=100f;
    public float xp_wave=20f;
    public float xp_shop=10f;
    public float xp_powerup=3f;
    public float xp_flying=7f;
    public float flyingTimeReq=25f;
    public float xp_staying=-5f;
    public float stayingTimeReq=4f;
    [Header("Luck Multiplier Values")]
    public float enballDropMulti=1;
    public float coinDropMulti=1;
    public float coreDropMulti=1;
    public float rarePwrupMulti=1;
    public float legendPwrupMulti=1;
    public float enballMultiAmnt=0.15f;
    public float coinMultiAmnt=0.12f;
    public float coreMultiAmnt=0.08f;
    public float rareMultiAmnt=0.075f;
    public float legendMultiAmnt=0.01f;
    [Header("Settings")]
    [Range(0.0f, 10.0f)] public float gameSpeed=1f;
    public float defaultGameSpeed=1f;
    public bool speedChanged;
    public bool slowingPause;
    [Header("Other")]
    public bool cheatmode;
    public bool dmgPopups=true;
    public bool analyticsOn=true;
    public int gameModeSelected;
    public const int gameModeMaxID=5;//-1
    [SerializeField]float restartTimer=-4;
    
    Player player;
    PostProcessVolume postProcessVolume;
    bool setValues;
    public float gameSessionTime=0;
    //[SerializeField] InputMaster inputMaster;
    [Range(0,2)]public static int maskMode=1;
    
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
        instance=this;
        StartCoroutine(SetGameRulesValues());
        #if UNITY_EDITOR
        cheatmode=true;
        #else
        cheatmode=false;
        #endif
    }
    private void SetUpSingleton(){
        int numberOfObj = FindObjectsOfType<GameSession>().Length;
        if(numberOfObj > 1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start(){
        Array.Clear(SaveSerial.instance.playerData.highscore,0,SaveSerial.instance.playerData.highscore.Length);
        //SetGameRulesValues();
    }
    IEnumerator SetGameRulesValues(){
    yield return new WaitForSeconds(0.03f);
    //Set values
    var i=GameRules.instance;
    if(i!=null){
        //Main
        defaultGameSpeed=i.defaultGameSpeed;gameSpeed=defaultGameSpeed;
        shopOn=i.shopOn;
        xpOn=i.xpOn;
        upgradesOn=i.upgradesOn;
        EVscoreMax=i.EVscoreMax;
        shopScoreMax=i.shopScoreMax;
        shopScoreMaxS=i.shopScoreMaxS;
        shopScoreMaxE=i.shopScoreMaxE;
        scoreMulti=i.scoreMulti;
        luckMulti=i.luckMulti;
        //Leveling
        xp_forCore=i.xp_forCore;
        xp_wave=i.xp_wave;
        xp_shop=i.xp_shop;
        xp_powerup=i.xp_powerup;
        xp_flying=i.xp_flying;
        flyingTimeReq=i.flyingTimeReq;
        xp_staying=i.xp_staying;
        stayingTimeReq=i.stayingTimeReq;
    }
    }
    private void Update()
    {
        if(gameSpeed>=0){Time.timeScale=gameSpeed;}if(gameSpeed<0){gameSpeed=0;}

        //Set values on Enter Game Room
        if(!setValues&&(SceneManager.GetActiveScene().name=="Game")){
            StartCoroutine(SetGameRulesValues());
            setValues=true;
        }
        if(SceneManager.GetActiveScene().name=="Game"&&FindObjectOfType<Player>()!=null){gameSessionTime+=Time.unscaledDeltaTime;}
        if(SceneManager.GetActiveScene().name!="Game"&&setValues==true){setValues=false;}

        if(shopOn&&(shopScore>=shopScoreMax && coins>0))
        {
            if(shopCargoOn){Shop.instance.SpawnCargo();}
            else{Shop.shopOpen = true;
            foreach(Enemy enemy in FindObjectsOfType<Enemy>()){
                enemy.givePts = false;
                enemy.health = -1;
                enemy.Die();
            }
            gameSpeed=0f;}
            shopScoreMax=UnityEngine.Random.Range(shopScoreMaxS,shopScoreMaxE);
            shopScore=0;
        }

        if(FindObjectOfType<Player>()!=null){
            if(FindObjectOfType<Player>().timeFlyingCore>flyingTimeReq){AddXP(xp_flying);FindObjectOfType<Player>().timeFlyingCore=0f;}
            if(FindObjectOfType<Player>().stayingTimerCore>stayingTimeReq){if(coresXp>-xp_staying)AddXP(xp_staying);FindObjectOfType<Player>().stayingTimerCore=0f;}
        }
        
        coresXp=Mathf.Clamp(coresXp,0,xp_forCore);
        if(coresXpTotal<0)coresXpTotal=0;
        if(xpOn&&coresXp>=xp_forCore){
            //cores++;
            if(upgradesOn){
                GameAssets.instance.Make("PowerCore",new Vector2(UnityEngine.Random.Range(-3.5f, 3.5f),7.4f));
                FindObjectOfType<UpgradeMenu>().total_UpgradesCount++;
            }
            //FindObjectOfType<UpgradeMenu>().total_UpgradesCount++;
            coresXp=0;
            //AudioManager.instance.Play("LvlUp");
            AudioManager.instance.Play("LvlUp2");
        }

        //Set speed to normal
        if(PauseMenu.GameIsPaused==false&&Shop.shopOpened==false&&UpgradeMenu.UpgradeMenuIsOpen==false&&
        (FindObjectOfType<Player>()!=null&&FindObjectOfType<Player>().matrix==false&&FindObjectOfType<Player>().accel==false)&&speedChanged!=true){gameSpeed=defaultGameSpeed;}
        if(SceneManager.GetActiveScene().name!="Game"){gameSpeed=1;}
        //if(Shop.shopOpen==false&&Shop.shopOpened==false){gameSpeed=1;}
        if(FindObjectOfType<Player>()==null){gameSpeed=defaultGameSpeed;}
        
        //Restart with R or Space/Resume with Space
        if(SceneManager.GetActiveScene().name=="Game"){
        if((GameOverCanvas.instance==null||GameOverCanvas.instance.gameOver==false)&&PauseMenu.GameIsPaused==false){restartTimer=-4;}
        if(PauseMenu.GameIsPaused==true){if(restartTimer==-4)restartTimer=0.5f;}
        if(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true){if(restartTimer==-4)restartTimer=1f;}
        else if(PauseMenu.GameIsPaused==true&&Input.GetKeyDown(KeyCode.Space)){FindObjectOfType<PauseMenu>().Resume();}
        if(restartTimer>0)restartTimer-=Time.unscaledDeltaTime;
        if(restartTimer<=0&&restartTimer!=-4){if(Input.GetKeyDown(KeyCode.R)||(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true&&Input.GetKeyDown(KeyCode.Space))){FindObjectOfType<Level>().RestartGame();restartTimer=-4;}}
        if(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true&&Input.GetKeyDown(KeyCode.Escape)){FindObjectOfType<Level>().LoadStartMenu();}
        }

        if((PauseMenu.GameIsPaused==true||Shop.shopOpened==true||UpgradeMenu.UpgradeMenuIsOpen==true)&&(FindObjectOfType<Player>()!=null&&FindObjectOfType<Player>().inverter==true)){
            foreach(AudioSource sound in FindObjectsOfType<AudioSource>()){
                if(sound!=null){
                    GameObject snd=sound.gameObject;
                    //if(sound!=musicPlayer){
                    if(snd.GetComponent<MusicPlayer>()==null){
                        //sound.pitch=1;
                        sound.Stop();
                    }
                }
            }
        }
        if(FindObjectOfType<Player>()!=null&&FindObjectOfType<Player>().inverter==false){
            foreach(AudioSource sound in FindObjectsOfType<AudioSource>()){
                if(sound!=null){
                    GameObject snd=sound.gameObject;
                    //if(sound!=musicPlayer){
                    if(snd.GetComponent<MusicPlayer>()==null){
                        if(sound.pitch==-1)sound.pitch=1;
                        if(sound.loop==true)sound.loop=false;
                    }
                }
            }
        }

        //Postprocessing
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance!=null){
        if(SaveSerial.instance.settingsData.pprocessing==true && postProcessVolume!=null){postProcessVolume.GetComponent<PostProcessVolume>().enabled=true;}
        if(SaveSerial.instance.settingsData.pprocessing==false && FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.GetComponent<PostProcessVolume>().enabled=false;}
        }

        if(UpgradeMenu.instance!=null)CalculateLuck();

        CheckCodes(0,0);
    }

    public int GetScore(){return score;}
    public int GetCoins(){return coins;}
    public int GetCores(){return cores;}
    public float GetCoresXP(){return coresXp;}
    public int GetEVScore(){return EVscore;}
    public int GetShopScore(){return shopScore; }
    public int GetHighscore(int i){return SaveSerial.instance.playerData.highscore[i];}
    public string GetVersion(){return SaveSerial.instance.settingsData.gameVersion;}

    public void AddToScore(int scoreValue){
        score += Mathf.RoundToInt(scoreValue*scoreMulti);
        EVscore += scoreValue;
        if(shopOn)shopScore += Mathf.RoundToInt(scoreValue*scoreMulti);
        ScorePopUpHUD(scoreValue*scoreMulti);
    }

    public void MultiplyScore(float multipl)
    {
        int result=Mathf.RoundToInt(score * multipl);
        score = result;
    }

    public void AddToScoreNoEV(int scoreValue){score += scoreValue;ScorePopUpHUD(scoreValue);}
    public void AddXP(float xpValue){if(xpOn){coresXp += xpValue;XPPopUpHUD(xpValue);}coresXpTotal+=xpValue;}
    public void AddEnemyCount(){enemiesCount++;if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().EnemiesCountHealDrone++;
    var ps=FindObjectsOfType<PowerupsSpawner>();
    foreach(PowerupsSpawner p in ps){
        if(p.enemiesCountReq!=-1){
            p.enemiesCount++;
        }    
    }}

    public void ResetScore(){
        score=0;
        EVscore = 0;
        shopScore = 0;
        coins = 0;
        coresXp = 0;
        cores = 0;
        enballDropMulti=1;
        coinDropMulti=1;
        coreDropMulti=1;
        rarePwrupMulti=1;
        legendPwrupMulti=1;
        gameSessionTime=0;
    }
    public void SaveHighscore(){
        if(score>SaveSerial.instance.playerData.highscore[GameSession.instance.gameModeSelected]){SaveSerial.instance.playerData.highscore[GameSession.instance.gameModeSelected]=score;}
        if(GameSession.instance.gameModeSelected==0){StartCoroutine(SaveAdventureI());}
        //FindObjectOfType<DataSavable>().highscore = highscore;
    }
    IEnumerator SaveAdventureI(){
        yield return new WaitForSecondsRealtime(0.02f);
        var u=UpgradeMenu.instance;
        var s=SaveSerial.instance;
        if(u!=null&&s!=null&&s.advD!=null){
        s.advD.xp=coresXp;
        s.advD.total_UpgradesCount=u.total_UpgradesCount;
        s.advD.total_UpgradesLvl=u.total_UpgradesLvl;
        s.advD.maxHealth_UpgradesCount=u.maxHealth_UpgradesCount;
        s.advD.maxHealth_UpgradesLvl=u.maxHealth_UpgradesLvl;
        s.advD.maxEnergy_UpgradesCount=u.maxEnergy_UpgradesCount;
        s.advD.maxEnergy_UpgradesLvl=u.maxEnergy_UpgradesLvl;
        s.advD.speed_UpgradesCount=u.speed_UpgradesCount;
        s.advD.speed_UpgradesLvl=u.speed_UpgradesLvl;
        s.advD.hpRegen_UpgradesCount=u.hpRegen_UpgradesCount;
        s.advD.hpRegen_UpgradesLvl=u.hpRegen_UpgradesLvl;
        s.advD.enRegen_UpgradesCount=u.enRegen_UpgradesCount;
        s.advD.enRegen_UpgradesLvl=u.enRegen_UpgradesLvl;
        s.advD.luck_UpgradesCount=u.luck_UpgradesCount;
        s.advD.luck_UpgradesLvl=u.luck_UpgradesLvl;
        yield return new WaitForSecondsRealtime(0.02f);
        s.SaveAdventure();
        Debug.Log("Adventure data saved in GameSession");
        }else{if(u==null){Debug.LogError("UpgradeMenu not present");}else if(s==null){Debug.LogError("SaveSerial not present");}else if(s.advD==null){Debug.LogError("Adventure Data null");}}
    }
    public IEnumerator LoadAdventureI(){
        yield return new WaitForSecondsRealtime(0.04f);
        var u=UpgradeMenu.instance;
        var s=SaveSerial.instance;
        if(u!=null&&s!=null&&s.advD!=null){
        coresXp=s.advD.xp;
        u.total_UpgradesCount=s.advD.total_UpgradesCount;
        u.total_UpgradesLvl=s.advD.total_UpgradesLvl;
        u.maxHealth_UpgradesCount=s.advD.maxHealth_UpgradesCount;
        u.maxHealth_UpgradesLvl=s.advD.maxHealth_UpgradesLvl;
        u.maxEnergy_UpgradesCount=s.advD.maxEnergy_UpgradesCount;
        u.maxEnergy_UpgradesLvl=s.advD.maxEnergy_UpgradesLvl;
        u.speed_UpgradesCount=s.advD.speed_UpgradesCount;
        u.speed_UpgradesLvl=s.advD.speed_UpgradesLvl;
        u.hpRegen_UpgradesCount=s.advD.hpRegen_UpgradesCount;
        u.hpRegen_UpgradesLvl=s.advD.hpRegen_UpgradesLvl;
        u.enRegen_UpgradesCount=s.advD.enRegen_UpgradesCount;
        u.enRegen_UpgradesLvl=s.advD.enRegen_UpgradesLvl;
        u.luck_UpgradesCount=s.advD.luck_UpgradesCount;
        u.luck_UpgradesLvl=s.advD.luck_UpgradesLvl;

        yield return new WaitForSeconds(0.1f);
        if(UpgradeMenu.instance!=null){
            for(var i=UpgradeMenu.instance.total_UpgradesLvl;i>0;i--){
                var g=GameRules.instance;
                var player=FindObjectOfType<Player>();
                if(g!=null&&player!=null){
                if(g.lvlEvents.Length>0){g.lvlEvents[0].Invoke();}
                var d=0;foreach(int id in g.lvlEventsIDs){if(d<g.lvlEventsIDs.Length-1){d++;}if(UpgradeMenu.instance.total_UpgradesLvl==g.lvlEventsIDs[d])g.lvlEvents[d].Invoke();}}
            }
        }
        Debug.Log("Adventure data laoded in GameSession");
        }else{if(u==null){Debug.LogError("UpgradeMenu not present");}else if(s==null){Debug.LogError("SaveSerial not present");}else if(s.advD==null){Debug.LogError("Adventure Data null");}}
    }
    public void SaveSettings(){
        /*var ss=SaveSerial.instance;
        var sm=FindObjectOfType<SettingsMenu>();
        ss.moveByMouse = sm.moveByMouse;
        ss.quality = sm.quality;
        ss.fullscreen = sm.fullscreen;
        ss.scbuttons = sm.scbuttons;
        ss.pprocessing = sm.pprocessing;
        ss.masterVolume = sm.masterVolume;
        ss.soundVolume = sm.soundVolume;
        ss.musicVolume = sm.musicVolume;
        ss.lefthand = sm.lefthand;*/

        SaveSerial.instance.SaveSettings();
    }
    public void SaveInventory(){
        SaveSerial.instance.playerData.skinID = FindObjectOfType<Inventory>().skinID;
        SaveSerial.instance.playerData.chameleonColor[0] = FindObjectOfType<Inventory>().chameleonColorArr[0];
        SaveSerial.instance.playerData.chameleonColor[1] = FindObjectOfType<Inventory>().chameleonColorArr[1];
        SaveSerial.instance.playerData.chameleonColor[2] = FindObjectOfType<Inventory>().chameleonColorArr[2];
    }
    public void Save(){ SaveSerial.instance.Save(); SaveSerial.instance.SaveSettings(); }
    public void Load(){ SaveSerial.instance.Load(); SaveSerial.instance.LoadSettings(); }
    //public void DeleteAllShowConfirm(){ GameObject.Find("OptionsUI").transform.GetChild(0).gameObject.SetActive((false)); GameObject.Find("OptionsUI").transform.GetChild(1).gameObject.SetActive((true)); }
    //public void DeleteAllHideConfirm(){ GameObject.Find("OptionsUI").transform.GetChild(0).gameObject.SetActive((true)); GameObject.Find("OptionsUI").transform.GetChild(1).gameObject.SetActive((false)); }
    public void DeleteAll(){ SaveSerial.instance.Delete();SaveSerial.instance.DeleteAdventure(); ResetSettings(); FindObjectOfType<Level>().LoadStartMenu();//FindObjectOfType<Level>().Restart();
    /*Destroy(SaveSerial.instance.gameObject);Destroy(this.gameObject); SaveSerial.instance=null;GameSession.instance=null;DamageValues.instance=null;*/}
    public void DeleteAdventure(){SaveSerial.instance.DeleteAdventure();}//FindObjectOfType<Level>().LoadStartMenu();}
    
    public void ResetSettings(){
        SaveSerial.instance.ResetSettings();
        FindObjectOfType<Level>().RestartScene();
        SaveSerial.instance.SaveSettings();
        var s=FindObjectOfType<SettingsMenu>();
        /*SaveSerial.instance.settingsData.gameVersion="0.4";
        SaveSerial.instance.settingsData.moveByMouse=true;
        SaveSerial.instance.settingsData.quality=4;
        SaveSerial.instance.settingsData.fullscreen=true;
        SaveSerial.instance.settingsData.masterVolume=0;
        SaveSerial.instance.settingsData.soundVolume=0;
        SaveSerial.instance.settingsData.musicVolume=-25;
        SaveSerial.instance.settingsData.joystickSize=1;*/
    }
    public void ResetMusicPitch(){
        if(FindObjectOfType<MusicPlayer>()!=null)FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().pitch=1;
    }
    float settingsOpenTimer;
    public void CloseSettings(bool goToPause){
    if(GameSession.instance!=null){
        if(SceneManager.GetActiveScene().name=="Options"){if(FindObjectOfType<Level>()!=null)FindObjectOfType<Level>().LoadStartMenu();}
        else if(SceneManager.GetActiveScene().name=="Game"&&PauseMenu.GameIsPaused){if(FindObjectOfType<SettingsMenu>()!=null)FindObjectOfType<SettingsMenu>().Close();if(FindObjectOfType<PauseMenu>()!=null&&goToPause)FindObjectOfType<PauseMenu>().Pause();}
    }}

    void CalculateLuck(){
        //ref float multiAmnt = ref enballMultiAmnt;
        //1+(0.08*((1.05-1)/0.05))
        if(luckMulti<2.5f){
            enballDropMulti=1+(enballMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt));
            //multiAmnt = ref coinMultiAmnt;
            coinDropMulti=1+(coinMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt));
            coreDropMulti=1+(coreMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt));
            rarePwrupMulti=1+(rareMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt));
            legendPwrupMulti=1+(legendMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt));
        }else{
            var enballMultiAmntS=0.003f;
            var coinMultiAmntS=0.004f;
            var coreMultiAmntS=0.002f;
            var rareMultiAmntS=0.0002f;
            var legendMultiAmntS=0.02f;
            enballDropMulti=Mathf.Clamp(1+(enballMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt)),0,3)+enballMultiAmntS*((luckMulti-3)/UpgradeMenu.instance.luck_UpgradeAmnt);
            coinDropMulti=Mathf.Clamp(1+(coinMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt)),0,3)+coinMultiAmntS*((luckMulti-3)/UpgradeMenu.instance.luck_UpgradeAmnt);
            coreDropMulti=Mathf.Clamp(1+(coreMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt)),0,2)+coreMultiAmntS*((luckMulti-2)/UpgradeMenu.instance.luck_UpgradeAmnt);
            rarePwrupMulti=Mathf.Clamp(1+(rareMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt)),0,3)+rareMultiAmntS*((luckMulti-3)/UpgradeMenu.instance.luck_UpgradeAmnt);
            legendPwrupMulti=Mathf.Clamp(1+(legendMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt)),0,3)+legendMultiAmntS*((luckMulti-3)/UpgradeMenu.instance.luck_UpgradeAmnt);
        }

        /*if(enballDropMulti>3){enballMultiAmnt=0.003f;}
        if(coinDropMulti>3){coinMultiAmnt=0.004f;}
        if(coreDropMulti>2){coreMultiAmnt=0.002f;}
        if(rarePwrupMulti>3){rareMultiAmnt=0.0002f;}
        if(legendPwrupMulti>3){legendMultiAmnt=0.01f;}*/
    }
    public void CheckCodes(int fkey, int nkey){
        if(fkey==0&&nkey==0){}
        if(Input.GetKey(KeyCode.Delete) || fkey==-1){
            if(Input.GetKeyDown(KeyCode.Alpha0) || nkey==0){
                cheatmode=true;
            }if(Input.GetKeyDown(KeyCode.Alpha9) || nkey==9){
                cheatmode=false;
            }
        }
        if(cheatmode==true){
            if(Input.GetKey(KeyCode.F1) || fkey==1){
                player=FindObjectOfType<Player>();
                if(Input.GetKeyDown(KeyCode.Alpha1) || nkey==1){player.health=player.maxHP;}
                if(Input.GetKeyDown(KeyCode.Alpha2) || nkey==2){player.energy=player.maxEnergy;}
                if(Input.GetKeyDown(KeyCode.Alpha3) || nkey==3){player.gclover=true;player.gcloverTimer=player.gcloverTime;}
                if(Input.GetKeyDown(KeyCode.Alpha4) || nkey==4){player.health=0;}
            }
            if(Input.GetKey(KeyCode.F2) || fkey==2){
                if(Input.GetKeyDown(KeyCode.Alpha1) || nkey==1){AddToScoreNoEV(100);}
                if(Input.GetKeyDown(KeyCode.Alpha2) || nkey==2){AddToScoreNoEV(1000);}
                if(Input.GetKeyDown(KeyCode.Alpha3) || nkey==3){EVscore=EVscoreMax;}
                if(Input.GetKeyDown(KeyCode.Alpha4) || nkey==4){coins+=1;shopScore=shopScoreMax;}
                if(Input.GetKeyDown(KeyCode.Alpha5) || nkey==5){AddXP(100);}
                if(Input.GetKeyDown(KeyCode.Alpha6) || nkey==6){coins+=100;cores+=100;}
                if(Input.GetKeyDown(KeyCode.Alpha7) || nkey==7){FindObjectOfType<UpgradeMenu>().total_UpgradesLvl+=10;}
                if(Input.GetKeyDown(KeyCode.Alpha8) || nkey==8){foreach(PowerupsSpawner ps in FindObjectsOfType<PowerupsSpawner>())ps.timer=0.01f;}
                if(Input.GetKeyDown(KeyCode.Alpha9) || nkey==9){foreach(PowerupsSpawner ps in FindObjectsOfType<PowerupsSpawner>())ps.enemiesCount=100;}
            }
            if(Input.GetKey(KeyCode.F3) || fkey==3){
                player=FindObjectOfType<Player>();
                if(Input.GetKeyDown(KeyCode.Alpha1) || nkey==1){player.powerup="laser3";}
                if(Input.GetKeyDown(KeyCode.Alpha2) || nkey==2){player.powerup="mlaser";}
                if(Input.GetKeyDown(KeyCode.Alpha3) || nkey==3){player.powerup="lsaber";}
                if(Input.GetKeyDown(KeyCode.Alpha4) || nkey==4){player.powerup="lclaws";}
                if(Input.GetKeyDown(KeyCode.Alpha5) || nkey==5){player.powerup="cstream";}
            }
        }
    }

    public void ScorePopUpHUD(float score){
        GameObject scpopupHud=GameObject.Find("ScoreDiffParrent");
        if(scpopupHud!=null){
        scpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //scpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(score<0)symbol="-";
        scpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=symbol+Mathf.Abs(score).ToString();
        }else{Debug.LogWarning("ScorePopUpHUD not present");}
    }public void XPPopUpHUD(float xp){
        GameObject xppopupHud=GameObject.Find("XPDiffParrent");
        if(xppopupHud!=null){
        xppopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //xppopupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(xp<0)symbol="-";
        xppopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=symbol+Mathf.Abs(xp).ToString();
        }else{Debug.LogWarning("XPPopUpHUD not present");}
    }
    //public void PlayDenySFX(){AudioManager.instance.Play("Deny");}
    public string FormatTime(float time){
        int minutes = (int) time / 60 ;
        int seconds = (int) time - 60 * minutes;
        //int milliseconds = (int) (1000 * (time - minutes * 60 - seconds));
    return string.Format("{0:00}:{1:00}"/*:{2:000}"*/, minutes, seconds/*, milliseconds*/ );
    }
    public string GetGameSessionTimeFormat(){
        return FormatTime(gameSessionTime);
    }public int GetGameSessionTime(){
        return Mathf.RoundToInt(gameSessionTime);
    }
    public void SetGameModeSelected(int i){gameModeSelected=i;}
    public void SetCheatmode(){if(!cheatmode){cheatmode=true;return;}else{cheatmode=false;return;}}
}
