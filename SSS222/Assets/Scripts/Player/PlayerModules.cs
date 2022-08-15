using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerModules : MonoBehaviour{
[Header("Settings")]
    public float timeTeleport=3f;
    public float timeDetemined=4f;
    public float timeGiveItToMe=8f;
    public GameObject exhaustColliderObj;
[Header("Level Values")]
    [SerializeField] public int shipLvl=0;
    [SerializeField] public int shipLvlFraction;
    [SerializeField] public List<ShipLvlFractionsValues> shipLvlFractionsValues;
    [DisableInEditorMode][SerializeField] public bool autoAscend=true;
    [DisableInEditorMode][SerializeField] public bool autoLvl=true;
    [DisableInEditorMode][SerializeField] public int lvlFractionsMax=0;
[Header("Modules & Skills Slots & List")]
    [SerializeField] public List<string> moduleSlots;
    [SerializeField] public List<string> skillsSlots;
    [SerializeField] public List<Module> modulesList;
    [SerializeField] public List<Skill> skillsList;
[Header("Stats")]
    public int accumulatedCelestPoints=0;
    public int bodyUpgraded=0;
    public int engineUpgraded=0;
    public int blastersUpgraded=0;
[Header("Timers etc")]
    [DisableInEditorMode]public string currentSkill="";
    [DisableInEditorMode]public float timerTeleport=-4;
    [DisableInEditorMode]public float timerDetemined=-4;
    [DisableInEditorMode]public float timerGiveItToMe=-4;

    Player player;
    void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.15f);
        var i=GameRules.instance;
        if(i!=null){
            //shipLvlFractionsValues=i.shipLvlFractionsValues;
            if(i.shipLvlFractionsValues.Find(x=>x.lvl==0)==null){shipLvlFractionsValues.Add(new ShipLvlFractionsValues{lvl=0,fractions=1});}
            foreach(ShipLvlFractionsValues s in i.shipLvlFractionsValues){shipLvlFractionsValues.Add(s);}
            if(moduleSlots.Count==0){for(var m=0;m<i.playerModulesCapacity;m++){moduleSlots.Add("");}}
            if(skillsSlots.Count==0){for(var m=0;m<2;m++){skillsSlots.Add("");}}
            //for(var m=0;m<i.playerSkillsCapacity;m++){skillsSlots.Add("");}
            if(modulesList.Count==0){var _id=0;foreach(ModulePropertiesGR m in i.modulesPlayer){if(m.equipped&&_id<moduleSlots.Count){SetModule(_id,m.item.name);}modulesList.Add(new Module{name=m.item.name,lvl=m.unlockedLvl});}_id++;}
            if(skillsList.Count==0){var _id=0;foreach(SkillPropertiesGR s in i.skillsPlayer){if(s.equipped&&_id<skillsSlots.Count){SetSkill(_id,s.item.name);}skillsList.Add(new Skill{name=s.item.name,lvl=s.unlockedLvl});}_id++;}
            timeTeleport=i.timeTeleport;
            timeDetemined=i.timeDetemined;
            timeGiveItToMe=i.timeGiveItToMe;
        }
        if(GameRules.instance.modulesOn!=true){Destroy(this);}
    }
    IEnumerator Start(){
        player=GetComponent<Player>();
        yield return new WaitForSeconds(0.2f);
        if(shipLvlFractionsValues.Count>0&&shipLvl==0)lvlFractionsMax=shipLvlFractionsValues[0].fractions;
        if(!GameSession.instance.CheckGamemodeSelected("Adventure")){autoLvl=true;}
    }

    void Update(){
        ShipLevel();
        CheckSkillButton();
        SkillsUpdate();
        ModulesUpdate();
        CheckExpired();
        CalculateStats();
    }

    void ShipLevel(){
        if(GameRules.instance.levelingOn){
            if(_isLvlUpable()&&_isAutoLvl()){LevelUp();return;}
            for(var i=0;i<shipLvlFractionsValues.Count;i++){
                if(i==shipLvlFractionsValues.Count-1){lvlFractionsMax=shipLvlFractionsValues[i].fractions;return;}
                else{
                    if(shipLvl>=shipLvlFractionsValues[i].lvl&&shipLvl<shipLvlFractionsValues[i+1].lvl){
                        //Debug.Log("Lvl: "+shipLvl+" | LvlThis: "+shipLvlFractionsValues[i].lvl+" | LvlAbove: "+shipLvlFractionsValues[i+1].lvl+" | FractionsMax: "+shipLvlFractionsValues[i].fractions);
                        lvlFractionsMax=shipLvlFractionsValues[i].fractions;return;
                    }
                }
            }
        }
    }

    public void Ascend(){
        if(!_isLvlUpable()){
            shipLvlFraction++;
            if(!GameSession.instance.CheckGamemodeSelected("Adventure")){if(shipLvl>=GameRules.instance.accumulateCelestPointsFromLvl)accumulatedCelestPoints++;}
            GameSession.instance.Ascend();
        }
    }
    public void LevelUp(){
        shipLvl++;shipLvlFraction=0;
        UpgradeMenu.instance.LevelUp();UpgradeMenu.instance.LvlEvents();
        if(FindObjectOfType<CelestialPoints>()!=null){FindObjectOfType<CelestialPoints>().RefreshCelestialPoints();}
    }
    public bool _isLvlUpable(){return shipLvlFraction>=lvlFractionsMax&&lvlFractionsMax!=0;}
    
    #region//Skills & Modules
    public void CheckSkillButton(int key=0){     if(!GameSession.GlobalTimeIsPaused){
        for(var i=0;i<skillsList.Count;i++){
            if(skillsList[i].cooldown>0){skillsList[i].cooldown-=Time.deltaTime;}
            if(skillsList[i].name=="Determined"&&skillsList[i].cooldown<0.1f&&player.health>25){skillsList[i].cooldown=0.01f;}
        }
        if(!Player.instance._hasStatus("hacked")){
            var _slot=0;
            bool _key1=(Input.GetKeyDown(KeyCode.Q)||Input.GetKeyDown(KeyCode.JoystickButton2)||key==1);
            bool _key2=(Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.JoystickButton1)||key==2);    if(_key2){_slot=1;}
            if(_key1||_key2){
                if(GetSkill(skillsSlots[_slot])!=null){
                    if(GetSkill(skillsSlots[_slot]).cooldown<=0){UseSkill(GetSkillProperties(skillsSlots[_slot]));}
                    else{AudioManager.instance.Play("Deny");}
                }
            }
        }
    }}
    public void DeathSkills(){
        if(_isSkillLvl("MPulse",2))UseSkill(GetSkillProperties("MPulse"),true);//PostMortem MagneticPulse
    }
    public void UseSkill(SkillPropertiesGR item,bool ignoreCosts=false){     if(item!=null){
        //var _item=GetSkillProperties(item.name);
        if(Player.instance.energy>0||ignoreCosts){
            Player.instance.AddSubEnergy(item.costTypeProperties.cost,false);
            if(item.item.name=="MPulse"){
                GameObject mPulse=GameAssets.instance.Make("MPulse",transform.position);
                GetSkill(item.item.name).cooldown=item.cooldown;
            }else if(item.item.name=="Teleport"){
                GameSession.instance.gameSpeed=0.025f;
                GameSession.instance.speedChanged=true;
                currentSkill="Teleport";
                GetSkill(item.item.name).cooldown=item.cooldown;
            }else if(item.item.name=="LShield"){bool _canMakeShield=false;
                var fragments=4;var cost=fragments*2;
                if(_isSkillLvl("LShield",2)){fragments=8;cost=fragments*2;}
                if(FindObjectOfType<LunarShield>()!=null){
                    var l=FindObjectOfType<LunarShield>();
                    if(l._notDamagedShieldPiecesCount()!=0&&GameSession.instance.coins>=cost){_canMakeShield=true;}
                    cost-=l._notDamagedShieldPiecesCount();
                    if(_canMakeShield)Destroy(l.gameObject);
                }else{_canMakeShield=true;}
                if(_canMakeShield){
                    var l=GameAssets.instance.Make("LunarShield",transform.position);l.transform.parent=transform;
                    l.GetComponent<LunarShield>().fragmentsPresent=fragments;
                    player.AddSubCoins(cost);
                    GetSkill(item.item.name).cooldown=item.cooldown;
                }
            }else if(item.item.name=="Determined"&&player.health<=25){
                timerDetemined=timeDetemined;
                GetSkill(item.item.name).cooldown=item.cooldown;
            }else if(item.item.name=="GiveItToMe"){
                timerGiveItToMe=timeGiveItToMe;
                GetSkill(item.item.name).cooldown=item.cooldown;
            }
        }else{AudioManager.instance.Play("Deny");}
    }}

    void SkillsUpdate(){
    if(!GameSession.GlobalTimeIsPaused){
        if(currentSkill=="Teleport"){
            if(timerTeleport==-4)timerTeleport=timeTeleport;
            if(timerTeleport>0){
                timerTeleport-=Time.unscaledDeltaTime;
                if(Input.GetMouseButtonDown(0)){
                    AudioManager.instance.Play("Portal");
                    GameObject tp1=GameAssets.instance.VFX("PortalVFX",transform.position,1.25f);
                    GameObject tp2=GameAssets.instance.VFX("PortalVFX",Player.instance.mousePos,1.25f);
                    var ps1=tp1.GetComponent<ParticleSystem>();var main1=ps1.main;
                    main1.startColor=Color.blue;
                    var ps2=tp2.GetComponent<ParticleSystem>();var main2=ps2.main;
                    main2.startColor=new Color(255,140,0,255);//Orange
                    transform.position=Player.instance.mousePos;
                    GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;timerTeleport=-4;currentSkill="";
                }
            }else if(timerTeleport<=0&&timerTeleport!=-4){
                GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;timerTeleport=-4;currentSkill="";
            }
        }
        //Determined
        if(timerDetemined>0){timerDetemined-=Time.deltaTime;}
        else if(timerDetemined<=0&&timerDetemined!=-4){timerDetemined=-4;}
        //GiveItToMe
        if(timerGiveItToMe>0){timerGiveItToMe-=Time.deltaTime;}
        else if(timerGiveItToMe<=0&&timerGiveItToMe!=-4){timerGiveItToMe=-4;}
    }}

    void ModulesUpdate(){
        SwitchExhaust(_isModuleEquipped("ROF"));
        if(_isModuleEquipped("CrMend")&&Player.instance.hpAbsorpAmnt<=0){
            if(GameSession.instance.coins>=GameRules.instance.crystalMend_refillCost){Player.instance.HPAbsorp(Player.instance.crystalMendAbsorp);GameSession.instance.coins-=GameRules.instance.crystalMend_refillCost;}
        }
        if(_isModuleEquipped("EnDiss")&&Player.instance.enAbsorpAmnt<=0){
            if(GameSession.instance.xp>=GameRules.instance.energyDiss_refillCost){Player.instance.EnAbsorp(Player.instance.energyDissAbsorp);GameSession.instance.xp-=GameRules.instance.energyDiss_refillCost;}
        }
        if(_isModuleEquipped("DkSurge")){
            if(GameSession.instance.xp>=GameSession.instance.xpMax){
                var dif=(GameSession.instance.xpMax*GameRules.instance.xpMaxOvefillMult)-GameSession.instance.xp;
                if(dif<=25&&!Player.instance._hasStatus("speed")){Player.instance.Speed(5,5,1.2f);if(Player.instance._hasStatus("slow")){Player.instance.RemoveStatus("slow");}}
                if(dif<=15&&!Player.instance._hasStatus("armored")){Player.instance.Armor(5,5,2);if(Player.instance._hasStatus("fragile")){Player.instance.RemoveStatus("fragile");}}
                if(dif==0&&!Player.instance._hasStatus("power")){Player.instance.Power(5,5,1.15f);if(Player.instance._hasStatus("weakns")){Player.instance.RemoveStatus("weakns");}}
            }
        }
        if(_isModuleEquipped("CodeBreaker")){
            if(player._hasStatus("hacked")){player.RemoveStatus("hacked");}
        }
        if(_isModuleEquipped("FOGas")){
            if(player._hasStatus("fuel")){player.RemoveStatus("fuel");}
        }else{if(player._statusesPersistent.Exists(x=>x.name=="fuel")){player.SetStatus("fuel",-5);}}
    }
    void CheckExpired(){
        foreach(ModulePropertiesGR m in GameRules.instance.modulesPlayer){if(shipLvl>=m.lvlExpire&&m.lvlExpire!=0)if(_isModuleEquipped(m.item.name))ClearModule(m.item.name);}
        foreach(SkillPropertiesGR s in GameRules.instance.skillsPlayer){if(shipLvl>=s.lvlExpire&&s.lvlExpire!=0)if(_isSkillEquipped(s.item.name))ClearSkill(s.item.name);}
    }
    #endregion
    public bool _canUnlockModuleSkill(string name){return GameSession.instance.cores>=GetModuleNextLvlVals(name).coreCost&&GetModuleNextLvlVals(name).coreCost>=0&&(shipLvl>=GetModuleNextLvlVals(name).lvlReq||!GameRules.instance.levelingOn);}

    public bool _isModuleUnlocked(string name){return modulesList.Find(x=>x.name==name&&x.lvl>=1)!=null;}
    public bool UnlockModule(string name){if(GetModuleProperties(name)!=null){
        if(GetModuleNextLvlVals(name)!=null){
            if(_canUnlockModuleSkill(name)){
                GameSession.instance.cores-=GetModuleNextLvlVals(name).coreCost;
                if(!_isModuleUnlocked(name)){GetModule(name).lvl=1;}
                else{GetModule(name).lvl++;}
                return true;
            }
        }
    }return false;}
    public bool _isModuleEquipped(string name){return moduleSlots.Contains(name);}
    public bool _isModuleLvl(string name,int lvl){return modulesList.Find(x=>x.name==name).lvl>=lvl;}
    public bool _isModuleBelowLvl(string name,int lvl){return modulesList.Find(x=>x.name==name).lvl<lvl;}
    public bool _isModuleMaxed(string name){return GetModuleNextLvlVals(name)==null;}
    public Module GetModule(string name){Module _target=null;_target=modulesList.Find(x=>x.name==name);if(_target!=null){return _target;}else{return null;}}
    public Module GetModuleFromID(int id){Module _target=null;_target=modulesList.Find(x=>x.name==moduleSlots[id]);if(_target!=null){return _target;}else{return null;}}
    public ModulePropertiesGR GetModuleProperties(string name){var _target=GameRules.instance.modulesPlayer.Find(x=>x.item.name==name);if(_target!=null){return _target;}else{Debug.LogWarning("No ModuleProperties in GameRules by name: "+name);return null;}}
    public ModuleSkillLvlVals GetModuleNextLvlVals(string name){
        var mp=GetModuleProperties(name);
        if(mp!=null){
            var m=GetModule(name);
            if(m!=null){if(m.lvl<mp.lvlVals.Count){return mp.lvlVals[m.lvl];}}
        }return null;
    }
    public void SetModule(int id, string item){moduleSlots[id]=item;}
    public void ClearModule(string name){SetModule(moduleSlots.FindIndex(x=>x==name),"");}
    public void ReplaceModule(string name, string item){SetModule(moduleSlots.FindIndex(x=>x==name),item);}

    public bool _isSkillUnlocked(string name){return skillsList.Find(x=>x.name==name&&x.lvl>=1)!=null;}
    public bool UnlockSkill(string name){if(GetSkillProperties(name)!=null){
        if(GetSkillNextLvlVals(name)!=null){
            if(GameSession.instance.cores>=GetSkillNextLvlVals(name).coreCost&&shipLvl>=GetSkillNextLvlVals(name).lvlReq){
                GameSession.instance.cores-=GetSkillNextLvlVals(name).coreCost;
                if(!_isSkillUnlocked(name)){GetSkill(name).lvl=1;}
                else{GetSkill(name).lvl++;}
                return true;
            }
        }
    }return false;}
    public bool _isSkillEquipped(string name){return skillsSlots.Contains(name);}
    public bool _isSkillLvl(string name,int lvl){return skillsList.Find(x=>x.name==name).lvl>=lvl;}
    public bool _isSkillBelowLvl(string name,int lvl){return skillsList.Find(x=>x.name==name).lvl<lvl;}
    public bool _isSkillMaxed(string name){return GetSkillNextLvlVals(name)==null;}
    public Skill GetSkill(string name){Skill _target=null;_target=skillsList.Find(x=>x.name==name);if(_target!=null){return _target;}else{return null;}}
    public Skill GetSkillFromID(int id){Skill _target=null;_target=skillsList.Find(x=>x.name==skillsSlots[id]);if(_target!=null){return _target;}else{return null;}}
    public SkillPropertiesGR GetSkillProperties(string name){var _target=GameRules.instance.skillsPlayer.Find(x=>x.item.name==name);if(_target!=null){return _target;}else{Debug.LogWarning("No SkillProperties in GameRules by name: "+name);return null;}}
    public ModuleSkillLvlVals GetSkillNextLvlVals(string name){
        var sp=GetSkillProperties(name);
        if(sp!=null){
            var s=GetSkill(name);
            if(s!=null){if(s.lvl<sp.lvlVals.Capacity){return sp.lvlVals[s.lvl];}}
        }return null;
    }
    public void SetSkill(int id, string item){skillsSlots[id]=item;}
    public void ReplaceSkill(string name, string item){SetSkill(skillsSlots.FindIndex(x=>x==name),item);}
    public void ClearSkill(string name){ReplaceSkill(name,"");}
    public bool _isAutoAscend(){return autoAscend||GameRules.instance.forceAutoAscend;}
    public bool _isAutoLvl(){return autoLvl;}//||GameRules.instance.forceAutoAscend;}
    public void ResetSkillCooldowns(){for(var i=0;i<skillsSlots.Count;i++){skillsList[i].cooldown=0;}}

    void CalculateStats(){
        player.defenseBase=player.defenseInit+(bodyUpgraded*GameRules.instance.bodyUpgrade_defense);
        player.powerupsCapacity=Mathf.Clamp(player.powerupsCapacityInit+(bodyUpgraded*GameRules.instance.bodyUpgrade_powerupCapacity),0,10);
        
        if(GameRules.instance._isAdventure()){
            player.moveSpeedBase=player.moveSpeedInit+(engineUpgraded*GameRules.instance.engineUpgrade_moveSpeed);
            if(player.enAbsorpAmnt==0&&engineUpgraded>=1)player.enAbsorpAmnt=(1*GameRules.instance.engineUpgrade_energyRegen);
            if(engineUpgraded>1)player.freqEnRegen=1f-((engineUpgraded-1)*GameRules.instance.engineUpgrade_energyRegenFreqMinus);
            //else{player.enAbsorpAmnt=1;}
            //player.energyMax=GameRules.instance.energyMaxPlayer+(engineUpgraded*GameRules.instance.engineUpgrade_energyMax);
        }else{
            player.moveSpeedBase=player.moveSpeedInit+(engineUpgraded*GameRules.instance.engineUpgrade_moveSpeed);
            if(engineUpgraded>1)player.freqEnRegen=1f-((engineUpgraded-1)*GameRules.instance.engineUpgrade_energyRegenFreqMinus);
        }
        
        float _determinedAddShootMulti=0;float _determinedAddCritChance=0;
        if(timerDetemined>0){_determinedAddShootMulti=1f;_determinedAddCritChance=5f;}
        player.shootMultiBase=player.shootMultiInit+(blastersUpgraded*GameRules.instance.blastersUpgrade_shootMulti)+_determinedAddShootMulti;
        player.critChanceBase=player.critChanceInit+(blastersUpgraded*GameRules.instance.blastersUpgrade_critChance)+_determinedAddCritChance;
    }

    public void SwitchExhaust(bool on=false){exhaustColliderObj.SetActive(on);}
    void SetActiveAllChildren(Transform transform, bool value){
        foreach (Transform child in transform){
            child.gameObject.SetActive(value);
            SetActiveAllChildren(child, value);
        }
    }
}


[System.Serializable]public class ShipLvlFractionsValues{
    public int lvl=1;
    [Range(1,10)]public int fractions=1;
}
[System.Serializable]public class Skill{
    public string name;
    [DisableInEditorMode]public int lvl;
    [DisableInEditorMode]public float cooldown;
}
[System.Serializable]public class Module{
    public string name;
    [DisableInEditorMode]public int lvl;
}