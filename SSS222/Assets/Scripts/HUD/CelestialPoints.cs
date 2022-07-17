using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CelestialPoints : MonoBehaviour{
    [ChildGameObjectsOnly][SerializeField] Transform container;
    [AssetsOnly][SerializeField] GameObject pointPrefab;
    [SerializeField] float defaultPointSize=150;
    [SerializeField] float morePointSize=92;
    [AssetsOnly][SerializeField] RuntimeAnimatorController ascendAnimController;
    
    void OnEnable(){RefreshCelestialPoints();}
    public void RefreshCelestialPoints(){
        var pm=Player.instance.GetComponent<PlayerModules>();
        foreach(Transform t in container){Destroy(t.gameObject);}
        for(var i=0;i<pm.lvlFractionsMax;i++){
            var go=Instantiate(pointPrefab,container);
            go.name="CelestialPoint_"+(i+1);
            if(pm.lvlFractionsMax>6){container.GetComponent<GridLayoutGroup>().cellSize=new Vector2(morePointSize,morePointSize);}
            else{container.GetComponent<GridLayoutGroup>().cellSize=new Vector2(defaultPointSize,defaultPointSize);}
            if((i+1)>pm.shipLvlFraction){go.GetComponent<Image>().material=GameAssets.instance.GetMat("GrayedOut");}
            if(i==pm.shipLvlFraction&&!pm._isAutoAscend()&&GameSession.instance.xp>=GameSession.instance.xpMax){
                var anim=go.AddComponent<Animator>();
                anim.updateMode=AnimatorUpdateMode.UnscaledTime;
                anim.runtimeAnimatorController=ascendAnimController;//anim.Play();
                var bt=go.AddComponent<Button>();
                bt.onClick.AddListener(()=>pm.Ascend());
                bt.onClick.AddListener(()=>RefreshCelestialPoints());
            }
        }
    }
}
