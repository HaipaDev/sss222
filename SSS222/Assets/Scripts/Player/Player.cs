using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;
//using UnityEditor;

public class Player : MonoBehaviour{
#region Vars
#region//Basic Player Values
    [Header("Player")]
    [SerializeField] public bool moveX = true;
    [SerializeField] public bool moveY = true;
    [SerializeField] bool moveByMouse = false;
    [SerializeField] public bool autoShoot = false;
    [SerializeField] float paddingX = -0.125f;
    [SerializeField] float paddingY = 0.45f;
    [SerializeField] public float moveSpeedInit = 5f;
    [SerializeField] public float lsaberSpeedMulti = 1.25f;
    public float moveSpeed = 5f;
    public float moveSpeedCurrent;
    public float health = 100f;
    [SerializeField] public float maxHP = 100f;
    [SerializeField] public string powerup = "null";
    [SerializeField] public bool energyOn = true;
    public float energy = 120f;
    //[SerializeField] public float maxEnergyStarting = 30f;
    [SerializeField] public float maxEnergy = 120f;
    [SerializeField] public bool ammoOn=true;
    [SerializeField] public int ammo = -4;
    [SerializeField] public string powerupDefault = "laser";
    public bool weaponsLimited=false;
    public float powerupTimer=-4;
    [SerializeField] public bool losePwrupOutOfEn;
    [SerializeField] public bool losePwrupOutOfAmmo;
    [SerializeField] public bool fuelOn=false;
    [SerializeField] public float fuelDrainAmnt=0.1f;
    [SerializeField] public float fuelDrainFreq=0.5f;
    [SerializeField] public float fuelDrainTimer=-4;
    [SerializeField] public int energyRefillUnlocked;
    [SerializeField] public bool overheatOn=true;
    public float overheatTimer = -4f;
    [SerializeField] public float overheatTimerMax = 8.66f;
    [SerializeField] public float overheatCdTimer;
    [SerializeField] public float overheatCooldown = 0.65f;
    public bool overheated;
    [SerializeField] public float overheatedTime=3;
    public float overheatedTimer;
    [SerializeField] public bool recoilOn=true;

    public bool enRegenEnabled;
    public float enAbsorpAmnt;
    public float energyDissAbsorp=18;
    public float timerEnRegen;
    [SerializeField] public float freqEnRegen=1f;
    [SerializeField] public float enRegenAmnt=2f;
    [SerializeField] public float energyForRegen=10f;
    public bool hpRegenEnabled;
    public float hpAbsorpAmnt;
    public float crystalMendAbsorp=10;
    public float timerHpRegen;
    [SerializeField] public float freqHpRegen=2f;
    [SerializeField] public float hpRegenAmnt=0.5f;
    //[SerializeField] public float hpForRegen=0f;
    [SerializeField] public float armorMultiInit=1f;
    [SerializeField] public float dmgMultiInit=1f;
    [SerializeField] public float shootMultiInit=1f;
    public float armorMulti=1f;
    public float dmgMulti=1f;
    public float shootMulti=1f;
    [SerializeField] public float shipScaleDefault=0.89f;
    [SerializeField] public bool bulletResize;
#endregion
#region//States
    [Header("States")]
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
    public float speedPrev;
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
    [SerializeField] public float shadowTime=10f;
    [SerializeField] public float shadowLength=0.33f;
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
    [SerializeField] public float microMedkitHpAmnt=10f;
    [SerializeField] public float medkitHpAmnt=25f;
    [SerializeField] public float pwrupEnergyGet=36f;
    [SerializeField] public float enForPwrupRefill=25f;
    [SerializeField] public float enPwrupDuplicate=42f;
    [SerializeField] public int crystalMend_refillCost=2;
    [SerializeField] public float energyDiss_refillCost=3.3f;
#endregion
#region//Other
    [Header("Effects")]
    #region//VFX
    GameObject explosionVFX;
    [HideInInspector]public GameObject flareHitVFX;
    GameObject flareShootVFX;
    GameObject shadowShootVFX;
    GameObject gcloverVFX;
    [HideInInspector]public GameObject gcloverOVFX;
    //GameObject crystalExplosionVFX;
    #endregion
    [Header("Others")]
    [SerializeField] GameObject shadowPrefab;
    MeshRenderer bgSprite;
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
    float weaponEnTimer;
    [HideInInspector]public Vector2 mousePos;
    [HideInInspector]public Vector2 mouseDir;
    [SerializeField]public float dist;
    [HideInInspector]public Vector2 velocity;
    public float shipScale=1f;
    float hPressedTime;
    float vPressedTime;
    public float mPressedTime;
    public float timeFlyingCore;
    public float timeFlyingTotal;
    public float stayingTimer;
    public float stayingTimerCore;
    public float stayingTimerTotal;

    Rigidbody2D rb;
    PlayerSkills pskills;
    Shake shake;
    PauseMenu pauseMenu;
    [HideInInspector]public Joystick joystick;
    //Settings settings;
    IEnumerator shootCoroutine;
    //FollowMouse followMouse;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

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
    //public @InputMaster inputMaster;
#endregion
#endregion
    private void Awake() {StartCoroutine(SetGameRuleValues());}
    void Start(){
        if(GameSession.maskMode!=0)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
        
        rb=GetComponent<Rigidbody2D>();
        pskills=GetComponent<PlayerSkills>();
        shake=GameObject.FindObjectOfType<Shake>();
        joystick=FindObjectOfType<VariableJoystick>();
        SetUpMoveBoundaries();
        dashTime=startDashTime;
        if(SaveSerial.instance.settingsData.inputType==InputType.mouse)SetMoveByMouse(true);else SetMoveByMouse(false);
        pauseMenu=FindObjectOfType<PauseMenu>();

        SetPrefabs();
        moveXwas=moveX;
        moveYwas=moveY;

        bgSprite=GameObject.Find("BG ColorM").GetComponent<MeshRenderer>();
        //inputMaster.Player.Shoot.performed += _ => Shoot();
        //if(!speeded&&!slowed){speedPrev=moveSpeedInit;}
    }
    void SetPrefabs(){
        explosionVFX=GameAssets.instance.GetVFX("Explosion");
        flareHitVFX=GameAssets.instance.GetVFX("FlareHit");
        flareShootVFX=GameAssets.instance.GetVFX("FlareShoot");
        //shadowShootVFX=GameAssets.instance.GetVFX("ShadowTrail");
        //gcloverVFX=GameAssets.instance.GetVFX("GCloverVFX");
        //gcloverOVFX=GameAssets.instance.GetVFX("GCloverOutVFX");
        //crystalExplosionVFX=GameAssets.instance.GetVFX("CExplVFX");
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
        maxHP=i.maxHPPlayer;
        energyOn=i.energyOnPlayer;
        energy=i.energyPlayer;
        maxEnergy=i.maxEnergyPlayer;
        ammoOn=i.ammoOn;
        fuelOn=i.fuelOn;
        fuelDrainAmnt=i.fuelDrainAmnt;
        fuelDrainFreq=i.fuelDrainFreq;
        powerup=i.powerupStarting;powerupDefault=i.powerupDefault;
        weaponsLimited=i.weaponsLimited;
        losePwrupOutOfEn=i.losePwrupOutOfEn;losePwrupOutOfAmmo=i.losePwrupOutOfAmmo;
        armorMultiInit=i.armorMultiPlayer;
        dmgMultiInit=i.dmgMultiPlayer;
        shootMultiInit=i.shootMultiPlayer;
        shipScaleDefault=i.shipScaleDefault;
        bulletResize=i.bulletResize;
        overheatOn=i.overheatOnPlayer;
        recoilOn=i.recoilOnPlayer;
        ///State Defaults
        flipTime=i.flipTime;
        gcloverTime=i.gcloverTime;
        shadowTime=i.shadowTime;
        shadowLength=i.shadowLength;
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
        //GameRules gr = (GameRules)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Config&UI/GameRulesets/GRDef.prefab", typeof(GameRules));
        GameRules gr=null;
        gr=GameCreator.instance.gamerulesetsPrefabs[GameSession.instance.gameModeSelected];
        if(gr!=null){
        foreach(WeaponProperties wi in gr.weaponProperties){
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
        microMedkitHpAmnt=i.microMedkitHpAmnt;
        medkitHpAmnt=i.medkitHpAmnt;
        pwrupEnergyGet=i.pwrupEnergyGet;
        enForPwrupRefill=i.enForPwrupRefill;
        enPwrupDuplicate=i.enPwrupDuplicate;

        yield return new WaitForSecondsRealtime(0.06f);
        var u=UpgradeMenu.instance;
        if(GameSession.instance.gameModeSelected==0){
        if(u!=null){
            maxHP+=(Mathf.Clamp(u.maxHealth_UpgradesLvl-1,0,999)*(u.maxHealth_UpgradesCountMax*u.maxHealth_UpgradeAmnt))+(u.maxHealth_UpgradeAmnt*u.maxHealth_UpgradesCount);/*if(u.total_UpgradesLvl>0)*/health=maxHP;
            maxEnergy+=(Mathf.Clamp(u.maxEnergy_UpgradesLvl-1,0,999)*(u.maxEnergy_UpgradesCountMax*u.maxEnergy_UpgradeAmnt))+(u.maxEnergy_UpgradeAmnt*u.maxEnergy_UpgradesCount);if(u.total_UpgradesLvl>0)energy=maxEnergy;
        }else{Debug.LogError("UpgradeMenu not found");}
        }
    }
        moveSpeed=moveSpeedInit;
        moveSpeedCurrent=moveSpeed;
        shootMulti=shootMultiInit;
        dmgMulti=dmgMultiInit;
        armorMulti=armorMultiInit;
    }

    void Update(){
        //if(SaveSerial.instance!=null)if(SaveSerial.instance.settingsData.inputType==InputType.mouse)moveByMouse=true;else moveByMouse=false;
        HandleInput(false);
        energy=Mathf.Clamp(energy,0,maxEnergy);
        health=Mathf.Clamp(health,0,maxHP);
        LosePowerup();
        if(!ammoOn)ammo=-4;
        DrawOtherWeapons();
        if(GetComponent<PlayerSkills>()!=null){if(GetComponent<PlayerSkills>().timerTeleport==-4){Shoot();}}else{Shoot();}
        States();
        Regen();
        Die();
        CountTimeMovementPressed();
        if(frozen!=true&&(!fuelOn||(fuelOn&&energy>0))){
            if(GetComponent<BackflameEffect>().enabled==false){GetComponent<BackflameEffect>().enabled=true;}
            if(transform.GetChild(0)!=null){if(transform.GetChild(0).gameObject.activeSelf==false){transform.GetChild(0).gameObject.SetActive(true);}}
            if(moveByMouse!=true){MovePlayer();}
            else{MoveWithMouse();}
        }else{
            if(GetComponent<BackflameEffect>().enabled==true){GetComponent<BackflameEffect>().enabled=false;}
            if(transform.GetChild(0)!=null){if(transform.GetChild(0).gameObject.activeSelf==true){transform.GetChild(0).gameObject.SetActive(false);}}
        }
        if(shootTimer>0)shootTimer -= Time.deltaTime;
        if(instantiateTimer>0)instantiateTimer-=Time.deltaTime;
        velocity=rb.velocity;
        if(moving==false){stayingTimer+=Time.deltaTime;stayingTimerCore+=Time.deltaTime;stayingTimerTotal+=Time.deltaTime;/*if(hpRegenEnabled)*/timerHpRegen+=Time.deltaTime;}
        if(moving==true){timeFlyingTotal+=Time.deltaTime;timeFlyingCore+=Time.deltaTime;stayingTimer=0;stayingTimerCore=0;
        if(fuelOn){if(fuelDrainTimer<=0){if(fuelDrainTimer!=-4&&energy>0){AddSubEnergy(fuelDrainAmnt,false);}fuelDrainTimer=fuelDrainFreq;}else{fuelDrainTimer-=Time.deltaTime;}}
        }

        
        if(overheatOn){
            if(overheatCdTimer>0)overheatCdTimer-=Time.deltaTime;
            if(overheatCdTimer<=0&&overheatTimer>0)overheatTimer-=Time.deltaTime*2;
            if(overheatTimer>=overheatTimerMax&&overheatTimerMax!=-4&&overheated!=true){OnFire(3.8f,1);
            overheatTimer=-4;overheated=true;overheatedTimer=overheatedTime;}
            if(overheated==true&&overheatedTimer>0&&Time.timeScale>0.0001f){overheatedTimer-=Time.deltaTime;
                //GameObject flareL = Instantiate(flareShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                //Destroy(flareL.gameObject, 0.04f);
                GameAssets.instance.VFX("Flare",new Vector2((transform.position.x+0.35f)*shipScale,(transform.position.y+flareShootYY)*shipScale),0.04f);
                GameAssets.instance.VFX("Flare",new Vector2((transform.position.x-0.35f)*shipScale,(transform.position.y+flareShootYY)*shipScale),0.04f);
                }
            if(overheatedTimer<=0&&overheatTimerMax!=4&&overheated!=false){overheated=false;if(autoShoot){shootCoroutine=null;Shoot();}}
        }//else{}
        //if((autoShoot&&energyOn&&energy>0)&&(shootTimer<=0||shootCoroutine==null)){Shoot();}
        //Debug.Log(shootTimer);
        //Debug.LogWarning(shootCoroutine);
        if(Application.platform==RuntimePlatform.Android)/*if(SaveSerial.instance.settingsData.inputType==InputType.touch)*/mousePos=Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

        if(weaponsLimited){if(powerupTimer>0){powerupTimer-=Time.deltaTime;}if(powerupTimer<=0&&powerupTimer!=-4){powerup=powerupDefault;powerupTimer=-4;if(autoShoot){shootCoroutine=null;Shoot();}AudioManager.instance.Play("PowerupOff");}}
    }
    public void SetMoveByMouse(bool set){moveByMouse=set;}
    void FixedUpdate(){
        // If we're first at-bat, handle the input immediately and mark it already-handled.
        //HandleInput(true);
        //MovePlayer();
        //if (!Input.GetButton("Fire1")){if(shootCoroutine!=null){StopCoroutine(shootCoroutine);StopCoroutine(ShootContinuously());}}
        Vector2 mPos=new Vector2(0,0);
        if(moveX&&moveY)mPos=new Vector2(mousePos.x,mousePos.y);
        if(moveX&&!moveY)mPos=new Vector2(mousePos.x,transform.position.y);
        if(!moveX&&moveY)mPos=new Vector2(transform.position.x,mousePos.y);
        dist=Vector2.Distance(mPos, transform.position);
    }
#region//Movement etc
    void HandleInput(bool isFixedUpdate){
        bool hadAlreadyHandled = hasHandledInputThisFrame;
        hasHandledInputThisFrame = isFixedUpdate;
        if (hadAlreadyHandled)
            return;
        /* Perform any instantaneous actions, using Time.fixedDeltaTime where necessary */
    }
    void CountTimeMovementPressed(){
        if(SaveSerial.instance.settingsData.inputType==InputType.touch){
        //if (Application.platform == RuntimePlatform.Android){
            if(joystick.Horizontal>0.2f || joystick.Horizontal<-0.2f){hPressedTime+=Time.unscaledDeltaTime; mPressedTime+=Time.unscaledDeltaTime;}
            if(joystick.Vertical>0.2f || joystick.Vertical<-0.2f){vPressedTime+=Time.unscaledDeltaTime; mPressedTime+=Time.unscaledDeltaTime;}
            if(((joystick.Horizontal>0.2f||joystick.Horizontal<-0.2f)||(joystick.Vertical>0.2f||joystick.Vertical<-0.2f))&&Time.timeScale>0.01f){moving=true;}//Add to total time flying
            if(joystick.Horizontal<=0.2f && joystick.Horizontal>=-0.2f){hPressedTime=0;}
            if(joystick.Vertical<=0.2f && joystick.Vertical>=-0.2f){vPressedTime=0;}
            if((joystick.Horizontal<=0.2f && joystick.Horizontal>=-0.2f)&&(joystick.Vertical<=0.2f && joystick.Vertical>=-0.2f)){mPressedTime=0;moving=false;}
        }else{
        if(moveByMouse!=true){
            if(Input.GetButton("Horizontal")){hPressedTime+=Time.unscaledDeltaTime; mPressedTime+=Time.unscaledDeltaTime;}
            if(Input.GetButton("Vertical")){vPressedTime+=Time.unscaledDeltaTime; mPressedTime+=Time.unscaledDeltaTime;}
            if(Input.GetButton("Horizontal")||Input.GetButton("Vertical")&&Time.timeScale>0.01f){moving=true;}//Add to total time flying
            if(!Input.GetButton("Horizontal")){hPressedTime=0;}
            if(!Input.GetButton("Vertical")){vPressedTime=0;}
            if(!Input.GetButton("Horizontal")&&!Input.GetButton("Vertical")){mPressedTime=0;moving=false;}
        }
        }
    }
    
    private void MovePlayer(){
        var deltaX=0f;
        var deltaY=0f;
        if (Input.GetButtonDown("Horizontal")){
            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= DCLICK_TIME&&(dashDir==0||(dashDir<-1||dashDir>1))) {dashDir=(int)Input.GetAxisRaw("Horizontal")*2; /*Debug.Log(dashDir);*/ DClick(dashDir); /*Debug.Log(dashDir);*/ deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedCurrent * moveDir; }
            else{ deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedCurrent * moveDir; }
            lastClickTime = Time.time;  }
        if(Input.GetButtonDown("Vertical")){
            float timeSinceLastClick = Time.time - lastClickTime;
            if(timeSinceLastClick<=DCLICK_TIME&&(dashDir==0||((dashDir<0&&dashDir>-2)||(dashDir>1||dashDir<2)))){dashDir=(int)Input.GetAxisRaw("Vertical"); /*Debug.Log(dashDir);*/ DClick(dashDir); /*Debug.Log(dashDir);*/ deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedCurrent * moveDir; }
            else{ deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedCurrent * moveDir; }
            lastClickTime = Time.time; }

        //if(Application.platform==RuntimePlatform.Android){
        if(SaveSerial.instance.settingsData.inputType==InputType.touch){
            deltaX = joystick.Horizontal * Time.deltaTime * moveSpeedCurrent * moveDir;
            deltaY = joystick.Vertical * Time.deltaTime * moveSpeedCurrent * moveDir;
        }else{
            deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedCurrent * moveDir;
            deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedCurrent * moveDir;
        }

        var newXpos = transform.position.x;
        var newYpos = transform.position.y;

        if (moveX==true) newXpos = Mathf.Clamp(transform.position.x,xMin,xMax) + deltaX;
        if (moveY==true) newYpos = Mathf.Clamp(transform.position.y,yMin,yMax) + deltaY;
        transform.position = new Vector2(newXpos,newYpos);
        //Debug.Log(timeSinceLastClick);
        //Debug.Log(dashTime);
        
    }

    private void MoveWithMouse(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDir = mousePos - (Vector2)transform.position;
        mousePos.x=Mathf.Clamp(mousePos.x, xMin, xMax);
        mousePos.y=Mathf.Clamp(mousePos.y, yMin, yMax);
        //dist in FixedUpdate()
        float distX=0;if(moveX){distX=Mathf.Abs(mousePos.x-transform.position.x);}
        float distY=0;if(moveY){distY=Mathf.Abs(mousePos.y-transform.position.y);}
        //if((distX>0f||distY>0f)&&(distX<0.35f||distY<0.35f)){dist=0.35f;}
        //if(((moveX&&distX>0f)||(moveY&&distY>0f))&&((moveX&&distX<0.35f)||(moveY&&distY<0.35f))){dist=0.35f;}
        if((moveX&&distX>0f&&distX<0.35f)||(moveY&&distY>0f&&distY<0.35f)){dist=0.35f;}
        //var actualdist = Vector2.Distance(mousePos, transform.position);
        if(dist>=0.3f&&Time.timeScale>0.01f){moving=true;}
        if((moveX&&moveY)&&dist<=0.05f){moving=false;}
        if(((moveX&&!moveY)||!moveX&&moveY)&&dist<=0.24f){moving=false;}
        //var minMoveDist=0f;
        //if(dist<minMoveDist)dist=0;

        float step = moveSpeedCurrent * Time.deltaTime;
        //if(FindObjectOfType<ShootButton>()!=null && FindObjectOfType<ShootButton>().pressed==false)transform.position = Vector2.MoveTowards(transform.position, mousePos*moveDir, step);
        //else if(FindObjectsOfType<ShootButton>()==null){transform.position = Vector2.MoveTowardqs(transform.position, mousePos*moveDir, step);}
        //if(dist>minMoveDist)
        if (moveX && moveY)transform.position = Vector2.MoveTowards(transform.position, mousePos*moveDir, step);
        if (moveX && !moveY)transform.position = Vector2.MoveTowards(transform.position, new Vector2(mousePos.x*moveDir,transform.position.y), step);
        if (!moveX && moveY)transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x,mousePos.y*moveDir), step);

        if(Input.GetButtonDown("Fire2")){
            //moveXwas=moveX;moveX=false;
            //moveYwas=moveY;moveY=false;
            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= DCLICK_TIME){DClick(0); }
            else{ lastClickTime = Time.time; }//moveX=moveXwas;moveY=moveYwas;}
        }

        var newXpos = transform.position.x;
        var newYpos = transform.position.y;

        if(moveX)newXpos = Mathf.Clamp(transform.position.x, xMin, xMax);
        if(moveY)newYpos = Mathf.Clamp(transform.position.y, yMin, yMax);

        //if(dist>minMoveDist)
        transform.position = new Vector2(newXpos, newYpos);
    }

    private void SetUpMoveBoundaries()
    {
        /*Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;*/

        /*xMin = -bgSprite.bounds.size.x + padding;
        xMax = bgSprite.bounds.size.x - padding;
        yMin = -bgSprite.bounds.size.y + padding;
        yMax = bgSprite.bounds.size.y - padding;*/

        xMin = -3.87f + paddingX;
        xMax = 3.87f - paddingX;
        yMin = -6.95f + paddingY;
        yMax = 7f - paddingY;
    }

    private void Shoot(){
        if(Time.timeScale>0.0001f){
            if(SaveSerial.instance.settingsData.inputType!=InputType.touch){
            //if(Application.platform != RuntimePlatform.Android){
                if(!autoShoot){
                    if(Input.GetButtonDown("Fire1")){
                        if(shootCoroutine!=null){return;}
                        else if(shootCoroutine==null&&shootTimer<=0f&&powerup!="null"){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                    }if(!Input.GetButton("Fire1")||shootTimer<-1f){
                        if(shootCoroutine!=null)StopCoroutine(shootCoroutine);
                        shootCoroutine=null;
                        //if(shootCoroutine!=null){
                            //StopCoroutine(shootCoroutine);StopCoroutine(ShootContinuously());StopCoroutine("ShootContinuously");//}
                        if(moving==true)/*if(enRegenEnabled)*/timerEnRegen+=Time.deltaTime;
                    }
                    //if (Input.GetButtonUp("Fire1")){StopCoroutine(shootCoroutine);}
                }else{
                    if(shootCoroutine!=null){return;}
                    else if(shootCoroutine==null&&shootTimer<=0f&&powerup!="null"){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                    //shootCoroutine=null;
                    if(moving==true)timerEnRegen+=Time.deltaTime;
                }
            }else{
                if(autoShoot){//Autoshoot on Android
                    if(shootCoroutine!=null){return;}
                    else if(shootCoroutine==null&&shootTimer<=0f&&powerup!="null"){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                    //shootCoroutine=null;
                    if(moving==true)timerEnRegen+=Time.deltaTime;
                }
            }
        }
    }

    public void ShootButton(bool pressed){
        if(!autoShoot){
            if(pressed){
                if(shootCoroutine!=null){return;}
                else if(shootCoroutine==null&&shootTimer<=0f&&powerup!="null"){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
            }else if(pressed==false||shootTimer<-1f){
                if(shootCoroutine!=null)StopCoroutine(shootCoroutine);
                shootCoroutine=null;
                if(moving==true)timerEnRegen+=Time.deltaTime;
            }
        }else{return;}//Autoshoot in Shoot()
    }
    public void ShadowButton(Vector2 pos){
        //if(pressed){
            if(moveX&&moveY)tpPos=pos;
            if(moveX&&!moveY)tpPos=new Vector2(pos.x,transform.position.y);
            if(!moveX&&moveY)tpPos=new Vector2(transform.position.x,pos.y);
            DClick(0);
            //float timeSinceLastClick = Time.time - lastClickTime;
            //if (timeSinceLastClick <= DCLICK_TIME){DClick(0); }
            //else{ lastClickTime = Time.time; }
        //}
    }
    public void DClick(int dir){
        //Debug.Log("DClick");
        if(shadow==true && (energy>0||!energyOn)){
            if(SaveSerial.instance.settingsData.inputType!=InputType.touch){
            //if(Application.platform!=RuntimePlatform.Android){
                if(moveByMouse!=true){
                    if(dir<0&&dir>-2) { rb.velocity = Vector2.down * dashSpeed * moveDir; }
                    if(dir>0&&dir<2){ rb.velocity = Vector2.up * dashSpeed * moveDir; }
                    if(dir<-1){ rb.velocity = Vector2.left * dashSpeed * moveDir; }
                    if(dir>1){ rb.velocity = Vector2.right * dashSpeed * moveDir; }
                    dashDir=0;
                    /*if(Application.platform == RuntimePlatform.Android){
                        if(joystick.Vertical<-0.2f) { rb.velocity = Vector2.down * dashSpeed * moveDir; }
                        if(joystick.Vertical>0.2f){ rb.velocity = Vector2.up * dashSpeed * moveDir; }
                        if(joystick.Horizontal<-0.2f){ rb.velocity = Vector2.left * dashSpeed * moveDir; }
                        if(joystick.Horizontal>0.2f){ rb.velocity = Vector2.right * dashSpeed * moveDir; }
                    }else{
                        if(Input.GetAxisRaw("Vertical")<0) { rb.velocity = Vector2.down * dashSpeed * moveDir; }
                        if(Input.GetAxisRaw("Vertical")>0){ rb.velocity = Vector2.up * dashSpeed * moveDir; }
                        if(Input.GetAxisRaw("Horizontal")<0){ rb.velocity = Vector2.left * dashSpeed * moveDir; }
                        if(Input.GetAxisRaw("Horizontal")>0){ rb.velocity = Vector2.right * dashSpeed * moveDir; }
                    }*/
                }else{
                    if(moveX&&moveY)tpPos=mousePos;
                    if(moveX&&!moveY)tpPos=new Vector2(mousePos.x,transform.position.y);
                    if(!moveX&&moveY)tpPos=new Vector2(transform.position.x,mousePos.y);
                    dashed=true;
                    /*Vector2 difference = mousePos - (Vector2)transform.position;
                    difference = difference.normalized;
                    float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                    Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

                    rb.velocity = Vector2.zero;
                    dir = new Vector2(mousePos.x, mousePos.y); // remember that mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    transform.position += dir; // the only thing to change is this cause you need to move for an equal ammount of units each time you attack
                    */
                    //rb.velocity = mousePos * dashSpeed * moveDir;
                    //if(new Vector2(transform.position.x,transform.position.y)==mousePos){rb.velocity=Vector2.zero;}
                    #region//Failed attempts of RayCasting
                    //float maxDist=10;
                    /*
                    shadowRaycast=Physics2D.RaycastAll(transform.position,mousePos,maxDist).ToList();
                    //Ray2D shadowRaycast=new Ray2D(transform.position,mousePos);
                    Debug.DrawRay(transform.position,shadowRaycast[shadowRaycast.Count-1].point,Color.green,0.5f);
                    if(Vector2.Distance(transform.position,mousePos)<=maxDist)transform.position=mousePos;
                    */
                    /*
                    Ray2D ray = new Ray2D(transform.position, mousePos);
                    shadowRaycast = Physics2D.RaycastAll(ray.origin, ray.direction, maxDist).ToList();
                    Debug.Log(shadowRaycast.Count);
                    foreach(RaycastHit2D hit in shadowRaycast){Debug.DrawRay(transform.position,hit.point,Color.green,0.5f);if(hit.collider!=null&&hit.collider.CompareTag("Enemy")){hit.collider.GetComponent<Enemy>().Die();}}
                    */
                    /*
                    int totalObjectsHit = Physics2D.RaycastNonAlloc(transform.position, mousePos, shadowRaycast, maxDist);
                    Debug.Log("Rays:"+totalObjectsHit);
    
                    //Iterate the objects hit by the laser
                    for (int i = 0; i < totalObjectsHit; i++)
                    {
                        RaycastHit2D hit = shadowRaycast[i];
                        Debug.DrawRay(transform.position,hit.point,Color.green,0.5f);
                        //Do something
                        if (hit.collider != null&&hit.collider.CompareTag("Enemy"))
                        {
                            hit.collider.GetComponent<Enemy>().Die();
                        }
                    }
                    var posRaycast = Physics2D.Raycast(transform.position, mousePos, maxDist);
                    transform.position=posRaycast.point;
                    */
                    /*
                    Ray2D ray = new Ray2D(transform.position, mousePos);
                    Debug.DrawRay(transform.position,mousePos, Color.red, 0.5f);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, mousePos);
                    Debug.Log("Hits: "+hits.Length);
                    foreach (RaycastHit2D hit in hits) {
                        Debug.DrawRay(transform.position,hit.point, Color.red, 0.5f);
                        if(hit.collider.CompareTag("Enemy")){Debug.Log("Enemy Hit");hit.collider.gameObject.GetComponent<Enemy>().Die();}
                    }
                    //transform.position=ray.GetPoint(maxDist);
                    */
                    #endregion
                    //transform.position=Vector2.MoveTowards(transform.position,mousePos,300*Time.deltaTime);
                }
            }else{
                dashed=true;
            }
            AddSubEnergy(shadowCost,false);
            dashing = true;
            shadowed = true;
            //AudioSource.PlayClipAtPoint(shadowdashSFX, new Vector2(transform.position.x, transform.position.y));
            AudioManager.instance.Play("Shadowdash");
            dashTime = startDashTime;
            //else{ rb.velocity = Vector2.zero; }
        }//else { dashTime = startDashTime; rb.velocity = Vector2.zero; }
        
    }

    private void Die(){
        if(health <= 0 && dead!=true){
            dead=true;
            Hide();
            Destroy(gameObject, 0.05f);//Kill player
            pskills.DeathSkills();
            if(GameSession.instance.analyticsOn==true){
            AnalyticsResult analyticsResult=Analytics.CustomEvent("Death",
            new Dictionary<string,object>{
                { "Mode: ", GameRules.instance.cfgName },
                { "Score: ", GameSession.instance.score },
                { "Time: ", GameSession.instance.GetGameSessionTime() },
                { "Source: ", GetComponent<PlayerCollider>().lastHitObj },
                { "Damage: ", GetComponent<PlayerCollider>().lastHitDmg },
                { "Full Report: ", GameRules.instance.cfgName+", "+GameSession.instance.score+", "+GameSession.instance.GetGameSessionTimeFormat()+", "+GetComponent<PlayerCollider>().lastHitObj+", "+GetComponent<PlayerCollider>().lastHitDmg }
            });
            Debug.Log("analyticsResult: "+analyticsResult);
            Debug.Log("Full Report: "+GameRules.instance.cfgName+", "+GameSession.instance.score+", "+GameSession.instance.GetGameSessionTimeFormat()+", "+GetComponent<PlayerCollider>().lastHitObj+", "+GetComponent<PlayerCollider>().lastHitDmg);
            }
            //Debug.Log("GameTime: "+GameSession.instance.GetGameSessionTime());

            GameObject explosion = GameAssets.instance.VFX("Explosion",transform.position,0.5f);//Destroy(explosion, 0.5f);
            AudioManager.instance.Play("Death");
            GameOverCanvas.instance.OpenGameOverCanvas(true);

            foreach(Tag_DestroyPlayerDead go in FindObjectsOfType<Tag_DestroyPlayerDead>()){Destroy(go.gameObject);}
        }
    }
    private void Hide(){
        GetComponent<SpriteRenderer>().enabled=false;
        GetComponent<Collider2D>().enabled=false;
        if(GetComponent<BackflameEffect>()!=null)GetComponent<BackflameEffect>().enabled=false;
        if(GetComponent<PlayerSkills>()!=null)GetComponent<PlayerSkills>().enabled=false;
        foreach(Transform c in transform){Destroy(c.gameObject);}
    }
#endregion

#region//Powerups
    public IEnumerator ShootContinuously(){while(true){if(Time.timeScale>0.0001f&&!PauseMenu.GameIsPaused&&!UpgradeMenu.UpgradeMenuIsOpen&&!Shop.shopOpened){
        WeaponProperties w=null;
        if(GetWeaponProperty(powerup)!=null){w=GetWeaponProperty(powerup);}else if(GetWeaponPropertyActive(powerup)!=null){w=GetWeaponPropertyActive(powerup);}else Debug.LogWarning(powerup+" not added to WeaponProperties List");
        if(w!=null){
        costTypeProperties wc=null;
        costTypeCrystalAmmo wcCA=null;
        costTypeBlackEnergy wcBE=null;
        if(w.costType==costType.energy){wc=(costTypeEnergy)w.costTypeProperties;}
        if(w.costType==costType.ammo){wc=(costTypeAmmo)w.costTypeProperties;}
        if(w.costType==costType.crystalAmmo){wc=(costTypeCrystalAmmo)w.costTypeProperties;wcCA=(costTypeCrystalAmmo)wc;}
        if(w.costType==costType.blackEnergy){wc=(costTypeBlackEnergy)w.costTypeProperties;wcBE=(costTypeBlackEnergy)wc;}
        if(w.weaponType==weaponType.bullet||w.weaponType==weaponType.hybrid){
            if(w.costType==costType.boomerang&&instantiateTimer<=0){ammo=1;}
            if((w.costType==costType.energy&&((energyOn&&energy>0)||(!energyOn)))
            ||((w.costType==costType.ammo&&ammo>0)||(w.costType==costType.boomerang&&ammo==1))
            ||((w.costType==costType.crystalAmmo)&&((energyOn&&wcCA.regularEnergyCost>0&&energy>0)||(!energyOn)||wcBE.regularEnergyCost==0)&&(ammo>0||GameSession.instance.coins>0))
            ||((w.costType==costType.blackEnergy)&&((energyOn&&wcBE.regularEnergyCost>0&&energy>0)||(!energyOn)||wcBE.regularEnergyCost==0)&&(GameSession.instance.coresXp>0))){
                if(overheated!=true&&electrc!=true){
                    weaponTypeBullet wp=null;
                    if(w.weaponType==weaponType.bullet){wp=(weaponTypeBullet)w.weaponTypeProperties;}
                    string asset=w.assetName;
                    if(w.weaponType==weaponType.hybrid){weaponTypeHybrid hybrid=(weaponTypeHybrid)w.weaponTypeProperties;wp=hybrid.b;asset=hybrid.bulletAsset;}
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
                            if(w.costType!=costType.boomerang)if(bulletL.GetComponent<Rigidbody2D>()!=null)bulletL.GetComponent<Rigidbody2D>().velocity=sL;
                            else{if(bulletR.GetComponent<ShootInArc>()!=null)bulletR.GetComponent<ShootInArc>().Shoot();}
                            if(bulletL.GetComponent<BounceFromEnemies>()!=null)bulletL.GetComponent<BounceFromEnemies>().speed=sL.y;
                            bulletL.transform.Rotate(rL);
                            if(bulletL.GetComponent<IntervalSound>()!=null)bulletL.GetComponent<IntervalSound>().interval=soundIntervalR;
                            if(bulletL.GetComponent<Tag_PlayerWeaponBlockable>()!=null&&w.costType==costType.energy){bulletL.GetComponent<Tag_PlayerWeaponBlockable>().energy=wc.cost/wp.bulletAmount;}
                        }}
                        if(wp.flare){GameAssets.instance.VFX("FlareShoot",posL,wp.flareDur);}
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
                            if(w.costType!=costType.boomerang)if(bulletR.GetComponent<Rigidbody2D>()!=null)bulletR.GetComponent<Rigidbody2D>().velocity=sR;
                            else{if(bulletR.GetComponent<ShootInArc>()!=null)bulletR.GetComponent<ShootInArc>().Shoot();}
                            if(bulletR.GetComponent<BounceFromEnemies>()!=null)bulletR.GetComponent<BounceFromEnemies>().speed=sR.y;
                            bulletR.transform.Rotate(rR);
                            if(bulletR.GetComponent<IntervalSound>()!=null)bulletR.GetComponent<IntervalSound>().interval=soundIntervalR;
                            if(bulletR.GetComponent<Tag_PlayerWeaponBlockable>()!=null&&w.costType==costType.energy){bulletR.GetComponent<Tag_PlayerWeaponBlockable>().energy=wc.cost/wp.bulletAmount;}
                        }}
                        if(wp.flare){GameAssets.instance.VFX("FlareShoot",posR,wp.flareDur);}
                    }
                    if(wp.leftSide)LeftSide();
                    if(wp.rightSide)RightSide();
                    if(wp.randomSide){if(UnityEngine.Random.Range(0,100)<50){LeftSide();}else{RightSide();}}
                    if(flareL!=null)Destroy(flareL.gameObject, wp.flareDur);
                    if(flareR!=null)Destroy(flareR.gameObject, wp.flareDur);
                    if(w.costType==costType.energy){AddSubEnergy(wc.cost,false);}
                    if(w.costType==costType.ammo){if(ammo>=wc.cost)AddSubAmmo(wc.cost,false);else{AddSubAmmo(ammo-wc.cost,false);}}
                    if(w.costType==costType.crystalAmmo){if(ammo>=wcCA.cost)AddSubAmmo(wcCA.cost,false);else{AddSubAmmo(wcCA.crystalAmmoCrafted,true);AddSubCoins(wcCA.crystalCost,false);}AddSubEnergy(wcCA.regularEnergyCost);}
                    if(w.costType==costType.blackEnergy){if(GameSession.instance.coresXp>=wc.cost){AddSubXP(wc.cost,false);}if(w.costType==costType.blackEnergy){AddSubEnergy(wcBE.regularEnergyCost);}}
                    if(w.ovheat!=0&&w.costType!=costType.boomerang)Overheat(w.ovheat);
                    if(w.costType==costType.boomerang){ammo=-1;instantiateTimer=w.ovheat;}
                    if(wp.recoilStrength!=0&&wp.recoilTime>0)Recoil(wp.recoilStrength,wp.recoilTime);
                    shootTimer=(wp.shootDelay/wp.tapDelayMulti)/shootMulti;
                    yield return new WaitForSeconds((wp.shootDelay/wp.holdDelayMulti)/shootMulti);
                }else{yield break;}
                }else{if(!autoShoot){AudioManager.instance.Play("NoEnergy");} shootTimer=0f; shootCoroutine=null; yield break;}
        }else{yield break;}
        }else{yield break;}
    }}}
    private void DrawOtherWeapons(){
        var cargoDist=2.8f;
        GameObject go=null;
        WeaponProperties w=null;
        if(GetWeaponProperty(powerup)!=null)w=GetWeaponProperty(powerup);else if(GetWeaponPropertyActive(powerup)!=null){w=GetWeaponPropertyActive(powerup);}else Debug.LogWarning(powerup+" not added to WeaponProperties List");
        if(w!=null&&(w.weaponType==weaponType.held||w.weaponType==weaponType.hybrid)){
            weaponTypeHeld wp=null;
            if(w.weaponType==weaponType.held){wp=(weaponTypeHeld)w.weaponTypeProperties;}
            if(w.weaponType==weaponType.hybrid){weaponTypeHybrid hybrid=(weaponTypeHybrid)w.weaponTypeProperties;wp=hybrid.h;}
            costTypeProperties wc=null;
            if(w.costType==costType.energy){wc=(costTypeEnergy)w.costTypeProperties;}
            if(powerup!=wp.nameActive&&(w.costType==costType.energy&&energyOn&&energy>0)||(w.costType!=costType.energy||!energyOn)){
                GameObject asset=GameAssets.instance.Get(w.assetName);
                foreach(Transform t in transform){if(t.gameObject.name.Contains(asset.name))go=t.gameObject;}
                //if(go!=null){go.transform.position=go.transform.position+new Vector3(wp.offset.x,wp.offset.y,go.transform.position.z);}
                if(go==null){go=Instantiate(asset,transform);go.transform.position=transform.position+new Vector3(wp.offset.x,wp.offset.y,0);}
                weaponEnTimer=wp.energyPeriod;
                powerup=wp.nameActive;
            }else{
                if(powerup==wp.nameActive){
                    if(weaponEnTimer>0){weaponEnTimer-=Time.deltaTime;}
                    if(weaponEnTimer<=0){weaponEnTimer=wp.energyPeriod;AddSubEnergy((float)System.Math.Round(wc.cost*shipScale,2),false);}
                    GameObject asset=GameAssets.instance.Get(w.assetName);
                    foreach(Transform t in transform){if(t.gameObject.name.Contains(asset.name))go=t.gameObject;}
                    if(powerup==wp.nameActive&&go==null)powerup=w.name;
                }
                else{
                if(go!=null){Destroy(go);}
                if(!speed&&!slow)moveSpeedCurrent=moveSpeed;
                if(losePwrupOutOfEn)powerup=powerupDefault;
                }
            }
            if(go!=null){if(FindObjectOfType<CargoShip>()!=null&&Vector2.Distance(go.transform.position,FindObjectOfType<CargoShip>().transform.position)<cargoDist){
                go.SetActive(false);}else{go.SetActive(true);}
            }
        }
    DestroyHeldWeapons();
    }
#endregion
 
#region//States
    private void States(){
        if(flip==true){flipTimer-=Time.deltaTime;moveDir=-1;}else{moveDir=1;}
        if(flipTimer<=0&&flipTimer>-4){ResetStatus("flip");AudioManager.instance.Play("PowerupOff");}

        if(gclover==true){
            health=maxHP;
            FindObjectOfType<HPBar>().GetComponent<HPBar>().gclover=true;
            gcloverTimer-=Time.deltaTime;
        }
        else{
            FindObjectOfType<HPBar>().GetComponent<HPBar>().gclover=false;
        }
        if(gcloverTimer<=0&&gcloverTimer>-4){ResetStatus("gclover");AudioManager.instance.Play("GCloverOff");}

        if(shadow==true){shadowTimer-=Time.deltaTime;}
        if(shadowTimer<=0&&shadowTimer>-4){ResetStatus("shadow");AudioManager.instance.Play("PowerupOff");}
        if(shadow==true){Shadow();if(GetComponent<BackflameEffect>().enabled==true)GetComponent<BackflameEffect>().enabled=false;}
        else{dashTime=-4;if(GetComponent<BackflameEffect>().enabled==false)GetComponent<BackflameEffect>().enabled=true;}
        if(shadow==true&&dashTime<=0&&dashTime!=-4){rb.velocity=Vector2.zero; dashing=false; /*moveX=moveXwas;moveY=moveYwas;*/ dashTime=-4;}
        else{dashTime-=Time.deltaTime; if(dashTime>0&&dashed){var step=mouseShadowSpeed*Time.deltaTime;transform.position=Vector2.MoveTowards(transform.position,tpPos,step);dashed=false;}/*if(rb.velocity!=Vector2.zero)rb.velocity-=new Vector2(0.01f,0.01f);*/}
        if(energy<=0){shadow=false;}
        if(shadow==false){dashing=false;}

        if(inverter==true){if(FindObjectOfType<InvertAllAudio>().GetComponent<SpriteRenderer>().enabled==false){
        FindObjectOfType<InvertAllAudio>().GetComponent<InvertAllAudio>().revertMusic=false;FindObjectOfType<InvertAllAudio>().GetComponent<InvertAllAudio>().enabled=true;FindObjectOfType<InvertAllAudio>().GetComponent<SpriteRenderer>().enabled=true;}
        inverterTimer+=Time.deltaTime;}
        else{if(FindObjectOfType<InvertAllAudio>().GetComponent<SpriteRenderer>().enabled==true){FindObjectOfType<InvertAllAudio>().GetComponent<SpriteRenderer>().enabled=false;}if(FindObjectOfType<InvertAllAudio>().GetComponent<InvertAllAudio>().revertMusic==false){FindObjectOfType<InvertAllAudio>().GetComponent<InvertAllAudio>().revertMusic=true;}
        if(FindObjectOfType<MusicPlayer>()!=null&&FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().pitch==-1){FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().pitch=1;}}
        if(inverterTimer >=10 && inverterTimer<14){ResetStatus("inverter"); inverterTimer=14;}

        if(magnet==true){
            if(FindObjectsOfType<Tag_MagnetAffected>()!=null){
                magnetTimer-=Time.deltaTime;
                Tag_MagnetAffected[] objs = FindObjectsOfType<Tag_MagnetAffected>();
                foreach(Tag_MagnetAffected obj in objs){
                    var followC = obj.GetComponent<Follow>();
                    if(followC==null){Follow follow = obj.gameObject.AddComponent(typeof(Follow)) as Follow; follow.target=this.gameObject;}
                    else{followC.distReq=6f;followC.speedFollow=5f;}
                }
            }else{
                Tag_Collectible[] objs = FindObjectsOfType<Tag_Collectible>();
                foreach(Tag_Collectible obj in objs){
                    var follow = obj.GetComponent<Follow>();
                    if(follow!=null)Destroy(follow);
                }
            }
        }
        if(magnetTimer<=0&&magnetTimer>-4){ResetStatus("magnet");}
        
        if(scaler==true){
            scalerTimer-=Time.deltaTime;
            //Scaler function in PlayerCollider
        }else{
            shipScale=shipScaleDefault;
        }
        if(scalerTimer <=0 && scalerTimer>-4){ResetStatus("scaler");}
        transform.localScale=new Vector3(shipScale,shipScale,1);
        
        if(matrix==true&&accel==false){
            if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true){
                matrixTimer-=Time.unscaledDeltaTime;//matrixTimer-=Time.deltaTime;
                //if((rb.velocity.x<0.7 && rb.velocity.x>-0.7) || (rb.velocity.y<0.7 && rb.velocity.y>-0.7)){
                //||(moveByMouse==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if(moveByMouse==true && dist<1){
                    GameSession.instance.gameSpeed=dist;
                    GameSession.instance.gameSpeed=Mathf.Clamp(GameSession.instance.gameSpeed,0.05f,GameSession.instance.defaultGameSpeed);
                }else if(moveByMouse==false && SaveSerial.instance.settingsData.inputType!=InputType.touch /*(Application.platform != RuntimePlatform.Android)*/ && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //GameSession.instance.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,GameSession.instance.defaultGameSpeed);
                }else if(moveByMouse==false && SaveSerial.instance.settingsData.inputType==InputType.touch /*(Application.platform == RuntimePlatform.Android)*/ && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,GameSession.instance.defaultGameSpeed);
                }else{
                    if(GameSession.instance.speedChanged!=true)GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
                }
            }else{
                GameSession.instance.gameSpeed=0f;
            }
        }
        if(matrixTimer <=0 && matrixTimer>-4){GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed; ResetStatus("matrix");}

        if(pmultiTimer>0){pmultiTimer-=Time.deltaTime;}
        if(pmultiTimer <=0 && pmultiTimer>-4){GameSession.instance.scoreMulti=GameSession.instance.defaultGameSpeed; ResetStatus("pmulti");}

        if(accel==true&&matrix==false){
            if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true){
                accelTimer-=Time.unscaledDeltaTime;//accelTimer-=Time.deltaTime;
                //if((rb.velocity.x<0.7 && rb.velocity.x>-0.7) || (rb.velocity.y<0.7 && rb.velocity.y>-0.7)){
                //||(moveByMouse==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if(moveByMouse==true && dist>0.35){
                    GameSession.instance.gameSpeed=dist+(1-0.35f);
                    GameSession.instance.gameSpeed=Mathf.Clamp(GameSession.instance.gameSpeed,GameSession.instance.defaultGameSpeed,GameSession.instance.defaultGameSpeed*2);
                }else if(moveByMouse==false && SaveSerial.instance.settingsData.inputType!=InputType.touch /*(Application.platform != RuntimePlatform.Android)*/ && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //GameSession.instance.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,GameSession.instance.defaultGameSpeed,GameSession.instance.defaultGameSpeed*2);
                }else if(moveByMouse==false && SaveSerial.instance.settingsData.inputType==InputType.touch /*(Application.platform == RuntimePlatform.Android)*/ && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,GameSession.instance.defaultGameSpeed,GameSession.instance.defaultGameSpeed*2);
                }else{
                    if(GameSession.instance.speedChanged!=true)GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
                }
            }else{
                GameSession.instance.gameSpeed=0f;
            }
        }
        if(accelTimer <=0 && accelTimer>-4){GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed; ResetStatus("accel");}

        if(accel==true&&matrix==true){
            if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true){
                accelTimer-=Time.unscaledDeltaTime;//accelTimer-=Time.deltaTime;
                matrixTimer-=Time.unscaledDeltaTime;//matrixTimer-=Time.deltaTime;
                //if((rb.velocity.x<0.7 && rb.velocity.x>-0.7) || (rb.velocity.y<0.7 && rb.velocity.y>-0.7)){
                //||(moveByMouse==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if(moveByMouse==true){
                    GameSession.instance.gameSpeed=dist;
                    GameSession.instance.gameSpeed=Mathf.Clamp(GameSession.instance.gameSpeed,0.05f,GameSession.instance.defaultGameSpeed*2);
                }else if(moveByMouse==false && SaveSerial.instance.settingsData.inputType!=InputType.touch /*(Application.platform != RuntimePlatform.Android)*/ && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //GameSession.instance.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,GameSession.instance.defaultGameSpeed*2);
                }else if(moveByMouse==false && SaveSerial.instance.settingsData.inputType==InputType.touch /*(Application.platform == RuntimePlatform.Android)*/ && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    GameSession.instance.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,GameSession.instance.defaultGameSpeed*2);
                }else{
                    if(GameSession.instance.speedChanged!=true)GameSession.instance.gameSpeed=GameSession.instance.defaultGameSpeed;
                }
            }else{
                GameSession.instance.gameSpeed=0f;
            }
        }

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
        if(speedTimer>0){speedTimer-=Time.deltaTime;}else{if(speedTimer>-4)if(speed){speedStrength=1;moveSpeedCurrent=speedPrev;}ResetStatus("speed");}
        if(slowTimer>0){slowTimer-=Time.deltaTime;}else{if(slowTimer>-4)if(slow){slowStrength=1;moveSpeedCurrent=speedPrev;}ResetStatus("slow");}
        if(armored==true&&fragile!=true){armorMulti=armoredStrength;}else if(fragile==true&&armored!=true){armorMulti=1/fragileStrength;}
        if(armored!=true&&fragile!=true){armorMulti=armorMultiInit;}if(armored==true&&fragile==true){armorMulti=(1/fragileStrength)*armoredStrength;}
        if(power==true&&weakns!=true){dmgMulti=powerStrength;}else if(weakns==true&&power!=true){dmgMulti=1/weaknsStrength;}
        if(power!=true&&weakns!=true){dmgMulti=dmgMultiInit;}if(power==true&&weakns==true){dmgMulti=(1/weaknsStrength)*powerStrength;}
        if(onfire){if(frozen){ResetStatus("frozen");/*Damage(1,dmgType.silent);*/}}
        if(infEnergyTimer>0){infEnergyTimer-=Time.deltaTime;}else{if(infEnergyTimer>-4){ResetStatus("infEnergy");}}
        if(infEnergy){energy=infPrevEnergy;}
        if(speed==true&&slow!=true){moveSpeedCurrent=speedPrev*(1.25f*speedStrength);}else if(slow==true&&speed!=true){moveSpeedCurrent=speedPrev/(2*slowStrength);}else if(speed&&slow){moveSpeedCurrent=moveSpeed;}
        //if(speeded!=true&&slowed!=true){moveSpeedCurrent=moveSpeed;}if(speeded==true&&slowed==true){}
    }
    
    private void Shadow(){
        if(Time.timeScale>0.0001f&&instantiateTimer<=0){
            GameObject shadow=Instantiate(shadowPrefab,new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
            shadow.transform.localScale=new Vector3(shipScale,shipScale,1);
            Destroy(shadow.gameObject, shadowLength);
            instantiateTimer=instantiateTime;
            //yield return new WaitForSeconds(0.2f);
        }
    }
    void Regen(){
        //if(UpgradeMenu.instance.crMend_upgraded>0){hpRegenEnabled=true;}
        //if(UpgradeMenu.instance.enDiss_upgraded>0){enRegenEnabled=true;}
        if(Time.timeScale>0.0001f){
            if(hpAbsorpAmnt<0){hpAbsorpAmnt=0;}
            if(enAbsorpAmnt<0){enAbsorpAmnt=0;}
            if(UpgradeMenu.instance.crMendEnabled&&hpAbsorpAmnt==0){if(GameSession.instance.coins>=crystalMend_refillCost){hpAbsorpAmnt+=crystalMendAbsorp;GameSession.instance.coins-=crystalMend_refillCost;}}
            if(UpgradeMenu.instance.enDissEnabled&&enAbsorpAmnt==0){if(GameSession.instance.coresXp>=energyDiss_refillCost){enAbsorpAmnt+=energyDissAbsorp;GameSession.instance.coresXp-=energyDiss_refillCost;}}
            if(hpAbsorpAmnt>0&&timerHpRegen>=freqHpRegen){if(health<maxHP)Damage(hpRegenAmnt,dmgType.heal);hpAbsorpAmnt-=hpRegenAmnt;timerHpRegen=0;}
            if(energyOn)if(enAbsorpAmnt>0&&timerEnRegen>=freqEnRegen){if(energy<maxEnergy)AddSubEnergy(enRegenAmnt,true);enAbsorpAmnt-=enRegenAmnt;timerEnRegen=0;}
            //if(hpRegenEnabled==true&&timerHpRegen>=freqHpRegen){Damage(hpRegenAmnt,dmgType.heal);timerHpRegen=0;}
            //if(energyOn)if(enRegenEnabled==true&&timerEnRegen>=freqEnRegen&&energy>energyForRegen){AddSubEnergy(enRegenAmnt,true);timerEnRegen=0;}
        }
    }
    public void Recoil(float strength, float time){
        //rb.velocity = Vector2.down*strength;
        //Debug.Log(rb.velocity);
        if(recoilOn)StartCoroutine(RecoilI(strength,time));
    }
    IEnumerator RecoilI(float strength,float time){
        shake.CamShake(0.1f,1/(time*4));
        if(SaveSerial.instance.settingsData.vibrations)Vibrator.Vibrate(2);
        rb.velocity = Vector2.down*strength;
        yield return new WaitForSeconds(time);
        rb.velocity=Vector2.zero;
    }

    public void Overhaul(){
        /*List<GameObject> pwrups=new List<GameObject>();
        foreach(GObject go in GameAssets.instance.objects){
            if(go.name.Contains("Pwrup")){
                pwrups.Add(
                    go.gobj);
        }}
        int i=UnityEngine.Random.Range(0,pwrups.Count-1);
        Instantiate(pwrups[i],transform.position,Quaternion.identity);*/
        GameObject randomizer=null;
        foreach(GObject go in GameAssets.instance.objects){if(go.name.Contains("RandomizerPwrup")){randomizer=go.gobj;}}
        if(randomizer!=null)Instantiate(randomizer,transform.position,Quaternion.identity);
    }
    
    public void OnFire(float duration,float strength){
        onfireTime=duration;
        SetStatus("onfire");
        StartCoroutine(OnFireI(strength));
    }
    IEnumerator OnFireI(float strength){
    while(true){
       if(onfire==true){
           Damage(onfireDmg*strength,dmgType.flame);
           yield return new WaitForSeconds(onfireTickrate);
       }else{yield break;}
    }}
    public void Decay(float duration,float strength){
        decayTime=duration;
        SetStatus("decay");
        StartCoroutine(DecayI(strength));
    }
    IEnumerator DecayI(float strength){
    while(true){
       if(decay==true){
           Damage(decayDmg*strength,dmgType.decay);
           yield return new WaitForSeconds(decayTickrate);
       }else{yield break;}
    }}
    public void Electrc(float duration){
        electrcTime=duration;
        SetStatus("electrc");
    }
    public void Freeze(float duration){
        frozenTime=duration;
        SetStatus("frozen");
    }
    public void Armor(float duration,float strength){
        armoredTime=duration;
        armoredStrength=strength;
        SetStatus("armored");
    }
    public void Fragile(float duration,float strength){
        fragileTime=duration;
        fragileStrength=strength;
        SetStatus("fragile");
    }public void Power(float duration,float strength){
        powerTime=duration;
        powerStrength=strength;
        SetStatus("power");
    }public void Weaken(float duration,float strength){
        weaknsTime=duration;
        weaknsStrength=strength;
        SetStatus("weakns");
    }public void Hack(float duration){
        hackedTime=duration;
        SetStatus("hacked");
    }public void Blind(float duration,float strength){
        blindTime=duration;
        blindStrenght=strength;
        SetStatus("blind");
    }public void InfEnergy(float duration){
        infPrevEnergy=energy;
        infEnergyTime=duration;
        SetStatus("infEnergy");
    }public void Speed(float duration,float strength){
        speedPrev=moveSpeedCurrent;
        speedTime=duration;
        speedStrength=strength;
        SetStatus("speed");
    }public void Slow(float duration,float strength){
        speedPrev=moveSpeedCurrent;
        slowTime=duration;
        slowStrength=strength;
        SetStatus("slow");
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
    }void ResizeStatuses(){
        //int notEmpty=0;
        foreach(string status in statuses){
            if(status=="")statuses.Remove(status);//notEmpty++;
        }
        //Array.Resize(ref statuses,notEmpty);
    }

    public void ResetStatusRandom(){
        var i=UnityEngine.Random.Range(0,statuses.Count-1);
        statuses[i]="";
        ResortStatuses();
    }

    public void SetPowerup(string name){
        powerup=name;
        WeaponProperties w=null;if(GetWeaponProperty(powerup)!=null){w=GetWeaponProperty(powerup);}else if(GetWeaponPropertyActive(powerup)!=null){w=GetWeaponPropertyActive(powerup);}
        if(w!=null&&w.duration>0)powerupTimer=w.duration;else Debug.LogWarning("WeeaponProperty not existant");
        //var i=this.GetType().GetField(name+"Duration").GetValue(this);
        //this.GetType().GetField("powerupTimer").SetValue(this,i);
    }

    public void Damage(float dmg, dmgType type){
        if(type!=dmgType.heal&&type!=dmgType.healSilent&&!gclover)if(dmg!=0){var dmgTot=(float)System.Math.Round(dmg/armorMulti,2);health-=dmgTot;HPPopUpHUD(-dmgTot);}
        else if(gclover){AudioManager.instance.Play("GCloverHit");}
        if(type==dmgType.silent){damaged=true;}
        if(type==dmgType.normal){damaged=true;AudioManager.instance.Play("ShipHit");}
        if(type==dmgType.flame){flamed=true;AudioManager.instance.Play("Overheat");}
        if(type==dmgType.decay){damaged=true;AudioManager.instance.Play("Decay");}
        if(type==dmgType.electr){electricified=true;Electrc(4f);}//electricified=true;AudioManager.instance.Play("Electric");}
        if(type==dmgType.shadow){shadowed=true;AudioManager.instance.Play("ShadowHit");}
        if(type==dmgType.heal){healed=true;if(dmg!=0){health+=dmg;HPPopUpHUD(dmg);}}
        if(type==dmgType.healSilent){if(dmg!=0){health+=dmg;HPPopUpHUD(dmg);}}
    }
    public void AddSubEnergy(float value,bool add=false){
    if(energyOn&&!infEnergy){
        if(inverter!=true){
            if(add){energy+=value;EnergyPopUpHUD(value);if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddEnergy(-value);}//EnergyCountVortexWheel-=value;}
            else{energy-=value;EnergyPopUpHUD(-value);if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddEnergy(value);}//EnergyCountVortexWheel+=value;}
        }else{
            if(add){energy-=value;EnergyPopUpHUD(-value);if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddEnergy(value);}//EnergyCountVortexWheel+=value;}
            else{energy+=value;EnergyPopUpHUD(value);if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddEnergy(-value);}//EnergyCountVortexWheel-=value;}
        }
    }}
    public void AddSubAmmo(float value,bool add=false){
        var v=(int)value;
        if(inverter!=true){
            if(add){ammo+=v;AmmoPopUpHUD(v);}
            else{ammo-=v;AmmoPopUpHUD(-v);}
        }else{
            if(add){ammo-=v;AmmoPopUpHUD(-v);}
            else{ammo+=v;AmmoPopUpHUD(v);}
        }
    }
    public void AddSubCoins(int value,bool add=false){
    if(inverter!=true){
        if(add){GameSession.instance.coins+=value;CoinsPopUpHUD(value);}
        else{GameSession.instance.coins-=value;CoinsPopUpHUD(-value);}
    }else{
        if(add){GameSession.instance.coins-=value;CoinsPopUpHUD(-value);}
        else{GameSession.instance.coins+=value;CoinsPopUpHUD(value);}
    }
    }public void AddSubXP(float value,bool add=false){
    if(inverter!=true){
        if(add){GameSession.instance.AddXP(value);}
        else{GameSession.instance.AddXP(-value);}
    }else{
        if(add){GameSession.instance.AddXP(-value);}
        else{GameSession.instance.AddXP(value);}
    }
    }public void AddSubCores(int value,bool add=false){
    if(inverter!=true){
        if(add){GameSession.instance.cores+=value;CoresPopUpHUD(value);}
        else{GameSession.instance.cores-=value;CoresPopUpHUD(-value);}
    }else{
        if(add){GameSession.instance.cores-=value;CoresPopUpHUD(-value);}
        else{GameSession.instance.cores+=value;CoresPopUpHUD(value);}
    }
    }
    public void Overheat(float value,bool add=true){
        if(overheatOn){
        if(overheatTimerMax!=-4){
        if(overheated!=true){
            if(inverter!=true){
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
            }else if(w.weaponType==weaponType.held){
                weaponTypeHeld wp=(weaponTypeHeld)w.weaponTypeProperties;
                if(w.name==name){return w;}
                if(wp.nameActive==name){return null;}
                //if(w.name!=name&&wp.nameActive!=name){Debug.LogWarning("No WeaponProperty by name: "+name);return null;}
            }else if(w.weaponType==weaponType.hybrid){
                weaponTypeHybrid hybrid=(weaponTypeHybrid)w.weaponTypeProperties;
                weaponTypeHeld wp=(weaponTypeHeld)hybrid.h;
                if(w.name==name){return w;}
                if(wp.nameActive==name){return null;}
            }//else{Debug.LogWarning("Type not set in GetWeaponProperty();");}
        }return null;
    }public WeaponProperties GetWeaponPropertyActive(string name){
        foreach(WeaponProperties w in weaponProperties){
            if(w.weaponType==weaponType.held){
                weaponTypeHeld wp=(weaponTypeHeld)w.weaponTypeProperties;
                if(wp.nameActive==name){return w;}
                //if(w.name!=name&&wp.nameActive!=name){Debug.LogWarning("No WeaponProperty by name: "+name);return null;}
            }else if(w.weaponType==weaponType.hybrid){
                weaponTypeHybrid hybrid=(weaponTypeHybrid)w.weaponTypeProperties;
                weaponTypeHeld wp=(weaponTypeHeld)hybrid.h;
                if(wp.nameActive==name){return w;}
            }//else{Debug.LogWarning("Type not set in GetWeaponPropertyActive();");}
        }return null;
    }
    void DestroyHeldWeapons(){
        foreach(WeaponProperties ws in weaponProperties){
            if(ws.weaponType==weaponType.held){
                var wpt=(weaponTypeHeld)ws.weaponTypeProperties;
                if(powerup!=ws.name&&powerup!=wpt.nameActive){
                    GameObject asset=GameAssets.instance.Get(ws.assetName);
                    foreach(Transform t in transform){if(t.gameObject.name.Contains(ws.assetName)){Destroy(t.gameObject);}}
                }
            }
        }
    }
    void LosePowerup(){
        if(losePwrupOutOfEn&&energy<=0){SetPowerup(powerupDefault);}
        if(ammoOn&&((GetWeaponProperty(powerup)!=null&&GetWeaponProperty(powerup).costType==costType.ammo)||(GetWeaponPropertyActive(powerup)!=null&&GetWeaponPropertyActive(powerup).costType!=costType.boomerang)))if(losePwrupOutOfAmmo&&ammo<=0&&ammo!=-4){SetPowerup(powerup=powerupDefault);ammo=-4;}
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
void HPPopUpHUD(float amnt){GameCanvas.instance.HpPopupSwitch(amnt);}
void EnergyPopUpHUD(float amnt){GameCanvas.instance.EnPopupSwitch(amnt);}
void AmmoPopUpHUD(float amnt){GameCanvas.instance.AmmoPopupSwitch(amnt);}
void CoinsPopUpHUD(float amnt){GameCanvas.instance.CoinPopupSwitch(amnt);}
void CoresPopUpHUD(float amnt){GameCanvas.instance.CorePopupSwitch(amnt);}
void ScorePopUpHUD(float amnt){GameCanvas.instance.ScorePopupSwitch(amnt);}
void XPPopUpHUD(float amnt){GameCanvas.instance.XpPopupSwitch(amnt);}
#endregion
}
