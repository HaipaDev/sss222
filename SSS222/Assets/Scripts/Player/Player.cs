using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;
//using UnityEditor;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour{    public static Player instance;
#region Vars
#region//Basic Player Values
[Header("Player")]
    [SerializeField] public bool moveX = true;
    [SerializeField] public bool moveY = true;
    [SerializeField] InputType inputType = InputType.mouse;
    [SerializeField] public bool autoShoot = false;
    [DisableInPlayMode] public float moveSpeedInit = 5f;
    [DisableInEditorMode]public float moveSpeedBase;
    [DisableInEditorMode]public float moveSpeedCurrent;
    [SerializeField]public ShaderMatProps shaderMatProps;
    [SerializeField]public float health = 100f;
    [HideInEditorMode][SerializeField]public float healthStart;
    [SerializeField] public float healthMax = 100f;
    [DisableInPlayMode] public int defenseInit = 0;
    [DisableInEditorMode] public int defenseBase = 0;
    [DisableInEditorMode] public int defense = 0;
    [SerializeField] public bool energyOn = true;
    [DisableInEditorMode]public float energy = 120f;
    [SerializeField] public float energyMax = 120f;
    [Range(1,10)][DisableInPlayMode] public int powerupsCapacityInit=5;
    [Range(1,10)][DisableInEditorMode] public int powerupsCapacity;
    [SerializeField] public List<Powerup> powerups;
    [SerializeField] public int powerupCurID=0;
    [SerializeField] public string powerupDefault="laser";
    [SerializeField] public bool weaponsLimited=false;
    [SerializeField] public bool losePwrupOutOfEn;
    [SerializeField] public bool losePwrupOutOfAmmo;
    [SerializeField] public int energyRefillUnlocked;
    [SerializeField] public bool overheatOn=true;
    [DisableInEditorMode]public float overheatTimer = -4f;
    [SerializeField] public float overheatTimerMax = 8.66f;
    [SerializeField] public float overheatCdTimer;
    [SerializeField] public float overheatCooldown = 0.65f;
    [DisableInEditorMode]public bool overheated;
    [SerializeField] public float overheatedTime=3;
    [DisableInEditorMode]public float overheatedTimer;
    [SerializeField] public bool recoilOn=true;
    [DisableInPlayMode] public float critChanceInit=4f;
    [DisableInEditorMode] public float critChanceBase;
    [DisableInEditorMode] public float critChance;

    public bool enRegenEnabled;
    public float enAbsorpAmnt;
    public float energyDissAbsorp=18;
    [DisableInEditorMode]public float timerEnRegen;
    [SerializeField] public float freqEnRegen=1f;
    [SerializeField] public float enRegenAmnt=2f;
    [SerializeField] public float energyForRegen=10f;
    public bool hpRegenEnabled;
    public float hpAbsorpAmnt;
    public float crystalMendAbsorp=10;
    [DisableInEditorMode]public float timerHpRegen;
    [SerializeField] public float freqHpRegen=2f;
    [SerializeField] public float hpRegenAmnt=0.5f;
    [DisableInPlayMode] public float dmgMultiInit=1f;
    [DisableInEditorMode] public float dmgMultiBase=1f;
    [DisableInEditorMode] public float dmgMulti=1f;
    [DisableInPlayMode] public float shootMultiInit=1f;
    [DisableInEditorMode] public float shootMultiBase=1f;
    [DisableInEditorMode] public float shootMulti=1f;
    [SerializeField] public float shipScaleDefault=0.89f;
    [SerializeField] public bool bulletResize;
#endregion
#region//Statuses
[Header("Statuses")]
    [SerializeField] public List<StatusFx> statuses=new List<StatusFx>();
    [DisableInEditorMode] public List<StatusFx> _statusesPersistent=new List<StatusFx>();
    public float dashTime;
    public float infPrevEnergy;
    public List<float> speedPrev=new List<float>(1);
    [SerializeField] public float flipTime=7f;
    [SerializeField] public float gcloverTime=6f;
    [SerializeField] public float shadowTime=10f;
    [SerializeField] public float shadowLength=0.33f;
    [SerializeField] public float shadowtracesSpeed=1.3f;
    [SerializeField] public float dashSpeed=10f;
    [SerializeField] public float startDashTime=0.2f;
    [SerializeField] public float inverterTimeMax=10f;
    [SerializeField] public float magnetTime=15f;
    [SerializeField] public float scalerTime=15f;
    [SerializeField] public float[] scalerSizes={0.45f,0.75f,1.2f,1.5f,1.75f,2f,2.5f};
    //[SerializeField] public float shipScaleMin=0.45f;
    //[SerializeField] public float shipScaleMax=2.5f;
    [SerializeField] public float matrixTime=7f;
    [SerializeField] public float accelTime=7f;
    [SerializeField] public float noHealTime=14f;
    [SerializeField] public float lifeStealTime=13f;
    [SerializeField] public float thornsTime=13f;
    [SerializeField] public float fuelTime=30f;
    [SerializeField] public float onfireTickrate=0.38f;
    [SerializeField] public float onfireDmg=1f;
    [SerializeField] public float decayTickrate=0.5f;
    [SerializeField] public float decayDmg=0.5f;
    [SerializeField] public float fuelDrainAmnt=0.1f;
    [SerializeField] public float fuelDrainFreq=0.5f;
    [SerializeField] public float fuelDrainTimer=-4;
#endregion

#region//Weapon Properties / Energy Costs etc
[Header("Weapon Properties / Energy Costs etc")]
    [SerializeField] public List<WeaponProperties> weaponProperties;
#endregion
#region //Other
[Header("Others")]
    Renderer bgSprite;
    const float flareShootYY = 0.2f;
    int moveDir = 1;
    const float DCLICK_TIME = 0.2f;
    float lastClickTime;
    int dashDir=0;
    int dashDirX;
    int dashDirY;
    [HideInInspector]public bool dashing = false;
    [HideInInspector]public float shootTimer = 2f;
    [HideInInspector]public float shadowdashInstTime = 0.025f;
    [HideInInspector]public float shadowtracesInstTime = 0.1f;
    [HideInInspector]public float shadowInstTimer = 0f;
    [SerializeField]public float meleeCostTimer;
    [SerializeField]public Vector2 mousePos;
    [HideInInspector]public Vector2 mouseDir;
    [SerializeField]public float dist;
    [HideInInspector]public Vector2 vel;
    public float shipScale=1f;
    float hPressedTime;
    float vPressedTime;
    public float mPressedTime;
    //public float timeFlyingTotal;
    //public float stayingTimerTotal;

    Rigidbody2D rb;
    PlayerModules pmodules;
    [HideInInspector]public Joystick joystick;
    //Settings settings;
    IEnumerator shootCoroutine;
    //FollowMouse followMouse;

    Vector2 xRange;
    Vector2 yRange;

    bool hasHandledInputThisFrame = false;
    bool scaleUp;
    public bool moving;
    //public float energyUsedCount;

    bool moveXwas;
    bool moveYwas;
    public RaycastHit2D[] shadowRaycast=new RaycastHit2D[4];
    ContactFilter2D filter2D;
    const float mouseShadowSpeed=150;
    bool dashed;
    public Vector2 tpPos;
    bool dead;
    [HideInInspector]public int collidedId;
    [HideInInspector]public float collidedIdChangeTime;
    public bool _costOnHitMelee;
    public bool _costOnPhaseMelee;
    //public @InputMaster inputMaster;
#endregion
#endregion
    private void Awake(){instance=this;StartCoroutine(SetGameRuleValues());}
    IEnumerator Start(){
        if(GameSession.maskMode!=0)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
        
        rb=GetComponent<Rigidbody2D>();
        pmodules=GetComponent<PlayerModules>();
        joystick=FindObjectOfType<VariableJoystick>();
        SetUpMoveBoundaries();
        dashTime=startDashTime;

        moveXwas=moveX;
        moveYwas=moveY;

        bgSprite=GameObject.Find("BG ColorM").GetComponent<Renderer>();

        yield return new WaitForSeconds(0.06f);
        if(FindObjectOfType<BossAI>()!=null)shipScaleDefault=GameRules.instance.shipScaleBoss;

        //inputMaster.Player.Shoot.performed += _ => Shoot();
        //if(!speeded&&!slowed){speedPrev=moveSpeedInit;}
    }
    IEnumerator SetGameRuleValues(){
    statuses=new List<StatusFx>();
    yield return new WaitForSecondsRealtime(0.02f);
    //Set values
    var i=GameRules.instance;
    if(i!=null){
        ///Basic value
        transform.position=i.startingPosPlayer;
        moveX=i.moveX;moveY=i.moveY;
        moveSpeedInit=i.moveSpeedPlayer;
        autoShoot=i.autoShootPlayer;
        shaderMatProps=i.playerShaderMatProps;
        health=i.healthPlayer;
        healthStart=i.healthPlayer;
        healthMax=i.healthMaxPlayer;
        defenseInit=i.defensePlayer;
        energyOn=i.energyOnPlayer;
        energy=i.energyPlayer;
        energyMax=i.energyMaxPlayer;
        fuelDrainAmnt=i.fuelDrainAmnt;
        fuelDrainFreq=i.fuelDrainFreq;
        powerupsCapacityInit=i.powerupsCapacity;
        powerupsCapacity=powerupsCapacityInit;
        //powerups=new List<Powerup>(powerupsCapacity);
        for(var p=0;p<powerupsCapacity;p++){powerups.Add(new Powerup());}
        for(var p=0;p<powerupsCapacity&&p<i.powerupsStarting.Count;p++){powerups[p]=i.powerupsStarting[p];}
        powerupDefault=i.powerupDefault;
        weaponsLimited=i.weaponsLimited;
        losePwrupOutOfEn=i.losePwrupOutOfEn;losePwrupOutOfAmmo=i.losePwrupOutOfAmmo;
        dmgMultiInit=i.dmgMultiPlayer;
        shootMultiInit=i.shootMultiPlayer;
        shipScaleDefault=i.shipScaleDefault;
        bulletResize=i.bulletResize;
        overheatOn=i.overheatOnPlayer;
        recoilOn=i.recoilOnPlayer;
        critChanceInit=i.critChancePlayer;
        ///State Defaults
        foreach(StatusFx st in i.statusesStart){statuses.Add(st);if(st.timer==-6)_statusesPersistent.Add(st);}
        flipTime=i.flipTime;
        gcloverTime=i.gcloverTime;
        shadowTime=i.shadowTime;
        shadowLength=i.shadowLength;
        shadowtracesSpeed=i.shadowtracesSpeed;
        dashSpeed=i.dashSpeed;
        startDashTime=i.startDashTime;
        inverterTimeMax=i.inverterTimeMax;
        magnetTime=i.magnetTime;
        scalerTime=i.scalerTime;
        scalerSizes=i.scalerSizes;
        matrixTime=i.matrixTime;
        accelTime=i.accelTime;
        onfireTickrate=i.onfireTickrate;
        onfireDmg=i.onfireDmg;
        decayTickrate=i.decayTickrate;
        decayDmg=i.decayDmg;
        //WeaponProperties
        GameRules grdef=GameCreator.instance.gamerulesetsPrefabs[0];
        if(grdef!=null){
        foreach(WeaponProperties wi in grdef.weaponProperties){
            if(!weaponProperties.Contains(wi)){weaponProperties.Add(wi);}
        }}
        foreach(WeaponProperties wi in i.weaponProperties){
        for(var t=0;t<weaponProperties.Count;t++){
            if(weaponProperties[t].name==wi.name){weaponProperties[t]=wi;}
        }}
        yield return new WaitForSecondsRealtime(0.04f);
        for(var w=0;w<weaponProperties.Count;w++){
            var wp=weaponProperties[w];
            var wc=Instantiate(wp);
            weaponProperties[w]=wc;
        }
    }

        yield return new WaitForSecondsRealtime(0.06f);
        
        moveSpeedBase=moveSpeedInit;
        moveSpeedCurrent=moveSpeedBase;
        speedPrev[0]=moveSpeedBase;
        defenseBase=defenseInit;
        shootMultiBase=shootMultiInit;
        dmgMultiBase=dmgMultiInit;
        critChanceBase=critChanceInit;

        FindObjectOfType<PowerupInventory>().SetCapacity();
    }

    void Update(){
        SetPowerupsCapacity();
        if(!GameSession.GlobalTimeIsPaused){
            SetInputType(SaveSerial.instance.settingsData.inputType);
            HandleInput(false);
            if(!GameSession.instance._adventureLoading&&!GameSession.instance._lvlEventsLoading)health=Mathf.Clamp(health,0,healthMax);
            energy=Mathf.Clamp(energy,0,energyMax);
            transform.localScale=new Vector3(shipScale,shipScale,1);
            CheckPowerups();
            DrawMeleeWeapons();
            HideMeleeWeapons();
            UpdateItems();
            QuickHeal();
            if(pmodules!=null){if(pmodules.timerTeleport==-4){Shoot();}}else{Shoot();}
            Statuses();
            CalculateDefenseSpeed();
            Regen();
            Die();
            CountTimeMovementPressed();
            //SetPowerupsCapacity();
            SelectItemSlot();
            Mathf.Clamp(transform.position.x,xRange.x,xRange.y);
            Mathf.Clamp(transform.position.y,yRange.x,yRange.y);
            if(shaderMatProps!=null){GetComponent<SpriteRenderer>().material=GameAssets.instance.UpdateShaderMatProps(GetComponent<SpriteRenderer>().material,shaderMatProps);}
            else{GetComponent<SpriteRenderer>().material=GameAssets.instance.UpdateShaderMatProps(GetComponent<SpriteRenderer>().material,new ShaderMatProps());}
            if(!_hasStatus("frozen")){//&&(!_hasStatus("fuel")||(_hasStatus("fuel")&&energy>0))){
                if(GetComponent<TrailVFX>()!=null){
                    if(GetComponent<TrailVFX>().enabled==false){GetComponent<TrailVFX>().enabled=true;}
                    if(GetComponent<TrailVFX>().trailObj!=null)if(GetComponent<TrailVFX>().trailObj.activeSelf==false){GetComponent<TrailVFX>().trailObj.SetActive(true);}
                }
                if(inputType!=InputType.mouse&&inputType!=InputType.drag){MovePlayer();}
                else if(inputType==InputType.drag){MoveWithDrag();}
                else{MoveWithMouse();}
            }else{
                if(GetComponent<TrailVFX>()!=null){
                    if(GetComponent<TrailVFX>().trailObj!=null)if(GetComponent<TrailVFX>().trailObj.activeSelf==true){GetComponent<TrailVFX>().trailObj.SetActive(false);}
                    if(GetComponent<TrailVFX>().enabled==true){GetComponent<TrailVFX>().enabled=false;}
                }
            }
            if(shootTimer>0)shootTimer-=Time.deltaTime;
            if(shadowInstTimer>0)shadowInstTimer-=Time.deltaTime;
            rb.velocity=vel;
            if(!moving){spawnReqsMono.AddStayingTime(Time.deltaTime);GameSession.instance.stayingTimeXP+=Time.deltaTime;/*stayingTimerTotal+=Time.deltaTime;*/timerHpRegen+=Time.deltaTime;}
            if(moving){spawnReqsMono.AddMovingTime(Time.deltaTime);GameSession.instance.movingTimeXP+=Time.deltaTime;//timeFlyingTotal+=Time.deltaTime;timeFlyingCore+=Time.deltaTime;
                if(_hasStatus("fuel")){
                    if(fuelDrainTimer<=0){
                        var _drain=fuelDrainAmnt*GetStatus("fuel").strength;
                        if(fuelDrainTimer!=-4&&energy>0){AddSubEnergy(_drain,false);}
                        else if(fuelDrainTimer!=-4&&energy<=0){GetComponent<PlayerCollider>().SetLastHit("Fuel",_drain);Damage(_drain);}
                        fuelDrainTimer=fuelDrainFreq;
                    }else{fuelDrainTimer-=Time.deltaTime;}
                }
            }

            if(overheatOn){
                if(GameRules.instance.overheatShaderIdentif){
                    GetComponent<SpriteRenderer>().material.EnableKeyword("HITEFFECT_ON");
                    GetComponent<SpriteRenderer>().material.SetColor("_HitEffectColor",new Color(1,0.6f,0,1));
                    GetComponent<SpriteRenderer>().material.SetFloat("_HitEffectGlow",1.5f);
                    GetComponent<SpriteRenderer>().material.SetFloat("_HitEffectBlend",Mathf.Clamp((overheatTimer/overheatTimerMax)*0.3f,0,0.3f));
                }
                if(overheatCdTimer>0)overheatCdTimer-=Time.deltaTime;
                if(overheatCdTimer<=0&&overheatTimer>0)overheatTimer-=Time.deltaTime*2;
                if(overheatTimer>=overheatTimerMax&&overheatTimerMax!=-4&&overheated!=true){
                    OnFire(3.8f,1);
                    overheatTimer=-4;overheated=true;overheatedTimer=overheatedTime;
                }
                if(overheated==true&&overheatedTimer>0&&!GameSession.GlobalTimeIsPaused){overheatedTimer-=Time.deltaTime;
                    GameAssets.instance.VFX("FlareHit",new Vector2((transform.position.x+0.35f)*shipScale,(transform.position.y+flareShootYY)*shipScale),0.04f);
                    GameAssets.instance.VFX("FlareHit",new Vector2((transform.position.x-0.35f)*shipScale,(transform.position.y+flareShootYY)*shipScale),0.04f);
                }
                if(overheatedTimer<=0&&overheatTimerMax!=4&&overheated!=false){overheated=false;if(autoShoot){shootCoroutine=null;Shoot();}}
            }
            if(Application.platform==RuntimePlatform.Android){mousePos=Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);}
            else{mousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition);}

            if(weaponsLimited){if(_curPwrup().timer>0){_curPwrup().timer-=Time.deltaTime;}
            if(_curPwrup().timer<=0&&_curPwrup().timer!=-4){_curPwrup().timer=-4;
                if(NotEmptyPowerupsCount()>1){ClearCurrentPowerup();SelectAnyNotEmptyPowerup();}else{ResetPowerupDef();}
                if(autoShoot){shootCoroutine=null;Shoot();}AudioManager.instance.Play("PowerupOff");}
            }

            if(collidedIdChangeTime>0){collidedIdChangeTime-=Time.deltaTime;}
        }
    }
    public void SetInputType(InputType type){inputType=type;}
    void FixedUpdate(){
        // If we're first at-bat, handle the input immediately and mark it already-handled.
        //HandleInput(true);
        //MovePlayer();
        //if (!Input.GetButton("Fire1")){if(shootCoroutine!=null){StopCoroutine(shootCoroutine);StopCoroutine(ShootContinuously());}}
        Vector2 mPos=new Vector2(0,0);
        Vector2 _cappedmPos=mPos;
        if(inputType==InputType.mouse){
            if(moveX&&moveY)mPos=new Vector2(mousePos.x,mousePos.y);
            if(moveX&&!moveY)mPos=new Vector2(mousePos.x,transform.position.y);
            if(!moveX&&moveY)mPos=new Vector2(transform.position.x,mousePos.y);
            _cappedmPos=new Vector2(Mathf.Clamp(mPos.x,xRange.x,xRange.y),Mathf.Clamp(mPos.y,yRange.x,yRange.y));
            dist=Vector2.Distance(_cappedmPos,transform.position);
        }else if(inputType==InputType.drag){
            if(moveX&&moveY)mPos=new Vector2(mousePos.x,mousePos.y);
            if(moveX&&!moveY)mPos=new Vector2(mousePos.x,dragStartPos.y);
            if(!moveX&&moveY)mPos=new Vector2(dragStartPos.x,mousePos.y);
            _cappedmPos=new Vector2(Mathf.Clamp(mPos.x,xRange.x,xRange.y),Mathf.Clamp(mPos.y,yRange.x,yRange.y));
            if(dragStartPos!=Vector2.zero)dist=Vector2.Distance(_cappedmPos,dragStartPos);
            else{dist=0;}
        }
    }
#region//Movement etc
    void HandleInput(bool isFixedUpdate){
        bool hadAlreadyHandled=hasHandledInputThisFrame;
        hasHandledInputThisFrame=isFixedUpdate;
        if(hadAlreadyHandled)return;
        /* Perform any instantaneous actions, using Time.fixedDeltaTime where necessary */
    }
    void CountTimeMovementPressed(){
        if(inputType==InputType.touch){
            if(joystick.Horizontal>0.2f||joystick.Horizontal<-0.2f){hPressedTime+=Time.unscaledDeltaTime; mPressedTime+=Time.unscaledDeltaTime;}
            if(joystick.Vertical>0.2f||joystick.Vertical<-0.2f){vPressedTime+=Time.unscaledDeltaTime; mPressedTime+=Time.unscaledDeltaTime;}
            if(((joystick.Horizontal>0.2f||joystick.Horizontal<-0.2f)||(joystick.Vertical>0.2f||joystick.Vertical<-0.2f))&&!GameSession.GlobalTimeIsPaused){moving=true;}//Add to total time flying
            if(joystick.Horizontal<=0.2f && joystick.Horizontal>=-0.2f){hPressedTime=0;}
            if(joystick.Vertical<=0.2f && joystick.Vertical>=-0.2f){vPressedTime=0;}
            if((joystick.Horizontal<=0.2f && joystick.Horizontal>=-0.2f)&&(joystick.Vertical<=0.2f && joystick.Vertical>=-0.2f)){mPressedTime=0;moving=false;}
        }else if(inputType==InputType.keyboard){
            if(Input.GetButton("Horizontal")){hPressedTime+=Time.unscaledDeltaTime; mPressedTime+=Time.unscaledDeltaTime;}
            if(Input.GetButton("Vertical")){vPressedTime+=Time.unscaledDeltaTime; mPressedTime+=Time.unscaledDeltaTime;}
            if(Input.GetButton("Horizontal")||Input.GetButton("Vertical")&&!GameSession.GlobalTimeIsPaused){moving=true;}//Add to total time flying
            if(!Input.GetButton("Horizontal")){hPressedTime=0;}
            if(!Input.GetButton("Vertical")){vPressedTime=0;}
            if(!Input.GetButton("Horizontal")&&!Input.GetButton("Vertical")){mPressedTime=0;moving=false;}
        }
    }
    
    void MovePlayer(){
        var deltaX=0f;var axisX=(Input.GetAxis("Horizontal")*moveDir);
        var deltaY=0f;var axisY=(Input.GetAxis("Vertical")*moveDir);
        if(Input.GetButtonDown("Horizontal")){
            float timeSinceLastClick=Time.time-lastClickTime;
            if(timeSinceLastClick<=DCLICK_TIME && (dashDir==0 || (dashDir<-1||dashDir>1))){dashDir=(int)Input.GetAxisRaw("Horizontal")*2;DClick(dashDir);}//deltaX=(Input.GetAxis("Horizontal")*moveDir)*(Time.deltaTime*moveSpeedCurrent);}
            //else{deltaX=(Input.GetAxis("Horizontal")*moveDir)*(Time.deltaTime*moveSpeedCurrent);}
            lastClickTime=Time.time;
        }
        if(Input.GetButtonDown("Vertical")){
            float timeSinceLastClick=Time.time-lastClickTime;
            if(timeSinceLastClick<=DCLICK_TIME && (dashDir==0 || ((dashDir<0&&dashDir>-2) || (dashDir>1||dashDir<2)))){dashDir=(int)Input.GetAxisRaw("Vertical");DClick(dashDir);}//deltaY=Input.GetAxis("Vertical")*moveDir*Time.deltaTime*moveSpeedCurrent;}
            //else{deltaY=(Input.GetAxis("Vertical")*moveDir)*(Time.deltaTime*moveSpeedCurrent);}
            lastClickTime=Time.time;
        }
        

        float step=moveSpeedCurrent*Time.deltaTime;if(GameRules.instance.playerTimeIndependentFromDelta)step=moveSpeedCurrent*Time.unscaledDeltaTime;//*Mathf.Clamp(Time.deltaTime,0.008f,0.015f);
        if(inputType==InputType.touch){
            deltaX=joystick.Horizontal*step;
            deltaY=joystick.Vertical*step;
        }else{
            if(Input.GetButton("Horizontal"))deltaX=axisX*step;
            if(Input.GetButton("Vertical"))deltaY=axisY*step;

            if(Input.GetButtonUp("Horizontal")){deltaX-=(axisX*step)/100;}//deltaX=0;}
            if(Input.GetButtonUp("Vertical")){deltaY-=(axisY*step)/100;}//deltaY=0;}
        }
        

        var newXpos=transform.position.x;
        var newYpos=transform.position.y;

        if(moveX==true)newXpos=Mathf.Clamp(newXpos,xRange.x,xRange.y)+deltaX;
        if(moveY==true)newYpos=Mathf.Clamp(newYpos,yRange.x,yRange.y)+deltaY;
        var newPos=new Vector2(newXpos,newYpos);transform.position=newPos;
        //Debug.Log("deltaX: "+deltaX+" | deltaY: "+deltaY+" | newPos: "+newPos);
    }

    void MoveWithMouse(){
        mouseDir=mousePos-(Vector2)transform.position;
        mousePos.x=Mathf.Clamp(mousePos.x,xRange.x,xRange.y);
        mousePos.y=Mathf.Clamp(mousePos.y,yRange.x,yRange.y);
        //dist in FixedUpdate()
        float distX=0;if(moveX){distX=Mathf.Abs(mousePos.x-transform.position.x);}
        float distY=0;if(moveY){distY=Mathf.Abs(mousePos.y-transform.position.y);}
        if((moveX&&distX>0f&&distX<0.35f)||(moveY&&distY>0f&&distY<0.35f)){dist=0.35f;}
        if(dist>=0.3f&&!GameSession.GlobalTimeIsPaused){moving=true;}
        if((moveX&&moveY)&&dist<=0.05f){moving=false;}
        if(((moveX&&!moveY)||!moveX&&moveY)&&dist<=0.24f){moving=false;}

        float step=moveSpeedCurrent*Time.deltaTime;if(GameRules.instance.playerTimeIndependentFromDelta)step=moveSpeedCurrent*Time.unscaledDeltaTime;//Mathf.Clamp(Time.deltaTime,0.008f,0.015f);
        if (moveX && moveY)transform.position=Vector2.MoveTowards(transform.position,mousePos*moveDir,step);
        if (moveX && !moveY)transform.position=Vector2.MoveTowards(transform.position,new Vector2(mousePos.x*moveDir,transform.position.y),step);
        if (!moveX && moveY)transform.position=Vector2.MoveTowards(transform.position,new Vector2(transform.position.x,mousePos.y*moveDir),step);

        if(Input.GetButtonDown("Fire2")){
            float timeSinceLastClick=Time.time-lastClickTime;
            if(timeSinceLastClick<=DCLICK_TIME){DClick(0);}
            else{lastClickTime=Time.time;}
        }

        var newXpos=transform.position.x;
        var newYpos=transform.position.y;

        if(moveX)newXpos=Mathf.Clamp(transform.position.x,xRange.x,xRange.y);
        if(moveY)newYpos=Mathf.Clamp(transform.position.y,yRange.x,yRange.y);
        var newPos=new Vector2(newXpos,newYpos);transform.position=newPos;
    }
    public Vector2 dragStartPos=Vector2.zero;
    float dragStopTimer;
    Vector2 dragStopMousePos;
    public void MoveWithDrag(){
        //mouseDir=mousePos-(Vector2)dragStartPos;
        mousePos.x=Mathf.Clamp(mousePos.x,xRange.x,xRange.y);
        mousePos.y=Mathf.Clamp(mousePos.y,yRange.x,yRange.y);

        if(Input.GetButtonDown("Fire1")){if(dragStartPos==Vector2.zero){dragStartPos=mousePos;}}
        else if(Input.GetButtonUp("Fire1")){dragStartPos=Vector2.zero;}

        if(dragStopTimer>0){dragStopTimer-=Time.unscaledDeltaTime;}if(dragStopTimer<=0){dragStopTimer=0.075f;dragStopMousePos=mousePos;}
        if(mousePos==dragStopMousePos&&Input.GetButton("Fire1")){dragStartPos=mousePos;}

        var movePos=Vector2.zero;
        movePos=(Vector2)transform.position+(mousePos-dragStartPos);

        if(dragStartPos!=Vector2.zero){
            //dist in FixedUpdate()
            float distX=0;if(moveX){distX=Mathf.Abs(movePos.x-dragStartPos.x);}
            float distY=0;if(moveY){distY=Mathf.Abs(movePos.y-dragStartPos.y);}
            if((moveX&&distX>0f&&distX<0.35f)||(moveY&&distY>0f&&distY<0.35f)){dist=0.35f;}
            if(dist>=0.3f&&!GameSession.GlobalTimeIsPaused){moving=true;}
            if((moveX&&moveY)&&dist<=0.05f){moving=false;}
            if(((moveX&&!moveY)||!moveX&&moveY)&&dist<=0.24f){moving=false;}
            
            float step = moveSpeedCurrent*Time.deltaTime;
            if (moveX && moveY)transform.position=Vector2.MoveTowards(transform.position,movePos*moveDir,step);
            if (moveX && !moveY)transform.position=Vector2.MoveTowards(transform.position,new Vector2(movePos.x*moveDir,transform.position.y),step);
            if (!moveX && moveY)transform.position=Vector2.MoveTowards(transform.position,new Vector2(transform.position.x,movePos.y*moveDir),step);
        }

        /*if(Input.GetButtonDown("Fire2")){
            float timeSinceLastClick=Time.time-lastClickTime;
            if(timeSinceLastClick<=DCLICK_TIME){DClick(0);}
            else{lastClickTime=Time.time;}
        }*/

        var newXpos=transform.position.x;
        var newYpos=transform.position.y;

        if(moveX)newXpos=Mathf.Clamp(transform.position.x,xRange.x,xRange.y);
        if(moveY)newYpos=Mathf.Clamp(transform.position.y,yRange.x,yRange.y);

        transform.position=new Vector2(newXpos,newYpos);
    }
    #endregion

    void SetUpMoveBoundaries(){
        xRange=new Vector2(Playfield.xRange.x+GameRules.instance.playfieldPadding.x,Playfield.xRange.y-GameRules.instance.playfieldPadding.x);
        yRange=new Vector2(Playfield.yRange.x+GameRules.instance.playfieldPadding.y,Playfield.yRange.y-GameRules.instance.playfieldPadding.y);
    }

    //const float DCLICK_SHOOT_TIME=0.2f;
    float lastClickShootTime;
    void Shoot(){
        if(!GameSession.GlobalTimeIsPaused){
            if(inputType!=InputType.touch&&inputType!=InputType.drag){
                if(!autoShoot){
                    if(Input.GetButtonDown("Fire1")||Input.GetKeyDown(KeyCode.Z)){
                        if(!SaveSerial.instance.settingsData.dtapMouseShoot){
                            UseItemCurrent();
                            if(shootCoroutine!=null){return;}
                            else if(shootCoroutine==null&&shootTimer<=0f){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                        }else{
                            float timeSinceLastClick=Time.time-lastClickShootTime;
                            if(shootCoroutine!=null){return;}
                            else if(timeSinceLastClick<=DCLICK_TIME){UseItemCurrent();if(shootCoroutine==null&&shootTimer<=0f)shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                            else{lastClickShootTime=Time.time;}
                        }
                        
                    }if((!Input.GetButton("Fire1")&&!Input.GetKey(KeyCode.Z))||shootTimer<-1f){
                        if(shootCoroutine!=null)StopCoroutine(shootCoroutine);
                        shootCoroutine=null;
                        if(_canRegenEnergy())timerEnRegen+=Time.deltaTime;
                    }
                }else{
                    if(shootCoroutine!=null){return;}
                    else if(shootCoroutine==null&&shootTimer<=0f){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                    //shootCoroutine=null;
                    if(_canRegenEnergy())timerEnRegen+=Time.deltaTime;
                }
            }else if(inputType==InputType.drag){
                if(!autoShoot){
                    if(Input.GetButtonDown("Fire1")||Input.GetKeyDown(KeyCode.Z)){
                        UseItemCurrent();
                        float timeSinceLastClick=Time.time-lastClickShootTime;
                        if(shootCoroutine!=null){return;}
                        else if(timeSinceLastClick<=DCLICK_TIME&&shootCoroutine==null&&shootTimer<=0f){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                        else{lastClickShootTime=Time.time;}
                    }if((!Input.GetButton("Fire1")&&!Input.GetKey(KeyCode.Z))||shootTimer<-1f){
                        if(shootCoroutine!=null)StopCoroutine(shootCoroutine);
                        shootCoroutine=null;
                        if(_canRegenEnergy())timerEnRegen+=Time.deltaTime;
                    }
                }
            }else{//Regular shooting on Touch in ShootButton()
                if(autoShoot){//Autoshoot on Touch
                    if(shootCoroutine!=null){return;}
                    else if(shootCoroutine==null&&shootTimer<=0f){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                    if(_canRegenEnergy())timerEnRegen+=Time.deltaTime;
                }
            }
        }else{if(shootCoroutine!=null)StopCoroutine(shootCoroutine);shootCoroutine=null;shootTimer=0;}
    }

    public void ShootButton(bool pressed){      if(!GameSession.GlobalTimeIsPaused){
        if(!autoShoot){
            if(pressed){
                if(shootCoroutine!=null){return;}
                else if(shootCoroutine==null&&shootTimer<=0f){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
            }else if(pressed==false||shootTimer<-1f){
                if(shootCoroutine!=null)StopCoroutine(shootCoroutine);
                shootCoroutine=null;
                if(_canRegenEnergy())timerEnRegen+=Time.deltaTime;
            }
        }else{return;}//Autoshoot in Shoot()
    }}
    public void ShadowButton(Vector2 pos){
        if(moveX&&moveY)tpPos=pos;
        if(moveX&&!moveY)tpPos=new Vector2(pos.x,transform.position.y);
        if(!moveX&&moveY)tpPos=new Vector2(transform.position.x,pos.y);
        DClick(0);
    }
    bool _canRegenEnergy(){return (moving&&
        (_isPowerupEmptyCur()||
            (!_isPowerupEmptyCur()&&!_isCurPowerupAnItem()&&
                ((GetWeaponPropertyCur().weaponType==weaponType.melee&&GetWeaponPropertyCur().costType!=costType.energy)||GetWeaponPropertyCur().weaponType!=weaponType.melee)
            )
        )
    );}
    public void DClick(int dir){
        if(_hasStatus("shadowdash")&&(energy>0||!energyOn)){
            if(inputType==InputType.mouse){
                if(moveX&&moveY)tpPos=mousePos;
                if(moveX&&!moveY)tpPos=new Vector2(mousePos.x,transform.position.y);
                if(!moveX&&moveY)tpPos=new Vector2(transform.position.x,mousePos.y);
                dashed=true;
            }else if(inputType==InputType.keyboard){
                if(dir<0&&dir>-2){vel=Vector2.down*dashSpeed*moveDir;}
                if(dir>0&&dir<2){vel=Vector2.up*dashSpeed*moveDir;}
                if(dir<-1){vel=Vector2.left*dashSpeed*moveDir;}
                if(dir>1){vel=Vector2.right*dashSpeed*moveDir;}
                dashDir=0;
            }else if(inputType==InputType.touch){
                dashed=true;
            }
            AddSubEnergy(GameRules.instance.shadowCost,false);
            dashing=true;
            Screenflash.instance.Shadow();
            AudioManager.instance.Play("Shadowdash");
            dashTime=startDashTime;
            //else{ vel = Vector2.zero; }
        }//else { dashTime = startDashTime; vel = Vector2.zero; }
        
    }

    private void Die(){if(health<=0&&!dead){
        Hide();
        pmodules.DeathSkills();
        StatsAchievsManager.instance.AddDeaths();
        GameSession.instance.SetAnalytics();

        if(GameSession.instance.CheckGamemodeSelected("Adventure")){GameSession.instance.DieAdventure();}

        DeathFX();
        GameOverCanvas.instance.OpenGameOverCanvas();

        foreach(Tag_DestroyPlayerDead go in FindObjectsOfType<Tag_DestroyPlayerDead>()){Destroy(go.gameObject);}
        
        Destroy(gameObject,0.05f);
        dead=true;
    }}
    private void Hide(){
        GetComponent<SpriteRenderer>().enabled=false;
        GetComponent<Collider2D>().enabled=false;
        if(GetComponent<TrailVFX>()!=null)GetComponent<TrailVFX>().enabled=false;
        if(GetComponent<PlayerModules>()!=null)GetComponent<PlayerModules>().enabled=false;
        foreach(Transform c in transform){Destroy(c.gameObject);}
    }
    void DeathFX(){
        if(ShipCustomizationManager.instance!=null){
            var dfx=ShipCustomizationManager.instance.GetDeathFx();
            Instantiate(dfx.obj,transform.position,Quaternion.identity);
            AudioManager.instance.Play(dfx.sound);
        }else{
            GameAssets.instance.VFX("Explosion",transform.position,0.5f);
            AudioManager.instance.Play("Death");
        }
        
    }

#region//Powerups & Slotted items
    public IEnumerator ShootContinuously(){     while(!GameSession.GlobalTimeIsPausedNotSlowed){
        WeaponProperties w=null;
        if(!_isPowerupEmptyCur()){
            if(GetWeaponPropertyCur()!=null){w=GetWeaponPropertyCur();}
            else{Debug.LogWarning(powerups[powerupCurID]+" not added to WeaponProperties List");}
            
            if(w!=null){
                costTypeProperties wc=null;
                costTypeCrystalAmmo wcCA=null;
                costTypeBlackEnergy wcBE=null;
                if(w.costType==costType.energy){wc=(costTypeEnergy)w.costTypeProperties;}
                if(w.costType==costType.ammo){wc=(costTypeAmmo)w.costTypeProperties;}
                if(w.costType==costType.crystalAmmo){wc=(costTypeCrystalAmmo)w.costTypeProperties;wcCA=(costTypeCrystalAmmo)wc;}
                if(w.costType==costType.blackEnergy){wc=(costTypeBlackEnergy)w.costTypeProperties;wcBE=(costTypeBlackEnergy)wc;}
                if(w.weaponType==weaponType.bullet){
                    var _ammo=_curPwrup().ammo;
                    if((w.costType==costType.energy&&((energyOn&&energy>0)||(!energyOn)))
                    ||((w.costType==costType.ammo&&_ammo>0))
                    ||((w.costType==costType.crystalAmmo)&&((energyOn&&wcCA.regularEnergyCost>0&&energy>0)||(!energyOn)||wcCA.regularEnergyCost==0)&&(_ammo>0||_ammo==-5||GameSession.instance.coins>0))
                    ||((w.costType==costType.blackEnergy)&&((energyOn&&wcBE.regularEnergyCost>0&&energy>0)||(!energyOn)||wcBE.regularEnergyCost==0)&&(_ammo>0||_ammo==-5||GameSession.instance.xp>0))){
                        if((/*w.costType==costType.ammo&&*/!overheated)&&((w.costType==costType.energy&&!_hasStatus("electrc"))||w.costType!=costType.energy)){
                            weaponTypeBullet wp=null;
                            if(w.weaponType==weaponType.bullet){wp=(weaponTypeBullet)w.weaponTypeProperties;}
                            string asset=w.assetName;
                            GameObject bulletL=null,bulletR=null;
                            GameObject flareL=null,flareR=null;
                            Vector2 posL=(Vector2)transform.position+wp.leftAnchor*shipScale,posR=(Vector2)transform.position+wp.rightAnchor*shipScale;
                            float speedx=wp.speed.x,speedy=wp.speed.y;
                            if(wp.speedE!=Vector2.zero){speedx=UnityEngine.Random.Range(wp.speed.x,wp.speedE.x);speedy=UnityEngine.Random.Range(wp.speed.y,wp.speedE.y);}
                            float speedoffxL,speedoffyL;
                            float speedoffxR,speedoffyR;
                            Vector2 sL=new Vector2(-speedx,speedy),sR=new Vector2(speedx,speedy);
                            Vector3 rL=new Vector3(),rR=new Vector3();
                            float soundIntervalL=0,soundIntervalR=0;
                            
                            void LeftSide(){for(var i=0;i<wp.bulletAmount;i++,
                            rL=new Vector3(rL.x+=wp.serialOffsetAngle.x,0,rL.z+=wp.serialOffsetAngle.y),soundIntervalL+=wp.serialOffsetSound){
                                if(wp.leftAnchorE!=Vector2.zero){posL=(Vector2)transform.position+new Vector2(UnityEngine.Random.Range(wp.leftAnchor.x,wp.leftAnchorE.x),UnityEngine.Random.Range(wp.leftAnchor.y,wp.leftAnchorE.y))*shipScale;}
                                bulletL=GameAssets.instance.Make(asset, posL) as GameObject;
                                if(bulletL!=null){
                                    if(bulletResize){bulletL.transform.localScale*=shipScale;}
                                    if(i>0){speedoffxL=wp.serialOffsetSpeed.x;speedoffyL=wp.serialOffsetSpeed.y;
                                        if(wp.serialOffsetSpeedE!=Vector2.zero){speedoffxL=UnityEngine.Random.Range(wp.serialOffsetSpeed.x,wp.serialOffsetSpeedE.x);speedoffyL=UnityEngine.Random.Range(wp.serialOffsetSpeed.y,wp.serialOffsetSpeedE.y);}
                                        sL=new Vector2(sL.x-=speedoffxL,sL.y+=speedoffyL);}
                                    if(bulletL.GetComponent<Rigidbody2D>()!=null)bulletL.GetComponent<Rigidbody2D>().velocity=sL;
                                    //if(bulletR.GetComponent<ShootInArc>()!=null)bulletR.GetComponent<ShootInArc>().Shoot();
                                    if(bulletL.GetComponent<BounceThroughEnemies>()!=null)bulletL.GetComponent<BounceThroughEnemies>().speed=sL.y;
                                    if(bulletL.GetComponent<BounceBetweenEnemies>()!=null)bulletL.GetComponent<BounceBetweenEnemies>().speed=sL.y;
                                    bulletL.transform.Rotate(rL);
                                    if(bulletL.GetComponent<IntervalSound>()!=null)bulletL.GetComponent<IntervalSound>().interval=soundIntervalR;
                                    if(bulletL.GetComponent<Tag_PlayerWeapon>()!=null&&w.costType==costType.energy){bulletL.GetComponent<Tag_PlayerWeapon>().energy=wc.cost/wp.bulletAmount;}
                                }}
                                if(wp.flare){
                                    if(ShipCustomizationManager.instance!=null){flareL=Instantiate(ShipCustomizationManager.instance.GetFlareVFX(),posL,Quaternion.identity);Destroy(flareL,wp.flareDur);}
                                    else{flareL=GameAssets.instance.GetVFX("FlareShoot");}
                                }
                            }
                            void RightSide(){for(var i=0;i<wp.bulletAmount;i++,
                            rR=new Vector3(rR.x-=wp.serialOffsetAngle.x,0,rR.z-=wp.serialOffsetAngle.y),soundIntervalR+=wp.serialOffsetSound){
                                if(wp.rightAnchorE!=Vector2.zero){posR=(Vector2)transform.position+new Vector2(UnityEngine.Random.Range(wp.rightAnchor.x,wp.rightAnchorE.x),UnityEngine.Random.Range(wp.rightAnchor.y,wp.rightAnchorE.y))*shipScale;}
                                bulletR=GameAssets.instance.Make(asset, posR) as GameObject;
                                if(bulletR!=null){
                                    if(bulletResize){bulletR.transform.localScale*=shipScale;}
                                    if(i>0){speedoffxR=wp.serialOffsetSpeed.x;speedoffyR=wp.serialOffsetSpeed.y;
                                        if(wp.serialOffsetSpeedE!=Vector2.zero){speedoffxR=UnityEngine.Random.Range(wp.serialOffsetSpeed.x,wp.serialOffsetSpeedE.x);speedoffyR=UnityEngine.Random.Range(wp.serialOffsetSpeed.y,wp.serialOffsetSpeedE.y);}
                                        sR=new Vector2(sR.x+=speedoffxR,sR.y+=speedoffyR);}
                                    if(bulletR.GetComponent<Rigidbody2D>()!=null)bulletR.GetComponent<Rigidbody2D>().velocity=sR;
                                    //if(bulletR.GetComponent<ShootInArc>()!=null)bulletR.GetComponent<ShootInArc>().Shoot();
                                    if(bulletR.GetComponent<BounceThroughEnemies>()!=null)bulletR.GetComponent<BounceThroughEnemies>().speed=sR.y;
                                    if(bulletR.GetComponent<BounceBetweenEnemies>()!=null)bulletR.GetComponent<BounceBetweenEnemies>().speed=sR.y;
                                    bulletR.transform.Rotate(rR);
                                    if(bulletR.GetComponent<IntervalSound>()!=null)bulletR.GetComponent<IntervalSound>().interval=soundIntervalR;
                                    if(bulletR.GetComponent<Tag_PlayerWeapon>()!=null&&w.costType==costType.energy){bulletR.GetComponent<Tag_PlayerWeapon>().energy=wc.cost/wp.bulletAmount;}
                                }}
                                if(wp.flare){
                                    if(ShipCustomizationManager.instance!=null){flareR=Instantiate(ShipCustomizationManager.instance.GetFlareVFX(),posR,Quaternion.identity);Destroy(flareR,wp.flareDur);}
                                    else{flareR=GameAssets.instance.GetVFX("FlareShoot");}
                                }
                            }
                            if(wp.leftSide)LeftSide();
                            if(wp.rightSide)RightSide();
                            if(wp.randomSide){if(UnityEngine.Random.Range(0,100)<50){LeftSide();}else{RightSide();}}
                            if(w.costType==costType.energy){AddSubEnergy(wc.cost,false);}
                            if(w.costType==costType.ammo){if(_ammo>=wc.cost||_ammo==-5)AddSubAmmo(wc.cost,_curPwrupName(),false);else{AddSubAmmo(_ammo-wc.cost,_curPwrupName(),false);}}
                            if(w.costType==costType.crystalAmmo){if(_ammo>=wcCA.cost||_ammo==-5){AddSubAmmo(wcCA.cost,_curPwrupName(),false);}else{AddSubAmmo(wcCA.ammoCrafted,_curPwrupName(),true,true);AddSubCoins(wcCA.crystalCost,false,true);}AddSubEnergy(wcCA.regularEnergyCost);}
                            if(w.costType==costType.blackEnergy){if(_ammo>=wc.cost||_ammo==-5){AddSubAmmo(wcBE.cost,_curPwrupName(),false);}else{AddSubAmmo(wcBE.ammoCrafted,_curPwrupName(),true,true);AddSubXP(wc.cost,false);}AddSubEnergy(wcBE.regularEnergyCost);}
                            if(w.ovheat!=0)Overheat(w.ovheat);
                            if(wp.recoilStrength!=0&&wp.recoilTime>0)Recoil(wp.recoilStrength,wp.recoilTime);
                            shootTimer=(wp.shootDelay/wp.tapDelayMulti)/shootMulti;
                            StatsAchievsManager.instance.AddShotsTotal();
                            yield return new WaitForSeconds((wp.shootDelay/wp.holdDelayMulti)/shootMulti);
                        }else{yield break;}
                    }else{if(!autoShoot){AudioManager.instance.Play("NoEnergy");} shootTimer=0f; shootCoroutine=null; yield break;}
                }else{yield break;}
            }else{yield break;}
        }else{shootTimer=0.1f;yield return new WaitForSeconds(0.1f);}
    }}

    void DrawMeleeWeapons(){
        //var cargoDist=2.8f;
        GameObject go=null;
        WeaponProperties w=null;
        if(!_isCurPowerupAnItem()){
            if(!_isPowerupEmptyCur()){if(GetWeaponPropertyCur()!=null){w=GetWeaponPropertyCur();}else{Debug.LogWarning(_curPwrupName()+" not added to WeaponProperties List");}}
            if(w!=null){
            weaponTypeMelee wp=null;
            if(w.weaponType==weaponType.melee){wp=(weaponTypeMelee)w.weaponTypeProperties;
                costTypeProperties wc=null;
                costTypeCrystalAmmo wcCA=null;
                costTypeBlackEnergy wcBE=null;
                if(w.costType==costType.energy){wc=(costTypeEnergy)w.costTypeProperties;}
                if(w.costType==costType.ammo){wc=(costTypeAmmo)w.costTypeProperties;}
                if(w.costType==costType.crystalAmmo){wc=(costTypeCrystalAmmo)w.costTypeProperties;wcCA=(costTypeCrystalAmmo)wc;}
                if(w.costType==costType.blackEnergy){wc=(costTypeBlackEnergy)w.costTypeProperties;wcBE=(costTypeBlackEnergy)wc;}
                GameObject asset=GameAssets.instance.Get(w.assetName);
                var _ammo=_curPwrup().ammo;
                if(ComparePowerupStrCur(w.name)&&
                (
                    (w.costType==costType.energy&&!_hasStatus("electrc")&&((energyOn&&energy>0)||(!energyOn)))
                    ||((w.costType==costType.ammo&&_ammo>0))
                    ||((w.costType==costType.crystalAmmo)&&((energyOn&&wcCA.regularEnergyCost>0&&energy>0)||(!energyOn)||wcCA.regularEnergyCost==0)&&(_ammo>0||_ammo==-5||GameSession.instance.coins>0))
                    ||((w.costType==costType.blackEnergy)&&((energyOn&&wcBE.regularEnergyCost>0&&energy>0)||(!energyOn)||wcBE.regularEnergyCost==0)&&(_ammo>0||_ammo==-5||GameSession.instance.xp>0))
                )
                ){
                    foreach(Transform t in transform){if(t.gameObject.name.Contains(asset.name))go=t.gameObject;}
                    if(go==null){go=Instantiate(asset,transform);go.transform.position=transform.position+new Vector3(wp.offset.x,wp.offset.y,0.01f);}
                    if((_ammo<=0&&_ammo!=-5)&&wp.instaCraftAmmo){meleeCostTimer=0;}
                    if(!wp.costOnHit){
                        _costOnHitMelee=false;
                        if(meleeCostTimer>0){meleeCostTimer-=Time.deltaTime;}
                    }else{_costOnHitMelee=true;}
                    _costOnPhaseMelee=wp.costOnPhase;
                    if(_costOnHitMelee||_costOnPhaseMelee){if(_ammo<=0&&_ammo!=-5){meleeCostTimer=0;}}

                    if(meleeCostTimer<=0){meleeCostTimer=wp.costPeriod;
                        if((w.costType==costType.energy&&energyOn&&energy>0)||(w.costType!=costType.energy||!energyOn)){var _cost=wc.cost;if(wp.scaleCostWithShipSize){_cost=(float)System.Math.Round(wc.cost*shipScale,2);}AddSubEnergy(_cost,false);}
                        if(w.costType==costType.ammo){if(_ammo>=wc.cost||_ammo==-5)AddSubAmmo(wc.cost,_curPwrupName(),false);else{AddSubAmmo(_ammo-wc.cost,_curPwrupName(),false);}}
                        if(w.costType==costType.crystalAmmo){if(_ammo>=wcCA.cost||_ammo==-5){AddSubAmmo(wcCA.cost,_curPwrupName(),false);}else{AddSubAmmo(wcCA.ammoCrafted,_curPwrupName(),true,true);AddSubCoins(wcCA.crystalCost,false,true);}AddSubEnergy(wcCA.regularEnergyCost);}
                        if(w.costType==costType.blackEnergy){if(_ammo>=wcBE.cost||_ammo==-5){AddSubAmmo(wcBE.cost,_curPwrupName(),false);}else{AddSubAmmo(wcBE.ammoCrafted,_curPwrupName(),true,true);AddSubXP(wcBE.benergyCost,false);}AddSubEnergy(wcBE.regularEnergyCost);}
                    }
                }

                //Hide when near Cargo
                /*if(go!=null){if(FindObjectOfType<CargoShip>()!=null&&Vector2.Distance(go.transform.position,FindObjectOfType<CargoShip>().transform.position)<cargoDist){
                    go.SetActive(false);}else{go.SetActive(true);}
                }*/
            }else{_costOnHitMelee=false;_costOnPhaseMelee=false;}}
        }
    }
    void HideMeleeWeapons(){
        foreach(WeaponProperties ws in weaponProperties){
            if(ws.weaponType==weaponType.melee){
                var wpt=(weaponTypeMelee)ws.weaponTypeProperties;
                var _ammo=_curPwrup().ammo;
                if(!ComparePowerupStrCur(ws.name)||(
                    (ws.costType==costType.energy&&((energyOn&&energy<=0)||_hasStatus("electrc"))))
                    ||(ws.costType!=costType.energy&&_ammo<=0&&_ammo!=-5)
                ){
                    GameObject asset=GameAssets.instance.Get(ws.assetName);
                    foreach(Transform t in transform){if(t.gameObject.name.Contains(ws.assetName)){Destroy(t.gameObject);}}
                }
            }
        }
    }


    public const string _itemSuffix="_item";
    PowerupItemSettings GetItemSettings(string name){return GameRules.instance.GetItemSettings(name);}
    public void AddItem(string name){
        if(GameRules.instance.slottablePowerupItems){
            if(GetPowerupStr(name+_itemSuffix)!=null){if(GetPowerupStr(name+_itemSuffix).ammo<GetItemSettings(name).max){GetPowerupStr(name+_itemSuffix).ammo++;}}
            else{SetPowerupStr(name+_itemSuffix);GetPowerupStr(name+_itemSuffix).ammo=1;}
        }else{UseItem(name);}
    }
    void UpdateItems(){
        foreach(Powerup pwrup in powerups){
            if(pwrup!=null){
                if(!String.IsNullOrEmpty(pwrup.name)){
                    if(pwrup.name.Contains(_itemSuffix))
                    pwrup.timer=-4;
                }
            }
        }
    }
    void UseItemCurrent(){UseItem(_curPwrupName());}
    public void UseItem(string name){
        if(!String.IsNullOrEmpty(name)){
            if(name.Contains(_itemSuffix)){
                if(ContainsPowerup(name)){
                    if(GetPowerupStr(name).ammo>0){
                        if(name=="medkit"+_itemSuffix){MedkitUse();}
                        GetPowerupStr(name).ammo--;
                        if(GetPowerupStr(name).ammo==0){ClearPowerup(name);}
                    }else{ClearPowerup(name);}
                }
            }
        }
    }
    void QuickHeal(){if(Input.GetKeyDown(KeyCode.H)){UseItem("medkit_item");}}
    public void MedkitUse(){
        if(health<=healthMax-GameRules.instance.medkit_hpGain&&!_hasStatus("noHeal")){HPAdd(GameRules.instance.medkit_hpGain);}
        else{float _hpDif=healthMax-health;HPAdd(_hpDif);int _scoreVal=Mathf.RoundToInt(GameRules.instance.medkit_hpGain-_hpDif);GameSession.instance.AddToScoreNoEV(_scoreVal);}
        if(health==healthMax&&GameRules.instance.medkit_hpGain==1){GameSession.instance.AddToScoreNoEV(25);}
        AddSubEnergy(GameRules.instance.medkit_energyGain,true);
        AudioManager.instance.Play("Heal");
    }

    
    void SelectItemSlot(){   if(!PauseMenu.GameIsPaused&&!_hasStatus("hacked")){
            if(Input.GetAxis("Mouse ScrollWheel")>0f||Input.GetKeyDown(KeyCode.JoystickButton5)||Input.GetKeyDown(KeyCode.Equals)){
                int i=0;
                if(powerupCurID<powerups.Count-1){for(i=powerupCurID+1;i<powerups.Count-1;i++){if(_isPowerupSlotScrollable(i)){/*Debug.Log("Up: "+i);*/break;}}}
                else{for(i=0;i<powerups.Count-1;i++){if(_isPowerupSlotScrollable(i)){/*Debug.Log("Up(Last): "+i);*/break;}}}
                if(_isPowerupSlotScrollable(i)){powerupCurID=i;return;}
            }
            else if(Input.GetAxis("Mouse ScrollWheel")<0f||Input.GetKeyDown(KeyCode.JoystickButton4)||Input.GetKeyDown(KeyCode.Minus)){
                int i=0;
                if(powerupCurID>0){for(i=powerupCurID-1;i>0;i--){if(_isPowerupSlotScrollable(i)){/*Debug.Log("Down: "+i);*/break;}}}
                else{for(i=powerups.Count-1;i>0;i--){if(_isPowerupSlotScrollable(i)){/*Debug.Log("Down(First): "+i);*/break;}}}
                if(_isPowerupSlotScrollable(i)){powerupCurID=i;return;}
            }
            
            bool _isReplaced=false;
            int _key1=49;
            for(int i=0,_key=_key1;i<10&&i<powerups.Count;i++,_key++){
                if(_key>57)_key=48;//for the '0' key
                if(Input.GetKey((KeyCode)_key)){ReplacePowerupsSlots(i);}
                if(Input.GetKeyUp((KeyCode)_key)&&!_isReplaced){SelectPowerup(i);}
                if(Input.GetKeyUp((KeyCode)_key)&&_isReplaced){_isReplaced=false;}
            }
            
            if(Input.GetKeyUp(KeyCode.Backslash)){if(powerups.Count>1)DropCurrentPowerup();}
            void SelectPowerup(int _i){if(powerups.Count>_i&&_isPowerupSlotSelectable(_i)){powerupCurID=_i;}}
            void ReplacePowerupsSlots(int id){
                for(int i=0,_key=_key1;i<9&&i<powerups.Count;i++,_key++){
                    if(_key>57)_key=48;//for the '0' key
                    if(Input.GetKeyDown((KeyCode)_key)){
                        if(_key==48)_key=57;//bring it back after checking
                        var _pressedIDFromKey=(_key-_key1);
                        //Debug.Log("Held: "+i+" | "+"key: "+_key+" (id: "+_pressedIDFromKey+")");
                        Powerup  _tempPowerup=GetPowerup(id);
                        powerups[id]=GetPowerup(_pressedIDFromKey);
                        powerups[_pressedIDFromKey]=_tempPowerup;
                        _isReplaced=true;
                        SelectPowerup(_pressedIDFromKey);
                    }
                }
            }
    }}
    bool _isPowerupSlotSelectable(int i){return ((!SaveSerial.instance.settingsData.allowSelectingEmptySlots&&!_isPowerupEmptyID(i))||SaveSerial.instance.settingsData.allowSelectingEmptySlots);}
    bool _isPowerupSlotScrollable(int i){return ((!SaveSerial.instance.settingsData.allowScrollingEmptySlots&&!_isPowerupEmptyID(i))||SaveSerial.instance.settingsData.allowScrollingEmptySlots);}
    void DropCurrentPowerup(){ClearCurrentPowerup();}
#endregion
 
#region//Statuses
    bool playedStatusTimerCloseSound=false;
    void Statuses(){
        void CountdownStatusTimer(string name,string playSound="-",bool unscaledTime=false,bool revertToPrevSpeed=false,bool countQuickerWhenMoving=false,bool playTimerCloseSound=false){
            var s=GetStatus(name);
            if(_hasStatus(name)){
                if(s.timer>0){
                    float _step=0;float _mult=1;
                    if(countQuickerWhenMoving){_mult=(1+dist);}
                    if(!unscaledTime){_step=Time.deltaTime*_mult;}else{_step=Time.unscaledDeltaTime*_mult;}
                    s.timer-=_step;
                    if(playTimerCloseSound&&!playedStatusTimerCloseSound&&s.timer<=1.33f){AudioManager.instance.Play("TimerTicking");playedStatusTimerCloseSound=true;}
                }
                else{if(s.timer!=-5&&s.timer!=-6){
                    RemoveStatus(name);
                    if(playSound=="-"){AudioManager.instance.Play("PowerupOff");}else if(playSound!="-"&&playSound!=""){AudioManager.instance.Play(playSound);}
                    if(playTimerCloseSound){playedStatusTimerCloseSound=false;}
                    if(revertToPrevSpeed)RevertToSpeedPrev();
                }}
            }
        }
        if(!_hasStatus("onfire")){if(onFireCor!=null)StopCoroutine(onFireCor);onFireCor=null;}
        if(!_hasStatus("decay")){if(decayCor!=null)StopCoroutine(decayCor);decayCor=null;}

        if(!GameSession.GlobalTimeIsPaused){
            if(_hasStatus("flip")){
                CountdownStatusTimer("flip",playTimerCloseSound:true);
                moveDir=-1;
            }else{moveDir=1;}

            if(_hasStatus("gclover")){
                CountdownStatusTimer("gclover","GCloverOff");
                health=healthMax;
            }

            if(_hasStatus("shadowdash")){
                if(GetStatus("shadowdash").timer>0){GetStatus("shadowdash").timer-=Time.deltaTime;}
                else{AudioManager.instance.Play("PowerupOff");RevertToSpeedPrev();RemoveStatus("shadowdash");}

                if(dashTime<=0&&dashTime!=-4){vel=Vector2.zero;dashing=false;dashTime=-4;}
                else{if(!GameSession.GlobalTimeIsPaused)dashTime-=Time.deltaTime;}
                if(dashTime>0&&dashed){var step=mouseShadowSpeed*Time.deltaTime;transform.position=Vector2.MoveTowards(transform.position,tpPos,step);dashed=false;}
                if(energyOn&&energy<=0){RemoveStatus("shadowdash");}
                Shadow();if(GetComponent<TrailVFX>()!=null){if(GetComponent<TrailVFX>().enabled==true)GetComponent<TrailVFX>().enabled=false;}
            }else{vel=Vector2.zero;dashing=false;dashTime=-4;if(GetComponent<TrailVFX>()!=null){if(GetComponent<TrailVFX>().enabled==false)GetComponent<TrailVFX>().enabled=true;}}
            if(_hasStatus("shadowtraces")){
                CountdownStatusTimer("shadowtraces",revertToPrevSpeed:true);
                Shadow();
            }
            
            if(_hasStatus("blind")){
                if(GetStatus("blind").timer>0){GetStatus("blind").timer-=Time.deltaTime;}
                else{RemoveStatus("blind");if(BlindnessUI.instance!=null)BlindnessUI.instance.on=false;}
                if(BlindnessUI.instance!=null){if(!BlindnessUI.instance.on)BlindnessUI.instance.on=true;}
            }

            if(_hasStatus("inverter")){if(InverterFx.instance!=null){if(!InverterFx.instance.on)InverterFx.instance.on=true;}
                //else{if(InverterFx.instance.on){InverterFx.instance.on=false;}if(InverterFx.instance.revertMusic==false){InverterFx.instance.revertMusic=true;}
                //    if(MusicPlayer.instance!=null&&MusicPlayer.instance.GetComponent<AudioSource>().pitch==-1){MusicPlayer.instance.GetComponent<AudioSource>().pitch=1;}}
                if(GetStatus("inverter").timer<inverterTimeMax&&GetStatus("inverter").timer>-4){GetStatus("inverter").timer+=Time.deltaTime;}
                else if(GetStatus("inverter").timer>=inverterTimeMax&&GetStatus("inverter").timer!=-5&&GetStatus("inverter").timer!=-6){
                    GetStatus("inverter").timer=inverterTimeMax+4;RemoveStatus("inverter");AudioManager.instance.Play("PowerupOff");
                    if(InverterFx.instance!=null){InverterFx.instance.on=false;InverterFx.instance.reverted=false;}
                }
            }
            if(_hasStatus("scaler")){CountdownStatusTimer("scaler");}
            else{shipScale=shipScaleDefault;}

            if(_hasStatus("magnet")){
                CountdownStatusTimer("magnet");
                if(FindObjectsOfType<Tag_MagnetAffected>().Length>0){
                    foreach(Tag_MagnetAffected obj in FindObjectsOfType<Tag_MagnetAffected>()){
                        var followC=obj.GetComponent<Follow>();
                        if(followC==null){Follow follow=obj.gameObject.AddComponent<Follow>();follow.targetObj=this.gameObject;follow.distReq=4;follow.speedFollow=5;}
                        else{followC.distReq=obj.GetDistReq();followC.speedFollow=obj.GetSpeedFollow();}
                    }
                }/*else{
                    foreach(Tag_Collectible obj in FindObjectsOfType<Tag_Collectible>()){
                        var follow=obj.GetComponent<Follow>();
                        if(follow!=null)Destroy(follow);
                    }
                }*/
            }

            CountdownStatusTimer("onfire",countQuickerWhenMoving:true);
            CountdownStatusTimer("decay");
            CountdownStatusTimer("electrc");
            CountdownStatusTimer("frozen");
            CountdownStatusTimer("armored");
            CountdownStatusTimer("fragile");
            CountdownStatusTimer("power");
            CountdownStatusTimer("weakns");
            CountdownStatusTimer("hacked");
            CountdownStatusTimer("blind");
            CountdownStatusTimer("noHeal");
            CountdownStatusTimer("lifeSteal");
            CountdownStatusTimer("thorns");
            CountdownStatusTimer("fuel");

            //CalculateDefenseSpeed();
            
            shootMulti=shootMultiBase;
            critChance=critChanceBase;
            if(_hasStatus("power")&&!_hasStatus("weak")){dmgMulti=dmgMultiBase*GetStatus("power").strength;}else if(!_hasStatus("power")&&_hasStatus("weak")){dmgMulti=dmgMultiBase/GetStatus("weak").strength;}
            if(!_hasStatus("power")&&!_hasStatus("weak")){dmgMulti=dmgMultiBase;}if(_hasStatus("power")&&_hasStatus("weak")){dmgMulti=(dmgMultiBase/GetStatus("weak").strength)*GetStatus("power").strength;}
            if(_hasStatus("onfire")){if(_hasStatus("frozen")){RemoveStatus("frozen");/*Damage(1,dmgType.silent);*/}}
            if(_hasStatus("infEnergy")){energy=infPrevEnergy;if(GetStatus("infEnergy").timer>0){GetStatus("infEnergy").timer-=Time.deltaTime;}else{RemoveStatus("infEnergy");}}

            CountdownStatusTimer("speed",revertToPrevSpeed:true);
            CountdownStatusTimer("slow",revertToPrevSpeed:true);
        }
        
        #region//Matrix & Accel
        if(!GameSession.GlobalTimeIsPausedNotSlowed){
            var accelLimit=GameRules.instance.accelLimit;
            if(accelLimit<0){accelLimit=GameSession.instance.defaultGameSpeed*Mathf.Abs(GameRules.instance.accelLimit);}
            if(_hasStatus("matrix")&&!_hasStatus("accel")){
                CountdownStatusTimer("matrix",unscaledTime:true);
                //if((vel.x<0.7 && vel.x>-0.7) || (vel.y<0.7 && vel.y>-0.7)){
                //||(inputType==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if((inputType==InputType.mouse || inputType==InputType.drag) && dist<1){
                    GameSession.instance.gameSpeed=dist;
                    GameSession.instance.gameSpeed=Mathf.Clamp(GameSession.instance.gameSpeed,GameRules.instance.matrixLimit,GameSession.instance.defaultGameSpeed);
                }else if(inputType==InputType.keyboard && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //GameSession.instance.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,GameRules.instance.matrixLimit,GameSession.instance.defaultGameSpeed);
                }else if(inputType==InputType.touch && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,GameRules.instance.matrixLimit,GameSession.instance.defaultGameSpeed);
                }else{
                    if(GameSession.instance.speedChanged!=true)GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
                }
                //if(GetStatus("matrix").timer<=0){GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed; RemoveStatus("matrix");}
            }

            else if(_hasStatus("accel")&&!_hasStatus("matrix")){
                CountdownStatusTimer("accel",unscaledTime:true);
                //if((vel.x<0.7 && vel.x>-0.7) || (vel.y<0.7 && vel.y>-0.7)){
                //||(inputType==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if((inputType==InputType.mouse || inputType==InputType.drag) && dist>0.35){
                    GameSession.instance.gameSpeed=dist+(1-0.35f);
                    GameSession.instance.gameSpeed=Mathf.Clamp(GameSession.instance.gameSpeed,GameSession.instance.defaultGameSpeed,accelLimit);
                }else if(inputType==InputType.keyboard && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //GameSession.instance.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,GameSession.instance.defaultGameSpeed,accelLimit);
                }else if(inputType==InputType.touch && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,GameSession.instance.defaultGameSpeed,accelLimit);
                }else{
                    if(GameSession.instance.speedChanged!=true)GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
                }
                //if(GetStatus("accel").timer<=0){GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed; RemoveStatus("accel");}
            }

            else if(_hasStatus("matrix")&&_hasStatus("accel")){
                CountdownStatusTimer("matrix",unscaledTime:true);
                CountdownStatusTimer("accel",unscaledTime:true);
                //if((vel.x<0.7 && vel.x>-0.7) || (vel.y<0.7 && vel.y>-0.7)){
                //||(inputType==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if(inputType==InputType.mouse || inputType==InputType.drag){
                    GameSession.instance.gameSpeed=dist;
                    GameSession.instance.gameSpeed=Mathf.Clamp(GameSession.instance.gameSpeed,GameRules.instance.matrixLimit,accelLimit);
                }else if(inputType==InputType.keyboard && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //GameSession.instance.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,GameRules.instance.matrixLimit,accelLimit);
                }else if(inputType==InputType.touch && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,GameRules.instance.matrixLimit,accelLimit);
                }else{
                    if(GameSession.instance.speedChanged!=true)GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
                }
                //if(GetStatus("matrix").timer<=0){GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;RemoveStatus("matrix");}
                //if(GetStatus("accel").timer<=0){GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;RemoveStatus("accel");}
            }
        }
        #endregion
    }
    void CalculateDefenseSpeed(){
        if(_hasStatus("armored")&&!_hasStatus("fragile")){if(defenseBase==1){defense=2;}else{defense=Mathf.RoundToInt(defenseBase*GetStatus("armored").strength+0.5f);}}
        if(!_hasStatus("armored")&&_hasStatus("fragile")){if(defenseBase==1){defense=0;}else{defense=Mathf.RoundToInt(defenseBase/GetStatus("fragile").strength+1);}}
        if(_hasStatus("armored")&&_hasStatus("fragile")||!_hasStatus("armored")&&!_hasStatus("fragile")){defense=defenseBase;}

        if(_hasStatus("speed")&&!_hasStatus("slow")){moveSpeedCurrent=speedPrev[speedPrev.Count-1]*GetStatus("speed").strength;}
        else if(!_hasStatus("speed")&&_hasStatus("slow")){moveSpeedCurrent=speedPrev[speedPrev.Count-1]/GetStatus("slow").strength;}
        else if(_hasStatus("speed")&&_hasStatus("slow")){
            if(speedPrev.Count>1)moveSpeedCurrent=speedPrev[speedPrev.Count-2]/GetStatus("slow").strength*GetStatus("speed").strength;
            else moveSpeedCurrent=speedPrev[speedPrev.Count-1]/GetStatus("slow").strength*GetStatus("speed").strength;
        }else if(_hasStatus("shadowtraces")){
            moveSpeedCurrent=moveSpeedBase*shadowtracesSpeed;
        }else if(!_hasStatus("speed")&&!_hasStatus("slow")&&!_hasStatus("shadowtraces")){moveSpeedCurrent=moveSpeedBase;}
        
    }
    
    void Shadow(){
        if(!GameSession.GlobalTimeIsPaused&&shadowInstTimer<=0){
            GameObject shadow=null;
            if(_hasStatus("shadowdash")){shadow=GameAssets.instance.Make("PlayerShadow",transform.position);shadowInstTimer=shadowdashInstTime;}
            else if(_hasStatus("shadowtraces")){shadow=GameAssets.instance.Make("PlayerShadowColli",transform.position);shadowInstTimer=shadowtracesInstTime;}
            if(shadow!=null){
                shadow.transform.localScale=new Vector3(shipScale,shipScale,1);
                Destroy(shadow.gameObject,shadowLength);
            }
        }
    }
    void Regen(){
        if(!GameSession.GlobalTimeIsPaused){
            hpAbsorpAmnt=Mathf.Clamp(hpAbsorpAmnt,0,healthMax/GameRules.instance.hpAbsorpFractionCap);
            enAbsorpAmnt=Mathf.Clamp(enAbsorpAmnt,0,energyMax/GameRules.instance.enAbsorpFractionCap);
            if(hpAbsorpAmnt>0&&timerHpRegen>=freqHpRegen){if(health<healthMax&&!_hasStatus("noHeal")){HPAddSilent(hpRegenAmnt);HPAbsorp(-hpRegenAmnt);timerHpRegen=0;}}
            if(energyOn)if(enAbsorpAmnt>0&&timerEnRegen>=freqEnRegen){if(energy<energyMax&&!_hasStatus("infEnergy")&&!_hasStatus("electrc")){AddSubEnergy(enRegenAmnt,true);EnAbsorp(-enRegenAmnt);timerEnRegen=0;}}
        }
    }
    public void Thorns(){if(_hasStatus("thorns")){ShootRadialBullets("LSpike",6,6f);}}
    public void ShootRadialBullets(string asset="LSpike",int amnt=6,float speed=5){
        var lrb=gameObject.AddComponent<LaunchRadialBullets>();
        lrb.Setup(GameAssets.instance.Get(asset),amnt,speed);
        lrb.Shoot();
        Destroy(lrb,0.01f);
    }
    public void Recoil(float strength, float time){if(recoilOn)StartCoroutine(RecoilI(strength,time));}
    IEnumerator RecoilI(float strength,float time){     if(!GameSession.GlobalTimeIsPaused){
        Shake.instance.CamShake(0.1f,1/(time*4));
        if(SaveSerial.instance.settingsData.vibrations)Vibrator.Vibrate(2);
        vel = Vector2.down*strength;
        yield return new WaitForSeconds(time);
        vel=Vector2.zero;
    }}
    
    IEnumerator onFireCor;
    public void OnFire(float duration,float timerCap=0,float strength=1){
        SetStatus("onfire",duration,timerCap,strength);
        if(onFireCor==null){onFireCor=OnFireI();StartCoroutine(onFireCor);}
    }
    IEnumerator OnFireI(){while(_hasStatus("onfire")){
        var dmg=onfireDmg*GetStatus("onfire").strength;
        GetComponent<PlayerCollider>().SetLastHit("Fire",dmg);
        Damage(dmg,dmgType.flame);
        yield return new WaitForSeconds(onfireTickrate);//StartCoroutine(onFireCor);
    }yield break;}
    IEnumerator decayCor;
    public void Decay(float duration,float timerCap=0,float strength=1){
        SetStatus("decay",duration,timerCap,strength);
        if(decayCor==null){decayCor=DecayI();StartCoroutine(decayCor);}
    }
    IEnumerator DecayI(){while(_hasStatus("decay")){
        var dmg=decayDmg*GetStatus("decay").strength;
        GetComponent<PlayerCollider>().SetLastHit("Decay",dmg);
        Damage(dmg,dmgType.decay);
        yield return new WaitForSeconds(decayTickrate);//StartCoroutine(decayCor);
    }yield break;}
    public void Electrc(float duration,float timerCap=0){
        Screenflash.instance.Electrc();
        AudioManager.instance.Play("ElectricZap");
        SetStatus("electrc",duration,timerCap);
    }
    public void Freeze(float duration,float timerCap=0){
        Screenflash.instance.Freeze();
        AudioManager.instance.Play("Freeze");
        SetStatus("frozen",duration,timerCap);
    }
    public void Armor(float duration,float timerCap,/*int*/float strength=1){float _strgh=strength;
        //if(strength!=1)_strgh=strength;
        //else _strgh=1.5f;
        SetStatus("armored",duration,timerCap,_strgh);
    }
    public void Fragile(float duration,float timerCap,/*int*/float strength=1){float _strgh=strength;
        //if(strength!=1)_strgh=strength;
        //else _strgh=1.5f;
        SetStatus("fragile",duration,timerCap,_strgh);
    }public void Power(float duration,float timerCap=0,float strength=1){float _strgh=strength;
        if(strength!=1)_strgh=strength;
        else _strgh=1.5f;
        SetStatus("power",duration,timerCap,_strgh);
    }public void Weaken(float duration,float timerCap=0,float strength=1){float _strgh=strength;
        if(strength!=1)_strgh=strength;
        else _strgh=1.5f;
        SetStatus("weakns",duration,timerCap,_strgh);
    }public void Hack(float duration,float timerCap=0){
        SetStatus("hacked",duration,timerCap);
    }public void Blind(float duration,float timerCap=0,float strength=1){float _strgh=strength;
        if(strength!=1)_strgh=strength;
        else _strgh=1.5f;
        SetStatus("blind",duration,timerCap,_strgh);
    }public void Speed(float duration,float timerCap=0,float strength=1){float _strgh=strength;
        if(strength!=1)_strgh=strength;
        else _strgh=1.5f;
        SetStatus("speed",duration,timerCap,_strgh);
        if(!_hasStatus("speed")){
            SetSpeedPrev();
        }
    }public void Slow(float duration,float timerCap=0,float strength=1){float _strgh=strength;
        if(strength!=1)_strgh=strength;
        else _strgh=1.5f;
        SetStatus("slow",duration,timerCap,_strgh);
        if(!_hasStatus("slow")){
            SetSpeedPrev();
        }
    }public void InfEnergy(float duration,float timerCap=0){
        SetStatus("infEnergy",duration,timerCap);
        infPrevEnergy=energy;
    }
#endregion

#region//Other Functions
    public bool _hasStatus(string status){return statuses.Exists(x=>x.name==status);}
    public StatusFx GetStatus(string status){return statuses.Find(x=>x.name==status);}
    public void SetStatus(string status,float time,float timerCap=0,float strength=1){
        var _status=GetStatus(status);
        if(!_hasStatus(status)){statuses.Add(new StatusFx{name=status,timer=time,timerCap=timerCap,strength=strength});}
        else{
            if(_status.timer!=-5&&_status.timer!=-6){
                if(timerCap!=_status.timerCap){_status.timerCap=timerCap;}
                if(GameRules.instance.addToStatusTimer){if(_status.timer<(_status.timerCap-time)||_status.timerCap<=0)_status.timer+=time;}
                if(_status.strength<strength){ResetStatus(status,time,strength);}//_status.strength=strength;}
            }
        }
    }public void RemoveStatus(string status){statuses.RemoveAll(x=>x.name==status);}
    public void ResetStatus(string status,float time,float strength=1){RemoveStatus(status);SetStatus(status,time,strength);}

    public void RemoveStatusRandom(){
        var i=UnityEngine.Random.Range(0,statuses.Count-1);
        statuses.RemoveAt(i);
    }

    public bool _isPowerupEmpty(Powerup powerup){return (powerup==null||(powerup!=null&&String.IsNullOrEmpty(powerup.name)));}
    public bool _isPowerupEmptyID(int id){return (powerups[id]==null||(powerups[id]!=null&&String.IsNullOrEmpty(powerups[id].name)));}
    public bool _isPowerupEmptyCur(){return _isPowerupEmptyID(powerupCurID);}
    public Powerup GetPowerup(int id){Powerup pwrup=null;if(id<powerups.Count){
        pwrup=powerups[id];}return pwrup;}
    public Powerup GetPowerupRandom(){Powerup pwrup=null;while(pwrup==null||(pwrup!=null&&pwrup.name=="")){pwrup=powerups[UnityEngine.Random.Range(0,powerups.Count)];}return pwrup;}
    public Powerup GetPowerupRandomNotStarting(){Powerup pwrup=null;while(pwrup==null||(pwrup!=null&&pwrup.name==""&&GameRules.instance.powerupsStarting.Exists(x=>x.name==pwrup.name))){pwrup=powerups[UnityEngine.Random.Range(0,powerups.Count)];}if(GameRules.instance.powerupsStarting.Exists(x=>x.name==pwrup.name)){pwrup=null;}return pwrup;}
    public Powerup GetPowerupStr(string str){Powerup pwrup=null;
        pwrup=powerups.Find(x=>x.name==str);return pwrup;}
    public Powerup _curPwrup(){Powerup pwrup=null;if(powerups.Count>powerupCurID){pwrup=powerups[powerupCurID];}return pwrup;}
    public string _curPwrupName(){string str="";if(powerups.Count>powerupCurID){if(_curPwrup()!=null)str=_curPwrup().name;}return str;}
    public bool _isPowerupAnItem(Powerup powerup){if(!_isPowerupEmptyCur())return powerup.name.Contains(_itemSuffix);else return false;}
    public bool _isCurPowerupAnItem(){if(!_isPowerupEmptyCur())return _curPwrupName().Contains(_itemSuffix);else return false;}
    public bool ContainsPowerup(string str){bool b=false;if(powerups.Exists(x=>x.name==str)){b=true;}return b;}
    public int NotEmptyPowerupsCount(){int i=0;foreach(Powerup p in powerups){if(p.name!="")i++;}return i;}
    public void SetPowerup(Powerup val){if(!ContainsPowerup(val.name)){
        if(!SaveSerial.instance.settingsData.alwaysReplaceCurrentSlot){int emptyCount=0;
            for(var i=0;i<powerups.Count;i++){
                if(_isPowerupEmpty(powerups[i])){emptyCount++;
                    powerups[i]=val;
                    if(SaveSerial.instance.settingsData.autoselectNewItem){powerupCurID=i;}
                    break;
                }
            }if(emptyCount==0){powerups[powerupCurID]=new Powerup();powerups[powerupCurID]=val;}
        }else{powerups[powerupCurID]=val;}
        WeaponProperties w=null;if(GetWeaponProperty(val.name)!=null){w=GetWeaponProperty(val.name);}
        if(w!=null){
            if(w.duration>0&&weaponsLimited){powerups[powerupCurID].timer=w.duration;}
            if(w.costType!=costType.energy&&w.costType!=costType.ammo){powerups[powerupCurID].ammo=-4;}
        }else{Debug.LogWarning("WeaponProperty for "+val.name+" not defined");}
    }}
    public void SetPowerupStr(string val){if(!ContainsPowerup(val)){
        if(!SaveSerial.instance.settingsData.alwaysReplaceCurrentSlot){int emptyCount=0;
            for(var i=0;i<powerups.Count;i++){
                if(_isPowerupEmpty(powerups[i])){emptyCount++;
                    powerups[i]=new Powerup();
                    powerups[i].name=val;
                    if(SaveSerial.instance.settingsData.autoselectNewItem){powerupCurID=i;}
                    break;
                }
            }
            if(emptyCount==0){powerups[powerupCurID]=new Powerup();powerups[powerupCurID].name=val;}
        }else{
            powerups[powerupCurID]=new Powerup();
            powerups[powerupCurID].name=val;
        }
        if(!val.Contains(_itemSuffix)){
            WeaponProperties w=null;if(GetWeaponProperty(val)!=null){w=GetWeaponProperty(val);}
            if(w!=null){
                if(w.duration>0&&weaponsLimited){powerups[powerupCurID].timer=w.duration;}
                if(w.costType!=costType.energy&&w.costType!=costType.ammo){powerups[powerupCurID].ammo=-4;}
            }else{Debug.LogWarning("WeaponProperty for "+val+" not defined");}
        }
    }}

    public void ReplacePowerupName(string str, string rep){if(GetPowerupStr(str)!=null){GetPowerupStr(str).name=rep;Debug.Log("Powerup "+str+" replaced with "+rep);}else{Debug.Log("Powerup "+str+" is null!");}}

    public void ClearPowerup(string name){powerups[powerups.FindIndex(x=>x.name==name)]=new Powerup();}
    public void ClearCurrentPowerup(){powerups[powerupCurID]=new Powerup();}
    public void ResetPowerupDef(){powerups[powerupCurID]=new Powerup();powerups[powerupCurID].name=powerupDefault;}
    public void SelectAnyNotEmptyPowerup(){
        int i=-1;
        i=powerups.FindIndex(x=>x.name==powerupDefault);//prefer the default
        if(i==-1){powerups.FindIndex(x=>!String.IsNullOrEmpty(x.name));}
        if(i==-1){i=0;}
        powerupCurID=i;
    }
    public void SetPowerupDefaultStr(string val){
        powerupDefault=val;
        WeaponProperties w=null;if(GetWeaponProperty(val)!=null){w=GetWeaponProperty(val);}
        if(w==null){Debug.LogWarning("WeaponProperty for "+val+" not defined");}
    }
    public bool ComparePowerups(Powerup one,Powerup comp){bool b=false;
        if(one.name==comp.name){b=true;}
        return b;
    }
    public bool ComparePowerupsCur(Powerup comp){bool b=false;
        if(_curPwrupName()==comp.name){b=true;}
        return b;
    }
    public bool ComparePowerupStr(Powerup one,string comp){bool b=false;
        if(one.name==comp){b=true;}
        return b;
    }
    public bool ComparePowerupStrCur(string comp){bool b=false;
        if(_curPwrupName()==comp){b=true;}
        return b;
    }
    void SetPowerupsCapacity(){
        if(powerups.Count!=powerupsCapacity){
            //Debug.Log("PowerupsCapacity: "+powerupsCapacity+" | powerups.Count: "+powerups.Count+" | Dif: "+(powerups.Count-powerupsCapacity).ToString());
            for(var i=powerups.Count-1;i>powerupsCapacity-1;i--){powerups.RemoveAt(i);FindObjectOfType<PowerupInventory>().SetCapacity();}
            for(var i=powerups.Count;i<powerupsCapacity;i++){powerups.Add(new Powerup());FindObjectOfType<PowerupInventory>().SetCapacity();}
        }
    }
    public bool _allPowerupsEmpty(){bool b=true;for(var i=0;i<powerups.Count;i++){if(!String.IsNullOrEmpty(powerups[i].name)){b=false;}}return b;}
    bool _allPowerupsEmptyStart=true;
    float _checkPowerupsTimerStart=0.5f;
    void CheckPowerups(){
        if(_checkPowerupsTimerStart>0){_checkPowerupsTimerStart-=Time.deltaTime;}//Debug.Log(_checkPowerupsTimerStart);}
        else{
            //Debug.Log("Checking on powerups");
            if(_allPowerupsEmptyStart){for(var i=0;i<powerups.Count;i++){if(!String.IsNullOrEmpty(powerups[i].name)){_allPowerupsEmptyStart=false;}}}
            if(_allPowerupsEmptyStart){for(var i=0;i<GameRules.instance.powerupsStarting.Count;i++){powerups[i]=GameRules.instance.powerupsStarting[i];Debug.Log("All powerups empty, setting to starting");}}
            //else{Debug.Log("All good, powerups not empty on start");}

            if(_allPowerupsEmpty()){ResetPowerupDef();Debug.Log("All powerups empty, setting to default");}
            //else{Debug.Log("All good, powerups not empty at all");}
            if(losePwrupOutOfEn&&energy<=0&&!ComparePowerupStrCur(powerupDefault)){ResetPowerupDef();}
            if(!_isPowerupEmptyCur()){if(GetWeaponPropertyCur()!=null&&GetWeaponPropertyCur().costType==costType.ammo){if(losePwrupOutOfAmmo&&_curPwrup().ammo<=0)ResetPowerupDef();}}
        }
    }

    public void HPAdd(float hp){Damage(hp,dmgType.heal);}
    public void HPAddSilent(float hp){Damage(hp,dmgType.healSilent);}
    public void Damage(float dmg, dmgType type=dmgType.normal,bool ignoreInvert=true, float electrTime=4f){//Later add on possible Inverter options?
        if(type!=dmgType.heal&&type!=dmgType.healSilent&&type!=dmgType.decay&&!_hasStatus("gclover"))if(dmg!=0){var dmgTot=(float)System.Math.Round(dmg,2);health-=dmgTot;HpPopUpHUD(-dmgTot);FindObjectOfType<HPBarLost>().TriggerBar();}
        else if(_hasStatus("gclover")){AudioManager.instance.Play("GCloverHit");}

        if(type==dmgType.silent){Screenflash.instance.Damage();}
        if(type==dmgType.normal){Screenflash.instance.Damage();AudioManager.instance.Play("ShipHit");}
        if(type==dmgType.flame){Screenflash.instance.Flame();AudioManager.instance.Play("Overheat");}
        if(type==dmgType.decay){if(dmg!=0&&health>dmg*2){var dmgTot=(float)System.Math.Round(dmg,2);health-=dmgTot;HpPopUpHUD(-dmgTot);Screenflash.instance.Damage();AudioManager.instance.Play("Decay");}}
        if(type==dmgType.electr){Screenflash.instance.Electrc();Electrc(electrTime,GameRules.instance.statusCapDefault);}//electricified=true;AudioManager.instance.Play("Electric");}
        if(type==dmgType.shadow){Screenflash.instance.Shadow();AudioManager.instance.Play("ShadowHit");}
        if(type==dmgType.heal){if(dmg!=0){Screenflash.instance.Heal();health+=dmg;HpPopUpHUD(dmg);UniCollider.DMG_VFX(2,GetComponent<Collider2D>(),transform,-dmg);}}
        if(type==dmgType.healSilent){if(dmg!=0){health+=dmg;HpPopUpHUD(dmg);}}
    }
    public void AddSubEnergy(float value, bool add=false,bool ignoreInvert=false){
        if(energyOn&&!_hasStatus("infEnergy")){
            if(!_hasStatus("inverter")||ignoreInvert){
                if(add){energy+=value;EnPopUpHUD(value);}//if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddEnergy(-value);}//EnergyCountVortexWheel-=value;}
                else{energy-=value;EnPopUpHUD(-value);spawnReqsMono.AddEnergy(value);}//if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddEnergy(value);}//EnergyCountVortexWheel+=value;}
            }else{
                if(add){energy-=value;EnPopUpHUD(-value);spawnReqsMono.AddEnergy(value);}//if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddEnergy(value);}//EnergyCountVortexWheel+=value;}
                else{energy+=value;EnPopUpHUD(value);}//if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddEnergy(-value);}//EnergyCountVortexWheel-=value;}
            }
        }
    }
    public void AddSubAmmo(float value,string name, bool add=false,bool ignoreInvert=false){
        var v=(int)value;
        var pwrup=powerups.Find(x=>x.name==name);
        if(pwrup!=null){
            if(pwrup.ammo<0&&pwrup.ammo!=-5)pwrup.ammo=0;
            if(!_hasStatus("inverter")||ignoreInvert){
                if(add){pwrup.ammo+=v;AmmoPopUpHUD(v);}
                else{if(pwrup.ammo>=v)pwrup.ammo-=v;AmmoPopUpHUD(-v);}
            }else{
                if(add){if(pwrup.ammo>=v)pwrup.ammo-=v;AmmoPopUpHUD(-v);}
                else{pwrup.ammo+=v;AmmoPopUpHUD(v);}
            }
        }
    }
    public void AddSubCoins(int value, bool add=true,bool ignoreInvert=false){
        if(!_hasStatus("inverter")||ignoreInvert){
            if(add){GameSession.instance.coins+=value;CoinsPopUpHUD(value);}
            else{GameSession.instance.coins-=value;CoinsPopUpHUD(-value);}
        }else{
            if(add){GameSession.instance.coins-=value;CoinsPopUpHUD(-value);}
            else{GameSession.instance.coins+=value;CoinsPopUpHUD(value);}
        }
    }
    public void AddSubXP(float value, bool add=true,bool ignoreInvert=false){
        if(!_hasStatus("inverter")||ignoreInvert){
            if(add){GameSession.instance.AddXP(value);}
            else{GameSession.instance.AddXP(-value);}
        }else{
            if(add){GameSession.instance.AddXP(-value);}
            else{GameSession.instance.AddXP(value);}
        }
    }
    public void AddSubCores(int value, bool add=true,bool ignoreInvert=false){
        if(!_hasStatus("inverter")||ignoreInvert){
            if(add){GameSession.instance.cores+=value;CoresPopUpHUD(value);}
            else{GameSession.instance.cores-=value;CoresPopUpHUD(-value);}
        }else{
            if(add){GameSession.instance.cores-=value;CoresPopUpHUD(-value);}
            else{GameSession.instance.cores+=value;CoresPopUpHUD(value);}
        }
    }
    public void HPAbsorp(float value, bool add=true,bool ignoreInvert=true){
        if(!_hasStatus("inverter")||ignoreInvert){
            if(add){var _val=value;if(GetComponent<PlayerModules>()!=null&&(GetComponent<PlayerModules>()._isModuleEquipped("AbsorpConc"))){Damage(value*0.5f,dmgType.healSilent);}else{_val*=0.5f;}hpAbsorpAmnt+=_val;HpAbsorpPopUpHUD(_val);}
            else{hpAbsorpAmnt-=value;HpAbsorpPopUpHUD(-value);}
        }else{
            if(add){hpAbsorpAmnt-=value;HpAbsorpPopUpHUD(-value);}
            else{var _val=value;if(GetComponent<PlayerModules>()!=null&&(GetComponent<PlayerModules>()._isModuleEquipped("AbsorpConc"))){Damage(value*0.5f,dmgType.healSilent);}else{_val*=0.5f;}hpAbsorpAmnt+=_val;HpAbsorpPopUpHUD(_val);}
        }
    }
    public void EnAbsorp(float value, bool add=true,bool ignoreInvert=true){
        if(!_hasStatus("inverter")||ignoreInvert){
            if(add){enAbsorpAmnt+=value;EnAbsorpPopUpHUD(value);}
            else{enAbsorpAmnt-=value;EnAbsorpPopUpHUD(-value);}
        }else{
            if(add){enAbsorpAmnt-=value;EnAbsorpPopUpHUD(-value);}
            else{enAbsorpAmnt+=value;EnAbsorpPopUpHUD(value);}
        }
    }

    public void Overheat(float value, bool add=true,bool ignoreInvert=false){
        if(overheatOn){
        if(overheatTimerMax!=-4){
        if(overheated!=true){
            if(!_hasStatus("inverter")||ignoreInvert){
                if(add){if(overheatTimer==-4){overheatTimer=0;}overheatTimer+=value;overheatCdTimer=overheatCooldown;}
                else{overheatTimer-=value;}
            }else{
                if(add){overheatTimer-=value;overheatCdTimer=overheatCooldown;}
                else{if(overheatTimer==-4){overheatTimer=0;}overheatTimer+=value;}
            }
        }
        }
        }
    }
    public WeaponProperties GetWeaponProperty(string name){return weaponProperties.Find(x=>x.name==name);}
    public WeaponProperties GetWeaponPropertyCur(){return GetWeaponProperty(_curPwrupName());}

    public void SetSpeedPrev(){
        //if(speedPrev.Count==1&&speedPrev[0]==moveSpeedBase){speedPrev[0]=moveSpeedCurrent;}
        //else{speedPrev.Add(moveSpeedCurrent);}
        if(moveSpeedCurrent!=moveSpeedBase){speedPrev.Add(moveSpeedCurrent);}
    }
    public void RevertToSpeedPrev(){
        if(speedPrev.Count>1){moveSpeedCurrent=speedPrev[speedPrev.Count-1];speedPrev.Remove(speedPrev[speedPrev.Count-1]);}
        else{moveSpeedCurrent=moveSpeedBase;speedPrev[0]=moveSpeedBase;}
    }

    public Enemy FindClosestEnemy(){
        KdTree<Enemy> Enemies=new KdTree<Enemy>();
        Enemy[] EnemiesArr;
        EnemiesArr=FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in EnemiesArr){Enemies.Add(enemy);}
        Enemy closest=Enemies.FindClosest(transform.position);
        return closest;
    }
    private void SetActiveAllChildren(Transform transform, bool value){foreach (Transform child in transform){child.gameObject.SetActive(value);SetActiveAllChildren(child, value);}}
#endregion
#region//Pop-Ups
void HpPopUpHUD(float amnt){GameCanvas.instance.HpPopupSwitch(amnt);}
void HpAbsorpPopUpHUD(float amnt){GameCanvas.instance.HpAbsorpPopupSwitch(amnt);}
void EnPopUpHUD(float amnt){GameCanvas.instance.EnPopupSwitch(amnt);}
void EnAbsorpPopUpHUD(float amnt){GameCanvas.instance.EnAbsorpPopupSwitch(amnt);}
void CoinsPopUpHUD(float amnt){GameCanvas.instance.CoinPopupSwitch(amnt);}
void CoresPopUpHUD(float amnt){GameCanvas.instance.CorePopupSwitch(amnt);}
void XPPopUpHUD(float amnt){GameCanvas.instance.XpPopupSwitch(amnt);}
void ScorePopUpHUD(float amnt){GameCanvas.instance.ScorePopupSwitch(amnt);}
void AmmoPopUpHUD(float amnt){GameCanvas.instance.AmmoPopupSwitch(amnt);}
#endregion
}
[System.Serializable]
public class StatusFx{
    public string name;
    public float timer=0;
    public float timerCap=0;
    public float strength=1;
}