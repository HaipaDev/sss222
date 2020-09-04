using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_OnlyAndroid : MonoBehaviour{
    void Start(){
        if (Application.platform != RuntimePlatform.Android){
            Destroy(this.gameObject);
        }
    }
}
