using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotatePointButton_SandboxIcon : MonoBehaviour, IEndDragHandler{
    public void OnEndDrag(PointerEventData eventData){
        SandboxCanvas.instance.saveInfo.iconRotation=GetComponent<RotatePointButton>().currentAngle;
    }
}
