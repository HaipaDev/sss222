using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlacesCanvas : MonoBehaviour{
    [SerializeField] bool setStart;
    [SerializeField] bool lefthandSwitch;
    [SerializeField] RectTransform[] placeSwitchItemsL;
    [SerializeField] RectTransform[] placeSwitchItemsR;
    [SerializeField] PosSetTransform[] placeSwitchSpec;
    [SerializeField] RectTransform[] anchorSwitchItemsL;
    [SerializeField] RectTransform[] anchorSwitchItemsR;
    void Start(){
        if(setStart){Set();}
    }
    public void Set(){
        for(var i=0;i<placeSwitchItemsL.Length&&i<placeSwitchItemsR.Length;i++){
            var posL=placeSwitchItemsL[i].localPosition;var posR=placeSwitchItemsR[i].localPosition;
            placeSwitchItemsL[i].localPosition=posR;placeSwitchItemsR[i].localPosition=posL;
        }
        for(var i=0;i<anchorSwitchItemsL.Length;i++){
            anchorSwitchItemsL[i].SetAnchor(AnchorPresets.BottomRight);
        }
        for(var i=0;i<anchorSwitchItemsR.Length;i++){
            anchorSwitchItemsR[i].SetAnchor(AnchorPresets.BottomLeft);
        }
        for(var i=0;i<placeSwitchSpec.Length;i++){
            placeSwitchSpec[i].SetPos(placeSwitchSpec[i].GetPos());
        }
        Destroy(this);
    }
    private void Update() {
        if(lefthandSwitch){if(FindObjectOfType<SaveSerial>().lefthand){Set();}}
    }
    [System.Serializable]class PosSetTransform{
        [SerializeField]RectTransform rectTransform;
        [SerializeField]Vector2 pos;
        public void SetPos(Vector2 pos){rectTransform.localPosition=pos;}
        public Vector2 GetPos(){return pos;}
    }
}
