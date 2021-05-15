using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnScreenButtons : MonoBehaviour{
    void Update(){
        if(SaveSerial.instance.settingsData.scbuttons==false&&!transform.GetChild(0).GetComponent<Animator>().GetBool("on")){
            foreach(Button bt in GetComponentsInChildren<Button>()){
                bt.enabled=false;
                bt.GetComponent<Image>().enabled=false;
            }
        }else{
            foreach(Button bt in GetComponentsInChildren<Button>()){
                bt.enabled=true;
                bt.GetComponent<Image>().enabled=true;
            }
        }
    }
    public void Pause(){
        var pause=FindObjectOfType<PauseMenu>();
        if(PauseMenu.GameIsPaused!=true){
            if(Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true){pause.pauseMenuUI.SetActive(true);
            pause.Pause();}
        }else{pause.Resume();}
    }
    public void OpenUpgrade(){
        var umenu=FindObjectOfType<UpgradeMenu>();
        if(UpgradeMenu.UpgradeMenuIsOpen!=true){
            if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && Player.instance!=null){umenu.upgradeMenuUI.SetActive(true);
            umenu.Open();}
        }else{umenu.Resume();}
    }

    public void UseSkillQ(){
        if(FindObjectOfType<PlayerSkills>()!=null)FindObjectOfType<PlayerSkills>().UseSkills(1);
    }public void UseSkillE(){
        if(FindObjectOfType<PlayerSkills>()!=null)FindObjectOfType<PlayerSkills>().UseSkills(2);
    }
}
