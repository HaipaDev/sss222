using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tag_BlurImg : MonoBehaviour{
    public bool on;
    Image img;
    void Start(){
        img=GetComponent<Image>();
    }

    void Update(){
        img.enabled=on;
    }
    void OnValidate(){
        GetComponent<Image>().enabled=on;
    }
}
