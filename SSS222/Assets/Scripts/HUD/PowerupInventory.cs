using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PowerupInventory : MonoBehaviour{
    [AssetsOnly][SerializeField]GameObject elementPrefab;
    void Start(){SetCapacity();}
    public void SetCapacity(){
        if(Player.instance.powerups.Count==1){transform.GetChild(0).gameObject.SetActive(false);}
        else{transform.GetChild(0).gameObject.SetActive(true);}
        foreach(Transform t in transform.GetChild(0)){Destroy(t.gameObject);}
        for(var i=0;i<Player.instance.powerups.Count;i++){var go=Instantiate(elementPrefab,transform.GetChild(0));go.GetComponent<PowerupDisplay>().number=i;}
    }
}
