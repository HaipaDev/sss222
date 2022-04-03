using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class BGManager : MonoBehaviour{
    [SerializeField]Material defMat;
    [DisableInEditorMode][SerializeField]Material curMat;
    [SerializeField]Texture2D text;
    IEnumerator Start(){
        /*if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Game"){*/yield return new WaitForSecondsRealtime(0.075f);//}else{}

        SetColorMat(defMat);
        if(SceneManager.GetActiveScene().name!="Loading"){
            if(GameRules.instance!=null){if(GameRules.instance.bgMaterial!=null){SetColorMat(GameRules.instance.bgMaterial);}
                else{SetColorMat(defMat);}
            }else{SetColorMat(defMat);}
        }
    }
    void OnValidate(){SetColorMat(defMat);UpdateColorMat();}
    void Update(){
        if(SceneManager.GetActiveScene().name=="SandboxMode"){if(GameRules.instance.bgMaterial!=null){SetColorMat(GameRules.instance.bgMaterial);}}
    }
    public void SetColorMat(Material mat){curMat=mat;UpdateColorMat();}
    public void UpdateColorMat(){
        foreach(Tag_BGColor t in transform.GetComponentsInChildren<Tag_BGColor>()){
            t.GetComponent<Renderer>().material=curMat;
        }
    }
    public Texture2D GetBgTexture(){return text;}
}