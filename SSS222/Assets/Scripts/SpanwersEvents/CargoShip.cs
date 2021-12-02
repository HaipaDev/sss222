using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoShip : MonoBehaviour{
    dir cargoDirSpawn=dir.down;
    [SerializeField] float speed=2;
    [SerializeField] float healthStart=44;
    public float health;
    [SerializeField] public int[] repMinusHit=new int[2]{1,3};
    [SerializeField] public int repMinusKill=7;
    public bool visited;
    bool[] tagged=new bool[2];
    bool shieldOn=true;
    void Awake(){
        //Set values
        var i=GameRules.instance;
        speed=i.cargoSpeed;
        healthStart=i.cargoHealth;
        health=healthStart;
        repMinusHit=i.repMinusCargoHit;
        repMinusKill=i.repMinusCargoKill;
    }
    void Start(){
        TurnShieldOn();
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")&&visited==false){
            Shop.shopOpen=true;
            visited=true;
            //GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if(other.CompareTag("PlayerWeapons")&&other.GetComponent<HealthLeech>()==null){
            float dmg=UniCollider.TriggerCollision(other,transform,new List<colliTypes>(){colliTypes.playerWeapons});
            if(shieldOn)dmg*=0.75f;
            if(dmg!=0)health-=dmg;

            UniCollider.DMG_VFX(0,other,transform,dmg,ColorInt32.dmgPlayerColor);
        }else if(other.GetComponent<HealthLeech>()!=null){Shop.instance.reputation+=1;}
        if(other.GetComponent<Shredder>()!=null){Destroy(gameObject);}
    }
    void Update(){
        if(health>=healthStart/2&&health!=healthStart){if(tagged[0]==false){Shop.instance.RepChange(repMinusHit[0],false);tagged[0]=true;}}
        //if(health<=healthStart/2){if(tagged[1]==false){Shop.instance.RepChange(repMinusHit[1],false);tagged[1]=true;}}
        /*if(health<=healthStart/3)*/else if(health<=healthStart/2){TurnShieldOff();if(tagged[1]==false){Shop.instance.RepChange(repMinusHit[1],false);tagged[1]=true;}}

        if(health<=0){
            AudioManager.instance.Play("Explosion");
            GameObject explosion=GameAssets.instance.VFX("Explosion",transform.position,0.5f);
            Shake.instance.CamShake(2,1);
            Destroy(gameObject,0.01f);
            GameAssets.instance.MakeSpread("CoinB",transform.position,5);
            Shop.instance.RepChange(repMinusKill,false);
        }
    }
    void OnDestroy(){
        if(visited==false&&Shop.instance.subbed==false){
            Shop.instance.purchasedNotTimes++;
            Shop.instance.subbed=true;
        }
    }

    void TurnShieldOn(){
        if(transform.GetChild(0).gameObject.activeSelf==false){
            transform.GetChild(0).gameObject.SetActive(true);
            shieldOn=true;
        }
    }
    void TurnShieldOff(){
        if(transform.GetChild(0).gameObject.activeSelf==true){
            transform.GetChild(0).gameObject.SetActive(false);
            shieldOn=false;
            AudioManager.instance.Play("CargoHit");
        }
    }
    public void SetCargoSpawnDir(dir dir){StartCoroutine(SetCargoSpawnDirI(dir));}
    IEnumerator SetCargoSpawnDirI(dir dir){
        cargoDirSpawn=dir;
        float hspeed=0;
        float vspeed=0;
        float zRot=0;
        //GameObject pt=GameAssets.instance.GetVFX("BFlameUp");
        //float ptRot=180;
        //float ptxx=0.3f;
        //float ptyy=0.5f;
        switch(cargoDirSpawn){
            case dir.up:vspeed=-speed;zRot=0;break;
            case dir.down:vspeed=speed;zRot=180;break;
            case dir.left:hspeed=speed;zRot=90;break;
            case dir.right:hspeed=-speed;zRot=-90;break;
        }
        //foreach(BackflameEffect co in GetComponents<BackflameEffect>()){co.part=pt;}//co.BFlame.transform.eulerAngles=new Vector3(0,0,ptRot);}//co.xx=ptxx;co.yy=ptyy;}
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().velocity=new Vector2(hspeed,vspeed);
        transform.eulerAngles=new Vector3(transform.rotation.x,transform.rotation.y,zRot);
    }
}