using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HLaserCollision : MonoBehaviour{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name.Contains("MagneticPulse")){
            Destroy(transform.root.gameObject);
        }
    }
}
