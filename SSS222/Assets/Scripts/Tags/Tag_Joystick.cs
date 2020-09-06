using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_Joystick : MonoBehaviour{
    [SerializeField] JoystickType joystickType;
    void Start(){
        joystickType=SaveSerial.instance.joystickType;
        GetComponent<VariableJoystick>().SetMode(joystickType);
    }

    void Update(){
        var size=SaveSerial.instance.joystickSize;
        transform.GetChild(0).localScale=new Vector2(size,size);
    }
}
