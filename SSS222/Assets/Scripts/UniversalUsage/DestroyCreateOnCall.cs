using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCreateOnCall : MonoBehaviour{
    public void CreateObject(GameObject obj){
        Instantiate(obj);
    }
    public void DestroyObject(GameObject obj){
        Destroy(obj);
    }
}
