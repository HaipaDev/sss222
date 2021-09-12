using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTableEntryPowerup{
    [HideInInspector]public string name;
    public PowerupItem lootItem;
    public rarityPowerup rarity;
    public float dropChance=0f;
    public int levelReq=0;
}
[System.Serializable]
public class ItemPercentagePowerup{
    [HideInInspector]public string name;
}
public class LootTablePowerups : MonoBehaviour{
    [SerializeField]public List<LootTableEntryPowerup> itemList;
    public int currentLvl;

    public List<float> dropList;
    private Dictionary<PowerupItem, float> itemTable;
    [SerializeField] ItemPercentagePowerup[] itemsPercentage;
    public float sum;
    private void Awake(){StartCoroutine(SetValues());}
    private IEnumerator SetValues(){
        yield return new WaitForSeconds(0.1f);
        var i=GameRules.instance;
        if(i!=null){
            var ps=GetComponent<PowerupsSpawner>();
            if(ps.spawnerType==spawnerType.powerupStatus){
                itemList=i.pwrupStatusList;
                ps.mTimePowerupSpawns=i.mTimePowerupStatusSpawns;
                ps.mTimePowerupSpawnsS=i.mTimePowerupStatusSpawnsS;
                ps.mTimePowerupSpawnsE=i.mTimePowerupStatusSpawnsE;
                ps.firstSpawn=i.firstPowerupStatusSpawn;
                ps.enemiesCountReq=i.enemiesPowerupStatusCountReq;
            }
            if(ps.spawnerType==spawnerType.powerupWeapon){
                itemList=i.pwrupWeaponList;
                ps.mTimePowerupSpawns=i.mTimePowerupWeaponsSpawns;
                ps.mTimePowerupSpawnsS=i.mTimePowerupWeaponsSpawnsS;
                ps.mTimePowerupSpawnsE=i.mTimePowerupWeaponsSpawnsE;
                ps.firstSpawn=i.firstPowerupWeaponsSpawn;
                ps.enemiesCountReq=i.enemiesPowerupWeaponsCountReq;
            }
        }
        if(itemList.Count==0){Destroy(this);}
        yield return new WaitForSeconds(0.2f);
        SumUp();
    }
    void OnValidate(){
        SumUp();
        //SumUpAfter();
    }
    void Update(){
        if(UpgradeMenu.instance!=null)currentLvl=UpgradeMenu.instance.total_UpgradesLvl;
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
    void SumUp(){
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
            else if(entry.rarity==rarityPowerup.Rare)dropList[i]=entry.dropChance*GameSession.instance.rarePwrupMulti;
            else if(entry.rarity==rarityPowerup.Legendary)dropList[i]=entry.dropChance*GameSession.instance.legendPwrupMulti;
            if(currentLvl<entry.levelReq&&GameRules.instance.upgradesOn)dropList[i]=0;
            
            var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
            if(entry!=null&&itemsPercentage!=null&&itemsPercentage[i]!=null)itemsPercentage[i].name=entry.name+" - "+value+"%"+" - "+dropList[i]+"/"+(sum-dropList[i]);
        }
        sum=dropList.Sum();
    }
}
