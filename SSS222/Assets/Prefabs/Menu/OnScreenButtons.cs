using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenButtons : MonoBehaviour{
    void Start(){
        
    }

    void Update(){
        
    }
    public void Pause(){
        var pause=FindObjectOfType<PauseMenu>();
    if(PauseMenu.GameIsPaused!=true){
        if(Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true){pause.pauseMenuUI.SetActive(true);
        pause.Pause();}
    }else{pause.Resume();}
        //if(PauseMenu.GameIsPaused)pause.Resume();
    }
    public void OpenUpgrade(){
        var umenu=FindObjectOfType<UpgradeMenu>();
    if(UpgradeMenu.UpgradeMenuIsOpen!=true){
        if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && FindObjectOfType<Player>()!=null){umenu.upgradeMenuUI.SetActive(true);
        umenu.Open();}
    }else{umenu.Resume();}
        //if(UpgradeMenu.UpgradeMenuIsOpen)umenu.Resume();
    }
}
