using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SpritesLib : MonoBehaviour{
    [SerializeField]List<Sprite> sprites;
    public Sprite GetSprite(int i){return sprites[i];}
    public Sprite GetRandomSprite(int i){return sprites[Random.Range(0,sprites.Count-1)];}
    public void AddSprite(Sprite spr){sprites.Add(spr);}
}