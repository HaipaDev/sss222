using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class AdventureZonesCanvas : MonoBehaviour{
    [AssetsOnly][SerializeField] GameObject zoneButtonPrefab;
    [ChildGameObjectsOnly][SerializeField] Transform listContent;
    [ChildGameObjectsOnly][SerializeField] ShipUI shipUI;
    [SerializeField] float regularZoneSize=1.7f;
    [SerializeField] float bossZoneSize=2.5f;
    void Start(){Setup();}
    public void Setup(){
        foreach(Transform t in listContent){if(t.name!="Future"&&t!=shipUI.transform)Destroy(t.gameObject);}
        for(var i=0;i<GameCreator.instance.adventureZones.Capacity;i++){if(GameCreator.instance.adventureZones[i].enabled){
            var _i=i;
            var go=Instantiate(zoneButtonPrefab,listContent);
            go.name="Zone_"+GameCreator.instance.adventureZones[i].name;
            go.GetComponent<RectTransform>().anchoredPosition=GameCreator.instance.adventureZones[i].pos;
            if(!SaveSerial.instance.advD.lockedZones.Contains(GameCreator.instance.adventureZones[i].name))go.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(()=>GSceneManager.instance.LoadAdventureZone(_i));

            var mat=GameAssets.instance.UpdateShaderMatProps(GameAssets.instance.GetMat("AIOShaderMat_UI",true),GameCreator.instance.adventureZones[i].gameRules.bgMaterial,true);
            go.transform.GetChild(1).GetComponent<Image>().material=null;go.transform.GetChild(1).GetComponent<Image>().material=mat;//refresh it
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text=GameCreator.instance.adventureZones[i].name;
            go.transform.GetChild(4).GetComponent<ShipLevelRequired>().value=GameCreator.instance.adventureZones[i].lvlReq;

            if(GameCreator.instance.adventureZones[i].isBoss){
                go.transform.localScale=new Vector2(bossZoneSize,bossZoneSize);
                if(GameCreator.instance.adventureZones[i].gameRules.cfgIconsGo!=null){
                    Destroy(go.transform.GetChild(3).GetComponent<Image>());
                    var icons=Instantiate(GameCreator.instance.adventureZones[i].gameRules.cfgIconsGo,go.transform.GetChild(3));
                    if(GameCreator.instance.adventureZones[i].bossBlackOutImg){foreach(Image img in icons.transform){var lvreq=img.gameObject.AddComponent<ShipLevelRequired>();lvreq.adventureData=true;lvreq.blackOutImg=true;lvreq.value=GameCreator.instance.adventureZones[i].lvlReq;}}
                }else{
                    if(GameCreator.instance.adventureZones[i].gameRules.cfgIconAssetName!=""){
                        var img=go.transform.GetChild(3).GetComponent<Image>();
                        go.transform.GetChild(3).GetComponent<Image>().sprite=GameAssets.instance.SprAny(GameCreator.instance.adventureZones[i].gameRules.cfgIconAssetName);
                        if(GameCreator.instance.adventureZones[i].bossBlackOutImg){var lvreq=img.gameObject.AddComponent<ShipLevelRequired>();lvreq.adventureData=true;lvreq.blackOutImg=true;lvreq.value=GameCreator.instance.adventureZones[i].lvlReq;}
                    }
                }
                if(!SaveSerial.instance.advD.defeatedBosses.Contains(GameCreator.instance.adventureZones[i].gameRules.bossInfo.name)){Destroy(go.transform.GetChild(6).gameObject);}
                Destroy(go.transform.GetChild(2).gameObject);
            }else{go.transform.localScale=new Vector2(regularZoneSize,regularZoneSize);Destroy(go.transform.GetChild(6).gameObject);Destroy(go.transform.GetChild(5).gameObject);Destroy(go.transform.GetChild(3).gameObject);}
            if(!SaveSerial.instance.advD.lockedZones.Contains(GameCreator.instance.adventureZones[i].name)){if(go.transform.childCount>5){Destroy(go.transform.GetChild(5).gameObject);}else{Destroy(go.transform.GetChild(4).gameObject);}}
        }}
        shipUI.transform.SetAsLastSibling();
    }
    public void Back(){if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Game"){UpgradeMenu.instance.Back();}else{GSceneManager.instance.LoadGameModeChooseScene();}}
}