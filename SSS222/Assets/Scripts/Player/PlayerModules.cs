using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]public class Skill{
    public SkillProperties item;
    public float cooldown;
}
public class PlayerModules : MonoBehaviour{
    [Header("Settings")]
    public bool exhaustROF=true;
    public float timeTeleport=3f;
    public float timeOverhaul=10f;
    [Header("Slots")]
    [SerializeField] public List<string> moduleSlots;
    [SerializeField] public List<Skill> skillsSlots=new List<Skill>(2);
    [Header("Timers etc")]
    [ReadOnly]public string currentSkill="";
    [ReadOnly]public float timerTeleport=-4;
    [ReadOnly]public float timerOverhaul=-4;

    GameObject timerUI;
    void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.15f);
        var i=GameRules.instance;
        if(i!=null){
            moduleSlots=new List<string>(i.playerModulesCapacity);
            //skillsSlots=new Skill[i.playerSkillsCapacity];
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
        for(var i=0;i<skillsSlots.Capacity;i++){
            if(skillsSlots[i].cooldown>0)skillsSlots[i].cooldown-=Time.deltaTime;
        }
        if(!Player.instance._hasStatus("hacked")){
            var _slot=0;
            bool _key1=(Input.GetKeyDown(KeyCode.Q)||Input.GetKeyDown(KeyCode.JoystickButton2)||key==1);
            bool _key2=(Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.JoystickButton1)||key==2);    if(_key2){_slot=1;}
            if(_key1||_key2){
                if(skillsSlots[_slot].cooldown<=0){Skills(skillsSlots[_slot].item);}
                else{AudioManager.instance.Play("Deny");}
            }
        }
    }}
    public void DeathSkills(){
        if(UpgradeMenu.instance.mPulse_upgraded==2)Skills(GetSkillProperties("Magnetic Pulse"));//PostMortem MagneticPulse
    }
    
    #region//Skills
    public void Skills(SkillProperties item){
        if(item.name!="Overhaul"){
            if(Player.instance.energy>0){
                Player.instance.AddSubEnergy(item.costTypeProperties.cost,false);
                if(item.name=="Magnetic Pulse"){
                    GameObject mPulse=GameAssets.instance.Make("MPulse",transform.position);
                    GetSkill(item.name).cooldown=item.cooldown;
                }if(item.name=="Teleport"){//Teleport
                    GameSession.instance.gameSpeed=0.025f;
                    GameSession.instance.speedChanged=true;
                    if(timerUI!=null)SetActiveAllChildren(timerUI.transform,true);
                    currentSkill="Teleport";
                GetSkill(item.name).cooldown=item.cooldown;
                }
            }else{AudioManager.instance.Play("Deny");}
        }else if(item.name=="Overhaul"){//Overhaul
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
                GetSkill(item.name).cooldown=item.cooldown;
            }else{AudioManager.instance.Play("Deny");}
        }
    }

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
    public bool _isModuleEquiped(string name){return moduleSlots.Contains(name);}
    public void SetModule(int id, string item){moduleSlots[id]=item;}
    public void SetSkillByID(int id, SkillProperties item){skillsSlots[id].item=item;}
    public void SetSkillByName(string name, SkillProperties item){skillsSlots.Find(x=>x.item.name==name).item=item;}
    public Skill GetSkill(string name){return skillsSlots.Find(x=>x.item.name==name);}
    public SkillProperties GetSkillProperties(string name){return GameRules.instance.skillsPlayer.Find(x=>x.name==name);}

    public void ResetSkillCooldowns(){
        for(var i=0;i<skillsSlots.Capacity;i++){skillsSlots[i].cooldown=0;}
    }

    private void SetActiveAllChildren(Transform transform, bool value){
        foreach (Transform child in transform){
            child.gameObject.SetActive(value);
            SetActiveAllChildren(child, value);
        }
    }
}
