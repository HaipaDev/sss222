using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour{
    [SerializeField]ParticleSystem transition;
    [SerializeField]Animator transitioner;
    [SerializeField]float transitionTime=0.35f;
    //float prevGameSpeed;
    private void Awake(){
        GameSession.instance.gameSpeed=1f;
        Time.timeScale=1f;
        SetUpSingleton();
    }
    private void SetUpSingleton(){
        int numberOfObj = FindObjectsOfType<Level>().Length;
        if(numberOfObj > 1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start(){
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //prevGameSpeed = GameSession.instance.gameSpeed;
    }
    void Update(){
        CheckESC();
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //transitioner=FindObjectOfType<Tag_Transition>().GetComponent<Animator>();
    }

    public void LoadStartMenu(){
        SaveSerial.instance.Save();
        GameSession.instance.ResetMusicPitch();
        SceneManager.LoadScene("Menu");
        //LoadLevel("Menu");
        if(SceneManager.GetActiveScene().name=="Menu"){GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;}
    }
    public void LoadStartMenuGame(){FindObjectOfType<Level>().StartCoroutine(LoadStartMenuGameI());}
    IEnumerator LoadStartMenuGameI(){
        if(SceneManager.GetActiveScene().name=="Game"){
            GameSession.instance.SaveHighscore();
            yield return new WaitForSecondsRealtime(0.01f);
            GameSession.instance.ResetScore();
        }
        yield return new WaitForSecondsRealtime(0.05f);
        SaveSerial.instance.Save();
        GameSession.instance.ResetMusicPitch();
        SceneManager.LoadScene("Menu");
        //LoadLevel("Menu");
        if(SceneManager.GetActiveScene().name=="Menu"){GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;}
        
        //GameSession.instance.savableData.Save();
        //SaveSerial.instance.Save();
    }
    public void LoadGameScene(){
        //GameSession.instance.SetGameModeSelected(i);
        if(DamageValues.instance!=null)DamageValues.instance.StartCoroutine(DamageValues.instance.SetValues());
        if(GameSession.instance.gameModeSelected==0){SaveSerial.instance.LoadAdventure();GameSession.instance.StartCoroutine(GameSession.instance.LoadAdventureI());}
        SceneManager.LoadScene("Game");
        //LoadLevel("Game");
        GameSession.instance.ResetScore();
        GameSession.instance.gameSpeed=1f;
        Time.timeScale = 1f;
    }
    public void LoadGameModeChooseScene(){SceneManager.LoadScene("ChooseGameMode");}
    public void LoadGameModeInfoScene(int i){SceneManager.LoadScene("InfoGameMode");GameSession.instance.SetGameModeSelected(i);}
    public void LoadOptionsScene(){SceneManager.LoadScene("Options");}
    public void LoadInventoryScene(){SceneManager.LoadScene("Inventory");}
    public void LoadLoginScene(){SceneManager.LoadScene("Login");}
    public void LoadLeaderboardScene(){SceneManager.LoadScene("Leaderboard");}
    public void LoadCreditsScene(){SceneManager.LoadScene("Credits");}
    public void LoadWebsite(string url){Application.OpenURL(url);}
    public void RestartGame(){FindObjectOfType<Level>().StartCoroutine(RestartGameI());}
    IEnumerator RestartGameI(){
        //PauseMenu.GameIsPaused=false;
        //if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().DestroyAll();Destroy(FindObjectOfType<DisruptersSpawner>().gameObject);
        GameSession.instance.SaveHighscore();
        yield return new WaitForSecondsRealtime(0.01f);
        GameSession.instance.ResetScore();
        GameSession.instance.ResetMusicPitch();
        yield return new WaitForSecondsRealtime(0.05f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
        //if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().StartCoroutine(FindObjectOfType<DisruptersSpawner>().SetValues());
    }
    public void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void Restart(){
        SceneManager.LoadScene("Loading");
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
    }
    void CheckESC(){
    if(Input.GetKeyDown(KeyCode.Escape)){
            var scene=SceneManager.GetActiveScene().name;
            if(scene=="ChooseGameMode"||scene=="Inventory"||scene=="Credits"||scene=="Leaderboards"||scene=="Login"){
                LoadStartMenu();
            }if(scene=="Options"){
            if(FindObjectOfType<SettingsMenu>()!=null){
                if(FindObjectOfType<SettingsMenu>().transform.GetChild(1).gameObject.activeSelf==true){
                    FindObjectOfType<SettingsMenu>().OpenSettings();
                }else{
                    LoadStartMenu(); 
                }
            }else Debug.LogError("No SettingsMenu");
            }
    }}

    void LoadLevel(string sceneName){
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
    }
}
