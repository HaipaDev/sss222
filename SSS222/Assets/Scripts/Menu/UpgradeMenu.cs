using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour{
    public static bool UpgradeMenuIsOpen = false;
    public GameObject upgradeMenuUI;
    public GameObject upgradeMenu2UI;
    public GameObject statsMenu;
    public GameObject skillsMenu;
    public float prevGameSpeed = 1f;
    [HeaderAttribute("Upgrade Values")]
    public float maxHealth_UpgradeAmnt=5f;
    public int maxHealth_UpgradeCost=1;
    public float maxEnergy_UpgradeAmnt=5f;
    public int maxEnergy_UpgradeCost=1;
    [HeaderAttribute("Upgrade Counts")]
    public int total_UpgradesCount;
    public int total_UpgradesLvl=0;
    public int total_UpgradesCountMax=10;
    public int other_UpgradesCountMax=10;
    public int maxHealth_UpgradesCount;
    public int maxHealth_UpgradesLvl=1;
    public int maxEnergy_UpgradesCount;
    public int maxEnergy_UpgradesLvl=1;
    GameSession gameSession;
    Player player;
    void Start(){
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
    }
    void Update(){
        if(gameSession==null)gameSession = FindObjectOfType<GameSession>();
        if (Input.GetKeyDown(KeyCode.E)){
            if(UpgradeMenuIsOpen){
                Resume();
            }else{
                if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && FindObjectOfType<Player>()!=null)Open();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && UpgradeMenuIsOpen)Resume();

        LevelEverything();
    }

    public void Resume(){
        upgradeMenuUI.SetActive(false);
        upgradeMenu2UI.SetActive(false);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        gameSession.gameSpeed = prevGameSpeed;
        UpgradeMenuIsOpen = false;
    }
    public void Open(){
        prevGameSpeed = gameSession.gameSpeed;
        upgradeMenuUI.SetActive(true);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        gameSession.gameSpeed = 0f;
        UpgradeMenuIsOpen = true;
        //ParticleSystem.Stop();
        //var ptSystems = FindObjectOfType<ParticleSystem>();
        //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
    }

    public void PreviousGameSpeed(){gameSession.gameSpeed = prevGameSpeed;}

    public void OpenStats(){upgradeMenu2UI.SetActive(true);//upgradeMenuUI.SetActive(false);
    //skillsMenu.SetActive(false);
    if(skillsMenu.activeSelf!=true)statsMenu.SetActive(true);
    }
    public void OpenSkills(){upgradeMenu2UI.SetActive(true);//upgradeMenuUI.SetActive(false);
    //statsMenu.SetActive(false);
    if(statsMenu.activeSelf!=true)skillsMenu.SetActive(true);
    }
    public void Back(){
        statsMenu.SetActive(false);skillsMenu.SetActive(false);
        upgradeMenu2UI.SetActive(false);upgradeMenuUI.SetActive(true);
        }

    public void UpgradeFloat(ref float value,float amnt,int cost,bool add,ref float value2,ref int countValue){
        if(gameSession.cores>=cost){value+=amnt;gameSession.cores-=cost;if(add==true){value2+=amnt*2;}countValue++;total_UpgradesCount++;GetComponent<AudioSource>().Play();}
        else{AudioSource.PlayClipAtPoint(gameSession.denySFX,transform.position);}
    }
    //public void AddFloat(ref float value,float amnt,int cost){if(gameSession.cores>=cost){value+=amnt;}}
    //if(gameSession.cores>=maxHealth_UpgradeCost)player.maxHP+=maxHealth_UpgradeAmnt;gameSession.cores-=maxHealth_UpgradeCost;
    public void AddMaxHP(){UpgradeFloat(ref player.maxHP,maxHealth_UpgradeAmnt,maxHealth_UpgradeCost, true, ref player.health,ref maxHealth_UpgradesCount);}
    public void AddMaxEnergy(){UpgradeFloat(ref player.maxEnergy,maxEnergy_UpgradeAmnt,maxEnergy_UpgradeCost, true, ref player.energy,ref maxEnergy_UpgradesCount);}

    void LevelEverything(){
        if(total_UpgradesCount>=total_UpgradesCountMax){total_UpgradesCount=0;total_UpgradesLvl++;}
        if(maxHealth_UpgradesCount>=other_UpgradesCountMax){maxHealth_UpgradesCount=0;maxHealth_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}
        if(maxEnergy_UpgradesCount>=other_UpgradesCountMax){maxEnergy_UpgradesCount=0;maxEnergy_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}

        if(maxHealth_UpgradesLvl>0)maxHealth_UpgradeCost=maxHealth_UpgradesLvl;
        if(maxEnergy_UpgradesLvl>0)maxEnergy_UpgradeCost=maxEnergy_UpgradesLvl;
    }
}
