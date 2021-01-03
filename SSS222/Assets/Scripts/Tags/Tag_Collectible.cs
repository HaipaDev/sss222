using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_Collectible : MonoBehaviour{
    public bool isPowerup=true;
    void Start(){if(GameSession.maskMode!=0)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;}
}