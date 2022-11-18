using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDisplayTable : MonoBehaviour{
    [Sirenix.OdinInspector.AssetsOnly][SerializeField]GameObject elementPrefab;

    void Update(){
        if(Player.instance!=null){
            for(var i=0;(i<Player.instance.statuses.Count&&i<_maxStatusDisplayCount())&&(transform.GetChild(0).childCount<=i||transform.GetChild(0).childCount==0);i++){
                GameObject go=Instantiate(elementPrefab,transform.GetChild(0));
                go.name="StateDisplay"+i;
                go.GetComponent<StatusDisplay>().number=i;
            }
        }
    }
    int _maxStatusDisplayCount(){
        if(GameCanvas._canSetUpscaledHud()){return 40;}
        else{return 20;}
    }
}
