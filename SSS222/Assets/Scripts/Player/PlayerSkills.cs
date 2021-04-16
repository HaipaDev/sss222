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
    [SerializeField] GameObject timerUI;
    [HeaderAttribute("Prefabs etc")]
    GameObject mPulsePrefab;
    [SerializeField] GameObject portalVFX;
    [HeaderAttribute("Timers etc")]
    public float cooldownQ;
    public float cooldownE;
    public int currentSkillID=-1;
    public float timerTeleport=-4;
    public float timerOverhaul=-4;
    public float timeOverhaul=10;
    
    //References
    Player player;
    UpgradeMenu umenu;
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
        umenu=FindObjectOfType<UpgradeMenu>();
        player=GetComponent<Player>();
        timerUI=GameObject.Find("SkillTimerUI");
        mPulsePrefab=GameAssets.instance.Get("MPulse");
        if(GameRules.instance.upgradesOn!=true){Destroy(this);}
    }

    void Update(){
        UseSkills(0);
        SkillsUpdate();
    }

    private void OnValidate(){
        if(skills.Length>0){foreach(Skill s in skills){/*s.ID=s.item.ID;*/if(s.item!=null)s.name=s.item.name;  /*s.enCost=s.item.enCost;s.cooldown=s.item.cooldown;*/}}
        ResizeSet();
    }
    private void ResizeSet(){
        System.Array.Resize(ref skillsBinds, skills.Length);
        for(var i=0;i<skills.Length;i++){skills[i].ID=i;}
        System.Array.Resize(ref cooldowns, skills.Length);
    }

    public void UseSkills(int key){
        for(var i=0;i<cooldowns.Length;i++){
            if(cooldowns[i]>0)cooldowns[i]-=Time.deltaTime;

            if(skillsBinds[i]==skillKeyBind.Q){cooldownQ=cooldowns[i];}
            else if(skillsBinds[i]==skillKeyBind.E){cooldownE=cooldowns[i];}
        }
    if(Time.deltaTime>0.0001&&player.hacked!=true){
        
        //if(cooldownQ>0)cooldownQ-=Time.deltaTime;
        //if(cooldownE>0)cooldownE-=Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Q) || key==1){
            foreach(Skill skill in skills){
                var i=skill.ID;
                //if(skill.keySet==skillKeyBind.Q){
                if(skillsBinds[i]==skillKeyBind.Q){
                    //if(i==0){
                    if(cooldownQ<=0 && Time.deltaTime>0.0001f){
                        Skills(skillKeyBind.Q,i,skill.enCost,skill.cooldown);
                    }else{AudioManager.instance.Play("Deny");}
                    //}
                }
            }
        }if(Input.GetKeyDown(KeyCode.E) || key==2){
            foreach(Skill skill in skills){
                var i=skill.ID;
                //if(skill.keySet==skillKeyBind.E){
                if(skillsBinds[i]==skillKeyBind.E){
                    //if(i==0){
                    if(cooldownE<=0 && Time.deltaTime>0.0001f){
                        Skills(skillKeyBind.E,i,skill.enCost,skill.cooldown);
                    }else{AudioManager.instance.Play("Deny");}
                    //}
                }
            }
        }
    }
    }
    public void DeathSkills(){
        if(umenu.mPulse_upgraded==2)Skills(skillKeyBind.Disabled,0,0,0);//PostMortem MagneticPulse
    }
    
    #region//Skills
    public void Skills(skillKeyBind key,int i,float enCost,float cooldown){
        cooldowns[i]=skills[i].cooldown;
        //if(skillsBinds[i]==skillKeyBind.Q){cooldownQ=skills[i].cooldown;}
        //else if(skillsBinds[i]==skillKeyBind.E){cooldownE=skills[i].cooldown;}
        if(i!=2){
        if(player.energy>0){
            player.AddSubEnergy(enCost,false);
            //if(key==skillKeyBind.Q){cooldownQ=cooldown;}
            //if(key==skillKeyBind.E){cooldownE=cooldown;}
            if(key==skillKeyBind.Disabled){}
            if(i==0){//Magnetic Pulse
                GameObject mPulse=Instantiate(mPulsePrefab, transform.position,Quaternion.identity);
            }if(i==1){//Teleport
                GameSession.instance.gameSpeed=0.025f;
                GameSession.instance.speedChanged=true;
                SetActiveAllChildren(timerUI.transform,true);
                currentSkillID=1;
            }
        }else{AudioManager.instance.Play("Deny");}
        }else if(i==2){//Overhaul
        if(GameSession.instance.coresXp>0){
                //if(key==skillKeyBind.Q){cooldownQ=cooldown;}
                //if(key==skillKeyBind.E){cooldownE=cooldown;}
                if(key==skillKeyBind.Disabled){}
                if(player.energy<1){player.AddSubEnergy(20);}
                var ratio=(GameSession.instance.coresXp/GameSession.instance.xp_forCore);
                GameCanvas.instance.XPPopUpHUD(-GameSession.instance.coresXp);
                player.InfEnergy(ratio*33);
                player.Power(16,Mathf.Clamp(3f*ratio,1.1f,2.2f));
                timerOverhaul=timeOverhaul;
                GameSession.instance.coresXp=0;
                AudioManager.instance.Play("Overhaul");
                AudioManager.instance.GetSource("Overhaul").loop=true;
                //if(overhaulAudio==null){overhaulAudio=gameObject.AddComponent(typeof(AudioSource)) as AudioSource;}
                //if(overhaulAudio!=null){overhaulAudio.clip=AudioManager.instance.Get("Overhaul");overhaulAudio.loop=true;overhaulAudio.Play();}
        }
        }else{AudioManager.instance.Play("Deny");}
    }

    public void SkillsUpdate(){
        if(currentSkillID!=1){
            SetActiveAllChildren(timerUI.transform,false);
        }
        if(currentSkillID==1){
            if(timerTeleport==-4)timerTeleport=3f;
            if(timerTeleport>0){
                if(Time.timeScale>0.0001f)timerTeleport-=Time.unscaledDeltaTime;
                if(Input.GetMouseButtonDown(0)){
                    AudioManager.instance.Play("Portal");
                    GameObject tp1 = Instantiate(portalVFX,transform.position,Quaternion.identity);
                    GameObject tp2 = Instantiate(portalVFX,player.mousePos,Quaternion.identity);
                    var ps1=tp1.GetComponent<ParticleSystem>();var main1=ps1.main;
                    main1.startColor=Color.blue;
                    var ps2=tp2.GetComponent<ParticleSystem>();var main2=ps2.main;
                    main2.startColor=new Color(255,140,0,255);//Orange
                    Destroy(tp1,1.25f);Destroy(tp2,1.25f);
                    transform.position=player.mousePos;
                    SetActiveAllChildren(timerUI.transform,false);
                    GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;timerTeleport=-4;currentSkillID=-1;}
            }else if(timerTeleport<0 && timerTeleport!=-4){
                SetActiveAllChildren(timerUI.transform,false);
                GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;timerTeleport=-4;currentSkillID=-1;
            }
        }
        if(timerOverhaul>0&&player.infEnergy){
            timerOverhaul-=Time.deltaTime;
        }if((timerOverhaul<0&&timerOverhaul!=-4)&&player.infEnergy){player.Overhaul();timerOverhaul=timeOverhaul;}
        if(!player.infEnergy&&AudioManager.instance!=null&&AudioManager.instance.GetSource("Overhaul").isPlaying){AudioManager.instance.StopPlaying("Overhaul");}//Destroy(overhaulAudio);}
    }
    #endregion

    private void SetActiveAllChildren(Transform transform, bool value){
        foreach (Transform child in transform){
            child.gameObject.SetActive(value);
            SetActiveAllChildren(child, value);
        }
    }
}
