using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_NotAndroid : MonoBehaviour{
    void Start(){
        if (Application.platform == RuntimePlatform.Android){
            Destroy(this.gameObject);
        }
    }
}
