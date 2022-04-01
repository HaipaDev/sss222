using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

public class LvlTreePowerups : MonoBehaviour{
    [SerializeField] GameObject mainList;
    [SerializeField] GameObject listElement;
    [SerializeField] GameObject gridElement;
    [SerializeField] GameObject[] lists;
    [SerializeField] List<LootTableEntryPowerup> powerups;
    [SerializeField] List<int> levels;
    int minLevel=0;
    public float sum;
    void Start(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.15f);
        if(FindObjectOfType<LootTablePowerups>()!=null)foreach(LootTablePowerups lt in FindObjectsOfType<LootTablePowerups>()){powerups.AddRange(lt.itemList);}
        else if(GameRules.instance!=null){
            foreach(PowerupsSpawnerGR ps in GameRules.instance.powerupSpawners){
                powerups.AddRange(ps.powerupList);
            }
        }
        yield return new WaitForSecondsRealtime(0.015f);
        List<LootTableEntryPowerup> removeList=new List<LootTableEntryPowerup>();
        foreach(LootTableEntryPowerup entry in powerups){if(entry.dropChance<=0){removeList.Add(entry);}else{sum+=entry.dropChance;}}
        foreach(LootTableEntryPowerup rem in removeList){if(powerups.Contains(rem)){powerups.Remove(rem);}}
        yield return new WaitForSecondsRealtime(0.015f);
        SetLevels();
        yield return new WaitForSecondsRealtime(0.015f);
        for(var i=0; i<levels.Count; i++){
            Array.Resize(ref lists,levels.Count);
            GameObject go;
            if(lists[i]==null)go=Instantiate(listElement,mainList.transform);
            else{go=lists[i];}
            go.GetComponent<LvlTree_ListElement>().level=levels[i];
            go.name="Lvl"+levels[i].ToString();
            var p=-1;
            foreach(LootTableEntryPowerup powerup in powerups){
                p++;
                if(levels[i]==powerup.levelReq){
                    GameObject go2=Instantiate(gridElement,go.transform.GetChild(1));
                    go2.name=powerup.name;
                    go2.transform.GetComponentInChildren<LvlTree_ElementDroprate>().drop=powerup.dropChance;
                    go2.GetComponent<Image>().sprite=GameAssets.instance.Get(powerup.lootItem.assetName).GetComponent<SpriteRenderer>().sprite;
                    go.GetComponent<LvlTree_ListElement>().elements.Add(go2);
                }
            }
            lists[i]=go;
        }
    }
    void SetLevels(){
        levels.Clear();
        for(var i=0; i<powerups.Count; i++){
            int p=powerups[i].levelReq;
            if(p>=minLevel&&!levels.Contains(p))levels.Add(p);
            levels.Sort();
        }
    }
}
