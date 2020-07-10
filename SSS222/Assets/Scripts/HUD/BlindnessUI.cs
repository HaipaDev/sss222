using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlindnessUI : MonoBehaviour{
    public bool on;
    void Start(){
        
    }

    void Update(){
        on=FindObjectOfType<Player>().blind;
        GetComponent<Image>().enabled=on;
    }
}
