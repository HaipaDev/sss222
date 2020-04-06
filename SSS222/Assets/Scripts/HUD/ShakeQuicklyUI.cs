using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeQuicklyUI : MonoBehaviour{
    void Update(){
        if (FindObjectOfType<DetachFromFollow>().attached == true){GetComponent<TMPro.TextMeshProUGUI>().enabled = true; }
        if(FindObjectOfType<DetachFromFollow>()==null){GetComponent<TMPro.TextMeshProUGUI>().enabled = false;}
    }
}
