using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeQuicklyUI : MonoBehaviour{
    void Update(){
        if(FindObjectOfType<LeechAttach>()!=null&&FindObjectOfType<LeechAttach>().attached==true){GetComponent<TMPro.TextMeshProUGUI>().enabled=true;}
        else{GetComponent<TMPro.TextMeshProUGUI>().enabled=false;}
    }
}
