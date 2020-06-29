using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop Slot ID")]
public class ShopSlotID : ScriptableObject{
    [HeaderAttribute("Properties")]
    [SerializeField] public string itemName = "";
    [SerializeField] public string pwrupName = "";
    [SerializeField] public int ID;
    [SerializeField] public int price=-1;
    [SerializeField] public int priceS;
    [SerializeField] public int priceE;
    [SerializeField] public Sprite sprite;
    [SerializeField] public float scaleX=1;
    [SerializeField] public float scaleY=1;
    //[SerializeField] public float spawnRate = 10f;
}
