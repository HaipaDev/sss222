using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using BayatGames.SaveGameFree;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Analytics;
//using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class GameSession : MonoBehaviour{
    public static GameSession instance;
    public static bool GlobalTimeIsPaused;
    public static bool GlobalTimeIsPausedNotSlowed;
    [Header("Current Player Values")]
    public int score=0;
    public float scoreMulti=1f;
    public float luckMulti=1f;
    public int coins=0;
    public int cores=0;
    public float xp=0f;
    public float xpTotal=0f;
    public int enemiesCount=0;
    public float stayingTimeXP=0f;
    public float movingTimeXP=0f;
    [Header("GameRules/Event Values")]
    public bool anyUpgradesOn=true;
    /*public int EVscore=0;
    public int EVscoreMax=0;
    public int shopScore=0;
    public int shopScoreMax=0;*/
    public float xpMax=100f;
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
    public float vertCameraSize=7f;
    public float horizCameraSize=3.92f;
    [Header("Other")]
    public string gameVersion="0.5t3";
    public bool cheatmode;
    public bool dmgPopups=true;
    public bool analyticsOn=true;
    public int gamemodeSelected=1;
    public const int gameModeMaxID=4;
    [SerializeField]float restartTimer=-4;
    
    Player player;
    PostProcessVolume postProcessVolume;
    bool setValues;
    public float currentPlaytime=0;
    public float presenceTimer=0;
    public bool presenceTimeSet=false;
    //[SerializeField] InputMaster inputMaster;
    [Range(0,2)]public static int maskMode=1;
    //public string gameVersion;

    void Awake(){
        SetUpSingleton();
        StartCoroutine(SetGameRulesValues());
        #if UNITY_EDITOR
        cheatmode=true;
        #else
        cheatmode=false;
        #endif
    }
    void SetUpSingleton(){if(GameSession.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
    void Start(){
        Array.Clear(SaveSerial.instance.playerData.highscore,0,SaveSerial.instance.playerData.highscore.Length);
        if(SceneManager.GetActiveScene().name=="Game"&&GetComponent<spawnReqsMono>()==null){gameObject.AddComponent<spawnReqsMono>();}

        presenceTimeSet=false;
    }
    IEnumerator SetGameRulesValues(){
    yield return new WaitForSeconds(0.03f);
    var i=GameRules.instance;if(i!=null){
        ///Main
        defaultGameSpeed=i.defaultGameSpeed;gameSpeed=defaultGameSpeed;
        scoreMulti=i.scoreMulti;
        luckMulti=i.luckMulti;
        xpMax=i.xpMax;

        if(GameRules.instance.modulesOn||GameRules.instance.statUpgOn||GameRules.instance.iteminvOn){anyUpgradesOn=true;}else{anyUpgradesOn=false;}

        RandomizeWaveScoreMax();
        RandomizeShopScoreMax();
    }}
    
    public void EnterGameScene(){
        if(GetComponent<spawnReqsMono>()==null){gameObject.AddComponent<spawnReqsMono>();}
        StartCoroutine(SetGameRulesValues());
        RandomizeWaveScoreMax();
        RandomizeShopScoreMax();
        
        if(SaveSerial.instance.settingsData.playfieldRot==PlaneDir.horiz){FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,90);FindObjectOfType<Camera>().orthographicSize=horizCameraSize;}
        else{FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,0);FindObjectOfType<Camera>().orthographicSize=vertCameraSize;}
    }
    public void RandomizeWaveScoreMax(){
        if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;if(sr!=null){if(sr.scoreMaxSetRange.x!=-5&&sr.scoreMaxSetRange.y!=-5)spawnReqsMono.RandomizeScoreMax(-1);}}
    }
    public void RandomizeShopScoreMax(){
        if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;if(sr!=null){if(sr.scoreMaxSetRange.x!=-5&&sr.scoreMaxSetRange.y!=-5)spawnReqsMono.RandomizeScoreMax(-2);}}
    }
    void Update(){
        if(gameSpeed>=0){Time.timeScale=gameSpeed;}if(gameSpeed<0){gameSpeed=0;}
        if(SceneManager.GetActiveScene().name=="Game"){
        if(Time.timeScale<=0.0001f||PauseMenu.GameIsPaused||Shop.shopOpened||UpgradeMenu.UpgradeMenuIsOpen){GlobalTimeIsPaused=true;}else{GlobalTimeIsPaused=false;}
        if(PauseMenu.GameIsPaused||Shop.shopOpened||UpgradeMenu.UpgradeMenuIsOpen){GlobalTimeIsPausedNotSlowed=true;}else{GlobalTimeIsPausedNotSlowed=false;}
        }else{GlobalTimeIsPaused=false;}

        if(SceneManager.GetActiveScene().name=="Game"&&Player.instance!=null&&!GlobalTimeIsPaused){currentPlaytime+=Time.unscaledDeltaTime;}
        if(SceneManager.GetActiveScene().name!="Game"&&setValues==true){setValues=false;}
        
        xp=Mathf.Clamp(xp,0,xpMax);
        cores=Mathf.Clamp(cores,0,99999);
        coins=Mathf.Clamp(coins,0,99999);
        
        if(GameRules.instance!=null&&SceneManager.GetActiveScene().name=="Game"){
            //Open Shop
            

            if(xpTotal<0)xpTotal=0;
            if(GameRules.instance.xpOn&&xp>=xpMax&&GameRules.instance.levelingOn){
                //cores++;
                if(GameRules.instance.coresOn){
                    GameAssets.instance.Make("PowerCore",new Vector2(UnityEngine.Random.Range(-3.5f, 3.5f),7.4f));
                    FindObjectOfType<UpgradeMenu>().total_UpgradesCount++;
                }
                //FindObjectOfType<UpgradeMenu>().total_UpgradesCount++;
                xp=0;
                //AudioManager.instance.Play("LvlUp");
                AudioManager.instance.Play("LvlUp");
                
            }

            if(stayingTimeXP>GameRules.instance.stayingTimeReq){AddXP(GameRules.instance.xp_staying);stayingTimeXP=0f;}
            if(movingTimeXP>GameRules.instance.flyingTimeReq){AddXP(GameRules.instance.xp_flying);movingTimeXP=0f;}
        }

        //Set speed to normal
        if(PauseMenu.GameIsPaused==false&&Shop.shopOpened==false&&UpgradeMenu.UpgradeMenuIsOpen==false&&
        (Player.instance!=null&&Player.instance.matrix==false&&Player.instance.accel==false)&&speedChanged!=true){gameSpeed=defaultGameSpeed;}
        if(SceneManager.GetActiveScene().name!="Game"){gameSpeed=1;}
        if(Player.instance==null){gameSpeed=defaultGameSpeed;}
        
        //Restart with R or Space/Resume with Space
        if(SceneManager.GetActiveScene().name=="Game"){
        if((GameOverCanvas.instance==null||GameOverCanvas.instance.gameOver==false)&&PauseMenu.GameIsPaused==false){restartTimer=-4;}
        if(PauseMenu.GameIsPaused==true){if(restartTimer==-4)restartTimer=0.5f;}
        if(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true){if(restartTimer==-4)restartTimer=1f;}
        else if(PauseMenu.GameIsPaused==true&&Input.GetKeyDown(KeyCode.Space)){FindObjectOfType<PauseMenu>().Resume();}
        if(restartTimer>0)restartTimer-=Time.unscaledDeltaTime;
        if(restartTimer<=0&&restartTimer!=-4){if(Input.GetKeyDown(KeyCode.R)||(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true&&Input.GetKeyDown(KeyCode.Space))){GSceneManager.instance.RestartGame();restartTimer=-4;}}
        if(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true&&Input.GetKeyDown(KeyCode.Escape)){GSceneManager.instance.LoadStartMenu();}
        }

        if((PauseMenu.GameIsPaused==true||Shop.shopOpened==true||UpgradeMenu.UpgradeMenuIsOpen==true)&&(Player.instance!=null&&Player.instance.inverter==true)){
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
        if(Player.instance!=null&&Player.instance.inverter==false){
            foreach(AudioSource sound in AudioManager.instance.GetComponents(typeof(AudioSource))){
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

        if(presenceTimer>0){presenceTimer-=Time.unscaledDeltaTime;}
        if(presenceTimer<=0){
            string presenceDetails="";
            string presenceStatus="";
            string sceneName=SceneManager.GetActiveScene().name;
            string nickname="";if(SaveSerial.instance!=null){if(SaveSerial.instance.hyperGamerLoginData!=null){nickname=SaveSerial.instance.hyperGamerLoginData.username;}}
            string nickInfo="";if(!String.IsNullOrEmpty(nickname))nickInfo=" | "+nickname;
            if(sceneName!="Game"){presenceStatus="In Menus"+nickInfo;presenceDetails="";}
            else{presenceStatus=GameRules.instance.cfgName+nickInfo;presenceDetails="Score: "+score+" | "+"Game Time: "+GetGameSessionTimeFormat();}
            if(presenceTimeSet==false){
                DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                int presenceTimeTotal = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
                DiscordPresence.PresenceManager.UpdatePresence(detail: presenceDetails, state: presenceStatus, start: presenceTimeTotal);
                presenceTimeSet=true;
            }
            DiscordPresence.PresenceManager.UpdatePresence(detail: presenceDetails, state: presenceStatus);

            presenceTimer=1f;
        }

        if(UpgradeMenu.instance!=null)CalculateLuck();
        CheckCodes(".",".");
    }

    public void AddToScore(int scoreValue){
        score+=Mathf.RoundToInt(scoreValue*scoreMulti);
        spawnReqsMono.AddScore(Mathf.RoundToInt(scoreValue*scoreMulti));
        //if(GameRules.instance.shopOn)spawnReqs.AddScore(Mathf.RoundToInt(scoreValue*scoreMulti),-2);
        GameCanvas.instance.ScorePopupSwitch(scoreValue*scoreMulti);
    }
    public void AddToScoreNoEV(int scoreValue){score+=scoreValue;GameCanvas.instance.ScorePopupSwitch(scoreValue);}
    public void MultiplyScore(float multipl){score=Mathf.RoundToInt(score*multipl);}
    public void AddXP(float xpValue){if(GameRules.instance.xpOn){xp+=xpValue;GameCanvas.instance.XpPopupSwitch(xpValue);}xpTotal+=xpValue;}
    public void DropXP(float xpAmnt, Vector2 pos, float rangeX=0.5f, float rangeY=0.5f){
        var amnt=Mathf.RoundToInt(xpAmnt);
        GameAssets.instance.MakeSpread("CelestBall",pos,amnt,rangeX,rangeY);
        if(xpAmnt-amnt!=0)GameSession.instance.AddXP(xpAmnt-amnt);
    }
    public void AddEnemyCount(){
        enemiesCount++;
        spawnReqsMono.AddKills();
        /*if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddKills(1);
        var ps=System.Array.FindAll(FindObjectsOfType<PowerupsSpawner>(),x=>x.powerupSpawnerType==powerupSpawnerType.kills);
        foreach(var p in ps){p.enemiesCount++;}*/
    }

    public void ResetScore(){
        score=0;
        if(!CheckGamemodeSelected("Adventure")){
        coins=0;
        cores=0;
        }
        xp=0;
        xpTotal=0;
        stayingTimeXP=0;
        movingTimeXP=0;
        enballDropMulti=1;
        coinDropMulti=1;
        coreDropMulti=1;
        rarePwrupMulti=1;
        legendPwrupMulti=1;
        currentPlaytime=0;
        if(GetComponent<spawnReqsMono>()!=null)Destroy(GetComponent<spawnReqsMono>());
    }
    public void ResetAfterAdventure(){
        coins=0;
        cores=0;
    }
    public void SaveHighscore(){
        if(CheckGamemodeSelected("Adventure")){SaveAdventure();}
        if(gamemodeSelected>=0&&gamemodeSelected<SaveSerial.instance.playerData.highscore.Length){
            if(score>GetHighscoreCurrent()){SaveSerial.instance.playerData.highscore[GameSession.instance.gamemodeSelected-1]=score;}
        }else if(gamemodeSelected>=SaveSerial.instance.playerData.highscore.Length){Debug.LogWarning("Score not submittable for this gamemode");}
        StatsAchievsManager.instance.AddScoreTotal(score);
        StatsAchievsManager.instance.AddPlaytime(GetGameSessionTime());
    }
    public void SaveAdventure(){StartCoroutine(SaveAdventureI());}
    IEnumerator SaveAdventureI(){
        //next steps in SaveSerial
        yield return new WaitForSecondsRealtime(0.02f);
        var u=UpgradeMenu.instance;
        var s=SaveSerial.instance;
        var ss=SaveSerial.instance.advD;
        if(s==null){Debug.LogError("SaveSerial not present");}
        if(ss!=null){
            ss.coins=coins;
            ss.cores=cores;
            if(u!=null){
            if(ss.total_UpgradesLvl>=u.saveBarsFromLvl){ss.total_UpgradesCount=u.total_UpgradesCount;}
            ss.total_UpgradesLvl=u.total_UpgradesLvl;
            ss.healthMax_UpgradesCount=u.healthMax_UpgradesCount;
            ss.healthMax_UpgradesLvl=u.healthMax_UpgradesLvl;
            ss.energyMax_UpgradesCount=u.energyMax_UpgradesCount;
            ss.energyMax_UpgradesLvl=u.energyMax_UpgradesLvl;
            ss.speed_UpgradesCount=u.speed_UpgradesCount;
            ss.speed_UpgradesLvl=u.speed_UpgradesLvl;
            ss.luck_UpgradesCount=u.luck_UpgradesCount;
            ss.luck_UpgradesLvl=u.luck_UpgradesLvl;
            //
            ss.mPulse_upgraded=u.mPulse_upgraded;
            ss.teleport_upgraded=u.teleport_upgraded;
            ss.crMend_upgraded=u.crMend_upgraded;
            ss.enDiss_upgraded=u.enDiss_upgraded;
            yield return new WaitForSecondsRealtime(0.02f);
            Debug.Log("Adventure data saved in GameSession");
            yield return new WaitForSecondsRealtime(0.033f);
            SaveSerial.instance.SaveAdventure();
            }else{Debug.LogError("UpgradeMenu not present");}
        }else{Debug.LogError("Adventure Data null");}
    }
    //LoadAdventure() in GSceneManager.cs
    public void LoadAdventurePre(){
        SaveSerial.instance.LoadAdventure();
        var ss=SaveSerial.instance.advD;
        coins=ss.coins;
        cores=ss.cores;
    }
    public void LoadAdventurePost(){StartCoroutine(LoadAdventureI());}
    IEnumerator LoadAdventureI(){
        //First load from SaveSerial
        yield return new WaitForSecondsRealtime(0.04f);
        var u=UpgradeMenu.instance;
        var s=SaveSerial.instance;
        var ss=SaveSerial.instance.advD;
        if(u!=null&&s!=null&&s.advD!=null){
        u.total_UpgradesCount=ss.total_UpgradesCount;
        u.total_UpgradesLvl=ss.total_UpgradesLvl;
        u.healthMax_UpgradesCount=ss.healthMax_UpgradesCount;
        u.healthMax_UpgradesLvl=ss.healthMax_UpgradesLvl;
        u.energyMax_UpgradesCount=ss.energyMax_UpgradesCount;
        u.energyMax_UpgradesLvl=ss.energyMax_UpgradesLvl;
        u.speed_UpgradesCount=ss.speed_UpgradesCount;
        u.speed_UpgradesLvl=ss.speed_UpgradesLvl;
        u.luck_UpgradesCount=ss.luck_UpgradesCount;
        u.luck_UpgradesLvl=ss.luck_UpgradesLvl;
        //
        u.mPulse_upgraded=ss.mPulse_upgraded;
        u.teleport_upgraded=ss.teleport_upgraded;
        u.crMend_upgraded=ss.crMend_upgraded;
        u.enDiss_upgraded=ss.enDiss_upgraded;

        yield return new WaitForSeconds(0.1f);
        if(UpgradeMenu.instance!=null){
            for(var i=UpgradeMenu.instance.total_UpgradesLvl;i>0;i--){
                UpgradeMenu.instance.LvlEvents();
            }
        }
        Debug.Log("Adventure data loaded in GameSession");
        }else{if(u==null){Debug.LogError("UpgradeMenu not present");}else if(s==null){Debug.LogError("SaveSerial not present");}else if(s.advD==null){Debug.LogError("Adventure Data null");}}
    }
    public void SaveSettings(){SaveSerial.instance.SaveSettings();}
    public void SaveInventory(){
        SaveSerial.instance.playerData.skinName=FindObjectOfType<CustomizationInventory>().skinName;
        SaveSerial.instance.playerData.overlayColor[0]=FindObjectOfType<CustomizationInventory>().overlayColorArr[0];
        SaveSerial.instance.playerData.overlayColor[1]=FindObjectOfType<CustomizationInventory>().overlayColorArr[1];
        SaveSerial.instance.playerData.overlayColor[2]=FindObjectOfType<CustomizationInventory>().overlayColorArr[2];
    }
    public void DeleteAll(){SaveSerial.instance.Delete();SaveSerial.instance.DeleteAdventure();DeleteStatsAchievs();ResetSettings();GSceneManager.instance.LoadStartMenu();}
    public void DeleteAdventure(){SaveSerial.instance.DeleteAdventure();}
    public void ResetSettings(){
        SaveSerial.instance.ResetSettings();
        GSceneManager.instance.RestartScene();
        SaveSerial.instance.SaveSettings();
    }
    public void DeleteStatsAchievs(){
        GameObject sa=FindObjectOfType<StatsAchievsManager>().gameObject;
        Destroy(sa.GetComponent<StatsAchievsManager>());
        sa.AddComponent<StatsAchievsManager>();
        SaveSerial.instance.DeleteStats();
    }
    public void ResetMusicPitch(){
        if(MusicPlayer.instance!=null)MusicPlayer.instance.GetComponent<AudioSource>().pitch=1;
    }
    float settingsOpenTimer;
    public void CloseSettings(bool goToPause){
        if(SceneManager.GetActiveScene().name=="Options"){GSceneManager.instance.LoadStartMenu();}
        else if(SceneManager.GetActiveScene().name=="Game"&&PauseMenu.GameIsPaused){if(FindObjectOfType<SettingsMenu>()!=null)FindObjectOfType<SettingsMenu>().Close();if(FindObjectOfType<PauseMenu>()!=null&&goToPause)FindObjectOfType<PauseMenu>().Pause();}
    }

    public void SetAnalytics(){
        if(analyticsOn==true){
        AnalyticsResult analyticsResult=Analytics.CustomEvent("Death",
        new Dictionary<string,object>{
            { "Mode: ", GameRules.instance.cfgName },
            { "Score: ", score },
            { "Time: ", GetGameSessionTime() },
            { "Source: ", Player.instance.GetComponent<PlayerCollider>().lastHitObj },
            { "Damage: ", Player.instance.GetComponent<PlayerCollider>().lastHitDmg },
            { "Full Report: ", GameRules.instance.cfgName+", "+score+", "+instance.GetGameSessionTimeFormat()+", "+Player.instance.GetComponent<PlayerCollider>().lastHitObj+", "+Player.instance.GetComponent<PlayerCollider>().lastHitDmg }
        });
        Debug.Log("analyticsResult: "+analyticsResult);
        Debug.Log("Full Report: "+GameRules.instance.cfgName+", "+score+", "+instance.GetGameSessionTimeFormat()+", "+Player.instance.GetComponent<PlayerCollider>().lastHitObj+", "+Player.instance.GetComponent<PlayerCollider>().lastHitDmg);
        }
    }

    public void DieAdventure(){
        coins/=2;
        cores/=3;
    }

    void CalculateLuck(){
        if(luckMulti<2.5f){
            enballDropMulti=1+(enballMultiAmnt*((luckMulti-1)/UpgradeMenu.instance.luck_UpgradeAmnt));
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
    }
    public void CheckCodes(string fkey, string nkey){
        //if(fkey=="0"&&nkey=="0"){}
        if(Input.GetKey(KeyCode.Delete) || fkey=="Del"){
            if(Input.GetKeyDown(KeyCode.Alpha0) || nkey=="0"){
                cheatmode=true;
                AudioManager.instance.Play("LvlUp");
            }if(Input.GetKeyDown(KeyCode.Alpha9) || nkey=="9"){
                cheatmode=false;
                AudioManager.instance.Play("Death");
            }
        }
        if(cheatmode==true){
            if(Input.GetKey(KeyCode.Alpha1) || fkey=="1"){
                player=Player.instance;
                if(Input.GetKeyDown(KeyCode.Q) || nkey=="Q"){player.health=player.healthMax;}
                if(Input.GetKeyDown(KeyCode.W) || nkey=="W"){player.energy=player.energyMax;}
                if(Input.GetKeyDown(KeyCode.E) || nkey=="E"){player.gclover=true;player.gcloverTimer=player.gcloverTime;}
                if(Input.GetKeyDown(KeyCode.R) || nkey=="R"){player.health=0;}
            }
            if(Input.GetKey(KeyCode.Alpha2) || fkey=="2"){
                if(Input.GetKeyDown(KeyCode.Q) || nkey=="Q"){AddToScoreNoEV(100);}
                if(Input.GetKeyDown(KeyCode.W) || nkey=="W"){AddToScoreNoEV(1000);}
                if(Input.GetKeyDown(KeyCode.E) || nkey=="E"){FindObjectOfType<Waves>().StartCoroutine(FindObjectOfType<Waves>().RandomizeWave());}//spawnReqsMono.AddScore(-5,-1);}
                if(Input.GetKeyDown(KeyCode.R) || nkey=="R"){
                    spawnReqsMono.AddScore(-5,-2);
                }
                if(Input.GetKeyDown(KeyCode.T) || nkey=="T"){AddXP(100);}
                if(Input.GetKeyDown(KeyCode.Y) || nkey=="Y"){coins+=100;cores+=100;}
                if(Input.GetKeyDown(KeyCode.U) || nkey=="U"){FindObjectOfType<UpgradeMenu>().total_UpgradesLvl+=10;}
            }
            if(Input.GetKey(KeyCode.Alpha3) || fkey==""){
                player=Player.instance;
                if(Input.GetKeyDown(KeyCode.Q) || nkey=="Q"){}//var ps=Array.FindAll(FindObjectsOfType<PowerupsSpawner>(),x=>x.powerupsSpawner.spawnReqsType==spawnReqsType.time);foreach(var p in ps){p.powerupsSpawner.timer=0.01f;}}
                if(Input.GetKeyDown(KeyCode.W) || nkey=="W"){}//var ps=Array.FindAll(FindObjectsOfType<PowerupsSpawner>(),x=>x.powerupsSpawner.spawnReqsType==spawnReqsType.kills);foreach(var p in ps){p.powerupsSpawner.enemiesCount=9999999;}}
                if(Input.GetKeyDown(KeyCode.E) || nkey=="E"){player.powerup="laser3";}
                if(Input.GetKeyDown(KeyCode.R) || nkey=="R"){player.powerup="mlaser";}
                if(Input.GetKeyDown(KeyCode.T) || nkey=="T"){player.powerup="lsaber";}
                if(Input.GetKeyDown(KeyCode.Y) || nkey=="Y"){player.powerup="cstream";}
                if(Input.GetKeyDown(KeyCode.U) || nkey=="U"){player.powerup="plaser";}
            }
        }
    }
    public string FormatTime(float time){
        int minutes=(int) time / 60;
        int seconds=(int) time - (60*minutes);
        //int milliseconds=(int) (1000 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}"/*:{2:000}"*/, minutes, seconds/*, milliseconds*/ );
    }
    public string FormatTimeWithHours(float time){
        int hours=(int) time / 3600;
        int minutes=(int) time / 60;
        int seconds=(int) time -(60*minutes);
        minutes=(int) time/60 -(60*hours);
        return string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
    }
    public string GetGameSessionTimeFormat(){return FormatTime(currentPlaytime);}
    public int GetGameSessionTime(){return Mathf.RoundToInt(currentPlaytime);}

    public void SetGamemodeSelected(int i){gamemodeSelected=i;}
    public void SetGamemodeSelectedStr(string name){int i=0;i=Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name))+1;
        if(i==0){i=Array.FindIndex(GameCreator.instance.adventureZonesPrefabs,e=>e.cfgName.Contains(name));}gamemodeSelected=i;}
    public bool CheckGamemodeSelected(string name){bool selected=false;if(gamemodeSelected>0&&gamemodeSelected==Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name))+1){selected=true;}
        else if(gamemodeSelected<0&&Mathf.Abs(gamemodeSelected)+1==Array.FindIndex(GameCreator.instance.adventureZonesPrefabs,e=>e.cfgName.Contains(name))){selected=true;}return selected;}
    public int GetGamemodeID(string name){return Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name));}
    public int GetGamemodeIDM1(string name){return Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name))-1;}
    public int GetGamemodeIDCurrentM1(){return gamemodeSelected-1;}
    /*public int GetGamemodeID(string name){int i=0;i=Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name));
        if(i==0){i=Array.FindIndex(GameCreator.instance.adventureZonesPrefabs,e=>e.cfgName.Contains(name))+1;}return i;}
    public string GetGamemodeName(int id){string n="";if(id>0&&gamemodeSelected<GameCreator.instance.gamerulesetsPrefabs.Length+1){n=GameCreator.instance.gamerulesetsPrefabs[id].cfgName;}
        else if(gamemodeSelected<0&&Mathf.Abs(gamemodeSelected)<GameCreator.instance.adventureZonesPrefabs.Length){n=GameCreator.instance.adventureZonesPrefabs[Mathf.Abs(id)].cfgName;}return n;}*/
    public GameRules GetGameRulesCurrent(){
        GameRules gr=null;
        if(gamemodeSelected>0){gr=GameCreator.instance.gamerulesetsPrefabs[GetGamemodeIDCurrentM1()];}
        else if(gamemodeSelected<0){gr=GameCreator.instance.adventureZonesPrefabs[Mathf.Abs(gamemodeSelected)-1];}
        return gr;
    }
    public string GetCurrentGamemodeName(){string n="";if(gamemodeSelected>0&&gamemodeSelected<GameCreator.instance.gamerulesetsPrefabs.Length+1){n=GameCreator.instance.gamerulesetsPrefabs[gamemodeSelected-1].cfgName;}
        else if(gamemodeSelected<0&&Mathf.Abs(gamemodeSelected)<GameCreator.instance.adventureZonesPrefabs.Length){n=GameCreator.instance.adventureZonesPrefabs[Mathf.Abs(gamemodeSelected)].cfgName;}return n;}

        
    //public int GetHighscore(int i){return SaveSerial.instance.playerData.highscore[i];}
    public int GetHighscoreByName(string str){int i=0;if(SaveSerial.instance.playerData.highscore.Length>GetGamemodeID(str)){i=SaveSerial.instance.playerData.highscore[GetGamemodeID(str)];}return i;}
    public int GetHighscoreCurrent(){int i=0;if(SaveSerial.instance.playerData.highscore.Length>GetGamemodeIDCurrentM1()){i=SaveSerial.instance.playerData.highscore[GetGamemodeIDCurrentM1()];}return i;}
    public void SetCheatmode(){if(!cheatmode){cheatmode=true;return;}else{cheatmode=false;return;}}
}
public enum dir{up,down,left,right}