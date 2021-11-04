using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_PlayerWeapon:MonoBehaviour{void Start(){if(GetComponent<Tag_PauseVelocity>()==null&&GetComponent<Lightsaber>()==null){gameObject.AddComponent<Tag_PauseVelocity>();}}}