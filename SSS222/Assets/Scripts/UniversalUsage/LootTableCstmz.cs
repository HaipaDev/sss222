using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTableEntryCstmz{
    public string lootItem;
    public CstmzType cstmzType=CstmzType.skin;
    public float dropChance=0f;

}
public class LootTableCstmz : MonoBehaviour{
    [SerializeField]
    public List<LootTableEntryCstmz> itemList;
    private Dictionary<string, float> itemTable;
    private List<float> dropList;
    [SerializeField] ItemPercentageLable[] itemsPercentage;
    public float sum;
    public bool restart;
    
    void Awake(){itemList=new List<LootTableEntryCstmz>();dropList=new List<float>();}
    void OnValidate(){
        //SumUp();
        if(restart){
            foreach(LootTableEntryCstmz entry in itemList){
                entry.dropChance=0;
            }
            restart=false;
        }
    }
    public LootTableEntryCstmz GetItem(){
        float randomWeight = 0;
        do{
            //No weight on any number?
            if (sum == 0) return null;
            randomWeight = Random.Range(0, sum);
        } while (randomWeight == sum);
        foreach(LootTableEntryCstmz entry in itemList){
            if(randomWeight<entry.dropChance) return entry;
            randomWeight-=entry.dropChance;
        }
        return null;
    }
    public void SumUp(){
        if(dropList.Count<itemList.Count){
            dropList=new List<float>(itemList.Count);
            itemTable=new Dictionary<string,float>();
            var i=-1;
            System.Array.Resize(ref itemsPercentage, itemList.Count);
            foreach(LootTableEntryCstmz entry in itemList){
                i++;
                dropList.Add(entry.dropChance);
                
                itemTable.Add(
                    entry.lootItem+" | "+entry.cstmzType,
                (float)dropList[i]);
                var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
                    if(entry!=null&&itemsPercentage!=null&&itemsPercentage[i]!=null){
                        itemsPercentage[i].name=
                        entry.lootItem
                        +" - "+value+"%"+" - "+
                        dropList[i]+"/"+
                        (sum-dropList[i]);
                    }
            }
            sum=dropList.Sum();
            System.Array.Resize(ref itemsPercentage, itemList.Count);
        }
    }
    /*void SumUpAfter(){
        if(dropList.Count<itemList.Count){dropList.Capacity=itemList.Capacity;}
        var i=-1;
        foreach(LootTableEntryCstmz entry in itemList){
            i++;
            
            var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
                if(entry!=null&&itemsPercentage!=null&&itemsPercentage[i]!=null){
                    itemsPercentage[i].name=
                    entry.lootItem
                    +" - "+value+"%"+" - "+
                    dropList[i]+"/"+
                    (sum-dropList[i]);
                }
        }
        sum=dropList.Sum();
    }*/
}
