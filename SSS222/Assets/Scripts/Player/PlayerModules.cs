using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]public class Skill{
    public string name;
    [DisableInEditorMode]public float cooldown;
}
public class PlayerModules : MonoBehaviour{
    [Header("Settings")]
    public bool exhaustROF=true;
    public float timeTeleport=3f;
    public float timeOverhaul=10f;
    [Header("Slots")]
    [SerializeField] public List<string> moduleSlots;
    [SerializeField] public List<string> skillsSlots=new List<string>(2);
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
            //moduleSlots=new List<string>(i.playerModulesCapacity);
            for(var m=0;m<i.playerModulesCapacity;m++){moduleSlots.Add("");}
            for(var m=0;m<2;m++){skillsSlots.Add("");}
            //skillsSlots=new Skill[i.playerSkillsCapacity];
            //for(var m=0;m<i.playerSkillsCapacity;m++){skillsSlots.Add("");}
            //for(var s=0;s<skillsSlots.Capacity;s++){skillsSlots[s]=new Skill();}
            foreach(SkillPropertiesGR sk in i.skillsPlayer){skillsList.Add(new Skill{name=sk.item.name});}
            timeOverhaul=i.timeOverhaul;
            exhaustROF=i.playerExhaustROF;
        }
        if(!exhaustROF){GetComponent<PlayerExhaust>().DestroyExhaust();}
        if(GameRules.instance.modulesOn!=true){Destroy(this);}
    }
    void Start(){
        timerUI=GameObject.Find("SkillTimer_par");
    }

    void Update(){
        UseSkills();
        SkillsUpdate();
    }

    public void UseSkills(int key=0){     if(!GameSession.GlobalTimeIsPaused){
        for(var i=0;i<skillsList.Capacity-2;i++){
            if(skillsList[i].cooldown>0)skillsList[i].cooldown-=Time.deltaTime;
        }
        if(!Player.instance._hasStatus("hacked")){
            var _slot=0;
            bool _key1=(Input.GetKeyDown(KeyCode.Q)||Input.GetKeyDown(KeyCode.JoystickButton2)||key==1);
            bool _key2=(Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.JoystickButton1)||key==2);    if(_key2){_slot=1;}
            if(_key1||_key2){
                if(GetSkill(skillsSlots[_slot])!=null){
                    if(GetSkill(skillsSlots[_slot]).cooldown<=0){Skills(GetSkillProperties(skillsSlots[_slot]));}
                    else{AudioManager.instance.Play("Deny");}
                }
            }
        }
    }}
    public void DeathSkills(){
        if(UpgradeMenu.instance.mPulse_upgraded==2)Skills(GetSkillProperties("Magnetic Pulse"));//PostMortem MagneticPulse
    }
    
    #region//Skills
    public void Skills(SkillPropertiesGR item){     if(item!=null){
        //var _item=GetSkillProperties(item.name);
        if(item.item.name!="Overhaul"){
            if(Player.instance.energy>0){
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
            if(GameSession.instance.xp>0){
                if(Player.instance.energy<1){Player.instance.AddSubEnergy(20);}
                var ratio=(GameSession.instance.xp/GameSession.instance.xpMax);
                GameCanvas.instance.XPPopUpHUD(-GameSession.instance.xp);
                Player.instance.InfEnergy(ratio*33);
                Player.instance.Power(16,Mathf.Clamp(3f*ratio,1.1f,2.2f));
                timerOverhaul=timeOverhaul;
                GameSession.instance.xp=0;
                AudioManager.instance.Play("Overhaul");
                AudioManager.instance.GetSource("Overhaul").loop=true;
                GetSkill(item.item.name).cooldown=item.cooldown;
            }else{AudioManager.instance.Play("Deny");}
        }
    }}

    public void SkillsUpdate(){
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
    #endregion

    public bool _isModuleEquipped(string name){return moduleSlots.Contains(name);}
    public void SetModule(int id, string item){moduleSlots[id]=item;}
    public void ClearModule(string name){SetModule(moduleSlots.FindIndex(x=>x==name),"");}
    public void ReplaceModule(string name, string item){SetModule(moduleSlots.FindIndex(x=>x==name),item);}
    public ModulePropertiesGR GetModuleProperties(string name){var _target=GameRules.instance.modulesPlayer.Find(x=>x.item.name==name);if(_target!=null){return _target;}else{Debug.LogWarning("No ModuleProperties in GameRules by name: "+name);return null;}}

    public bool _isSkillEquipped(string name){return skillsSlots.Contains(name);}
    public Skill GetSkill(string name){Skill _target=null;_target=skillsList.Find(x=>x.name==name);if(_target!=null){return _target;}else{return null;}}
    public Skill GetSkillFromID(int id){Skill _target=null;_target=skillsList.Find(x=>x.name==skillsSlots[id]);if(_target!=null){return _target;}else{return null;}}
    public SkillPropertiesGR GetSkillProperties(string name){var _target=GameRules.instance.skillsPlayer.Find(x=>x.item.name==name);if(_target!=null){return _target;}else{Debug.LogWarning("No SkillProperties in GameRules by name: "+name);return null;}}
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
