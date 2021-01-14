using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_Joystick : MonoBehaviour{
    [SerializeField] JoystickType joystickType;
    [SerializeField] Vector2 fixedPosition;
    IEnumerator Start(){
        yield return new WaitForSeconds(0.05f);
        joystickType=SaveSerial.instance.settingsData.joystickType;
        var vj=GetComponent<VariableJoystick>();
        vj.fixedPosition=fixedPosition;
        if(SaveSerial.instance.settingsData.lefthand){vj.fixedPosition=new Vector2(-fixedPosition.x,fixedPosition.y);}
        if(FindObjectOfType<Player>()!=null){
            var p=FindObjectOfType<Player>();
            if(p.moveX&&p.moveY)vj.AxisOptions=AxisOptions.Both;
            else if(p.moveX&&!p.moveY)vj.AxisOptions=AxisOptions.Horizontal;
            else if(!p.moveX&&p.moveY)vj.AxisOptions=AxisOptions.Vertical;
        }
        vj.SetMode(joystickType);
        var size=SaveSerial.instance.settingsData.joystickSize;
        transform.GetChild(0).localScale=new Vector2(size,size);
        Destroy(this);
    }
}
