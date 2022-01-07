using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class WorldCanvas : MonoBehaviour{
    public static WorldCanvas instance;
    void Awake(){if(WorldCanvas.instance!=null){Destroy(gameObject);}else{instance=this;}}
    public GameObject DMGPopup(float dmg, Vector2 pos, Color color, float scale=1, bool isPlayer=false){    if(SaveSerial.instance.settingsData.dmgPopups){
        GameObject dmgpopup=WorldCanvas.CreateOnUI(GameAssets.instance.GetVFX("DMGPopup"),pos);
        dmgpopup.GetComponentInChildren<TextMeshProUGUI>().color=color;
        dmgpopup.transform.localScale=new Vector2(scale,scale);
        string symbol="";if(dmg<0){symbol="+";}
        //if(Player.instance!=null&&isPlayer)dmgpopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+System.Math.Round(dmg/Player.instance.armorMulti,1).ToString();
        /*else{*/dmgpopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+System.Math.Round(Mathf.Abs(dmg),1).ToString();//}
        return dmgpopup;
    }else{return null;/*Debug.LogWarning("DMGPopups are disabled!");*/}}

    public static GameObject CreateOnUI(GameObject obj, Vector2 position){
        WorldCanvas canvas=WorldCanvas.instance;
        GameObject childObject=Instantiate(obj,Camera.main.WorldToScreenPoint(position),Quaternion.identity,canvas.transform);
        //childObject.transform.parent = canvas.transform;
        childObject.transform.SetParent(canvas.transform);
        childObject.transform.position=Camera.main.WorldToScreenPoint(position);
        return childObject;
    }
}
