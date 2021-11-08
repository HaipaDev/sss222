using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlTree_ElementDroprate : MonoBehaviour{
    public float drop;
    public float sum;
    void Update(){
        if(gameObject.GetComponentInParent(typeof(LvlTreePowerups))!=null){var comp=gameObject.GetComponentInParent(typeof(LvlTreePowerups)) as LvlTreePowerups;sum=comp.sum;}
        if(gameObject.GetComponentInParent(typeof(LvlTreeWaves))!=null){var comp=gameObject.GetComponentInParent(typeof(LvlTreeWaves)) as LvlTreeWaves;sum=comp.sum;}
        string perc="?";perc=Mathf.Round(((drop/sum)*100)).ToString();
        GetComponent<TMPro.TextMeshProUGUI>().text=perc+"%";
    }
}
