using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
/*public class LootTableEntryPowerup{
    [HideInInspector]public string name;
    [SerializeField]public PowerupItem lootItem;
    //[HideInInspector]public float dropChance=0f;
}
//[System.Serializable]
public class LootTableDropPowerup{
    [HideInInspector]public string name;
    [SerializeField]public float dropChance=0f;
}*/
[System.Serializable]
public class ItemPercentagePowerup{
    [HideInInspector]public string name;
    //[SerializeField]public float itemPercentage;
}
public class LootTablePowerups : MonoBehaviour{
    [SerializeField]
    public List<PowerupItem> itemList;
    public int currentLvl;
    public List<float> dropList;
    private Dictionary<PowerupItem, float> itemTable;
    [SerializeField] ItemPercentagePowerup[] itemsPercentage;
    //[HideInInspector] ItemPercentagePowerup[] itemsPercentage2;
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
        SumUpAfter();
    }
    void Update(){
        currentLvl=UpgradeMenu.instance.total_UpgradesLvl;
        SumUpAfter();
    }
    public PowerupItem GetItem(){
        float randomWeight = 0;
        do
        {
            //No weight on any number?
            if (sum == 0) return null;
            randomWeight = Random.Range(0, sum);
        } while (randomWeight == sum);
        var i=-1;
        foreach(PowerupItem entry in itemList){
            i++;
            //foreach(float drop in dropList){
                if(randomWeight<dropList[i]) return entry;
                randomWeight-=dropList[i];
            //}
        }
        /*foreach (LootItem item in items)
        {
            if (randomWeight < item.GetComponent<LootItem>().spawnRate) return item;
            randomWeight -= item.GetComponent<LootItem>().spawnRate;
        }*/
        return null;
    }
    void SumUp(){
        //System.Array.Resize(ref dropList, itemList.Count);
        dropList.Clear();
        //foreach(float dropChance in itemTable.Values){sum+=dropChance;};
        //itemTable = new Dictionary<PowerupItem, float>();
        //itemsPercentage = new ItemPercentage[itemList.Count];
        var i=-1;
        System.Array.Resize(ref itemsPercentage, itemList.Count);
        foreach(PowerupItem entry in itemList){
            i++;
            dropList.Add(entry.dropChance);
            //dropList.Add(entry.dropChance);
            //foreach(float drop in dropList){
            //itemTable.Add(entry, (float)dropList[i]);
            //entry.name=entry.lootItem.name;
            
            var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
            //itemsPercentage.Add(value);
            //for(var i=0; i<itemTable.Count; i++){
                
                //itemsPercentage.Join();
                //ItemPercentage itemsPercentage= new ItemPercentage();
                //itemsPercentage[i].itemPercentage=value;
                string r="";
                if(entry.rarity==rarityPowerup.Legendary){}r="|L";
                if(entry.rarity==rarityPowerup.Rare){r="|R";}
                if(entry.rarity==rarityPowerup.Common){r="c";}
                
                
                itemsPercentage[i].name=entry.name+"("+entry.levelReq+r+")"+" - "+value+"%"+" - "+dropList[i]+"/"+(sum-dropList[i]);
                //foreach(ItemPercentage item in itemsPercentage){item.name=entry.name;item.itemPercentage=value;}
            //}
            //}
        }
        sum=dropList.Sum();
    }
    void SumUpAfter(){
        var i=-1;
        foreach(PowerupItem entry in itemList){
            i++;
            if(currentLvl<entry.levelReq){
                dropList[i]=0;
            }else{
                dropList[i]=entry.dropChance;
                //System.Array.Resize(ref itemsPercentage, itemList.Count);
            }
            //System.Array.Resize(ref itemsPercentage, itemList.Count);
            var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
            itemsPercentage[i].name=entry.name+" - "+value+"%"+" - "+dropList[i]+"/"+(sum-dropList[i]);
        }
        sum=dropList.Sum();
    }
}
