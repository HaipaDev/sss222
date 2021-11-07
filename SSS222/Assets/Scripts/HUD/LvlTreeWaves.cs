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
    //int maxHSlots=13;
    int minLevel=0;
    void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.15f);
        if(FindObjectOfType<LootTableWaves>()!=null){foreach(LootTableWaves lt in FindObjectsOfType<LootTableWaves>()){waves.AddRange(lt.itemList);}}
        else if(GameRules.instance!=null){waves.AddRange(GameRules.instance.waveList);}
        yield return new WaitForSecondsRealtime(0.015f);
        List<LootTableEntryWaves> removeList=new List<LootTableEntryWaves>();
        foreach(LootTableEntryWaves entry in waves){if(entry.dropChance<=0){removeList.Add(entry);}}
        foreach(LootTableEntryWaves rem in removeList){if(waves.Contains(rem)){waves.Remove(rem);}}
        yield return new WaitForSecondsRealtime(0.015f);
        SetLevels();
        yield return new WaitForSecondsRealtime(0.015f);
        for(var i=0; i<levels.Count; i++){
            Array.Resize(ref lists,levels.Count);
            GameObject go;
            if(lists[i]==null)go=Instantiate(listElement,mainList.transform);
            else{go=lists[i];}
            go.GetComponent<LvlTreeList>().level=levels[i];
            go.name="Lvl"+levels[i].ToString();
            var p=-1;
            foreach(LootTableEntryWaves wave in waves){
                p++;
                if(levels[i]==wave.levelReq){
                    //for(var a=0;a<20;a++){//test make 20x more
                    GameObject go2=Instantiate(gridElement,go.transform.GetChild(0));
                    go2.name=wave.name;
                    go2.GetComponent<Image>().sprite=wave.lootItem.thumbnail;
                    go.GetComponent<LvlTreeList>().elements.Add(go2);
                    //}
                    #region LvlTreeList AutoScale
                    /*
                    //}
                    //if(go.GetComponent<LvlTreeList>().elements.Count>maxHSlots){
                    //for(var lp=0;lp<4;lp++){
                    //for(var ll=0;ll<maxHSlots;ll++){
                        var l=1;//20>13 l=1+1=2 // 40>13 l=3+1
                        if(go.GetComponent<LvlTreeList>().elements.Count>maxHSlots){l=(int)System.Math.Truncate((decimal)go.GetComponent<LvlTreeList>().elements.Count/maxHSlots)+1;}
                        RectTransform rt = go.GetComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y*l);
                    //}
                    //}
                    //}
                    */
                    #endregion
                }
            }
            lists[i]=go;
        }
    }
    void OnValidate() {
        //waves=AssetDatabase.FindAssets("t:PowerupItem");
        //waves=Resources.FindObjectsOfTypeAll(typeof(PowerupItem));
        //Array.Resize(ref wavesSpr,waves.Count);
        //Array.Resize(ref wavesSpr,waves.Length);
        //SetLevels();
        //string paths=AssetDatabase.GUIDToAssetPath(waves[0]);
    }
    void SetLevels(){
        levels.Clear();
        for(var i=0; i<waves.Count; i++){
            int p=waves[i].levelReq;
            //wavesSpr[i]=(GameObject)waves[i].GetType().GetField("item").GetValue(waves[i]);
            //var p=(int)waves[i].GetType().GetField("levelReq").GetValue(waves[i]);
            if(p>=minLevel&&!levels.Contains(p))levels.Add(p);
            levels.Sort();
        }
    }
}
