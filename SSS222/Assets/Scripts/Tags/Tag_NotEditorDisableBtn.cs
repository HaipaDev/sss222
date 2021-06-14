using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tag_NotEditorDisableBtn : MonoBehaviour{
    void Start(){
        #if !UNITY_EDITOR
            GetComponent<Button>().interactable=false;
        #endif
    }
}
