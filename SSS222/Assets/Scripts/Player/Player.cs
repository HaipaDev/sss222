using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Player : MonoBehaviour{
    #region Vars
    [HeaderAttribute("Player")]
    [SerializeField] bool moveX = true;
    [SerializeField] bool moveY = true;
    [SerializeField] bool moveByMouse = false;
    [SerializeField] public float moveSpeedInit = 5f;
    [SerializeField] public float lsaberSpeedMulti = 1.25f;
    public float moveSpeed = 5f;
    public float moveSpeedCurrent;
    [SerializeField] float padding = 0.1f;
    [SerializeField] public float health = 200f;
    [SerializeField] public float maxHP = 200f;
    [SerializeField] public string powerup = "laser";
    [SerializeField] public float energy = 80f;
    [SerializeField] public float maxEnergy = 200f;
    [SerializeField] public float overheatTimer = -4f;
    [SerializeField] public float overheatTimerMax = 3f;
    [SerializeField] public float overheatDmg = 10f;
    [SerializeField] public float overheatCdTimer;
    public float overheatCooldown = 1.5f;
    [SerializeField] public bool overheated;
    [SerializeField] public float overheatedTime=3;
    public float overheatedTimer;
    [SerializeField] public string powerupDefault = "laser";
    [SerializeField] public bool losePwrupOutOfEn;
    [SerializeField] public bool energyRefillUnlocked;
    [HeaderAttribute("Weapons")]
    GameObject laserPrefab;
    GameObject phaserPrefab;
    GameObject hrocketPrefab;
    GameObject mlaserPrefab;
    [HideInInspector]public GameObject lsaberPrefab;
    [HideInInspector]public GameObject lclawsPrefab;
    GameObject lclawsVFX;
    GameObject shadowBTPrefab;
    GameObject qrocketPrefab;
    GameObject procketPrefab;
    GameObject cbulletPrefab;
    GameObject plaserPrefab;
    [SerializeField] float laserSpeed = 9f;
    [SerializeField] float laserShootPeriod = 0.34f;
    [SerializeField] float phaserSpeed = 10.5f;
    [SerializeField] float phaserShootPeriod = 0.2f;
    [SerializeField] float hrocketSpeed = 6.5f;
    [SerializeField] float hrocketShootPeriod = 0.3f;
    [SerializeField] float mlaserSpeedS = 8.5f;
    [SerializeField] float mlaserSpeedE = 10f;
    [SerializeField] float mlaserShootPeriod = 0.1f;
    [SerializeField] int mlaserBulletsAmmount = 10;
    [SerializeField] float lsaberEnPeriod = 0.1f;
    [SerializeField] float shadowBTSpeed = 4f;
    [SerializeField] float shadowBTShootPeriod = 0.65f;
    [SerializeField] float qrocketSpeed = 9.5f;
    [SerializeField] float qrocketShootPeriod = 0.3f;
    [SerializeField] float procketSpeedS = 9.5f;
    [SerializeField] float procketSpeedE = 10.5f;
    [SerializeField] float procketShootPeriod = 0.5f;
    [SerializeField] int procketsBulletsAmmount = 10;
    [SerializeField] float cbulletSpeed = 8.25f;
    [SerializeField] float cbulletShootPeriod = 0.15f;
    [SerializeField] float plaserSpeed = 9.5f;
    [SerializeField] float plaserShootPeriod = 0.7f;

    [HeaderAttribute("States")]
    [SerializeField] public bool flip = false;
    [SerializeField] public float flipTime = 7f;
    [HideInInspector]public float flipTimer=-4;
    [SerializeField] public bool gclover = false;
    [SerializeField] public float gcloverTime = 6f;
    [HideInInspector]public float gcloverTimer =-4;
    [SerializeField] public bool shadow = false;
    [SerializeField] public float shadowTime = 10f;
    [HideInInspector]public float shadowTimer = -4;
    [SerializeField] public float shadowLength=0.33f;
    [SerializeField] public float dashSpeed=10f;
    [SerializeField] public float startDashTime=0.2f;
    [HideInInspector]public float dashTime;
    [SerializeField] public bool inverter = false;
    [SerializeField] public float inverterTime=10f;
    [HideInInspector]public float inverterTimer = 14;
    [SerializeField] public bool magnet = false;
    [SerializeField] public float magnetTime=15f;
    [HideInInspector]public float magnetTimer = -4;
    [SerializeField] public bool scaler = false;
    [SerializeField] public float shipScaleMin=0.45f;
    [SerializeField] public float shipScaleMax=2.5f;
    [SerializeField] public float scalerTime=15f;
    [HideInInspector]public float scalerTimer = -4;
    [SerializeField] public bool matrix = false;
    [SerializeField] public float matrixTime=7f;
    [HideInInspector]public float matrixTimer = -4;
    [SerializeField] public float pmultiTime=24f;
    [HideInInspector]public float pmultiTimer = -4;
    [SerializeField] public bool accel = false;
    [SerializeField] public float accelTime=7f;
    [HideInInspector]public float accelTimer = -4;
    [HeaderAttribute("Energy Costs")]
    //Weapons
    [SerializeField] public float laserEn = 0.3f;
    [SerializeField] public float laser2En = 1.25f;
    [SerializeField] public float laser3En = 2.5f;
    [SerializeField] public float phaserEn = 1.5f;
    [SerializeField] public float mlaserEn = 0.075f;
    [SerializeField] public float lsaberEn = 0.375f;
    [SerializeField] public float lclawsEn = 6.3f;
    [SerializeField] public float shadowEn = 5f;
    [SerializeField] public float shadowBTEn = 10f;
    [SerializeField] public float cbulletEn = 1.3f;
    [SerializeField] public float plaserEn = 7f;
    //Rockets
    [SerializeField] public float hrocketEn = 0.2f;//2.6
    [SerializeField] public float qrocketEn = 0.8f;//5.5
    [SerializeField] public float procketEn = 0.86f;//0.26
    [SerializeField] public float procketOh = 0.09f;
    //Collectibles
    [SerializeField] public float energyBallGet = 9f;
    [SerializeField] public float medkitEnergyGet = 26f;
    [SerializeField] public float medkitUEnergyGet = 40f;
    [SerializeField] public float medkitHpAmnt = 25f;
    [SerializeField] public float medkitUHpAmnt = 62f;
    [SerializeField] public float pwrupEnergyGet = 36f;
    [SerializeField] public float enForPwrupRefill = 25f;
    [SerializeField] public float enPwrupDuplicate = 42f;
    public float refillEnergyAmnt=110f;
    public int refillCostS=1;
    public int refillCostE=2;
    public float refillDelay=1.6f;
    public float refillCount;
    public bool refillRandomized;
    [HeaderAttribute("Effects")]
    #region//VFX
    GameObject explosionVFX;
    public GameObject flareHitVFX;
    GameObject flareShootVFX;
    GameObject shadowShootVFX;
    GameObject gcloverVFX;
    public GameObject gcloverOVFX;
    GameObject crystalExplosionVFX;
    #endregion
    #region//SFX
    /*
    //[SerializeField] AudioClip shootLaserSFX;
    [SerializeField] public AudioClip shipHitSFX;
    [SerializeField] public AudioClip explosionSFX;
    [SerializeField] public AudioClip deathSFX;
    [SerializeField] public AudioClip soundwaveHitSFX;
    [SerializeField] public AudioClip powerupSFX;
    [SerializeField] public AudioClip powerupOffSFX;
    [SerializeField] public AudioClip gcloverSFX;
    [SerializeField] public AudioClip gcloverOffSFX;
    [SerializeField] public AudioClip shadowbtPwrupSFX;
    [SerializeField] public AudioClip noEnergySFX;
    [SerializeField] public AudioClip energyBallSFX;
    [SerializeField] public AudioClip energyRefillSFX;
    [SerializeField] public AudioClip coinSFX;
    [SerializeField] public AudioClip leechBiteSFX;
    [SerializeField] public AudioClip shadowdashSFX;
    [SerializeField] public AudioClip matrixGetSFX;
    */
    #endregion
    [HeaderAttribute("Others")]
    [SerializeField] GameObject gameOverCanvasPrefab;
    [SerializeField] GameObject shadowPrefab;
    [SerializeField] const float flareShootYY = 0.2f;
    [SerializeField] public MeshRenderer bgSprite;
    int moveDir = 1;
    const float DCLICK_TIME = 0.2f;
    float lastClickTime;
    int dashDirX;
    int dashDirY;
    [HideInInspector]public bool damaged = false;
    [HideInInspector]public bool healed = false;
    [HideInInspector]public bool shadowed = false;
    [HideInInspector]public bool dashing = false;
    [HideInInspector]public float shootTimer = 0f;
    [HideInInspector]public float instantiateTime = 0.025f;
    [HideInInspector]public float instantiateTimer = 0f;
    float lsaberEnTimer;
    [HideInInspector]public Vector2 mousePos;
    [HideInInspector]public float shipScale=1f;
    [HideInInspector]public float dist;
    [HideInInspector]public Vector2 velocity;
     float hPressedTime;
     float vPressedTime;
    public float mPressedTime;
    public float timeFlyingTotal;
    public float timeFlyingCore;
    public bool enRegenEnabled;
    public float timerEnRegen;
    [SerializeField] public float freqEnRegen=1f;
    [SerializeField] public float enRegenAmnt=2f;
    [SerializeField] public float energyForRegen=10f;
    public float stayingTimer;
    public float stayingTimerCore;
    public bool hpRegenEnabled;
    public float timerHpRegen;
    [SerializeField] public float freqHpRegen=1f;
    [SerializeField] public float hpRegenAmnt=0.1f;

    Rigidbody2D rb;
    PlayerSkills pskills;
    GameSession gameSession;
    SaveSerial saveSerial;
    public Joystick joystick;
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
    public float energyUsedCount;
    PauseMenu pauseMenu;
    GameObject refillUI;
    GameObject refilltxtS;
    GameObject refilltxtE;
    //AudioSource audioSource;
#endregion

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        pskills=GetComponent<PlayerSkills>();
        gameSession=FindObjectOfType<GameSession>();
        saveSerial = FindObjectOfType<SaveSerial>();
        joystick=FindObjectOfType<FloatingJoystick>();
        //settings = FindObjectOfType<Settings>();
        //followMouse = GetComponent<FollowMouse>();
        SetUpMoveBoundaries();
        moveSpeed = moveSpeedInit;
        moveSpeedCurrent = moveSpeed;
        dashTime = startDashTime;
        moveByMouse = saveSerial.moveByMouse;
        pauseMenu=FindObjectOfType<PauseMenu>().GetComponent<PauseMenu>();
        refillUI=GameObject.Find("RefillUI");
        refilltxtS=GameObject.Find("RefillText1");
        refilltxtE=GameObject.Find("RefillText2");

        SetPrefabs();
    }
    void SetPrefabs(){
        laserPrefab=GameAssets.instance.Get("Laser");
        mlaserPrefab=GameAssets.instance.Get("MLaser");
        hrocketPrefab=GameAssets.instance.Get("HRocket");
        phaserPrefab=GameAssets.instance.Get("Phaser");
        lsaberPrefab=GameAssets.instance.Get("LSaber");
        lclawsPrefab=GameAssets.instance.Get("LClaws");
        lclawsVFX=GameAssets.instance.Get("LClawsVFX");
        shadowBTPrefab=GameAssets.instance.Get("ShadowBt");
        qrocketPrefab=GameAssets.instance.Get("QRocket");
        procketPrefab=GameAssets.instance.Get("PRocket");
        cbulletPrefab=GameAssets.instance.Get("CBullet");
        plaserPrefab=GameAssets.instance.Get("PLaser");

        explosionVFX=GameAssets.instance.GetVFX("Explosion");
        flareHitVFX=GameAssets.instance.GetVFX("FlareHit");
        flareShootVFX=GameAssets.instance.GetVFX("FlareShoot");
        shadowShootVFX=GameAssets.instance.GetVFX("ShadowTrail");
        gcloverVFX=GameAssets.instance.GetVFX("GCloverVFX");
        gcloverOVFX=GameAssets.instance.GetVFX("GCloverOutVFX");
        crystalExplosionVFX=GameAssets.instance.GetVFX("CExplVFX");
    }

    void Update(){
        HandleInput(false);
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        health =Mathf.Clamp(health,0f,maxHP);
        //PlayerBFlame();
        //if(powerup=="lsaber"||powerup=="lclaws")StartCoroutine(DrawOtherWeapons());
        DrawOtherWeapons();
        if(GetComponent<PlayerSkills>().timerTeleport==-4)Shoot();
        States();
        Regen();
        Die();
        CountTimeMovementPressed();
        RefillEnergy();
        if(moveX!=false&&moveY!=false){
            if(moveByMouse!=true){ MovePlayer(); }//followMouse.enabled = false; }
            else{ MoveWithMouse(); }// followMouse.enabled = true; }
        }
        shootTimer -= Time.deltaTime;
        instantiateTimer-=Time.deltaTime;
        velocity=rb.velocity;
        if(moving==false){stayingTimer+=Time.deltaTime;stayingTimerCore+=Time.deltaTime;timerHpRegen+=Time.deltaTime;}
        if(moving==true){timeFlyingTotal+=Time.deltaTime;timeFlyingCore+=Time.deltaTime;stayingTimer=0;stayingTimerCore=0;}
        
        if(overheatCdTimer>0)overheatCdTimer-=Time.deltaTime;
        if(overheatCdTimer<=0&&overheatTimer>0)overheatTimer-=Time.deltaTime*2;
        if(overheatTimer>=overheatTimerMax&&overheated!=true){health-=overheatDmg;DMGPopUpHud(overheatDmg);damaged=true;AudioManager.instance.Play("Overheat");
        overheatTimer=-4;overheated=true;overheatedTimer=overheatedTime;}
        if(overheated==true&&overheatedTimer>0){overheatedTimer-=Time.deltaTime;
            GameObject flareL = Instantiate(flareShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
            GameObject flareR = Instantiate(flareShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
            Destroy(flareL.gameObject, 0.04f);
            Destroy(flareR.gameObject, 0.04f);
            }
        if(overheatedTimer<=0){overheated=false;}
        //Debug.Log(shootTimer);
        //Debug.LogWarning(shootCoroutine);
    }
    void FixedUpdate()
    {
        // If we're first at-bat, handle the input immediately and mark it already-handled.
        HandleInput(true);
        //MovePlayer();
        /*if (!Input.GetButton("Fire1")){
            if(shootCoroutine!=null){StopCoroutine(shootCoroutine);StopCoroutine(ShootContinuously());}
        }*/
        dist = Vector2.Distance(mousePos, transform.position);
    }
#region//Movement etc
    void HandleInput(bool isFixedUpdate)
    {
        bool hadAlreadyHandled = hasHandledInputThisFrame;
        hasHandledInputThisFrame = isFixedUpdate;
        if (hadAlreadyHandled)
            return;

        /* Perform any instantaneous actions, using Time.fixedDeltaTime where necessary */
    }
    void CountTimeMovementPressed(){
        if (Application.platform == RuntimePlatform.Android){
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
    
    private void MovePlayer()
    {
        var deltaX=0f;
        var deltaY=0f;
        if (Input.GetButtonDown("Horizontal")){
            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= DCLICK_TIME) { DClick(); deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedCurrent * moveDir; }
            else{ deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedCurrent * moveDir; }
            lastClickTime = Time.time;  }
        if(Input.GetButtonDown("Vertical")){
            float timeSinceLastClick = Time.time - lastClickTime;
            if(timeSinceLastClick<=DCLICK_TIME){ DClick(); deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedCurrent * moveDir; }
            else{ deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedCurrent * moveDir; }
            lastClickTime = Time.time; }

        if(Application.platform == RuntimePlatform.Android){
            deltaX = joystick.Horizontal * Time.deltaTime * moveSpeedCurrent * moveDir;
            deltaY = joystick.Vertical * Time.deltaTime * moveSpeedCurrent * moveDir;
        }else{
            deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedCurrent * moveDir;
            deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedCurrent * moveDir;
        }

        var newXpos = transform.position.x;
        var newYpos = transform.position.y;

        if (moveX==true) newXpos = Mathf.Clamp(transform.position.x,xMin,xMax) + deltaX;
        if (moveY == true) newYpos = Mathf.Clamp(transform.position.y,yMin,yMax) + deltaY;
        transform.position = new Vector2(newXpos,newYpos);
        //Debug.Log(timeSinceLastClick);
        //Debug.Log(dashTime);
    }

    private void MoveWithMouse(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x=Mathf.Clamp(mousePos.x, xMin, xMax);
        mousePos.y=Mathf.Clamp(mousePos.y, yMin, yMax);
        //dist in FixedUpdate()
        var distX=Mathf.Abs(mousePos.x-transform.position.x);
        var distY=Mathf.Abs(mousePos.y-transform.position.y);
        if((distX>0f||distY>0f)&&(distX<0.35f||distY<0.35f)){dist=0.35f;}
        //var actualdist = Vector2.Distance(mousePos, transform.position);
        if(dist>=0.3f&&Time.timeScale>0.01f){moving=true;}
        if(dist==0f){moving=false;}
        //var minMoveDist=0f;
        //if(dist<minMoveDist)dist=0;

        float step = moveSpeedCurrent * Time.deltaTime;
        //if(FindObjectOfType<ShootButton>()!=null && FindObjectOfType<ShootButton>().pressed==false)transform.position = Vector2.MoveTowards(transform.position, mousePos*moveDir, step);
        //else if(FindObjectsOfType<ShootButton>()==null){transform.position = Vector2.MoveTowardqs(transform.position, mousePos*moveDir, step);}
        //if(dist>minMoveDist)
        transform.position = Vector2.MoveTowards(transform.position, mousePos*moveDir, step);

        if(Input.GetButtonDown("Fire2")){
            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= DCLICK_TIME){DClick(); }
            else{ lastClickTime = Time.time; }
        }

        var newXpos = transform.position.x;
        var newYpos = transform.position.y;

        if (moveX == true) newXpos = Mathf.Clamp(transform.position.x, xMin, xMax);
        if (moveY == true) newYpos = Mathf.Clamp(transform.position.y, yMin, yMax);

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

        xMin = -3.87f + padding;
        xMax = 3.87f - padding;
        yMin = -6.95f + padding;
        yMax = 7f - padding;
    }

    private void Shoot(){
        if(Time.timeScale>0.0001f){
            if (Application.platform != RuntimePlatform.Android){
                if(Input.GetButtonDown("Fire1")){
                    if(shootCoroutine!=null){return;}
                    else if(shootCoroutine==null&&shootTimer<=0f){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
                }if(!Input.GetButton("Fire1")||shootTimer<-1f){
                    if(shootCoroutine!=null)StopCoroutine(shootCoroutine);
                    shootCoroutine=null;
                    //if(shootCoroutine!=null){
                        //StopCoroutine(shootCoroutine);StopCoroutine(ShootContinuously());StopCoroutine("ShootContinuously");//}
                    if(moving==true)timerEnRegen+=Time.deltaTime;
                }
                /*if (Input.GetButtonUp("Fire1")){
                    StopCoroutine(shootCoroutine);
                }*/
            }
        }
    }

    public void ShootButton(bool pressed){
        if(pressed){
            if(shootCoroutine!=null){return;}
            else if(shootCoroutine==null&&shootTimer<=0f){shootCoroutine=ShootContinuously();StartCoroutine(shootCoroutine);}
        }else if(pressed==false||shootTimer<-1f){
            if(shootCoroutine!=null)StopCoroutine(shootCoroutine);
            shootCoroutine=null;
            if(moving==true)timerEnRegen+=Time.deltaTime;
        }
    }
    public void DClick(){
        //Debug.Log("DClick");
        if(shadow==true && energy>0){
            if(moveByMouse!=true){
                if(Application.platform == RuntimePlatform.Android){
                    if(joystick.Vertical<-0.2f) { rb.velocity = Vector2.down * dashSpeed * moveDir; }
                    if(joystick.Vertical>0.2f){ rb.velocity = Vector2.up * dashSpeed * moveDir; }
                    if(joystick.Horizontal<-0.2f){ rb.velocity = Vector2.left * dashSpeed * moveDir; }
                    if(joystick.Horizontal>0.2f){ rb.velocity = Vector2.right * dashSpeed * moveDir; }
                }else{
                    if(Input.GetAxisRaw("Vertical")<0) { rb.velocity = Vector2.down * dashSpeed * moveDir; }
                    if(Input.GetAxisRaw("Vertical")>0){ rb.velocity = Vector2.up * dashSpeed * moveDir; }
                    if(Input.GetAxisRaw("Horizontal")<0){ rb.velocity = Vector2.left * dashSpeed * moveDir; }
                    if(Input.GetAxisRaw("Horizontal")>0){ rb.velocity = Vector2.right * dashSpeed * moveDir; }
                }
            }else{
                rb.velocity = mousePos * dashSpeed * moveDir;
                if(new Vector2(transform.position.x,transform.position.y)==mousePos){rb.velocity=Vector2.zero;}
            }
            energy -= shadowEn;
            EnergyPopUpHUD(shadowEn);
            dashing = true;
            shadowed = true;
            //AudioSource.PlayClipAtPoint(shadowdashSFX, new Vector2(transform.position.x, transform.position.y));
            AudioManager.instance.Play("Shadowdash");
            dashTime = startDashTime;
            //else{ rb.velocity = Vector2.zero; }
        }//else { dashTime = startDashTime; rb.velocity = Vector2.zero; }
        
    }

    private void Die(){
        if (health <= 0){
            GameObject explosion = GameAssets.instance.VFX("Explosion",transform.position);
            AudioManager.instance.Play("Death");
            Destroy(explosion, 0.5f);
            pskills.DeathSkills();
            Destroy(gameObject, 0.05f);//Kill player
            gameOverCanvasPrefab.gameObject.SetActive(true);
            var lsaberName = lsaberPrefab.name; var lsaberName1 = lsaberPrefab.name + "(Clone)";
            Destroy(GameObject.Find(lsaberName));
            Destroy(GameObject.Find(lsaberName1));
            var lclawsName = lclawsPrefab.name; var lclawsName1 = lclawsPrefab.name + "(Clone)";
            Destroy(GameObject.Find(lclawsName));
            Destroy(GameObject.Find(lclawsName1));
        }
    }
#endregion

#region//Powerups
bool stopped=false;
    public IEnumerator ShootContinuously(){
        while (true){
        if (Time.timeScale > 0.0001f){
        if (energy>0){
            if(overheated!=true){
                //SetPrefabs();
                if (powerup=="laser"){
                    GameObject laserL = GameAssets.instance.Make("Laser", new Vector2(transform.position.x - 0.35f, transform.position.y)) as GameObject;
                    GameObject laserR = Instantiate(laserPrefab, new Vector2(transform.position.x + 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flareL = Instantiate(flareShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(flareShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    laserL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
                    laserR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
                    AddSubEnergy(laserEn,false);
                    laserL.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                    laserR.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                        shootTimer = laserShootPeriod * 0.75f;
                        stopped=false;
                        yield return new WaitForSeconds(laserShootPeriod*1.7f);
                        stopped=true;
                        //shootCoroutine=null;
                }else if(powerup=="laser2"){
                    GameObject laserL = Instantiate(laserPrefab, new Vector2(transform.position.x - 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserL2 = Instantiate(laserPrefab, new Vector2(transform.position.x - 0.4f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR = Instantiate(laserPrefab, new Vector2(transform.position.x + 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR2 = Instantiate(laserPrefab, new Vector2(transform.position.x + 0.4f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flareL = Instantiate(flareShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(flareShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    laserL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
                    laserR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
                    laserL2.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.55f, laserSpeed);
                    laserR2.GetComponent<Rigidbody2D>().velocity = new Vector2(+0.55f, laserSpeed);
                    laserL2.GetComponent<IntervalSound>().interval = 0.03f;
                    laserR2.GetComponent<IntervalSound>().interval = 0.03f;
                    laserL2.transform.eulerAngles=new Vector3(0,0,10f);
                    laserR2.transform.eulerAngles=new Vector3(0,0,-10f);
                    AddSubEnergy(laser2En,false);
                    laserL.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                    laserR.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                    laserL2.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                    laserR2.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                        shootTimer = laserShootPeriod * 0.75f;
                        yield return new WaitForSeconds(laserShootPeriod);
                        //shootCoroutine=null;
                }
                else if (powerup == "laser3"){
                    GameObject laserL = Instantiate(laserPrefab, new Vector2(transform.position.x - 0.3f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserL2 = Instantiate(laserPrefab, new Vector2(transform.position.x - 0.36f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserL3 = Instantiate(laserPrefab, new Vector2(transform.position.x - 0.43f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR = Instantiate(laserPrefab, new Vector2(transform.position.x + 0.3f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR2 = Instantiate(laserPrefab, new Vector2(transform.position.x + 0.36f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR3 = Instantiate(laserPrefab, new Vector2(transform.position.x + 0.43f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flareL = Instantiate(flareShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(flareShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    laserL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
                    laserR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
                    laserL2.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.55f, laserSpeed);
                    laserL3.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.65f, laserSpeed);
                    laserR2.GetComponent<Rigidbody2D>().velocity = new Vector2(+0.55f, laserSpeed);
                    laserR3.GetComponent<Rigidbody2D>().velocity = new Vector2(+0.65f, laserSpeed);
                    laserL2.GetComponent<IntervalSound>().interval = 0.03f;
                    laserL3.GetComponent<IntervalSound>().interval = 0.06f;
                    laserR2.GetComponent<IntervalSound>().interval = 0.03f;
                    laserR3.GetComponent<IntervalSound>().interval = 0.06f;
                    laserL2.transform.eulerAngles = new Vector3(0, 0, 8f);
                    laserL3.transform.eulerAngles = new Vector3(0, 0, 13f);
                    laserR2.transform.eulerAngles = new Vector3(0, 0, -8f);
                    laserR3.transform.eulerAngles = new Vector3(0, 0, -13f);
                    AddSubEnergy(laser3En,false);
                    laserL.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                    laserR.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                    laserL2.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                    laserR2.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                    laserL3.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                    laserR3.GetComponent<Tag_PlayerWeaponBlockable>().energy=laserEn;
                        shootTimer = laserShootPeriod*0.75f;
                        yield return new WaitForSeconds(laserShootPeriod);
                        //shootCoroutine=null;
                }else if (powerup == "phaser"){
                    GameObject phaserL = Instantiate(phaserPrefab, new Vector2(transform.position.x - 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject phaserR = Instantiate(phaserPrefab, new Vector2(transform.position.x + 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flareL = Instantiate(flareShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(flareShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    phaserL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, phaserSpeed);
                    phaserR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, phaserSpeed);
                    AddSubEnergy(phaserEn,false);
                    phaserL.GetComponent<Tag_PlayerWeaponBlockable>().energy=phaserEn;
                    phaserR.GetComponent<Tag_PlayerWeaponBlockable>().energy=phaserEn;
                        shootTimer = phaserShootPeriod;
                        yield return new WaitForSeconds(phaserShootPeriod);
                        //shootCoroutine=null;
                }else if (powerup == "mlaser"){
                    var xxL = transform.position.x - 0.35f + UnityEngine.Random.Range(0.05f, 0.1f); var xxR = transform.position.x + 0.35f + UnityEngine.Random.Range(0.05f, 0.1f);
                    var yyL = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f); var yyR = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f);
                    for (var i=0; i<mlaserBulletsAmmount; i++){
                        xxL = transform.position.x - 0.35f + UnityEngine.Random.Range(0.05f, 0.1f); xxR = transform.position.x + 0.35f + UnityEngine.Random.Range(0.05f, 0.1f);
                        yyL = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f); yyR = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f);
                        GameObject mlaserL = Instantiate(mlaserPrefab, new Vector2(xxL, yyL), Quaternion.identity) as GameObject;
                        GameObject mlaserR = Instantiate(mlaserPrefab, new Vector2(xxR, yyR), Quaternion.identity) as GameObject;
                        Rigidbody2D rbL = mlaserL.GetComponent<Rigidbody2D>(); rbL.velocity = new Vector2(rbL.velocity.x, UnityEngine.Random.Range(mlaserSpeedS, mlaserSpeedE));
                        Rigidbody2D rbR = mlaserR.GetComponent<Rigidbody2D>(); rbR.velocity = new Vector2(rbR.velocity.x, UnityEngine.Random.Range(mlaserSpeedS, mlaserSpeedE));
                        //AddSubEnergy(mlaserEn,false);
                        mlaserL.GetComponent<Tag_PlayerWeaponBlockable>().energy=mlaserEn;
                        mlaserR.GetComponent<Tag_PlayerWeaponBlockable>().energy=mlaserEn;
                    }
                    AddSubEnergy(mlaserEn*mlaserBulletsAmmount,false);
                    //EnergyPopUpHUD(mlaserEn*mlaserBulletsAmmount);
                    GameObject flareL = Instantiate(flareShootVFX, new Vector2(xxL, yyL - flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(flareShootVFX, new Vector2(xxR, yyR - flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    Recoil(7f,0.15f);
                        shootTimer = mlaserShootPeriod;
                        yield return new WaitForSeconds(mlaserShootPeriod);
                        //shootCoroutine=null;
                //Rockets
                }else if (powerup == "hrocket"){
                    var xx = transform.position.x - 0.35f;
                    if (UnityEngine.Random.Range(0f,1f)>0.5f){ xx = transform.position.x + 0.35f; }
                    GameObject hrocket = Instantiate(hrocketPrefab, new Vector2(xx, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flare = Instantiate(flareShootVFX, new Vector2(xx, transform.position.y+flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flare.gameObject, 0.3f);
                    hrocket.GetComponent<Rigidbody2D>().velocity = new Vector2(0, hrocketSpeed);
                    //AddSubEnergy(hrocketEn,false);
                    Overheat(hrocketEn,true);
                        shootTimer = hrocketShootPeriod;
                        yield return new WaitForSeconds(hrocketShootPeriod);
                        //shootCoroutine=null;
                }else if (powerup == "qrocket"){
                    GameObject hrocketL = Instantiate(qrocketPrefab, new Vector2(transform.position.x - 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject hrocketR = Instantiate(qrocketPrefab, new Vector2(transform.position.x + 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flareL = Instantiate(flareShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(flareShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    hrocketL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, qrocketSpeed);
                    hrocketR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, qrocketSpeed);
                    //AddSubEnergy(qrocketEn,false);
                    Overheat(qrocketEn,true);
                        shootTimer = qrocketShootPeriod;
                        yield return new WaitForSeconds(qrocketShootPeriod);
                        //shootCoroutine=null;
                }else if (powerup == "procket"){
                    var xxL = transform.position.x - 0.35f + UnityEngine.Random.Range(0.05f, 0.1f); var xxR = transform.position.x + 0.35f + UnityEngine.Random.Range(0.05f, 0.1f);
                    var yyL = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f); var yyR = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f);
                    for (var i=0; i<procketsBulletsAmmount; i++){
                        xxL = transform.position.x - 0.35f + UnityEngine.Random.Range(0.05f, 0.1f); xxR = transform.position.x + 0.35f + UnityEngine.Random.Range(0.05f, 0.1f);
                        yyL = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f); yyR = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f);
                        GameObject procketL = Instantiate(procketPrefab, new Vector2(xxL, yyL), Quaternion.identity) as GameObject;
                        GameObject procketR = Instantiate(procketPrefab, new Vector2(xxR, yyR), Quaternion.identity) as GameObject;
                        Rigidbody2D rbL = procketL.GetComponent<Rigidbody2D>(); rbL.velocity = new Vector2(rbL.velocity.x, UnityEngine.Random.Range(procketSpeedS, procketSpeedE));
                        Rigidbody2D rbR = procketR.GetComponent<Rigidbody2D>(); rbR.velocity = new Vector2(rbR.velocity.x, UnityEngine.Random.Range(procketSpeedS, procketSpeedE));
                        //AddSubEnergy(procketEn,false);
                    }
                        AddSubEnergy(procketEn*procketsBulletsAmmount,false);
                        Overheat(procketOh*procketsBulletsAmmount,true);
                        //EnergyPopUpHUD(procketEn*procketsBulletsAmmount);
                        shootTimer = procketShootPeriod;
                        yield return new WaitForSeconds(procketShootPeriod);
                        //shootCoroutine=null;
                }else if(powerup=="lclawsA"){
                    var enemy = FindClosestEnemy();
                    if(enemy!=null){
                        //AudioSource.PlayClipAtPoint(enemy.lsaberHitSFX, transform.position);
                        GameObject clawsPart = Instantiate(lclawsVFX, enemy.transform.position, Quaternion.identity) as GameObject;
                        Destroy(clawsPart, 0.15f);
                        //enemy.health -= FindObjectOfType<DamageDealer>().GetDamageLCLaws();
                    }else{ AudioManager.instance.Play("NoEnergy"); }
                    AddSubEnergy(lclawsEn,false);
                        shootTimer = 0.5f;
                        yield return new WaitForSeconds(0.5f);
                        //shootCoroutine=null;
                }else if (powerup=="cstream"){
                    var xx = transform.position.x - 0.35f;
                    if (UnityEngine.Random.Range(0f,1f)>0.5f){ xx = transform.position.x + 0.35f; }
                    GameObject cbullet = Instantiate(cbulletPrefab, new Vector2(xx, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flare = Instantiate(flareShootVFX, new Vector2(xx, transform.position.y+flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flare.gameObject, 0.3f);
                    cbullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
                    AddSubEnergy(cbulletEn,false);
                    cbullet.GetComponent<Tag_PlayerWeaponBlockable>().energy=cbulletEn;
                        shootTimer = cbulletShootPeriod * 0.825f;
                        yield return new WaitForSeconds(cbulletShootPeriod);
                        //shootCoroutine=null;
                }else if (powerup == "shadowbt"){
                    GameObject laserL = Instantiate(shadowBTPrefab, new Vector2(transform.position.x - 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR = Instantiate(shadowBTPrefab, new Vector2(transform.position.x + 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flareL = Instantiate(shadowShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(shadowShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    //laserL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, shadowBTSpeed);
                    //laserR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, shadowBTSpeed);
                    AddSubEnergy(shadowBTEn,false);
                        shootTimer = shadowBTShootPeriod;
                        yield return new WaitForSeconds(shadowBTShootPeriod);
                        //shootCoroutine=null;
                }else if (powerup == "plaser"){
                    float xx=0.2f;
                    GameObject laserL = Instantiate(plaserPrefab, new Vector2(transform.position.x - 0.3f+xx, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserL2 = Instantiate(plaserPrefab, new Vector2(transform.position.x - 0.36f+xx, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserL3 = Instantiate(plaserPrefab, new Vector2(transform.position.x - 0.43f+xx, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR = Instantiate(plaserPrefab, new Vector2(transform.position.x + 0.3f-xx, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR2 = Instantiate(plaserPrefab, new Vector2(transform.position.x + 0.36f-xx, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR3 = Instantiate(plaserPrefab, new Vector2(transform.position.x + 0.43f-xx, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flareL = Instantiate(flareShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(flareShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    laserL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, plaserSpeed);
                    laserR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, plaserSpeed);
                    laserL2.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.45f, plaserSpeed);
                    laserL3.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.55f, plaserSpeed);
                    laserR2.GetComponent<Rigidbody2D>().velocity = new Vector2(+0.45f, plaserSpeed);
                    laserR3.GetComponent<Rigidbody2D>().velocity = new Vector2(+0.55f, plaserSpeed);
                    laserL2.GetComponent<IntervalSound>().interval = 0.03f;
                    laserL3.GetComponent<IntervalSound>().interval = 0.06f;
                    laserR2.GetComponent<IntervalSound>().interval = 0.03f;
                    laserR3.GetComponent<IntervalSound>().interval = 0.06f;
                    laserL2.transform.eulerAngles = new Vector3(0, 0, 8f);
                    laserL3.transform.eulerAngles = new Vector3(0, 0, 13f);
                    laserR2.transform.eulerAngles = new Vector3(0, 0, -8f);
                    laserR3.transform.eulerAngles = new Vector3(0, 0, -13f);
                    AddSubEnergy(plaserEn,false);
                    Recoil(10f,0.2f);
                        shootTimer = plaserShootPeriod*0.75f;
                        yield return new WaitForSeconds(plaserShootPeriod);
                        //shootCoroutine=null;
                }else {if(powerup!="lsaberA" && powerup!="lclawsA" &&powerup!="cstream"&&powerup!="shadowbt")/*if(losePwrupOutOfEn)*/powerup = powerupDefault; shootTimer = 1f; yield return new WaitForSeconds(1f); }
            }/*if(overheatedTimer==-4){
                //yield break;}
                //if(powerup!="lclawsA"&&powerup!="cstream"&&powerup!="shadowbt"){}
                
                else if(powerup!="lsaberA" && powerup!="lclawsA" &&powerup!="cstream"&&powerup!="shadowbt"){yield break;}
            }*/
                //else if (powerup != "lsaber" && powerup != "lsaberA"){ yield return new WaitForSeconds(lsaberEnPeriod); }
                //else {if(powerup!="lsaberA" && powerup!="lclawsA")/*if(losePwrupOutOfEn)*/powerup = powerupDefault; shootTimer = 1f; yield return new WaitForSeconds(1f); }
        }else{ energy = 0; AudioManager.instance.Play("NoEnergy"); shootTimer = 0f; yield return new WaitForSeconds(1f); }
        }
        }
    }
    /*void SetShootTimer(){
        if(powerup=="laser"||powerup=="laser2"||powerup=="laser3")shootTimer=laserShootPeriod * 0.75f;
        if(powerup=="mlaser")shootTimer = mlaserShootPeriod;
        if(powerup=="phaser")shootTimer = phaserShootPeriod;
        if(powerup=="hrocket")shootTimer = hrocketShootPeriod;
        if(powerup=="procket")shootTimer = procketShootPeriod;
        if(powerup=="qrocket")shootTimer = qrocketShootPeriod;
        if(powerup=="cstream")shootTimer = cbulletShootPeriod*0.825f;
        if(powerup=="shadowbt")shootTimer = shadowBTShootPeriod;
        if(powerup=="lclawsA")shootTimer = 0.5f;
    }*/
    private void DrawOtherWeapons(){
        GameObject lsaber;
        GameObject lclaws;
        var lsaberName = lsaberPrefab.name; var lsaberName1 = lsaberPrefab.name + "(Clone)";
        var lclawsName = lclawsPrefab.name; var lclawsName1 = lclawsPrefab.name + "(Clone)";
        if (energy > 0){
            //yield return new WaitForSeconds(lsaberEnPeriod);
            if (powerup == "lsaber"){
                //yield return new WaitForSeconds(lsaberEnPeriod);
                //Destroy(GameObject.Find(lclawsName1));
                lsaber = Instantiate(lsaberPrefab, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity) as GameObject;
                moveSpeedCurrent = moveSpeed*lsaberSpeedMulti;
                lsaberEnTimer=lsaberEnPeriod;
                powerup = "lsaberA";
            }else if (powerup == "lsaberA"){
                if(Time.timeScale>0.0001f){
                    if(lsaberEnTimer>0){lsaberEnTimer-=Time.deltaTime;}
                    if(lsaberEnTimer<=0){lsaberEnTimer=lsaberEnPeriod;AddSubEnergy(lsaberEn,false);}
                    if(GameObject.Find(lsaberName)==null&&GameObject.Find(lsaberName1)==null){powerup="lsaber";}
                }
            }
            if (powerup == "lclaws"){
                //Destroy(GameObject.Find(lsaberName1));
                lclaws = Instantiate(lclawsPrefab, new Vector2(transform.position.x, transform.position.y + 0.8f), Quaternion.identity) as GameObject;
                lclaws.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -10);
                moveSpeedCurrent = moveSpeed*lsaberSpeedMulti;
                powerup = "lclawsA";
            }else if (powerup == "lclawsA"){
                if(Time.timeScale>0.0001f){
                    //if(lsaberEnTimer>0){lsaberEnTimer-=Time.deltaTime;}
                    //if(lsaberEnTimer<=0){energy -= lsaberEn*4;lsaberEnTimer=lsaberEnPeriod*9;}//0.2
                    if(GameObject.Find(lclawsName)==null&&GameObject.Find(lclawsName1)==null){powerup="lclaws";}
                }
            }
            if(powerup!="lsaberA" && powerup!="lsaber"){
                Destroy(GameObject.Find(lsaberName1));
            }if(powerup!="lclawsA" && powerup!="lclaws"){
                Destroy(GameObject.Find(lclawsName1));
            }
            if(powerup!="lsaberA"&&powerup!="lclawsA"){
                Destroy(GameObject.Find(lsaberName1));
                Destroy(GameObject.Find(lclawsName1));
                moveSpeedCurrent = moveSpeed;
            }
        }else { energy = 0; //yield return new WaitForSeconds(0.2f);
            Destroy(GameObject.Find(lsaberName1));
            Destroy(GameObject.Find(lclawsName1));
            moveSpeedCurrent = moveSpeed;
            if(powerup=="lsaberA")powerup="lsaber";
            if(powerup=="lclawsA")powerup="lclaws";
            if(losePwrupOutOfEn)powerup = powerupDefault;
        }
    }
#endregion
 
#region//States
    private void States(){
        if (flip == true) { flipTimer -= Time.deltaTime; moveDir = -1; } else { moveDir = 1; }
        if(flipTimer<= 0 && flipTimer>-4) { ResetStatus("flip"); AudioManager.instance.Play("PowerupOff"); }

        if (gclover == true) {
            health = maxHP;
            FindObjectOfType<HPBar>().GetComponent<HPBar>().gclover = true;
            gcloverTimer -= Time.deltaTime;
        }
        else{
            FindObjectOfType<HPBar>().GetComponent<HPBar>().gclover = false;
        }
        if (gcloverTimer <= 0 && gcloverTimer>-4) { ResetStatus("gclover"); AudioManager.instance.Play("GCloverOff"); }

        if (shadow == true) { shadowTimer -= Time.deltaTime; }
        if (shadowTimer <= 0 && shadowTimer > -4) { ResetStatus("shadow"); AudioManager.instance.Play("PowerupOff"); }
        if (shadow==true){ Shadow(); GetComponent<BackflameEffect>().enabled = false; }
        else{ dashTime = startDashTime; GetComponent<BackflameEffect>().enabled=true; }
        if (dashTime <= 0&&dashTime!=-4) { rb.velocity = Vector2.zero; dashing = false; dashTime=-4;}
        else{ dashTime -= Time.deltaTime; }
        if(energy<=0){ shadow = false; }

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
                PowerupWeights[] objs = FindObjectsOfType<PowerupWeights>();
                foreach(PowerupWeights obj in objs){
                    var follow = obj.GetComponent<Follow>();
                    if(follow!=null)Destroy(follow);
                }
            }
        }
        if(magnetTimer <=0 && magnetTimer>-4){ResetStatus("magnet");}
        
        if(scaler==true){
            var i=0;
            scalerTimer-=Time.deltaTime;
            if(Time.timeScale>0.0001f){// && instantiateTimer<=0){
                for(i=0; i<100; i++){
                    if(UnityEngine.Random.Range(0,100)>50){scaleUp=true;}else{scaleUp=false;}
                    if(i>99)i=0;
                }

                if(scaleUp==false && shipScale>shipScaleMin){shipScale-=2f*Time.deltaTime;}
                if(scaleUp==true && shipScale<shipScaleMax){shipScale+=2f*Time.deltaTime;}
                //if(shipScale<=0.45){scaleUp=true;}
                //if(shipScale>=1.64){scaleUp=false;}
                //instantiateTimer=instantiateTime;
            }
        }else{
            shipScale=1;
        }
        if(scalerTimer <=0 && scalerTimer>-4){ResetStatus("scaler");}
        transform.localScale=new Vector3(shipScale,shipScale,1);
        
        if(matrix==true&&accel==false){
            if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true){
                matrixTimer-=Time.unscaledDeltaTime;//matrixTimer-=Time.deltaTime;
                //if((rb.velocity.x<0.7 && rb.velocity.x>-0.7) || (rb.velocity.y<0.7 && rb.velocity.y>-0.7)){
                //||(moveByMouse==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if(moveByMouse==true && dist<1){
                    gameSession.gameSpeed=dist;
                }else if(moveByMouse==false && (Application.platform != RuntimePlatform.Android) && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //gameSession.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    gameSession.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,1f);
                }else if(moveByMouse==false && (Application.platform == RuntimePlatform.Android) && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    gameSession.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,1f);
                }else{
                    if(gameSession.speedChanged!=true)gameSession.gameSpeed=1f;
                }
            }else{
                gameSession.gameSpeed=0f;
            }
        }
        if(matrixTimer <=0 && matrixTimer>-4){gameSession.gameSpeed=1f; ResetStatus("matrix");}

        if(pmultiTimer>0){pmultiTimer-=Time.deltaTime;}
        if(pmultiTimer <=0 && pmultiTimer>-4){gameSession.scoreMulti=1f; ResetStatus("pmulti");}

        if(accel==true&&matrix==false){
            if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true){
                accelTimer-=Time.unscaledDeltaTime;//accelTimer-=Time.deltaTime;
                //if((rb.velocity.x<0.7 && rb.velocity.x>-0.7) || (rb.velocity.y<0.7 && rb.velocity.y>-0.7)){
                //||(moveByMouse==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if(moveByMouse==true && dist>0.35){
                    gameSession.gameSpeed=dist+(1-0.35f);
                }else if(moveByMouse==false && (Application.platform != RuntimePlatform.Android) && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //gameSession.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    gameSession.gameSpeed=Mathf.Clamp(mPressedTime,1f,2f);
                }else if(moveByMouse==false && (Application.platform == RuntimePlatform.Android) && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    gameSession.gameSpeed=Mathf.Clamp(mPressedTime,1f,2f);
                }else{
                    if(gameSession.speedChanged!=true)gameSession.gameSpeed=1f;
                }
            }else{
                gameSession.gameSpeed=0f;
            }
        }
        if(accelTimer <=0 && accelTimer>-4){gameSession.gameSpeed=1f; ResetStatus("accel");}

        if(accel==true&&matrix==true){
            if(PauseMenu.GameIsPaused!=true && Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true){
                accelTimer-=Time.unscaledDeltaTime;//accelTimer-=Time.deltaTime;
                matrixTimer-=Time.unscaledDeltaTime;//matrixTimer-=Time.deltaTime;
                //if((rb.velocity.x<0.7 && rb.velocity.x>-0.7) || (rb.velocity.y<0.7 && rb.velocity.y>-0.7)){
                //||(moveByMouse==false && (((Input.GetAxis("Horizontal")<0.6)||Input.GetAxis("Horizontal")>-0.6))||((Input.GetAxis("Vertical")<0.6)||Input.GetAxis("Vertical")>-0.6))
                if(moveByMouse==true){
                    gameSession.gameSpeed=dist;
                }else if(moveByMouse==false && (Application.platform != RuntimePlatform.Android) && (((Input.GetAxis("Horizontal")<0.5)||Input.GetAxis("Horizontal")>-0.5)||((Input.GetAxis("Vertical")<0.5)||Input.GetAxis("Vertical")>-0.5))){
                    
                    //gameSession.gameSpeed=(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical"))/2);
                    gameSession.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,2f);
                }else if(moveByMouse==false && (Application.platform == RuntimePlatform.Android) && (((joystick.Horizontal<0.4f)||joystick.Horizontal>-0.4f)||((joystick.Vertical<0.4f)||joystick.Vertical>-0.4f))){
                    gameSession.gameSpeed=Mathf.Clamp(mPressedTime,0.05f,2f);
                }else{
                    if(gameSession.speedChanged!=true)gameSession.gameSpeed=1f;
                }
            }else{
                gameSession.gameSpeed=0f;
            }
        }
    }
    
    private void Shadow(){
        if (Time.timeScale > 0.0001f && instantiateTimer<=0)
        {
            GameObject shadow = Instantiate(shadowPrefab,new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
            shadow.transform.localScale=new Vector3(shipScale,shipScale,1);
            Destroy(shadow.gameObject, shadowLength);
            instantiateTimer=instantiateTime;
            //yield return new WaitForSeconds(0.2f);
        }
    }
    void Regen(){
        if(timerHpRegen>=freqHpRegen && hpRegenEnabled==true){health+=hpRegenAmnt;timerHpRegen=0;HPPopUpHUD(hpRegenAmnt);}
        if(timerEnRegen>=freqEnRegen && enRegenEnabled==true && energy>energyForRegen){energy+=enRegenAmnt;timerEnRegen=0;EnergyPopUpHUDPlus(enRegenAmnt);}
    }
    void Recoil(float strength, float time){
        //rb.velocity = Vector2.down*strength;
        //Debug.Log(rb.velocity);
        StartCoroutine(RecoilI(strength,time));
    }
    IEnumerator RecoilI(float strength,float time){
        rb.velocity = Vector2.down*strength;
        yield return new WaitForSeconds(time);
        rb.velocity=Vector2.zero;
    }
#endregion

#region//Skills
    //Skills are in PlayerSkills
    private void RefillEnergy(){
        if(energy<=0){
        if(energyRefillUnlocked==true){
            if(refillDelay>0)refillDelay-=Time.deltaTime;
            if(refillDelay<=0){refillDelay=-4;}
            //refillUI.gameObject.SetActive(true);

            if(refillRandomized==false){
                SetActiveAllChildren(refillUI.transform,true);
                //refilltxtS=GameObject.Find("RefillText1");
                //refilltxtE=GameObject.Find("RefillText2");
                if(refillCount==0){
                    GameObject.Find("RandomArrows").SetActive(false);
                    //GameObject.Find("HUD 9:16/Game Canvas/RefillUI/RandomArrows").SetActive(false);
                    refilltxtE.SetActive(false);
                    refillCostS=1;
                    refillCostE=1;
                }else if(refillCount>0 && refillCount<=2){
                    refillCostS=1;
                    refillCostE=2;
                }else if(refillCount>=3 && refillCount<=5){
                    var choose=UnityEngine.Random.Range(1,3);
                    if(choose==1){
                        refillCostS=1;
                        refillCostE=3;
                    }else if(choose==2){
                        refillCostS=2;
                        refillCostE=4;
                    }if(choose==3){
                        GameObject.Find("RandomArrows").SetActive(false);
                        refilltxtE.SetActive(false);
                        refillCostS=3;
                        refillCostE=3;
                    }
                }else if(refillCount>5){
                    var choose=UnityEngine.Random.Range(1,3);
                    if(choose==1){
                        refillCostS=3;
                        refillCostE=4;
                    }else if(choose==2){
                        refillCostS=3;
                        refillCostE=5;
                    }if(choose==3){
                        GameObject.Find("RandomArrows").SetActive(false);
                        refilltxtE.SetActive(false);
                        refillCostS=4;
                        refillCostE=4;
                    }
                }
                refillRandomized=true;
            }

            refilltxtS.GetComponent<TMPro.TextMeshProUGUI>().text=refillCostS.ToString();
            refilltxtE.GetComponent<TMPro.TextMeshProUGUI>().text=refillCostE.ToString();
            if(Input.GetButtonDown("Fire1")){
                if(refillDelay==-4){
                    var refillCost=UnityEngine.Random.Range(refillCostS,refillCostE);
                    if(gameSession.coins>refillCost){
                        energy+=refillEnergyAmnt;
                        EnergyPopUpHUDPlus(refillEnergyAmnt);
                        refillCount++;
                        gameSession.coins-=refillCost;
                        refillRandomized=false;
                        AudioManager.instance.Play("EnergyRefill");
                        GameObject crystalVFX = Instantiate(crystalExplosionVFX, new Vector2(0, 0), Quaternion.identity);
                        Destroy(crystalVFX,0.1f);
                    }else{
                        refilltxtS.GetComponent<TMPro.TextMeshProUGUI>().text=refillCost.ToString();
                        refilltxtE.GetComponent<TMPro.TextMeshProUGUI>().text="";
                    }
                    refillDelay=1.6f;
                }
            }
        }
        }else{
            if(refillUI.gameObject.activeSelf==true)SetActiveAllChildren(refillUI.transform,false);//refillUI.gameObject.SetActive(false);
        }
        
    }
#endregion

#region//Pop-Ups
    public void DMGPopUpHud(float dmg){
        GameObject dmgpopupHud=GameObject.Find("HPDiffParrent");
        dmgpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //dmgpupupHud.GetComponent<Animator>().SetTrigger(0);
        dmgpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="-"+dmg.ToString();
    }public void HPPopUpHUD(float dmg){
        GameObject dmgpopupHud=GameObject.Find("HPDiffParrent");
        dmgpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //dmgpupupHud.GetComponent<Animator>().SetTrigger(0);
        dmgpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="+"+dmg.ToString();
    }
    public void EnergyPopUpHUD(float en){
        GameObject enpopupHud=GameObject.Find("EnergyDiffParrent");
        enpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        enpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="-"+en.ToString();
        energyUsedCount+=en;
    }public void EnergyPopUpHUDPlus(float en){
        GameObject enpopupHud=GameObject.Find("EnergyDiffParrent");
        enpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        enpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="+"+en.ToString();
    }
#endregion

#region//Other Functions
    public string status1="";
    public string status2="";
    public string status3="";
    public string statusc="";
    public void SetStatus(string status){
        statusc=status;
        //var v = this.GetType().GetProperty(status);
        if(this.GetType().GetField(status)!=null)this.GetType().GetField(status).SetValue(this,true);
        var i=this.GetType().GetField(status+"Time").GetValue(this);
        this.GetType().GetField((status+"Timer")).SetValue(this,i);
        //v.SetValue(this,true,null);
        //((dynamic)this).status=true;
	    //this.GetType().GetProperty(status+"Timer").SetValue(this,status+"Time");
        SortStatuses();
    }public void ResetStatus(string status){
        if(this.GetType().GetField(status)!=null)this.GetType().GetField(status).SetValue(this,false);
        this.GetType().GetField((status+"Timer")).SetValue(this,-4);
        if(status1==status){status1="";}
        if(status2==status){status2="";}
        if(status3==status){status3="";}
        ResortStatuses();
    }void SortStatuses(){
        if(status1!=statusc&&status2!=statusc&&status3!=statusc){
            if(status1==""){status1=statusc;statusc="";}
            if(status2=="" && status1!=""){status2=statusc;statusc="";}
            if(status3=="" && status2!="" && status1!=""){status3=statusc;statusc="";}
        }
    }void ResortStatuses(){
        if(status1=="" && status2!=""){status1=status2;status2="";}
        if(status2=="" && status3!=""){status2=status3;status3="";}
        //if(status3=="" && status2!="" && status1!=""){status3=statusc;statusc="";}
    }


    public void AddSubEnergy(float value,bool add){
        if(inverter!=true){
            if(add){energy+=value;EnergyPopUpHUDPlus(value);FindObjectOfType<DisruptersSpawner>().EnergyCountVortexWheel+=value;}
            else{energy-=value;EnergyPopUpHUD(value);FindObjectOfType<DisruptersSpawner>().EnergyCountVortexWheel-=value;}
        }else{
            if(add){energy-=value;EnergyPopUpHUD(value);FindObjectOfType<DisruptersSpawner>().EnergyCountVortexWheel-=value;}
            else{energy+=value;EnergyPopUpHUDPlus(value);FindObjectOfType<DisruptersSpawner>().EnergyCountVortexWheel+=value;}
        }
    }public void Overheat(float value,bool add){
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


    public Enemy FindClosestEnemy(){
        KdTree<Enemy> Enemies = new KdTree<Enemy>();
        Enemy[] EnemiesArr;
        EnemiesArr = FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in EnemiesArr){
            Enemies.Add(enemy);
        }
        Enemy closest = Enemies.FindClosest(transform.position);
        return closest;
    }
    /*public Enemy FindClosestEnemy()
    {
        Enemy[] gos;
        gos = Enemy.FindObjectsOfType<Enemy>();
        Enemy closest;
        float distance = 44f;
        Vector3 position = transform.position;
        foreach (Enemy go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
                return closest;
            }else { return null; }
        }
        return null;
    }*/
    private void SetActiveAllChildren(Transform transform, bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);

            SetActiveAllChildren(child, value);
        }
    }
    /*private void OnTriggerStay2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        bool stay = false;
        var dmg = damageDealer.GetDamage();
        var cometName = cometPrefab.name; var cometName1 = cometPrefab.name + "(Clone)";
        if (other.gameObject.name == cometName || other.gameObject.name == cometName1) { stay = false; }
        if (stay!=true){if(other.GetComponent<Enemy>().health>0){ other.GetComponent<Enemy>().health = -1; other.GetComponent<Enemy>().Die();} }
    }*/
    public float GetAmmo(){return energy;}
    public float GetFlipTimer(){ return flipTimer; }
    public float GetGCloverTimer(){ return gcloverTimer; }
    public float GetShadowTimer(){ return shadowTimer; }
    public float GetInverterTimer(){ return inverterTimer; }
    public float GetMagnetTimer(){ return magnetTimer; }
    public float GetScalerTimer(){ return scalerTimer; }
    public float GetMatrixTimer(){ return matrixTimer; }
    public float GetPMultiTimer(){ return pmultiTimer; }
#endregion
}
