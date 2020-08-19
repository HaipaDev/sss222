using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeQuicklyUI : MonoBehaviour{
    void Update(){
        if(FindObjectOfType<DetachFromFollow>()!=null && FindObjectOfType<DetachFromFollow>().attached == true){GetComponent<TMPro.TextMeshProUGUI>().enabled = true; }
        else{GetComponent<TMPro.TextMeshProUGUI>().enabled = false;}
    }
}
