using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontShootUI : MonoBehaviour{
    void Update(){
        if(FindObjectOfType<VortexWheel>()!=null&&FindObjectOfType<VortexWheel>().timer<FindObjectOfType<VortexWheel>().GetTimeToDie()){GetComponent<TMPro.TextMeshProUGUI>().enabled=true;}
        else{GetComponent<TMPro.TextMeshProUGUI>().enabled=false;}
    }
}
