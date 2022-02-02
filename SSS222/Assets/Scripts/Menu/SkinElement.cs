using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class SkinElement : MonoBehaviour, IPointerClickHandler{
    [SerializeField] public string skinName="Mk.22";
    [SerializeField] public CstmzRarity rarity;
    [SerializeField] public bool variant;
    [EnableIf("@this.variant")][SerializeField] public int variantId=-1;
    void Update(){
        GetComponent<Image>().color=CustomizationInventory.instance.GetRarityColor(rarity);
        if(ShipCustomizationManager.instance.GetOverlaySprite(skinName+_VariantID())!=null){transform.GetChild(2).GetComponent<Image>().color=CustomizationInventory.instance.overlayColor;}
        else{transform.GetChild(2).GetComponent<Image>().color=Color.clear;}
    }
    public void SetSkin(){CustomizationInventory.instance.SetSkin(skinName);}
    public void SetVariant(){CustomizationInventory.instance.SetSkin(skinName+_VariantID());}

    public void OpenVariants(){CustomizationInventory.instance.OpenVariants(skinName);}
    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button==PointerEventData.InputButton.Left){
            SetSkin();
            SetVariant();
        }
        else if(eventData.button==PointerEventData.InputButton.Right&&!variant){
            OpenVariants();
        }
    }
    string _VariantID(){string _str="";if(variantId>=0){_str="_"+variantId;}else{}return _str;}
}