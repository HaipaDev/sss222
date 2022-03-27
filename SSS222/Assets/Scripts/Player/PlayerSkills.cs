using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]public class Skill{
    [HideInInspector]public string name;
    [HideInInspector]public int ID;
    public SkillSlotID item;
    public float enCost;
    public float cooldown;
}
public class PlayerSkills : MonoBehaviour{
    [SerializeField] public Skill[] skills;
    [SerializeField] public skillKeyBind[] skillsBinds;
    public float[] cooldowns;
    [HeaderAttribute("Timers etc")]
    public float cooldownQ;
    public float cooldownE;
    public int currentSkillID=-1;
    public float timerTeleport=-4;
    public float timerOverhaul=-4;
    public float timeOverhaul=10;

    Player player;
    GameObject timerUI;
    void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.15f);
        //Set values
        var i=GameRules.instance;
        if(i!=null){
            skills=i.skillsPlayer;
            timeOverhaul=i.timeOverhaul;
        }
        ResizeSet();
    }
    void Start(){
        player=GetComponent<Player>();
        timerUI=GameObject.Find("SkillTimer_par");
        if(GameRules.instance.modulesOn!=true){Destroy(this);}
    }

    void Update(){
        UseSkills(0);
        SkillsUpdate();
    }

    private void OnValidate(){
        if(skills.Length>0){foreach(Skill s in skills){if(s.item!=null)s.name=s.item.name;}}
        ResizeSet();
    }
    private void ResizeSet(){
        System.Array.Resize(ref skillsBinds, skills.Length);
        for(var i=0;i<skills.Length;i++){skills[i].ID=i;}
        System.Array.Resize(ref cooldowns, skills.Length);
    }

    public void UseSkills(int key){ if(!GameSession.GlobalTimeIsPaused){
        for(var i=0;i<cooldowns.Length;i++){
            if(cooldowns[i]>0)cooldowns[i]-=Time.deltaTime;

            if(skillsBinds[i]==skillKeyBind.Q){cooldownQ=cooldowns[i];}
            else if(skillsBinds[i]==skillKeyBind.E){cooldownE=cooldowns[i];}
        }
        if(player.hacked!=true){
            if(Input.GetKeyDown(KeyCode.Q)||Input.GetKeyDown(KeyCode.JoystickButton2) || key==1){
                foreach(Skill skill in skills){
                    var i=skill.ID;
                    if(skillsBinds[i]==skillKeyBind.Q){
                        if(cooldownQ<=0 && Time.deltaTime>0.0001f){
                            Skills(skillKeyBind.Q,i,skill.enCost,skill.cooldown);
                        }else{AudioManager.instance.Play("Deny");}
                    }
                }
            }if(Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.JoystickButton1) || key==2){
                foreach(Skill skill in skills){
                    var i=skill.ID;
                    if(skillsBinds[i]==skillKeyBind.E){
                        if(cooldownE<=0 && Time.deltaTime>0.0001f){
                            Skills(skillKeyBind.E,i,skill.enCost,skill.cooldown);
                        }else{AudioManager.instance.Play("Deny");}
                    }
                }
            }
        }
    }}
    public void DeathSkills(){
        if(UpgradeMenu.instance.mPulse_upgraded==2)Skills(skillKeyBind.Disabled,0,0,0);//PostMortem MagneticPulse
    }
    
    #region//Skills
    public void Skills(skillKeyBind key,int i,float enCost,float cooldown){
        if(i!=2){
        if(player.energy>0){
            cooldowns[i]=skills[i].cooldown;
            player.AddSubEnergy(enCost,false);
            if(key==skillKeyBind.Disabled){}
            if(i==0){//Magnetic Pulse
                GameObject mPulse=GameAssets.instance.Make("MPulse",transform.position);
            }if(i==1){//Teleport
                GameSession.instance.gameSpeed=0.025f;
                GameSession.instance.speedChanged=true;
                if(timerUI!=null)SetActiveAllChildren(timerUI.transform,true);
                currentSkillID=1;
            }
        }else{AudioManager.instance.Play("Deny");}
        }else if(i==2){//Overhaul
        if(GameSession.instance.xp>0){
                cooldowns[i]=skills[i].cooldown;
                if(key==skillKeyBind.Disabled){}
                if(player.energy<1){player.AddSubEnergy(20);}
                var ratio=(GameSession.instance.xp/GameSession.instance.xpMax);
                GameCanvas.instance.XPPopUpHUD(-GameSession.instance.xp);
                player.InfEnergy(ratio*33);
                player.Power(16,Mathf.Clamp(3f*ratio,1.1f,2.2f));
                timerOverhaul=timeOverhaul;
                GameSession.instance.xp=0;
                AudioManager.instance.Play("Overhaul");
                AudioManager.instance.GetSource("Overhaul").loop=true;
        }
        }else{AudioManager.instance.Play("Deny");}
    }

    public void SkillsUpdate(){
    if(!GameSession.GlobalTimeIsPaused){
        if(currentSkillID!=1){
            if(timerUI!=null)SetActiveAllChildren(timerUI.transform,false);
        }
        if(currentSkillID==1){
            if(timerTeleport==-4)timerTeleport=3f;
            if(timerTeleport>0){
                timerTeleport-=Time.unscaledDeltaTime;
                if(Input.GetMouseButtonDown(0)){
                    AudioManager.instance.Play("Portal");
                    GameObject tp1=GameAssets.instance.VFX("PortalVFX",transform.position,1.25f);
                    GameObject tp2=GameAssets.instance.VFX("PortalVFX",player.mousePos,1.25f);
                    var ps1=tp1.GetComponent<ParticleSystem>();var main1=ps1.main;
                    main1.startColor=Color.blue;
                    var ps2=tp2.GetComponent<ParticleSystem>();var main2=ps2.main;
                    main2.startColor=new Color(255,140,0,255);//Orange
                    transform.position=player.mousePos;
                    if(timerUI!=null)SetActiveAllChildren(timerUI.transform,false);
                    GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;timerTeleport=-4;currentSkillID=-1;}
            }else if(timerTeleport<0 && timerTeleport!=-4){
                if(timerUI!=null)SetActiveAllChildren(timerUI.transform,false);
                GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;timerTeleport=-4;currentSkillID=-1;
            }
        }
        if(timerOverhaul>0&&player.infEnergy){
            timerOverhaul-=Time.deltaTime;
        }if((timerOverhaul<0&&timerOverhaul!=-4)&&player.infEnergy){timerOverhaul=timeOverhaul;}
        if(!player.infEnergy&&AudioManager.instance.GetSource("Overhaul").isPlaying){AudioManager.instance.StopPlaying("Overhaul");}
    }}
    #endregion
    public void ResetSkillCooldowns(){
        for(var i=0;i<cooldowns.Length;i++){cooldowns[i]=0;}
        cooldownQ=0;
        cooldownE=0;
    }

    private void SetActiveAllChildren(Transform transform, bool value){
        foreach (Transform child in transform){
            child.gameObject.SetActive(value);
            SetActiveAllChildren(child, value);
        }
    }
}
