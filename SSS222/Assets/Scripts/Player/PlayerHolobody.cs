using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerHolobody : MonoBehaviour{
    [SerializeField] float timeToUncover=1.5f;
    [DisableInEditorMode]public int crystalsStored;
    [DisableInEditorMode]public Powerup powerupStored=null;
    [DisableInEditorMode][SerializeField]float timeLeft=-4;
    void Update(){
        if(Player.instance!=null){
            if(timeLeft>0&&GameSession.instance._noBreak()){timeLeft-=Time.deltaTime;}
            if(timeLeft<=timeToUncover&&timeLeft!=-4){Switch(true,true);}
        }
    }
    public void Switch(bool show=false,bool collectible=false){foreach(MonoBehaviour c in GetComponents<MonoBehaviour>()){
        if(c!=this&&c.GetType()!=typeof(Tag_Collectible)){c.enabled=show;}else if(c.GetType()==typeof(Tag_Collectible)){c.enabled=collectible;}}}
    public void SetTime(float time){timeLeft=time;}
    public float GetTimeLeft(){return timeLeft;}
    public string GetDistanceLeft(){return (Mathf.RoundToInt(timeLeft)*GameRules.instance.secondToDistanceRatio).ToString();}
}
