using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_RepBar : MonoBehaviour{
    void Start(){
        if(Shop.instance.repEnabled!=true){Destroy(this.gameObject);}Destroy(this);
    }
}
