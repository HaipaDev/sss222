using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesLib : MonoBehaviour{
    [SerializeField]Sprite[] sprites;
    public Sprite GetSprite(int i){return sprites[i];}
}
