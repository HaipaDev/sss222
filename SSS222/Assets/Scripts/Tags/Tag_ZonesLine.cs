using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class Tag_ZonesLine : MonoBehaviour{
    public void SetPoints(int zoneId1, int zoneId2){
        var z1Pos=CoreSetup.instance.adventureZones[zoneId1].pos;
        var z2Pos=CoreSetup.instance.adventureZones[zoneId2].pos;
        Debug.Log(zoneId1 +" = "+ z1Pos + " | " + zoneId2 +" = "+ z2Pos);
        GetComponent<UILineRenderer>().Points[0]=new Vector2(z1Pos.x,z1Pos.y);
        GetComponent<UILineRenderer>().Points[1]=new Vector2(z2Pos.x,z2Pos.y);
    }
    public void SetPointsDirect(Vector2 point1, Vector2 point2){
        GetComponent<UILineRenderer>().Points[0]=new Vector2(point1.x,point1.y);
        GetComponent<UILineRenderer>().Points[1]=new Vector2(point2.x,point2.y);
    }
    public void SetBothPointsNull(){
        GetComponent<UILineRenderer>().Points[0]=Vector2.zero;
        GetComponent<UILineRenderer>().Points[1]=Vector2.zero;
    }
    public Vector2 GetPoint(int id){return GetComponent<UILineRenderer>().Points[id];}
    public bool BothPointsNull(){return GetPoint(0)==Vector2.zero&&GetPoint(1)==Vector2.zero;}
}