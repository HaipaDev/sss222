using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class BGManager : MonoBehaviour{
    ShaderMatProps _shaderMatPropsDef=new ShaderMatProps();
    [DisableInEditorMode][SerializeField]ShaderMatProps shaderMatProps;
    [DisableInEditorMode][SerializeField]Material material;
    [SerializeField]Material _materialDef;
    [SerializeField]Texture2D text;
    [DisableInPlayMode][SerializeField]bool setOnValidate;
    IEnumerator Start(){
        /*if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Game"){*/yield return new WaitForSecondsRealtime(0.075f);//}else{}

        SetStartingProperties();
        if(SceneManager.GetActiveScene().name!="Loading"){
            if(GameRules.instance!=null){if(GameRules.instance.bgMaterial!=null){SetMatProps(GameRules.instance.bgMaterial);}
                else{SetMatProps(_shaderMatPropsDef);}
            }else{SetMatProps(_shaderMatPropsDef);}
        }
    }
    void OnValidate(){if(setOnValidate){
        SetStartingProperties();
    }}
    void SetStartingProperties(){
        _shaderMatPropsDef.text=text;
        shaderMatProps.text=text;
        if(_materialDef==null)_materialDef=GetBgMat();
        if(material==null&&_materialDef!=null)material=_materialDef;
        SetMatProps(_shaderMatPropsDef);
    }
    void Update(){
        if(SceneManager.GetActiveScene().name=="SandboxMode"){if(GameRules.instance.bgMaterial!=null){SetMatProps(GameRules.instance.bgMaterial);}}
    }
    public void SetMatProps(ShaderMatProps mat){shaderMatProps=mat;if(material!=null){material=GameAssets.instance.UpdateShaderMatProps(material,shaderMatProps);}UpdateMaterials();}
    public void UpdateMaterials(){foreach(Tag_BGColor t in transform.GetComponentsInChildren<Tag_BGColor>()){if(material!=null)t.GetComponent<Renderer>().sharedMaterial=material;}}
    public Texture2D GetBgTexture(){return text;}
    public Material GetBgMat(){return transform.GetComponentInChildren<Tag_BGColor>().GetComponent<Renderer>().sharedMaterial;}
}