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

public class GameSession : MonoBehaviour{   public static GameSession instance;
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
    //public bool slowingPause;
    public float vertCameraSize=7f;
    public float horizCameraSize=3.92f;
    [Header("Other")]
    public string gameVersion;
	public float buildVersion;
    [SerializeField][ReadOnly] string _tempSandboxSaveName;
    [SerializeField][ReadOnly] string _selectedUsersDataName;
    public bool isSteam=true;
    public bool steamAchievsStatsLeaderboards=true;
    public bool cheatmode;
    public bool dmgPopups=true;
    public bool analyticsOn=true;
    public int gamemodeSelected=1;
    public const int gameModeMaxID=4;
    [SerializeField]float restartTimer=-4;
    
    PostProcessVolume postProcessVolume;
    bool setValues;
    public float currentPlaytime=0;
    public float presenceTimer=0;
    public bool presenceTimeSet=false;
    //[SerializeField] InputMaster inputMaster;
    [Range(0,2)]public static int maskMode=1;

    void Awake(){
        SetUpSingleton();
        StartCoroutine(SetGameRulesValues());
        #if UNITY_EDITOR
        cheatmode=true;
        #else
        cheatmode=false;
        #endif
        gameObject.AddComponent<gitignoreScript>();
    }
    void SetUpSingleton(){if(GameSession.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
    void Start(){
        if(SceneManager.GetActiveScene().name=="Game"){ReAddSpawnReqsMono();}
        else if(SceneManager.GetActiveScene().name!="Game"){RemoveSpawnReqsMono();}

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
        ReAddSpawnReqsMono();
        if(GameRules.instance.cfgName.Contains("Sandbox Mode"))SetTempSandboxSaveName();
        StartCoroutine(SetGameRulesValues());
        RandomizeWaveScoreMax();
        RandomizeShopScoreMax();
        
        if(SaveSerial.instance.settingsData.playfieldRot==PlaneDir.horiz){FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,90);FindObjectOfType<Camera>().orthographicSize=horizCameraSize;}
        else{FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,0);FindObjectOfType<Camera>().orthographicSize=vertCameraSize;}
    }
    public void SetTempSandboxSaveName(){if(SandboxCanvas.instance!=null)_tempSandboxSaveName=SandboxCanvas.instance.saveSelected;else Debug.LogWarning("No SandboxCanvas instance!");}
    public void ResetTempSandboxSaveName(){_tempSandboxSaveName="";}
    public string GetTempSandboxSaveName(){return _tempSandboxSaveName;}
    public void SetSelectedUsersDataName(string str){_selectedUsersDataName=str;}
    public void ResetSelectedUsersDataName(){_selectedUsersDataName="";}
    public string GetSelectedUsersDataName(){return _selectedUsersDataName;}
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
        (Player.instance!=null&&!Player.instance._hasStatus("matrix")&&!Player.instance._hasStatus("accel"))&&speedChanged!=true){gameSpeed=defaultGameSpeed;}
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
        if(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true&&Input.GetKeyDown(KeyCode.Escape)){GSceneManager.instance.LoadStartMenuGame();}
        }

        if((PauseMenu.GameIsPaused==true||Shop.shopOpened==true||UpgradeMenu.UpgradeMenuIsOpen==true)&&(Player.instance!=null&&Player.instance._hasStatus("inverter"))){
            foreach(AudioSource sound in FindObjectsOfType<AudioSource>()){
                if(sound!=null){
                    GameObject snd=sound.gameObject;
                    //if(sound!=Jukebox){
                    if(snd.GetComponent<Jukebox>()==null){
                        //sound.pitch=1;
                        sound.Stop();
                    }
                }
            }
        }
        if(Player.instance!=null&&!Player.instance._hasStatus("inverter")){
            foreach(AudioSource sound in AudioManager.instance.GetComponents(typeof(AudioSource))){
                if(sound!=null){
                    GameObject snd=sound.gameObject;
                    //if(sound!=Jukebox){
                    if(snd.GetComponent<Jukebox>()==null){
                        if(sound.pitch==-1)sound.pitch=1;
                        if(sound.loop==true)sound.loop=false;
                    }
                }
            }
        }

        //Postprocessing
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance!=null&&postProcessVolume!=null){
            if(SaveSerial.instance.settingsData.pprocessing==true){postProcessVolume.GetComponent<PostProcessVolume>().enabled=true;}
            else{postProcessVolume.GetComponent<PostProcessVolume>().enabled=false;}
        }

        if(presenceTimer>0){presenceTimer-=Time.unscaledDeltaTime;}
        if(presenceTimer<=0){
            string presenceDetails="";
            string presenceStatus="";
            string _prefixDetails="",_suffixDetails="";
            string _prefixStatus="",_suffixStatus="";
            #if UNITY_EDITOR
            _prefixStatus="DEVELOPING | ";
            #endif
            string sceneName=SceneManager.GetActiveScene().name;
            string nickname="";if(SaveSerial.instance!=null){if(SaveSerial.instance.hyperGamerLoginData!=null){nickname=SaveSerial.instance.hyperGamerLoginData.username;}}
            string nickInfo="";if(!String.IsNullOrEmpty(nickname))nickInfo=" | "+nickname;
            if(sceneName!="Game"){
                if(sceneName=="SandboxMode"){presenceStatus=_prefixStatus+"Creating a gamemode"+nickInfo+_suffixStatus;}
                else if(sceneName=="Customization"){presenceStatus=_prefixStatus+"Customizing"+nickInfo+_suffixStatus;}
                else{presenceStatus=_prefixStatus+"In Menus"+nickInfo+_suffixStatus;}
                presenceDetails=_prefixDetails+""+_suffixDetails;
            }else{
                presenceDetails=_prefixDetails+"Score: "+score+" | "+"Game Time: "+GetGameSessionTimeFormat()+_suffixDetails;
                presenceStatus=_prefixStatus+GameRules.instance.cfgName+nickInfo+_suffixStatus;
            }
            
            if(presenceTimeSet==false){
                DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                int presenceTimeTotal = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
                DiscordPresence.PresenceManager.UpdatePresence(detail: presenceDetails, state: presenceStatus, start: presenceTimeTotal);
                presenceTimeSet=true;
            }
            DiscordPresence.PresenceManager.UpdatePresence(detail: presenceDetails, state: presenceStatus);

            presenceTimer=1f;
        }

        //Check if using any GamePad
        if(Input.GetKeyDown(KeyCode.JoystickButton0)){if(SaveSerial.instance!=null)if(SaveSerial.instance.settingsData.inputType!=InputType.keyboard){SaveSerial.instance.settingsData.inputType=InputType.keyboard;}}

        if(!isSteam){steamAchievsStatsLeaderboards=false;}

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
        if(!CheckGamemodeSelected("Adventure")){ResetAfterAdventure();}
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
        RemoveSpawnReqsMono();
    }
    public void ResetAfterAdventure(){
        coins=0;
        cores=0;
    }
    public void SaveHighscore(){
        if(CheckGamemodeSelected("Adventure")){SaveAdventure();}
        if(gamemodeSelected>0&&(gamemodeSelected-1)<SaveSerial.instance.playerData.highscore.Length){
            if(score>GetHighscoreCurrent().score){
                SaveSerial.instance.playerData.highscore[GameSession.instance.gamemodeSelected-1]=new Highscore(){score=score,playtime=Mathf.Round(currentPlaytime),
                version=gameVersion,build=(float)System.Math.Round(buildVersion,2),
                date=DateTime.Now};
                Debug.Log("Highscore set for: "+GetCurrentGamemodeName());
            }
        }else if((gamemodeSelected-1)>=SaveSerial.instance.playerData.highscore.Length||gamemodeSelected<=0){Debug.LogWarning("Score not submittable for this gamemode");}
        StatsAchievsManager.instance.AddScoreTotal(score);
        StatsAchievsManager.instance.AddPlaytime(GetGameSessionTime());
        StatsAchievsManager.instance.SumStatsTotal();
        StatsAchievsManager.instance.SetSteamStats();
        if(SaveSerial.instance.settingsData.autosubmitScores)SubmitScore.SubmitScoreFunc();
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
    public void DeleteAll(){DeleteStatsAchievs();ResetSettings();SaveSerial.instance.Delete();SaveSerial.instance.DeleteAdventure();GSceneManager.instance.LoadStartMenu();}
    public void DeleteAdventure(){SaveSerial.instance.DeleteAdventure();}
    public void ResetSettings(){
        SaveSerial.instance.ResetSettings();
        GSceneManager.instance.ReloadScene();
        SaveSerial.instance.SaveSettings();
    }
    public void DeleteStatsAchievs(){
        StatsAchievsManager.instance.ResetStatsAchievs();
        SaveSerial.instance.DeleteStats();
    }
    public void ResetMusicPitch(){
        if(Jukebox.instance!=null)Jukebox.instance.GetComponent<AudioSource>().pitch=1;
    }
    float settingsOpenTimer;
    public void SetAnalytics(){
        string _phasing="";string _sent="";
        if(Player.instance.GetComponent<PlayerCollider>()._LastHitPhasing()){_phasing=" (Phase)";}
        string FullReport(){
            return GameRules.instance.cfgName+", "+score+", "+instance.GetGameSessionTimeFormat()+", "
            +Player.instance.GetComponent<PlayerCollider>()._LastHitName()+_phasing+", "
            +Player.instance.GetComponent<PlayerCollider>()._LastHitDmg()+", "
            +Player.instance.GetComponent<PlayerCollider>()._LastHp();
        }
        #if UNITY_EDITOR
            analyticsOn=false;
        #endif
        if(analyticsOn){
            _sent=" (Sent)";
            AnalyticsResult analyticsResult=Analytics.CustomEvent("Death",
            new Dictionary<string,object>{
                { "Mode: ", GameRules.instance.cfgName },
                { "Score: ", score },
                { "Time: ", GetGameSessionTime() },
                { "Source: ", Player.instance.GetComponent<PlayerCollider>()._LastHitName()+_phasing },
                { "Damage: ", Player.instance.GetComponent<PlayerCollider>()._LastHitDmg() },
                { "LastHP: ", Player.instance.GetComponent<PlayerCollider>()._LastHp() },
                { "Full Report: ", FullReport() }
            });
            Debug.Log("analyticsResult: "+analyticsResult);
        }
        Debug.Log("Full Report"+_sent+": "+FullReport());
    }

    public void DieAdventure(){
        coins/=2;
        cores/=3;
    }

    void CalculateLuck(){
        if(luckMulti<2.5f){
            enballDropMulti=1+(enballMultiAmnt*(luckMulti-1));
            coinDropMulti=1+(coinMultiAmnt*(luckMulti-1));
            coreDropMulti=1+(coreMultiAmnt*(luckMulti-1));
            rarePwrupMulti=1+(rareMultiAmnt*(luckMulti-1));
            legendPwrupMulti=1+(legendMultiAmnt*(luckMulti-1));
        }else{
            var enballMultiAmntS=0.003f;
            var coinMultiAmntS=0.004f;
            var coreMultiAmntS=0.002f;
            var rareMultiAmntS=0.0002f;
            var legendMultiAmntS=0.02f;
            enballDropMulti=Mathf.Clamp(1+(enballMultiAmnt*(luckMulti-1)),0,3)+enballMultiAmntS*((luckMulti-3));
            coinDropMulti=Mathf.Clamp(1+(coinMultiAmnt*(luckMulti-1)),0,3)+coinMultiAmntS*((luckMulti-3));
            coreDropMulti=Mathf.Clamp(1+(coreMultiAmnt*(luckMulti-1)),0,2)+coreMultiAmntS*((luckMulti-2));
            rarePwrupMulti=Mathf.Clamp(1+(rareMultiAmnt*(luckMulti-1)),0,3)+rareMultiAmntS*((luckMulti-3));
            legendPwrupMulti=Mathf.Clamp(1+(legendMultiAmnt*(luckMulti-1)),0,3)+legendMultiAmntS*((luckMulti-3));
        }
    }
    public void CheckCodes(string fkey, string nkey){gitignoreScript.instance.CheckCodes(fkey, nkey);}
    public static string FormatTime(float time){
        int minutes=(int) time / 60;
        int seconds=(int) time - (60*minutes);
        //int milliseconds=(int) (1000 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}"/*:{2:000}"*/, minutes, seconds/*, milliseconds*/ );
    }
    public static string FormatTimeWithHours(float time){
        int hours=(int) time / 3600;
        int minutes=(int) time / 60;
        int seconds=(int) time -(60*minutes);
        minutes=(int) time/60 -(60*hours);
        return string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
    }
    public string GetGameSessionTimeFormat(){return FormatTime(currentPlaytime);}
    public int GetGameSessionTime(){return Mathf.RoundToInt(currentPlaytime);}

    public void SetGamemodeSelected(int i){gamemodeSelected=i;}
    public void SetGamemodeSelectedStr(string name){int i=0;
        if(!name.Contains("Sandbox Mode")){
            i=Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name))+1;
            if(i==0){i=Array.FindIndex(GameCreator.instance.adventureZonesPrefabs,e=>e.cfgName.Contains(name));}
            if(i!=0){gamemodeSelected=i;}
            else{Debug.LogWarning("Cant find GameMode by name: "+name);}
        }else{gamemodeSelected=0;}
    }
    public bool CheckGamemodeSelected(string name){
        return (
            (gamemodeSelected>0&&gamemodeSelected==Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name))+1)
            ||(gamemodeSelected<0&&Mathf.Abs(gamemodeSelected)+1==Array.FindIndex(GameCreator.instance.adventureZonesPrefabs,e=>e.cfgName.Contains(name)))
            ||(gamemodeSelected==0&&GameRules.instance.cfgName.Contains(name))
        );}
    public int GetGamemodeID(string name){return Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name));}
    public GameRules GetGameRules(string name){return Array.Find(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name));}
    /*public int GetGamemodeID(string name){int i=0;i=Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name));
        if(i==0){i=Array.FindIndex(GameCreator.instance.adventureZonesPrefabs,e=>e.cfgName.Contains(name))+1;}return i;}
    public string GetGamemodeName(int id){string n="";if(id>0&&gamemodeSelected<GameCreator.instance.gamerulesetsPrefabs.Length+1){n=GameCreator.instance.gamerulesetsPrefabs[id].cfgName;}
        else if(gamemodeSelected<0&&Mathf.Abs(gamemodeSelected)<GameCreator.instance.adventureZonesPrefabs.Length){n=GameCreator.instance.adventureZonesPrefabs[Mathf.Abs(id)].cfgName;}return n;}*/
    public GameRules GetGameRulesCurrent(){
        GameRules gr=null;
        if(gamemodeSelected>0){gr=GameCreator.instance.gamerulesetsPrefabs[gamemodeSelected-1];}
        else if(gamemodeSelected<0){gr=GameCreator.instance.adventureZonesPrefabs[Mathf.Abs(gamemodeSelected)-1];}
        return gr;
    }
    public string GetCurrentGamemodeName(){
        string n="";if(gamemodeSelected>0&&gamemodeSelected<GameCreator.instance.gamerulesetsPrefabs.Length+1){n=GameCreator.instance.gamerulesetsPrefabs[gamemodeSelected-1].cfgName;}
        else if(gamemodeSelected<0&&Mathf.Abs(gamemodeSelected)<GameCreator.instance.adventureZonesPrefabs.Length){n=GameCreator.instance.adventureZonesPrefabs[Mathf.Abs(gamemodeSelected)].cfgName;}
        else if(gamemodeSelected==0){n="Sandbox Mode";}
        return n;}
    public bool _isSandboxMode(){return (SceneManager.GetActiveScene().name.Contains("Sandbox")||(SceneManager.GetActiveScene().name=="Game"&&GetCurrentGamemodeName().Contains("Sandbox")));}

        
    //public int GetHighscore(int i){return SaveSerial.instance.playerData.highscore[i];}
    public Highscore GetHighscoreByName(string str){if(SaveSerial.instance.playerData.highscore.Length>GetGamemodeID(str)){return SaveSerial.instance.playerData.highscore[GetGamemodeID(str)];}return null;}
    public Highscore GetHighscoreCurrent(){if(gamemodeSelected>0){if(SaveSerial.instance.playerData.highscore.Length>gamemodeSelected-1){return SaveSerial.instance.playerData.highscore[gamemodeSelected-1];}}return null;}
    public void SetCheatmode(){cheatmode=!cheatmode;return;}
    public void ReAddSpawnReqsMono(){RemoveSpawnReqsMono();AddSpawnReqsMono();}
    public void AddSpawnReqsMono(){if(GetComponent<spawnReqsMono>()==null){gameObject.AddComponent<spawnReqsMono>();}}
    public void RemoveSpawnReqsMono(){if(GetComponent<spawnReqsMono>()!=null){Destroy(GetComponent<spawnReqsMono>());}}
    public void ResetAndRemoveSpawnReqsMono(){if(GetComponent<spawnReqsMono>()!=null){spawnReqsMono.RestartAllValues();spawnReqsMono.ResetSpawnReqsList();ReAddSpawnReqsMono();}}
}

public enum dir{up,down,left,right}
public enum hAlign{left,right}
public enum vAlign{up,down}

public enum InputType{mouse,keyboard,touch,drag}
public enum PlaneDir{vert,horiz}