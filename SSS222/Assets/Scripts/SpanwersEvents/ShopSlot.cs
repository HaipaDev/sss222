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
    void Start(){
        
    }
    void Update(){
        if(item!=null){
            if(!itemCloned){item=Instantiate(item);itemCloned=true;}
            if(!priceSet){item.price=Random.Range(item.priceS,item.priceE);priceSet=true;}
            nameTxt.text=item.name;
            descTxt.text=item.desc;
            priceTxt.text=item.price.ToString();
            itemImg.sprite=item.img;
        }
    }
    public void Purchase(){
        if(GameSession.instance.coins>=item.price){
            Shop.instance.Purchase();
        }
    }
}
