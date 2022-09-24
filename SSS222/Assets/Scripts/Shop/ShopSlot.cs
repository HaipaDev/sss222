using System;
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
    [SerializeField] public int price;
    [SerializeField] public int limit=1;
    [SerializeField] public int rep=1;
    public int purchasedCount;
    public int limitCount;
    public void SetItem(ShopItemID item){
        purchasedCount=0;
        limitCount=0;
        this.item=item;
    }
    public void SetPrice(int val){price=val;}
    public void SetLimit(int val){limit=val;}
    public void SetRep(int val){rep=val;}
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
    if(Player.instance!=null){
    if(GameManager.instance.coins>=price&&limitCount<limit){
        GameManager.instance.coins-=price;
        purchasedCount++;
        Shop.instance.RepChange(rep);
        if(Shop.instance.currentSlotID>=Shop.instance.lootTable.currentQueue.slotsWhenLimit)limitCount++;
        var pos=Player.instance.transform.position;
        if(!String.IsNullOrEmpty(item.assetName)){Instantiate(AssetsManager.instance.Get(item.assetName),pos,Quaternion.identity);}
        else{}
        Shop.instance.Purchase();
    }
    }}
}
