using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockboxItemDrop : MonoBehaviour{
    public string name;
    void Start(){
        if(name!=""){
            var lb=AssetsManager.instance.lockboxes.Find(x=>x.name==name);
            GetComponent<SpriteRenderer>().sprite=lb.icon;
        }
    }
}
