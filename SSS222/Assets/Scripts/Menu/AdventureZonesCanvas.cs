using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class AdventureZonesCanvas : MonoBehaviour{
    [AssetsOnly][SerializeField] GameObject zoneButtonPrefab;
    [AssetsOnly][SerializeField] GameObject zoneLinePrefab;
    [ChildGameObjectsOnly][SerializeField] Transform listContent;
    [ChildGameObjectsOnly][SerializeField] Tag_ZonesLine travelLine;
    [ChildGameObjectsOnly][SerializeField] ShipUI shipUI;
    [SerializeField] float regularZoneSize=1.7f;
    [SerializeField] float bossZoneSize=2.5f;
    void Start(){Setup();}
    void Update(){
        if(listContent.Find("Zone_0")!=null){Setup();}

        //Set Travel Line
        if(travelLine!=null){
            if(GameManager.instance.zoneToTravelTo!=-1&&
            (travelLine.GetPoint(0)!=travelLine.GetPosFromZoneId(GameManager.instance.zoneSelected)&&travelLine.GetPoint(1)!=travelLine.GetPosFromZoneId(GameManager.instance.zoneToTravelTo))
            ){UpdateTravelLine();travelLine.SetPoints(GameManager.instance.zoneSelected,GameManager.instance.zoneToTravelTo,true);}
            if(GameManager.instance.zoneToTravelTo==-1&&!travelLine.BothPointsNull()){UpdateTravelLine();travelLine.SetBothPointsNull();}
            //if(travelLine!=null){travelLine.GetComponent<UnityEngine.UI.Extensions.UILineRenderer>().enabled=1;travelLine.GetComponent<UnityEngine.UI.Extensions.UILineRenderer>().Resolution=0;Debug.Log("Updating the line");}
        }else{Debug.LogWarning("Travel Line not set in the inspector");}
    }
    public void UpdateTravelLine(){StartCoroutine(UpdateTravelLineI());}
    IEnumerator UpdateTravelLineI(){
        yield return new WaitForSeconds(0.03f);
        //if(travelLine!=null){travelLine.GetComponent<UnityEngine.UI.Extensions.UILineRenderer>().Resolution=1;travelLine.GetComponent<UnityEngine.UI.Extensions.UILineRenderer>().Resolution=0;
        if(travelLine!=null){travelLine.GetComponent<UnityEngine.UI.Extensions.UILineRenderer>().enabled=false;travelLine.GetComponent<UnityEngine.UI.Extensions.UILineRenderer>().enabled=true;
        //Debug.Log("Updating the line");
        }
    }
    public void Setup(){
        foreach(Transform t in listContent){if(t.name!="Future"&&t!=shipUI.transform&&t!=travelLine.transform)Destroy(t.gameObject);}
        for(var i=0;i<CoreSetup.instance.adventureZones.Capacity;i++){if(!CoreSetup.instance.adventureZones[i].enabled){break;}
            var _i=i;
            var go=Instantiate(zoneButtonPrefab,listContent);
            go.name="Zone_"+CoreSetup.instance.adventureZones[i].name;
            go.GetComponent<RectTransform>().anchoredPosition=CoreSetup.instance.adventureZones[i].pos;
            if((SaveSerial.instance.advD!=null&&SaveSerial.instance.advD.lockedZones!=null&&!SaveSerial.instance.advD.lockedZones.Contains(CoreSetup.instance.adventureZones[i].name))||SaveSerial.instance.advD==null||SaveSerial.instance.advD.lockedZones==null||SaveSerial.instance.advD.lockedZones.Count==0){
                go.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(()=>GSceneManager.instance.LoadAdventureZone(_i,(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="AdventureZones")));
            }

            var mat=AssetsManager.instance.UpdateShaderMatProps(AssetsManager.instance.GetMat("AIOShaderMat_UI",true),CoreSetup.instance.adventureZones[i].gameRules.bgMaterial,true);
            go.transform.GetChild(1).GetComponent<Image>().material=null;go.transform.GetChild(1).GetComponent<Image>().material=mat;//refresh it
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text=CoreSetup.instance.adventureZones[i].name;
            go.transform.GetChild(4).GetComponent<ShipLevelRequired>().value=CoreSetup.instance.adventureZones[i].lvlReq;

            if(CoreSetup.instance.adventureZones[i].isBoss){
                go.transform.localScale=new Vector2(bossZoneSize,bossZoneSize);
                if(CoreSetup.instance.adventureZones[i].gameRules.cfgIconsGo!=null){
                    Destroy(go.transform.GetChild(3).GetComponent<Image>());
                    var icons=Instantiate(CoreSetup.instance.adventureZones[i].gameRules.cfgIconsGo,go.transform.GetChild(3));
                    if(CoreSetup.instance.adventureZones[i].bossBlackOutImg){foreach(Image img in icons.transform){var lvreq=img.gameObject.AddComponent<ShipLevelRequired>();lvreq.adventureData=true;lvreq.blackOutImg=true;lvreq.value=CoreSetup.instance.adventureZones[i].lvlReq;}}
                }else{
                    if(CoreSetup.instance.adventureZones[i].gameRules.cfgIconAssetName!=""){
                        var img=go.transform.GetChild(3).GetComponent<Image>();
                        go.transform.GetChild(3).GetComponent<Image>().sprite=AssetsManager.instance.SprAny(CoreSetup.instance.adventureZones[i].gameRules.cfgIconAssetName);
                        if(CoreSetup.instance.adventureZones[i].bossBlackOutImg){var lvreq=img.gameObject.AddComponent<ShipLevelRequired>();lvreq.adventureData=true;lvreq.blackOutImg=true;lvreq.value=CoreSetup.instance.adventureZones[i].lvlReq;}
                    }
                }
                if((SaveSerial.instance.advD!=null&&SaveSerial.instance.advD.lockedZones!=null&&!(SaveSerial.instance.advD.defeatedBosses.Contains(CoreSetup.instance.adventureZones[i].gameRules.bossInfo.name)||SaveSerial.instance.advD.defeatedBosses.Contains(CoreSetup.instance.adventureZones[i].gameRules.bossInfo.codeName)))||SaveSerial.instance.advD==null||SaveSerial.instance.advD.defeatedBosses==null||SaveSerial.instance.advD.defeatedBosses.Count==0){
                    Destroy(go.transform.GetChild(6).gameObject);
                }
                Destroy(go.transform.GetChild(2).gameObject);
            }else{go.transform.localScale=new Vector2(regularZoneSize,regularZoneSize);Destroy(go.transform.GetChild(6).gameObject);Destroy(go.transform.GetChild(5).gameObject);Destroy(go.transform.GetChild(3).gameObject);}
            if((SaveSerial.instance.advD!=null&&SaveSerial.instance.advD.lockedZones!=null&&!SaveSerial.instance.advD.lockedZones.Contains(CoreSetup.instance.adventureZones[i].name))||SaveSerial.instance.advD==null||SaveSerial.instance.advD.lockedZones==null||SaveSerial.instance.advD.lockedZones.Count==0){
                if(go.transform.childCount>5){Destroy(go.transform.GetChild(5).gameObject);}
                else{Destroy(go.transform.GetChild(4).gameObject);}
            }

            //Setup line
            if(i<CoreSetup.instance.adventureZones.Capacity-1){
                var l=Instantiate(zoneLinePrefab,listContent);
                l.name="_zonesLine_"+(i+1)+"-"+(i+2);
                l.GetComponent<Tag_ZonesLine>().SetPoints(i,i+1);
                l.transform.SetAsFirstSibling();
            }
        }
        travelLine.transform.SetAsLastSibling();
        shipUI.transform.SetAsLastSibling();
    }
    public void Back(){if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Game"){UpgradeMenu.instance.Back();}else{GSceneManager.instance.LoadGameModeChooseScene();}}
}