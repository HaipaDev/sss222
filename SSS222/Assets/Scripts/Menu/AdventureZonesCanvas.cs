using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class AdventureZonesCanvas : MonoBehaviour{
    [ChildGameObjectsOnly][SerializeField] GameObject zoneButtonChildObject;
    [ChildGameObjectsOnly][SerializeField] Transform listContent;
    [ChildGameObjectsOnly][SerializeField] ShipUI shipUI;
    [SerializeField] float regularZoneSize=1.7f;
    [SerializeField] float bossZoneSize=2.5f;
    void Start(){
        for(var i=0;i<GameCreator.instance.adventureZones.Capacity;i++){if(GameCreator.instance.adventureZones[i].enabled){
            var _i=i;
            var go=Instantiate(zoneButtonChildObject,listContent);
            go.name="Zone_"+GameCreator.instance.adventureZones[i].name;
            go.GetComponent<RectTransform>().anchoredPosition=GameCreator.instance.adventureZones[i].pos;
            go.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(()=>GSceneManager.instance.LoadAdventureZone(_i));

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
                Destroy(go.transform.GetChild(2).gameObject);
            }else{go.transform.localScale=new Vector2(regularZoneSize,regularZoneSize);Destroy(go.transform.GetChild(3).gameObject);}
        }}
        Destroy(zoneButtonChildObject);
        shipUI.transform.SetAsLastSibling();
    }
    public void Back(){if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Game"){UpgradeMenu.instance.Back();}else{GSceneManager.instance.LoadGameModeChooseScene();}}
}