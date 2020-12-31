using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnScreenButtons : MonoBehaviour{
    void Start(){
        if(SaveSerial.instance.scbuttons==false){
            /*foreach(Transform child in transform){
                if(child.GetComponent<Button>()!=null){
                    child.GetComponent<Button>().enabled=false;
                    child.GetComponent<Image>().enabled=false;
                }
                foreach(Transform child2 in child.transform){
                    if(child2.GetComponent<Button>()!=null){
                        child2.GetComponent<Button>().enabled=false;
                        child2.GetComponent<Image>().enabled=false;
                    }
                }
            }*/
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

    void Update(){
        if(SaveSerial.instance!=null)if(SaveSerial.instance.scbuttons==false){
            /*foreach(Transform child in transform){
                if(child.GetComponent<Button>()!=null){
                    if(child.GetComponent<Button>().enabled==true)child.GetComponent<Button>().enabled=false;
                    if(child.GetComponent<Image>().enabled==true)child.GetComponent<Image>().enabled=false;
                }
                foreach(Transform child2 in child.transform){
                    if(child2.GetComponent<Button>()!=null){
                        if(child2.GetComponent<Button>().enabled==true)child2.GetComponent<Button>().enabled=false;
                        if(child2.GetComponent<Image>().enabled==true)child2.GetComponent<Image>().enabled=false;
                    }
                }
            }*/
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

    public void UseSkillQ(){
        if(FindObjectOfType<PlayerSkills>()!=null)FindObjectOfType<PlayerSkills>().UseSkills(1);
    }public void UseSkillE(){
        if(FindObjectOfType<PlayerSkills>()!=null)FindObjectOfType<PlayerSkills>().UseSkills(2);
    }
}
