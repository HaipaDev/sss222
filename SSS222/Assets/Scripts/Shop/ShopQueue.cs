using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class EntryShopQueue{
    public int slotUnlock=4;
    public List<LootTableEntryShopQueue> itemList;
}[System.Serializable]
public class LootTableEntryShopQueue{
    [HideInInspector]public string name;
    public ShopItemID lootItem;
    public float dropChance=0f;
    public Vector2 price=new Vector2(5,15);
    public Vector2 limit=new Vector2(3,5);
    public Vector2 rep=new Vector2(1,6);
}
[System.Serializable]
public class ItemPercentageSlotsQueue{
    [SerializeReference]public string[] list;
}

[CreateAssetMenu(menuName = "ShopQueue")]
public class ShopQueue:ScriptableObject{
    [SerializeField] public int preUnlocked=1;
    [SerializeField] public int slotsWhenLimit=3;
    [SerializeField] public List<EntryShopQueue> slotList;
    [SerializeField] Dictionary<ShopItemID, float>[] itemTable;
    [SerializeField] ItemPercentageSlotsQueue[] itemsPercentage;
    public float[] sum;
    
    void OnValidate(){SumUp();}
    
    public LootTableEntryShopQueue GetEntry(int currentSlotID){
        float randomWeight = 0;
        do{//No weight on any number?
            if(sum[currentSlotID]==0)return null;
            randomWeight=Random.Range(0,sum[currentSlotID]);
        }while(randomWeight==sum[currentSlotID]);
        foreach(LootTableEntryShopQueue entry in slotList[currentSlotID].itemList){
            if(randomWeight<entry.dropChance) return entry;
            randomWeight-=entry.dropChance;
        }
        return null;
    }
    public EntryShopQueue GetSlot(int currentSlotID){return slotList[currentSlotID];}
    public ShopItemID GetItem(int currentSlotID){return GetEntry(currentSlotID).lootItem;}
    public int GetPrice(int currentSlotID){return (int)Random.Range(GetEntry(currentSlotID).price.x,GetEntry(currentSlotID).price.y);}
    public int GetLimit(int currentSlotID){return (int)Random.Range(GetEntry(currentSlotID).limit.x,GetEntry(currentSlotID).limit.y);}
    public int GetRep(int currentSlotID){return (int)Random.Range(GetEntry(currentSlotID).rep.x,GetEntry(currentSlotID).rep.y);}
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
