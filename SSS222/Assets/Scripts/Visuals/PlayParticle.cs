using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour{
    void Awake(){
        GetComponent<ParticleSystem>().Play();
    }
    void Start(){
        GetComponent<ParticleSystem>().Play();
    }
}
