using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PowerupInventory : MonoBehaviour{
    [AssetsOnly][SerializeField]GameObject elementPrefab;
    void Start(){
        if(GameRules.instance.powerupsCapacity==1){Destroy(gameObject);}
        var _iMax=transform.GetChild(0).childCount;
        for(var i=0;i<_iMax;i++){Destroy(transform.GetChild(0).GetChild(i).gameObject);}
        for(var i=0;i<GameRules.instance.powerupsCapacity;i++){var go=Instantiate(elementPrefab,transform.GetChild(0));go.GetComponent<PowerupDisplay>().number=i;}
    }
}
