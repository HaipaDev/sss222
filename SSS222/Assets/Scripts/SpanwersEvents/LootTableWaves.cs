using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTableEntryWaves{
    [HideInInspector]public string name;
    public WaveConfig lootItem;
    public float dropChance=0f;

}
[System.Serializable]
public class ItemPercentageWaves{
    [HideInInspector]public string name;
    //[SerializeField]public float itemPercentage;
}
public class LootTableWaves : MonoBehaviour{
    [SerializeField]
    public List<LootTableEntryWaves> itemList;
    private Dictionary<WaveConfig, float> itemTable;
    [SerializeField] ItemPercentageWaves[] itemsPercentage;
    [HideInInspector] ItemPercentageWaves[] itemsPercentage2;
    public float sum;
    
    private void Awake(){
        /*itemTable = new Dictionary<LootItem, float>();
        foreach(LootTableEntry entry in itemList){
            itemTable.Add(entry.lootItem, entry.dropChance);
        }*/
        //foreach(float dropChance in itemTable.Values){sum+=dropChance;}
        SumUp();
    }
    void OnValidate(){
        /*itemTable = new Dictionary<LootItem, float>();
        foreach(LootTableEntry entry in itemList){
            itemTable.Add(entry.lootItem, entry.dropChance);
        }*/
        SumUp();
    }
    public WaveConfig GetItem(){
        float randomWeight = 0;
        do
        {
            //No weight on any number?
            if (sum == 0) return null;
            randomWeight = Random.Range(0, sum);
        } while (randomWeight == sum);
        foreach(LootTableEntryWaves entry in itemList){
            if(randomWeight<entry.dropChance) return entry.lootItem;
            randomWeight-=entry.dropChance;
        }
        /*foreach (LootItem item in items)
        {
            if (randomWeight < item.GetComponent<LootItem>().spawnRate) return item;
            randomWeight -= item.GetComponent<LootItem>().spawnRate;
        }*/
        return null;
    }
    void SumUp(){
        //foreach(float dropChance in itemTable.Values){sum+=dropChance;};
        itemTable = new Dictionary<WaveConfig, float>();
        //itemsPercentage = new ItemPercentage[itemList.Count];
        var i=-1;
        foreach(LootTableEntryWaves entry in itemList){
            i++;
            entry.name=entry.lootItem.name;
            itemTable.Add(entry.lootItem, (float)entry.dropChance);
            var value=System.Convert.ToSingle(System.Math.Round((entry.dropChance/sum*100),2));
            //itemsPercentage.Add(value);
            //for(var i=0; i<itemTable.Count; i++){
                //i++;
                //itemsPercentage.Join();
                //ItemPercentage itemsPercentage= new ItemPercentage();
                //itemsPercentage[i].itemPercentage=value;
                if(i>=0&&i<itemsPercentage.Length)itemsPercentage[i].name=entry.name+" - "+value+"%"+" - "+entry.dropChance+"/"+(sum-entry.dropChance);
                //foreach(ItemPercentage item in itemsPercentage){item.name=entry.name;item.itemPercentage=value;}
            //}
        }
        sum=itemTable.Values.Sum();
        System.Array.Resize(ref itemsPercentage, itemList.Count);
    }
}
