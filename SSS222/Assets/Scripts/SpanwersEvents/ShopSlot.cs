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
    [SerializeField] bool itemCloned;
    [SerializeField] bool priceSet;
    public void SetItem(ShopItemID item){
        itemCloned=false;
        priceSet=false;
        this.item=item;
    }
    void Update(){
        if(item!=null){
            if(!itemCloned){item=Instantiate(item);itemCloned=true;}
            if(!priceSet&&item.price==0){item.price=Random.Range((int)item.priceR.x,(int)item.priceR.y);priceSet=true;}
            nameTxt.text=item.name;
            descTxt.text=item.desc;
            priceTxt.text=item.price.ToString();
            itemImg.sprite=item.img;
        }
    }
    public void Purchase(){
    if(GameSession.instance.coins>=item.price){
        GameSession.instance.coins-=item.price;
        var pos=Player.instance.transform.position;
        switch(item.ID){
            case 0:Instantiate(GameAssets.instance.Get("Laser2Pwrup"),pos,Quaternion.identity); break;
            case 1:Instantiate(GameAssets.instance.Get("Laser3Pwrup"),pos,Quaternion.identity); break;
            case 2:Instantiate(GameAssets.instance.Get("ArmorPwrup"),pos,Quaternion.identity); break;
            case 3:Instantiate(GameAssets.instance.Get("MicroMedkit"),pos,Quaternion.identity); break;
            case 4:Instantiate(GameAssets.instance.Get("Battery"),pos,Quaternion.identity); break;
            case 5:Instantiate(GameAssets.instance.Get("ScalerPwrup"),pos,Quaternion.identity); break;
            case 6:Instantiate(GameAssets.instance.Get("ShadowdashPwrup"),pos,Quaternion.identity); break;
            case 7:Instantiate(GameAssets.instance.Get("MagnetPwrup"),pos,Quaternion.identity); break;
            case 8:Instantiate(GameAssets.instance.Get("LSaberPwrup"),pos,Quaternion.identity); break;
            case 9:Instantiate(GameAssets.instance.Get("HRocketPwrup"),pos,Quaternion.identity); break;
            case 10:Instantiate(GameAssets.instance.Get("QRocketPwrup"),pos,Quaternion.identity); break;
            case 11:Instantiate(GameAssets.instance.Get("InverterPwrup"),pos,Quaternion.identity); break;
            case 12:Instantiate(GameAssets.instance.Get("MatrixPwrup"),pos,Quaternion.identity); break;
            case 13:Instantiate(GameAssets.instance.Get("AcceleratorPwrup"),pos,Quaternion.identity); break;
            case 14:Instantiate(GameAssets.instance.Get("CStreamPwrup"),pos,Quaternion.identity); break;
            case 15:Instantiate(GameAssets.instance.Get("BlackFiol"),pos,Quaternion.identity); break;
            case 16:Instantiate(GameAssets.instance.Get("RandomizerPwrup"),pos,Quaternion.identity); break;
            case 17:Instantiate(GameAssets.instance.Get("MLaserPwrup"),pos,Quaternion.identity); break;
            case 18:Instantiate(GameAssets.instance.Get("PLaserPwrup"),pos,Quaternion.identity); break;
        }
        Shop.instance.Purchase();
    }
    }
}
