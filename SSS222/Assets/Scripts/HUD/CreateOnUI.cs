using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOnUI : MonoBehaviour{
    public static GameObject CreateOnUIFunc(GameObject obj, Vector2 position){
        Tag_GameCanvas canvas = FindObjectOfType<Tag_GameCanvas>();
        GameObject childObject = Instantiate(obj,Camera.main.WorldToScreenPoint(position),Quaternion.identity,canvas.transform);
        //childObject.transform.parent = canvas.transform;
        childObject.transform.SetParent(canvas.transform);
        childObject.transform.position=Camera.main.WorldToScreenPoint(position);
        return childObject;
    }
}
