using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonDragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    private ScrollRect scrollRect;
    private Button button;

    private void Awake() {
        scrollRect = GetComponentInParent<ScrollRect>();
        button = GetComponent<Button>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (eventData.pointerPress.GetComponent<Button>() == button) {
            button.interactable = false;
            Debug.Log("Disabling button interaction");
        } else {
            Debug.Log("Redirecting OnBeginDrag event to ScrollRect");
            scrollRect.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData) {
        // If the button is pressed and being dragged, disable button interactions
        if (button == eventData.pointerPress.GetComponent<Button>()) {
            button.interactable = false;
            Debug.Log("Disabling button interaction");
        }

        // If not dragging on a button, redirect the drag event to the ScrollRect
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Button>() != null && eventData.pointerEnter.GetComponent<Button>() != button) {
            return;
        } else {
            Debug.Log("Redirecting OnDrag event to ScrollRect");
            scrollRect.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (button.interactable == false) {
            button.interactable = true;
            Debug.Log("Enabling button interaction");
        } else {
            Debug.Log("Redirecting OnEndDrag event to ScrollRect");
            scrollRect.OnEndDrag(eventData);
        }
    }
/*public class ButtonDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
    private ScrollRect scrollRect;
    private Button button;

    private void Awake()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
        button = GetComponent<Button>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerPress.GetComponent<Button>() == button)
        {
            button.interactable = false;
        }
        else
        {
            scrollRect.OnBeginDrag(eventData);
        }
    }
    public void OnDrag(PointerEventData eventData){
        // If the button is pressed and being dragged, disable button interactions
        if (button == eventData.pointerPress.GetComponent<Button>())
        {
            button.interactable = false;
        }

        // If not dragging on a button, redirect the drag event to the ScrollRect
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Button>() != null && eventData.pointerEnter.GetComponent<Button>() != button)
        {
            return;
        }
        else
        {
            scrollRect.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (button.interactable == false)
        {
            button.interactable = true;
        }
        else
        {
            scrollRect.OnEndDrag(eventData);
        }
    }
*/

/*public class ButtonDragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerDownHandler {

    private bool isDragging = false;
    private Button buttonPressed = null;
    private ScrollRect scrollRect;

    private void Awake() {
        scrollRect = GetComponentInParent<ScrollRect>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        buttonPressed = eventData.pointerPress.GetComponent<Button>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        isDragging = true;
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        isDragging = false;
        if (buttonPressed != null) {
            buttonPressed.interactable = true;
            buttonPressed = null;
        }
        scrollRect.OnEndDrag(eventData);
    }

    private void Update() {
        if (isDragging && buttonPressed != null) {
            buttonPressed.interactable = false;
        }
    }

*/
/*
public class ButtonDragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler{
    ScrollRect scrollRect;
    bool isDragging;
    Button buttonPressed;

    void Awake(){
        scrollRect=GetComponentInParent<ScrollRect>();
        isDragging = false;
        buttonPressed = null;
    }

    public void OnPointerDown(PointerEventData eventData){
        buttonPressed=eventData.pointerPress.GetComponent<Button>();
    }

    public void OnBeginDrag(PointerEventData eventData){
        scrollRect.OnBeginDrag(eventData);
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData){
        scrollRect.OnDrag(eventData);

        // Disable button interactions only if the button being pressed is the same as the button initially pressed down
        if(buttonPressed!=null && buttonPressed == eventData.pointerPress.GetComponent<Button>()){
            buttonPressed.interactable=false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Re-enable button interaction when the user finishes dragging
        if (buttonPressed != null)
        {
            buttonPressed.interactable = true;
            buttonPressed = null;
        }

        // Disable scrolling
        scrollRect.OnEndDrag(eventData);
        isDragging = false;
    }

    void Update()
    {
        if (!isDragging && buttonPressed != null && !Input.GetMouseButton(0))
        {
            // User is no longer dragging and has released the button, re-enable button interaction
            buttonPressed.interactable = true;
            buttonPressed = null;
        }
    }
    */


    /*
    public void OnPointerDown(PointerEventData eventData)
    {
        // Disable button interaction when the user presses down on it
        GetComponent<Button>().interactable = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Enable scrolling
        var scrollRect = GetComponentInParent<ScrollRect>();
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Re-enable button interaction when the user finishes dragging
        GetComponent<Button>().interactable = true;

        // Disable scrolling
        var scrollRect = GetComponentInParent<ScrollRect>();
        scrollRect.OnEndDrag(eventData);
    }
    */
    

    /*ScrollRect scrollRect;
    Button buttonPressed;

    void Awake(){
        scrollRect=GetComponentInParent<ScrollRect>();
        buttonPressed=null;
    }

    public void OnPointerDown(PointerEventData eventData){
        buttonPressed=eventData.pointerPress.GetComponent<Button>();
    }

    public void OnBeginDrag(PointerEventData eventData){
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData){
        scrollRect.OnDrag(eventData);

        // Disable button interactions only if the button being pressed is the same as the button initially pressed down
        if(buttonPressed==eventData.pointerPress.GetComponent<Button>()){
            buttonPressed.interactable=false;
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        scrollRect.OnEndDrag(eventData);

        // Re-enable interactions on the button that was disabled
        if(buttonPressed!=null){
            buttonPressed.interactable=true;
            buttonPressed=null;
        }
    }
    */


    /*
    ScrollRect scrollRect;

    void Awake(){
        scrollRect=GetComponentInParent<ScrollRect>();
    }

    public void OnPointerDown(PointerEventData eventData){
        // Disable the button interactable property while dragging
        scrollRect.StopMovement();
        GetComponent<Button>().interactable = false;
    }

    public void OnPointerUp(PointerEventData eventData){
        // Enable the button interactable property when not dragging
        GetComponent<Button>().interactable = true;
    }*/


    /*
    public void OnBeginDrag(PointerEventData eventData){
        if(eventData.pointerPress==gameObject){//Check if this is the button being dragged
            GetComponent<Button>().interactable=false;
            scrollRect.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData){
        scrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData){
        GetComponent<Button>().interactable=true;
        scrollRect.OnEndDrag(eventData);
    }*/
}