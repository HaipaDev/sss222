using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LockboxElement : MonoBehaviour{//, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{
    [SerializeField] public Image iconImg;
    [SerializeField] public TextMeshProUGUI titleText;
    [SerializeField] public TextMeshProUGUI countText;
    [SerializeField] public TextMeshProUGUI costText;
    [DisableInEditorMode] public new string name;
    void Start(){}
    void Update(){
        if(countText!=null)countText.text="x"+_lockboxCount(name).count.ToString();
        if(costText!=null)costText.text=AssetsManager.instance.GetLockbox(name).cost.ToString();
        if(CustomizationInventory._literallyEverythingInCategoryUnlocked(AssetsManager.instance.GetLockbox(name).category))Debug.Log("everything unlocked in: "+name);
    }
    public void UseLockbox(){
        if(_lockboxCount(name).count>0&&!CustomizationInventory._literallyEverythingInCategoryUnlocked(AssetsManager.instance.GetLockbox(name).category)){
            CustomizationInventory.instance.OpenLockboxOpeningPanel(name);
            _lockboxCount(name).count--;
        }else{AudioManager.instance.Play("Deny");}
    }
    public void CraftLockbox(){
        if(SaveSerial.instance.playerData.dynamCelestStars>=AssetsManager.instance.GetLockbox(name).cost&&!CustomizationInventory._literallyEverythingInCategoryUnlocked(AssetsManager.instance.GetLockbox(name).category)){
            _lockboxCount(name).count++;
            SaveSerial.instance.playerData.dynamCelestStars-=AssetsManager.instance.GetLockbox(name).cost;
            AudioManager.instance.Play("LockboxCraft");
        }else{AudioManager.instance.Play("Deny");}
    }
    public LockboxCount _lockboxCount(string str){return SaveSerial.instance.playerData.lockboxesInventory.Find(x=>x.name==str);}
    /*public void OnPointerClick(PointerEventData eventData){
        if(eventData.button==PointerEventData.InputButton.Left){
            UseLockbox();
        }
        else if(eventData.button==PointerEventData.InputButton.Right){
            CraftLockbox();
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        //if(FindObjectOfType<CstmzSelectedInfo>()!=null){FindObjectOfType<CstmzSelectedInfo>().selectedElement=gameObject;}
    }
    public void OnPointerExit(PointerEventData eventData){//if(FindObjectOfType<CstmzSelectedInfo>()!=null){if(!UIInputSystem.instance.inputSelecting)
        //if(FindObjectOfType<CstmzSelectedInfo>().selectedElement==gameObject){FindObjectOfType<CstmzSelectedInfo>().selectedElement=null;}}
    }*/
}
