using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_OutsideZone : MonoBehaviour{void Start(){if(!GameRules.instance.barrierOn)Destroy(transform.parent.gameObject);}}
