using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShopItemID")]
public class ShopItemID : ScriptableObject{
    [SerializeField] public int ID;
    [SerializeField] public string name;
    [SerializeField] public string desc;
    [SerializeField] public Sprite img;
    [SerializeField] public Vector2 priceR=new Vector2(5,15);
    [SerializeField] public int price;
    [SerializeField] public Vector2 limit=new Vector2(3,5);
}