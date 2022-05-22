using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShopItemID")]
public class ShopItemID : ScriptableObject{
    //[SerializeField] public int ID;
    [SerializeField] new public string name;
    [SerializeField] public string desc;
    [SerializeField] public Sprite img;
    [SerializeField] public string assetName;
}