using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotatePointButton : MonoBehaviour, IDragHandler{
    [SerializeField] RectTransform objectToRotate;
    [SerializeField] RectTransform pivotObject;
    [SerializeField] float initialOffset = -90f;//-90 if its directly above
    [SerializeField] bool roundCur = true;
    [SerializeField] bool invert = false;

    Vector3 pivotPoint;
    public Quaternion currentQuaternion;
    public float currentAngle;
    public float currentAngleNormalized;

    void Start(){
        if(pivotObject==null){pivotObject=transform.parent.GetComponent<RectTransform>();Debug.LogWarning("pivotObject is null! Setting to parent");}
        pivotObject.localPosition=objectToRotate.localPosition;
        pivotObject.localScale=objectToRotate.localScale;
        pivotPoint=objectToRotate.position;
        pivotPoint=pivotObject.TransformPoint(pivotObject.rect.center);
    }

    public void OnDrag(PointerEventData eventData){
        Vector3 mousePos = eventData.position;
        Vector3 dir = mousePos - pivotPoint;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        UpdateAngles(Quaternion.AngleAxis(angle + initialOffset, Vector3.forward));
    }
    public void UpdateAngles(Quaternion _q){
        currentQuaternion=_q;
        currentAngleNormalized=currentQuaternion.z;
        pivotObject.rotation=currentQuaternion;

        currentAngle=pivotObject.eulerAngles.z;
        if(roundCur)currentAngle=(float)System.Math.Round(pivotObject.eulerAngles.z,2);
    }
    public void UpdateAnglesZ(float z){
        currentQuaternion=Quaternion.Euler(0,0,z);
        currentAngleNormalized=currentQuaternion.z;
        pivotObject.rotation=currentQuaternion;

        currentAngle=pivotObject.eulerAngles.z;
        if(roundCur)currentAngle=(float)System.Math.Round(pivotObject.eulerAngles.z,2);
    }

    void Update(){
        //pivotPoint=objectToRotate.position;
        pivotPoint=pivotObject.TransformPoint(pivotObject.rect.center);
        if(!invert){objectToRotate.rotation=currentQuaternion;}
        else{objectToRotate.rotation=Quaternion.Inverse(currentQuaternion);}
    }
}