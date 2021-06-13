using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour{
    void Start(){
        GetComponent<SpriteRenderer>().sprite=Player.instance.GetComponent<SpriteRenderer>().sprite;
    }
}
