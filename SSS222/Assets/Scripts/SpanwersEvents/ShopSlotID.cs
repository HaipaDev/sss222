using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop Slot ID")]
public class ShopSlotID : ScriptableObject{
    [HeaderAttribute("Properties")]
    [SerializeField] public string itemName = "";
    [SerializeField] public string pwrupName = "";
    [SerializeField] public int ID;
    [SerializeField] public float[] dropChance=new float[LootTableShop2.repLength+1];
    [SerializeField] public int price=-1;
    [SerializeField] public int priceS;
    [SerializeField] public int priceE;
    [SerializeField] public Sprite sprite;
    [SerializeField] public float scaleX=1;
    [SerializeField] public float scaleY=1;
    //[SerializeField] public float spawnRate = 10f;

    private void OnValidate() {
        Array.Resize(ref dropChance,LootTableShop2.repLength+1);
        /*float dr=0f;
        foreach(float drop in dropChance){
            if(drop!=0)dr=drop;
        }
        for(var i=0;i<dropChance.Length;i++){
            if(dropChance[i]==0)dropChance[i]=dr;
        }*/
    }
}
