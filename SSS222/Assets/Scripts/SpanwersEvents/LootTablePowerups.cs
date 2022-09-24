using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class LootTableEntryPowerup{
    [HideInInspector]public string name;
    public PowerupItem lootItem;
    public rarityPowerup rarity;
    public float dropChance=0f;
    public int levelReq=0;
}
public class LootTablePowerups : MonoBehaviour{
    [Searchable][SerializeField]public List<LootTableEntryPowerup> itemList;
    [DisableInPlayMode][SerializeField]public int currentLvl;

    [ReadOnly]public List<float> dropList;
    private Dictionary<PowerupItem, float> itemTable;
    [ReadOnly][SerializeField] ItemPercentageLable[] itemsPercentage;
    [ReadOnly]public float sum;


    void OnValidate(){
        SumUp();
        //SumUpAfter();
    }
    void Update(){
        if(Player.instance!=null)if(GameRules.instance.modulesOn)currentLvl=Player.instance.GetComponent<PlayerModules>().shipLvl;
        SumUp();
        SumUpAfter();
    }
    public PowerupItem GetItem(){
        float randomWeight=0;
        do{
            if(sum==0)return null;
            randomWeight=Random.Range(0,sum);
        }while(randomWeight==sum);
        var i=-1;
        foreach(LootTableEntryPowerup entry in itemList){
            i++;
            if(randomWeight<dropList[i])return entry.lootItem;
            randomWeight-=dropList[i];
        }
        return null;
    }
    public void SumUp(){
        if(dropList.Count<itemList.Count){
        dropList=new List<float>(itemList.Count);
        var i=-1;
        System.Array.Resize(ref itemsPercentage, itemList.Count);
        foreach(LootTableEntryPowerup entry in itemList){
            i++;
            dropList.Add(entry.dropChance);
            
            var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
                string r="";
                if(entry.rarity==rarityPowerup.Legendary){}r="|L";
                if(entry.rarity==rarityPowerup.Rare){r="|R";}
                
                if(entry!=null&&itemsPercentage!=null&&itemsPercentage[i]!=null)itemsPercentage[i].name=entry.name+"("+entry.levelReq+r+")"+" - "+value+"%"+" - "+dropList[i]+"/"+(sum-dropList[i]);
        }
        sum=dropList.Sum();
        }
    }
    void SumUpAfter(){
        if(dropList.Count<itemList.Count){dropList.Capacity=itemList.Capacity;}
        var i=-1;
        foreach(LootTableEntryPowerup entry in itemList){
            i++;
            if(entry.rarity==rarityPowerup.Common)dropList[i]=entry.dropChance;
            else if(entry.rarity==rarityPowerup.Rare)dropList[i]=entry.dropChance*GameManager.instance.rarePwrupMulti;
            else if(entry.rarity==rarityPowerup.Legendary)dropList[i]=entry.dropChance*GameManager.instance.legendPwrupMulti;
            if(!GameRules.instance.levelingOn){entry.levelReq=0;}
            if(currentLvl<entry.levelReq&&GameRules.instance.levelingOn)dropList[i]=0;
            
            var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
            if(entry!=null&&itemsPercentage!=null&&itemsPercentage[i]!=null)itemsPercentage[i].name=entry.name+" - "+value+"%"+" - "+dropList[i]+"/"+(sum-dropList[i]);
        }
        sum=dropList.Sum();
    }
}
