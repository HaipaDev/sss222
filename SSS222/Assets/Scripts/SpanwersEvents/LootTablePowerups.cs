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
    public float levelReq=0f;
    //[HideInInspector]public float dropChance=0f;
}
//[System.Serializable]
/*public class LootTableDropPowerup{
    [HideInInspector]public string name;
    [SerializeField]public float dropChance=0f;
}*/
[System.Serializable]
public class ItemPercentagePowerup{
    [HideInInspector]public string name;
    //[SerializeField]public float itemPercentage;
}
public class LootTablePowerups : MonoBehaviour{
    [SerializeField]public List<LootTableEntryPowerup> itemList;
    public int currentLvl;
    //public float[] dropSetList;
    //public int[] lvlList;

    public List<float> dropList;
    private Dictionary<PowerupItem, float> itemTable;
    [SerializeField] ItemPercentagePowerup[] itemsPercentage;
    //[HideInInspector] ItemPercentagePowerup[] itemsPercentage2;
    public float sum;
    GameSession gameSession;
    private void Awake(){
        /*itemTable = new Dictionary<LootItem, float>();
        foreach(LootTableEntry entry in itemList){
            itemTable.Add(entry.lootItem, entry.dropChance);
        }*/
        //foreach(float dropChance in itemTable.Values){sum+=dropChance;}
        StartCoroutine(SetValues());
    }
    private IEnumerator SetValues(){
        //Set values
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
    private void Start() {
        gameSession=FindObjectOfType<GameSession>();
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
        SumUp();
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
        foreach(LootTableEntryPowerup entry in itemList){
            i++;
            //foreach(float drop in dropList){
                if(randomWeight<dropList[i]) return entry.lootItem;
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
        if(dropList.Count<itemList.Count){
        //System.Array.Resize(ref dropList, itemList.Count);
        //if(dropList.Count>itemList.Count)dropList.Clear();
        dropList=new List<float>(itemList.Count);
        //foreach(float dropChance in itemTable.Values){sum+=dropChance;};
        //itemTable = new Dictionary<PowerupItem, float>();
        //itemsPercentage = new ItemPercentage[itemList.Count];
        var i=-1;
        System.Array.Resize(ref itemsPercentage, itemList.Count);
        //System.Array.Resize(ref dropSetList, itemList.Count);
        //System.Array.Resize(ref lvlList, itemList.Count);
        foreach(LootTableEntryPowerup entry in itemList){
            i++;
            //dropSetList[i]=entry.dropChance;
            //lvlList[i]=entry.levelReq;
            /*
            entry.name=entry.lootItem.name;
            entry.rarity=entry.lootItem.rarity;
            entry.dropChance=entry.lootItem.dropChance;
            entry.levelReq=entry.lootItem.levelReq;
            */
            //dropList[i]=entry.dropChance;
            dropList.Add(entry.dropChance);
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
                //if(entry.rarity==rarityPowerup.Common){r="c";}
                
                
                itemsPercentage[i].name=entry.name+"("+entry.levelReq+r+")"+" - "+value+"%"+" - "+dropList[i]+"/"+(sum-dropList[i]);
                //foreach(ItemPercentage item in itemsPercentage){item.name=entry.name;item.itemPercentage=value;}
            //}
            //}
        }
        sum=dropList.Sum();
        }
    }
    /*void SumUpAfter(){
        int i=0;
        foreach(PowerupItem entry in itemList){
            if(i>=0&&i<dropList.Count){
                if(currentLvl<entry.levelReq){
                    dropList[i]=0;
                }else{
                    if(entry.rarity==rarityPowerup.Common)dropList[i]=entry.dropChance;
                    else if(entry.rarity==rarityPowerup.Rare)
                    dropList[i]=
                    entry.dropChance*
                    Mathf.Clamp(GameSession.instance.rarePwrupMulti,0,5f);
                    else if(entry.rarity==rarityPowerup.Legendary)
                    dropList[i]=
                    entry.dropChance*
                    GameSession.instance.legendPwrupMulti;
                    //System.Array.Resize(ref itemsPercentage, itemList.Count);
                }
                //System.Array.Resize(ref itemsPercentage, itemList.Count);
                var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
                itemsPercentage[i].name=entry.name+" - "+value+"%"+" - "+dropList[i]+"/"+(sum-dropList[i]);
            }
            i++;
        }
        sum=dropList.Sum();
    }*/
    void SumUpAfter(){
        if(dropList.Count<itemList.Count){dropList.Capacity=itemList.Capacity;}
        var i=-1;
        foreach(LootTableEntryPowerup entry in itemList){
            i++;
            if(currentLvl<entry.levelReq){
                dropList[i]=0;
            }else{
                if(entry.rarity==rarityPowerup.Common)dropList[i]=entry.dropChance;
                else if(entry.rarity==rarityPowerup.Rare)dropList[i]=entry.dropChance*gameSession.rarePwrupMulti;
                else if(entry.rarity==rarityPowerup.Legendary)dropList[i]=entry.dropChance*gameSession.legendPwrupMulti;
                //System.Array.Resize(ref itemsPercentage, itemList.Count);
            }
            //System.Array.Resize(ref itemsPercentage, itemList.Count);
            var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
            itemsPercentage[i].name=entry.name+" - "+value+"%"+" - "+dropList[i]+"/"+(sum-dropList[i]);
        }
        sum=dropList.Sum();
    }
}
