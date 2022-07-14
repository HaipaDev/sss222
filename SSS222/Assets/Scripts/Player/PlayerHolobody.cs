using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerHolobody : MonoBehaviour{
    [SerializeField] int secondToDistanceRatio=100;
    [SerializeField] float timeToUncover=1.5f;
    [DisableInEditorMode]public int crystalsStored;
    [DisableInEditorMode][SerializeField]float timeLeft=-4;
    void Update(){
        if(timeLeft>0){timeLeft-=Time.deltaTime;}
        if(timeLeft<=timeToUncover&&timeLeft!=-4){Switch(true,true);}
    }
    public void Switch(bool show=false,bool collectible=false){foreach(MonoBehaviour c in GetComponents<MonoBehaviour>()){
        if(c!=this&&c.GetType()!=typeof(Tag_Collectible)){c.enabled=show;}else if(c.GetType()==typeof(Tag_Collectible)){c.enabled=collectible;}}}
    public void SetTime(float time){timeLeft=time;}
}
