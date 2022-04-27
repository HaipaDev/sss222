using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class ModeChooseCanvas : MonoBehaviour{
    [SceneObjectsOnly][SerializeField] Transform container;
    [AssetsOnly][SerializeField] GameObject elementPrefab;
    void Start(){
        foreach(GameRules gr in GameCreator.instance.gamerulesetsPrefabs){
            string name=gr.cfgName;     if(name.Contains(" Mode"))name=name.Replace(" Mode","");
            if(!gr.cfgName.Contains("Arcade Mode")){
                GameObject go=Instantiate(elementPrefab,container);
                go.name=name+"-ModeButton";
                go.GetComponent<Button>().onClick.AddListener(()=>OpenGamemode(gr.cfgName));
                go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=name;
                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text=gr.cfgDesc;
                if(go.transform.GetChild(1).GetChild(0)!=null){Destroy(go.transform.GetChild(1).GetChild(0).gameObject);}
                if(gr.cfgIconsGo!=null){Instantiate(gr.cfgIconsGo,go.transform.GetChild(1));}
                else{go.transform.GetChild(1).gameObject.AddComponent<Image>().sprite=GameAssets.instance.Spr(gr.cfgIconAssetName);}
            }
        }
    }
    public void OpenGamemode(string str){GSceneManager.instance.LoadGameModeInfoSceneSetStr(str);}
    public void OpenAdventure(){GSceneManager.instance.LoadAdventureZonesScene();}
    public void OpenSandbox(){GSceneManager.instance.LoadSandboxModeScene();}
}
