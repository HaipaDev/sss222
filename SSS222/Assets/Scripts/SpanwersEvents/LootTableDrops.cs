using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTableEntryDrops{
    public string name;
    //public string lootItem;
    public Vector2 ammount=new Vector2(1,1);
    public float dropChance=10f;
}
public class LootTableDrops : MonoBehaviour{
    [SerializeField]public List<LootTableEntryDrops> itemList;/*=new List<LootTableEntryDrops>(){
        {new LootTableEntryDrops(){lootItem=GameAssets.instance.Get("EnBall"),ammount=new Vector2(1,1),dropChance=10}},
        {new LootTableEntryDrops(){lootItem=GameAssets.instance.Get("Coin"),ammount=new Vector2(1,1),dropChance=10}},
        {new LootTableEntryDrops(){lootItem=GameAssets.instance.Get("Core"),ammount=new Vector2(1,1),dropChance=10}}
    };*/

    public List<float> dropList;
    private void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        yield return new WaitForSeconds(0.04f);
        if(itemList.Count==0){Destroy(this);}
        SumUp();
    }
    /*IEnumerator SetValues(){
        yield return new WaitForSeconds(0.1f);
        var i=GameRules.instance;
        if(i!=null){
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f,0.1f));
            if(GetComponent<Enemy>()!=null){itemList=Array.Find(i.enemies,e => e.name.Contains(GetComponent<Enemy>().name)).drops;SumUp();}
            if(itemList.Count==0){//After Enemy
                if(GetComponent<CometRandomProperties>()!=null){if(i.cometSettings.drops.Count!=0)itemList=i.cometSettings.drops;SumUp();}
            }
        }
        if(itemList.Count==0){Destroy(this);}
        yield return new WaitForSeconds(0.02f);
        SumUp();
    }*/
    void OnValidate(){SumUp();}
    void SumUp(){
        dropList.Clear();
        foreach(LootTableEntryDrops entry in itemList){
            dropList.Add(entry.dropChance);
            //entry.name=entry.lootItem;
        }
    }
}
