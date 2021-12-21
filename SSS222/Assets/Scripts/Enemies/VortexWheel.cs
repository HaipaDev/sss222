using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexWheel : MonoBehaviour{
    [Header("Properties")]
    [SerializeField] public float startTimer=3f;
    [SerializeField] public float timeToDieMin=8f;
    [SerializeField] public float timeToDieMax=13f;
    [SerializeField] public float chargeMultip=0.8f;
    [SerializeField] public float chargeMultipS=1.3f;
    [SerializeField] Sprite[] sprites;
    [Header("Values")]
    Tag_PlayerWeapon[] WeapsArr;
    KdTree<Tag_PlayerWeapon> Weaps;
    public float timer=-4;
    
    float timeToDie;
    Sprite spr;
    void Awake(){
        var i=GameRules.instance;
        if(i!=null){
            var e=i.vortexWheelSettings;
            startTimer=e.startTimer;
            chargeMultip=e.chargeMultip;
            chargeMultipS=e.chargeMultipS;
        }
    }
    void Start(){
        spr=GetComponent<SpriteRenderer>().sprite;
        timeToDie=UnityEngine.Random.Range(timeToDieMin,timeToDieMax);
        if(timer==-4){timer=startTimer;}
    }

    void Update(){
        if(!GameSession.GlobalTimeIsPaused){
            Weaps=FindAllLaserWeapons();
            Discharge();
            GetComponent<SpriteRenderer>().sprite=spr;
        }
    }
    public KdTree<Tag_PlayerWeapon> FindAllLaserWeapons(){
        KdTree<Tag_PlayerWeapon> Weaps = new KdTree<Tag_PlayerWeapon>();
        //List<string> sublist = Enemies.FindAll(isHealingDrone);
        foreach(Tag_PlayerWeapon weap in FindObjectsOfType<Tag_PlayerWeapon>()){
            if(weap.charged==false){Weaps.Add(weap);
            
                if(timer==-4){timer=startTimer;}
                else{if(timer>-4){
                    if(weap.energy>1.5){timer-=weap.energy*chargeMultip;}else{timer-=weap.energy*chargeMultipS;if(timer<-4){timer=0;Shoot();}}
                    }
                }
            }
            weap.charged=true;
        }
        return Weaps;
    }

    void Discharge(){
        if(timer>-4)timer+=Time.deltaTime;
        if(timer>timeToDie){StartCoroutine(Die());}
        
        if(timer<=0 && timer>-4){StartCoroutine(Shoot());}
        if(timer<=1 && timer>0)spr=sprites[4];
        if(timer>=1 && timer<1.5)spr=sprites[3];
        if(timer>=1.5 && timer<2)spr=sprites[2];
        if(timer>=2.5 && timer<3)spr=sprites[1];
        if(timer>=3 || timer==-4)spr=sprites[0];
    }

    IEnumerator Die(){
        timer=-5;
        GetComponent<CircleCollider2D>().enabled=false;
        spr=sprites[7];
        AudioManager.instance.Play("VortexDie");
        yield return new WaitForSeconds(1.65f);
        if(GetComponent<ParticleDelay>()!=null)GetComponent<ParticleDelay>().on=true;
        var enemy=GetComponent<Enemy>();enemy.health=-1;enemy.Die();
    }

    IEnumerator Shoot(){
        timer=-5;
        spr=sprites[5];
        AudioManager.instance.Play("VortexCharge");
        var angle=0;
        //if(angle<360)angle+=15;
        for(var i=angle;i<360;i+=15){
            angle=i;
            yield return new WaitForSeconds(0.01f);
            transform.Rotate(0.0f, 0.0f, angle, Space.Self);
        }
        yield return new WaitForSeconds(0.33f);
        GetComponent<LaunchRadialBullets>().Shoot();
        yield return new WaitForSeconds(0.33f);
        spr=sprites[6];
        yield return new WaitForSeconds(0.66f);
        timer=startTimer;
    }
    public float GetTimeToDie(){return timeToDie;}
}
