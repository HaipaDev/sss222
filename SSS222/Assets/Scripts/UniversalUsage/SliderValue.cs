using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SliderValue : MonoBehaviour{
    [SerializeField] public string value="masterVolume";
    [DisableInPlayMode][SerializeField] bool onlyOnEnable=false;
    [HideInPlayMode][SerializeField] bool onValidate=false;
    void Start(){if(onlyOnEnable)ChangeSlider();}
    void OnEnable(){if(onlyOnEnable)ChangeSlider();}
    void OnValidate(){if(onValidate)ChangeSlider();}
    void Update(){if(!onlyOnEnable)ChangeSlider();}

    void ChangeSlider(){
        float _val=0f;
        var s=SaveSerial.instance;  var ss=SaveSerial.instance.settingsData;
        SettingsMenu sm=null;if(SettingsMenu.instance!=null)sm=SettingsMenu.instance;
        var gr=GameRules.instance;var sb=SandboxCanvas.instance;var saveInfo=sb.saveInfo;
        if(gr!=null){
            switch(value){
                case "powerupsCapacity":_val=gr.powerupsCapacity;break;
                case "gameSpeed":_val=gr.defaultGameSpeed;break;
                case "bgHueGR":_val=gr.bgMaterial.hue;break;
                case "bgSaturGR":_val=gr.bgMaterial.saturation;break;
                case "bgValueGR":_val=gr.bgMaterial.value;break;
                case "bgNegativeGR":_val=gr.bgMaterial.negative;break;

                
                case "sprMatHue_Icon":_val=saveInfo.iconShaderMatProps.hue;break;
                case "sprMatSatur_Icon":_val=saveInfo.iconShaderMatProps.saturation;break;
                case "sprMatValue_Icon":_val=saveInfo.iconShaderMatProps.value;break;
                case "sprMatNegative_Icon":_val=saveInfo.iconShaderMatProps.negative;break;
                case "sprMatPixelate_Icon":_val=saveInfo.iconShaderMatProps.pixelate;break;
                case "sprMatBlur_Icon":_val=saveInfo.iconShaderMatProps.blur;break;

                case "sprMatHue_Player":_val=gr.playerShaderMatProps.hue;break;
                case "sprMatSatur_Player":_val=gr.playerShaderMatProps.saturation;break;
                case "sprMatValue_Player":_val=gr.playerShaderMatProps.value;break;
                case "sprMatNegative_Player":_val=gr.playerShaderMatProps.negative;break;
                case "sprMatPixelate_Player":_val=gr.playerShaderMatProps.pixelate;break;
                case "sprMatBlur_Player":_val=gr.playerShaderMatProps.blur;break;
            }
            if(sb!=null){
                switch(value){
                    case "sprMatHue_EnemySB":_val=sb._enModSprMat().hue;break;
                    case "sprMatSatur_EnemySB":_val=sb._enModSprMat().saturation;break;
                    case "sprMatValue_EnemySB":_val=sb._enModSprMat().value;break;
                    case "sprMatNegative_EnemySB":_val=sb._enModSprMat().negative;break;
                    case "sprMatPixelate_EnemySB":_val=sb._enModSprMat().pixelate;break;
                    case "sprMatBlur_EnemySB":_val=sb._enModSprMat().blur;break;
                }
            }
        }
        GetComponent<Slider>().value=_val;
    }
}
