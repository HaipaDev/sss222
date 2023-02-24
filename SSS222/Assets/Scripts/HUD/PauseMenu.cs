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
    void Start(){
        instance=this;
        Resume();
        if(GameRules.instance.instaPause)unpausedTimeReq=0;
        //shop=FindObjectOfType<Shop>();
    }
    void Update(){
        var _isEditor=false;
        #if UNITY_EDITOR
            _isEditor=true;
        #endif
        if(GSceneManager.EscPressed()||Input.GetKeyDown(KeyCode.Backspace)||Input.GetKeyDown(KeyCode.JoystickButton7)||(!Application.isFocused&&!_isEditor)){
            if(GameIsPaused){
                if(pauseMenuUI.activeSelf){Resume();return;}
                if(optionsUI.transform.GetChild(0).gameObject.activeSelf){SaveSerial.instance.SaveSettings();pauseMenuUI.SetActive(true);return;}
                if(optionsUI.transform.GetChild(1).gameObject.activeSelf){optionsUI.GetComponent<SettingsMenu>().OpenSettings();PauseEmpty();return;}
            }else{
                if(_isPausable()){Pause();}
            }
        }//if(Input.GetKeyDown(KeyCode.R)){//in GameManager}
        if(!GameIsPaused){
            if(unpausedTimer==-1)unpausedTimer=0;
            unpausedTimer+=Time.unscaledDeltaTime;
        }
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        if(optionsUI.transform.GetChild(0).gameObject.activeSelf){SettingsMenu.instance.Back();}
        //if(optionsUI.transform.GetChild(1).gameObject.activeSelf){optionsUI.GetComponent<SettingsMenu>().OpenSettings();}
        GameManager.instance.gameSpeed=1;
        //StartCoroutine(SpeedUp());
        GameIsPaused = false;
        slowDownCo=null;
        //Debug.Log("Resuming pause");
    }
    public void PauseEmpty(){
        GameIsPaused = true;
        if(!GameRules.instance.instaPause){if(slowDownCo==null)slowDownCo=SlowDown();StartCoroutine(slowDownCo);}
        else{GameManager.instance.gameSpeed=0;}
        unpausedTimer=-1;
        //Debug.Log("Pausing");
    }
    public bool _isPausable(){return
        ((Shop.instance!=null&&!Shop.shopOpened)||(Shop.instance==null))&&
        ((UpgradeMenu.instance!=null&&!UpgradeMenu.UpgradeMenuIsOpen)||(UpgradeMenu.instance==null))
        &&!GameOverCanvas.instance.gameOver&&(unpausedTimer>=unpausedTimeReq||unpausedTimer==-1)
        &&(FindObjectOfType<BossAI>()==null||(FindObjectOfType<BossAI>()!=null&&FindObjectOfType<BossAI>().GetComponent<Enemy>().health>0));
    }
    public void Pause(){
        prevGameSpeed = GameManager.instance.gameSpeed;
        pauseMenuUI.SetActive(true);
        PauseEmpty();
        //ParticleSystem.Stop();
        //var ptSystems = FindObjectOfType<ParticleSystem>();
        //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
    }
    IEnumerator SlowDown(){
        while(GameManager.instance.gameSpeed>0){
        GameManager.instance.speedChanged=true; GameManager.instance.gameSpeed-=slowPauseSpeed;
        yield return new WaitForEndOfFrame();
        }
    }IEnumerator SpeedUp(){
        while(GameManager.instance.gameSpeed<1){
        GameManager.instance.speedChanged=true; GameManager.instance.gameSpeed+=slowPauseSpeed;
        yield return new WaitForEndOfFrame();
        }
    }
    public void OpenOptions(){
        optionsUI.GetComponent<SettingsMenu>().OpenSettings();
        pauseMenuUI.SetActive(false);
    }
    public void PreviousGameSpeed(){GameManager.instance.gameSpeed = prevGameSpeed;}
}