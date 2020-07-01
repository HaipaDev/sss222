using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeMenu : MonoBehaviour{
    public static UpgradeMenu instance;
    public static bool UpgradeMenuIsOpen = false;
    public GameObject upgradeMenuUI;
    public GameObject upgradeMenu2UI;
    public GameObject lvltreeUI;
    public GameObject statsMenu;
    public GameObject skillsMenu;
    public GameObject skills1Menu;
    public GameObject skills2Menu;
    public XPBars lvlbar;
    public float prevGameSpeed = 1f;
    [HeaderAttribute("Upgrade Values")]
    public float maxHealth_UpgradeAmnt=5f;
    public int maxHealth_UpgradeCost=1;
    public float maxEnergy_UpgradeAmnt=5f;
    public int maxEnergy_UpgradeCost=1;
    public float speed_UpgradeAmnt=0.05f;
    public int speed_UpgradeCost=1;
    public float hpRegen_UpgradeAmnt=0.2f;
    public int hpRegen_UpgradeCost=1;
    public float enRegen_UpgradeAmnt=2f;
    public int enRegen_UpgradeCost=1;
    public int defaultPowerup_upgradeCost1=1;
    public int defaultPowerup_upgradeCost2=3;
    public int defaultPowerup_upgradeCost3=6;
    public int energyRefill_upgradeCost=3;
    public int mPulse_upgradeCost=3;
    public int postMortem_upgradeCost=1;
    public int teleport_upgradeCost=2;
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
    public int hpRegen_UpgradesCount;
    public int hpRegen_UpgradesCountMax=2;
    public int hpRegen_UpgradesLvl=0;
    public int enRegen_UpgradesCount;
    public int enRegen_UpgradesCountMax=2;
    public int enRegen_UpgradesLvl=0;
    public int defaultPowerup_upgradeCount;
    public int energyRefill_upgraded;
    public int magneticPulse_upgraded;
    public int teleport_upgraded;
    GameSession gameSession;
    Player player;
    PlayerSkills pskills;
    IEnumerator co;

    int lvlID;
    int lvlcr;

    void Start(){
        instance=this;
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
        pskills = FindObjectOfType<PlayerSkills>();
        Resume();
    }
    void Update(){
        if(gameSession==null)gameSession = FindObjectOfType<GameSession>();
        if (Input.GetKeyDown(KeyCode.F)){
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
        lvltreeUI.SetActive(false);
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

    public void OpenStats(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        //skillsMenu.SetActive(false);
        if(skillsMenu.activeSelf!=true)statsMenu.SetActive(true);
    }
    public void OpenSkills(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        //statsMenu.SetActive(false);
        if(statsMenu.activeSelf!=true)skillsMenu.SetActive(true);
        if(skills1Menu.activeSelf==false&&skills2Menu.activeSelf==false){skills1Menu.SetActive(true);}
    }
    public void OpenSkillsPrev(){
        if(statsMenu.activeSelf!=true)skillsMenu.SetActive(true);
        if(skills2Menu.activeSelf==true){skills1Menu.SetActive(true);skills2Menu.SetActive(false);}
        else if(skills1Menu.activeSelf==true&&skills2Menu.activeSelf==false){skills2Menu.SetActive(true);skills1Menu.SetActive(false);}
    }public void OpenSkillsNext(){
        if(statsMenu.activeSelf!=true)skillsMenu.SetActive(true);
        if(skills1Menu.activeSelf==true){skills2Menu.SetActive(true);skills1Menu.SetActive(false);}
        else if(skills2Menu.activeSelf==true&&skills1Menu.activeSelf==false){skills1Menu.SetActive(true);skills2Menu.SetActive(false);}
    }public void OpenLvlTree(){
        upgradeMenuUI.SetActive(false);
        lvltreeUI.SetActive(true);
    }
    public void Back(){
        statsMenu.SetActive(false);skillsMenu.SetActive(false);
        upgradeMenu2UI.SetActive(false);lvltreeUI.SetActive(false);
        upgradeMenuUI.SetActive(true);
    }

    public void UnlockSkillUni(int ID, ref int value,int number,int cost){
        if(gameSession.cores>=cost && value==number-1){value=number;gameSession.cores-=cost;GetComponent<AudioSource>().Play();
        var skills=FindObjectOfType<Player>().GetComponent<PlayerSkills>().skillsBinds;
        for(var i=0;i<skills.Length;i++){if(i!=ID)if(skills[i]==skillKeyBind.Q && skills[i]!=skillKeyBind.E){skills[ID]=skillKeyBind.E;}}//Set to E if Q is occuppied
        for(var i=0;i<skills.Length;i++){if(i!=ID)if(skills[i]!=skillKeyBind.Q){skills[ID]=skillKeyBind.Q;}}//Set to Q by default
        }
        else{AudioManager.instance.Play("Deny");}
    }
    public void UnlockSkill(int ID){
        if(ID==0){UnlockSkillUni(ID,ref magneticPulse_upgraded,1,mPulse_upgradeCost);}
        if(ID==1){UnlockSkillUni(ID,ref teleport_upgraded,1,teleport_upgradeCost);}

        
    }
    public void UpgradeSkill(int ID){
        if(ID==0){UnlockSkillUni(ID,ref magneticPulse_upgraded,2,postMortem_upgradeCost);}
        if(ID==1){UnlockSkillUni(ID,ref teleport_upgraded,2,1);}
    }
    public void SetSkillQ(int ID){
        var skills=FindObjectOfType<Player>().GetComponent<PlayerSkills>().skillsBinds;

        //foreach(skillKeyBind skillOther in skills){skillOther=skillKeyBind.Disabled;}
        for(var i=0;i<skills.Length;i++){if(i!=ID){if(skills[i]!=skillKeyBind.E){skills[i]=skillKeyBind.Disabled;}}}
        //skills[ID]=skillKeyBind.Q;
        //if(pskills.cooldownE>0 && pskills.cooldownQ==0){pskills.cooldownQ=pskills.cooldownE;}
        //if(skills[ID]!=skillKeyBind.Q){
        /*var k=EventSystem.current.currentSelectedGameObject.GetComponent<ColorSkillKey>();
        if(k.ID!=ID&&skills[ID]!=skillKeyBind.Q){
            var Q=pskills.cooldownQ;
            var E=pskills.cooldownE;
            pskills.cooldownE=Q;
            pskills.cooldownQ=E;
        }*/
        var ii=0;
        foreach(ColorSkillKey k in FindObjectsOfType<ColorSkillKey>()){
            if(k.ID==ID&&k.on==false){
                if(ii<2)ii++;
            }
        }
        if(ii==2){
            if(skills[ID]!=skillKeyBind.Q){
                var Q=pskills.cooldownQ;
                var E=pskills.cooldownE;
                pskills.cooldownE=Q;
                pskills.cooldownQ=E;
                }
        }
        /*if(skills[ID]!=skillKeyBind.E){
            var Q=pskills.cooldownQ;
            var E=pskills.cooldownE;
            pskills.cooldownE=Q;
            pskills.cooldownQ=E;
        }*/
        skills[ID]=skillKeyBind.Q;
        /*var skills=FindObjectOfType<Player>().GetComponent<PlayerSkills>().skills;
        foreach(SkillSlotID skillOther in skills){skillOther.keySet=skillKeyBind.Disabled;}
        var skill=skills[ID];
        skill.keySet=skillKeyBind.Q;*/
    }public void SetSkillE(int ID){
        var skills=FindObjectOfType<Player>().GetComponent<PlayerSkills>().skillsBinds;
        for(var i=0;i<skills.Length;i++){if(i!=ID){if(skills[i]!=skillKeyBind.Q){skills[i]=skillKeyBind.Disabled;}}}
        var ii=0;
        foreach(ColorSkillKey k in FindObjectsOfType<ColorSkillKey>()){
            if(k.ID==ID&&k.on==false){
                if(ii<2)ii++;
            }
        }
        if(ii==2){
            if(skills[ID]!=skillKeyBind.E){
                var Q=pskills.cooldownQ;
                var E=pskills.cooldownE;
                pskills.cooldownE=Q;
                pskills.cooldownQ=E;
            }
        }
        skills[ID]=skillKeyBind.E;
    }

    public void UpgradeFloat(ref float value,float amnt,int cost,bool add,ref float value2,ref int countValue, ref int countLvl,bool bypass){
        if(gameSession.cores>=cost&&(bypass==true||(bypass==false&&countLvl<=total_UpgradesLvl))){value+=amnt;value=(float)System.Math.Round(value,2);gameSession.cores-=cost;if(add==true){value2+=amnt*2;}countValue++;/*total_UpgradesCount++;*/
        GetComponent<AudioSource>().Play();}//var go=EventSystem.current.currentSelectedGameObject; go.GetComponent<Image>().color=new Color(79,169,107); go.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=new Color(49,188,80);}
        else{AudioManager.instance.Play("Deny");}//var go=EventSystem.current.currentSelectedGameObject; go.GetComponent<Image>().color=Color.red; go.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;}
    }public void UpgradeAfterStartingVal(ref bool valueEnable,ref float value,float startingVal,float secondVal,float amnt,int cost,bool add,ref float value2,ref int countValue,ref int countLvl, bool bypass){
        if(gameSession.cores>=cost&&(bypass==true||(bypass==false&&countLvl<=total_UpgradesLvl&&total_UpgradesLvl>0))){if(valueEnable!=true){valueEnable=true;}if(value>=secondVal){value+=amnt;}else{if(value==startingVal&&(countLvl>1))value=secondVal;}value=(float)System.Math.Round(value,2);gameSession.cores-=cost;if(add==true){value2+=amnt*2;}countValue++;/*total_UpgradesCount++;*/
        GetComponent<AudioSource>().Play();}//var go=EventSystem.current.currentSelectedGameObject; go.GetComponent<Image>().color=new Color(79,169,107); go.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=new Color(49,188,80);}
        else{AudioManager.instance.Play("Deny");}//var go=EventSystem.current.currentSelectedGameObject; go.GetComponent<Image>().color=Color.red; go.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;}
    }
    //public void AddFloat(ref float value,float amnt,int cost){if(gameSession.cores>=cost){value+=amnt;}}
    //if(gameSession.cores>=maxHealth_UpgradeCost)player.maxHP+=maxHealth_UpgradeAmnt;gameSession.cores-=maxHealth_UpgradeCost;
    public void AddMaxHP(){UpgradeFloat(ref player.maxHP,maxHealth_UpgradeAmnt,maxHealth_UpgradeCost, true, ref player.health,ref maxHealth_UpgradesCount, ref maxHealth_UpgradesLvl,false);}
    public void AddMaxEnergy(){UpgradeFloat(ref player.maxEnergy,maxEnergy_UpgradeAmnt,maxEnergy_UpgradeCost, true, ref player.energy,ref maxEnergy_UpgradesCount, ref maxEnergy_UpgradesLvl,false);}
    public void AddSpeed(){UpgradeFloat(ref player.moveSpeed,speed_UpgradeAmnt,speed_UpgradeCost, false, ref player.moveSpeedCurrent,ref speed_UpgradesCount, ref speed_UpgradesLvl,false);}
    public void AddSpeedBypass(){UpgradeFloat(ref player.moveSpeed,speed_UpgradeAmnt/2,0, false, ref player.moveSpeedCurrent,ref speed_UpgradesCount, ref speed_UpgradesLvl,true);}
    public void AddHpRegen(){UpgradeAfterStartingVal(ref player.hpRegenEnabled,ref player.hpRegenAmnt,0.1f,0.2f,hpRegen_UpgradeAmnt,hpRegen_UpgradeCost, false, ref player.hpRegenAmnt,ref hpRegen_UpgradesCount,ref hpRegen_UpgradesLvl,false);}
    public void AddEnRegen(){UpgradeAfterStartingVal(ref player.enRegenEnabled,ref player.enRegenAmnt,0.5f,1f,enRegen_UpgradeAmnt,enRegen_UpgradeCost, false, ref player.enRegenAmnt,ref enRegen_UpgradesCount,ref enRegen_UpgradesLvl,false);}

    public void DefaultPowerupChange(string prevPowerup,string powerup,int cost,bool add,ref float value,float amnt,bool permament,int upgradeXPamnt){
        if(gameSession.cores>=cost && player.powerupDefault==prevPowerup){player.powerupDefault=powerup;if(permament!=true){player.powerup=powerup;}gameSession.cores-=cost;if(add==true){value+=amnt;}defaultPowerup_upgradeCount++;/*total_UpgradesCount+=upgradeXPamnt;*/if(permament==true){player.losePwrupOutOfEn=false;}GetComponent<AudioSource>().Play();}
        else{AudioManager.instance.Play("Deny");}
    }
    //public void DefaultPowerupL2(){if(gameSession.cores>powerupDefaultL2_UpgradeCost){player.powerupDefault="laser2";gameSession.cores-=powerupDefaultL2_UpgradeCost;total_UpgradesCount++;}else{GetComponent<AudioSource>().Play();}}
    public void DefaultPowerupL2(){DefaultPowerupChange("laser","laser2",defaultPowerup_upgradeCost1,true,ref player.energy,100,false,0);}//defaultPowerup_upgradeCost1+1);}
    public void DefaultPowerupL3(){DefaultPowerupChange("laser2","laser3",defaultPowerup_upgradeCost2,true,ref player.energy,115,false,0);}//defaultPowerup_upgradeCost2);}
    public void DefaultPowerupPerma(){DefaultPowerupChange("laser3","perma",defaultPowerup_upgradeCost3,true,ref player.energy,130,true,0);}//defaultPowerup_upgradeCost3);}

    public void UnlockEnergyRefill(){
        if(gameSession.cores>=energyRefill_upgradeCost && energyRefill_upgraded!=1){player.energyRefillUnlocked=true;gameSession.cores-=energyRefill_upgradeCost;energyRefill_upgraded=1;/*total_UpgradesCount+=energyRefill_upgradeCost;*/GetComponent<AudioSource>().Play();}
        else{AudioManager.instance.Play("Deny");}
    }

    [SerializeField]XPBars barr;
    [SerializeField]XPBars hpreg;
    [SerializeField]XPBars enreg;
    void LevelEverything(){
        if(total_UpgradesCount>=total_UpgradesCountMax){LastBar(total_UpgradesCountMax,"total_UpgradesCount");total_UpgradesCount=Mathf.Clamp(total_UpgradesCount-total_UpgradesCountMax,0,99);total_UpgradesLvl++;}//AudioManager.instance.Play("LvlUp2");}
        var on=false;
        if(upgradeMenuUI.activeSelf==true)on=true;
        if(total_UpgradesLvl==0){
            total_UpgradesCountMax=1;//1 for Lvl 0
            if(lvlbar.ID!=1){
                if(co==null&&on==true){ChangeLvlBar(1,ref lvlbar);}
                }
        }else if(total_UpgradesLvl<=2 && total_UpgradesLvl>0){
            total_UpgradesCountMax=2;//2 for Lvl 1-2
            if(lvlbar.ID!=2){
                if(co==null&&on==true){ChangeLvlBar(2,ref lvlbar);}
                }
        }else if(total_UpgradesLvl<5&&total_UpgradesLvl>2){
            total_UpgradesCountMax=total_UpgradesLvl;
            if(lvlbar.ID!=total_UpgradesLvl){
                if(co==null&&on==true){ChangeLvlBar(total_UpgradesLvl,ref lvlbar);}
                }
        }else if(total_UpgradesLvl>=5&&total_UpgradesLvl<10){
            total_UpgradesCountMax=5;//5-9 is 5 XP
            if(lvlbar.ID!=5){
                if(co==null&&on==true){ChangeLvlBar(5,ref lvlbar);}
                }
        }else if(total_UpgradesLvl>=10){
            total_UpgradesCountMax=10;//10 or above is 10 XP
            if(lvlbar.ID!=6){
                if(co==null&&on==true){ChangeLvlBar(6,ref lvlbar);}
                }
        }
        if(lvlbar.current==null)lvlbar.created=2;
        if(barr==lvlbar){lvlbar.ID=lvlID;lvlbar.created=lvlcr;}
        /*if(statsMenu.activeSelf==true){
            if(hpreg==null)hpreg=GameObject.Find("LvlBarHPReg").GetComponent<XPBars>();
            if(enreg==null)enreg=GameObject.Find("LvlBarEnReg").GetComponent<XPBars>();
            if(barr==hpreg){hpreg.ID=lvlID;hpreg.created=lvlcr;}
            else if(barr==enreg){enreg.ID=lvlID;enreg.created=lvlcr;}
            if(hpreg!=null){
                if(hpRegen_UpgradesLvl>=1&&hpreg.ID!=2){
                    hpRegen_UpgradesCountMax=2;
                    if(co==null){ChangeLvlBar(2,hpreg);}
                }
            }else{Debug.LogError("HPReg Lvl Bar Not Found");}
            if(enreg!=null){
                if(enRegen_UpgradesLvl>=1&&enreg.ID!=2){
                    enRegen_UpgradesCountMax=2;
                    if(co==null){ChangeLvlBar(2,enreg);}
                }
            }else{Debug.LogError("EnReg Lvl Bar Not Found");}
        }*/

        if(maxHealth_UpgradesCount>=maxHealth_UpgradesCountMax){LastBar(maxHealth_UpgradesCountMax,"maxHealth_UpgradesCount");maxHealth_UpgradesCount=0;maxHealth_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}
        if(maxEnergy_UpgradesCount>=maxEnergy_UpgradesCountMax){LastBar(maxEnergy_UpgradesCountMax,"maxEnergy_UpgradesCount");maxEnergy_UpgradesCount=0;maxEnergy_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}
        if(speed_UpgradesCount>=speed_UpgradesCountMax){LastBar(speed_UpgradesCountMax,"speed_UpgradesCount");speed_UpgradesCount=0;speed_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}
        /**/if(hpRegen_UpgradesCount==1 && hpRegen_UpgradesLvl==0){hpRegen_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}
        if(hpRegen_UpgradesCount>=hpRegen_UpgradesCountMax){LastBar(hpRegen_UpgradesCountMax,"hpRegen_UpgradesCount");hpRegen_UpgradesCount=0;hpRegen_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}
        /**/if(enRegen_UpgradesCount==1 && enRegen_UpgradesLvl==0){enRegen_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}
        if(enRegen_UpgradesCount>=enRegen_UpgradesCountMax){LastBar(enRegen_UpgradesCountMax,"enRegen_UpgradesCount");enRegen_UpgradesCount=0;enRegen_UpgradesLvl++;gameSession.AddToScoreNoEV(75);}
        

        if(maxHealth_UpgradesLvl>0)maxHealth_UpgradeCost=maxHealth_UpgradesLvl;
        if(maxEnergy_UpgradesLvl>0)maxEnergy_UpgradeCost=maxEnergy_UpgradesLvl;
        if(speed_UpgradesLvl>0)speed_UpgradeCost=speed_UpgradesLvl;
        if(hpRegen_UpgradesLvl>0)hpRegen_UpgradeCost=hpRegen_UpgradesLvl;
        if(enRegen_UpgradesLvl>0)enRegen_UpgradeCost=enRegen_UpgradesLvl;
    }
    void LastBar(int max,string name){
        foreach(XPFill obj in FindObjectsOfType<XPFill>()){
            if(obj.valueReq==max&&obj.valueName==name){obj.UpgradeParticles();}
        }
    }
    void ChangeLvlBar(int ID, ref XPBars bar){
        bar.ID=ID;
        bar.Recreate();
    }

    public void CheatCores(){
        gameSession.CheckCodes(-1,0);
        gameSession.CheckCodes(2,6);
        //gameSession.CheckCodes(-1,9);
    }public void CheatLevels(){
        gameSession.CheckCodes(-1,0);
        gameSession.CheckCodes(2,7);
        //gameSession.CheckCodes(-1,9);
    }public void CheatXP(){
        gameSession.coresXp=gameSession.xp_forCore;
    }
}