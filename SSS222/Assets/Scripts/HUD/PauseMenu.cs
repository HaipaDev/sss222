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
        //shop=FindObjectOfType<Shop>();
    }
    void FixedUpdate(){
        if(gameSession==null)gameSession = FindObjectOfType<GameSession>();
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true)pauseMenuUI.SetActive(true);
            PauseButton();
        }/*{if(Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true)if(!GameIsPaused){GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
            gameSession.gameSpeed = 0f;Pause();}else{gameSession.gameSpeed = 1f;Resume();}}*/
        //if(GameIsPaused && !AudioListener.pause) {AudioListener.pause = true;}
    }

    public void PauseButton(){
        if(GameIsPaused){
            Resume();
        }else{
            if(Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true)Pause();
        }
        //Debug.Log("Shop = "+Shop.shopOpened);Debug.Log("Upgrade Menu = "+UpgradeMenu.UpgradeMenuIsOpen);
    }
    public void Resume(){
        gameSession = FindObjectOfType<GameSession>();
        pauseMenuUI.SetActive(false);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        gameSession.gameSpeed = prevGameSpeed;
        GameIsPaused = false;
        //AudioListener.pause = false;
        //AudioListener.volume = 1; //set audio volume
    }
    public void Pause(){
        if(Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true){
            gameSession = FindObjectOfType<GameSession>();
            prevGameSpeed = gameSession.gameSpeed;
            pauseMenuUI.SetActive(true);
            GameIsPaused = true;
            //AudioListener.pause = true;
            //AudioListener.volume = 0; //set audio volume
            GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
            gameSession.gameSpeed = 0f;
            Time.timeScale=0f;
            //ParticleSystem.Stop();
            //var ptSystems = FindObjectOfType<ParticleSystem>();
            //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
        }
    }
    public void Menu(){
        //gameSession.gameSpeed = prevGameSpeed;
        gameSession.gameSpeed = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void PreviousGameSpeed(){gameSession.gameSpeed = prevGameSpeed;}
}
