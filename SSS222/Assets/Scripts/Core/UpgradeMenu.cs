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
    [ChildGameObjectsOnly]public GameObject upgradeMenuUI;
    [ChildGameObjectsOnly]public GameObject upgradeMenu2UI;
    [ChildGameObjectsOnly]public GameObject lvltreeUI;
    [ChildGameObjectsOnly]public GameObject lvltreeUI1;
    [ChildGameObjectsOnly]public GameObject lvltreeUI2;
    [ChildGameObjectsOnly]public GameObject zoneMap;
    [ChildGameObjectsOnly]public GameObject invMenu;
    [ChildGameObjectsOnly]public GameObject statsMenu;
    [ChildGameObjectsOnly]public GameObject modulesSkillsPanel;
    [ChildGameObjectsOnly]public GameObject modulesSkillsInventory;
    [ChildGameObjectsOnly]public GameObject modulesList;
    [ChildGameObjectsOnly]public GameObject skillsList;
    [ChildGameObjectsOnly]public Toggle autoascendToggle;
    [ChildGameObjectsOnly]public Toggle autoLvlToggle;
    [ChildGameObjectsOnly]public GameObject backButton;
    [AssetsOnly]public GameObject moduleSkillElementPrefab;
    [AssetsOnly]public GameObject moduleSlotPrefab;
    [AssetsOnly]public GameObject skillSlotPrefab;

    [DisableInEditorMode][SerializeField]public int selectedModuleSlot=-1;
    [DisableInEditorMode][SerializeField]public int selectedSkillSlot=-1;
    PlayerModules pmodules;

    void Start(){
        instance=this;
        pmodules=Player.instance.GetComponent<PlayerModules>();
        //if(GameSession.instance.CheckGamemodeSelected("Adventure")){LvlEventsAdventure();}
        SetupModulesAndSkills();
        if(GameRules.instance.forceAutoAscend)Destroy(autoascendToggle.gameObject);

        Resume();
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.F)||Input.GetKeyDown(KeyCode.JoystickButton3)){
            if(UpgradeMenuIsOpen){Resume();}
            else{if(PauseMenu.GameIsPaused!=true&&Shop.shopOpened!=true&&Player.instance!=null)if(!Player.instance._hasStatus("hacked"))Open();}
        }
        if(Input.GetKeyDown(KeyCode.M)&&GameSession.instance.CheckGamemodeSelected("Adventure")&&FindObjectOfType<BossAI>()==null){if(!UpgradeMenuIsOpen){Open();OpenZoneMap();}}
        if(GSceneManager.EscPressed()||Input.GetKeyDown(KeyCode.Backspace)||Input.GetKeyDown(KeyCode.JoystickButton1)){Back();}
        //SetModulesAndSkillsPreviews();
    }

    public void Resume(){
        upgradeMenuUI.SetActive(false);
        upgradeMenu2UI.SetActive(false);
        lvltreeUI.SetActive(false);
        zoneMap.SetActive(false);
        BackToModulesSkillsInventory();
        GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
        UpgradeMenuIsOpen=false;
    }
    public void Open(){
        if(autoascendToggle!=null)autoascendToggle.isOn=pmodules.autoAscend;
        if(autoLvlToggle!=null)autoLvlToggle.isOn=pmodules.autoLvl;
        if(FindObjectOfType<CelestialPoints>()!=null)FindObjectOfType<CelestialPoints>().RefreshCelestialPoints();
        upgradeMenuUI.SetActive(true);
        /*if(GameRules.instance.upgradeMenuPause)*/GameSession.instance.gameSpeed=GameRules.instance.upgradeMenuOpenGameSpeed;
        //else{GameSession.instance.gameSpeed=GameRules.instance.upgradeMenuSlowdownSpeed;}
        UpgradeMenuIsOpen=true;
        StartCoroutine(ForceLayoutUpdate());
    }

    IEnumerator ForceLayoutUpdate(){
        yield return new WaitForSecondsRealtime(0.02f);
        upgradeMenuUI.transform.GetComponentInChildren<VerticalLayoutGroup>().GetComponent<Image>().enabled=false;
        yield return new WaitForSecondsRealtime(0.02f);
        upgradeMenuUI.transform.GetComponentInChildren<VerticalLayoutGroup>().GetComponent<Image>().enabled=true;//force update layout
    }

    
    public void OpenInv(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        invMenu.SetActive(true);
        statsMenu.SetActive(false);
        modulesSkillsPanel.SetActive(false);
    }
    public void OpenStats(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        invMenu.SetActive(false);
        statsMenu.SetActive(true);
        modulesSkillsPanel.SetActive(false);
    }
    public void OpenModules(){upgradeMenu2UI.SetActive(true);upgradeMenuUI.SetActive(false);
        invMenu.SetActive(false);
        statsMenu.SetActive(false);
        modulesSkillsPanel.SetActive(true);
        SetModulesAndSkillsPreviews();
    }
    public void OpenZoneMap(){
        if(FindObjectOfType<BossAI>()==null){
            upgradeMenuUI.SetActive(false);
            zoneMap.SetActive(true);
        }
        zoneMap.GetComponent<AdventureZonesCanvas>().Setup();
    }
    public void OpenZoneMapPostBoss(){StartCoroutine(OpenZoneMapPostBossI());}
    IEnumerator OpenZoneMapPostBossI(){yield return new WaitForSeconds(3f);Open();OpenZoneMap();}
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
        if(statsMenu.activeSelf||modulesSkillsPanel.activeSelf||lvltreeUI.activeSelf||zoneMap.activeSelf){
            statsMenu.SetActive(false);modulesSkillsPanel.SetActive(false);
            upgradeMenu2UI.SetActive(false);lvltreeUI.SetActive(false);zoneMap.SetActive(false);
            upgradeMenuUI.SetActive(true);
            return;
        }
        StartCoroutine(ForceLayoutUpdate());
        if(upgradeMenuUI.activeSelf){Resume();}
    }

    public void CloseAllModulesSkills(){
        modulesSkillsInventory.SetActive(false);
        modulesList.SetActive(false);
        skillsList.SetActive(false);
    }
    public void BackToModulesSkillsInventory(){CloseAllModulesSkills();modulesSkillsInventory.SetActive(true);selectedModuleSlot=-1;selectedSkillSlot=-1;SetModulesAndSkillsPreviews();SetModulesAndSkillsLvlVals();}
    public void OpenModulesList(int id){selectedModuleSlot=id;CloseAllModulesSkills();modulesList.SetActive(true);SetModulesAndSkillsLvlVals();}
    public void OpenSkillsList(int id){selectedSkillSlot=id;CloseAllModulesSkills();skillsList.SetActive(true);SetModulesAndSkillsLvlVals();}

    public void UnlockLvlModule(string name){
        if(pmodules.UnlockModule(name)){AudioManager.instance.Play("Upgrade");SetModulesAndSkillsLvlVals();StatsAchievsManager.instance.ModuleUnlocked();if(pmodules.moduleSlots[selectedModuleSlot]==""){SetModuleSlot(name);}}//pmodules.SetModule(selectedModuleSlot,name);}}
        else{AudioManager.instance.Play("Deny");}
    }
    public void SetModuleSlot(string name){
        if(pmodules._isModuleUnlocked(name)||name==""){
            if(pmodules._isModuleEquipped(name)&&name!=""){pmodules.ClearModule(name);}
            pmodules.SetModule(selectedModuleSlot,name);
            if(name=="Dark Surge"){SetAutoascend(false);}
            //BackToModulesSkillsInventory();
        }
    }

    public void UnlockLvlSkill(string name){
        if(pmodules.UnlockSkill(name)){AudioManager.instance.Play("Upgrade");SetModulesAndSkillsLvlVals();StatsAchievsManager.instance.ModuleUnlocked();if(pmodules.skillsSlots[selectedSkillSlot]==""){SetSkillSlot(name);}}//pmodules.SetSkill(selectedSkillSlot,name);}}
        else{AudioManager.instance.Play("Deny");}
    }
    public void SetSkillSlot(string name){
        if(pmodules._isSkillUnlocked(name)||name==""){
            if(pmodules._isSkillEquipped(name)&&name!=""){pmodules.ClearSkill(name);}
            pmodules.SetSkill(selectedSkillSlot,name);
            //BackToModulesSkillsInventory();
        }
    }

    bool _modulesSetup;
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
        goModule0.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(()=>SetModuleSlot(""));
        goModule0.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<XPFill>().valueName="moduleEmptyThisSlot";
        Destroy(goModule0.transform.GetChild(6).gameObject);
        Destroy(goModule0.transform.GetChild(5).GetChild(0).gameObject);
        Destroy(goModule0.transform.GetChild(4).gameObject);
        Destroy(goModule0.transform.GetChild(3).gameObject);

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
            go.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(()=>UnlockLvlModule(m.item.name));
            go.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<XPFill>().valueName="moduleUnlocked_"+m.item.name;
            go.transform.GetChild(3).GetChild(3).GetComponent<ValueDisplay>().value="moduleLvl_"+m.item.name;
            //go.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text=m.coreCost.ToString();
            //go.transform.GetChild(4).GetComponent<ShipLevelRequired>().value=m.lvlReq;
            go.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(()=>SetModuleSlot(m.item.name));
            go.transform.GetChild(5).GetChild(0).GetComponent<ValueDisplay>().value="moduleEquippedSlot_"+m.item.name;
            go.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<XPFill>().valueName="moduleEquippedThisSlot_"+m.item.name;
            go.transform.GetChild(6).GetComponent<ShipLevelRequired>().value=m.lvlExpire;
            if(m.lvlExpire==0){Destroy(go.transform.GetChild(6).gameObject);}
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
        goSkill0.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(()=>SetSkillSlot(""));
        goSkill0.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<XPFill>().valueName="skillEmptyThisSlot";
        Destroy(goSkill0.transform.GetChild(6).gameObject);
        Destroy(goSkill0.transform.GetChild(5).GetChild(0).gameObject);
        Destroy(goSkill0.transform.GetChild(4).gameObject);
        Destroy(goSkill0.transform.GetChild(3).gameObject);

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
            go.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(()=>UnlockLvlSkill(s.item.name));
            go.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<XPFill>().valueName="skillUnlocked_"+s.item.name;
            go.transform.GetChild(3).GetChild(3).GetComponent<ValueDisplay>().value="skillLvl_"+s.item.name;
            //go.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text=s.coreCost.ToString();
            //go.transform.GetChild(4).GetComponent<ShipLevelRequired>().value=s.lvlReq;
            go.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(()=>SetSkillSlot(s.item.name));
            go.transform.GetChild(5).GetChild(0).GetComponent<ValueDisplay>().value="skillEquippedSlot_"+s.item.name;
            go.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<XPFill>().valueName="skillEquippedThisSlot_"+s.item.name;
            go.transform.GetChild(6).GetComponent<ShipLevelRequired>().value=s.lvlExpire;
            if(s.lvlExpire==0){Destroy(go.transform.GetChild(6).gameObject);}
        }
        _modulesSetup=true;
        SetModulesAndSkillsLvlVals_del();
    }
    void SetModulesAndSkillsLvlVals_del(){StartCoroutine(SetModulesAndSkillsLvlVals_delI());}
    IEnumerator SetModulesAndSkillsLvlVals_delI(){yield return new WaitForSecondsRealtime(0.05f);SetModulesAndSkillsLvlVals();}
    void SetModulesAndSkillsLvlVals(){if(_modulesSetup){
        var modulesContainer=modulesList.transform.GetChild(0);
        foreach(Transform t in modulesContainer){if(t.gameObject.name!="Empty"){SetValues(t);}}
        var skillsContainer=skillsList.transform.GetChild(0);
        foreach(Transform t in skillsContainer){if(t.gameObject.name!="Empty"){SetValues(t,true);}}

        void SetValues(Transform t, bool skill=false){
            var lvlExpire=0;var lvlVals=new ModuleSkillLvlVals();
            if(!skill){lvlVals=pmodules.GetModuleNextLvlVals(t.gameObject.name);lvlExpire=pmodules.GetModuleProperties(t.gameObject.name).lvlExpire;}
            else{lvlVals=pmodules.GetSkillNextLvlVals(t.gameObject.name);lvlExpire=pmodules.GetSkillProperties(t.gameObject.name).lvlExpire;}
            if(lvlVals!=null){
                if(lvlVals.coreCost>=0){
                    t.GetChild(3).GetChild(0).GetComponent<Image>().enabled=true;
                    t.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text=lvlVals.coreCost.ToString();
                    t.GetChild(4).gameObject.GetComponent<ShipLevelRequired>().value=lvlVals.lvlReq;
                    t.GetChild(4).gameObject.GetComponent<ShipLevelRequired>().Switch(true);
                    /*if(t.childCount>6){
                        Debug.Log(lvlExpire);
                        if(lvlExpire>0){
                            t.GetChild(6).gameObject.GetComponent<ShipLevelRequired>().value=lvlExpire;
                            t.GetChild(6).gameObject.GetComponent<ShipLevelRequired>().Switch(false);
                        }else{Destroy(t.GetChild(6).gameObject);}
                    }*/
                }else{
                    t.GetChild(3).GetChild(0).GetComponent<Image>().enabled=false;
                    t.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text="";
                }
            }else{
                t.GetChild(3).GetChild(0).GetComponent<Image>().enabled=false;
                t.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text="";
                t.GetChild(4).gameObject.GetComponent<ShipLevelRequired>().Switch(false);
            }
        }
    }}
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

    public void UpgradeBody(){
        var _isAdventure=GameSession.instance.CheckGamemodeSelected("Adventure");
        if(_isAdventure&&pmodules.shipLvlFraction>=GameRules.instance.bodyUpgrade_price){pmodules.bodyUpgraded++;pmodules.shipLvlFraction-=GameRules.instance.bodyUpgrade_price;}
        else if(!_isAdventure&&pmodules.accumulatedCelestPoints>=GameRules.instance.bodyUpgrade_price){pmodules.bodyUpgraded++;pmodules.accumulatedCelestPoints-=GameRules.instance.bodyUpgrade_price;}
    }
    public void UpgradeEngine(){
        var _isAdventure=GameSession.instance.CheckGamemodeSelected("Adventure");
        if(_isAdventure&&pmodules.shipLvlFraction>=GameRules.instance.engineUpgrade_price){pmodules.engineUpgraded++;pmodules.shipLvlFraction-=GameRules.instance.engineUpgrade_price;}
        else if(!_isAdventure&&pmodules.accumulatedCelestPoints>=GameRules.instance.engineUpgrade_price){pmodules.engineUpgraded++;pmodules.accumulatedCelestPoints-=GameRules.instance.engineUpgrade_price;}
    }
    public void UpgradeBlasters(){
        var _isAdventure=GameSession.instance.CheckGamemodeSelected("Adventure");
        if(_isAdventure&&pmodules.shipLvlFraction>=GameRules.instance.blastersUpgrade_price){pmodules.blastersUpgraded++;pmodules.shipLvlFraction-=GameRules.instance.blastersUpgrade_price;}
        else if(!_isAdventure&&pmodules.accumulatedCelestPoints>=GameRules.instance.blastersUpgrade_price){pmodules.blastersUpgraded++;pmodules.accumulatedCelestPoints-=GameRules.instance.blastersUpgrade_price;}
    }

    public void SetAutoascend(bool isOn){pmodules.autoAscend=isOn;autoascendToggle.isOn=isOn;}
    public void SetAutoLvl(bool isOn){pmodules.autoLvl=isOn;autoLvlToggle.isOn=isOn;}
    public void LevelUp(){
        AudioManager.instance.Play("LvlUp2");
        FindObjectOfType<OnScreenButtons>().GetComponent<Animator>().SetTrigger("on");
        var lvlPopup=GameAssets.instance.FindNotifUIByType(notifUI_type.lvlUp);
        lvlPopup.GetComponent<ValueDisplay>().value="lvlPopup";
        FindObjectOfType<OnScreenButtons>().lvldUp=true;
    }
    IEnumerator _lvlEvCor;
    public void LvlEvents(){if(_lvlEvCor==null){_lvlEvCor=LvlEventsI();StartCoroutine(_lvlEvCor);}}
    IEnumerator LvlEventsI(){
        yield return new WaitForSecondsRealtime(0.2f);
        if(GameRules.instance!=null){
        foreach(ListEvents le in GameRules.instance.lvlEvents){
            if(le.lvls.x==0&&le.lvls.y==0){le.events.Invoke();}
            else{if(pmodules.shipLvl>=le.lvls.x&&pmodules.shipLvl<=le.lvls.y&&!le.skipRe){le.events.Invoke();}}
        }}
        _lvlEvCor=null;
    }
    public void LvlEventsAdventure(){if(_lvlEvCor==null){_lvlEvCor=LvlEventsAdventureI();StartCoroutine(_lvlEvCor);}}
    IEnumerator LvlEventsAdventureI(){
        yield return new WaitForSecondsRealtime(0.2f);
        if(GameRules.instance!=null){
        foreach(ListEvents le in GameRules.instance.lvlEvents){
            if(le.lvls.x==0&&le.lvls.y==0){for(var i=0;i<pmodules.shipLvl;i++)le.events.Invoke();}
            else{if(pmodules.shipLvl>=le.lvls.x&&!le.skipRe){le.events.Invoke();}}
        }}
        GameSession.instance._lvlEventsLoading=false;
        Debug.Log("LvlEvents Loaded");
        _lvlEvCor=null;
    }
    public void CheatCores(){gitignoreScript.instance.CheatCores();}
    public void CheatLevels(){gitignoreScript.instance.CheatLevels();}
    public void CheatXP(){GameSession.instance.xp=GameSession.instance.xpMax;}
}