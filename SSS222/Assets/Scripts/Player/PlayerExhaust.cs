using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExhaust : MonoBehaviour{
    [Sirenix.OdinInspector.SceneObjectsOnly][SerializeField]GameObject exhaustColliderObj;
    void Start(){
        //exhaustColliderObj.transform.localPosition=GetComponent<TrailVFX>().trailVFX.transform.localPosition;
    }
    void Update(){
        if(GameRules.instance.levelingOn&&UpgradeMenu.instance!=null){
                if(((Player.instance.GetComponent<PlayerModules>().shipLvl<Player.instance.bflameDmgTillLvl||Player.instance.bflameDmgTillLvl==-5))
                //||(Player.instance.GetComponent<PlayerModules>().shipLvl>=Player.instance.bflameDmgTillLvl&&Player.instance.bflameDmgTillLvl>0))
                &&(!exhaustColliderObj.activeSelf)){
                    exhaustColliderObj.SetActive(true);
                }else if(((Player.instance.GetComponent<PlayerModules>().shipLvl>=Player.instance.bflameDmgTillLvl)||Player.instance.bflameDmgTillLvl==0)
                //||(Player.instance.GetComponent<PlayerModules>().shipLvl<=Player.instance.bflameDmgTillLvl))
                &&(exhaustColliderObj.activeSelf)){
                    exhaustColliderObj.SetActive(false);
                }
        }
    }
    public void DestroyExhaust(){Destroy(exhaustColliderObj);Destroy(this);}
}
