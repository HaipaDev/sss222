using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
//using UnityEngine.Events.UnityEventTools;
using Sirenix.OdinInspector;

public class CstmzElement : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{
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
    void Awake(){GetComponent<Button>().onClick.AddListener(SetElement);}
    void Update(){
        GetComponent<Image>().color=GameAssets.instance.GetRarityColor(rarity);
        if(elementType==CstmzType.skin){
            if(!CustomizationInventory.instance.SkinHasVariants(elementName)||variant){if(editButton.activeSelf)editButton.SetActive(false);}
            if(CustomizationInventory.instance.GetSkinSprite(elementName+_VariantID())!=null){elementPv.GetComponent<Image>().sprite=CustomizationInventory.instance.GetSkinSprite(elementName+_VariantID());}
            if(CustomizationInventory.instance.GetSkin(elementName).animated){elementPv.GetComponent<Image>().sprite=GetSkinSpriteAnim(elementName);}
            if(CustomizationInventory.instance.GetOverlaySprite(elementName+_VariantID())!=null){overlayImg.GetComponent<Image>().color=CustomizationInventory.instance.overlayColor;}
            else{overlayImg.GetComponent<Image>().color=Color.clear;}
        }else{if(overlayImg!=null){Destroy(overlayImg);}if(editButton.activeSelf)editButton.SetActive(false);}

        if(elementType==CstmzType.skin){if(!CustomizationInventory._isSkinUnlocked(elementName)){elementPv.GetComponent<Image>().color=new Color(0.15f,0.15f,0.15f,1);}else{elementPv.GetComponent<Image>().color=Color.white;}}
        if(elementType==CstmzType.trail){if(!CustomizationInventory._isTrailUnlocked(elementName)){elementPv.GetComponent<Image>().color=new Color(0.15f,0.15f,0.15f,1);}else{elementPv.GetComponent<Image>().color=Color.clear;}}
        if(elementType==CstmzType.flares){if(!CustomizationInventory._isFlaresUnlocked(elementName)){elementPv.GetComponent<Image>().color=new Color(0.15f,0.15f,0.15f,1);}else{elementPv.GetComponent<Image>().color=Color.clear;}}
        if(elementType==CstmzType.deathFx){if(!CustomizationInventory._isDeathFxUnlocked(elementName)){elementPv.GetComponent<Image>().color=new Color(0.15f,0.15f,0.15f,1);}else{elementPv.GetComponent<Image>().color=Color.clear;}}
        if(elementType==CstmzType.music){if(!CustomizationInventory._isMusicUnlocked(elementName)){elementPv.GetComponent<Image>().color=new Color(0.15f,0.15f,0.15f,1);}else{elementPv.GetComponent<Image>().color=Color.white;}}

        if(UIInputSystem.instance.currentSelected==gameObject){if(Input.GetKeyDown(KeyCode.E))OpenVariants();}
        /*if(UIInputSystem.instance.currentSelected==gameObject){
            UnityEventTools.AddPersistentListener(GetComponent<Button>().onClick,SetElement());
        }else{UnityEventTools.RemovePersistentListener(GetComponent<Button>().onClick,SetElement());}*/
    }
    public void SetSkin(){if(CustomizationInventory._isSkinUnlocked(elementName))CustomizationInventory.instance.SetSkin(elementName+_VariantID());}
    public void SetTrail(){if(CustomizationInventory._isTrailUnlocked(elementName))CustomizationInventory.instance.SetTrail(elementName);}
    public void SetFlares(){if(CustomizationInventory._isFlaresUnlocked(elementName))CustomizationInventory.instance.SetFlares(elementName);FindObjectOfType<ShipUI>().FlaresPreview();}
    public void SetDeathFx(){if(CustomizationInventory._isDeathFxUnlocked(elementName))CustomizationInventory.instance.SetDeathFx(elementName);}
    public void SetMusic(){if(CustomizationInventory._isMusicUnlocked(elementName))CustomizationInventory.instance.SetMusic(elementName);}

    
    public void SetElement(){
        switch(elementType){
            case CstmzType.skin:SetSkin();break;
            case CstmzType.trail:SetTrail();break;
            case CstmzType.flares:SetFlares();break;
            case CstmzType.deathFx:SetDeathFx();break;
            case CstmzType.music:SetMusic();break;
        }
    }
    public void OpenVariants(){if(!variant&&CustomizationInventory._isSkinUnlocked(elementName))CustomizationInventory.instance.OpenVariants(elementName);}
    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button==PointerEventData.InputButton.Left){
            SetElement();
        }
        else if(eventData.button==PointerEventData.InputButton.Right){
            OpenVariants();
        }
    }
    public void OnPointerEnter(PointerEventData eventData){
        if(FindObjectOfType<CstmzSelectedInfo>()!=null&&!variant){FindObjectOfType<CstmzSelectedInfo>().selectedElement=gameObject;}
    }
    public void OnPointerExit(PointerEventData eventData){if(FindObjectOfType<CstmzSelectedInfo>()!=null){if(!UIInputSystem.instance.inputSelecting)
        if(FindObjectOfType<CstmzSelectedInfo>().selectedElement==gameObject){FindObjectOfType<CstmzSelectedInfo>().selectedElement=null;}}
    }

    string _VariantID(){string _str="";if(variantId>=0){_str="_"+variantId.ToString();}else{}return _str;}
    public Sprite GetSkinSpriteAnim(string str){CstmzSkin skin=null;Sprite spr=null;
        skin=GameAssets.instance.GetSkin(str);
        if(anim==null){animSpr=skin.animVals[0].spr;anim=StartCoroutine(AnimateSkin(skin));}
        if(animSpr!=null)spr=animSpr;
        return spr;
    }
    Coroutine anim;int iAnim=0;Sprite animSpr;
    IEnumerator AnimateSkin(CstmzSkin skin){Sprite spr;
        if(skin.animSpeed>0){yield return new WaitForSeconds(skin.animSpeed);}
        else{yield return new WaitForSeconds(skin.animVals[iAnim].delay);}
        spr=skin.animVals[iAnim].spr;
        if(iAnim==skin.animVals.Count-1)iAnim=0;
        if(iAnim<skin.animVals.Count)iAnim++;
        animSpr=spr;
        if(elementName==skin.name)anim=StartCoroutine(AnimateSkin(skin));
        else{if(anim!=null)StopCoroutine(anim);anim=null;iAnim=0;}
    }
}