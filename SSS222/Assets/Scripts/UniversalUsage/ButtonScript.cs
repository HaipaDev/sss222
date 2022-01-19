using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class ButtonScript : EventTrigger{
    [DisableInEditorMode]public bool selected=false;
    [DisableInEditorMode]public bool pressed=false;
    public override void OnSelect(BaseEventData data){AudioManager.instance.Play("ButtonSelect");foreach(ButtonScript b in FindObjectsOfType<ButtonScript>()){b.selected=false;}selected=true;}
    public override void OnSubmit(BaseEventData data){AudioManager.instance.Play("ButtonPress");}
    public override void OnPointerClick(PointerEventData data){AudioManager.instance.Play("ButtonPress");}
    //public void OnUpdateSelected(BaseEventData data){if(isPressed)Shoot();}
    public void OnPointerDown(PointerEventData data){StartCoroutine(Pressed());}
    //public void OnPointerUp(PointerEventData data){isPressed=false;}
    IEnumerator Pressed(){pressed=true;yield return new WaitForSeconds(0.05f);pressed=false;}
}
