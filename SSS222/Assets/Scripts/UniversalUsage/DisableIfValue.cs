using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class DisableIfValue : MonoBehaviour{
    [SerializeField] string valueName;
    [SerializeField] float valueSet=1;
    [SerializeField] bool below;
    [SerializeField] bool disableComponents=true;
    [SerializeField] bool disableChildren=false;
    [ShowIf("disableChildren")][SerializeField] bool disableChildrenComponents=true;
    [ReadOnly][SerializeField] float value;
    [SerializeField] float timeToCheckForPresence=3;
    [DisableInEditorMode][SerializeField] float timerToCheckForPresence;
    [DisableInEditorMode][SerializeField] float delay;
    void Start(){timerToCheckForPresence=timeToCheckForPresence;delay=0.15f;}
    void Update(){
        if(timerToCheckForPresence>0){timerToCheckForPresence-=Time.deltaTime;}
        if(delay>0){delay-=Time.unscaledDeltaTime;}
        if(delay<=0){
            if(valueName=="holodeath_popup"){
                if(FindObjectOfType<PlayerHolobody>()!=null&&Player.instance!=null){
                    value=FindObjectOfType<PlayerHolobody>().GetTimeLeft();
                }else{DisableIfNotPresent();}
            }
            else if(valueName=="isBossZone"){value=GameAssets.BoolToInt(FindObjectOfType<BossAI>()!=null);}

            void DisableIfNotPresent(){if(timerToCheckForPresence<=0){Disable();}}
            if(!below){if(value>=valueSet){Disable();}}
            else{if(value<=valueSet){Disable();}}
        }
    }
    void Disable(){
        if(disableComponents){
            foreach(MonoBehaviour c in GetComponents<MonoBehaviour>()){if(c!=this)c.enabled=false;}
        }
        if(disableChildren){
            if(disableChildrenComponents){foreach(MonoBehaviour c in GetComponentsInChildren<MonoBehaviour>()){c.enabled=false;}}
            else{foreach(Transform t in transform){t.gameObject.SetActive(false);}}
        }else{gameObject.SetActive(false);}
    }
}
