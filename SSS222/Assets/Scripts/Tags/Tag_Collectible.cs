using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_Collectible : MonoBehaviour{void Start(){if(GameSession.maskMode!=0)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;}}