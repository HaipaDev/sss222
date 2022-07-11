using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class AdventureZonesCanvas : MonoBehaviour{
    [ChildGameObjectsOnly][SerializeField] GameObject zoneIconChildObject;
    [ChildGameObjectsOnly][SerializeField] Transform listContent;
    [SerializeField] List<AdventureZoneData> zonesList;
    void Start(){
        //zoneIconChildObject.GetComponent<Button>().onClick.AddListener(()=>GSceneManager.instance.LoadAdventureZone(0));
        //zoneIconChildObject.GetComponent<RectTransform>().anchoredPosition=zonesList[0].pos;

        for(var i=0;i<zonesList.Capacity;i++){
            var _i=i;
            var go=Instantiate(zoneIconChildObject,listContent);
            go.name="Zone_"+(i+1);
            go.GetComponent<RectTransform>().anchoredPosition=zonesList[i].pos;
            go.GetComponent<Button>().onClick.AddListener(()=>GSceneManager.instance.LoadAdventureZone(_i));
            var mat=GameAssets.instance.UpdateShaderMatProps(GameAssets.instance.GetMat("AIOShaderMat_UI",true),GameCreator.instance.adventureZonesPrefabs[i].bgMaterial,true);
            go.transform.GetChild(1).GetComponent<Image>().material=null;go.transform.GetChild(1).GetComponent<Image>().material=mat;//refresh it
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text=(i+1).ToString();
        }
        Destroy(zoneIconChildObject);
    }
}

[System.Serializable]
public class AdventureZoneData{
    public Vector2 pos;
    public bool isBoss;
}