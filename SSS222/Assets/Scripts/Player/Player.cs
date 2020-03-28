using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    //Config params
    [HeaderAttribute("Player")]
    [SerializeField] bool moveX = true;
    [SerializeField] bool moveY = true;
    [SerializeField] bool moveByMouse = false;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float lsaberSpeedMulti = 1.25f;
    public float moveSpeedCurrent;
    [SerializeField] float padding = 0.1f;
    [SerializeField] public float health = 200f;
    [SerializeField] public float maxHP = 200f;
    [SerializeField] public string powerup = "laser";
    [SerializeField] public float energy = 80f;
    [SerializeField] public float maxEnergy = 200f;
    [HeaderAttribute("Weapons")]
    [SerializeField] public GameObject laserPrefab;
    [SerializeField] public GameObject phaserPrefab;
    [SerializeField] public GameObject hrocketPrefab;
    [SerializeField] public GameObject mlaserPrefab;
    [SerializeField] public GameObject lsaberPrefab;
    [SerializeField] public GameObject shadowBTPrefab;
    [SerializeField] float laserSpeed = 9f;
    [SerializeField] float laserShootPeriod = 0.34f;
    [SerializeField] float phaserSpeed = 10.5f;
    [SerializeField] float phaserShootPeriod = 0.2f;
    [SerializeField] float hrocketSpeed = 6.5f;
    [SerializeField] float hrocketShootPeriod = 0.3f;
    [SerializeField] float mlaserSpeedS = 8.5f;
    [SerializeField] float mlaserSpeedE = 10f;
    [SerializeField] float mlaserShootPeriod = 0.1f;
    [SerializeField] int mlaserBulletsAmount = 10;
    [SerializeField] float lsaberEnPeriod = 0.1f;
    [SerializeField] float shadowBTSpeed = 4f;
    [SerializeField] float shadowBTShootPeriod = 0.65f;
    [HeaderAttribute("States")]
    [SerializeField] public bool flip = false;
    [SerializeField] public float flipTime = 7f;
    public float flipTimer=-4;
    [SerializeField] public bool gclover = false;
    [SerializeField] public float gcloverTime = 6f;
    public float gcloverTimer =-4;
    [SerializeField] public bool shadow = false;
    [SerializeField] public float shadowTime = 10f;
    public float shadowTimer = -4;
    [SerializeField] public float shadowLength=0.33f;
    [SerializeField] public float dashSpeed=10f;
    [SerializeField] public float startDashTime=0.1f;
    float dashTime;
    [HeaderAttribute("Energy Costs")]
    [SerializeField] public float laserEn = 2f;
    [SerializeField] public float laser2En = 4f;
    [SerializeField] public float laser3En = 6f;
    [SerializeField] public float phaserEn = 3f;
    [SerializeField] public float hrocketEn = 5f;
    [SerializeField] public float mlaserEn = 0.225f;
    [SerializeField] public float lsaberEn = 0.2f;
    [SerializeField] public float shadowEn = 3.5f;
    [SerializeField] public float shadowBTEn = 10f;
    [SerializeField] public float energyBallGet = 6f;
    [SerializeField] public float medkitEnergyGet = 2f;
    [SerializeField] public float medkitUEnergyGet = 8f;
    [SerializeField] public float pwrupEnergyGet = 48f;
    [HeaderAttribute("Effects")]
    [SerializeField] public GameObject explosionVFX;
    [SerializeField] public GameObject flareHitVFX;
    [SerializeField] public GameObject flareShootVFX;
    [SerializeField] public GameObject shadowShootVFX;
    [SerializeField] public GameObject gcloverVFX;
    [SerializeField] public GameObject gcloverOVFX;
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
    [SerializeField] public AudioClip coinSFX;
    [HeaderAttribute("Others")]
    [SerializeField] GameObject gameOverCanvasPrefab;
    [SerializeField] GameObject shadowPrefab;
    [SerializeField] float flareShootYY = 0.2f;
    [SerializeField] public MeshRenderer bgSprite;
    int moveDir = 1;
    const float DCLICK_TIME = 0.2f;
    float lastClickTime;
    int dashDirX;
    int dashDirY;
    public bool damaged = false;
    public bool healed = false;
    public bool shadowed = false;
    public bool dashing = false;

    public new Vector2 mousePos;
    public float dist;

    Rigidbody2D rb;
    GameSession gameSession;
    Coroutine shootCoroutine;
    //FollowMouse followMouse;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    bool hasHandledInputThisFrame = false;
    
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        gameSession=FindObjectOfType<GameSession>();
        //followMouse = GetComponent<FollowMouse>();
        SetUpMoveBoundaries();
        moveSpeedCurrent = moveSpeed;
        dashTime = startDashTime;
    }
    // Update is called once per frame
    void Update(){
        HandleInput(false);
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        health =Mathf.Clamp(health,0f,maxHP);
        //PlayerBFlame();
        StartCoroutine(DrawOtherWeapons());
        Shoot();
        States();
        Die();
        if(moveByMouse!=true){ MovePlayer(); }//followMouse.enabled = false; }
        else{ MoveWithMouse(); }// followMouse.enabled = true; }
    }
    void FixedUpdate()
    {
        // If we're first at-bat, handle the input immediately and mark it already-handled.
        HandleInput(true);
        //MovePlayer();
        if (!Input.GetButton("Fire1"))
        {
            StopCoroutine(shootCoroutine);
        }
    }
    void HandleInput(bool isFixedUpdate)
    {
        bool hadAlreadyHandled = hasHandledInputThisFrame;
        hasHandledInputThisFrame = isFixedUpdate;
        if (hadAlreadyHandled)
            return;

        /* Perform any instantaneous actions, using Time.fixedDeltaTime where necessary */
    }

    public float GetFlipTimer(){ return flipTimer; }
    public float GetGCloverTimer(){ return gcloverTimer; }
    public float GetShadowTimer(){ return shadowTimer; }

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

        deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedCurrent * moveDir;
        deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedCurrent * moveDir;

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
        dist = Vector2.Distance(mousePos, transform.position);

        float step = moveSpeedCurrent * Time.deltaTime;
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
        if (Input.GetButtonDown("Fire1")){
            shootCoroutine = StartCoroutine(ShootContinuously());
        }if(!Input.GetButton("Fire1")){
            StopCoroutine(shootCoroutine);
        }
        /*if (Input.GetButtonUp("Fire1")){
            StopCoroutine(shootCoroutine);
        }*/
    }
    public void DClick(){
        //Debug.Log("DClick");
        if(shadow==true && energy>0){
            if(moveByMouse!=true){
                if(Input.GetAxisRaw("Vertical")<0) { rb.velocity = Vector2.down * dashSpeed * moveDir; }
                if(Input.GetAxisRaw("Vertical")>0){ rb.velocity = Vector2.up * dashSpeed * moveDir; }
                if(Input.GetAxisRaw("Horizontal")<0){ rb.velocity = Vector2.left * dashSpeed * moveDir; }
                if(Input.GetAxisRaw("Horizontal")>0){ rb.velocity = Vector2.right * dashSpeed * moveDir; }
            }else{
                rb.velocity = mousePos * dashSpeed * moveDir;
            }
            energy -= shadowEn;
            dashing = true;
            shadowed = true;
            dashTime = startDashTime;
            //else{ rb.velocity = Vector2.zero; }
        }//else { dashTime = startDashTime; rb.velocity = Vector2.zero; }
        
    }
    #region//Powerups
    IEnumerator ShootContinuously(){
        while (true){
            if (energy > 0){
                if (powerup=="laser"){
                    GameObject laserL = Instantiate(laserPrefab, new Vector2(transform.position.x - 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR = Instantiate(laserPrefab, new Vector2(transform.position.x + 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flareL = Instantiate(flareShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(flareShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    laserL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
                    laserR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
                    energy -= laserEn;
                    yield return new WaitForSeconds(laserShootPeriod);
                    //AudioSource.PlayClipAtPoint(shootLaserSFX, new Vector2(transform.position.x, transform.position.y));
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
                    laserL2.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.5f, laserSpeed);
                    laserR2.GetComponent<Rigidbody2D>().velocity = new Vector2(+0.5f, laserSpeed);
                    laserL2.GetComponent<IntervalSound>().interval = 0.03f;
                    laserR2.GetComponent<IntervalSound>().interval = 0.03f;
                    laserL2.transform.eulerAngles=new Vector3(0,0,10f);
                    laserR2.transform.eulerAngles=new Vector3(0,0,-10f);
                    energy -= laser2En;
                    yield return new WaitForSeconds(laserShootPeriod);
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
                    laserL2.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.45f, laserSpeed);
                    laserL3.GetComponent<Rigidbody2D>().velocity = new Vector2(-0.55f, laserSpeed);
                    laserR2.GetComponent<Rigidbody2D>().velocity = new Vector2(+0.45f, laserSpeed);
                    laserR3.GetComponent<Rigidbody2D>().velocity = new Vector2(+0.55f, laserSpeed);
                    laserL2.GetComponent<IntervalSound>().interval = 0.03f;
                    laserL3.GetComponent<IntervalSound>().interval = 0.06f;
                    laserR2.GetComponent<IntervalSound>().interval = 0.03f;
                    laserR3.GetComponent<IntervalSound>().interval = 0.06f;
                    laserL2.transform.eulerAngles = new Vector3(0, 0, 8f);
                    laserL3.transform.eulerAngles = new Vector3(0, 0, 13f);
                    laserR2.transform.eulerAngles = new Vector3(0, 0, -8f);
                    laserR3.transform.eulerAngles = new Vector3(0, 0, -13f);
                    energy -= laser3En;
                    yield return new WaitForSeconds(laserShootPeriod);
                }else if (powerup == "phaser"){
                    GameObject phaserL = Instantiate(phaserPrefab, new Vector2(transform.position.x - 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject phaserR = Instantiate(phaserPrefab, new Vector2(transform.position.x + 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flareL = Instantiate(flareShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(flareShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    phaserL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, phaserSpeed);
                    phaserR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, phaserSpeed);
                    energy -= phaserEn;
                    yield return new WaitForSeconds(phaserShootPeriod);
                }else if (powerup == "mlaser"){
                    var xxL = transform.position.x - 0.35f + UnityEngine.Random.Range(0.05f, 0.1f); var xxR = transform.position.x + 0.35f + UnityEngine.Random.Range(0.05f, 0.1f);
                    var yyL = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f); var yyR = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f);
                    for (var i=0; i<mlaserBulletsAmount; i++){
                        xxL = transform.position.x - 0.35f + UnityEngine.Random.Range(0.05f, 0.1f); xxR = transform.position.x + 0.35f + UnityEngine.Random.Range(0.05f, 0.1f);
                        yyL = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f); yyR = transform.position.y + 0.1f + UnityEngine.Random.Range(0.25f, 0.45f);
                        GameObject mlaserL = Instantiate(mlaserPrefab, new Vector2(xxL, yyL), Quaternion.identity) as GameObject;
                        GameObject mlaserR = Instantiate(mlaserPrefab, new Vector2(xxR, yyR), Quaternion.identity) as GameObject;
                        Rigidbody2D rbL = mlaserL.GetComponent<Rigidbody2D>(); rbL.velocity = new Vector2(rbL.velocity.x, UnityEngine.Random.Range(mlaserSpeedS, mlaserSpeedE));
                        Rigidbody2D rbR = mlaserR.GetComponent<Rigidbody2D>(); rbR.velocity = new Vector2(rbR.velocity.x, UnityEngine.Random.Range(mlaserSpeedS, mlaserSpeedE));
                        energy -= mlaserEn;
                    }
                    GameObject flareL = Instantiate(flareShootVFX, new Vector2(xxL, yyL - flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(flareShootVFX, new Vector2(xxR, yyR - flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    yield return new WaitForSeconds(mlaserShootPeriod);
                }else if (powerup == "hrockets"){
                    var xx = transform.position.x - 0.35f;
                    if (UnityEngine.Random.Range(0f,1f)>0.5f){ xx = transform.position.x + 0.35f; }
                    GameObject hrocket = Instantiate(hrocketPrefab, new Vector2(xx, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flare = Instantiate(flareShootVFX, new Vector2(xx, transform.position.y+flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flare.gameObject, 0.3f);
                    hrocket.GetComponent<Rigidbody2D>().velocity = new Vector2(0, hrocketSpeed);
                    energy -= hrocketEn;
                    yield return new WaitForSeconds(hrocketShootPeriod);
                }else if (powerup == "shadowbt")
                {
                    GameObject laserL = Instantiate(shadowBTPrefab, new Vector2(transform.position.x - 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject laserR = Instantiate(shadowBTPrefab, new Vector2(transform.position.x + 0.35f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject flareL = Instantiate(shadowShootVFX, new Vector2(transform.position.x - 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    GameObject flareR = Instantiate(shadowShootVFX, new Vector2(transform.position.x + 0.35f, transform.position.y + flareShootYY), Quaternion.identity) as GameObject;
                    Destroy(flareL.gameObject, 0.3f);
                    Destroy(flareR.gameObject, 0.3f);
                    laserL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, shadowBTSpeed);
                    laserR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, shadowBTSpeed);
                    energy -= shadowBTEn;
                    yield return new WaitForSeconds(shadowBTShootPeriod);
                    //AudioSource.PlayClipAtPoint(shootLaserSFX, new Vector2(transform.position.x, transform.position.y));
                }
                //else if (powerup != "lsaber" && powerup != "lsaberA"){ yield return new WaitForSeconds(lsaberEnPeriod); }
                else {if(powerup!="lsaberA")powerup = "laser";yield return new WaitForSeconds(1f); }
            }else{ energy = 0; AudioSource.PlayClipAtPoint(noEnergySFX, new Vector2(transform.position.x, transform.position.y)); yield return new WaitForSeconds(1f); }
            }
    }

    IEnumerator DrawOtherWeapons(){
        GameObject lsaber;
        if (energy > 0){
            if (powerup == "lsaber"){
                lsaber = Instantiate(lsaberPrefab, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity) as GameObject;
                powerup = "lsaberA";
                //yield return new WaitForSeconds(lsaberEnPeriod);
            }else if (powerup == "lsaberA"){
                energy -= lsaberEn;
                moveSpeedCurrent = moveSpeed*lsaberSpeedMulti;
                yield return new WaitForSeconds(lsaberEnPeriod);
            }else{
                var lsaberName = lsaberPrefab.name; var lsaberName1 = lsaberPrefab.name + "(Clone)";
                Destroy(GameObject.Find(lsaberName));
                Destroy(GameObject.Find(lsaberName1));
                moveSpeedCurrent = moveSpeed;
            }
        }else { energy = 0; yield return new WaitForSeconds(1f);
            var lsaberName = lsaberPrefab.name; var lsaberName1 = lsaberPrefab.name + "(Clone)";
            Destroy(GameObject.Find(lsaberName));
            Destroy(GameObject.Find(lsaberName1));
            moveSpeedCurrent = moveSpeed;
            powerup = "laser";
        }
    }
    #endregion

    private void States(){
        if (flip == true) { flipTimer -= Time.deltaTime; moveDir = -1; } else { moveDir = 1; }
        if(flipTimer<= 0 && flipTimer>-4) { flip = false; flipTimer = -4; AudioSource.PlayClipAtPoint(powerupOffSFX, new Vector2(transform.position.x, transform.position.y)); }

        if (gclover == true) {
            health = maxHP;
            FindObjectOfType<HPBar>().GetComponent<HPBar>().gclover = true;
            gcloverTimer -= Time.deltaTime;
        }
        else{
            FindObjectOfType<HPBar>().GetComponent<HPBar>().gclover = false;
        }
        if (gcloverTimer <= 0 && gcloverTimer>-4) { gclover = false; gcloverTimer = -4; AudioSource.PlayClipAtPoint(gcloverOffSFX, new Vector2(transform.position.x, transform.position.y)); }

        if (shadow == true) { shadowTimer -= Time.deltaTime; }
        if (shadowTimer <= 0 && shadowTimer > -4) { shadow = false; shadowTimer = -4; AudioSource.PlayClipAtPoint(powerupOffSFX, new Vector2(transform.position.x, transform.position.y)); }
        if (shadow==true){ Shadow(); GetComponent<BackflameEffect>().enabled = false; }
        else{ dashTime = startDashTime; rb.velocity = Vector2.zero; GetComponent<BackflameEffect>().enabled=true; }
        if (dashTime <= 0) { rb.velocity = Vector2.zero; dashing = false; }
        else{ dashTime -= Time.deltaTime; }
    }
    private void Shadow(){
        GameObject shadow = Instantiate(shadowPrefab,new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
        Destroy(shadow.gameObject, shadowLength);
        //yield return new WaitForSeconds(0.2f);
    }
    private void Die(){
        if (health <= 0){
            GameObject explosion = Instantiate(explosionVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            AudioSource.PlayClipAtPoint(deathSFX, new Vector2(transform.position.x, transform.position.y));
            Destroy(explosion, 0.5f);
            Destroy(gameObject, 0.05f);
            gameOverCanvasPrefab.gameObject.SetActive(true);
            var lsaberName = lsaberPrefab.name; var lsaberName1 = lsaberPrefab.name + "(Clone)";
            Destroy(GameObject.Find(lsaberName));
            Destroy(GameObject.Find(lsaberName1));
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
}
