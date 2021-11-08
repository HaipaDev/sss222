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
    //int maxHSlots=13;
    int minLevel=0;
    public float sum;
    void Start(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        yield return new WaitForSecondsRealtime(0.15f);
        if(FindObjectOfType<LootTablePowerups>()!=null)foreach(LootTablePowerups lt in FindObjectsOfType<LootTablePowerups>()){powerups.AddRange(lt.itemList);}
        else if(GameRules.instance!=null){powerups.AddRange(GameRules.instance.pwrupStatusList);powerups.AddRange(GameRules.instance.pwrupWeaponList);}
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
                    //for(var a=0;a<20;a++){//test make 20x more
                    GameObject go2=Instantiate(gridElement,go.transform.GetChild(1));
                    go2.name=powerup.name;
                    go2.transform.GetComponentInChildren<LvlTree_ElementDroprate>().drop=powerup.dropChance;
                    go2.GetComponent<Image>().sprite=powerup.lootItem.item.GetComponent<SpriteRenderer>().sprite;
                    go.GetComponent<LvlTree_ListElement>().elements.Add(go2);
                    //}
                    #region LvlTree_ListElement AutoScale
                    /*
                    //}
                    //if(go.GetComponent<LvlTree_ListElement>().elements.Count>maxHSlots){
                    //for(var lp=0;lp<4;lp++){
                    //for(var ll=0;ll<maxHSlots;ll++){
                        var l=1;//20>13 l=1+1=2 // 40>13 l=3+1
                        if(go.GetComponent<LvlTree_ListElement>().elements.Count>maxHSlots){l=(int)System.Math.Truncate((decimal)go.GetComponent<LvlTree_ListElement>().elements.Count/maxHSlots)+1;}
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
        //powerups=AssetDatabase.FindAssets("t:PowerupItem");
        //powerups=Resources.FindObjectsOfTypeAll(typeof(PowerupItem));
        //Array.Resize(ref powerupsItems,powerups.Count);
        //Array.Resize(ref powerupsItems,powerups.Length);
        //SetLevels();
        //string paths=AssetDatabase.GUIDToAssetPath(powerups[0]);
    }
    void SetLevels(){
        levels.Clear();
        for(var i=0; i<powerups.Count; i++){
            int p=powerups[i].levelReq;
            //powerupsItems[i]=(GameObject)powerups[i].GetType().GetField("item").GetValue(powerups[i]);
            //var p=(int)powerups[i].GetType().GetField("levelReq").GetValue(powerups[i]);
            if(p>=minLevel&&!levels.Contains(p))levels.Add(p);
            levels.Sort();
        }
    }
}
