using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_TouchInput : MonoBehaviour{
    List<MonoBehaviour> disabled=new List<MonoBehaviour>();
    void Update(){
    if(SaveSerial.instance!=null){
        if(SaveSerial.instance.settingsData.inputType!=InputType.touch){
        foreach(MonoBehaviour c in GetComponents<MonoBehaviour>()){
            if(c!=this){
                if(!disabled.Contains(c)){
                    disabled.Add(c);}
                    c.enabled=false;}}
        foreach(MonoBehaviour c in GetComponentsInChildren<MonoBehaviour>()){if(c!=this){if(!disabled.Contains(c)){disabled.Add(c);}c.enabled=false;}}
        }else{
            foreach(MonoBehaviour c in disabled)if(c!=this){c.enabled=true;}
        }
    }}
}
