using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ScreenflashImg : MonoBehaviour{
    [SerializeField] Sprite sprite;
    [SerializeField] Color color;
    [SerializeField] float speed;
    Image img;
    //bool setup;
    void Start(){
        img=GetComponent<Image>();
        img.color=color;
    }

    void Update(){
        var _colorClear=new Color(color.r,color.g,color.b,0);
        if(SaveSerial.instance.settingsData.screenflash&&Player.instance!=null){
            img.color=Color.Lerp(img.color, _colorClear, speed*Time.deltaTime);
            if(img.color.a<=0.01f/*&&setup*/){Destroy(gameObject);}
        }
    }
    public void Setup(Sprite spr,Color _color,float _speed){speed=_speed;  color=_color;     if(spr!=null){sprite=spr;img.sprite=sprite;}}//setup=true;}
}
