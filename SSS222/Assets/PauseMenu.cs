using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public float prevGameSpeed = 1f;

    GameSession gameSession;
    void Start(){
        gameSession = FindObjectOfType<GameSession>();
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            }else{
                Pause();
            }
        }
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        gameSession.gameSpeed = prevGameSpeed;
        GameIsPaused = false;
    }
    public void Pause(){
        prevGameSpeed = gameSession.gameSpeed;
        pauseMenuUI.SetActive(true);
        gameSession.gameSpeed = 0f;
        GameIsPaused = true;
        //ParticleSystem.Stop();
    }
}
