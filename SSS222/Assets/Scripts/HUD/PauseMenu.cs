using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public float prevGameSpeed = 1f;

    GameSession gameSession;
    void Start(){
        gameSession = FindObjectOfType<GameSession>();
    }
    void Update(){
        if(gameSession==null)gameSession = FindObjectOfType<GameSession>();
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            }else{
                Pause();
            }
        }
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        gameSession.gameSpeed = 1f;
        GameIsPaused = false;
    }
    public void Pause(){
        //prevGameSpeed = gameSession.gameSpeed;
        pauseMenuUI.SetActive(true);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        gameSession.gameSpeed = 0.0001f;
        GameIsPaused = true;
        //ParticleSystem.Stop();
        //var ptSystems = FindObjectOfType<ParticleSystem>();
        //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
    }
    public void Menu(){
        //gameSession.gameSpeed = prevGameSpeed;
        gameSession.gameSpeed = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void PreviousGameSpeed(){
        gameSession.gameSpeed = 1f;
    }
}
