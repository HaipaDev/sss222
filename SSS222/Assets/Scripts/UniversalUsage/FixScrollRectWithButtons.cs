using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class FixScrollRectWithButtons : MonoBehaviour{//, IBeginDragHandler, IEndDragHandler{
    /*private bool buttonPressed = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (buttonPressed)
        {
            eventData.pointerDrag.GetComponent<Button>().interactable = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerPress != null && eventData.pointerPress.GetComponent<Button>() != null)
        {
            buttonPressed = true;
            eventData.pointerPress.GetComponent<Button>().interactable = false;
        }
        else
        {
            buttonPressed = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (buttonPressed)
        {
            eventData.pointerDrag.GetComponent<Button>().interactable = true;
            buttonPressed = false;
        }
    }*/
    IEnumerator Start(){
        // Get all Button components in children
        yield return new WaitForSeconds(0.1f);
        foreach(Button bt in GetComponentsInChildren<Button>()){
            bt.gameObject.AddComponent<ButtonDragHandler>();
        }
    }
    /*public void OnBeginDrag(PointerEventData eventData){
        // Disable all interactable buttons in the ScrollRect
        foreach(Button button in GetComponentsInChildren<Button>()){
            button.interactable = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        // Enable all interactable buttons in the ScrollRect
        foreach(Button button in GetComponentsInChildren<Button>()){
            button.interactable = true;
        }
    }*/
}