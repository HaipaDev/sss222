using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class UpgradeMenu : MonoBehaviour{       public static UpgradeMenu instance;
    public static bool UpgradeMenuIsOpen=false;
    [SceneObjectsOnly]public GameObject upgradeMenuUI;
    [SceneObjectsOnly]public GameObject upgradeMenu2UI;
    [SceneObjectsOnly]public GameObject lvltreeUI;
    [SceneObjectsOnly]public GameObject lvltreeUI1;
    [SceneObjectsOnly]public GameObject lvltreeUI2;
    [SceneObjectsOnly]public GameObject invMenu;
    [SceneObjectsOnly]public GameObject statsMenu;
    [SceneObjectsOnly]public GameObject modulesSkillsPanel;
    [SceneObjectsOnly]public GameObject modulesSkillsInventory;
    [SceneObjectsOnly]public GameObject modulesList;
    [SceneObjectsOnly]public GameObject skillsList;
    [SceneObjectsOnly]public GameObject backButton;
    [SceneObjectsOnly]public XPBars lvlbar;
    [AssetsOnly]public GameObject moduleSkillElementPrefab;
    [AssetsOnly]public GameObject moduleSlotPrefab;
    [AssetsOnly]public GameObject skillSlotPrefab;
    [Header("Upgrade Values")]
    public int total_UpgradesCountMax=5;
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
    public int saveBarsFromLvl=5;
    public int total_UpgradesLvl=0;
    public int mPulse_upgraded;
    public int teleport_upgraded;
    public int overhaul_upgraded;
    public int crMend_upgraded;
    public int enDiss_upgraded;
    PlayerModules pmodules;
    IEnumerator co;

    [DisableInEditorMode][SerializeField]int selectedModuleSlot=-1;
    [DisableInEditorMode][SerializeField]int selectedSkillSlot=-1;
    int lvlID;
    int lvlcr;
    float startTimer=0.5f;

    void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
    yield return new WaitForSeconds(0.07f);
    var i=GameRules.instance;
    if(i!=null){
        saveBarsFromLvl=i.saveBarsFromLvl;
        total_UpgradesCountMax=i.total_UpgradesCountMax;
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
        pmodules=Player.instance.GetComponent<PlayerModules>();
        if(GameSession.instance.CheckGamemodeSelected("Adventure")){LvlEventsAdventure();}
        SetupModulesAndSkills();

        Resume();
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.F)||Input.GetKeyDown(KeyCode.JoystickButton3)){
            if(UpgradeMenuIsOpen){Resume();}
            else{if(PauseMenu.GameIsPaused!=true&&Shop.shopOpened!=true&&Player.instance!=null)if(!Player.instance._hasStatus("hacked"))Open();}
        }
        if(GSceneManager.EscPressed()||Input.GetKeyDown(KeyCode.Backspace)||Input.GetKeyDown(KeyCode.JoystickButton1)){Back();}
        LevelEverything();
        //SetModulesAndSkillsPreviews();
    }

    public void Resume(){
        upgradeMenuUI.SetActive(false);
        upgradeMenu2UI.SetActive(false);
        lvltreeUI.SetActive(false);
        BackToModulesSkillsInventory();
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
        UpgradeMenuIsOpen=false;
    }
    public void Open(){
        upgradeMenuUI.SetActive(true);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        /*if(GameRules.instance.upgradeMenuPause)*/GameSession.instance.gameSpeed=GameRules.instance.upgradeMenuOpenGameSpeed;
        //else{GameSession.instance.gameSpeed=GameRules.instance.upgradeMenuSlowdownSpeed;}
        UpgradeMenuIsOpen=true;
        StartCoroutine(ForceLayoutUpdate());
    }

    IEnumerator ForceLayoutUpdate(){
        yield return new WaitForSecondsRealtime(0.02f);
        GameObject.Find("Container-Buttons").GetComponent<Image>().enabled=false;
        yield return new WaitForSecondsRealtime(0.02f);
        GameObject.Find("Container-Buttons").GetComponent<Image>().enabled=true;//force update layout
    }

    
    public void OpenInv(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        invMenu.SetActive(true);
        statsMenu.SetActive(false);
        modulesSkillsPanel.SetActive(false);
    }
    public void OpenStats(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        invMenu.SetActive(false);
        statsMenu.SetActive(false);
        modulesSkillsPanel.SetActive(true);
    }
    public void OpenModules(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        invMenu.SetActive(false);
        statsMenu.SetActive(false);
        modulesSkillsPanel.SetActive(true);
        SetModulesAndSkillsPreviews();
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
        if(modulesList.activeSelf||skillsList.activeSelf){BackToModulesSkillsInventory();return;}
        if(statsMenu.activeSelf||modulesSkillsPanel.activeSelf){Resume();
            statsMenu.SetActive(false);modulesSkillsPanel.SetActive(false);
            upgradeMenu2UI.SetActive(false);lvltreeUI.SetActive(false);
            return;
        }
        upgradeMenuUI.SetActive(true);
        if(upgradeMenuUI.activeSelf){Resume();}
    }

    public void CloseAllModulesSkills(){
        modulesSkillsInventory.SetActive(false);
        modulesList.SetActive(false);
        skillsList.SetActive(false);
    }
    public void BackToModulesSkillsInventory(){CloseAllModulesSkills();modulesSkillsInventory.SetActive(true);selectedModuleSlot=-1;selectedSkillSlot=-1;SetModulesAndSkillsPreviews();}
    public void OpenModulesList(int id){selectedModuleSlot=id;CloseAllModulesSkills();modulesList.SetActive(true);}
    public void OpenSkillsList(int id){selectedSkillSlot=id;CloseAllModulesSkills();skillsList.SetActive(true);}
    public void SetModuleSlot(string name){
        if(pmodules._isModuleEquipped(name)){pmodules.ClearModule(name);}
        pmodules.SetModule(selectedModuleSlot,name);
        BackToModulesSkillsInventory();
    }
    public void SetSkillSlot(string name){
        if(pmodules._isSkillEquipped(name)){pmodules.ClearSkill(name);}
        pmodules.SetSkill(selectedSkillSlot,name);
        BackToModulesSkillsInventory();
    }

    void SetupModulesAndSkills(){
        var modulesSkillsSlotsContainer=modulesSkillsInventory.transform.GetChild(0);
        var modulesSlotsContainer=modulesSkillsSlotsContainer.transform.GetChild(0).GetChild(1);
        var skillsSlotsContainer=modulesSkillsSlotsContainer.transform.GetChild(1).GetChild(1);
        ///ModuleSlots
        foreach(Transform t in modulesSlotsContainer){Destroy(t.gameObject);}
        for(var i=0;i<GameRules.instance.playerModulesCapacity;i++){
            var go=Instantiate(moduleSlotPrefab,modulesSlotsContainer);
            go.name="ModuleSlot"+i;
            var _i=i;go.GetComponent<Button>().onClick.AddListener(()=>OpenModulesList(_i));
            //go.GetComponent<Button>().onClick.AddListener(()=>OpenModulesList(i));
           // go.GetComponent<Button>().onClick.SetPersistentListenerState(0,);
        }
        //var _i=0;foreach(Transform t in modulesSlotsContainer){t.GetComponent<Button>().onClick.AddListener(()=>OpenModulesList(_i));Debug.Log(_i);_i++;}

        ///SkillSlots
        foreach(Transform t in skillsSlotsContainer){Destroy(t.gameObject);}
        for(var i=0;i<2/*GameRules.instance.playerSkillsCapacity*/;i++){
            var go=Instantiate(skillSlotPrefab,skillsSlotsContainer);
            go.name="SkillSlot"+i;
            var _i=i;go.GetComponent<Button>().onClick.AddListener(()=>OpenSkillsList(_i));
            var _key="Q";if(i==1){_key="E";}
            go.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text=_key;
        }
        //_i=0;foreach(Transform t in skillsSlotsContainer){t.GetComponent<Button>().onClick.AddListener(()=>OpenSkillsList(_i));_i++;}

        ///Modules
        var modulesContainer=modulesList.transform.GetChild(0);
        foreach(Transform t in modulesContainer){Destroy(t.gameObject);}
        var goModule0=Instantiate(moduleSkillElementPrefab,modulesContainer);
        goModule0.name="Empty";
        goModule0.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="Empty";
        goModule0.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text="";
        goModule0.transform.GetChild(1).GetComponent<TextMeshProUGUI>().colorGradient=new VertexGradient(Color.white);
        goModule0.transform.GetChild(2).GetComponent<Image>().sprite=moduleSlotPrefab.GetComponent<Image>().sprite;
        foreach(Transform t in goModule0.transform.GetChild(2)){Destroy(t.gameObject);}
        goModule0.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(()=>SetModuleSlot(""));
        Destroy(goModule0.transform.GetChild(3).GetChild(1).gameObject);
        Destroy(goModule0.transform.GetChild(3).GetChild(0).gameObject);
        Destroy(goModule0.transform.GetChild(4).gameObject);

        foreach(ModulePropertiesGR m in GameRules.instance.modulesPlayer){
            var go=Instantiate(moduleSkillElementPrefab,modulesContainer);
            go.name=m.item.name;
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=m.item.name;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=m.item.desc;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().colorGradient=new VertexGradient(m.item.descGradient.topLeft,m.item.descGradient.topRight,m.item.descGradient.bottomLeft,m.item.descGradient.bottomRight);
            go.transform.GetChild(2).GetComponent<Image>().sprite=moduleSlotPrefab.GetComponent<Image>().sprite;
            if(m.item.sprite!=null){go.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite=m.item.sprite;}
            else{
                foreach(Transform t in go.transform.GetChild(2)){Destroy(t.gameObject);}
                if(m.item.iconsGo!=null){Instantiate(m.item.iconsGo,go.transform.GetChild(2));}
            }
            go.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(()=>SetModuleSlot(m.item.name));
            go.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text=m.coreCost.ToString();
            go.transform.GetChild(4).GetComponent<ShipLevelRequired>().value=m.lvlReq;
        }

        ///Skills
        var skillsContainer=skillsList.transform.GetChild(0);
        foreach(Transform t in skillsContainer){Destroy(t.gameObject);}
        var goSkill0=Instantiate(moduleSkillElementPrefab,skillsContainer);
        goSkill0.name="Empty";
        goSkill0.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="Empty";
        goSkill0.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text="";
        goSkill0.transform.GetChild(1).GetComponent<TextMeshProUGUI>().colorGradient=new VertexGradient(Color.white);
        goSkill0.transform.GetChild(2).GetComponent<Image>().sprite=skillSlotPrefab.GetComponent<Image>().sprite;
        foreach(Transform t in goSkill0.transform.GetChild(2)){Destroy(t.gameObject);}
        goSkill0.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(()=>SetSkillSlot(""));
        Destroy(goSkill0.transform.GetChild(3).GetChild(1).gameObject);
        Destroy(goSkill0.transform.GetChild(3).GetChild(0).gameObject);
        Destroy(goSkill0.transform.GetChild(4).gameObject);

        foreach(SkillPropertiesGR s in GameRules.instance.skillsPlayer){
            var go=Instantiate(moduleSkillElementPrefab,skillsContainer);
            go.name=s.item.name;
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=s.item.name;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=s.item.desc;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().colorGradient=new VertexGradient(s.item.descGradient.topLeft,s.item.descGradient.topRight,s.item.descGradient.bottomLeft,s.item.descGradient.bottomRight);
            go.transform.GetChild(2).GetComponent<Image>().sprite=skillSlotPrefab.GetComponent<Image>().sprite;
            if(s.item.sprite!=null){go.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite=s.item.sprite;}
            else{
                foreach(Transform t in go.transform.GetChild(2)){Destroy(t.gameObject);}
                if(s.item.iconsGo!=null){Instantiate(s.item.iconsGo,go.transform.GetChild(2));}
            }
            go.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(()=>SetSkillSlot(s.item.name));
            go.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text=s.coreCost.ToString();
            go.transform.GetChild(4).GetComponent<ShipLevelRequired>().value=s.lvlReq;
        }
    }

    //void SetModulesAndSkillsPreviews(){StartCoroutine(SetModulesAndSkillsPreviewsI());}
    //IEnumerator SetModulesAndSkillsPreviewsI(){
    void SetModulesAndSkillsPreviews(){
        //yield return new WaitForSecondsRealtime(0.05f);
        var modulesSkillsSlotsContainer=modulesSkillsInventory.transform.GetChild(0);
        var modulesSlotsContainer=modulesSkillsSlotsContainer.transform.GetChild(0).GetChild(1);
        var skillsSlotsContainer=modulesSkillsSlotsContainer.transform.GetChild(1).GetChild(1);

        for(var i=0;i<modulesSlotsContainer.childCount;i++){
            var t=modulesSlotsContainer.GetChild(i);
            if(pmodules.moduleSlots[i]!=""){
                var m=pmodules.GetModuleProperties(pmodules.moduleSlots[i]);
                if(t.GetComponent<Image>().material!=null)t.GetComponent<Image>().material=null;
                if(m.item.sprite!=null){
                    if(t.GetChild(0).childCount>0){foreach(Transform tt in t.GetChild(0))Destroy(tt.gameObject);}
                    t.GetChild(0).GetComponent<Image>().enabled=true;
                    t.GetChild(0).GetComponent<Image>().sprite=m.item.sprite;
                }else{
                    t.GetChild(0).GetComponent<Image>().enabled=false;
                    if(t.GetChild(0).childCount>0){foreach(Transform tt in t.GetChild(0))Destroy(tt.gameObject);}
                    if(m.item.iconsGo!=null)Instantiate(m.item.iconsGo,t.GetChild(0));
                }
            }else{
                t.GetComponent<Image>().material=GameAssets.instance.GetMat("GrayedOut");
                t.GetChild(0).GetComponent<Image>().enabled=false;
                if(t.GetChild(0).childCount>0){foreach(Transform tt in t.GetChild(0))Destroy(tt.gameObject);}
            }
        }

        for(var i=0;i<skillsSlotsContainer.childCount;i++){
            var t=skillsSlotsContainer.GetChild(i);
            if(pmodules.skillsSlots[i]!=""){
                var s=pmodules.GetSkillProperties(pmodules.skillsSlots[i]);
                if(t.GetComponent<Image>().material!=null)t.GetComponent<Image>().material=null;
                if(s.item.sprite!=null){
                    if(t.GetChild(0).childCount>0){foreach(Transform tt in t.GetChild(0))Destroy(tt.gameObject);}
                    t.GetChild(0).GetComponent<Image>().enabled=true;
                    t.GetChild(0).GetComponent<Image>().sprite=s.item.sprite;
                }else{
                    t.GetChild(0).GetComponent<Image>().enabled=false;
                    if(t.GetChild(0).childCount>0){foreach(Transform tt in t.GetChild(0))Destroy(tt.gameObject);}
                    if(s.item.iconsGo!=null)Instantiate(s.item.iconsGo,t.GetChild(0));
                }
            }else{
                t.GetComponent<Image>().material=GameAssets.instance.GetMat("GrayedOut");
                t.GetChild(0).GetComponent<Image>().enabled=false;
                if(t.GetChild(0).childCount>0){foreach(Transform tt in t.GetChild(0))Destroy(tt.gameObject);}
            }
        }
    }

    public void UnlockSkillUni(int ID, ref int value,int number,int cost){
        if(GameSession.instance.cores>=cost && value==number-1){
            value=number;GameSession.instance.cores-=cost;GetComponent<AudioSource>().Play();
        }else{AudioManager.instance.Play("Deny");}
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
    XPBars barr;
    void LevelEverything(){
        if(startTimer>0){startTimer-=Time.unscaledDeltaTime;}
        if(startTimer<=0){
            if(GameRules.instance.levelingOn){
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
        FindObjectOfType<OnScreenButtons>().GetComponent<Animator>().SetTrigger("on");
        FindObjectOfType<OnScreenButtons>().lvldUp=true;
    }
    public void LvlEvents(){
        if(GameRules.instance!=null){
        foreach(ListEvents le in GameRules.instance.lvlEvents){
            if(le.lvls.x==0&&le.lvls.y==0){le.events.Invoke();}
            else{if(UpgradeMenu.instance.total_UpgradesLvl>=le.lvls.x&&UpgradeMenu.instance.total_UpgradesLvl<=le.lvls.y&&!le.skipRe){le.events.Invoke();}}
        }}
    }
    public void LvlEventsAdventure(){
        if(GameRules.instance!=null){
        foreach(ListEvents le in GameRules.instance.lvlEvents){
            if(le.lvls.x==0&&le.lvls.y==0){le.events.Invoke();}
            else{if(UpgradeMenu.instance.total_UpgradesLvl>=le.lvls.x&&!le.skipRe){le.events.Invoke();}}
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
    }public void CheatXP(){GameSession.instance.xp=GameSession.instance.xpMax;}
}