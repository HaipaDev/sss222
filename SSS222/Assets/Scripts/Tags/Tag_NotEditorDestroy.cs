using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_NotEditorDestroy : MonoBehaviour{
    void Start(){
        #if !UNITY_EDITOR
            Destroy(gameObject);
        #endif
    }
    void Update(){
        
    }
}
