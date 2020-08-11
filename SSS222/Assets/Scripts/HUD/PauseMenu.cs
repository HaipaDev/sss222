using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public float prevGameSpeed = 1f;

    GameSession gameSession;
    //Shop shop;
    void Start(){
        gameSession = FindObjectOfType<GameSession>();
        Resume();
        //shop=FindObjectOfType<Shop>();
    }
    void Update(){
        if(gameSession==null)gameSession = FindObjectOfType<GameSession>();
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused&&Time.timeScale==0){
                Resume();
            }else{
                if(Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true)Pause();
            }
        }if(Input.GetKeyDown(KeyCode.R)){
            
        }
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        gameSession.gameSpeed=1;
        //StartCoroutine(SpeedUp());
        GameIsPaused = false;
    }
    public void Pause(){
        prevGameSpeed = gameSession.gameSpeed;
        pauseMenuUI.SetActive(true);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        GameIsPaused = true;
        //gameSession.gameSpeed=0;
        StartCoroutine(SlowDown());
        
        //ParticleSystem.Stop();
        //var ptSystems = FindObjectOfType<ParticleSystem>();
        //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
    }
    IEnumerator SlowDown(){
        while(gameSession.gameSpeed>0){
        gameSession.speedChanged=true; gameSession.gameSpeed -= 0.075f;
        yield return new WaitForEndOfFrame();
        }
    }IEnumerator SpeedUp(){
        while(gameSession.gameSpeed<1){
        gameSession.speedChanged=true; gameSession.gameSpeed += 0.075f;
        yield return new WaitForEndOfFrame();
        }
    }
    public void Menu(){
        //gameSession.gameSpeed = prevGameSpeed;
        gameSession.gameSpeed = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void PreviousGameSpeed(){gameSession.gameSpeed = prevGameSpeed;}
}