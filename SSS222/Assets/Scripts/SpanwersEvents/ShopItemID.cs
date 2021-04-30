using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShopItemID")]
public class ShopItemID : ScriptableObject{
    [SerializeField] public int ID;
    [SerializeField] public string name;
    [SerializeField] public string desc;
    [SerializeField] public Sprite img;
    [SerializeField] public int price;
    [SerializeField] public int priceS=5;
    [SerializeField] public int priceE=15;
}
