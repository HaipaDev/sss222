using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour{
    GameSession gameSession;
    [SerializeField]ParticleSystem transition;
    [SerializeField]Animator transitioner;
    [SerializeField]float transitionTime=0.35f;
    //float prevGameSpeed;
    private void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
        gameSession.gameSpeed=1f;
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
        gameSession = FindObjectOfType<GameSession>();
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //prevGameSpeed = gameSession.gameSpeed;
    }
    void Update()
    {
        gameSession = FindObjectOfType<GameSession>();
        CheckESC();
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //transitioner=FindObjectOfType<Tag_Transition>().GetComponent<Animator>();
    }

    public void LoadStartMenu(){
        FindObjectOfType<GameSession>().SaveHighscore();
        FindObjectOfType<GameSession>().ResetScore();
        FindObjectOfType<SaveSerial>().Save();
        FindObjectOfType<GameSession>().ResetMusicPitch();
        SceneManager.LoadScene("Menu");
        //LoadLevel("Menu");
        if(SceneManager.GetActiveScene().name=="Menu")FindObjectOfType<GameSession>().gameSpeed=1f;
        //Time.timeScale = 1f;
        
        //FindObjectOfType<GameSession>().savableData.Save();
        //FindObjectOfType<SaveSerial>().Save();
    }
    public void LoadGameScene(int i){
        GameSession.instance.SetGameModeSelected(i);
        SceneManager.LoadScene("Game");
        //LoadLevel("Game");
        FindObjectOfType<GameSession>().ResetScore();
        FindObjectOfType<GameSession>().gameSpeed=1f;
        Time.timeScale = 1f;
    }
    public void LoadGameModeChooseScene(){SceneManager.LoadScene("GameModeChoose");}
    public void LoadOptionsScene(){SceneManager.LoadScene("Options");}
    public void LoadInventoryScene(){SceneManager.LoadScene("Inventory");}
    public void LoadCreditsScene(){SceneManager.LoadScene("Credits");}
    public void LoadWebsite(string url){Application.OpenURL(url);}
    public void RestartGame(){
        PauseMenu.GameIsPaused=false;
        FindObjectOfType<GameSession>().SaveHighscore();
        FindObjectOfType<GameSession>().ResetScore();
        FindObjectOfType<GameSession>().ResetMusicPitch();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FindObjectOfType<GameSession>().gameSpeed=1f;
        Time.timeScale = 1f;
    }public void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameSession.gameSpeed=1f;
        Time.timeScale = 1f;
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void Restart(){
        SceneManager.LoadScene("Loading");
        gameSession.gameSpeed=1f;
        Time.timeScale = 1f;
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
