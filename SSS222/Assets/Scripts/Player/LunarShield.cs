using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LunarShield : MonoBehaviour{
    [SerializeField] float startHp=25;
    [SerializeField] float rotSpeed=1;
    [Range(0,8)][SerializeField] public int fragmentsStart=8;
    [ReadOnly][SerializeField] public List<LunarShield_fragment> fragments;
    void Start(){
        foreach(Transform t in transform){
            fragments.Add(t.gameObject.AddComponent<LunarShield_fragment>());
            t.GetComponent<LunarShield_fragment>().hp=startHp;
        }
    }
    float _colliDelay;
    void Update(){
        if(_colliDelay>0){_colliDelay-=Time.deltaTime;}
        float rotStep=rotSpeed*Time.deltaTime;
        if(!GameSession.GlobalTimeIsPaused)transform.Rotate(new Vector3(0,0,rotSpeed));
        
        if(transform.childCount!=fragmentsStart){
            for(var i=transform.childCount;i>fragmentsStart;i--){
                fragments.Remove(transform.GetChild(i-1).GetComponent<LunarShield_fragment>());
                Destroy(transform.GetChild(i-1).gameObject);
            }
        }

        foreach(LunarShield_fragment l in fragments){
            if(l.hp!=startHp&&l.hp>0)Debug.Log(l.hp/startHp);//GameAssets.Normalize(l.hp,0f,startHp));
            l.GetComponent<SpriteRenderer>().color=new Color(1,1,1,(l.hp/startHp));
            if(l.hp<=0){l.gameObject.SetActive(false);}
        }
        if(_allPiecesDestroyed()){Destroy(gameObject);}
    }
    public int _damagedShieldPiecesCount(){var i=0;foreach(LunarShield_fragment l in fragments){if(l.hp<=startHp*0.75f){i++;}}return i;}
    public int _notDamagedShieldPiecesCount(){return fragments.Count-_damagedShieldPiecesCount();}
    public int _fullShieldPiecesCount(){var i=fragments.Count;foreach(LunarShield_fragment l in fragments){if(l.hp<=startHp){i--;}}return i;}
    public int _destroyedShieldPiecesCount(){var i=0;foreach(LunarShield_fragment l in fragments){if(l.hp<=0){i++;}}return i;}
    public bool _allPiecesDestroyed(){return _destroyedShieldPiecesCount()==fragments.Count;}//return !(fragments.Exists(x=>x.hp>0));}

    public void SetDelay(){_colliDelay=0.05f;}
    public bool _isNotDelayed(){return _colliDelay<=0;}
}

public class LunarShield_fragment : MonoBehaviour{
    public float hp;
    void OnTriggerEnter2D(Collider2D other){
        if(transform.parent.GetComponent<LunarShield>()._isNotDelayed()){
            float dmg=0;
            DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
            if(dmgVal!=null){
                dmg=UniCollider.TriggerCollision(other,transform,new List<colliTypes>(){colliTypes.enemyWeapons});
                hp-=dmg;
                if(!dmgVal.phase&&other.CompareTag("EnemyBullet")){Destroy(other.gameObject);}
                transform.parent.GetComponent<LunarShield>().SetDelay();
                //else if(other.name.Contains("Comet")){other.GetComponent<Enemy>().giveScore=false;other.GetComponent<Enemy>().Die();}
            }
        }
    }
}