using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_Joystick : MonoBehaviour{
    [SerializeField] JoystickType joystickType;
    [SerializeField] Vector2 fixedPosition;
    void Start(){
        joystickType=SaveSerial.instance.joystickType;
        var vj=GetComponent<VariableJoystick>();
        vj.fixedPosition=fixedPosition;
        if(FindObjectOfType<Player>()!=null){
            var p=FindObjectOfType<Player>();
            if(p.moveX&&p.moveY)vj.AxisOptions=AxisOptions.Both;
            else if(p.moveX&&!p.moveY)vj.AxisOptions=AxisOptions.Horizontal;
            else if(!p.moveX&&p.moveY)vj.AxisOptions=AxisOptions.Vertical;
        }
        vj.SetMode(joystickType);
        var size=SaveSerial.instance.joystickSize;
        transform.GetChild(0).localScale=new Vector2(size,size);
        Destroy(this);
    }

    void Update(){
        
    }
}
