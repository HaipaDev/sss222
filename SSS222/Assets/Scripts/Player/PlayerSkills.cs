using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour{
    [SerializeField] public SkillSlotID[] skills;
    [SerializeField] public skillKeyBind[] skillsBinds;
    public float cooldownQ;
    public float cooldownE;
    [SerializeField] GameObject timerUI;
    [HeaderAttribute("Prefabs etc")]
    GameObject mPulsePrefab;
    [HeaderAttribute("Effects")]
    [SerializeField] GameObject portalVFX;
    //[SerializeField] AudioClip portalSFX;
    [HeaderAttribute("Timers etc")]
    public int currentSkillID=-1;
    public float timerTeleport=-4;
    
    //References
    Player player;
    GameSession gameSession;
    UpgradeMenu umenu;
    void Start(){
        gameSession=FindObjectOfType<GameSession>();
        umenu=FindObjectOfType<UpgradeMenu>();
        player=GetComponent<Player>();
        mPulsePrefab=GameAssets.instance.Get("MPulse");
    }

    void Update(){
        UseSkills(0);
        SkillsUpdate();
    }

    private void OnValidate() {
        System.Array.Resize(ref skillsBinds, skills.Length);
    }

    public void UseSkills(int key){
        if(cooldownQ>0)cooldownQ-=Time.deltaTime;
        if(cooldownE>0)cooldownE-=Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Q) || key==1){
            foreach(SkillSlotID skill in skills){
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
            foreach(SkillSlotID skill in skills){
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
    public void DeathSkills(){
        if(umenu.magneticPulse_upgraded==2)Skills(skillKeyBind.Disabled,0,0,0);//PostMortem MagneticPulse
    }
    
    #region//Skills
    public void Skills(skillKeyBind key,int i,float enCost,float cooldown){
        if(player.energy>=enCost){
            player.AddSubEnergy(enCost,false);
            player.EnergyPopUpHUD(enCost);
            if(key==skillKeyBind.Q){cooldownQ=cooldown;}
            if(key==skillKeyBind.E){cooldownE=cooldown;}
            if(key==skillKeyBind.Disabled){}
            if(i==0){
                GameObject mPulse=Instantiate(mPulsePrefab, transform.position,Quaternion.identity);
            }if(i==1){
                gameSession.gameSpeed=0.025f;
                gameSession.speedChanged=true;
                SetActiveAllChildren(timerUI.transform,true);
                currentSkillID=i;
            }
        }
    }

    public void SkillsUpdate(){
        if(currentSkillID==-1){
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
                    gameSession.speedChanged=false;gameSession.gameSpeed=1f;timerTeleport=-4;currentSkillID=-1;}
            }else if(timerTeleport<0 && timerTeleport!=-4){
                SetActiveAllChildren(timerUI.transform,false);
                gameSession.speedChanged=false;gameSession.gameSpeed=1f;timerTeleport=-4;currentSkillID=-1;
            }
        }
    }
    #endregion

    private void SetActiveAllChildren(Transform transform, bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);

            SetActiveAllChildren(child, value);
        }
    }
}
