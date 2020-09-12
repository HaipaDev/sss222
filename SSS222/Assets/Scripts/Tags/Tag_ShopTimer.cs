using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_ShopTimer : MonoBehaviour{
    void OnEnable(){
        var g=GameRules.instance;
        if(g!=null){if(g.shopTimeLimitEnabled!=true){Destroy(this.gameObject);}}Destroy(this);
    }
}
