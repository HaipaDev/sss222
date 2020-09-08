using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour{
    [SerializeField]ParticleSystem transition;
    [SerializeField]Animator transitioner;
    [SerializeField]float transitionTime=0.35f;
    //float prevGameSpeed;
    private void Awake()
    {
        GameSession.instance = FindObjectOfType<GameSession>();
        GameSession.instance.gameSpeed=1f;
        Time.timeScale = 1f;
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
        FindObjectOfType<SaveSerial>().Save();
        FindObjectOfType<GameSession>().ResetMusicPitch();
        SceneManager.LoadScene("Menu");
        //LoadLevel("Menu");
        if(SceneManager.GetActiveScene().name=="Menu"){FindObjectOfType<GameSession>().speedChanged=false;FindObjectOfType<GameSession>().gameSpeed=1f;}
    }
    public void LoadStartMenuGame(){FindObjectOfType<Level>().StartCoroutine(LoadStartMenuGameI());}
    IEnumerator LoadStartMenuGameI(){
        if(SceneManager.GetActiveScene().name=="Game"){
            FindObjectOfType<GameSession>().SaveHighscore();
            yield return new WaitForSecondsRealtime(0.01f);
            FindObjectOfType<GameSession>().ResetScore();
        }
        yield return new WaitForSecondsRealtime(0.05f);
        FindObjectOfType<SaveSerial>().Save();
        FindObjectOfType<GameSession>().ResetMusicPitch();
        SceneManager.LoadScene("Menu");
        //LoadLevel("Menu");
        if(SceneManager.GetActiveScene().name=="Menu"){FindObjectOfType<GameSession>().speedChanged=false;FindObjectOfType<GameSession>().gameSpeed=1f;}
        
        //FindObjectOfType<GameSession>().savableData.Save();
        //FindObjectOfType<SaveSerial>().Save();
    }
    public void LoadGameScene(){
        //GameSession.instance.SetGameModeSelected(i);
        StartCoroutine(DamageValues.instance.SetValues());
        if(GameSession.instance.gameModeSelected==0){FindObjectOfType<SaveSerial>().LoadAdventure();GameSession.instance.StartCoroutine(GameSession.instance.LoadAdventureI());}
        SceneManager.LoadScene("Game");
        //LoadLevel("Game");
        FindObjectOfType<GameSession>().ResetScore();
        FindObjectOfType<GameSession>().gameSpeed=1f;
        Time.timeScale = 1f;
    }
    public void LoadGameModeChooseScene(){SceneManager.LoadScene("ChooseGameMode");}
    public void LoadGameModeInfoScene(int i){SceneManager.LoadScene("InfoGameMode");GameSession.instance.SetGameModeSelected(i);}
    public void LoadOptionsScene(){SceneManager.LoadScene("Options");}
    public void LoadInventoryScene(){SceneManager.LoadScene("Inventory");}
    public void LoadCreditsScene(){SceneManager.LoadScene("Credits");}
    public void LoadWebsite(string url){Application.OpenURL(url);}
    public void RestartGame(){FindObjectOfType<Level>().StartCoroutine(RestartGameI());}
    IEnumerator RestartGameI(){
        //PauseMenu.GameIsPaused=false;
        FindObjectOfType<GameSession>().SaveHighscore();
        yield return new WaitForSecondsRealtime(0.01f);
        FindObjectOfType<GameSession>().ResetScore();
        FindObjectOfType<GameSession>().ResetMusicPitch();
        yield return new WaitForSecondsRealtime(0.05f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
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
            if(scene=="GameModeChoose"||scene=="Inventory"||scene=="Credits"){
                LoadStartMenu();
            }if(scene=="Options"){
                if(GameObject.Find("OptionsUI").transform.GetChild(1).gameObject.activeSelf==true){
                    GameObject.Find("OptionsUI").transform.GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("OptionsUI").transform.GetChild(0).gameObject.SetActive(true);
                }else{
                    LoadStartMenu(); 
                }
            }
    }
    }

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
