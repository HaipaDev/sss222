using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class Tag_ZonesLine : MonoBehaviour{
    public void SetPoints(int zoneId1, int zoneId2,bool debugPoints=false){
        var z1Pos=CoreSetup.instance.adventureZones[zoneId1].pos;
        var z2Pos=CoreSetup.instance.adventureZones[zoneId2].pos;
        //if(debugPoints)Debug.Log(zoneId1 +" = "+ z1Pos + " | " + zoneId2 +" = "+ z2Pos);
        GetComponent<UILineRenderer>().Points[0]=new Vector2(z1Pos.x,z1Pos.y);
        GetComponent<UILineRenderer>().Points[1]=new Vector2(z2Pos.x,z2Pos.y);
        GetComponent<UILineRenderer>().SetAllDirty();
    }
    public void SetPointsDirect(Vector2 point1, Vector2 point2){
        GetComponent<UILineRenderer>().Points[0]=new Vector2(point1.x,point1.y);
        GetComponent<UILineRenderer>().Points[1]=new Vector2(point2.x,point2.y);
        GetComponent<UILineRenderer>().SetAllDirty();
    }
    public void SetBothPointsNull(){
        //Debug.Log("SetBothPointsNull()");
        GetComponent<UILineRenderer>().Points[0]=Vector2.zero;
        GetComponent<UILineRenderer>().Points[1]=Vector2.zero;
        GetComponent<UILineRenderer>().SetAllDirty();
    }
    public Vector2 GetPoint(int id){return GetComponent<UILineRenderer>().Points[id];}
    public Vector2 GetPosFromZoneId(int id){return CoreSetup.instance.adventureZones[id].pos;}
    public bool BothPointsNull(){return GetPoint(0)==Vector2.zero&&GetPoint(1)==Vector2.zero;}
}