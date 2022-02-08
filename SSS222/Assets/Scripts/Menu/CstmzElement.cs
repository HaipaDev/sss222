using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class CstmzElement : MonoBehaviour, IPointerClickHandler{
    [Header("Object References")]
    [SceneObjectsOnly][SerializeField] public GameObject selectedBg;
    [SceneObjectsOnly][SerializeField] public GameObject elementPv;
    [SceneObjectsOnly][SerializeField] public GameObject overlayImg;
    [SceneObjectsOnly][SerializeField] public GameObject editButton;
    [Header("Properties")]
    [SerializeField] public CstmzType elementType=CstmzType.skin;
    [SerializeField] public string elementName="Mk.22";
    [SerializeField] public CstmzRarity rarity;
    [SerializeField] public bool variant;
    [EnableIf("@this.variant")][SerializeField] public int variantId=-1;
    void Update(){
        GetComponent<Image>().color=CustomizationInventory.instance.GetRarityColor(rarity);
        if(elementType==CstmzType.skin){
            if(!CustomizationInventory.instance.SkinHasVariants(elementName)||variant){if(editButton.activeSelf)editButton.SetActive(false);}
            if(CustomizationInventory.instance.GetSkinSprite(elementName+_VariantID())!=null){elementPv.GetComponent<Image>().sprite=CustomizationInventory.instance.GetSkinSprite(elementName+_VariantID());}
            if(CustomizationInventory.instance.GetOverlaySprite(elementName+_VariantID())!=null){overlayImg.GetComponent<Image>().color=CustomizationInventory.instance.overlayColor;}
            else{overlayImg.GetComponent<Image>().color=Color.clear;}
        }else{if(overlayImg!=null){Destroy(overlayImg);}if(editButton.activeSelf)editButton.SetActive(false);}
        //editButton.GetComponent<Button>().onClick.AddListener(OpenVariants);
    }
    public void SetSkin(){CustomizationInventory.instance.SetSkin(elementName+_VariantID());}
    public void SetTrail(){CustomizationInventory.instance.SetTrail(elementName);}
    public void SetFlares(){CustomizationInventory.instance.SetFlares(elementName);}
    public void SetDeathFx(){CustomizationInventory.instance.SetDeathFx(elementName);}
    public void SetMusic(){CustomizationInventory.instance.SetMusic(elementName);}

    public void OpenVariants(){CustomizationInventory.instance.OpenVariants(elementName);}
    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button==PointerEventData.InputButton.Left){
            if(elementType==CstmzType.skin)SetSkin();
            else if(elementType==CstmzType.trail)SetTrail();
            else if(elementType==CstmzType.flares)SetFlares();
            else if(elementType==CstmzType.deathFx)SetDeathFx();
            else if(elementType==CstmzType.music)SetMusic();
        }
        else if(eventData.button==PointerEventData.InputButton.Right&&!variant){
            OpenVariants();
        }
    }
    string _VariantID(){string _str="";if(variantId>=0){_str="_"+variantId;}else{}return _str;}
}