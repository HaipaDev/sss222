using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDisplayTable : MonoBehaviour{
    [Sirenix.OdinInspector.AssetsOnly][SerializeField]GameObject elementPrefab;
    GameObject table;
    void Start(){
        table=transform.GetChild(0).gameObject;
    }

    void Update(){
        //Create
        if(Player.instance!=null){
        for(var i=0;i<Player.instance.statuses.Count;i++){
            if(table.transform.childCount==0){
                GameObject go=Instantiate(elementPrefab,table.transform);
                go.name="StateDisplay"+0;
                go.GetComponent<StatusDisplay>().number=0;
            }
            if(i>0&&table.transform.childCount<=i){
                //if(table.transform.childCount==i){}
                //else{
                GameObject go=Instantiate(elementPrefab,table.transform);
                go.name="StateDisplay"+i;
                go.GetComponent<StatusDisplay>().number=i;
                //}
            }
        }
        }
        //Destroy
        /*for(var i=player.statuses.Count;i<26;i++){
            if(table.transform.childCount>=i){
            if(table.transform.GetChild(i).GetComponent<PowerupDisplay>().state==""){
                Destroy(table.transform.GetChild(i).gameObject);
            }
            }
        }*/
    }
}
