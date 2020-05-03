using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop Slot ID")]
public class ShopSlotID : ScriptableObject{
    [HeaderAttribute("Properties")]
    [SerializeField] public string itemName = "";
    [SerializeField] public string pwrupName = "";
    [SerializeField] public int ID;
    [SerializeField] public int price;
    [SerializeField] public Sprite sprite;
    //[SerializeField] public float spawnRate = 10f;
}
