using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FlipRotateAnim : MonoBehaviour{
    [SerializeField] bool horiz=true;
    [ShowIf("horiz")][SerializeField] float rotateSpeedH=20;
    [SerializeField] bool vert=false;
    [ShowIf("vert")][SerializeField] float rotateSpeedV=20;
    [SerializeField] bool backAndForth=false;
    int flipMultY=1,flipMultX=1;

    GameObject go=null;
    Transform sprT=null;
    void Awake(){
        if(go==null){
            var _eGo=new GameObject("SpriteRenderer");
            go=Instantiate(_eGo,transform);go.transform.parent=transform;Destroy(_eGo);
            go.AddComponent<SpriteRenderer>();
            go.GetComponent<SpriteRenderer>().sprite=GetComponent<SpriteRenderer>().sprite;
            Destroy(GetComponent<SpriteRenderer>());

            sprT=go.transform;
        }
    }
    void Update(){
        if(!GameManager.GlobalTimeIsPausedNotSlowed){if(sprT!=null){
            if(backAndForth){
                if(horiz){if(sprT.rotation.y>=0.99&&flipMultY!=-1){flipMultY=-1;}if(sprT.rotation.y<=0.5&&flipMultY!=1){flipMultY=1;}}
                if(vert){if(sprT.rotation.x>=0.99&&flipMultX!=-1){flipMultX=-1;}if(sprT.rotation.x<=0.5&&flipMultX!=1){flipMultX=1;}}
            }else{flipMultX=1;flipMultY=1;}
            Debug.Log(sprT.rotation.y+" | "+flipMultY);
            sprT.Rotate(new Vector3(
                (AssetsManager.BoolToInt(vert)*flipMultX*(rotateSpeedV*10)*Time.deltaTime),
                (AssetsManager.BoolToInt(horiz)*flipMultY*(rotateSpeedH*10)*Time.deltaTime),
            0),Space.Self);
            //Debug.Log((AssetsManager.BoolToInt(horiz)*flipMultY*(rotateSpeedH*10)*Time.deltaTime));
        }}
    }
}
