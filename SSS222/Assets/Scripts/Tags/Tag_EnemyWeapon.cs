using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_EnemyWeapon:MonoBehaviour{
    public bool blockable;
    void Start(){if(GetComponent<Tag_PauseVelocity>()==null){gameObject.AddComponent<Tag_PauseVelocity>();}}
}