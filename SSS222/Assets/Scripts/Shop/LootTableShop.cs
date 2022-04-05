using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

[System.Serializable]
public class LootTableEntryShop{
    [HideInInspector]public string name;
    public ShopQueue lootItem;
    public float dropChance=0f;
}
[System.Serializable]
public class ItemPercentageShop{
    [HideInInspector]public string name;
}
public class LootTableShop : MonoBehaviour{
    [SerializeField]
    public List<LootTableEntryShop> itemList;
    Dictionary<ShopQueue, float> itemTable;
    [ReadOnly][SerializeField] ItemPercentageShop[] itemsPercentage;
    [ReadOnly]public float sum;
    [ReadOnly]public ShopQueue currentQueue;
    
    void Awake(){SumUp();}
    void OnValidate(){SumUp();}
    public ShopQueue GetQueue(){
        float randomWeight = 0;
        do{
            //No weight on any number?
            if (sum == 0) return null;
            randomWeight = Random.Range(0, sum);
        } while (randomWeight == sum);
        foreach(LootTableEntryShop entry in itemList){
            if(randomWeight<entry.dropChance) return entry.lootItem;
            randomWeight-=entry.dropChance;
        }
        return null;
    }
    void SumUp(){
        itemTable = new Dictionary<ShopQueue, float>();
        System.Array.Resize(ref itemsPercentage, itemList.Count);
        var i=-1;
        foreach(LootTableEntryShop entry in itemList){
            entry.name=entry.lootItem.name;
            itemTable.Add(entry.lootItem, (float)entry.dropChance);
            var value=System.Convert.ToSingle(System.Math.Round((entry.dropChance/sum*100),2));
                i++;
                itemsPercentage[i].name=entry.name+" - "+value+"%"+" - "+entry.dropChance+"/"+(sum-entry.dropChance);
        }
        sum=itemTable.Values.Sum();
    }
}
