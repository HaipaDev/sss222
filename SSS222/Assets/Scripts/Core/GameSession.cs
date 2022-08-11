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
    [HideIf("@this.isSteam==false")]public bool steamAchievsStatsLeaderboards=true;
    public bool cheatmode;
    public bool dmgPopups=true;
    public bool analyticsOn=true;
    public int gamemodeSelected=1;
    [HideIf("@this.gamemodeSelected!=-1")]public int zoneSelected=-1;
    [HideIf("@this.gamemodeSelected!=-1")]public int zoneToTravelTo=-1;
    [HideIf("@this.gamemodeSelected!=-1")]public float gameTimeLeft=-4;
    public const int gameModeMaxID=4;
    [SerializeField]float restartTimer=-4;
    
    PostProcessVolume postProcessVolume;
    bool setValues;
    public float currentPlaytime=0;
    public float presenceTimer=0;
    public bool presenceTimeSet=false;
    public float _preBossMusicVolume;
    public bool _adventureLoading;
    public bool _lvlEventsLoading;
    bool _coreSpawnedPreAscend;
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

        if(GameRules.instance.modulesOn||GameRules.instance.statUpgOn/*||GameRules.instance.iteminvOn*/){anyUpgradesOn=true;}else{anyUpgradesOn=false;}

        RandomizeWaveScoreMax();
        RandomizeShopScoreMax();
    }}
    
    public void EnterGameScene(){
        //Debug.Log("PRE: "+PauseMenu.GameIsPaused+" | "+UpgradeMenu.UpgradeMenuIsOpen+" | "+GameSession.GlobalTimeIsPaused);
        StartCoroutine(ForceUnpauseI());
        /*if(PauseMenu.instance!=null){PauseMenu.instance.Resume();}
        if(UpgradeMenu.instance!=null){UpgradeMenu.instance.Resume();}
        Debug.Log("POST: "+PauseMenu.GameIsPaused+" | "+UpgradeMenu.UpgradeMenuIsOpen);*/

        if(GameRules.instance.cfgName.Contains("Sandbox Mode"))SetTempSandboxSaveName();
        ReAddSpawnReqsMono();
        StartCoroutine(SetGameRulesValues());
        RandomizeWaveScoreMax();
        RandomizeShopScoreMax();
        
        if(SaveSerial.instance.settingsData.playfieldRot==PlaneDir.horiz){FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,90);FindObjectOfType<Camera>().orthographicSize=horizCameraSize;}
        else{FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,0);FindObjectOfType<Camera>().orthographicSize=vertCameraSize;}
    }
    IEnumerator ForceUnpauseI(){
        //Debug.Log("Started coroutine for ForceUnpauseI");
        yield return new WaitForSecondsRealtime(0.1f);
        if(PauseMenu.instance!=null){PauseMenu.instance.Resume();}
        if(UpgradeMenu.instance!=null){UpgradeMenu.instance.Resume();}
        if(Shop.instance!=null){Shop.instance.Resume();}
        //Debug.Log("POST: "+PauseMenu.GameIsPaused+" | "+UpgradeMenu.UpgradeMenuIsOpen+" | "+GameSession.GlobalTimeIsPaused);
    }
    /*void ForceUnpause(){
        if(PauseMenu.instance!=null){PauseMenu.instance.Resume();}
        if(UpgradeMenu.instance!=null){UpgradeMenu.instance.Resume();}
        if(Shop.instance!=null){Shop.instance.Resume();}
        Debug.Log("POST: "+PauseMenu.GameIsPaused+" | "+UpgradeMenu.UpgradeMenuIsOpen+" | "+GameSession.GlobalTimeIsPaused);
        _forceUnpause=false;Debug.Log("Forced unpause");
    }*/
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
    SpriteRenderer _blurImg;
    void Update(){
        if(gameSpeed>=0){Time.timeScale=gameSpeed;}if(gameSpeed<0){gameSpeed=0;}
        if(SceneManager.GetActiveScene().name=="Game"){
        if(Time.timeScale<=0.0001f||PauseMenu.GameIsPaused||Shop.shopOpened||UpgradeMenu.UpgradeMenuIsOpen){GlobalTimeIsPaused=true;}else{GlobalTimeIsPaused=false;}
        if(PauseMenu.GameIsPaused||Shop.shopOpened||UpgradeMenu.UpgradeMenuIsOpen){GlobalTimeIsPausedNotSlowed=true;}else{GlobalTimeIsPausedNotSlowed=false;}
        }else{GlobalTimeIsPaused=false;}

        if(SceneManager.GetActiveScene().name=="Game"&&Player.instance!=null&&!GlobalTimeIsPaused){
            if(_noBreak()){
                currentPlaytime+=Time.unscaledDeltaTime;
                if(gameTimeLeft!=-4){
                    /*if(Player.instance._hasStatus("inverter"))*/gameTimeLeft-=Time.deltaTime;
                    //else gameTimeLeft+=Time.deltaTime;
                }
            }
        }
        if(SceneManager.GetActiveScene().name!="Game"&&setValues==true){setValues=false;}
        
        var _maxXpOverflow=false;
        if(GameRules.instance!=null&&Player.instance!=null)if(Player.instance.GetComponent<PlayerModules>()!=null){
            if(Player.instance.GetComponent<PlayerModules>()._isModuleEquipped("DkSurge")){_maxXpOverflow=true;}
            else _maxXpOverflow=false;
        }
        if(_maxXpOverflow){xp=Mathf.Clamp(xp,0,xpMax*GameRules.instance.xpMaxOvefillMult);}
        else{xp=Mathf.Clamp(xp,0,xpMax);}
        cores=Mathf.Clamp(cores,0,9999);
        coins=Mathf.Clamp(coins,0,9999);
        
        if(GameRules.instance!=null&&SceneManager.GetActiveScene().name=="Game"){
            if(xpTotal<0)xpTotal=0;
            if(GameRules.instance.xpOn&&xp>=xpMax&&GameRules.instance.levelingOn){
                if(!_coreSpawnedPreAscend){
                    if(GameRules.instance.coresOn){GameAssets.instance.Make("PowerCore",new Vector2(UnityEngine.Random.Range(-3.5f, 3.5f),7.4f));}

                    var lvlPopup=GameAssets.instance.FindNotifUIByType(notifUI_type.lvlUp);
                    lvlPopup.GetComponent<ValueDisplay>().value="celestPointPopup";
                    FindObjectOfType<OnScreenButtons>().GetComponent<Animator>().SetTrigger("on");
                    FindObjectOfType<OnScreenButtons>().lvldUp=true;
                    if(FindObjectOfType<CelestialPoints>()!=null)FindObjectOfType<CelestialPoints>().RefreshCelestialPoints();

                    _coreSpawnedPreAscend=true;
                }
                if(Player.instance!=null){
                    if(Player.instance.GetComponent<PlayerModules>()!=null){
                        if(Player.instance.GetComponent<PlayerModules>()._isAutoAscend()
                        ||(!Player.instance.GetComponent<PlayerModules>()._isAutoAscend()&&Input.GetKeyDown(KeyCode.L))){
                            Player.instance.GetComponent<PlayerModules>().Ascend();
                            Ascend();
                        }
                        if(Player.instance.GetComponent<PlayerModules>()._isLvlUpable()&&Player.instance.GetComponent<PlayerModules>()._isAutoLvl()&&Input.GetKeyDown(KeyCode.L)){Player.instance.GetComponent<PlayerModules>().LevelUp();}
                    }
                }
                
                
            }

            if(stayingTimeXP>GameRules.instance.stayingTimeReq){AddXP(GameRules.instance.xp_staying);stayingTimeXP=0f;}
            if(movingTimeXP>GameRules.instance.flyingTimeReq){AddXP(GameRules.instance.xp_flying);movingTimeXP=0f;}

            if(GameRules.instance.breakEncounter){
                if(GetComponent<BreakEncounter>()==null)gameObject.AddComponent<BreakEncounter>();
            }
        }else{if(GetComponent<BreakEncounter>()!=null)Destroy(GetComponent<BreakEncounter>());}

        //Set speed to normal
        if(PauseMenu.GameIsPaused==false&&Shop.shopOpened==false&&UpgradeMenu.UpgradeMenuIsOpen==false&&
        (Player.instance!=null&&!Player.instance._hasStatus("matrix")&&!Player.instance._hasStatus("accel"))&&speedChanged!=true){gameSpeed=defaultGameSpeed;}
        if(SceneManager.GetActiveScene().name!="Game"){gameSpeed=1;}
        if(Player.instance==null){gameSpeed=defaultGameSpeed;}
        
        //Restart with R or Space/Resume with Space
        if(SceneManager.GetActiveScene().name=="Game"){
            if(_blurImg==null)_blurImg=GameObject.Find("BlurImage").GetComponent<SpriteRenderer>();_blurImg.enabled=(PauseMenu.GameIsPaused||UpgradeMenu.UpgradeMenuIsOpen||Shop.shopOpened);
            if((GameOverCanvas.instance==null||GameOverCanvas.instance.gameOver==false)&&PauseMenu.GameIsPaused==false){restartTimer=-4;}
            if(PauseMenu.GameIsPaused==true){if(restartTimer==-4)restartTimer=0.5f;}
            if(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true){if(restartTimer==-4)restartTimer=1f;}
            else if(PauseMenu.GameIsPaused==true&&Input.GetKeyDown(KeyCode.Space)){PauseMenu.instance.Resume();}
            if(restartTimer>0)restartTimer-=Time.unscaledDeltaTime;
            if(restartTimer<=0&&restartTimer!=-4){
                if(Input.GetKeyDown(KeyCode.R)||(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true&&Input.GetKeyDown(KeyCode.Space))){GSceneManager.instance.RestartGame();restartTimer=-4;}
                if(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true&&Input.GetKeyDown(KeyCode.Escape)){GSceneManager.instance.LoadStartMenuGame();}
            }
        }

        if((PauseMenu.GameIsPaused||Shop.shopOpened||UpgradeMenu.UpgradeMenuIsOpen)&&(Player.instance!=null&&Player.instance._hasStatus("inverter"))){
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
            _prefixStatus="DEV | ";
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
                if(gamemodeSelected!=-1){
                    presenceDetails=_prefixDetails+"Score: "+score+" | "+"Game Time: "+GetGameSessionTimeFormat()+_suffixDetails;
                    presenceStatus=_prefixStatus+GameRules.instance.cfgName+nickInfo+_suffixStatus;
                }else{//Adventure
                    if(zoneToTravelTo!=-1){//Traveling
                        presenceDetails=_prefixDetails+"Time left: "+GetGameTimeLeftFormat()+_suffixDetails;
                        presenceStatus=_prefixStatus+"Traveling to Zone "+GameCreator.instance.adventureZones[zoneToTravelTo].name+nickInfo+_suffixStatus;
                    }else{
                        if(FindObjectOfType<BossAI>()==null){
                            var shipLvl="0";
                            if(FindObjectOfType<PlayerModules>()!=null){var pm=FindObjectOfType<PlayerModules>();shipLvl=pm.shipLvl.ToString()+" ("+pm.shipLvlFraction+"/"+pm.lvlFractionsMax+")";}
                            else{var advD=SaveSerial.instance.advD;shipLvl=advD.shipLvl.ToString();}
                            presenceDetails=_prefixDetails+"Lvl: "+shipLvl+" | "+"Game Time: "+GetGameSessionTimeFormat()+_suffixDetails;
                            presenceStatus=_prefixStatus+"Adventure Zone "+GameCreator.instance.adventureZones[zoneSelected].name+nickInfo+_suffixStatus;
                        }else{
                            var b=FindObjectOfType<BossAI>();var be=b.GetComponent<Enemy>();
                            //presenceDetails=_prefixDetails+"Boss Health: "+be.health+"/"+be.healthMax+_suffixDetails;
                            presenceDetails=_prefixDetails+"Progress: "+System.Math.Round((be.health/be.healthMax)*100f,2)+"%"+_suffixDetails;
                            presenceStatus=_prefixStatus+"Fighting "+be.name+nickInfo+_suffixStatus;
                        }
                    }
                }
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
        
        if(SceneManager.GetActiveScene().name!="Game"&&SceneManager.GetActiveScene().name!="AdventureZones"){zoneSelected=-1;zoneToTravelTo=-1;gameTimeLeft=-4;}
        else if(SceneManager.GetActiveScene().name=="Game"){
            if(gamemodeSelected!=-1){zoneSelected=-1;zoneToTravelTo=-1;}
            else{
                if(zoneSelected==-1){zoneSelected=0;GameRules.instance.ReplaceAdventureZoneInfo(GameCreator.instance.adventureZones[zoneSelected].gameRules);}
                
                if(zoneToTravelTo!=-1){if(gameTimeLeft==-4){gameTimeLeft=CalcZoneTravelTime();}}
                else{gameTimeLeft=-4;}
                if(gameTimeLeft<=0&&gameTimeLeft!=-4){zoneSelected=-1;GSceneManager.instance.LoadAdventureZone(zoneToTravelTo);}
            }
        }else if(SceneManager.GetActiveScene().name=="AdventureZones"){
            gamemodeSelected=-1;
            //if(zoneSelected==-1){zoneSelected=0;}
        }
    }
    public float CalcZoneTravelTime(){
        return (Vector2.Distance(GameCreator.instance.adventureZones[zoneSelected].pos,GameCreator.instance.adventureZones[zoneToTravelTo].pos)*GameCreator.instance.adventureTravelZonePrefab.travelTimeToDistanceRatio);
    }
    public float NormalizedZoneTravelTimeLeft(){return GameAssets.Normalize(GameSession.instance.gameTimeLeft,0,GameSession.instance.CalcZoneTravelTime());}

    public void AddToScore(int scoreValue){
        score+=Mathf.RoundToInt(scoreValue*scoreMulti);
        if(ZoneNotBossNorTravel())spawnReqsMono.AddScore(Mathf.RoundToInt(scoreValue*scoreMulti));
        //if(GameRules.instance.shopOn)spawnReqs.AddScore(Mathf.RoundToInt(scoreValue*scoreMulti),-2);
        if(GameRules.instance.scoreDisplay!=scoreDisplay.timeLeft&&GameRules.instance.scoreDisplay!=scoreDisplay.bossHealth)GameCanvas.instance.ScorePopupSwitch(scoreValue*scoreMulti);
    }
    public void AddToScoreNoEV(int scoreValue){score+=scoreValue;GameCanvas.instance.ScorePopupSwitch(scoreValue);}
    public void MultiplyScore(float multipl){score=Mathf.RoundToInt(score*multipl);}
    public void AddXP(float xpValue){if(GameRules.instance.xpOn){xp+=xpValue;GameCanvas.instance.XpPopupSwitch(xpValue);}xpTotal+=xpValue;}
    public void DropXP(float xpAmnt, Vector2 pos, float rangeX=0.5f, float rangeY=0.5f){
        var amnt=Mathf.RoundToInt(xpAmnt);
        GameAssets.instance.MakeSpread("CelestBall",pos,amnt,rangeX,rangeY);
        if(xpAmnt-amnt!=0)GameSession.instance.AddXP(xpAmnt-amnt);
    }
    public void Ascend(){
        xp=xp-xpMax;
        AudioManager.instance.Play("LvlUp");
        _coreSpawnedPreAscend=false;
        if(FindObjectOfType<CelestialPoints>()!=null)FindObjectOfType<CelestialPoints>().RefreshCelestialPoints();
        if(BreakEncounter.instance!=null)BreakEncounter.instance.Ascended();
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
        //if(CheckGamemodeSelected("Adventure")){ResetAfterAdventure();}
        if(gamemodeSelected!=-1)xp=0;
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
        //cores=0;
    }
    public void SaveHighscore(){
        //if(CheckGamemodeSelected("Adventure")&&Player.instance!=null){SaveAdventure();}
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
        var p=Player.instance;
        var s=SaveSerial.instance;
        var ss=SaveSerial.instance.advD;
        if(s==null){Debug.LogError("SaveSerial not present");}
        if(ss!=null){
            ss.buildLastLoaded=buildVersion;
            if(zoneToTravelTo!=-1&&p!=null&&p.health<=0){//Die during travel
                if(GameCreator.instance.adventureTravelZonePrefab.travelTimeToAddOnDeath==0){
                    zoneToTravelTo=-1;//var _zoneBack=zoneSelected;zoneSelected=-1;LoadAdventureZone(_zoneBack);
                }else{gameTimeLeft+=GameCreator.instance.adventureTravelZonePrefab.travelTimeToAddOnDeath;}
            }
            ss.cores=cores;
            ss.zoneSelected=zoneSelected;
            ss.zoneToTravelTo=zoneToTravelTo;
            if(zoneToTravelTo!=-1){ss.travelTimeLeft=gameTimeLeft;}else{ss.travelTimeLeft=-4;}
            //ss.gameSessionTime=currentPlaytime;
            if(FindObjectOfType<PlayerHolobody>()!=null){
                var phb=FindObjectOfType<PlayerHolobody>();
                ss.holo_crystalsStored=phb.crystalsStored;
                ss.holo_powerupStored=phb.powerupStored;
                ss.holo_posX=phb.transform.position.x;
                if(phb.GetTimeLeft()<=0){ss.holo_timeAt=Mathf.RoundToInt(currentPlaytime*GameRules.instance.holodeathTimeRatio);}
                else{ss.holo_timeAt=Mathf.RoundToInt(phb.GetTimeLeft());}
            }
            if(BreakEncounter.instance!=null){ss.calledBreak=BreakEncounter.instance.calledBreak;}else{ss.calledBreak=false;}
            if(p!=null){
                if(p.health>0){
                    ss.xp=xp;
                    ss._coreSpawnedPreAscend=_coreSpawnedPreAscend;
                    ss.healthStart=p.healthStart;
                    ss.health=p.health;
                    ss.hpAbsorpAmnt=p.hpAbsorpAmnt;
                    ss.energy=p.energy;
                    ss.enAbsorpAmnt=p.enAbsorpAmnt;
                    ss.powerups=p.powerups;
                    ss.powerupCurID=p.powerupCurID;
                    ss.statuses=p.statuses;
                }else{
                    //ss.gameSessionTime=0;
                    ss.xp=0;
                    ss._coreSpawnedPreAscend=false;
                    ss.health=0;
                    ss.healthStart=p.healthStart;
                    ss.hpAbsorpAmnt=0;
                    ss.energy=GameRules.instance.energyPlayer;
                    ss.enAbsorpAmnt=0;
                    ss.powerups=GameRules.instance.powerupsStarting;
                    //ss.powerups=new List<Powerup>();
                    ss.powerupCurID=0;
                    ss.statuses=new List<StatusFx>();
                }
                var pm=p.GetComponent<PlayerModules>();
                if(pm!=null){
                    ss.shipLvl=pm.shipLvl;
                    ss.shipLvlFraction=pm.shipLvlFraction;
                    ss.autoAscend=pm.autoAscend;
                    ss.autoLvl=pm.autoLvl;
                    ss.moduleSlots=pm.moduleSlots;
                    ss.skillsSlots=pm.skillsSlots;
                    ss.modulesList=pm.modulesList;
                    ss.skillsList=pm.skillsList;

                    ss.bodyUpgraded=pm.bodyUpgraded;
                    ss.engineUpgraded=pm.engineUpgraded;
                    ss.blastersUpgraded=pm.blastersUpgraded;
                }else{Debug.LogError("PlayerModules not present");}
            yield return new WaitForSecondsRealtime(0.02f);
            Debug.Log("Adventure data saved in GameSession");
            yield return new WaitForSecondsRealtime(0.033f);
            SaveSerial.instance.SaveAdventure();
            }else{Debug.LogError("Player.instance not present");}
        }else{Debug.LogError("Adventure Data null");}
    }
    //LoadAdventure() in GSceneManager.cs
    public void LoadAdventurePre(){
        SaveSerial.instance.LoadAdventure();
        var ss=SaveSerial.instance.advD;
        cores=ss.cores;
        zoneSelected=ss.zoneSelected;
        zoneToTravelTo=ss.zoneToTravelTo;
        gameTimeLeft=ss.travelTimeLeft;
        //currentPlaytime=ss.gameSessionTime;
        Debug.Log("Adventure preloaded");
    }
    public void LoadAdventurePost(){StartCoroutine(LoadAdventureI());}
    IEnumerator LoadAdventureI(){
        //First load from SaveSerial
        _adventureLoading=true;
        yield return new WaitForSecondsRealtime(0.04f);
        var p=Player.instance;
        var s=SaveSerial.instance;
        var ss=s.advD;
        if(p!=null&&s!=null&&ss!=null){
            if(ss.holo_timeAt>0&&ss.holo_zoneSelected==zoneSelected&&zoneToTravelTo==-1){
                var phb=CreatePlayerHoloBody(new Vector2(ss.holo_posX,7.6f));
                phb.SetTime(ss.holo_timeAt);
            }
            if(ss.calledBreak&&_noBreak()){BreakEncounter.instance.CallBreak();BreakEncounter.instance.waitForCargoSpawn=false;}
            p.energy=ss.energy;
            if(ss.health>0){p.health=ss.health;xp=ss.xp;_coreSpawnedPreAscend=ss._coreSpawnedPreAscend;}
            else{xp=0;_coreSpawnedPreAscend=false;
                if(ss.healthStart>0){if(ss.healthStart<GameRules.instance.healthPlayer){ss.healthStart=GameRules.instance.healthPlayer;}p.healthStart=ss.healthStart;p.health=p.healthStart;}
                else{Debug.Log("Health start is 0");//if(p.healthStart!=0&&p.healthStart!=GameRules.instance.healthPlayer){ss.healthStart=p.health;}
                p.healthStart=GameRules.instance.healthPlayer;p.health=p.healthStart;ss.healthStart=p.healthStart;p.energy=GameRules.instance.energyPlayer;ss.energy=p.energy;}
            }//currentPlaytime=0;}
            p.hpAbsorpAmnt=ss.hpAbsorpAmnt;
            p.enAbsorpAmnt=ss.enAbsorpAmnt;
            p.powerups=ss.powerups;
            p.powerupCurID=ss.powerupCurID;
            p.statuses=ss.statuses;
            
            var pm=p.GetComponent<PlayerModules>();
            if(pm!=null){
                pm.shipLvl=ss.shipLvl;
                pm.shipLvlFraction=ss.shipLvlFraction;
                pm.autoAscend=ss.autoAscend;
                pm.autoLvl=ss.autoLvl;
                pm.moduleSlots=ss.moduleSlots;
                pm.skillsSlots=ss.skillsSlots;
                pm.modulesList=ss.modulesList;
                pm.skillsList=ss.skillsList;

                pm.bodyUpgraded=ss.bodyUpgraded;
                pm.engineUpgraded=ss.engineUpgraded;
                pm.blastersUpgraded=ss.blastersUpgraded;
            }else{Debug.LogError("PlayerModules not present");}
            yield return new WaitForSeconds(0.1f);
            if(UpgradeMenu.instance!=null){
                //for(var i=Player.instance.GetComponent<PlayerModules>().shipLvl;i>0;i--){
                    //if(ss.health>0){
                    _lvlEventsLoading=true;
                    UpgradeMenu.instance.LvlEventsAdventure();
                    //}else{UpgradeMenu.instance.LvlEvents();}
                //}
            }
            Debug.Log("Adventure data loaded in GameSession");
        }else{
            if(p==null){Debug.LogError("Player.instance not present");}
            else if(s==null){Debug.LogError("SaveSerial not present");}
            else if(s.advD==null){Debug.LogError("Adventure Data null");}
        }
        //currentPlaytime=ss.gameSessionTime;
        _adventureLoading=false;
    }
    public void DeleteAll(){DeleteStatsAchievs();ResetSettings();SaveSerial.instance.Delete();SaveSerial.instance.DeleteAdventure();GSceneManager.instance.LoadStartMenu();gamemodeSelected=0;}
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

    public bool ZoneNotBossNorTravel(){return !GameRules.instance._isAdventureBossZone&&zoneToTravelTo==-1;}
    public void DieAdventure(){
        if(coins>0&&ZoneNotBossNorTravel())CreatePlayerHoloBody(Player.instance.transform.position,true,false,true);
        SaveAdventure();
    }
    public PlayerHolobody CreatePlayerHoloBody(Vector2 pos,bool show=false,bool collectible=false,bool force=false){
        if(FindObjectOfType<PlayerHolobody>()==null||force){
            if(force&&FindObjectOfType<PlayerHolobody>()!=null){Destroy(FindObjectOfType<PlayerHolobody>().gameObject);}
            var go=GameAssets.instance.Make("PlayerHolobody",pos);
            var phb=go.GetComponent<PlayerHolobody>();
            if(!show){phb.crystalsStored=SaveSerial.instance.advD.holo_crystalsStored;phb.powerupStored=SaveSerial.instance.advD.holo_powerupStored;}
            else{phb.crystalsStored=Mathf.RoundToInt(coins*GameRules.instance.holodeathCrystalsRatio);phb.powerupStored=Player.instance.GetPowerupRandomNotStarting();}
            phb.Switch(show,collectible);
            return phb;
        }else{return null;}
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
    public string GetGameTimeLeftFormat(){return FormatTime(gameTimeLeft);}
    public int GetGameTimeLeft(){return Mathf.RoundToInt(gameTimeLeft);}

    public void SetGamemodeSelected(int i){gamemodeSelected=i;}
    public void SetGamemodeSelectedStr(string name){int i=0;
        if(name.Contains("Sandbox"))gamemodeSelected=0;
        else if(name.Contains("Adventure"))gamemodeSelected=-1;
        else{
            i=Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name))+1;
            if(i>0){gamemodeSelected=i;}
            else{Debug.LogWarning("Cant find GameMode by name: "+name);}
        }
    }
    public bool CheckGamemodeSelected(string name){
        return (
            (gamemodeSelected>0&&gamemodeSelected==Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e=>e.cfgName.Contains(name))+1)
            ||((gamemodeSelected==-1)&&name.Contains("Adventure"))
            ||((gamemodeSelected==0)&&name.Contains("Sandbox"))
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
        //else if(gamemodeSelected<0){gr=GameCreator.instance.adventureZonesPrefabs[Mathf.Abs(gamemodeSelected)-1];}
        return gr;
    }
    public string GetCurrentGamemodeName(){
        string n="";if(gamemodeSelected>0&&gamemodeSelected<GameCreator.instance.gamerulesetsPrefabs.Length+1){n=GameCreator.instance.gamerulesetsPrefabs[gamemodeSelected-1].cfgName;}
        else if(gamemodeSelected==-1){n="Adventure Mode";}
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

    public bool _noBreak(){return (BreakEncounter.instance!=null&&!BreakEncounter.instance.calledBreak)||BreakEncounter.instance==null;}
}

public enum dir{up,down,left,right}
public enum hAlign{left,right}
public enum vAlign{up,down}

public enum InputType{mouse,keyboard,touch,drag}
public enum PlaneDir{vert,horiz}