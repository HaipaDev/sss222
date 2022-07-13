using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerModules : MonoBehaviour{
    [Header("Settings")]
    public bool exhaustROF=true;
    public float timeTeleport=3f;
    public float timeOverhaul=10f;
    [Header("Level Values")]
    [SerializeField] public int shipLvl=0;
    [SerializeField] public int shipLvlFraction;
    [SerializeField] public List<ShipLvlFractionsValues> shipLvlFractionsValues;
    [SerializeField] public int lvlFractionsMax=1;
    [Header("Modules & Skills Slots & List")]
    [SerializeField] public List<string> moduleSlots;
    [SerializeField] public List<string> skillsSlots;
    [SerializeField] public List<Module> modulesList;
    [SerializeField] public List<Skill> skillsList;
    [Header("Timers etc")]
    [DisableInEditorMode]public string currentSkill="";
    [DisableInEditorMode]public float timerTeleport=-4;
    [DisableInEditorMode]public float timerOverhaul=-4;

    GameObject timerUI;
    void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.15f);
        var i=GameRules.instance;
        if(i!=null){
            //shipLvlFractionsValues=i.shipLvlFractionsValues;
            if(i.shipLvlFractionsValues.Find(x=>x.lvl==0)==null){shipLvlFractionsValues.Add(new ShipLvlFractionsValues{lvl=0,fractions=1});}
            foreach(ShipLvlFractionsValues s in i.shipLvlFractionsValues){shipLvlFractionsValues.Add(s);}
            if(moduleSlots.Capacity==0)for(var m=0;m<i.playerModulesCapacity;m++){moduleSlots.Add("");}
            if(skillsSlots.Capacity==0)for(var m=0;m<2;m++){skillsSlots.Add("");}
            //for(var m=0;m<i.playerSkillsCapacity;m++){skillsSlots.Add("");}
            if(modulesList.Capacity==0)foreach(ModulePropertiesGR m in i.modulesPlayer){modulesList.Add(new Module{name=m.item.name});}
            if(skillsList.Capacity==0)foreach(SkillPropertiesGR s in i.skillsPlayer){skillsList.Add(new Skill{name=s.item.name});}
            timeOverhaul=i.timeOverhaul;
            exhaustROF=i.playerExhaustROF;
        }
        if(!exhaustROF){GetComponent<PlayerExhaust>().DestroyExhaust();}
        if(GameRules.instance.modulesOn!=true){Destroy(this);}
    }
    void Start(){
        if(shipLvlFractionsValues.Capacity>0)lvlFractionsMax=shipLvlFractionsValues[0].fractions;
        timerUI=GameObject.Find("SkillTimer_par");
    }

    void Update(){
        ShipLevel();
        CheckSkillButton();
        SkillsUpdate();
        ModulesUpdate();
    }

    void ShipLevel(){
        if(GameRules.instance.levelingOn){
            if(shipLvlFraction>=lvlFractionsMax){shipLvl++;shipLvlFraction=0;UpgradeMenu.instance.LevelUp();UpgradeMenu.instance.LvlEvents();return;}
            for(var i=0;i<shipLvlFractionsValues.Capacity;i++){
                if(i<shipLvlFractionsValues.Capacity-1){
                    if(shipLvl>=shipLvlFractionsValues[i].lvl&&shipLvl<shipLvlFractionsValues[i+1].lvl){
                        //Debug.Log("Lvl: "+shipLvl+" | LvlThis: "+shipLvlFractionsValues[i].lvl+" | LvlAbove: "+shipLvlFractionsValues[i+1].lvl+" | FractionsMax: "+shipLvlFractionsValues[i].fractions);
                        lvlFractionsMax=shipLvlFractionsValues[i].fractions;return;
                    }
                }else{lvlFractionsMax=shipLvlFractionsValues[i].fractions;return;}
            }
            //for(var i=0;i<shipLvlFractionsValues.Capacity-1;i++){if(shipLvl<shipLvlFractionsValues[i].lvl){lvlFractionsMax=shipLvlFractionsValues[i++].fractions;return;}}
            //shipLvlFraction=Mathf.Clamp(shipLvlFraction-lvlFractionsMax,0,99);
        }
    }
    
    #region//Skills & Modules
    public void CheckSkillButton(int key=0){     if(!GameSession.GlobalTimeIsPaused){
        for(var i=0;i<skillsList.Capacity-2;i++){
            if(skillsList[i].cooldown>0)skillsList[i].cooldown-=Time.deltaTime;
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
        if(_isSkillLvl("Magnetic Pulse",2))UseSkill(GetSkillProperties("Magnetic Pulse"),true);//PostMortem MagneticPulse
    }
    public void UseSkill(SkillPropertiesGR item,bool ignoreCosts=false){     if(item!=null){
        //var _item=GetSkillProperties(item.name);
        if(item.item.name!="Overhaul"){
            if(Player.instance.energy>0||ignoreCosts){
                Player.instance.AddSubEnergy(item.costTypeProperties.cost,false);
                if(item.item.name=="Magnetic Pulse"){
                    GameObject mPulse=GameAssets.instance.Make("MPulse",transform.position);
                    GetSkill(item.item.name).cooldown=item.cooldown;
                }if(item.item.name=="Teleport"){//Teleport
                    GameSession.instance.gameSpeed=0.025f;
                    GameSession.instance.speedChanged=true;
                    if(timerUI!=null)SetActiveAllChildren(timerUI.transform,true);
                    currentSkill="Teleport";
                GetSkill(item.item.name).cooldown=item.cooldown;
                }
            }else{AudioManager.instance.Play("Deny");}
        }else if(item.item.name=="Overhaul"){//Overhaul
            if(GameSession.instance.xp>0||ignoreCosts){
                if(Player.instance.energy<1){Player.instance.AddSubEnergy(20);}
                var ratio=(GameSession.instance.xp/GameSession.instance.xpMax);
                GameCanvas.instance.XPPopUpHUD(-GameSession.instance.xp);
                Player.instance.InfEnergy(ratio*33,0);
                Player.instance.Power(16,Mathf.Clamp(3f*ratio,1.1f,2.2f));
                timerOverhaul=timeOverhaul;
                GameSession.instance.xp=0;
                AudioManager.instance.Play("Overhaul");
                AudioManager.instance.GetSource("Overhaul").loop=true;
                GetSkill(item.item.name).cooldown=item.cooldown;
            }else{AudioManager.instance.Play("Deny");}
        }
    }}

    void SkillsUpdate(){
    if(!GameSession.GlobalTimeIsPaused){
        if(currentSkill!="Teleport"){
            if(timerUI!=null)SetActiveAllChildren(timerUI.transform,false);
        }else if(currentSkill=="Teleport"){
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
                    if(timerUI!=null)SetActiveAllChildren(timerUI.transform,false);
                    GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;timerTeleport=-4;currentSkill="";}
            }else if(timerTeleport<=0&&timerTeleport!=-4){
                if(timerUI!=null)SetActiveAllChildren(timerUI.transform,false);
                GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;timerTeleport=-4;currentSkill="";
            }
        }
        if(timerOverhaul>0&&Player.instance._hasStatus("infenergy")){
            timerOverhaul-=Time.deltaTime;
        }if((timerOverhaul<0&&timerOverhaul!=-4)&&Player.instance._hasStatus("infenergy")){timerOverhaul=timeOverhaul;}
        if(!Player.instance._hasStatus("infenergy")&&AudioManager.instance.GetSource("Overhaul").isPlaying){AudioManager.instance.StopPlaying("Overhaul");}
    }}

    void ModulesUpdate(){
        if(_isModuleEquipped("Crystal Mending")&&Player.instance.hpAbsorpAmnt<=0){if(GameSession.instance.coins>=GameRules.instance.crystalMend_refillCost){Player.instance.HPAbsorp(Player.instance.crystalMendAbsorp);GameSession.instance.coins-=GameRules.instance.crystalMend_refillCost;}}
        if(_isModuleEquipped("Energy Dissolution")&&Player.instance.enAbsorpAmnt<=0){if(GameSession.instance.xp>=GameRules.instance.energyDiss_refillCost){Player.instance.EnAbsorp(Player.instance.energyDissAbsorp);GameSession.instance.xp-=GameRules.instance.energyDiss_refillCost;}}
    }
    #endregion
    public bool _canUnlockModuleSkill(string name){return GameSession.instance.cores>=GetModuleNextLvlVals(name).coreCost&&(shipLvl>=GetModuleNextLvlVals(name).lvlReq||!GameRules.instance.levelingOn);}

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
    public bool _isModuleMaxed(string name){return GetModuleNextLvlVals(name)==null;}
    public Module GetModule(string name){Module _target=null;_target=modulesList.Find(x=>x.name==name);if(_target!=null){return _target;}else{return null;}}
    public Module GetModuleFromID(int id){Module _target=null;_target=modulesList.Find(x=>x.name==moduleSlots[id]);if(_target!=null){return _target;}else{return null;}}
    public ModulePropertiesGR GetModuleProperties(string name){var _target=GameRules.instance.modulesPlayer.Find(x=>x.item.name==name);if(_target!=null){return _target;}else{Debug.LogWarning("No ModuleProperties in GameRules by name: "+name);return null;}}
    public ModuleSkillLvlVals GetModuleNextLvlVals(string name){
        var mp=GetModuleProperties(name);
        if(mp!=null){
            var m=GetModule(name);
            if(m!=null){if(m.lvl<mp.lvlVals.Capacity){return mp.lvlVals[m.lvl];}}
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

    public void ResetSkillCooldowns(){for(var i=0;i<skillsSlots.Capacity;i++){skillsList[i].cooldown=0;}}

    private void SetActiveAllChildren(Transform transform, bool value){
        foreach (Transform child in transform){
            child.gameObject.SetActive(value);
            SetActiveAllChildren(child, value);
        }
    }
}


[System.Serializable]public class ShipLvlFractionsValues{
    public int lvl=1;
    [Range(0,10)]public int fractions=1;
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