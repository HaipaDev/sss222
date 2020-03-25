using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HLaserKillThis : MonoBehaviour{
    [SerializeField] GameObject hlaserPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var hlaserName = hlaserPrefab.name; var hlaserName1 = hlaserPrefab.name + "(Clone)";
        if (other.gameObject.name == hlaserName || other.gameObject.name == hlaserName1) { Destroy(gameObject); }
    }
}
