using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAsChild : MonoBehaviour{
    [SerializeField] GameObject obj;
    void Start(){
        Instantiate(obj,transform,this.transform);
        this.enabled=false;
    }
}
