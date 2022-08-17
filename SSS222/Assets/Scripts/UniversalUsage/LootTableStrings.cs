using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTableEntryString{
    public string lootItem;
    public float dropChance=0f;

}
public class LootTableString : MonoBehaviour{
    [SerializeField]
    public List<LootTableEntryString> itemList;
    private Dictionary<string, float> itemTable;
    [HideInInspector] ItemPercentageLable[] itemsPercentage;
    public float sum;
    public bool restart;
    
    void Awake(){SumUp();}
    void OnValidate(){
        SumUp();
        if(restart){
            foreach(LootTableEntryString entry in itemList){
                entry.dropChance=0;
            }
            restart=false;
        }
    }
    public string GetItem(){
        float randomWeight = 0;
        do
        {
            //No weight on any number?
            if (sum == 0) return null;
            randomWeight = Random.Range(0, sum);
        } while (randomWeight == sum);
        foreach(LootTableEntryString entry in itemList){
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
        itemTable = new Dictionary<string, float>();
        //itemsPercentage = new ItemPercentage[itemList.Count];
        var i=-1;
        foreach(LootTableEntryString entry in itemList){
            itemTable.Add(entry.lootItem, (float)entry.dropChance);
            var value=System.Convert.ToSingle(System.Math.Round((entry.dropChance/sum*100),2));
            //itemsPercentage.Add(value);
            //for(var i=0; i<itemTable.Count; i++){
                i++;
                //itemsPercentage.Join();
                //ItemPercentage itemsPercentage= new ItemPercentage();
                //itemsPercentage[i].itemPercentage=value;
                if(i>=0&&i<itemsPercentage.Length)itemsPercentage[i].name=entry.lootItem+" - "+value+"%"+" - "+entry.dropChance+"/"+(sum-entry.dropChance);
                //foreach(ItemPercentage item in itemsPercentage){item.name=entry.name;item.itemPercentage=value;}
            //}
        }
        sum=itemTable.Values.Sum();
        System.Array.Resize(ref itemsPercentage, itemList.Count);
    }
}
