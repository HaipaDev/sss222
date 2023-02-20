using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnableOnPause : MonoBehaviour{
    [SerializeField] bool children=true;
    [SerializeField] bool components=false;
    [SerializeField] bool enable=true;
    List<Transform> _disabledList;
    List<MonoBehaviour> _disabledComponentsList;
    void Update(){
        if(PauseMenu.GameIsPaused){
            if(!children){
                if(!components){gameObject.SetActive(enable);}
                else{
                    if(_disabledComponentsList!=null&&_disabledComponentsList.Count>0)
                    foreach(MonoBehaviour c in transform.GetComponents<MonoBehaviour>()){
                        c.enabled=enable;_disabledComponentsList.Remove(c);
                    }
                }
                
            }else{
                if(!components){
                    if(_disabledList!=null&&_disabledList.Count>0)
                    foreach(Transform t in _disabledList){
                        t.gameObject.SetActive(enable);_disabledList.Remove(t);
                    }
                }else{
                    if(_disabledList!=null&&_disabledList.Count>0)
                    foreach(MonoBehaviour c in _disabledComponentsList){
                        c.enabled=enable;_disabledComponentsList.Remove(c);
                    }
                }
            }
        }else{
            if(!children){
                if(!components){gameObject.SetActive(!enable);}
                else{
                    foreach(MonoBehaviour c in transform.GetComponents<MonoBehaviour>()){
                        if(c.enabled&&!_disabledComponentsList.Contains(c)){_disabledComponentsList.Add(c);c.enabled=!enable;}
                    }
                }
            }else{
                if(!components){
                    if(_disabledList!=null&&_disabledList.Count>0)
                    foreach(Transform t in transform.GetComponentsInChildren<Transform>()){
                        if(t.gameObject.activeSelf&&!_disabledList.Contains(t)){_disabledList.Add(t);t.gameObject.SetActive(!enable);}
                    }
                }
                else{
                    if(_disabledComponentsList!=null&&_disabledComponentsList.Count>0)
                    foreach(MonoBehaviour c in transform.GetComponents<MonoBehaviour>()){
                        if(c.enabled&&!_disabledComponentsList.Contains(c)){_disabledComponentsList.Add(c);c.enabled=!enable;}
                    }
                }
            }
        }
    }
}
