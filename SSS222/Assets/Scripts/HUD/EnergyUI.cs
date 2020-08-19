using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUI : MonoBehaviour{
    void Start(){
        if(FindObjectOfType<Player>()!=null){if(!FindObjectOfType<Player>().energyOn)Destroy(gameObject);}
        Destroy(this);
    }
}
