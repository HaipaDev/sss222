﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_PlayerWeapon:MonoBehaviour{
    public float energy;
    public bool charged;
    public bool blockable;
    void Start(){if(GetComponent<Tag_PauseVelocity>()==null){gameObject.AddComponent<Tag_PauseVelocity>();}}
}