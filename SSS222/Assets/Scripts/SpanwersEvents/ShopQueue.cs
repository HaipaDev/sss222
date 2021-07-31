using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class EntryShopQueue{
    public List<LootTableEntryShopQueue> itemList;
}[System.Serializable]
public class LootTableEntryShopQueue{
    [HideInInspector]public string name;
    public ShopItemID lootItem;
    public float dropChance=0f;
}
[System.Serializable]
public class ItemPercentageSlotsQueue{
    [SerializeReference]public string[] list;
}

[CreateAssetMenu(menuName = "ShopQueue")]
public class ShopQueue:ScriptableObject{
    [SerializeField] public List<EntryShopQueue> slotList;
    [SerializeField] Dictionary<ShopItemID, float>[] itemTable;
    [SerializeField] ItemPercentageSlotsQueue[] itemsPercentage;
    public float[] sum;
    
    void OnValidate(){SumUp();}
    public ShopItemID GetItem(int currentSlotID){
        float randomWeight = 0;
        do{//No weight on any number?
            if(sum[currentSlotID]==0)return null;
            randomWeight=Random.Range(0,sum[currentSlotID]);
        }while(randomWeight==sum[currentSlotID]);
        foreach (LootTableEntryShopQueue entry in slotList[currentSlotID].itemList){
            if(randomWeight<entry.dropChance) return entry.lootItem;
            randomWeight-=entry.dropChance;
        }
        return null;
    }
    [ContextMenu("SumUp")]void SumUp(){
        System.Array.Resize(ref itemsPercentage, slotList.Count);
        System.Array.Resize(ref itemTable, slotList.Count);
        System.Array.Resize(ref sum, slotList.Count);
        for(var it=0;it<itemTable.Length;it++){itemTable[it]=new Dictionary<ShopItemID, float>();}
        for(var p=0;p<itemsPercentage.Length;p++){System.Array.Resize(ref itemsPercentage[p].list, slotList[p].itemList.Count);}
        for(var q=0;q<slotList.Count;q++){
            foreach(LootTableEntryShopQueue entry in slotList[q].itemList){
                entry.name=entry.lootItem.name;
                itemTable[q].Add(entry.lootItem,entry.dropChance);
                sum[q]=itemTable[q].Values.Sum();   
            }
            var i=-1;
            foreach(LootTableEntryShopQueue entry in slotList[q].itemList){
                var value=System.Convert.ToSingle(System.Math.Round((entry.dropChance/sum[q]*100),2));
                i++;
                if(i>=0)itemsPercentage[q].list[i]=entry.name+" - "+value+"%"+" - "+entry.dropChance+"/"+(sum[q]-entry.dropChance);
            }
        }
        
    }
}
