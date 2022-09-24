using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class CstmzSelectedInfo : MonoBehaviour{
    [Header("Settings")]
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI artistTxt;
    Vector2 posLeft;
    [SerializeField]float posXRight;
    [SerializeField]float xFlipRight=36f;
    [Header("Variables")]
    public TextAlignmentOptions txtAlign=TextAlignmentOptions.Left;
    public GameObject selectedElement;

    
    void Awake(){posLeft=transform.GetChild(0).localPosition;}
    void Update(){
        var name="Mk.22";Color color=Color.white;var artist="Hyper";
        CstmzElement ce=null;CstmzTypeElement cte=null;
        //T ceType=AssetsManager.instance.GetCustomizationTypeFromEnum<ce.elementType>();

        if(!UIInputSystem.instance.inputSelecting){transform.position=Input.mousePosition;}
        else{
            var cs=UIInputSystem.instance.currentSelected;
            if(cs!=null){
                if((cs.GetComponent<CstmzElement>()!=null&&!cs.GetComponent<CstmzElement>().variant)||cs.GetComponent<CstmzTypeElement>()!=null){
                    selectedElement=cs;
                    transform.position=selectedElement.transform.position;
                }else{selectedElement=null;}
            }else{selectedElement=null;}
        }

        if(transform.localPosition.x>=xFlipRight){txtAlign=TextAlignmentOptions.Right;}else{txtAlign=TextAlignmentOptions.Left;}
        if(txtAlign==TextAlignmentOptions.Left){transform.GetChild(0).localPosition=posLeft;}
        else if(txtAlign==TextAlignmentOptions.Right){transform.GetChild(0).localPosition=new Vector2(posXRight,posLeft.y);}
        nameTxt.alignment=txtAlign;
        artistTxt.alignment=txtAlign;

        if(selectedElement!=null){
            transform.GetChild(0).gameObject.SetActive(true);

            //if(ce!=null){name=ce.elementName;artist=AssetsManager.GetArtist<ceType>(ce.elementName);}
            if(selectedElement.GetComponent<CstmzElement>()!=null)ce=selectedElement.GetComponent<CstmzElement>();
            if(ce!=null){
                name=AssetsManager.instance.GetDisplayName(CutUnderscore(ce.elementName),ce.elementType);
                artist=AssetsManager.instance.GetArtist(CutUnderscore(ce.elementName),ce.elementType);
                color=AssetsManager.instance.GetRarityColor(ce.rarity);
            }

            if(selectedElement.GetComponent<CstmzTypeElement>()!=null)cte=selectedElement.GetComponent<CstmzTypeElement>();
            if(cte!=null){
                name=AssetsManager.instance.GetDisplayName(CutUnderscore(cte.elementName),cte.elementType);
                artist=AssetsManager.instance.GetArtist(CutUnderscore(cte.elementName),cte.elementType);
                color=AssetsManager.instance.GetRarityColor(cte.rarity);
            }
        }else{transform.GetChild(0).gameObject.SetActive(false);}

        if(artist==""){artist="Hyper";}
        if(artist==null){if(artistTxt!=null){if(artistTxt.gameObject.activeSelf){artistTxt.gameObject.SetActive(false);}}}
        else {if(artistTxt!=null){if(!artistTxt.gameObject.activeSelf){artistTxt.gameObject.SetActive(true);}}}
        if(nameTxt!=null){nameTxt.text=name;nameTxt.color=color;}else{Debug.LogWarning("NameTxt obj not assigned");}
        if(artistTxt!=null){artistTxt.text="Artist: "+artist;}else{Debug.LogWarning("ArtistTxt obj not assigned");}
    }
    string CutUnderscore(string str){string _str=str;if(_str.Contains("_")){_str=_str.Split('_')[0];}return _str;}
}
