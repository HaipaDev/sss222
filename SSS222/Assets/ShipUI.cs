using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipUI : MonoBehaviour{
    void Start(){
        var ps=transform.GetChild(0).GetComponent<ParticleSystem>();
        if(ps.startSize<10)ps.startSize*=10;
        transform.GetChild(0).GetComponent<UnityEngine.UI.Extensions.UIParticleSystem>().material=ps.GetComponent<Renderer>().material;
    }
}
