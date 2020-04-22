using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRocketsExplode : MonoBehaviour{
    [SerializeField] GameObject explosionPrefab;
    private void OnTriggerEnter2D(Collider2D other) {
        if(!CompareTag(other.tag)){
            var explosion=Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion,1f);
            Destroy(gameObject);
        }
    }
}
