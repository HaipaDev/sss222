using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour{
    [SerializeField] public ShopItemID item;
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI descTxt;
    [SerializeField] TextMeshProUGUI priceTxt;
    [SerializeField] Image itemImg;
    [SerializeField] int price;
    [SerializeField] int limit=1;
    public int purchasedCount;
    public int limitCount;
    public void SetItem(ShopItemID item){
        purchasedCount=0;
        limitCount=0;
        this.item=item;
    }
    public void SetPrice(int val){price=val;}
    public void SetLimit(int val){limit=val;}
    void Update(){
        if(item!=null){
            nameTxt.text=item.name;
            descTxt.text=item.desc;
            priceTxt.text=price.ToString();
            itemImg.sprite=item.img;
            if(limitCount>=limit){GetComponentInChildren<Button>().interactable=false;}else{GetComponentInChildren<Button>().interactable=true;}
        }
    }
    public void Purchase(){
    if(GameSession.instance.coins>=price&&limitCount<limit){
        GameSession.instance.coins-=price;
        purchasedCount++;
        if(Shop.instance.currentSlotID>=Shop.instance.slotsWhenLimit)limitCount++;
        var pos=Player.instance.transform.position;
        switch(item.ID){
            case 0:Instantiate(GameAssets.instance.Get("Laser2Pwrup"),pos,Quaternion.identity);break;
            case 1:Instantiate(GameAssets.instance.Get("Laser3Pwrup"),pos,Quaternion.identity);break;
            case 2:Instantiate(GameAssets.instance.Get("ArmorPwrup"),pos,Quaternion.identity);break;
            case 3:Instantiate(GameAssets.instance.Get("MicroMedkit"),pos,Quaternion.identity);break;
            case 4:Instantiate(GameAssets.instance.Get("Battery"),pos,Quaternion.identity);break;
            case 5:Instantiate(GameAssets.instance.Get("ScalerPwrup"),pos,Quaternion.identity);break;
            case 6:Instantiate(GameAssets.instance.Get("ShadowdashPwrup"),pos,Quaternion.identity);break;
            case 7:Instantiate(GameAssets.instance.Get("MagnetPwrup"),pos,Quaternion.identity);break;
            case 8:Instantiate(GameAssets.instance.Get("LSaberPwrup"),pos,Quaternion.identity);break;
            case 9:Instantiate(GameAssets.instance.Get("HRocketPwrup"),pos,Quaternion.identity);break;
            case 10:Instantiate(GameAssets.instance.Get("QRocketPwrup"),pos,Quaternion.identity);break;
            case 11:Instantiate(GameAssets.instance.Get("InverterPwrup"),pos,Quaternion.identity);break;
            case 12:Instantiate(GameAssets.instance.Get("MatrixPwrup"),pos,Quaternion.identity);break;
            case 13:Instantiate(GameAssets.instance.Get("AccelPwrup"),pos,Quaternion.identity);break;
            case 14:Instantiate(GameAssets.instance.Get("CStreamPwrup"),pos,Quaternion.identity);break;
            case 15:Instantiate(GameAssets.instance.Get("BlackFiol"),pos,Quaternion.identity);break;
            case 16:Instantiate(GameAssets.instance.Get("RandomizerPwrup"),pos,Quaternion.identity);break;
            case 17:Instantiate(GameAssets.instance.Get("MLaserPwrup"),pos,Quaternion.identity);break;
            case 18:Instantiate(GameAssets.instance.Get("PLaserPwrup"),pos,Quaternion.identity);break;
        }
        Shop.instance.Purchase();
    }
    }
}
