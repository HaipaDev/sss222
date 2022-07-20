using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgePlayerWeaps : MonoBehaviour{
    public float distMin=1.6f;
    public float dodgeSpeed=2f;
    public float dodgeTime=0.5f;
    public float dodgeTimer;
    public int dodgeDir=1;
    [Header("Detecting Params")]
    public Vector2 selfPos;
    public Vector2 targetPos;
    public float dist;
    public Tag_PlayerWeapon closestWeapon;
    Rigidbody2D rb;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float padding=0.5f;
    void Awake(){
    //Set Values
    var i=GameRules.instance;
    if(GetComponent<HealingDrone>()!=null&&i!=null){
        var e=i.healingDroneSettings;
        distMin=e.distMin;
        dodgeSpeed=e.dodgeSpeed;
        dodgeTime=e.dodgeTime;
        dodgeTime=e.dodgeTime;
    }
    }
    void Start(){  
        rb=GetComponent<Rigidbody2D>();
        SetUpMoveBoundaries();
    }

    void Update(){
        selfPos = new Vector2(transform.position.x, transform.position.y);

        //closestWeapon=Weapons.FindClosest(transform.position);
        if(FindClosestWeapon()!=null){closestWeapon=FindClosestWeapon();
            targetPos = new Vector2(closestWeapon.transform.position.x, closestWeapon.transform.position.y);
            dist=Vector2.Distance(targetPos, selfPos);
        }else{
            dist=0f;
        }
        ChangeDir();

        Dodge();

        if(GetComponent<PointPathing>().waypointIndex>0){var clamp=true;if(clamp==true)ClampPosition();}
    }

    void Dodge(){
        if(dist<=distMin && dist>0){
            rb.velocity=new Vector2(dodgeSpeed*dodgeDir,0f);
            dodgeTimer=dodgeTime;
        }
        if(dodgeTimer>0f)dodgeTimer-=Time.deltaTime;
        if(dodgeTimer<=0)rb.velocity=Vector2.zero;
    }

    void ChangeDir(){
        if(targetPos.x>selfPos.x){dodgeDir=-1;}
        if(targetPos.x<selfPos.x){dodgeDir=1;}
    }
    private void SetUpMoveBoundaries(){
        xMin = -3.87f + padding;
        xMax = 3.87f - padding;
        yMin = -6.95f + padding;
        yMax = 7f - padding;
    }
    void ClampPosition(){
        var newXpos = Mathf.Clamp(transform.position.x, xMin, xMax);
        var newYpos = Mathf.Clamp(transform.position.y, yMin, yMax);
        transform.position = new Vector2(newXpos, newYpos);
    }
    public Tag_PlayerWeapon FindClosestWeapon(){
        KdTree<Tag_PlayerWeapon> Weapons = new KdTree<Tag_PlayerWeapon>();
        Tag_PlayerWeapon[] WeaponsArr;
        WeaponsArr = FindObjectsOfType<Tag_PlayerWeapon>();
        foreach(Tag_PlayerWeapon weapon in WeaponsArr){
            Weapons.Add(weapon);
        }
        Tag_PlayerWeapon closest = Weapons.FindClosest(transform.position);
        return closest;
    }
}
