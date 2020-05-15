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
        if(Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true)pause.pauseMenuUI.SetActive(true);
        pause.PauseButton();
    }
    public void OpenUpgrade(){
        var umenu=FindObjectOfType<UpgradeMenu>();
        if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && FindObjectOfType<Player>()!=null)umenu.upgradeMenuUI.SetActive(true);
        umenu.UpgradeButton();
    }
}
