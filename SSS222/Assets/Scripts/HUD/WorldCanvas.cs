using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour{
    public static WorldCanvas instance;
    void Awake(){instance=this;}
    public GameObject DMGPopup(float dmg, Vector2 pos, Color color, float scale=1, bool isPlayer=false){    if(SaveSerial.instance.settingsData.dmgPopups){
        GameObject dmgpopup=WorldCanvas.CreateOnUI(GameAssets.instance.GetVFX("DMGPopup"),pos);
        dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=color;
        dmgpopup.transform.localScale=new Vector2(scale,scale);
        if(Player.instance!=null&&isPlayer)dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg/Player.instance.armorMulti,2).ToString();
        else{dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg,1).ToString();}
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
