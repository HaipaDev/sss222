using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_CertainInput : MonoBehaviour{
    [SerializeField]InputType inputType=InputType.touch;
    List<MonoBehaviour> disabled=new List<MonoBehaviour>();
    void Update(){
    if(SaveSerial.instance!=null){
        if(inputType==InputType.touch&&SaveSerial.instance.settingsData.inputType!=InputType.touch){
            foreach(MonoBehaviour c in GetComponents<MonoBehaviour>()){if(c!=this){if(!disabled.Contains(c)){disabled.Add(c);}c.enabled=false;}}
            foreach(MonoBehaviour c in GetComponentsInChildren<MonoBehaviour>()){if(c!=this){if(!disabled.Contains(c)){disabled.Add(c);}c.enabled=false;}}
        }else if(inputType==InputType.touch&&SaveSerial.instance.settingsData.inputType==InputType.touch){
            if(Player.instance!=null){
                if(Player.instance.GetComponent<PlayerSkills>().timerTeleport!=-4){
                    foreach(MonoBehaviour c in GetComponents<MonoBehaviour>()){if(c!=this){if(!disabled.Contains(c)){disabled.Add(c);}c.enabled=false;}}
                    foreach(MonoBehaviour c in GetComponentsInChildren<MonoBehaviour>()){if(c!=this){if(!disabled.Contains(c)){disabled.Add(c);}c.enabled=false;}}
                }
                if(Player.instance.GetComponent<PlayerSkills>().timerTeleport==-4){foreach(MonoBehaviour c in disabled)if(c!=this){c.enabled=true;}}
            }else{foreach(MonoBehaviour c in disabled)if(c!=this){c.enabled=true;}}
        }
        else if(inputType==InputType.mouse&&SaveSerial.instance.settingsData.inputType!=InputType.mouse){
            foreach(MonoBehaviour c in GetComponents<MonoBehaviour>()){if(c!=this){if(!disabled.Contains(c)){disabled.Add(c);}c.enabled=false;}}
            foreach(MonoBehaviour c in GetComponentsInChildren<MonoBehaviour>()){if(c!=this){if(!disabled.Contains(c)){disabled.Add(c);}c.enabled=false;}}
        }else if(inputType==InputType.mouse&&SaveSerial.instance.settingsData.inputType==InputType.mouse){foreach(MonoBehaviour c in disabled)if(c!=this){c.enabled=true;}
        }
    }}
}
