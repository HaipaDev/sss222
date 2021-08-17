using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShopItemID")]
public class ShopItemID : ScriptableObject{
    [SerializeField] public int ID;
    [SerializeField] public string name;
    [SerializeField] public string desc;
    [SerializeField] public Sprite img;
}