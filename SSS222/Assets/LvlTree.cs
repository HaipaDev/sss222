using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

public class LvlTree : MonoBehaviour{
    [SerializeField] GameObject mainList;
    [SerializeField] GameObject listElement;
    [SerializeField] GameObject gridElement;
    [SerializeField] GameObject[] lists;
    [SerializeField] UnityEngine.Object[] powerups;
    [SerializeField] GameObject[] powerupsItems;
    [SerializeField] List<int> levels;
    void Awake(){
        for(var i=0; i<levels.Count; i++){
            Array.Resize(ref lists,levels.Count);
            GameObject go;
            if(lists[i]==null)go=Instantiate(listElement,mainList.transform);
            else{go=lists[i];}
            go.GetComponentInChildren<LvlTreeList>().level=levels[i];
            go.name="Lvl"+levels[i].ToString();
            var p=-1;
            foreach(PowerupItem powerup in powerups){
                p++;
                if(levels[i]==powerup.levelReq){
                    GameObject go2=Instantiate(gridElement,go.transform.GetChild(0));
                    go2.name=powerup.name;
                    go2.GetComponent<Image>().sprite=powerupsItems[p].GetComponent<SpriteRenderer>().sprite;
                    go.GetComponent<LvlTreeList>().elements.Add(go2);
                }
            }
            lists[i]=go;
        }
    }

    void Update(){
        
    }
    void OnValidate() {
        //powerups=AssetDatabase.FindAssets("t:PowerupItem");
        powerups=Resources.FindObjectsOfTypeAll(typeof(PowerupItem));
        Array.Resize(ref powerupsItems,powerups.Length);
        //Array.Resize(ref powerupsItems,powerups.Length);
        levels.Clear();
        for(var i=0; i<powerups.Length; i++){
            powerupsItems[i]=(GameObject)powerups[i].GetType().GetField("item").GetValue(powerups[i]);
            var p=(int)powerups[i].GetType().GetField("levelReq").GetValue(powerups[i]);
            if(p!=0&&!levels.Contains(p))levels.Add(p);
            levels.Sort();
        }
        //string paths=AssetDatabase.GUIDToAssetPath(powerups[0]);
    }
}
