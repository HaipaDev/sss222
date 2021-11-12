using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UpgradeMenu : MonoBehaviour{
    public static UpgradeMenu instance;
    public static bool UpgradeMenuIsOpen=false;
    public GameObject upgradeMenuUI;
    public GameObject upgradeMenu2UI;
    public GameObject lvltreeUI;
    public GameObject lvltreeUI1;
    public GameObject lvltreeUI2;
    public GameObject invMenu;
    public GameObject statsMenu;
    public GameObject skillsMenu;
    public GameObject backButton;
    public XPBars lvlbar;
    public float prevGameSpeed=1f;
    [Header("Upgrade Values")]
    public int total_UpgradesCountMax=5;
    public float maxHealth_UpgradeAmnt=5f;
    public int maxHealth_UpgradeCost=1;
    public int maxHealth_UpgradesCountMax=5;
    public float maxEnergy_UpgradeAmnt=5f;
    public int maxEnergy_UpgradeCost=1;
    public int maxEnergy_UpgradesCountMax=4;
    public float speed_UpgradeAmnt=0.1f;
    public int speed_UpgradeCost=1;
    public int speed_UpgradesCountMax=5;
    public float luck_UpgradeAmnt=0.05f;
    public int luck_UpgradeCost=1;
    public int luck_UpgradesCountMax=5;
    //Modules
    public int defaultPowerup_upgradeCost1=1;
    public int defaultPowerup_upgradeCost2=1;
    public int defaultPowerup_upgradeCost3=4;
    //public int energyRefill_upgradeCost=2;
    //public int energyRefill_upgradeCost2=3;
    public int mPulse_upgradeCost=3;
    public int mPulse_lvlReq=2;
    public int postMortem_upgradeCost=0;
    public int postMortem_lvlReq=5;
    public int teleport_upgradeCost=2;
    public int teleport_lvlReq=3;
    public int overhaul_upgradeCost=3;
    public int overhaul_lvlReq=4;
    public int crMend_upgradeCost=5;
    public int crMend_lvlReq=5;
    public int enDiss_upgradeCost=4;
    public int enDiss_lvlReq=5;
    [Header("Upgrade Counts")]
    public int total_UpgradesCount;
    public int total_UpgradesLvl=0;
    public int maxHealth_UpgradesCount;
    public int maxHealth_UpgradesLvl=1;
    public int maxEnergy_UpgradesCount;
    public int maxEnergy_UpgradesLvl=1;
    public int speed_UpgradesCount;
    public int speed_UpgradesLvl=1;
    public int luck_UpgradesCount;
    public int luck_UpgradesLvl=1;
    //Modules
    public int defaultPowerup_upgradeCount;
    public int mPulse_upgraded;
    public int teleport_upgraded;
    public int overhaul_upgraded;
    public int crMend_upgraded;
    public bool crMendEnabled;
    public int enDiss_upgraded;
    public bool enDissEnabled;
    Player player;
    PlayerSkills pskills;
    IEnumerator co;

    int lvlID;
    int lvlcr;
    float startTimer=0.5f;
    private void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
    yield return new WaitForSeconds(0.07f);
    var i=GameRules.instance;
    if(i!=null){
        total_UpgradesCountMax=i.total_UpgradesCountMax;
        maxHealth_UpgradeAmnt=i.maxHealth_UpgradeAmnt;
        maxHealth_UpgradeCost=i.maxHealth_UpgradeCost;
        maxHealth_UpgradesCountMax=i.maxHealth_UpgradesCountMax;
        maxEnergy_UpgradeAmnt=i.maxEnergy_UpgradeAmnt;
        maxEnergy_UpgradeCost=i.maxEnergy_UpgradeCost;
        maxEnergy_UpgradesCountMax=i.maxEnergy_UpgradesCountMax;
        speed_UpgradeAmnt=i.speed_UpgradeAmnt;
        speed_UpgradeCost=i.speed_UpgradeCost;
        speed_UpgradesCountMax=i.speed_UpgradesCountMax;
        luck_UpgradeAmnt=i.luck_UpgradeAmnt;
        luck_UpgradeCost=i.luck_UpgradeCost;
        luck_UpgradesCountMax=i.luck_UpgradesCountMax;
        //Modules
        defaultPowerup_upgradeCost1=i.defaultPowerup_upgradeCost1;
        defaultPowerup_upgradeCost2=i.defaultPowerup_upgradeCost2;
        defaultPowerup_upgradeCost3=i.defaultPowerup_upgradeCost3;
        //energyRefill_upgradeCost=i.energyRefill_upgradeCost;
        //energyRefill_upgradeCost2=i.energyRefill_upgradeCost2;
        mPulse_upgradeCost=i.mPulse_upgradeCost;
        mPulse_lvlReq=i.mPulse_lvlReq;
        postMortem_upgradeCost=i.postMortem_upgradeCost;
        postMortem_lvlReq=i.postMortem_lvlReq;
        teleport_upgradeCost=i.teleport_upgradeCost;
        teleport_lvlReq=i.teleport_lvlReq;
        overhaul_upgradeCost=i.overhaul_upgradeCost;
        overhaul_lvlReq=i.overhaul_lvlReq;
        crMend_upgradeCost=i.crMend_upgradeCost;
        crMend_lvlReq=i.crMend_lvlReq;
        enDiss_upgradeCost=i.enDiss_upgradeCost;
        enDiss_lvlReq=i.enDiss_lvlReq;
    }
    }
    void Start(){
        instance=this;
        player=Player.instance;
        pskills=Player.instance.GetComponent<PlayerSkills>();
        Resume();
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.F)){
            if(UpgradeMenuIsOpen){Resume();
            }else{if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && Player.instance!=null)Open();}
        }
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace)){if(upgradeMenu2UI.activeSelf || lvltreeUI.activeSelf){Back();Open();return;}
        else if(!(upgradeMenu2UI.activeSelf && lvltreeUI.activeSelf) && upgradeMenuUI.activeSelf){Resume();return;}}
        LevelEverything();
    }

    public void Resume(){
        upgradeMenuUI.SetActive(false);
        upgradeMenu2UI.SetActive(false);
        lvltreeUI.SetActive(false);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        GameSession.instance.gameSpeed=prevGameSpeed;
        UpgradeMenuIsOpen=false;
    }
    public void Open(){
        prevGameSpeed=GameSession.instance.gameSpeed;
        upgradeMenuUI.SetActive(true);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        GameSession.instance.gameSpeed=0f;
        UpgradeMenuIsOpen=true;
        StartCoroutine(ForceLayoutUpdate());
        //ParticleSystem.Stop();
        //var ptSystems = FindObjectOfType<ParticleSystem>();
        //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
    }

    IEnumerator ForceLayoutUpdate(){
        yield return new WaitForSecondsRealtime(0.02f);
        GameObject.Find("Container-Buttons").GetComponent<Image>().enabled=false;
        yield return new WaitForSecondsRealtime(0.02f);
        GameObject.Find("Container-Buttons").GetComponent<Image>().enabled=true;//force update layout
    }

    public void PreviousGameSpeed(){GameSession.instance.gameSpeed=prevGameSpeed;}

    
    public void OpenInv(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        invMenu.SetActive(true);
        statsMenu.SetActive(false);
        skillsMenu.SetActive(false);
    }
    public void OpenStats(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        invMenu.SetActive(false);
        statsMenu.SetActive(false);
        skillsMenu.SetActive(true);
    }
    public void OpenSkills(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        invMenu.SetActive(false);
        statsMenu.SetActive(false);
        skillsMenu.SetActive(true);
    }
    public void OpenLvlTree(){
        upgradeMenuUI.SetActive(false);
        lvltreeUI.SetActive(true);
        OpenLvlTree1();
    }public void OpenLvlTree1(){
        lvltreeUI1.SetActive(true);
        lvltreeUI2.SetActive(false);
    }public void OpenLvlTree2(){
        lvltreeUI2.SetActive(true);
        lvltreeUI1.SetActive(false);
    }public void NextLvlTree(){
        if(lvltreeUI1.activeSelf){lvltreeUI2.SetActive(true);lvltreeUI1.SetActive(false);return;}
        if(lvltreeUI2.activeSelf){lvltreeUI1.SetActive(true);lvltreeUI2.SetActive(false);return;}
    }
    public void Back(){
        statsMenu.SetActive(false);skillsMenu.SetActive(false);
        upgradeMenu2UI.SetActive(false);lvltreeUI.SetActive(false);
        upgradeMenuUI.SetActive(true);
    }

    public void UnlockSkillUni(int ID, ref int value,int number,int cost){
        if(GameSession.instance.cores>=cost && value==number-1){value=number;GameSession.instance.cores-=cost;GetComponent<AudioSource>().Play();
        var skills=Player.instance.GetComponent<PlayerSkills>().skillsBinds;
        for(var i=0;i<skills.Length;i++){if(i!=ID)if(skills[i]==skillKeyBind.Q && skills[i]!=skillKeyBind.E){skills[ID]=skillKeyBind.E;}}//Set to E if Q is occuppied
        for(var i=0;i<skills.Length;i++){if(i!=ID)if(skills[i]!=skillKeyBind.Q){skills[ID]=skillKeyBind.Q;}}//Set to Q by default
        }
        else{AudioManager.instance.Play("Deny");}
    }
    public void UnlockSkill(int ID){
        if(ID==0){UnlockSkillUni(ID,ref mPulse_upgraded,1,mPulse_upgradeCost);}
        if(ID==1){UnlockSkillUni(ID,ref teleport_upgraded,1,teleport_upgradeCost);}
        if(ID==2){UnlockSkillUni(ID,ref overhaul_upgraded,1,overhaul_upgradeCost);}
    }
    public void UpgradeSkill(int ID){
        if(ID==0){UnlockSkillUni(ID,ref mPulse_upgraded,2,postMortem_upgradeCost);}
        //if(ID==1){UnlockSkillUni(ID,ref teleport_upgraded,2,1);}
    }
    public void SetSkillQ(int ID){
        var skills=Player.instance.GetComponent<PlayerSkills>().skillsBinds;

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
                //pskills.cooldownE=Q;
                //pskills.cooldownQ=E;
            }
        }
        /*if(skills[ID]!=skillKeyBind.E){
            var Q=pskills.cooldownQ;
            var E=pskills.cooldownE;
            pskills.cooldownE=Q;
            pskills.cooldownQ=E;
        }*/
        skills[ID]=skillKeyBind.Q;
        /*var skills=Player.instance.GetComponent<PlayerSkills>().skills;
        foreach(SkillSlotID skillOther in skills){skillOther.keySet=skillKeyBind.Disabled;}
        var skill=skills[ID];
        skill.keySet=skillKeyBind.Q;*/
    }public void SetSkillE(int ID){
        var skills=Player.instance.GetComponent<PlayerSkills>().skillsBinds;
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
                //pskills.cooldownE=Q;
                //pskills.cooldownQ=E;
            }
        }
        skills[ID]=skillKeyBind.E;
    }

    public void UpgradeFloat(ref float value,float amnt,int cost,bool add,ref float value2,ref int countValue, ref int countLvl,bool bypass){
        if(GameSession.instance.cores>=cost&&(bypass==true||(bypass==false&&countLvl<=total_UpgradesLvl))){value+=amnt;value=(float)System.Math.Round(value,2);GameSession.instance.cores-=cost;if(add==true){value2+=amnt*2;}countValue++;/*total_UpgradesCount++;*/
        GetComponent<AudioSource>().Play();}//var go=EventSystem.current.currentSelectedGameObject; go.GetComponent<Image>().color=new Color(79,169,107); go.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=new Color(49,188,80);}
        else{AudioManager.instance.Play("Deny");}//var go=EventSystem.current.currentSelectedGameObject; go.GetComponent<Image>().color=Color.red; go.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;}
    }/*public void UpgradeAfterStartingVal(ref bool valueEnable,ref float value,float startingVal,float secondVal,float amnt,int cost,bool add,ref float value2,ref int countValue,ref int countLvl, bool bypass){
        if(GameSession.instance.cores>=cost&&(bypass==true||(bypass==false&&countLvl<=total_UpgradesLvl&&total_UpgradesLvl>0))){if(valueEnable!=true){valueEnable=true;}if(value>=secondVal){value+=amnt;}else{if(value==startingVal&&(countLvl>1))value=secondVal;}value=(float)System.Math.Round(value,2);GameSession.instance.cores-=cost;if(add==true){value2+=amnt*2;}countValue++;/*total_UpgradesCount++;*/
    //    GetComponent<AudioSource>().Play();}//var go=EventSystem.current.currentSelectedGameObject; go.GetComponent<Image>().color=new Color(79,169,107); go.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=new Color(49,188,80);}
    //    else{AudioManager.instance.Play("Deny");}//var go=EventSystem.current.currentSelectedGameObject; go.GetComponent<Image>().color=Color.red; go.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;}
    //}*/
    //public void AddFloat(ref float value,float amnt,int cost){if(GameSession.instance.cores>=cost){value+=amnt;}}
    //if(GameSession.instance.cores>=maxHealth_UpgradeCost)player.maxHP+=maxHealth_UpgradeAmnt;GameSession.instance.cores-=maxHealth_UpgradeCost;
    public void AddMaxHP(){UpgradeFloat(ref player.maxHP,maxHealth_UpgradeAmnt,maxHealth_UpgradeCost, true, ref player.health,ref maxHealth_UpgradesCount, ref maxHealth_UpgradesLvl,false);}
    public void AddMaxEnergy(){UpgradeFloat(ref player.maxEnergy,maxEnergy_UpgradeAmnt,maxEnergy_UpgradeCost, true, ref player.energy,ref maxEnergy_UpgradesCount, ref maxEnergy_UpgradesLvl,false);}
    public void AddSpeed(){UpgradeFloat(ref player.moveSpeed,speed_UpgradeAmnt,speed_UpgradeCost, false, ref player.moveSpeedCurrent,ref speed_UpgradesCount, ref speed_UpgradesLvl,false);}
    public void AddSpeedBypass(){UpgradeFloat(ref player.moveSpeed,speed_UpgradeAmnt/2,0, false, ref player.moveSpeedCurrent,ref speed_UpgradesCount, ref speed_UpgradesLvl,true);}
    //public void AddHpRegen(){UpgradeAfterStartingVal(ref player.hpRegenEnabled,ref player.hpRegenAmnt,0.1f,0.2f,hpRegen_UpgradeAmnt,hpRegen_UpgradeCost, false, ref player.hpRegenAmnt,ref hpRegen_UpgradesCount,ref hpRegen_UpgradesLvl,false);}
    //public void AddEnRegen(){UpgradeAfterStartingVal(ref player.enRegenEnabled,ref player.enRegenAmnt,0.5f,1f,enRegen_UpgradeAmnt,enRegen_UpgradeCost, false, ref player.enRegenAmnt,ref enRegen_UpgradesCount,ref enRegen_UpgradesLvl,false);}
    public void AddLuck(){UpgradeFloat(ref GameSession.instance.luckMulti,luck_UpgradeAmnt,luck_UpgradeCost, false, ref GameSession.instance.luckMulti,ref luck_UpgradesCount, ref luck_UpgradesLvl,false);}

    public void DefaultPowerupChange(string prevPowerup,string powerup,int cost,bool add,ref float value,float amnt,bool permament,int upgradeXPamnt){
        if(GameSession.instance.cores>=cost && player.powerupDefault==prevPowerup){player.powerupDefault=powerup;if(permament!=true){player.powerup=powerup;}GameSession.instance.cores-=cost;if(add==true){value+=amnt;}defaultPowerup_upgradeCount++;/*total_UpgradesCount+=upgradeXPamnt;*/if(permament==true){player.losePwrupOutOfEn=false;}GetComponent<AudioSource>().Play();}
        else{AudioManager.instance.Play("Deny");}
    }
    //public void DefaultPowerupL2(){if(GameSession.instance.cores>powerupDefaultL2_UpgradeCost){player.powerupDefault="laser2";GameSession.instance.cores-=powerupDefaultL2_UpgradeCost;total_UpgradesCount++;}else{GetComponent<AudioSource>().Play();}}
    public void DefaultPowerupL2(){DefaultPowerupChange("laser","laser2",defaultPowerup_upgradeCost1,true,ref player.energy,100,false,0);}//defaultPowerup_upgradeCost1+1);}
    public void DefaultPowerupL3(){DefaultPowerupChange("laser2","laser3",defaultPowerup_upgradeCost2,true,ref player.energy,115,false,0);}//defaultPowerup_upgradeCost2);}
    public void DefaultPowerupPerma(){DefaultPowerupChange("laser3","perma",defaultPowerup_upgradeCost3,true,ref player.energy,130,true,0);}//defaultPowerup_upgradeCost3);}
    public void UnlockCrystalMend(){if(crMend_upgraded<=0){crMend_upgraded=1;crMendEnabled=true;GameSession.instance.cores-=crMend_upgradeCost;}}
    public void SwitchCrMend(){if(crMend_upgraded>=1){if(crMendEnabled==true){crMendEnabled=false;return;}if(crMendEnabled==false){crMendEnabled=true;Player.instance.Damage(5,dmgType.silent);return;}}}
    public void UnlockEnergyDiss(){if(enDiss_upgraded<=0){enDiss_upgraded=1;enDissEnabled=true;GameSession.instance.cores-=enDiss_upgradeCost;}}
    public void SwitchEnDiss(){if(enDiss_upgraded>=1){if(enDissEnabled==true){enDissEnabled=false;return;}if(enDissEnabled==false){enDissEnabled=true;Player.instance.AddSubEnergy(10,false);return;}}}

    /*public void UnlockEnergyRefill(){
        if(GameSession.instance.cores>=energyRefill_upgradeCost && energyRefill_upgraded!=1){player.energyRefillUnlocked=1;GameSession.instance.cores-=energyRefill_upgradeCost;energyRefill_upgraded=1;GetComponent<AudioSource>().Play();}
        else{AudioManager.instance.Play("Deny");}
    }public void UnlockEnergyRefill2(){
        if(GameSession.instance.cores>=energyRefill_upgradeCost2 && energyRefill_upgraded==1){player.energyRefillUnlocked=2;GameSession.instance.cores-=energyRefill_upgradeCost;energyRefill_upgraded=2;GetComponent<AudioSource>().Play();}
        else{AudioManager.instance.Play("Deny");}
    }*/

    [SerializeField]XPBars barr;
    void LevelEverything(){
    if(startTimer>0){startTimer-=Time.unscaledDeltaTime;}
    if(startTimer<=0){
        if(GameSession.instance.levelingOn){
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
            if(total_UpgradesCount>=total_UpgradesCountMax){
                LastBar(total_UpgradesCountMax,"total_UpgradesCount");total_UpgradesCount=Mathf.Clamp(total_UpgradesCount-total_UpgradesCountMax,0,99);
                LevelUp();
                total_UpgradesLvl++;
                UpgradeMenu.instance.LvlEvents();
                }
            if(lvlbar.current==null)lvlbar.created=2;
            if(barr==lvlbar){lvlbar.ID=lvlID;lvlbar.created=lvlcr;}
        }
        

        if(maxHealth_UpgradesCount==1 && maxHealth_UpgradesLvl==0){maxHealth_UpgradesLvl++;GameSession.instance.AddToScoreNoEV(15);}
        if(maxHealth_UpgradesCount>=maxHealth_UpgradesCountMax){LastBar(maxHealth_UpgradesCountMax,"maxHealth_UpgradesCount");maxHealth_UpgradesCount=0;maxHealth_UpgradesLvl++;GameSession.instance.AddToScoreNoEV(75);}
        if(maxEnergy_UpgradesCount==1 && maxEnergy_UpgradesLvl==0){maxEnergy_UpgradesLvl++;GameSession.instance.AddToScoreNoEV(15);}
        if(maxEnergy_UpgradesCount>=maxEnergy_UpgradesCountMax){LastBar(maxEnergy_UpgradesCountMax,"maxEnergy_UpgradesCount");maxEnergy_UpgradesCount=0;maxEnergy_UpgradesLvl++;GameSession.instance.AddToScoreNoEV(75);}
        if(speed_UpgradesCount==1 && speed_UpgradesLvl==0){speed_UpgradesLvl++;GameSession.instance.AddToScoreNoEV(15);}
        if(speed_UpgradesCount>=speed_UpgradesCountMax){LastBar(speed_UpgradesCountMax,"speed_UpgradesCount");speed_UpgradesCount=0;speed_UpgradesLvl++;GameSession.instance.AddToScoreNoEV(75);}
        if(luck_UpgradesCount==1 && luck_UpgradesLvl==0){luck_UpgradesLvl++;GameSession.instance.AddToScoreNoEV(15);}
        if(luck_UpgradesCount>=luck_UpgradesCountMax){LastBar(luck_UpgradesCountMax,"luck_UpgradesCount");luck_UpgradesCount=0;luck_UpgradesLvl++;GameSession.instance.AddToScoreNoEV(75);}


        if(maxHealth_UpgradesLvl>0)maxHealth_UpgradeCost=maxHealth_UpgradesLvl;
        if(maxEnergy_UpgradesLvl>0)maxEnergy_UpgradeCost=maxEnergy_UpgradesLvl;
        if(speed_UpgradesLvl>0)speed_UpgradeCost=speed_UpgradesLvl;
        if(luck_UpgradesLvl>0)luck_UpgradeCost=luck_UpgradesLvl;

        if(total_UpgradesLvl>=postMortem_lvlReq&&mPulse_upgraded==1){mPulse_upgraded=2;}
    }
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
    void LevelUp(){
        AudioManager.instance.Play("LvlUp2");
        FindObjectOfType<OnScreenButtons>().transform.GetChild(0).GetComponent<Animator>().SetTrigger("on");
    }
    public void LvlEvents(){
        if(GameRules.instance!=null&&Player.instance!=null){
        foreach(ListEvents le in GameRules.instance.lvlEvents){
            if(le.lvls.x==0&&le.lvls.y==0){le.events.Invoke();}
            else{if(UpgradeMenu.instance.total_UpgradesLvl>=le.lvls.x&&UpgradeMenu.instance.total_UpgradesLvl<=le.lvls.y){le.events.Invoke();}}
        }}
    }
    public void CheatCores(){
        GameSession.instance.CheckCodes("Del","0");
        GameSession.instance.CheckCodes("2","Y");
        //GameSession.instance.CheckCodes("Del","9");
    }public void CheatLevels(){
        GameSession.instance.CheckCodes("Del","0");
        GameSession.instance.CheckCodes("2","U");
        //GameSession.instance.CheckCodes("Del","9");
    }public void CheatXP(){GameSession.instance.xp=GameSession.instance.xp_max;}
}