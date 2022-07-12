using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class AdventureZonesCanvas : MonoBehaviour{
    [ChildGameObjectsOnly][SerializeField] GameObject zoneButtonChildObject;
    [ChildGameObjectsOnly][SerializeField] Transform listContent;
    [SerializeField] float regularZoneSize=1.7f;
    [SerializeField] float bossZoneSize=2.5f;
    void Start(){
        for(var i=0;i<GameCreator.instance.adventureZones.Capacity;i++){if(GameCreator.instance.adventureZones[i].enabled){
            var _i=i;
            var go=Instantiate(zoneButtonChildObject,listContent);
            go.name="Zone_"+(i+1);
            go.GetComponent<RectTransform>().anchoredPosition=GameCreator.instance.adventureZones[i].pos;
            go.GetComponent<Button>().onClick.AddListener(()=>GSceneManager.instance.LoadAdventureZone(_i));

            var mat=GameAssets.instance.UpdateShaderMatProps(GameAssets.instance.GetMat("AIOShaderMat_UI",true),GameCreator.instance.adventureZones[i].gameRules.bgMaterial,true);
            go.transform.GetChild(1).GetComponent<Image>().material=null;go.transform.GetChild(1).GetComponent<Image>().material=mat;//refresh it
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text=(i+1).ToString();

            if(GameCreator.instance.adventureZones[i].isBoss){
                go.transform.localScale=new Vector2(bossZoneSize,bossZoneSize);
                if(GameCreator.instance.adventureZones[i].gameRules.cfgIconsGo!=null){
                    Destroy(go.transform.GetChild(3).GetComponent<Image>());
                    Instantiate(GameCreator.instance.adventureZones[i].gameRules.cfgIconsGo,go.transform.GetChild(3));
                }else{if(GameCreator.instance.adventureZones[i].gameRules.cfgIconAssetName!=""){go.transform.GetChild(3).GetComponent<Image>().sprite=GameAssets.instance.SprAny(GameCreator.instance.adventureZones[i].gameRules.cfgIconAssetName);}}
                Destroy(go.transform.GetChild(2).gameObject);
            }else{go.transform.localScale=new Vector2(regularZoneSize,regularZoneSize);Destroy(go.transform.GetChild(3).gameObject);}
        }}
        Destroy(zoneButtonChildObject);
    }
    public void Back(){if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Game"){UpgradeMenu.instance.Back();}else{GSceneManager.instance.LoadGameModeChooseScene();}}
}