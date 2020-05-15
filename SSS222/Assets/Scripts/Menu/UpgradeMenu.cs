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
    public float speed_UpgradeAmnt=0.05f;
    public int speed_UpgradeCost=1;
    public int defaultPowerup_upgradeCost1=1;
    public int defaultPowerup_upgradeCost2=3;
    public int defaultPowerup_upgradeCost3=6;
    public int energyRefill_upgradeCost=3;
    [HeaderAttribute("Upgrade Counts")]
    public int total_UpgradesCount;
    public int total_UpgradesCountMax=10;
    public int total_UpgradesLvl=0;
    public int other_UpgradesCountMax=10;
    public int maxHealth_UpgradesCount;
    public int maxHealth_UpgradesCountMax=10;
    public int maxHealth_UpgradesLvl=1;
    public int maxEnergy_UpgradesCount;
    public int maxEnergy_UpgradesCountMax=4;
    public int maxEnergy_UpgradesLvl=1;
    public int speed_UpgradesCount;
    public int speed_UpgradesCountMax=10;
    public int speed_UpgradesLvl=1;
    public int defaultPowerup_upgradeCount;
    public int energyRefill_upgraded;
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

        LevelEverything();
    }

    public void UpgradeButton(){
        if(UpgradeMenuIsOpen){
            Resume();
        }else{
            if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && FindObjectOfType<Player>()!=null)Open();
        }
        //Debug.Log("Shop = "+Shop.shopOpened);Debug.Log("Pause = "+PauseMenu.GameIsPaused);
        if (Input.GetKeyDown(KeyCode.Escape) && UpgradeMenuIsOpen)Resume();
    }
    public void Resume(){
        gameSession = FindObjectOfType<GameSession>();
        upgradeMenuUI.SetActive(false);
        upgradeMenu2UI.SetActive(false);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        gameSession.gameSpeed = prevGameSpeed;
        UpgradeMenuIsOpen = false;
    }
    public void Open(){
        if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && FindObjectOfType<Player>()!=null){
            gameSession = FindObjectOfType<GameSession>();
            prevGameSpeed = gameSession.gameSpeed;
            upgradeMenuUI.SetActive(true);
            UpgradeMenuIsOpen = true;
            GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
            gameSession.gameSpeed = 0f;
            //ParticleSystem.Stop();
            //var ptSystems = FindObjectOfType<ParticleSystem>();
            //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
        }
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
        if(gameSession.cores>=cost){value+=amnt;value=(float)System.Math.Round(value,2);gameSession.cores-=cost;if(add==true){value2+=amnt*2;}countValue++;total_UpgradesCount++;GetComponent<AudioSource>().Play();}
        else{AudioSource.PlayClipAtPoint(gameSession.denySFX,transform.position);}
    }
    //public void AddFloat(ref float value,float amnt,int cost){if(gameSession.cores>=cost){value+=amnt;}}
    //if(gameSession.cores>=maxHealth_UpgradeCost)player.maxHP+=maxHealth_UpgradeAmnt;gameSession.cores-=maxHealth_UpgradeCost;
    public void AddMaxHP(){UpgradeFloat(ref player.maxHP,maxHealth_UpgradeAmnt,maxHealth_UpgradeCost, true, ref player.health,ref maxHealth_UpgradesCount);}
    public void AddMaxEnergy(){UpgradeFloat(ref player.maxEnergy,maxEnergy_UpgradeAmnt,maxEnergy_UpgradeCost, true, ref player.energy,ref maxEnergy_UpgradesCount);}
    public void AddSpeed(){UpgradeFloat(ref player.moveSpeed,speed_UpgradeAmnt,speed_UpgradeCost, false, ref player.moveSpeedCurrent,ref speed_UpgradesCount);}

    public void DefaultPowerupChange(string prevPowerup,string powerup,int cost,bool add,ref float value,float amnt,bool permament,int upgradeXPamnt){
        if(gameSession.cores>=cost && player.powerupDefault==prevPowerup){player.powerupDefault=powerup;if(permament!=true){player.powerup=powerup;}gameSession.cores-=cost;if(add==true){value+=amnt;}defaultPowerup_upgradeCount++;total_UpgradesCount+=upgradeXPamnt;if(permament==true){player.losePwrupOutOfEn=false;}GetComponent<AudioSource>().Play();}
        else{AudioSource.PlayClipAtPoint(gameSession.denySFX,transform.position);}
    }
    //public void DefaultPowerupL2(){if(gameSession.cores>powerupDefaultL2_UpgradeCost){player.powerupDefault="laser2";gameSession.cores-=powerupDefaultL2_UpgradeCost;total_UpgradesCount++;}else{GetComponent<AudioSource>().Play();}}
    public void DefaultPowerupL2(){DefaultPowerupChange("laser","laser2",defaultPowerup_upgradeCost1,true,ref player.energy,100,false,0);}//defaultPowerup_upgradeCost1+1);}
    public void DefaultPowerupL3(){DefaultPowerupChange("laser2","laser3",defaultPowerup_upgradeCost2,true,ref player.energy,115,false,0);}//defaultPowerup_upgradeCost2);}
    public void DefaultPowerupPerma(){DefaultPowerupChange("laser3","perma",defaultPowerup_upgradeCost3,true,ref player.energy,130,true,0);}//defaultPowerup_upgradeCost3);}

    public void UnlockEnergyRefill(){
        if(gameSession.cores>=energyRefill_upgradeCost && energyRefill_upgraded!=1){player.energyRefillUnlocked=true;gameSession.cores-=energyRefill_upgradeCost;energyRefill_upgraded=1;/*total_UpgradesCount+=energyRefill_upgradeCost;*/GetComponent<AudioSource>().Play();}
        else{AudioSource.PlayClipAtPoint(gameSession.denySFX,transform.position);}
    }

    void LevelEverything(){
        if(total_UpgradesCount>=total_UpgradesCountMax){total_UpgradesCount=total_UpgradesCount-total_UpgradesCountMax-1;total_UpgradesLvl++;}
        if(maxHealth_UpgradesCount>=maxHealth_UpgradesCountMax){maxHealth_UpgradesCount=0;maxHealth_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}
        if(maxEnergy_UpgradesCount>=maxEnergy_UpgradesCountMax){maxEnergy_UpgradesCount=0;maxEnergy_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}
        if(speed_UpgradesCount>=speed_UpgradesCountMax){speed_UpgradesCount=0;speed_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}

        if(maxHealth_UpgradesLvl>0)maxHealth_UpgradeCost=maxHealth_UpgradesLvl;
        if(maxEnergy_UpgradesLvl>0)maxEnergy_UpgradeCost=maxEnergy_UpgradesLvl;
        if(speed_UpgradesLvl>0)speed_UpgradeCost=speed_UpgradesLvl;
    }
}
