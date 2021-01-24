using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicHitbox : MonoBehaviour{
    [SerializeField] int currentID=-1;
    [SerializeField] HitboxValues[] hitboxValues;
    void Update(){
        if(currentID!=-1){
            SetHitbox(currentID);
        }
    }
    public void SetHitbox(int ID){
        if(GetComponent<BoxCollider2D>()!=null){GetComponent<BoxCollider2D>().offset=hitboxValues[ID].offset;GetComponent<BoxCollider2D>().size=hitboxValues[ID].size;}
        if(GetComponent<CircleCollider2D>()!=null){GetComponent<CircleCollider2D>().offset=hitboxValues[ID].offset;GetComponent<CircleCollider2D>().radius=hitboxValues[ID].size.x;}
    }
}
[System.Serializable]public class HitboxValues{
    public Vector2 offset;
    public Vector2 size;
}