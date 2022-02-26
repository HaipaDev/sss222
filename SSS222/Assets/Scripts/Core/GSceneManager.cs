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
        if(GameSession.instance!=null)
        SceneManager.LoadScene("Menu");
        if(GameSession.instance!=null){GameSession.instance.ResetMusicPitch();if(SceneManager.GetActiveScene().name=="Menu")GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;}
        Resources.UnloadUnusedAssets();
    }
    public void LoadStartMenuGame(){GSceneManager.instance.StartCoroutine(LoadStartMenuGameI());}
    IEnumerator LoadStartMenuGameI(){
        if(SceneManager.GetActiveScene().name=="Game"){
            GameSession.instance.SaveHighscore();
            yield return new WaitForSecondsRealtime(0.01f);
            GameSession.instance.ResetScore();
        }
        yield return new WaitForSecondsRealtime(0.05f);
        SaveSerial.instance.Save();
        SaveSerial.instance.SaveLogin();
        StatsAchievsManager.instance.SaveStats();
        SaveSerial.instance.SaveStats();
        GameSession.instance.ResetMusicPitch();
        SceneManager.LoadScene("Menu");
        yield return new WaitForSecondsRealtime(0.01f);
        GameSession.instance.speedChanged=false;GameSession.instance.defaultGameSpeed=1f;GameSession.instance.gameSpeed=1f;
        GameSession.instance.ResetAfterAdventure();
        Resources.UnloadUnusedAssets();
        /*GameSession.instance.SetGamemodeSelected(0);*/
    }
    public void LoadGameScene(){
        SceneManager.LoadScene("Game");GameSession.instance.ResetScore();
        GameSession.instance.gameSpeed=1f;
        GameSession.instance.EnterGameScene();
        GameRules.instance.EnterGameScene();
    }
    public void LoadAdventureZone(int i){
        GameSession.instance.SetGamemodeSelected(i);
        SceneManager.LoadScene("Game");
        GameSession.instance.gameSpeed=1f;
        GameSession.instance.LoadAdventurePost();
        GameSession.instance.EnterGameScene();
        GameRules.instance.EnterGameScene();
    }
    public void LoadGameModeChooseScene(){SceneManager.LoadScene("ChooseGameMode");/*GameSession.instance.SetGamemodeSelected(0);*/}
    public void LoadAdventureZonesScene(){SceneManager.LoadScene("AdventureZones");GameSession.instance.LoadAdventurePre();}
    public void LoadGameModeInfoScene(){SceneManager.LoadScene("InfoGameMode");}
    public void LoadGameModeInfoSceneSet(int i){SceneManager.LoadScene("InfoGameMode");GameSession.instance.SetGamemodeSelected(i);}
    public void LoadGameModeInfoSceneSetStr(string str){SceneManager.LoadScene("InfoGameMode");GameSession.instance.SetGamemodeSelectedStr(str);}
    public void LoadOptionsScene(){SceneManager.LoadScene("Options");}
    public void LoadCustomizationScene(){SceneManager.LoadScene("Customization");}
    public void LoadSocialsScene(){SceneManager.LoadScene("Socials");}
    public void LoadLoginScene(){SceneManager.LoadScene("Login");}
    public void LoadLeaderboardsScene(){SceneManager.LoadScene("Leaderboards");}
    public void LoadAchievementsScene(){SceneManager.LoadScene("Achievements");}
    public void LoadStatsSocialScene(){SceneManager.LoadScene("StatsSocial");}
    public void LoadScoreSubmitScene(){SceneManager.LoadScene("ScoreSubmit");}
    public void LoadCreditsScene(){SceneManager.LoadScene("Credits");}
    public void LoadWebsite(string url){Application.OpenURL(url);}
    public void SubmitScore(){if(SaveSerial.instance.hyperGamerLoginData.loggedIn){LoadScoreSubmitScene();}else{LoadLoginScene();}}
    public void RestartGame(){GSceneManager.instance.StartCoroutine(GSceneManager.instance.RestartGameI());}
    IEnumerator RestartGameI(){
        GameSession.instance.SaveHighscore();
        //if(GameSession.instance.CheckGamemodeSelected("Adventure"))GameSession.instance.SaveAdventure();//not sure if Restart should save or not
        yield return new WaitForSecondsRealtime(0.01f);
        spawnReqsMono.RestartAllValues();
        spawnReqsMono.ResetSpawnReqsList();
        GameSession.instance.ResetScore();
        GameSession.instance.ResetMusicPitch();
        yield return new WaitForSecondsRealtime(0.05f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
        if(GameSession.instance.CheckGamemodeSelected("Adventure")){
        yield return new WaitForSecondsRealtime(0.1f);GameSession.instance.LoadAdventurePost();}
        GameSession.instance.EnterGameScene();
        GameRules.instance.EnterGameScene();
    }
    public void RestartScene(){
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
    public void Restart(){
        SceneManager.LoadScene("Loading");
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
    }
    void CheckESC(){
    if(Input.GetKeyDown(KeyCode.Escape)){
            var scene=SceneManager.GetActiveScene().name;
            if(scene=="ChooseGameMode"||scene=="Credits"||scene=="Socials"){//||scene=="Customization){
                LoadStartMenu();
            }else if(scene=="Login"||scene=="Leaderboards"||scene=="Achievements"||scene=="StatsSocial"){
                LoadSocialsScene();
            }else if(scene=="InfoGameMode"||scene=="AdventureZones"){
                LoadGameModeChooseScene();
            }else if(scene=="ScoreSubmit"){
                LoadGameModeInfoScene();
            }else if(scene=="Options"){
                if(FindObjectOfType<SettingsMenu>()!=null){
                    if(FindObjectOfType<SettingsMenu>().transform.GetChild(1).gameObject.activeSelf==true){
                        FindObjectOfType<SettingsMenu>().OpenSettings();
                    }else{
                        LoadStartMenu(); 
                    }
                }else Debug.LogError("No SettingsMenu");
            }
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
