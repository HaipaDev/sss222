using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GSceneManager : MonoBehaviour{ public static GSceneManager instance;
    /*ParticleSystem transition;
    Animator transitioner;
    float transitionTime=0.35f;*/
    //float prevGameSpeed;
    void OnDisable(){Debug.LogWarning("GSceneManager disabled?");}
    void Awake(){if(GSceneManager.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
    void Start(){
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //prevGameSpeed = GameSession.instance.gameSpeed;
    }
    void Update(){
        CheckESC();
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //transitioner=FindObjectOfType<Tag_Transition>().GetComponent<Animator>();
    }
    public void LoadStartMenuLoader(){SceneManager.LoadScene("Menu");}
    public void LoadStartMenu(){
        SaveSerial.instance.Save();
        SaveSerial.instance.SaveLogin();
        if(StatsAchievsManager.instance!=null){
            StatsAchievsManager.instance.SaveStats();
            SaveSerial.instance.SaveStats();
        }
        SceneManager.LoadScene("Menu");
        GameSession.instance.ResetMusicPitch();if(SceneManager.GetActiveScene().name=="Menu")GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;
        Resources.UnloadUnusedAssets();
        GameSession.instance.ResetTempSandboxSaveName();
    }
    public void LoadStartMenuGame(){GSceneManager.instance.StartCoroutine(LoadStartMenuGameI());}
    IEnumerator LoadStartMenuGameI(){
        if(SceneManager.GetActiveScene().name=="Game"){
            GameSession.instance.SaveHighscore();
            if(GameSession.instance.gamemodeSelected==-1&&Player.instance!=null)GameSession.instance.SaveAdventure();
            yield return new WaitForSecondsRealtime(0.01f);
            GameSession.instance.ResetScore();
            GameSession.instance.ResetAfterAdventure();
        }
        yield return new WaitForSecondsRealtime(0.05f);
        SaveSerial.instance.Save();
        SaveSerial.instance.SaveLogin();
        StatsAchievsManager.instance.SaveStats();
        SaveSerial.instance.SaveStats();
        GameSession.instance.ResetMusicPitch();
        if(GameSession.instance.gamemodeSelected!=0)SceneManager.LoadScene("Menu");
        else{if(GameRules.instance!=null){if(GameRules.instance.cfgName.Contains("Sandbox")){SceneManager.LoadScene("SandboxMode");}}}
        yield return new WaitForSecondsRealtime(0.01f);
        GameSession.instance.speedChanged=false;GameSession.instance.defaultGameSpeed=1f;GameSession.instance.gameSpeed=1f;
        Resources.UnloadUnusedAssets();
        /*GameSession.instance.SetGamemodeSelected(0);*/
    }
    public void RestartGame(){GSceneManager.instance.StartCoroutine(GSceneManager.instance.RestartGameI());}
    IEnumerator RestartGameI(){
        GameSession.instance.SaveHighscore();
        //if(GameSession.instance.CheckGamemodeSelected("Adventure"))GameSession.instance.SaveAdventure();//not sure if Restart should save or not
        yield return new WaitForSecondsRealtime(0.01f);
        //spawnReqsMono.RestartAllValues();
        //spawnReqsMono.ResetSpawnReqsList();
        GameSession.instance.ResetAndRemoveSpawnReqsMono();
        GameSession.instance.ResetScore();
        GameSession.instance.ResetMusicPitch();
        yield return new WaitForSecondsRealtime(0.05f);
        if(GameSession.instance.gamemodeSelected==-1){
            if(GameSession.instance.zoneToTravelTo==-1){LoadAdventureZone(GameSession.instance.zoneSelected,true);}
            else{ReloadScene();}
            //yield return new WaitForSecondsRealtime(0.1f);GameSession.instance.LoadAdventurePre();GameSession.instance.LoadAdventurePost();
        }else{ReloadScene();}
        GameSession.instance.ResetAfterAdventure();
        GameSession.instance.EnterGameScene();
        GameRules.instance.EnterGameScene();
    }
    public void LoadGameScene(){
        SceneManager.LoadScene("Game");GameSession.instance.ResetScore();
        GameSession.instance.gameSpeed=1f;
        GameSession.instance.EnterGameScene();
        GameRules.instance.EnterGameScene();
    }
    public void LoadAdventureZone(int i, bool _force=false){StartCoroutine(LoadAdventureZoneI(i,_force));}
    IEnumerator LoadAdventureZoneI(int i, bool _force=false){
        GameSession.instance.SetGamemodeSelected(-1);
        bool boss=GameCreator.instance.adventureZones[i].isBoss;
        if((GameSession.instance.zoneToTravelTo==-1&&GameSession.instance.zoneSelected!=i&&GameSession.instance.zoneSelected!=-1)||(_force)){//Travel
            if(GameRules.instance!=null){GameRules.instance.ReplaceAdventureZoneInfo(GameCreator.instance.adventureTravelZonePrefab,false);}
            else{Instantiate(GameCreator.instance.adventureGamerulesPrefab);GameRules.instance.ReplaceAdventureZoneInfo(GameCreator.instance.adventureTravelZonePrefab,false);}
            GameSession.instance.zoneToTravelTo=i;
            if(Player.instance!=null)GameSession.instance.SaveAdventure();
            StartCoroutine(ResetStuffAndLoadGameScene());
            yield return new WaitForSecondsRealtime(0.2f);
            GameSession.instance.EnterGameScene();
            if(GameSession.instance.zoneSelected==-1)GameSession.instance.LoadAdventurePre();
            GameSession.instance.LoadAdventurePost();
            GameRules.instance.EnterGameScene();
        }else if((GameSession.instance.zoneSelected==-1)||(GameSession.instance.zoneToTravelTo==-1&&_force)){
            if(GameSession.instance.zoneSelected==-1)GameSession.instance.LoadAdventurePre();
            GameSession.instance.zoneSelected=i;
            GameSession.instance.zoneToTravelTo=-1;
            GameSession.instance.gameTimeLeft=-4;
            StartCoroutine(ResetStuffAndLoadGameScene());
            
            if(GameRules.instance!=null){GameRules.instance.ReplaceAdventureZoneInfo(GameCreator.instance.adventureZones[i].gameRules,boss);}
            else{Instantiate(GameCreator.instance.adventureGamerulesPrefab);GameRules.instance.ReplaceAdventureZoneInfo(GameCreator.instance.adventureZones[i].gameRules,boss);}
            yield return new WaitForSecondsRealtime(0.2f);
            GameSession.instance.EnterGameScene();
            GameSession.instance.LoadAdventurePost();
            GameRules.instance.EnterGameScene();
        }
        IEnumerator ResetStuffAndLoadGameScene(){
            GameSession.instance.ResetScore();
            yield return new WaitForSecondsRealtime(0.05f);
            GameSession.instance.ResetAndRemoveSpawnReqsMono();
            //RestartGame();//ReloadScene();
            SceneManager.LoadScene("Game");
            GameSession.instance.gameSpeed=1f;
        }
    }
    public void LoadGameModeChooseScene(){SceneManager.LoadScene("ChooseGameMode");GameSession.instance.ResetTempSandboxSaveName();GameSession.instance.defaultGameSpeed=1;StatsAchievsManager.instance.SaveStats();SaveSerial.instance.SaveStats();}
    public void LoadAdventureZonesScene(){StartCoroutine(LoadAdventureZonesSceneI());}
    IEnumerator LoadAdventureZonesSceneI(){
        SceneManager.LoadScene("AdventureZones");GameSession.instance.gamemodeSelected=-1;
        yield return new WaitForSecondsRealtime(0.03f);
        GameSession.instance.LoadAdventurePre();
        yield return new WaitForSecondsRealtime(0.02f);
        if(GameSession.instance.zoneToTravelTo!=-1){LoadAdventureZone(GameSession.instance.zoneToTravelTo,true);}
        else if(GameSession.instance.zoneSelected!=-1){LoadAdventureZone(GameSession.instance.zoneSelected,true);}
        //else{LoadGameScene();}
    }
    public void LoadSandboxModeScene(){SceneManager.LoadScene("SandboxMode");GameSession.instance.SetGamemodeSelected(0);StatsAchievsManager.instance.SaveStats();SaveSerial.instance.SaveStats();}
    public void LoadGameModeInfoScene(){SceneManager.LoadScene("InfoGameMode");}
    public void LoadGameModeInfoSceneSet(int i){SceneManager.LoadScene("InfoGameMode");GameSession.instance.SetGamemodeSelected(i);}
    public void LoadGameModeInfoSceneSetStr(string str){SceneManager.LoadScene("InfoGameMode");GameSession.instance.SetGamemodeSelectedStr(str);}
    public void LoadOptionsScene(){SceneManager.LoadScene("Options");}
    public void LoadCustomizationScene(){SceneManager.LoadScene("Customization");}
    public void LoadSocialsScene(){SceneManager.LoadScene("Socials");}
    public void LoadLoginScene(){SceneManager.LoadScene("Login");}
    public void LoadLeaderboardsScene(){SceneManager.LoadScene("Leaderboards");}
    public void LoadScoreUsersDataScene(){SceneManager.LoadScene("ScoreUsersData");}
    public void LoadAchievementsScene(){SceneManager.LoadScene("Achievements");}
    public void LoadStatsSocialScene(){SceneManager.LoadScene("StatsSocial");}
    public void LoadScoreSubmitScene(){SceneManager.LoadScene("ScoreSubmit");}
    public void LoadCreditsScene(){SceneManager.LoadScene("Credits");}
    public void LoadWebsite(string url){Application.OpenURL(url);}
    public void SubmitScore(){if(SaveSerial.instance.hyperGamerLoginData.loggedIn){LoadScoreSubmitScene();}else{LoadLoginScene();}}
    public void ReloadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
    }
    public void QuitGame(){
        Application.Quit();
    }
    /*void OnApplicationQuit(){
        GameSession.
    }*/
    public void RestartApp(){
        SceneManager.LoadScene("Loading");
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
    }
    public static bool EscPressed(){return Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Joystick1Button1);}
    void CheckESC(){    if(EscPressed()){
            var scene=SceneManager.GetActiveScene().name;
            if(scene=="ChooseGameMode"||scene=="Credits"||scene=="Socials"){LoadStartMenu();}
            else if(scene=="Achievements"||scene=="StatsSocial"){LoadSocialsScene();}
            else if(scene=="AdventureZones"){LoadGameModeChooseScene();}
            else if(scene=="ScoreSubmit"){LoadGameModeInfoScene();}
    }}

    /*void LoadLevel(string sceneName){
        //StartCoroutine(LoadTransition(sceneName));
        LoadTransition(sceneName);
    }
    void LoadTransition(string sceneName){
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        transitioner=FindObjectOfType<Tag_Transition>().GetComponent<Animator>();
        
        //transition.Play();
        transitioner.SetTrigger("Start");

        //yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }*/
}
