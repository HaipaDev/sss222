using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableObject : MonoBehaviour{
    public void EnableObject(GameObject obj,bool thisObj){
        obj.SetActive(true);
    }
    public void DisableObject(GameObject obj){
        obj.SetActive(false);
    }
}
