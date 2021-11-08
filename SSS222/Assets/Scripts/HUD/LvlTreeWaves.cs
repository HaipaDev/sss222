using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

public class LvlTreeWaves : MonoBehaviour{
    [SerializeField] GameObject mainList;
    [SerializeField] GameObject listElement;
    [SerializeField] GameObject gridElement;
    [SerializeField] GameObject[] lists;
    [SerializeField] List<LootTableEntryWaves> waves;
    [SerializeField] List<int> levels;
    int minLevel=0;
    public float sum;
    void Start(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.15f);
        if(FindObjectOfType<LootTableWaves>()!=null){foreach(LootTableWaves lt in FindObjectsOfType<LootTableWaves>()){waves.AddRange(lt.itemList);}}
        else if(GameRules.instance!=null){waves.AddRange(GameRules.instance.waveList);}
        yield return new WaitForSecondsRealtime(0.015f);
        List<LootTableEntryWaves> removeList=new List<LootTableEntryWaves>();
        foreach(LootTableEntryWaves entry in waves){if(entry.dropChance<=0){removeList.Add(entry);}else{sum+=entry.dropChance;}}
        foreach(LootTableEntryWaves rem in removeList){if(waves.Contains(rem)){waves.Remove(rem);}}
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
            foreach(LootTableEntryWaves wave in waves){
                p++;
                if(levels[i]==wave.levelReq){
                    GameObject go2=Instantiate(gridElement,go.transform.GetChild(1));
                    go2.name=wave.name;
                    go2.transform.GetComponentInChildren<LvlTree_ElementDroprate>().drop=wave.dropChance;
                    go2.GetComponent<Image>().sprite=wave.lootItem.thumbnail;
                    go.GetComponent<LvlTree_ListElement>().elements.Add(go2);
                }
            }
            lists[i]=go;
        }
    }
    void SetLevels(){
        levels.Clear();
        for(var i=0; i<waves.Count; i++){
            int p=waves[i].levelReq;
            if(p>=minLevel&&!levels.Contains(p))levels.Add(p);
            levels.Sort();
        }
    }
}
