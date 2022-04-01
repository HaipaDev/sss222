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

public class Player : MonoBehaviour{
    public static Player instance;
#region Vars
#region//Basic Player Values
    [Header("Player")]
    [SerializeField] public bool moveX = true;
    [SerializeField] public bool moveY = true;
    [SerializeField] InputType inputType = InputType.mouse;
    [SerializeField] public bool autoShoot = false;
    [SerializeField] float paddingX = -0.125f;
    [SerializeField] float paddingY = 0.45f;
    [SerializeField] public float moveSpeedInit = 5f;
    [DisableInEditorMode]public float moveSpeed = 5f;//A variable for later modifications like Upgrades
    [DisableInEditorMode]public float moveSpeedCurrent;
    [SerializeField]public float health = 100f;
    [SerializeField] public float healthMax = 100f;
    [SerializeField] public int defenseInit = 0;
    [SerializeField] public int defenseModifBase = 0;
    [DisableInEditorMode]public int defense = 0;
    [SerializeField] public bool energyOn = true;
    [DisableInEditorMode]public float energy = 120f;
    [SerializeField] public float energyMax = 120f;
    [SerializeField] public bool ammoOn=true;
    //[SerializeField] public int ammo = -4;
    [SerializeField] public Powerup[] powerups;
    [SerializeField] public int powerupCurID=0;
    [SerializeField] public string powerupDefault="laser";
    [SerializeField] public bool weaponsLimited=false;
    [SerializeField] public bool losePwrupOutOfEn;
    [SerializeField] public bool losePwrupOutOfAmmo;
    [SerializeField] public bool fuelOn=false;
    [SerializeField] public float fuelDrainAmnt=0.1f;
    [SerializeField] public float fuelDrainFreq=0.5f;
    [SerializeField] public float fuelDrainTimer=-4;
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
    [SerializeField] public float critChance=4f;

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
    //[SerializeField] public float hpForRegen=0f;
    //[SerializeField] public float armorMultiInit=1f;
    [SerializeField] public float dmgMultiInit=1f;
    [SerializeField] public float shootMultiInit=1f;
    //public float armorMulti=1f;
    public float dmgMulti=1f;
    public float shootMulti=1f;
    [SerializeField] public float shipScaleDefault=0.89f;
    [SerializeField] public bool bulletResize;
    [SerializeField] public int bflameDmgTillLvl=1;
#endregion
#region//Statuses
    [Header("Statuses")]
    public List<string> statuses;
    public string statusc="";
    [SerializeField] public bool flip=false;
    public float flipTimer=-4;
    [SerializeField] public bool gclover=false;
    public float gcloverTimer =-4;
    [SerializeField] public bool shadow=false;
    public float shadowTimer=-4;
    public float dashTime;
    [SerializeField] public bool inverter=false;
    public float inverterTimer=14;
    [SerializeField] public bool magnet=false;
    public float magnetTimer=-4;
    [SerializeField] public bool matrix=false;
    public float matrixTimer=-4;
    [SerializeField] public bool scaler=false;
    public float scalerTimer=-4;
    public float pmultiTimer=-4;
    [SerializeField] public bool accel=false;
    public float accelTimer=-4;
    [SerializeField] public bool onfire=false;
    public float onfireTimer=-4;
    [HideInInspector]public float onfireTime=-4;
    [SerializeField] public bool decay=false;
    public float decayTimer=-4;
    [HideInInspector]public float decayTime=-4;
    [SerializeField] public bool electrc=false;
    public float electrcTimer=-4;
    [HideInInspector]public float electrcTime=-4;
    [SerializeField] public bool frozen=false;
    public float frozenTimer=-4;
    [HideInInspector]public float frozenTime=-4;
    [SerializeField] public bool armored=false;
    public float armoredTimer=-4;
    [HideInInspector]public float armoredTime=-4;
    [HideInInspector]public float armoredStrength=1;
    [SerializeField] public bool fragile=false;
    public float fragileTimer=-4;
    [HideInInspector]public float fragileTime=-4;
    [HideInInspector]public float fragileStrength=1;
    [SerializeField] public bool power=false;
    public float powerTimer=-4;
    [HideInInspector]public float powerTime=-4;
    [HideInInspector]public float powerStrength=1;
    [SerializeField] public bool weakns=false;
    public float weaknsTimer=-4;
    [HideInInspector]public float weaknsTime=-4;
    [HideInInspector]public float weaknsStrength=1;
    [SerializeField] public bool hacked=false;
    public float hackedTimer=-4;
    [HideInInspector]public float hackedTime=-4;
    [SerializeField] public bool blind=false;
    public float blindTimer=-4;
    [HideInInspector]public float blindStrenght;
    [HideInInspector]public float blindTime=-4;
    [SerializeField] public bool infEnergy=false;
    public float infEnergyTimer=-4;
    public float infPrevEnergy;
    [HideInInspector]public float infEnergyTime=-4;
    [SerializeField] public bool speed=false;
    public List<float> speedPrev=new List<float>(1);
    public float speedTimer=-4;
    [HideInInspector]public float speedTime=-4;
    [HideInInspector]public float speedStrength=1;
    [SerializeField] public bool slow=false;
    public float slowTimer=-4;
    [HideInInspector]public float slowTime=-4;
    [HideInInspector]public float slowStrength=1;
    
    [Header("State Defaults")]
    [SerializeField] public float flipTime=7f;
    [SerializeField] public float gcloverTime=6f;
    [SerializeField] public bool dashingEnabled=true;
    [SerializeField] public float shadowTime=10f;
    [SerializeField] public float shadowLength=0.33f;
    [SerializeField] public float shadowtracesSpeed=1.3f;
    [SerializeField] public float dashSpeed=10f;
    [SerializeField] public float startDashTime=0.2f;
    [SerializeField] public float inverterTime=10f;
    [SerializeField] public float magnetTime=15f;
    [SerializeField] public float scalerTime=15f;
    [SerializeField] public float[] scalerSizes={0.45f,0.75f,1.2f,1.5f,1.75f,2f,2.5f};
    //[SerializeField] public float shipScaleMin=0.45f;
    //[SerializeField] public float shipScaleMax=2.5f;
    [SerializeField] public float matrixTime=7f;
    [SerializeField] public float pmultiTime=24f;
    [SerializeField] public float accelTime=7f;
    [SerializeField] public float onfireTickrate=0.38f;
    [SerializeField] public float onfireDmg=1f;
    [SerializeField] public float decayTickrate=0.5f;
    [SerializeField] public float decayDmg=0.5f;
#endregion

#region//Energy/Weapon Durations
    [Header("Energy Costs")]
    [SerializeField] public List<WeaponProperties> weaponProperties;
    [SerializeField] public float shadowCost=5f;
    [Header("Energy Gains")]//Collectibles
    [SerializeField] public float energyBallGet=6f;
    [SerializeField] public float energyBatteryGet=11f;
    [SerializeField] public float medkitEnergyGet=40f;
    [SerializeField] public float medkitHpAmnt=25f;
    [SerializeField] public float microMedkitHpAmnt=10f;
    [SerializeField] public float pwrupEnergyGet=36f;
    [SerializeField] public float enForPwrupRefill=25f;
    [SerializeField] public float enPwrupDuplicate=42f;
    [SerializeField] public int crystalMend_refillCost=2;
    [SerializeField] public float energyDiss_refillCost=3.3f;
    [SerializeField] public int crystalGet=2;
    [SerializeField] public int crystalBGet=6;
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
    [HideInInspector]public bool damaged = false;
    [HideInInspector]public bool healed = false;
    [HideInInspector]public bool shadowed = false;
    [HideInInspector]public bool dashing = false;
    [HideInInspector]public bool flamed = false;
    [HideInInspector]public bool electricified = false;
    [HideInInspector]public float shootTimer = 2f;
    [HideInInspector]public float instantiateTime = 0.025f;
    [HideInInspector]public float instantiateTimer = 0f;
    [SerializeField]public float meleeCostTimer;
    [SerializeField]public Vector2 mousePos;
    [HideInInspector]public Vector2 mouseDir;
    [SerializeField]public float dist;
    [HideInInspector]public Vector2 velocity;
    public float shipScale=1f;
    float hPressedTime;
    float vPressedTime;
    public float mPressedTime;
    //public float timeFlyingTotal;
    //public float stayingTimerTotal;

    Rigidbody2D rb;
    PlayerSkills pskills;
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
    //public @InputMaster inputMaster;
#endregion
#endregion
    private void Awake(){instance=this;StartCoroutine(SetGameRuleValues());}
    IEnumerator Start(){
        if(GameSession.maskMode!=0)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
        
        rb=GetComponent<Rigidbody2D>();
        pskills=GetComponent<PlayerSkills>();
        joystick=FindObjectOfType<VariableJoystick>();
        SetUpMoveBoundaries();
        dashTime=startDashTime;

        moveXwas=moveX;
        moveYwas=moveY;

        bgSprite=GameObject.Find("BG ColorM").GetComponent<Renderer>();

        yield return new WaitForSeconds(0.06f);

        var u=UpgradeMenu.instance;
        if(GameSession.instance.CheckGamemodeSelected("Adventure")){
            if(u!=null){
                healthMax+=(Mathf.Clamp(u.healthMax_UpgradesLvl-1,0,999)*(u.healthMax_UpgradesCountMax*u.healthMax_UpgradeAmnt))+(u.healthMax_UpgradeAmnt*u.healthMax_UpgradesCount);/*if(u.total_UpgradesLvl>0)*/health=healthMax;
                energyMax+=(Mathf.Clamp(u.energyMax_UpgradesLvl-1,0,999)*(u.energyMax_UpgradesCountMax*u.energyMax_UpgradeAmnt))+(u.energyMax_UpgradeAmnt*u.energyMax_UpgradesCount);if(u.total_UpgradesLvl>0)energy=energyMax;
            }else{Debug.LogError("UpgradeMenu not found");}
        }else if(GameSession.instance.CheckGamemodeSelected("Hardcore")){
            GetComponent<AudioSource>().playOnAwake=true;
            GetComponent<AudioSource>().loop=true;
            GetComponent<AudioSource>().enabled=true;
            GetComponent<AudioSource>().Play();
        }
        //inputMaster.Player.Shoot.performed += _ => Shoot();
        //if(!speeded&&!slowed){speedPrev=moveSpeedInit;}
    }
    IEnumerator SetGameRuleValues(){
    yield return new WaitForSecondsRealtime(0.07f);
    //Set values
    var i=GameRules.instance;
    if(i!=null){
        ///Basic value
        transform.position=i.startingPosPlayer;
        moveX=i.moveX;moveY=i.moveY;
        paddingX=i.paddingX;paddingY=i.paddingY;
        moveSpeedInit=i.moveSpeedPlayer;
        autoShoot=i.autoShootPlayer;
        health=i.healthPlayer;
        healthMax=i.healthMaxPlayer;
        defenseInit=i.defensePlayer;
        energyOn=i.energyOnPlayer;
        energy=i.energyPlayer;
        energyMax=i.energyMaxPlayer;
        ammoOn=i.ammoOn;
        fuelOn=i.fuelOn;
        fuelDrainAmnt=i.fuelDrainAmnt;
        fuelDrainFreq=i.fuelDrainFreq;
        powerups=new Powerup[i.powerupsCapacity];
        for(var p=0;p<i.powerupsCapacity;p++){powerups[p]=new Powerup();}
        for(var p=0;p<i.powerupsCapacity&&p<i.powerupsStarting.Count;p++){powerups[p]=i.powerupsStarting[p];}
        powerupDefault=i.powerupDefault;
        weaponsLimited=i.weaponsLimited;
        losePwrupOutOfEn=i.losePwrupOutOfEn;losePwrupOutOfAmmo=i.losePwrupOutOfAmmo;
        //armorMultiInit=i.armorMultiPlayer;
        dmgMultiInit=i.dmgMultiPlayer;
        shootMultiInit=i.shootMultiPlayer;
        shipScaleDefault=i.shipScaleDefault;
        bulletResize=i.bulletResize;
        bflameDmgTillLvl=i.bflameDmgTillLvl;
        overheatOn=i.overheatOnPlayer;
        recoilOn=i.recoilOnPlayer;
        critChance=i.critChancePlayer;
        ///State Defaults
        flipTime=i.flipTime;
        gcloverTime=i.gcloverTime;
        dashingEnabled=i.dashingEnabled;
        shadowTime=i.shadowTime;
        shadowLength=i.shadowLength;
        shadowtracesSpeed=i.shadowtracesSpeed;
        shadowCost=i.shadowCost;
        dashSpeed=i.dashSpeed;
        startDashTime=i.startDashTime;
        inverterTime=i.inverterTime;
        magnetTime=i.magnetTime;
        scalerTime=i.scalerTime;
        scalerSizes=i.scalerSizes;
        matrixTime=i.matrixTime;
        pmultiTime=i.pmultiTime;
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
        ///Energy gains
        energyBallGet=i.energyBallGet;
        energyBatteryGet=i.energyBatteryGet;
        medkitEnergyGet=i.medkitEnergyGet;
        medkitHpAmnt=i.medkitHpAmnt;
        microMedkitHpAmnt=i.microMedkitHpAmnt;
        pwrupEnergyGet=i.pwrupEnergyGet;
        enForPwrupRefill=i.enForPwrupRefill;
        enPwrupDuplicate=i.enPwrupDuplicate;

        crystalGet=i.crystalGet;
        crystalBGet=i.crystalBGet;
    }

        yield return new WaitForSecondsRealtime(0.06f);
        
        moveSpeed=moveSpeedInit;
        moveSpeedCurrent=moveSpeed;
        speedPrev[0]=moveSpeed;
        defenseModifBase=defenseInit;
        shootMulti=shootMultiInit;
        //armorMulti=armorMultiInit;
        dmgMulti=dmgMultiInit;
    }

    void Update(){  if(!GameSession.GlobalTimeIsPaused){
        SetInputType(SaveSerial.instance.settingsData.inputType);
        HandleInput(false);
        health=Mathf.Clamp(health,0,healthMax);
        energy=Mathf.Clamp(energy,0,energyMax);
        //ammo=Mathf.Clamp(ammo,-4,999);
        SelectItemSlot();
        LosePowerup();
        //if(!ammoOn)ammo=-4;
        DrawMeleeWeapons();
        HideMeleeWeapons();
        UpdateItems();
        if(GetComponent<PlayerSkills>()!=null){if(GetComponent<PlayerSkills>().timerTeleport==-4){Shoot();}}else{Shoot();}
        Statuses();
        CalculateDefenseSpeed();
        Regen();
        Die();
        CountTimeMovementPressed();
        if(frozen!=true&&(!fuelOn||(fuelOn&&energy>0))){
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
        if(instantiateTimer>0)instantiateTimer-=Time.deltaTime;
        velocity=rb.velocity;
        if(moving==false){spawnReqsMono.AddStayingTime(Time.deltaTime);GameSession.instance.stayingTimeXP+=Time.deltaTime;/*stayingTimerTotal+=Time.deltaTime;*/timerHpRegen+=Time.deltaTime;}
        if(moving==true){spawnReqsMono.AddMovingTime(Time.deltaTime);GameSession.instance.movingTimeXP+=Time.deltaTime;//timeFlyingTotal+=Time.deltaTime;timeFlyingCore+=Time.deltaTime;
            if(fuelOn){if(fuelDrainTimer<=0){if(fuelDrainTimer!=-4&&energy>0){AddSubEnergy(fuelDrainAmnt,false);}fuelDrainTimer=fuelDrainFreq;}else{fuelDrainTimer-=Time.deltaTime;}}
        }

        
        if(overheatOn){
            if(overheatCdTimer>0)overheatCdTimer-=Time.deltaTime;
            if(overheatCdTimer<=0&&overheatTimer>0)overheatTimer-=Time.deltaTime*2;
            if(overheatTimer>=overheatTimerMax&&overheatTimerMax!=-4&&overheated!=true){OnFire(3.8f,1);
            overheatTimer=-4;overheated=true;overheatedTimer=overheatedTime;}
            if(overheated==true&&overheatedTimer>0&&!GameSession.GlobalTimeIsPaused){overheatedTimer-=Time.deltaTime;
                GameAssets.instance.VFX("Flare",new Vector2((transform.position.x+0.35f)*shipScale,(transform.position.y+flareShootYY)*shipScale),0.04f);
                GameAssets.instance.VFX("Flare",new Vector2((transform.position.x-0.35f)*shipScale,(transform.position.y+flareShootYY)*shipScale),0.04f);
                }
            if(overheatedTimer<=0&&overheatTimerMax!=4&&overheated!=false){overheated=false;if(autoShoot){shootCoroutine=null;Shoot();}}
        }
        if(Application.platform==RuntimePlatform.Android){mousePos=Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);}
        else{mousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition);}

        if(weaponsLimited){if(_curPwrup().timer>0){_curPwrup().timer-=Time.deltaTime;}
        if(_curPwrup().timer<=0&&_curPwrup().timer!=-4){_curPwrup().timer=-4;
            if(powerups.Length>1){ClearCurrentPowerup();SelectAnyNotEmptyPowerup();}else{ResetPowerupDef();}
            if(autoShoot){shootCoroutine=null;Shoot();}AudioManager.instance.Play("PowerupOff");}
        }

        if(collidedIdChangeTime>0){collidedIdChangeTime-=Time.deltaTime;}
    }
    Mathf.Clamp(transform.position.x,xRange.x,xRange.y);
    Mathf.Clamp(transform.position.y,yRange.x,yRange.y);
    }
    public void SetInputType(InputType type){inputType=type;}
    void FixedUpdate(){
        // If we're first at-bat, handle the input immediately and mark it already-handled.
        //HandleInput(true);
        //MovePlayer();
        //if (!Input.GetButton("Fire1")){if(shootCoroutine!=null){StopCoroutine(shootCoroutine);StopCoroutine(ShootContinuously());}}
        Vector2 mPos=new Vector2(0,0);
        if(inputType==InputType.mouse){
            if(moveX&&moveY)mPos=new Vector2(mousePos.x,mousePos.y);
            if(moveX&&!moveY)mPos=new Vector2(mousePos.x,transform.position.y);
            if(!moveX&&moveY)mPos=new Vector2(transform.position.x,mousePos.y);
            dist=Vector2.Distance(mPos,transform.position);
        }else if(inputType==InputType.drag){
            if(moveX&&moveY)mPos=new Vector2(mousePos.x,mousePos.y);
            if(moveX&&!moveY)mPos=new Vector2(mousePos.x,dragStartPos.y);
            if(!moveX&&moveY)mPos=new Vector2(dragStartPos.x,mousePos.y);
            if(dragStartPos!=Vector2.zero)dist=Vector2.Distance(mPos,dragStartPos);
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
    
    private void MovePlayer(){
        var deltaX=0f;
        var deltaY=0f;
        if(Input.GetButtonDown("Horizontal")){
            float timeSinceLastClick=Time.time-lastClickTime;
            if(timeSinceLastClick<=DCLICK_TIME && (dashDir==0 || (dashDir<-1||dashDir>1))){dashDir=(int)Input.GetAxisRaw("Horizontal")*2;DClick(dashDir);deltaX=Input.GetAxis("Horizontal")*Time.deltaTime*moveSpeedCurrent*moveDir;}
            else{deltaX=Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedCurrent*moveDir;}
            lastClickTime=Time.time;}
        if(Input.GetButtonDown("Vertical")){
            float timeSinceLastClick=Time.time-lastClickTime;
            if(timeSinceLastClick<=DCLICK_TIME && (dashDir==0 || ((dashDir<0&&dashDir>-2) || (dashDir>1||dashDir<2)))){dashDir=(int)Input.GetAxisRaw("Vertical");DClick(dashDir);deltaY=Input.GetAxis("Vertical")*Time.deltaTime*moveSpeedCurrent*moveDir;}
            else{deltaY=Input.GetAxis("Vertical")*Time.deltaTime*moveSpeedCurrent*moveDir;}
            lastClickTime=Time.time;}

        if(inputType==InputType.touch){
            deltaX=joystick.Horizontal*Time.deltaTime*moveSpeedCurrent*moveDir;
            deltaY=joystick.Vertical*Time.deltaTime*moveSpeedCurrent*moveDir;
        }else{
            deltaX=Input.GetAxis("Horizontal")*Time.deltaTime*moveSpeedCurrent*moveDir;
            deltaY=Input.GetAxis("Vertical")*Time.deltaTime*moveSpeedCurrent*moveDir;
        }

        var newXpos=transform.position.x;
        var newYpos=transform.position.y;

        if(moveX==true)newXpos=Mathf.Clamp(newXpos,xRange.x,xRange.y)+deltaX;
        if(moveY==true)newYpos=Mathf.Clamp(newYpos,yRange.x,yRange.y)+deltaY;
        transform.position=new Vector2(newXpos,newYpos);
        
    }

    private void MoveWithMouse(){
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

        float step = moveSpeedCurrent*Time.deltaTime;
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

        transform.position=new Vector2(newXpos,newYpos);
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
        xRange=new Vector2(Playfield.xRange.x+paddingX,Playfield.xRange.y-paddingX);
        yRange=new Vector2(Playfield.yRange.x+paddingY,Playfield.yRange.y-paddingY);
    }

    //const float DCLICK_SHOOT_TIME=0.2f;
    float lastClickShootTime;
    void Shoot(){
        if(!GameSession.GlobalTimeIsPaused){
            if(inputType!=InputType.touch&&inputType!=InputType.drag){
                if(!autoShoot){
                    if(Input.GetButtonDown("Fire1")){
                        if(!SaveSerial.instance.settingsData.dtapMouseShoot){
                            UseItemCurrent();
                            if(shootCoroutine!=null){return;}
                            else if(shootCoroutine==null&&shootTimer<=0f&&!_isPowerupEmptyCur()){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                        }else{
                            float timeSinceLastClick=Time.time-lastClickShootTime;
                            if(shootCoroutine!=null){return;}
                            else if(timeSinceLastClick<=DCLICK_TIME){UseItemCurrent();if(shootCoroutine==null&&shootTimer<=0f&&!_isPowerupEmptyCur())shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                            else{lastClickShootTime=Time.time;}
                        }
                        
                    }if(!Input.GetButton("Fire1")||shootTimer<-1f){
                        if(shootCoroutine!=null)StopCoroutine(shootCoroutine);
                        shootCoroutine=null;
                        if(moving==true)timerEnRegen+=Time.deltaTime;
                    }
                }else{
                    if(shootCoroutine!=null){return;}
                    else if(shootCoroutine==null&&shootTimer<=0f&&!_isPowerupEmptyCur()){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                    //shootCoroutine=null;
                    if(moving==true)timerEnRegen+=Time.deltaTime;
                }
            }else if(inputType==InputType.drag){
                if(!autoShoot){
                    if(Input.GetButtonDown("Fire1")){
                        UseItemCurrent();
                        float timeSinceLastClick=Time.time-lastClickShootTime;
                        if(shootCoroutine!=null){return;}
                        else if(timeSinceLastClick<=DCLICK_TIME&&shootCoroutine==null&&shootTimer<=0f&&!_isPowerupEmptyCur()){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                        else{lastClickShootTime=Time.time;}
                    }if(!Input.GetButton("Fire1")||shootTimer<-1f){
                        if(shootCoroutine!=null)StopCoroutine(shootCoroutine);
                        shootCoroutine=null;
                        if(moving==true)timerEnRegen+=Time.deltaTime;
                    }
                }
            }else{//Regular shooting on Touch in ShootButton()
                if(autoShoot){//Autoshoot on Touch
                    if(shootCoroutine!=null){return;}
                    else if(shootCoroutine==null&&shootTimer<=0f&&!_isPowerupEmptyCur()){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                    if(moving==true)timerEnRegen+=Time.deltaTime;
                }
            }
        }else{if(shootCoroutine!=null)StopCoroutine(shootCoroutine);shootCoroutine=null;shootTimer=0;}
    }

    public void ShootButton(bool pressed){      if(!GameSession.GlobalTimeIsPaused){
        if(!autoShoot){
            if(pressed){
                if(shootCoroutine!=null){return;}
                else if(shootCoroutine==null&&shootTimer<=0f&&!_isPowerupEmptyCur()){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
            }else if(pressed==false||shootTimer<-1f){
                if(shootCoroutine!=null)StopCoroutine(shootCoroutine);
                shootCoroutine=null;
                if(moving==true)timerEnRegen+=Time.deltaTime;
            }
        }else{return;}//Autoshoot in Shoot()
    }}
    public void ShadowButton(Vector2 pos){
        if(moveX&&moveY)tpPos=pos;
        if(moveX&&!moveY)tpPos=new Vector2(pos.x,transform.position.y);
        if(!moveX&&moveY)tpPos=new Vector2(transform.position.x,pos.y);
        DClick(0);
    }
    public void DClick(int dir){
        if((dashingEnabled&&shadow)&&(energy>0||!energyOn)){
            if(inputType==InputType.mouse){
                if(moveX&&moveY)tpPos=mousePos;
                if(moveX&&!moveY)tpPos=new Vector2(mousePos.x,transform.position.y);
                if(!moveX&&moveY)tpPos=new Vector2(transform.position.x,mousePos.y);
                dashed=true;
            }else if(inputType==InputType.keyboard){
                if(dir<0&&dir>-2){rb.velocity=Vector2.down*dashSpeed*moveDir;}
                if(dir>0&&dir<2){rb.velocity=Vector2.up*dashSpeed*moveDir;}
                if(dir<-1){rb.velocity=Vector2.left*dashSpeed*moveDir;}
                if(dir>1){rb.velocity=Vector2.right*dashSpeed*moveDir;}
                dashDir=0;
            }else if(inputType==InputType.touch){
                dashed=true;
            }
            AddSubEnergy(shadowCost,false);
            dashing=true;
            shadowed=true;
            AudioManager.instance.Play("Shadowdash");
            dashTime=startDashTime;
            //else{ rb.velocity = Vector2.zero; }
        }//else { dashTime = startDashTime; rb.velocity = Vector2.zero; }
        
    }

    private void Die(){if(health<=0&&!dead){
        Hide();
        pskills.DeathSkills();
        StatsAchievsManager.instance.AddDeaths();
        GameSession.instance.SetAnalytics();
        //Debug.Log("GameTime: "+GameSession.instance.GetGameSessionTime());

        if(GameSession.instance.CheckGamemodeSelected("Adventure")){GameSession.instance.DieAdventure();}

        DeathFX();
        GameOverCanvas.instance.OpenGameOverCanvas();

        foreach(Tag_DestroyPlayerDead go in FindObjectsOfType<Tag_DestroyPlayerDead>()){Destroy(go.gameObject);}
        
        Destroy(gameObject);
        dead=true;
    }}
    private void Hide(){
        GetComponent<SpriteRenderer>().enabled=false;
        GetComponent<Collider2D>().enabled=false;
        if(GetComponent<TrailVFX>()!=null)GetComponent<TrailVFX>().enabled=false;
        if(GetComponent<PlayerSkills>()!=null)GetComponent<PlayerSkills>().enabled=false;
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
    public IEnumerator ShootContinuously(){     while(!GameSession.GlobalTimeIsPaused){
        WeaponProperties w=null;
        if(!_isPowerupEmptyCur()){if(GetWeaponProperty(_curPwrupName())!=null){w=GetWeaponProperty(_curPwrupName());}else{Debug.LogWarning(powerups[powerupCurID]+" not added to WeaponProperties List");}}
        if(w!=null){
        costTypeProperties wc=null;
        costTypeCrystalAmmo wcCA=null;
        costTypeBlackEnergy wcBE=null;
        if(w.costType==costType.energy){wc=(costTypeEnergy)w.costTypeProperties;}
        if(w.costType==costType.ammo){wc=(costTypeAmmo)w.costTypeProperties;}
        if(w.costType==costType.crystalAmmo){wc=(costTypeCrystalAmmo)w.costTypeProperties;wcCA=(costTypeCrystalAmmo)wc;}
        if(w.costType==costType.blackEnergy){wc=(costTypeBlackEnergy)w.costTypeProperties;wcBE=(costTypeBlackEnergy)wc;}
        if(w.weaponType==weaponType.bullet){
            var ammo=_curPwrup().ammo;
            if((w.costType==costType.energy&&((energyOn&&energy>0)||(!energyOn)))
            ||((w.costType==costType.ammo&&ammo>0))
            ||((w.costType==costType.crystalAmmo)&&((energyOn&&wcCA.regularEnergyCost>0&&energy>0)||(!energyOn)||wcBE.regularEnergyCost==0)&&(ammo>0||GameSession.instance.coins>0))
            ||((w.costType==costType.blackEnergy)&&((energyOn&&wcBE.regularEnergyCost>0&&energy>0)||(!energyOn)||wcBE.regularEnergyCost==0)&&(GameSession.instance.xp>0))){
                if(overheated!=true&&electrc!=true){
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
                    if(w.costType==costType.ammo){if(ammo>=wc.cost)AddSubAmmo(wc.cost,_curPwrupName(),false);else{AddSubAmmo(ammo-wc.cost,_curPwrupName(),false);}}
                    if(w.costType==costType.crystalAmmo){if(ammo>=wcCA.cost)AddSubAmmo(wcCA.cost,_curPwrupName(),false);else{AddSubAmmo(wcCA.crystalAmmoCrafted,_curPwrupName(),true,true);AddSubCoins(wcCA.crystalCost,false,true);}AddSubEnergy(wcCA.regularEnergyCost);}
                    if(w.costType==costType.blackEnergy){if(GameSession.instance.xp>=wc.cost){AddSubXP(wc.cost,false);}if(w.costType==costType.blackEnergy){AddSubEnergy(wcBE.regularEnergyCost);}}
                    if(w.ovheat!=0)Overheat(w.ovheat);
                    if(wp.recoilStrength!=0&&wp.recoilTime>0)Recoil(wp.recoilStrength,wp.recoilTime);
                    shootTimer=(wp.shootDelay/wp.tapDelayMulti)/shootMulti;
                    StatsAchievsManager.instance.AddShotsTotal();
                    yield return new WaitForSeconds((wp.shootDelay/wp.holdDelayMulti)/shootMulti);
                }else{yield break;}
                }else{if(!autoShoot){AudioManager.instance.Play("NoEnergy");} shootTimer=0f; shootCoroutine=null; yield break;}
        }else{yield break;}
        }else{yield break;}
    }}

    void DrawMeleeWeapons(){
        //var cargoDist=2.8f;
        GameObject go=null;
        WeaponProperties w=null;
        if(!_isPowerupEmptyCur()){if(GetWeaponProperty(_curPwrupName())!=null){w=GetWeaponProperty(_curPwrupName());}else{Debug.LogWarning(_curPwrupName()+" not added to WeaponProperties List");}}
        if(w!=null&&w.weaponType==weaponType.melee){
            weaponTypeMelee wp=null;
            if(w.weaponType==weaponType.melee){wp=(weaponTypeMelee)w.weaponTypeProperties;}
            costTypeProperties wc=null;
            if(w.costType==costType.energy){wc=(costTypeEnergy)w.costTypeProperties;}
            GameObject asset=GameAssets.instance.Get(w.assetName);
            if(ComparePowerupStrCur(w.name)&&((w.costType==costType.energy&&energyOn&&energy>0)||(w.costType!=costType.energy||!energyOn))){
                foreach(Transform t in transform){if(t.gameObject.name.Contains(asset.name))go=t.gameObject;}
                if(go==null){go=Instantiate(asset,transform);go.transform.position=transform.position+new Vector3(wp.offset.x,wp.offset.y,0.01f);}
                if(meleeCostTimer>0){meleeCostTimer-=Time.deltaTime;}
                else if(meleeCostTimer<=0){meleeCostTimer=wp.costPeriod;
                    if((w.costType==costType.energy&&energyOn&&energy>0)||(w.costType!=costType.energy||!energyOn)){AddSubEnergy((float)System.Math.Round(wc.cost*shipScale,2),false);}}
            }

            //Hide when near Cargo
            /*if(go!=null){if(FindObjectOfType<CargoShip>()!=null&&Vector2.Distance(go.transform.position,FindObjectOfType<CargoShip>().transform.position)<cargoDist){
                go.SetActive(false);}else{go.SetActive(true);}
            }*/
        }
    }
    void HideMeleeWeapons(){
        foreach(WeaponProperties ws in weaponProperties){
            if(ws.weaponType==weaponType.melee){
                var wpt=(weaponTypeMelee)ws.weaponTypeProperties;
                if(!ComparePowerupStrCur(ws.name)){
                    GameObject asset=GameAssets.instance.Get(ws.assetName);
                    foreach(Transform t in transform){if(t.gameObject.name.Contains(ws.assetName)){Destroy(t.gameObject);}}
                }
            }
        }
    }


    public string _itemSuffix="_item";
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
                if(GetPowerupStr(name).ammo>0){
                    if(name=="medkit"+_itemSuffix){MedkitUse();}
                    GetPowerupStr(name).ammo--;
                    if(GetPowerupStr(name).ammo==0){ClearCurrentPowerup();}
                }else{ClearCurrentPowerup();}
            }
        }
    }

    public void MedkitUse(){
        if(health>=healthMax){GameSession.instance.AddToScoreNoEV(Mathf.RoundToInt(medkitHpAmnt));}
        else if(health!=healthMax&&health>(healthMax-medkitHpAmnt)){
            int val=Mathf.RoundToInt(medkitHpAmnt-(healthMax-health));
            if(val>0)GameSession.instance.AddToScoreNoEV(val);}
        HPAdd(medkitHpAmnt);
        AddSubEnergy(medkitEnergyGet,true);
    }

    
    void SelectItemSlot(){   if(!GameSession.GlobalTimeIsPaused){
            if(Input.GetAxis("Mouse ScrollWheel")>0f||Input.GetKeyDown(KeyCode.JoystickButton5)){if(powerupCurID<powerups.Length-1){powerupCurID++;}else{powerupCurID=0;}}
            else if(Input.GetAxis("Mouse ScrollWheel")<0f||Input.GetKeyDown(KeyCode.JoystickButton4)){if(powerupCurID>0){powerupCurID--;}else{powerupCurID=powerups.Length-1;}}
            if(Input.GetKeyDown(KeyCode.Alpha1)){powerupCurID=0;}
            else if(Input.GetKeyDown(KeyCode.Alpha2)){if(powerups.Length>1){powerupCurID=1;}}
            else if(Input.GetKeyDown(KeyCode.Alpha3)){if(powerups.Length>2){powerupCurID=2;}}
            else if(Input.GetKeyDown(KeyCode.Alpha4)){if(powerups.Length>3){powerupCurID=3;}}
            else if(Input.GetKeyDown(KeyCode.Alpha5)){if(powerups.Length>4){powerupCurID=4;}}
            else if(Input.GetKeyDown(KeyCode.Alpha6)){if(powerups.Length>5){powerupCurID=5;}}
            else if(Input.GetKeyDown(KeyCode.Alpha7)){if(powerups.Length>6){powerupCurID=6;}}
            else if(Input.GetKeyDown(KeyCode.Alpha8)){if(powerups.Length>7){powerupCurID=7;}}
            else if(Input.GetKeyDown(KeyCode.Alpha9)){if(powerups.Length>8){powerupCurID=8;}}
            else if(Input.GetKeyDown(KeyCode.Alpha0)){if(powerups.Length>9){powerupCurID=9;}}
            else if(Input.GetKeyDown(KeyCode.Backslash)){if(powerups.Length>1/*||(powerups.Length==1&&powerups[0].name==powerupDefault)*/)ClearCurrentPowerup();}
            else if(Input.GetKeyDown(KeyCode.Minus)){if(powerupCurID>0){powerupCurID--;}}
            else if(Input.GetKeyDown(KeyCode.Equals)){if(powerupCurID<powerups.Length){powerupCurID++;}}
    }}
#endregion
 
#region//Statuses
    void Statuses(){
        if(flip==true){moveDir=-1;}else{moveDir=1;}
        if(flipTimer<=0&&flipTimer>-4){ResetStatus("flip");AudioManager.instance.Play("PowerupOff");}

        if(gclover==true){
            health=healthMax;
            FindObjectOfType<HPBar>().GetComponent<HPBar>().gclover=true;
        }else{
            FindObjectOfType<HPBar>().GetComponent<HPBar>().gclover=false;
        }
        if(gcloverTimer<=0&&gcloverTimer>-4){AudioManager.instance.Play("GCloverOff");ResetStatus("gclover");}

        if(shadowTimer<=0&&shadowTimer>-4){AudioManager.instance.Play("PowerupOff");RevertToSpeedPrev();ResetStatus("shadow");}
        if(shadow==true){Shadow();if(GetComponent<TrailVFX>()!=null){if(GetComponent<TrailVFX>().enabled==true)GetComponent<TrailVFX>().enabled=false;}}
        else{dashTime=-4;if(GetComponent<TrailVFX>()!=null){if(GetComponent<TrailVFX>().enabled==false)GetComponent<TrailVFX>().enabled=true;}}
        if(dashingEnabled){
            if(shadow==true&&dashTime<=0&&dashTime!=-4){rb.velocity=Vector2.zero; dashing=false; /*moveX=moveXwas;moveY=moveYwas;*/ dashTime=-4;}
            else{if(!GameSession.GlobalTimeIsPaused){dashTime-=Time.deltaTime;}
            if(dashTime>0&&dashed){var step=mouseShadowSpeed*Time.deltaTime;transform.position=Vector2.MoveTowards(transform.position,tpPos,step);dashed=false;}/*if(rb.velocity!=Vector2.zero)rb.velocity-=new Vector2(0.01f,0.01f);*/}
            if(energyOn&&energy<=0){shadow=false;}
            if(shadow==false){dashing=false;}
        }

        if(blind==true){if(BlindnessUI.instance!=null){if(!BlindnessUI.instance.on)BlindnessUI.instance.on=true;}}
        if(blindTimer<=0&&blindTimer>-4){if(BlindnessUI.instance!=null){BlindnessUI.instance.on=false;}}

        if(inverter==true){if(InverterFx.instance!=null){if(!InverterFx.instance.on)InverterFx.instance.on=true;}}
        //else{if(InverterFx.instance.on){InverterFx.instance.on=false;}if(InverterFx.instance.revertMusic==false){InverterFx.instance.revertMusic=true;}
        //    if(MusicPlayer.instance!=null&&MusicPlayer.instance.GetComponent<AudioSource>().pitch==-1){MusicPlayer.instance.GetComponent<AudioSource>().pitch=1;}}
        if(inverterTimer>=inverterTime&&inverterTimer<inverterTime+4){inverterTimer=inverterTime+4;ResetStatus("inverter");if(InverterFx.instance!=null){InverterFx.instance.on=false;InverterFx.instance.reverted=false;}}

        if(magnet==true){
            if(FindObjectsOfType<Tag_MagnetAffected>()!=null){
                Tag_MagnetAffected[] objs=FindObjectsOfType<Tag_MagnetAffected>();
                foreach(Tag_MagnetAffected obj in objs){
                    var followC=obj.GetComponent<Follow>();
                    if(followC==null){Follow follow=obj.gameObject.AddComponent(typeof(Follow)) as Follow;follow.target=this.gameObject;follow.distReq=4;follow.speedFollow=5;}
                    else{followC.distReq=obj.GetDistReq();followC.speedFollow=obj.GetSpeedFollow();}
                }
            }else{
                Tag_Collectible[] objs=FindObjectsOfType<Tag_Collectible>();
                foreach(Tag_Collectible obj in objs){
                    var follow=obj.GetComponent<Follow>();
                    if(follow!=null)Destroy(follow);
                }
            }
        }
        if(magnetTimer<=0&&magnetTimer>-4){ResetStatus("magnet");}
        
        if(scaler==true){
            //Scaler function in PlayerCollider
        }else{
            shipScale=shipScaleDefault;
        }
        if(scalerTimer <=0 && scalerTimer>-4){ResetStatus("scaler");}
        transform.localScale=new Vector3(shipScale,shipScale,1);
        
        if(pmultiTimer <=0 && pmultiTimer>-4){GameSession.instance.scoreMulti=GameSession.instance.defaultGameSpeed; ResetStatus("pmulti");}

        if(!GameSession.GlobalTimeIsPausedNotSlowed){
            if(matrix==true&&accel==false){
                matrixTimer-=Time.unscaledDeltaTime;
                //if((rb.velocity.x<0.7 && rb.velocity.x>-0.7) || (rb.velocity.y<0.7 && rb.velocity.y>-0.7)){
                //||(inputType==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if((inputType==InputType.mouse || inputType==InputType.drag) && dist<1){
                    GameSession.instance.gameSpeed=dist;
                    GameSession.instance.gameSpeed=Mathf.Clamp(GameSession.instance.gameSpeed,0.05f,GameSession.instance.defaultGameSpeed);
                }else if(inputType==InputType.keyboard && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //GameSession.instance.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,GameSession.instance.defaultGameSpeed);
                }else if(inputType==InputType.touch && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,GameSession.instance.defaultGameSpeed);
                }else{
                    if(GameSession.instance.speedChanged!=true)GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
                }
            }
            if(matrixTimer <=0 && matrixTimer>-4){GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed; ResetStatus("matrix");}


            if(accel==true&&matrix==false){
                accelTimer-=Time.unscaledDeltaTime;
                //if((rb.velocity.x<0.7 && rb.velocity.x>-0.7) || (rb.velocity.y<0.7 && rb.velocity.y>-0.7)){
                //||(inputType==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if((inputType==InputType.mouse || inputType==InputType.drag) && dist>0.35){
                    GameSession.instance.gameSpeed=dist+(1-0.35f);
                    GameSession.instance.gameSpeed=Mathf.Clamp(GameSession.instance.gameSpeed,GameSession.instance.defaultGameSpeed,GameSession.instance.defaultGameSpeed*2);
                }else if(inputType==InputType.keyboard && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //GameSession.instance.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,GameSession.instance.defaultGameSpeed,GameSession.instance.defaultGameSpeed*2);
                }else if(inputType==InputType.touch && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,GameSession.instance.defaultGameSpeed,GameSession.instance.defaultGameSpeed*2);
                }else{
                    if(GameSession.instance.speedChanged!=true)GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
                }
            }
            if(accelTimer <=0 && accelTimer>-4){GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed; ResetStatus("accel");}

            if(accel==true&&matrix==true){
                accelTimer-=Time.unscaledDeltaTime;//accelTimer-=Time.deltaTime;
                matrixTimer-=Time.unscaledDeltaTime;//matrixTimer-=Time.deltaTime;
                //if((rb.velocity.x<0.7 && rb.velocity.x>-0.7) || (rb.velocity.y<0.7 && rb.velocity.y>-0.7)){
                //||(inputType==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if(inputType==InputType.mouse || inputType==InputType.drag){
                    GameSession.instance.gameSpeed=dist;
                    GameSession.instance.gameSpeed=Mathf.Clamp(GameSession.instance.gameSpeed,0.05f,GameSession.instance.defaultGameSpeed*2);
                }else if(inputType==InputType.keyboard && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //GameSession.instance.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,GameSession.instance.defaultGameSpeed*2);
                }else if(inputType==InputType.touch && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,GameSession.instance.defaultGameSpeed*2);
                }else{
                    if(GameSession.instance.speedChanged!=true)GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
                }
            }
        }
        if(!GameSession.GlobalTimeIsPaused){
            //Timers
            if(flipTimer>0){flipTimer-=Time.deltaTime;}
            if(gcloverTimer>0){gcloverTimer-=Time.deltaTime;}
            if(shadowTimer>0){shadowTimer-=Time.deltaTime;}
            if(magnetTimer>0){magnetTimer-=Time.deltaTime;}
            if(scalerTimer>0){scalerTimer-=Time.deltaTime;}
            if(pmultiTimer>0){pmultiTimer-=Time.deltaTime;}
            if(inverterTimer<inverterTime&&inverterTimer>-4){inverterTimer+=Time.deltaTime;}

            if(onfireTimer>0){onfireTimer-=Time.deltaTime*(1+dist);}else{if(onfireTimer>-4)ResetStatus("onfire");}
            if(decayTimer>0){decayTimer-=Time.deltaTime;}else{if(decayTimer>-4)ResetStatus("decay");}
            if(electrcTimer>0){electrcTimer-=Time.deltaTime;}else{if(electrcTimer>-4)ResetStatus("electrc");}
            if(frozenTimer>0){frozenTimer-=Time.deltaTime;}else{if(frozenTimer>-4)ResetStatus("frozen");}
            if(armoredTimer>0){armoredTimer-=Time.deltaTime;}else{if(armoredTimer>-4)if(armored){armoredStrength=1;}ResetStatus("armored");}
            if(fragileTimer>0){fragileTimer-=Time.deltaTime;}else{if(fragileTimer>-4)if(fragile){fragileStrength=1;}ResetStatus("fragile");}
            if(powerTimer>0){powerTimer-=Time.deltaTime;}else{if(powerTimer>-4)if(power){powerStrength=1;}ResetStatus("power");}
            if(weaknsTimer>0){weaknsTimer-=Time.deltaTime;}else{if(weaknsTimer>-4)if(weakns){weaknsStrength=1;}ResetStatus("weakns");}
            if(hackedTimer>0){hackedTimer-=Time.deltaTime;}else{if(hackedTimer>-4)ResetStatus("hacked");}
            if(blindTimer>0){blindTimer-=Time.deltaTime;}else{if(blindTimer>-4)ResetStatus("blind");}
            if(speedTimer>0){speedTimer-=Time.deltaTime;}else{if(speedTimer>-4)if(speed){speedStrength=1;RevertToSpeedPrev();}ResetStatus("speed");}
            if(slowTimer>0){slowTimer-=Time.deltaTime;}else{if(slowTimer>-4)if(slow){slowStrength=1;RevertToSpeedPrev();}ResetStatus("slow");}
            //if(armored==true&&fragile!=true){armorMulti=armorMultiInit*armoredStrength;}else if(armored!=true&&fragile==true){armorMulti=armorMultiInit/fragileStrength;}
            //if(armored!=true&&fragile!=true){armorMulti=armorMultiInit;}if(armored==true&&fragile==true){armorMulti=(dmgMultiInit/fragileStrength)*armoredStrength;}
            if(power==true&&weakns!=true){dmgMulti=dmgMultiInit*powerStrength;}else if(power!=true&&weakns==true){dmgMulti=dmgMultiInit/weaknsStrength;}
            if(power!=true&&weakns!=true){dmgMulti=dmgMultiInit;}if(power==true&&weakns==true){dmgMulti=(dmgMultiInit/weaknsStrength)*powerStrength;}
            if(onfire){if(frozen){ResetStatus("frozen");/*Damage(1,dmgType.silent);*/}}
            if(infEnergyTimer>0){infEnergyTimer-=Time.deltaTime;}else{if(infEnergyTimer>-4){ResetStatus("infEnergy");}}
            if(infEnergy){energy=infPrevEnergy;}
            if(speed==true&&slow!=true){moveSpeedCurrent=speedPrev[speedPrev.Count-1]*speedStrength;}else if(speed!=true&&slow==true){moveSpeedCurrent=speedPrev[speedPrev.Count-1]/slowStrength;}
            else if(speed&&slow){if(speedPrev.Count>1)moveSpeedCurrent=speedPrev[speedPrev.Count-2]/slowStrength*speedStrength;
            else moveSpeedCurrent=speedPrev[speedPrev.Count-1]/slowStrength*speedStrength;}
            //if(speeded!=true&&slowed!=true){moveSpeedCurrent=moveSpeed;}if(speeded==true&&slowed==true){}
        }
    }
    void CalculateDefenseSpeed(){
        int defArmored=0;
        //if(armoredStrength)
        defense=defenseModifBase+defArmored;
    }
    
    private void Shadow(){
        if(!GameSession.GlobalTimeIsPaused&&instantiateTimer<=0){
            GameObject shadow=null;
            if(dashingEnabled){shadow=GameAssets.instance.Make("PlayerShadow",transform.position);instantiateTimer=instantiateTime;}
            else{shadow=GameAssets.instance.Make("PlayerShadowColli",transform.position);instantiateTimer=instantiateTime*4f;}
            shadow.transform.localScale=new Vector3(shipScale,shipScale,1);
            Destroy(shadow.gameObject,shadowLength);
            
            //yield return new WaitForSeconds(0.2f);
        }
    }
    void Regen(){//Move to a universal modules script instead of PlayerSkills
        if(!GameSession.GlobalTimeIsPaused){
            hpAbsorpAmnt=Mathf.Clamp(hpAbsorpAmnt,0,healthMax);
            enAbsorpAmnt=Mathf.Clamp(enAbsorpAmnt,0,energyMax);
            if(UpgradeMenu.instance.crMendEnabled&&hpAbsorpAmnt<=0){if(GameSession.instance.coins>=crystalMend_refillCost){HPAbsorp(crystalMendAbsorp);GameSession.instance.coins-=crystalMend_refillCost;}}
            if(UpgradeMenu.instance.enDissEnabled&&enAbsorpAmnt<=0){if(GameSession.instance.xp>=energyDiss_refillCost){EnAbsorp(energyDissAbsorp);GameSession.instance.xp-=energyDiss_refillCost;}}
            if(hpAbsorpAmnt>0&&timerHpRegen>=freqHpRegen){if(health<healthMax)Damage(hpRegenAmnt,dmgType.heal);HPAbsorp(-hpRegenAmnt);timerHpRegen=0;}
            if(energyOn)if(enAbsorpAmnt>0&&timerEnRegen>=freqEnRegen){if(energy<energyMax)AddSubEnergy(enRegenAmnt,true);EnAbsorp(-enRegenAmnt);timerEnRegen=0;}
        }
    }
    public void Recoil(float strength, float time){if(recoilOn)StartCoroutine(RecoilI(strength,time));}
    IEnumerator RecoilI(float strength,float time){
        Shake.instance.CamShake(0.1f,1/(time*4));
        if(SaveSerial.instance.settingsData.vibrations)Vibrator.Vibrate(2);
        rb.velocity = Vector2.down*strength;
        yield return new WaitForSeconds(time);
        rb.velocity=Vector2.zero;
    }
    
    public void OnFire(float duration,float strength=1){
        onfireTime=duration;
        SetStatus("onfire");
        StartCoroutine(OnFireI(strength));
    }
    IEnumerator OnFireI(float strength){while(onfire){
        Damage(onfireDmg*strength,dmgType.flame);
        yield return new WaitForSeconds(onfireTickrate);
    }yield break;}
    public void Decay(float duration,float strength=1){
        decayTime=duration;
        SetStatus("decay");
        StartCoroutine(DecayI(strength));
    }
    IEnumerator DecayI(float strength){while(decay){
        Damage(decayDmg*strength,dmgType.decay);
        yield return new WaitForSeconds(decayTickrate);
    }yield break;}
    public void Electrc(float duration){
        electrcTime=duration;
        SetStatus("electrc");
    }
    public void Freeze(float duration){
        frozenTime=duration;
        SetStatus("frozen");
    }
    public void Armor(float duration,/*int*/float strength=1){
        armoredTime=duration;
        if(strength!=1)armoredStrength=strength;
        else armoredStrength=1.5f;
        SetStatus("armored");
    }
    public void Fragile(float duration,/*int*/float strength=1){
        fragileTime=duration;
        if(strength!=1)fragileStrength=strength;
        else fragileStrength=1.5f;
        SetStatus("fragile");
    }public void Power(float duration,float strength=1){
        powerTime=duration;
        if(strength!=1)powerStrength=strength;
        else powerStrength=1.5f;
        SetStatus("power");
    }public void Weaken(float duration,float strength=1){
        weaknsTime=duration;
        if(strength!=1)weaknsStrength=strength;
        else weaknsStrength=1.5f;
        SetStatus("weakns");
    }public void Hack(float duration){
        hackedTime=duration;
        SetStatus("hacked");
    }public void Blind(float duration,float strength=1){
        blindTime=duration;
        if(strength!=1)blindStrenght=strength;
        else blindStrenght=1.5f;
        SetStatus("blind");
    }public void Speed(float duration,float strength=1){
        speedTime=duration;
        if(speed!=true){
            SetSpeedPrev();
            if(strength!=1)speedStrength=strength;
            else speedStrength=1.5f;
            SetStatus("speed");
        }
    }public void Slow(float duration,float strength=1){
        slowTime=duration;
        if(slow!=true){
            SetSpeedPrev();
            if(strength!=1)slowStrength=strength;
            else slowStrength=1.5f;
            SetStatus("slow");
        }
    }public void InfEnergy(float duration){
        infPrevEnergy=energy;
        infEnergyTime=duration;
        SetStatus("infEnergy");
    }
#endregion

#region//Other Functions
    public void SetStatus(string status){
        statusc=status;
        //var v = this.GetType().GetProperty(status);
        if(this.GetType().GetField(status)!=null)this.GetType().GetField(status).SetValue(this,true);
        var i=this.GetType().GetField(status+"Time").GetValue(this);
        this.GetType().GetField((status+"Timer")).SetValue(this,i);
        //v.SetValue(this,true,null);
        //((dynamic)this).status=true;
	    //this.GetType().GetProperty(status+"Timer").SetValue(this,status+"Time");
        ResizeStatuses();
        SortStatuses();
    }public void ResetStatus(string status){
        if(this.GetType().GetField(status)!=null)this.GetType().GetField(status).SetValue(this,false);
        this.GetType().GetField((status+"Timer")).SetValue(this,-4);
        for(var i=0;i<statuses.Count;i++){
            if(statuses[i]==status){statuses[i]="";}
        }
        ResortStatuses();
    }void SortStatuses(){
        //ResizeStatuses();
        //Array.Resize(ref statuses,statuses.Length+1);
        //for(var i=0;i<statuses.Count;i++){
        //if(statuses[i]!=statusc){
        if(!statuses.Contains(statusc)){
            statuses.Add(statusc);statusc="";
            //if(i>0&&statuses[i]!=""){statuses.Add(statusc);statusc="";}
            //if(status3=="" && status2!="" && status1!=""){status3=statusc;statusc="";}
        //}
        }
    }void ResortStatuses(){
        for(var i=0;i<statuses.Count-1;i++){
            if(i>-1&&i<statuses.Count){
                if(statuses[i]=="" && statuses[i+1]!=""){statuses[i]=statuses[i+1];statuses[i+1]="";statuses.Remove(statuses[i+1]);}
                //if(status2=="" && status3!=""){status2=status3;status3="";}
                //if(status3=="" && status2!="" && status1!=""){status3=statusc;statusc="";}
            }
        }
        ResizeStatuses();
    }public void ResizeStatuses(){statuses.RemoveAll(x=>x=="");}

    public void ResetStatusRandom(){
        var i=UnityEngine.Random.Range(0,statuses.Count-1);
        statuses[i]="";
        ResortStatuses();
    }

    public bool _isPowerupEmpty(Powerup powerup){bool b=false;if(powerup==null){b=true;}else if(powerup!=null&&String.IsNullOrEmpty(powerup.name)){b=true;}return b;}
    public bool _isPowerupEmptyID(int id){bool b=false;if(powerups[id]==null){b=true;}else if(powerups[id]!=null&&String.IsNullOrEmpty(powerups[id].name)){b=true;}return b;}
    public bool _isPowerupEmptyCur(){bool b=false;if(powerups[powerupCurID]==null){b=true;}else if(powerups[powerupCurID]!=null&&String.IsNullOrEmpty(powerups[powerupCurID].name)){b=true;}return b;}
    public Powerup GetPowerup(int id){Powerup pwrup=null;if(id<powerups.Length){
        pwrup=powerups[id];}return pwrup;}
    public Powerup GetPowerupStr(string str){Powerup pwrup=null;
        pwrup=Array.Find(powerups,x=>x.name==str);return pwrup;}
    public Powerup _curPwrup(){Powerup pwrup=null;if(powerups.Length>powerupCurID){pwrup=powerups[powerupCurID];}return pwrup;}
    public string _curPwrupName(){string str="";if(powerups.Length>powerupCurID){if(_curPwrup()!=null)str=_curPwrup().name;}return str;}
    public bool ContainsPowerup(string str){bool b=false;if(Array.Exists(powerups,x=>x.name==str)){b=true;}return b;}
    public void SetPowerup(Powerup val){if(!ContainsPowerup(val.name)){
        if(!SaveSerial.instance.settingsData.alwaysReplaceCurrentSlot){int emptyCount=0;
            for(var i=0;i<powerups.Length;i++){
                if(_isPowerupEmpty(powerups[i])){emptyCount++;
                    powerups[i]=val;
                    if(SaveSerial.instance.settingsData.autoselectNewItem){powerupCurID=i;}
                    break;
                }
            }if(emptyCount==0){powerups[powerupCurID]=new Powerup();powerups[powerupCurID]=val;}
        }else{powerups[powerupCurID]=val;}
        WeaponProperties w=null;if(GetWeaponProperty(val.name)!=null){w=GetWeaponProperty(val.name);}
        if(w!=null&&w.duration>0&&weaponsLimited){powerups[powerupCurID].timer=w.duration;}
            else if(w==null){Debug.LogWarning("WeaponProperty for "+val.name+" not defined");}
    }}
    public void SetPowerupStr(string val){if(!ContainsPowerup(val)){
        if(!SaveSerial.instance.settingsData.alwaysReplaceCurrentSlot){int emptyCount=0;
            for(var i=0;i<powerups.Length;i++){
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
        WeaponProperties w=null;if(GetWeaponProperty(val)!=null){w=GetWeaponProperty(val);}
        if(w!=null&&w.duration>0&&weaponsLimited){powerups[powerupCurID].timer=w.duration;}
            else if(w==null){Debug.LogWarning("WeaponProperty for "+val+" not defined");}
    }}

    public void ReplacePowerupName(string str, string rep){if(GetPowerupStr(str)!=null){GetPowerupStr(str).name=rep;Debug.Log("Powerup "+str+" replaced with "+rep);}else{Debug.Log("Powerup "+str+" is null!");}}

    public void ClearPowerup(string name){powerups[Array.FindIndex(powerups,x=>x.name==name)]=new Powerup();}
    public void ClearCurrentPowerup(){powerups[powerupCurID]=new Powerup();}
    public void ResetPowerupDef(){powerups[powerupCurID]=new Powerup();powerups[powerupCurID].name=powerupDefault;}
    public void SelectAnyNotEmptyPowerup(){
        int i=-1;
        i=Array.FindIndex(powerups,x=>x.name==powerupDefault);//prefer the default
        if(i==-1){Array.FindIndex(powerups,x=>!String.IsNullOrEmpty(x.name));}
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

    public void HPAdd(float hp){Damage(hp,dmgType.heal);}
    public void Damage(float dmg, dmgType type,bool ignoreInvert=true, float electrTime=4f){//Later add on possible Inverter options?
        if(type!=dmgType.heal&&type!=dmgType.healSilent&&type!=dmgType.decay&&!gclover)if(dmg!=0){var dmgTot=(float)System.Math.Round(dmg,2);health-=dmgTot;HpPopUpHUD(-dmgTot);}
        else if(gclover){AudioManager.instance.Play("GCloverHit");}

        if(type==dmgType.silent){damaged=true;}
        if(type==dmgType.normal){damaged=true;AudioManager.instance.Play("ShipHit");}
        if(type==dmgType.flame){flamed=true;AudioManager.instance.Play("Overheat");}
        if(type==dmgType.decay){if(dmg!=0&&health>dmg*2){var dmgTot=(float)System.Math.Round(dmg,2);health-=dmgTot;HpPopUpHUD(-dmgTot);damaged=true;AudioManager.instance.Play("Decay");}}
        if(type==dmgType.electr){electricified=true;Electrc(electrTime);}//electricified=true;AudioManager.instance.Play("Electric");}
        if(type==dmgType.shadow){shadowed=true;AudioManager.instance.Play("ShadowHit");}
        if(type==dmgType.heal){healed=true;if(dmg!=0){health+=dmg;HpPopUpHUD(dmg);UniCollider.DMG_VFX(2,GetComponent<Collider2D>(),transform,-dmg);}}
        if(type==dmgType.healSilent){if(dmg!=0){health+=dmg;HpPopUpHUD(dmg);}}
    }
    public void AddSubEnergy(float value, bool add=false,bool ignoreInvert=false){
        if(energyOn&&!infEnergy){
            if(inverter!=true||ignoreInvert){
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
        var pwrup=Array.Find(powerups,x=>x.name==name);
        if(pwrup!=null){
            if(pwrup.ammo<0)pwrup.ammo=0;
            if(inverter!=true||ignoreInvert){
                if(add){pwrup.ammo+=v;AmmoPopUpHUD(v);}
                else{if(pwrup.ammo>=v)pwrup.ammo-=v;AmmoPopUpHUD(-v);}
            }else{
                if(add){if(pwrup.ammo>=v)pwrup.ammo-=v;AmmoPopUpHUD(-v);}
                else{pwrup.ammo+=v;AmmoPopUpHUD(v);}
            }
        }
    }
    public void AddSubCoins(int value, bool add=true,bool ignoreInvert=false){
        if(inverter!=true||ignoreInvert){
            if(add){GameSession.instance.coins+=value;CoinsPopUpHUD(value);}
            else{GameSession.instance.coins-=value;CoinsPopUpHUD(-value);}
        }else{
            if(add){GameSession.instance.coins-=value;CoinsPopUpHUD(-value);}
            else{GameSession.instance.coins+=value;CoinsPopUpHUD(value);}
        }
    }
    public void AddSubXP(float value, bool add=true,bool ignoreInvert=false){
        if(inverter!=true||ignoreInvert){
            if(add){GameSession.instance.AddXP(value);}
            else{GameSession.instance.AddXP(-value);}
        }else{
            if(add){GameSession.instance.AddXP(-value);}
            else{GameSession.instance.AddXP(value);}
        }
    }
    public void AddSubCores(int value, bool add=true,bool ignoreInvert=false){
        if(inverter!=true||ignoreInvert){
            if(add){GameSession.instance.cores+=value;CoresPopUpHUD(value);}
            else{GameSession.instance.cores-=value;CoresPopUpHUD(-value);}
        }else{
            if(add){GameSession.instance.cores-=value;CoresPopUpHUD(-value);}
            else{GameSession.instance.cores+=value;CoresPopUpHUD(value);}
        }
    }
    public void HPAbsorp(float value, bool add=true,bool ignoreInvert=true){
        if(inverter!=true||ignoreInvert){
            if(add){hpAbsorpAmnt+=value;HpAbsorpPopUpHUD(value);}
            else{hpAbsorpAmnt-=value;HpAbsorpPopUpHUD(-value);}
        }else{
            if(add){hpAbsorpAmnt-=value;HpAbsorpPopUpHUD(-value);}
            else{hpAbsorpAmnt+=value;HpAbsorpPopUpHUD(value);}
        }
    }
    public void EnAbsorp(float value, bool add=true,bool ignoreInvert=true){
        if(inverter!=true||ignoreInvert){
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
            if(inverter!=true||ignoreInvert){
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
    public WeaponProperties GetWeaponProperty(string name){
        foreach(WeaponProperties w in weaponProperties){
            if(w.weaponType==weaponType.bullet){
                if(w.name==name){return w;}//else{Debug.LogWarning("No WeaponProperty by name: "+name);return null;}
            }else if(w.weaponType==weaponType.melee){
                weaponTypeMelee wp=(weaponTypeMelee)w.weaponTypeProperties;
                if(w.name==name){return w;}//else{Debug.LogWarning("No WeaponProperty by name: "+name);return null;}
            }
        }return null;
    }
    void LosePowerup(){
        if(losePwrupOutOfEn&&energy<=0&&!ComparePowerupStrCur(powerupDefault)){ResetPowerupDef();}
        if(!_isPowerupEmptyCur()){if(ammoOn&&((GetWeaponProperty(_curPwrupName())!=null&&GetWeaponProperty(_curPwrupName()).costType==costType.ammo))){if(losePwrupOutOfAmmo&&_curPwrup().ammo<=0)ResetPowerupDef();}}
    }

    public void SetSpeedPrev(){
        //if(speedPrev.Count==1&&speedPrev[0]==moveSpeed){speedPrev[0]=moveSpeedCurrent;}
        //else{speedPrev.Add(moveSpeedCurrent);}
        if(moveSpeedCurrent!=moveSpeed){speedPrev.Add(moveSpeedCurrent);}
    }
    public void RevertToSpeedPrev(){
        if(speedPrev.Count>1){moveSpeedCurrent=speedPrev[speedPrev.Count-1];speedPrev.Remove(speedPrev[speedPrev.Count-1]);}
        else{moveSpeedCurrent=moveSpeed;speedPrev[0]=moveSpeed;}
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