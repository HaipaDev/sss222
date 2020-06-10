using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPBars : MonoBehaviour{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] int ID;
    GameObject current;
    [SerializeField] bool created;
    void Start(){
        
    }

    void Update(){
        
    }
    private void OnValidate() {
        if(current!=null)Destroy(current);
        created=false;
        if(prefabs[ID]!=null)current=Instantiate(prefabs[ID]);
        created=true;
    }
}
