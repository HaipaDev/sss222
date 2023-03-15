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
    void Awake(){if(GSceneManager.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);gameObject.name=gameObject.name.Split('(')[0];}}
    void Start(){
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //prevGameSpeed = GameManager.instance.gameSpeed;
    }
    void Update(){
        CheckESC();
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //transitioner=FindObjectOfType<Tag_Transition>().GetComponent<Animator>();
    }
    public void LoadStartMenuLoader(){SceneManager.LoadScene("Menu");Instantiate(CoreSetup.instance.GetJukeboxPrefab());Jukebox.instance.SetMusicToCstmzMusic();}
    public void LoadStartMenu(){
        SaveSerial.instance.Save();
        SaveSerial.instance.SaveLogin();
        if(StatsAchievsManager.instance!=null){
            StatsAchievsManager.instance.SaveStats();
            SaveSerial.instance.SaveStats();
        }
        SceneManager.LoadScene("Menu");
        GameManager.instance.ResetMusicPitch();if(GSceneManager.CheckScene("Menu"))GameManager.instance.speedChanged=false;GameManager.instance.gameSpeed=1f;
        Resources.UnloadUnusedAssets();
        GameManager.instance.ResetTempSandboxSaveName();
    }
    public void LoadStartMenuGame(){GSceneManager.instance.StartCoroutine(LoadStartMenuGameI());}
    IEnumerator LoadStartMenuGameI(){
        if(GSceneManager.CheckScene("Game")){
            GameManager.instance.SaveHighscore();
            if(GameManager.instance.gamemodeSelected==-1&&Player.instance!=null)GameManager.instance.SaveAdventure();
            yield return new WaitForSecondsRealtime(0.01f);
            GameManager.instance.ResetScore();
            GameManager.instance.ResetAfterAdventure();
        }
        yield return new WaitForSecondsRealtime(0.05f);
        SaveSerial.instance.Save();
        SaveSerial.instance.SaveLogin();
        StatsAchievsManager.instance.SaveStats();
        SaveSerial.instance.SaveStats();
        AudioManager.instance.ClearPausedSounds();
        GameManager.instance.ResetMusicPitch();
        if(Jukebox.instance!=null){if(GameRules.instance._isAdventureBossZone){Jukebox.instance.CrossfadeBossToBG();}}//Jukebox.instance.SetMusicToCstmzMusic();}
        if(GameManager.instance.gamemodeSelected!=0)SceneManager.LoadScene("Menu");
        else{if(GameRules.instance!=null){if(GameRules.instance.cfgName.Contains("Sandbox")){SceneManager.LoadScene("SandboxMode");}}}
        yield return new WaitForSecondsRealtime(0.01f);
        GameManager.instance.speedChanged=false;GameManager.instance.defaultGameSpeed=1f;GameManager.instance.gameSpeed=1f;
        Resources.UnloadUnusedAssets();
        /*GameManager.instance.SetGamemodeSelected(0);*/
    }
    public void RestartGame(){GSceneManager.instance.StartCoroutine(GSceneManager.instance.RestartGameI());}
    IEnumerator RestartGameI(){
        GameManager.instance.SaveHighscore();
        //if(GameManager.instance.CheckGamemodeSelected("Adventure"))GameManager.instance.SaveAdventure();//not sure if Restart should save or not
        yield return new WaitForSecondsRealtime(0.01f);
        //spawnReqsMono.RestartAllValues();
        //spawnReqsMono.ResetSpawnReqsList();
        GameManager.instance.ResetAndRemoveSpawnReqsMono();
        GameManager.instance.ResetScore();
        GameManager.instance.ResetMusicPitch();
        yield return new WaitForSecondsRealtime(0.05f);
        if(GameManager.instance.gamemodeSelected==-1){
            if(GameManager.instance.zoneToTravelTo==-1){
                if(GameRules.instance._isAdventureBossZone){if(Jukebox.instance!=null){Jukebox.instance.StopBossMusic();}}
                LoadAdventureZone(GameManager.instance.zoneSelected,true);
            }else{ReloadScene();}
            //yield return new WaitForSecondsRealtime(0.1f);GameManager.instance.LoadAdventurePre();GameManager.instance.LoadAdventurePost();
        }else{ReloadScene();}
        GameManager.instance.ResetAfterAdventure();
        AudioManager.instance.ClearPausedSounds();
        GameManager.instance.EnterGameScene();
        GameRules.instance.EnterGameScene();
    }
    public void LoadGameScene(){
        SceneManager.LoadScene("Game");GameManager.instance.ResetScore();
        GameManager.instance.gameSpeed=1f;
        AudioManager.instance.ClearPausedSounds();
        GameManager.instance.EnterGameScene();
        GameRules.instance.EnterGameScene();
    }
    public void LoadAdventureZone(int i, bool _force=false){gameObject.SetActive(true);Debug.Log("LoadAdventureZoneI("+i+", "+_force+");");
        GSceneManager.instance.StartCoroutine(GSceneManager.instance.LoadAdventureZoneI(i,_force));}
    IEnumerator LoadAdventureZoneI(int i, bool _force=false){
        GameManager.instance.SetGamemodeSelected(-1);
        bool boss=CoreSetup.instance.adventureZones[i].isBoss;
        if((GameManager.instance.zoneToTravelTo==-1&&GameManager.instance.zoneSelected!=i&&GameManager.instance.zoneSelected!=-1)||(GameManager.instance.zoneToTravelTo!=-1&&_force)){//Travel
            //if(GameManager.instance.zoneSelected!=-1&&GameManager.instance.zoneToTravelTo==-1){}
            Debug.Log("TRAVEL");
            GameManager.instance.zoneToTravelTo=i;
            //if(Player.instance!=null)GameManager.instance.SaveAdventure();
            if(GameRules.instance!=null){GameRules.instance.ReplaceAdventureZoneInfo(CoreSetup.instance.adventureTravelZonePrefab,false);}
            else{Instantiate(CoreSetup.instance.adventureGamerulesPrefab);GameRules.instance.ReplaceAdventureZoneInfo(CoreSetup.instance.adventureTravelZonePrefab,false);}
            if(boss){if(Jukebox.instance!=null)Jukebox.instance.FadeOutBGMusic();Debug.Log("FadeOutBGMusic();");}
            else{if(Jukebox.instance!=null)Jukebox.instance.CrossfadeBossToBG();Debug.Log("CrossfadeBossToBG();");}
            StartCoroutine(ResetStuffAndLoadGameScene());
            yield return new WaitForSecondsRealtime(0.2f);
            GameManager.instance.EnterGameScene();
            //if(GameManager.instance.zoneSelected==-1)GameManager.instance.LoadAdventurePre();
            GameManager.instance.LoadAdventurePost();
            GameRules.instance.EnterGameScene();
        }else if((GameManager.instance.zoneSelected==-1)||(GameManager.instance.zoneToTravelTo==-1&&_force)){
            Debug.Log("LOAD A REGULAR ZONE");
            //if(GameManager.instance.zoneSelected==-1)GameManager.instance.LoadAdventurePre();
            GameManager.instance.zoneSelected=i;
            GameManager.instance.zoneToTravelTo=-1;
            GameManager.instance.gameTimeLeft=-4;
            //GameManager.instance.SaveAdventure();
            StartCoroutine(ResetStuffAndLoadGameScene());
            
            if(GameRules.instance!=null){GameRules.instance.ReplaceAdventureZoneInfo(CoreSetup.instance.adventureZones[i].gameRules,boss);}
            else{Instantiate(CoreSetup.instance.adventureGamerulesPrefab);GameRules.instance.ReplaceAdventureZoneInfo(CoreSetup.instance.adventureZones[i].gameRules,boss);}
            if(boss){if(Jukebox.instance!=null)Jukebox.instance.PauseBGMusic();Debug.Log("PauseBGMusic();");}
            else{if(Jukebox.instance!=null)Jukebox.instance.StopBossMusic();Jukebox.instance.UnPauseBGMusic();
                if(Jukebox.instance.BGMusicSilenced()){Jukebox.instance.FadeInBGMusic();Debug.Log("BGMusicSilenced, FadeIn();");}
                Debug.Log("StopBossMusic(); UnPauseBGMusic();");}
            yield return new WaitForSecondsRealtime(0.2f);
            GameManager.instance.EnterGameScene();
            GameManager.instance.LoadAdventurePost();
            GameRules.instance.EnterGameScene();
            GameManager.instance.zoneToTravelTo=-1;
            GameManager.instance.gameTimeLeft=-4;
            //GameManager.instance.SaveAdventure();
        }
        //if(GameManager.instance.zoneSelected==-1){GameManager.instance.LoadAdventurePre();LoadAdventureZone(GameManager.instance.zoneSelected);}
        IEnumerator ResetStuffAndLoadGameScene(){
            GameManager.instance.ResetScore();
            yield return new WaitForSecondsRealtime(0.05f);
            GameManager.instance.ResetAndRemoveSpawnReqsMono();
            //RestartGame();//ReloadScene();
            SceneManager.LoadScene("Game");
            GameManager.instance.gameSpeed=1f;
        }
    }
    public void LoadGameModeChooseScene(){SceneManager.LoadScene("ChooseGameMode");GameManager.instance.ResetTempSandboxSaveName();GameManager.instance.defaultGameSpeed=1;StatsAchievsManager.instance.SaveStats();SaveSerial.instance.SaveStats();}
    public void LoadAdventureZonesScene(){StartCoroutine(LoadAdventureZonesSceneI());}
    public void LoadAdventureFromMainMenu(){
        if(GameManager.instance.zoneToTravelTo!=-1){Debug.Log("Load Travel Zone");LoadAdventureZone(GameManager.instance.zoneToTravelTo,true);return;}
        else if(GameManager.instance.zoneSelected!=-1){Debug.Log("Load Regular Zone");LoadAdventureZone(GameManager.instance.zoneSelected,true);return;}
    }
    IEnumerator LoadAdventureZonesSceneI(){
        SceneManager.LoadScene("AdventureZones");GameManager.instance.gamemodeSelected=-1;
        yield return new WaitForSecondsRealtime(0.03f);
        GameManager.instance.LoadAdventurePre();
        /*yield return new WaitForSecondsRealtime(0.02f);
        if(GameManager.instance.zoneToTravelTo!=-1){Debug.Log("Load Travel Zone");LoadAdventureZone(GameManager.instance.zoneToTravelTo,true);yield break;}
        else if(GameManager.instance.zoneSelected!=-1){Debug.Log("Load Regular Zone");LoadAdventureZone(GameManager.instance.zoneSelected,true);yield break;}*/
        //else{LoadGameScene();}
    }
    public void LoadSandboxModeScene(){SceneManager.LoadScene("SandboxMode");GameManager.instance.SetGamemodeSelected(0);StatsAchievsManager.instance.SaveStats();SaveSerial.instance.SaveStats();}
    public void LoadGameModeInfoScene(){SceneManager.LoadScene("InfoGameMode");}
    public void LoadGameModeInfoSceneSet(int i){SceneManager.LoadScene("InfoGameMode");GameManager.instance.SetGamemodeSelected(i);}
    public void LoadGameModeInfoSceneSetStr(string str){SceneManager.LoadScene("InfoGameMode");GameManager.instance.SetGamemodeSelectedStr(str);}
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
        GameManager.instance.speedChanged=false;
        GameManager.instance.gameSpeed=1f;
    }
    public void QuitGame(){
        Application.Quit();
    }
    /*void OnApplicationQuit(){
        GameManager.
    }*/
    public void RestartApp(){
        if(Jukebox.instance!=null)Destroy(Jukebox.instance.gameObject);
        SceneManager.LoadScene("Loading");
        GameManager.instance.speedChanged=false;
        GameManager.instance.gameSpeed=1f;
    }
    public static bool EscPressed(){return Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Joystick1Button1);}
    void CheckESC(){    if(EscPressed()){
            var scene=SceneManager.GetActiveScene().name;
            if(scene=="ChooseGameMode"||scene=="Credits"||scene=="Socials"){LoadStartMenu();}
            else if(scene=="Achievements"||scene=="StatsSocial"){LoadSocialsScene();}
            else if(scene=="AdventureZones"){LoadGameModeChooseScene();}
            else if(scene=="ScoreSubmit"){LoadGameModeInfoScene();}
    }}
    public static bool CheckScene(string name){return SceneManager.GetActiveScene().name==name;}
}
