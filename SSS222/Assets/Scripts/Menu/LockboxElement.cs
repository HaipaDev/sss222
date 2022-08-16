using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LockboxElement : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{
    [SerializeField] public Image iconImg;
    [SerializeField] public TextMeshProUGUI titleText;
    [SerializeField] public TextMeshProUGUI countText;
    public string name;
    void Start(){}
    void Update(){
        if(countText!=null)countText.text="x"+_lockboxCount(name).count.ToString();
    }
    public void UseLockbox(){
        if(_lockboxCount(name).count>0){
            CustomizationInventory.instance.OpenLockboxOpeningPanel(name);
            _lockboxCount(name).count--;
        }else{AudioManager.instance.Play("Deny");}
    }
    public void AddLockbox(){
        _lockboxCount(name).count++;
    }
    public LockboxCount _lockboxCount(string str){return SaveSerial.instance.playerData.lockboxesInventory.Find(x=>x.name==str);}
    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button==PointerEventData.InputButton.Left){
            UseLockbox();
        }
        else if(eventData.button==PointerEventData.InputButton.Right){
            AddLockbox();
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        //if(FindObjectOfType<CstmzSelectedInfo>()!=null){FindObjectOfType<CstmzSelectedInfo>().selectedElement=gameObject;}
    }
    public void OnPointerExit(PointerEventData eventData){//if(FindObjectOfType<CstmzSelectedInfo>()!=null){if(!UIInputSystem.instance.inputSelecting)
        //if(FindObjectOfType<CstmzSelectedInfo>().selectedElement==gameObject){FindObjectOfType<CstmzSelectedInfo>().selectedElement=null;}}
    }
}
