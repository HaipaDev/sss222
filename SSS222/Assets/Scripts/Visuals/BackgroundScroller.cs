using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour{
    [SerializeField] float bgFarSpeed = 0.2f;
    Material myMat;
    Vector2 offSet;
    // Start is called before the first frame update
    void Start(){
        myMat = GetComponent<Renderer>().material;
        offSet = new Vector2(0f,bgFarSpeed);
    }

    // Update is called once per frame
    void Update(){
        myMat.mainTextureOffset += offSet * Time.deltaTime;
    }
}
