using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour{
    [SerializeField] public SkillSlotID[] skills;
    [SerializeField] public skillKeyBind[] skillsBinds;
    public float cooldownQ;
    public float cooldownE;
    [HeaderAttribute("Prefabs etc")]
    [SerializeField] GameObject mPulsePrefab;
    //int index;
    Player player;
    GameSession gameSession;
    void Start(){
        gameSession=FindObjectOfType<GameSession>();
        player=GetComponent<Player>();
    }

    void Update(){
        UseSkills();
    }

    private void OnValidate() {
        System.Array.Resize(ref skillsBinds, skills.Length);
    }

    public void UseSkills(){
        if(cooldownQ>0)cooldownQ-=Time.deltaTime;
        if(cooldownE>0)cooldownE-=Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Q)){
            foreach(SkillSlotID skill in skills){
                var i=skill.ID;
                //if(skill.keySet==skillKeyBind.Q){
                if(skillsBinds[i]==skillKeyBind.Q){
                    //if(i==0){
                    if(cooldownQ<=0){
                        Skills(skillKeyBind.Q,i,skill.enCost,skill.cooldown);
                    }else{AudioSource.PlayClipAtPoint(gameSession.denySFX,transform.position);}
                    //}
                }
            }
        }if(Input.GetKeyDown(KeyCode.E)){
            foreach(SkillSlotID skill in skills){
                var i=skill.ID;
                //if(skill.keySet==skillKeyBind.E){
                if(skillsBinds[i]==skillKeyBind.E){
                    //if(i==0){
                    if(cooldownE<=0){
                        Skills(skillKeyBind.E,i,skill.enCost,skill.cooldown);
                    }else{AudioSource.PlayClipAtPoint(gameSession.denySFX,transform.position);}
                    //}
                }
            }
        }
    }
    
    #region//Skills
    public void Skills(skillKeyBind key,int i,float enCost,float cooldown){
        if(player.energy>=enCost){
            player.energy-=enCost;
            player.EnergyPopUpHUD(enCost);
            if(key==skillKeyBind.Q){cooldownQ=cooldown;}
            if(key==skillKeyBind.E){cooldownE=cooldown;}
            if(i==0){
                GameObject mPulse=Instantiate(mPulsePrefab, transform.position,Quaternion.identity);
            }
        }
    }
    #endregion
}
