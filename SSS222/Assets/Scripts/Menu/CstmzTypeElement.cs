using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
//using UnityEngine.Events.UnityEventTools;
using Sirenix.OdinInspector;

public class CstmzTypeElement : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{
    [Header("Object References")]
    [SceneObjectsOnly][SerializeField] public GameObject selectedBg;
    [SceneObjectsOnly][SerializeField] public GameObject elementPv;
    [SceneObjectsOnly][SerializeField] public GameObject overlayImg;
    [Header("Properties")]
    [SerializeField] public CstmzType elementType=CstmzType.skin;
    [SerializeField] public string elementName="Mk.22";
    [SerializeField] public CstmzRarity rarity;
    void Awake(){GetComponent<Button>().onClick.AddListener(SetType);}
    void Update(){
        GetComponent<Image>().color=CustomizationInventory.instance.GetRarityColor(rarity);
        switch(elementType){
            case CstmzType.skin:
                elementName=CustomizationInventory.instance.skinName;
                if(CustomizationInventory.instance.GetSkinSprite(elementName)!=null){elementPv.GetComponent<Image>().sprite=CustomizationInventory.instance.GetSkinSprite(elementName);}
                if(CustomizationInventory.instance.GetOverlaySprite(elementName)!=null){overlayImg.GetComponent<Image>().color=CustomizationInventory.instance.overlayColor;}
                else{overlayImg.GetComponent<Image>().color=Color.clear;}
                rarity=CustomizationInventory.instance.GetSkin(elementName).rarity;
            break;
            case CstmzType.trail:
                elementName=CustomizationInventory.instance.trailName;
                rarity=CustomizationInventory.instance.GetTrail(elementName).rarity;
            break;
            case CstmzType.flares:
                elementName=CustomizationInventory.instance.flaresName;
                rarity=CustomizationInventory.instance.GetFlares(elementName).rarity;
            break;
            case CstmzType.deathFx:
                elementName=CustomizationInventory.instance.deathFxName;
                rarity=CustomizationInventory.instance.GetDeathFx(elementName).rarity;
            break;
            case CstmzType.music:
                elementName=CustomizationInventory.instance.musicName;
                rarity=CustomizationInventory.instance.GetMusic(elementName).rarity;
            break;
        }
        /*if(UIInputSystem.instance.currentSelected==gameObject){
            UnityEventTools.AddPersistentListener(GetComponent<Button>().onClick,SetType());
        }else{UnityEventTools.RemovePersistentListener(GetComponent<Button>().onClick,SetType());}*/
    }
    public void SetType(){CustomizationInventory.instance.SetType(elementType);}
    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button==PointerEventData.InputButton.Left){
            SetType();
        }
    }
    public void OnPointerEnter(PointerEventData eventData){
        if(FindObjectOfType<CstmzSelectedInfo>()!=null){FindObjectOfType<CstmzSelectedInfo>().selectedElement=gameObject;}
    }
    public void OnPointerExit(PointerEventData eventData){if(FindObjectOfType<CstmzSelectedInfo>()!=null){if(!UIInputSystem.instance.inputSelecting)
        if(FindObjectOfType<CstmzSelectedInfo>().selectedElement==gameObject){FindObjectOfType<CstmzSelectedInfo>().selectedElement=null;}}}
}