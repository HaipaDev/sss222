using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour{
    [SerializeField] float bgFarSpeed = 0.2f;
    Material myMat;
    Vector2 offSet;
    void Start(){
        myMat=GetComponent<Renderer>().material;
    }

    void Update(){
        offSet=new Vector2(0f,bgFarSpeed);
        //#if !UNITY_EDITOR
        if(!GameSession.GlobalTimeIsPaused)
        //#endif
            myMat.mainTextureOffset+=offSet*Time.deltaTime;
    }
}
