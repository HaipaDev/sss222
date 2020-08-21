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
    Tag_PlayerWeaponBlockable[] WeapsArr;
    KdTree<Tag_PlayerWeaponBlockable> Weaps;
    public float chargeEnergy;
    public float timer=-4;
    
    float timeToDie;
    Sprite spr;
    void Awake(){
    //Set Values
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
    }

    void Update(){
        Weaps=FindAllLaserWeapons();
        Discharge();
        GetComponent<SpriteRenderer>().sprite=spr;
        /*WeapsArr = FindObjectsOfType<Tag_PlayerWeaponBlockable>();
        foreach(Tag_PlayerWeaponBlockable weap in WeapsArr){
            chargeEnergy++;
            WeapsArr.RemoveAll(enemy);
        }*/
    }
    public KdTree<Tag_PlayerWeaponBlockable> FindAllLaserWeapons(){
        KdTree<Tag_PlayerWeaponBlockable> Weaps = new KdTree<Tag_PlayerWeaponBlockable>();
        Tag_PlayerWeaponBlockable[] WeapsArr;
        WeapsArr = FindObjectsOfType<Tag_PlayerWeaponBlockable>();
        //List<string> sublist = Enemies.FindAll(isHealingDrone);
        foreach(Tag_PlayerWeaponBlockable weap in WeapsArr){
            if(weap.charged==false){Weaps.Add(weap);
            //chargeEnergy++;
            chargeEnergy+=weap.energy;
            weap.charged=true;
                if(timer==-4){timer=startTimer;}
                else{if(timer>-4){
                    if(weap.energy>1.5){timer-=weap.energy*chargeMultip;}else{timer-=weap.energy*chargeMultipS;}
                    }
                }
            }
            //if(enemy.GetComponent<HealingDrone>()!=null){Enemies.RemoveAt(System.Array.IndexOf(EnemiesArr, this));}
            //Debug.Log(System.Array.IndexOf(EnemiesArr, this));
            //Enemies.Find(this.GetComponent<HealingDrone>() healDrone);
            //Enemies.RemoveAll(this.GetComponent<HealingDrone>() healDrone);
        }
        //Tag_PlayerWeaponBlockable closest = Weaps.FindClosest(transform.position);
        return Weaps;
    }

    void Discharge(){
        if(Time.timeScale>0.0001f)chargeEnergy+=0.01f;
        //if(timer>0)
        if(timer>-4)timer+=Time.deltaTime;
        if(timer<-4)timer=-4;
        //if(timer<=0 && timer>-4){StartCoroutine(Die());}
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
}
