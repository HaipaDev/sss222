using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;

public class ButtonScript : EventTrigger{
    public override void OnSelect(BaseEventData data){AudioManager.instance.Play("ButtonSelect");}
    public override void OnSubmit(BaseEventData data){AudioManager.instance.Play("ButtonPress");}
    public override void OnPointerClick(PointerEventData data){AudioManager.instance.Play("ButtonPress");}
}
