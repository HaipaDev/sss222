using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SwitchIfValue : MonoBehaviour{
    [SerializeField] string valueName;
    [SerializeField] float valueSet=1;
    [ReadOnly][SerializeField] float value;
    [SerializeField] bool below;
    [SerializeField] bool enable=false;
    [SerializeField] bool revertAfterChanged=true;
    [SerializeField] bool components=true;
    [SerializeField] bool children=false;
    [ShowIf("children")][SerializeField] bool childrenComponents=true;
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
            else if(valueName=="breakEncounter"){
                if(BreakEncounter.instance!=null){
                    value=GameAssets.BoolToInt(BreakEncounter.instance.calledBreak);
                }else{DisableIfNotPresent();}
            }

            void DisableIfNotPresent(){if(timerToCheckForPresence<=0){Switch(enable);}}
            if(!below){if(value>=valueSet){Switch(enable);}else{if(revertAfterChanged)Switch(!enable);}}
            else{if(value<=valueSet){Switch(enable);}else{if(revertAfterChanged)Switch(!enable);}}
        }
    }
    void Switch(bool on=false){
        if(components){
            foreach(MonoBehaviour c in GetComponents<MonoBehaviour>()){if(c!=this)c.enabled=on;}
        }
        if(children){
            if(childrenComponents){foreach(MonoBehaviour c in GetComponentsInChildren<MonoBehaviour>(true)){if(c!=this)c.enabled=on;}}
            else{foreach(Transform t in transform){t.gameObject.SetActive(on);}}
        }
        if(!components&&!children){gameObject.SetActive(on);}
    }
}
