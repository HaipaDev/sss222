using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour{     public static PauseMenu instance;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsUI;
    public float prevGameSpeed = 1f;
    float slowPauseSpeed = 0.075f;
    float unpausedTimer;
    float unpausedTimeReq=0.3f;
    IEnumerator slowDownCo;
    //Shop shop;
    IEnumerator Start(){
        instance=this;
        yield return new WaitForSeconds(0.05f);
        Resume();
        if(GameRules.instance.instaPause)unpausedTimeReq=0;
        //shop=FindObjectOfType<Shop>();
    }
    void Update(){
        if(GSceneManager.EscPressed()||Input.GetKeyDown(KeyCode.Backspace)||Input.GetKeyDown(KeyCode.JoystickButton7)){
            if(GameIsPaused){
                if(pauseMenuUI.activeSelf){Resume();return;}
                if(optionsUI.transform.GetChild(0).gameObject.activeSelf){SaveSerial.instance.SaveSettings();pauseMenuUI.SetActive(true);return;}
                if(optionsUI.transform.GetChild(1).gameObject.activeSelf){optionsUI.GetComponent<SettingsMenu>().OpenSettings();PauseEmpty();return;}
            }else{
                if(((Shop.instance!=null&&Shop.shopOpened!=true)||(Shop.instance==null))&&
                ((UpgradeMenu.instance!=null&&UpgradeMenu.UpgradeMenuIsOpen!=true)||(UpgradeMenu.instance==null))&&!GameOverCanvas.instance.gameOver&&(unpausedTimer>=unpausedTimeReq||unpausedTimer==-1))Pause();
            }
        }//if(Input.GetKeyDown(KeyCode.R)){//in GameSession}
        if(!GameIsPaused){
            if(unpausedTimer==-1)unpausedTimer=0;
            unpausedTimer+=Time.unscaledDeltaTime;
        }
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        if(optionsUI.transform.GetChild(0).gameObject.activeSelf){SettingsMenu.instance.Back();}
        //if(optionsUI.transform.GetChild(1).gameObject.activeSelf){optionsUI.GetComponent<SettingsMenu>().OpenSettings();}
        GameSession.instance.gameSpeed=1;
        //StartCoroutine(SpeedUp());
        GameIsPaused = false;
        slowDownCo=null;
        //Debug.Log("Resuming pause");
    }
    public void PauseEmpty(){
        
        GameIsPaused = true;
        if(!GameRules.instance.instaPause){if(slowDownCo==null)slowDownCo=SlowDown();StartCoroutine(slowDownCo);}
        else{GameSession.instance.gameSpeed=0;}
        unpausedTimer=-1;
        //Debug.Log("Pausing");
    }
    public void Pause(){
        prevGameSpeed = GameSession.instance.gameSpeed;
        pauseMenuUI.SetActive(true);
        PauseEmpty();
        //ParticleSystem.Stop();
        //var ptSystems = FindObjectOfType<ParticleSystem>();
        //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
    }
    IEnumerator SlowDown(){
        while(GameSession.instance.gameSpeed>0){
        GameSession.instance.speedChanged=true; GameSession.instance.gameSpeed-=slowPauseSpeed;
        yield return new WaitForEndOfFrame();
        }
    }IEnumerator SpeedUp(){
        while(GameSession.instance.gameSpeed<1){
        GameSession.instance.speedChanged=true; GameSession.instance.gameSpeed+=slowPauseSpeed;
        yield return new WaitForEndOfFrame();
        }
    }
    public void Menu(){
        //GameSession.instance.gameSpeed = prevGameSpeed;
        GameSession.instance.gameSpeed = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void OpenOptions(){
        optionsUI.GetComponent<SettingsMenu>().OpenSettings();
        pauseMenuUI.SetActive(false);
    }
    public void PreviousGameSpeed(){GameSession.instance.gameSpeed = prevGameSpeed;}
}