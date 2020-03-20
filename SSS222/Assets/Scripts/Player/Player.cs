using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    //Config params
    [HeaderAttribute("Player")]
    [SerializeField] bool moveX = true;
    [SerializeField] bool moveY = true;
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
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject phaserPrefab;
    [SerializeField] GameObject hrocketPrefab;
    [SerializeField] GameObject mlaserPrefab;
    [SerializeField] GameObject lsaberPrefab;
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
    [HeaderAttribute("States")]
    [SerializeField] public bool flip = false;
    [SerializeField] float flipTime = 7f;
    public float flipTimer=-4;
    [SerializeField] public bool gclover = false;
    [SerializeField] float gcloverTime = 6f;
    public float gcloverTimer =-4;
    [HeaderAttribute("Energy Costs")]
    [SerializeField] float laserEn = 2f;
    [SerializeField] float laser2En = 4f;
    [SerializeField] float laser3En = 6f;
    [SerializeField] float phaserEn = 3f;
    [SerializeField] float hrocketEn = 5f;
    [SerializeField] float mlaserEn = 0.225f;
    [SerializeField] float lsaberEn = 0.2f;
    [SerializeField] float energyBallGet = 6f;
    [SerializeField] float medkitEnergyGet = 2f;
    [SerializeField] float medkitUEnergyGet = 8f;
    [SerializeField] float pwrupEnergyGet = 48f;
    [HeaderAttribute("Powerups")]
    [SerializeField] GameObject CoinPrefab;
    [SerializeField] GameObject enBallPrefab;
    [SerializeField] GameObject armorPwrupPrefab;
    [SerializeField] GameObject armorUPwrupPrefab;
    [SerializeField] GameObject laser2PwrupPrefab;
    [SerializeField] GameObject laser3PwrupPrefab;
    [SerializeField] GameObject phaserPwrupPrefab;
    [SerializeField] GameObject hrocketPwrupPrefab;
    [SerializeField] GameObject mlaserPwrupPrefab;
    [SerializeField] GameObject lsaberPwrupPrefab;
    [SerializeField] GameObject flipPwrupPrefab;
    [SerializeField] GameObject gcloverPwrupPrefab;
    [HeaderAttribute("Damage Dealers")]
    [SerializeField] GameObject cometPrefab;
    [SerializeField] GameObject batPrefab;
    [SerializeField] GameObject enShip1Prefab;
    [SerializeField] GameObject soundwavePrefab;
    [SerializeField] GameObject EBtPrefab;
    [HeaderAttribute("Effects")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] GameObject flareHitVFX;
    [SerializeField] GameObject flareShootVFX;
    [SerializeField] GameObject gcloverVFX;
    [SerializeField] GameObject gcloverOVFX;
    //[SerializeField] AudioClip shootLaserSFX;
    [SerializeField] AudioClip shipHitSFX;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip soundwaveHitSFX;
    [SerializeField] AudioClip powerupSFX;
    [SerializeField] AudioClip powerupOffSFX;
    [SerializeField] AudioClip gcloverSFX;
    [SerializeField] AudioClip gcloverOffSFX;
    [SerializeField] AudioClip noEnergySFX;
    [SerializeField] AudioClip energyBallSFX;
    [SerializeField] AudioClip coinSFX;
    [HeaderAttribute("Others")]
    [SerializeField] GameObject gameOverCanvasPrefab;
    [SerializeField] float flareShootYY = 0.2f;
    int moveDir = 1;

    GameSession gameSession;
    Coroutine shootCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start(){
        gameSession=FindObjectOfType<GameSession>();
        SetUpMoveBoundaries();
        moveSpeedCurrent = moveSpeed;
    }
    // Update is called once per frame
    void Update(){
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        health =Mathf.Clamp(health,0f,maxHP);
        MovePlayer();
        //PlayerBFlame();
        Shoot();
        StartCoroutine(DrawOtherWeapons());
        States();
        Die();
    }

    public float GetFlipTimer(){ return flipTimer; }
    public float GetGCloverTimer(){ return gcloverTimer; }

    private void MovePlayer()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedCurrent * moveDir;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedCurrent * moveDir;

        var newXpos = transform.position.x;
        var newYpos = transform.position.y;

        if (moveX==true) newXpos = Mathf.Clamp(transform.position.x,xMin,xMax) + deltaX;
        if (moveY == true) newYpos = Mathf.Clamp(transform.position.y,yMin,yMax) + deltaY;
        transform.position = new Vector2(newXpos,newYpos);
    }
    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Shoot(){
        if (Input.GetButtonDown("Fire1")){
            shootCoroutine = StartCoroutine(ShootContinuously());
        }
        if (Input.GetButtonUp("Fire1")){
            StopCoroutine(shootCoroutine);
        }
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
        if (flip == true) { moveDir = -1; flipTimer -= Time.deltaTime; } else { moveDir = 1; }
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


    private void OnTriggerEnter2D(Collider2D other){
        
        if (!other.CompareTag(tag))
        {
            #region//Enemies
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet")){
                DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
                if (!damageDealer) { return; }
                bool en = false;
                var dmg=damageDealer.GetDamage();

                var cometName = cometPrefab.name; var cometName1 = cometPrefab.name + "(Clone)";
                if (other.gameObject.name == cometName || other.gameObject.name == cometName1) { dmg = damageDealer.GetDamageComet(); en = true; }
                var batName = batPrefab.name; var batName1 = batPrefab.name + "(Clone)";
                if (other.gameObject.name == batName || other.gameObject.name == batName1){dmg = damageDealer.GetDamageBat();en=true;}
                var enShip1Name = enShip1Prefab.name; var enShip1Name1 = enShip1Prefab.name + "(Clone)";
                if (other.gameObject.name == enShip1Name || other.gameObject.name == enShip1Name1){dmg = damageDealer.GetDamageEnemyShip1();en=true;}

                var Sname = soundwavePrefab.name;var Sname1 = soundwavePrefab.name + "(Clone)";
                if (other.gameObject.name == Sname || other.gameObject.name == Sname1) {dmg =damageDealer.GetDamageSoundwave(); AudioSource.PlayClipAtPoint(soundwaveHitSFX, new Vector2(transform.position.x, transform.position.y)); }
                var EBtname = EBtPrefab.name; var EBtname1 = EBtPrefab.name + "(Clone)";
                if (other.gameObject.name == EBtname || other.gameObject.name == EBtname1) dmg = damageDealer.GetDamageEBt();
                health -= dmg;
                if(en!=true){Destroy(other.gameObject, 0.05f);}
                else{ if (other.GetComponent<Enemy>().health > -1) { other.GetComponent<Enemy>().givePts = false; other.GetComponent<Enemy>().health = -1; other.GetComponent<Enemy>().Die();  } }

                AudioSource.PlayClipAtPoint(shipHitSFX, new Vector2(transform.position.x, transform.position.y));
                var flare = Instantiate(flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                Destroy(flare.gameObject, 0.3f);
            }
            #endregion
            #region//Powerups
            else if (other.gameObject.CompareTag("Powerups")){
                var enBallName = enBallPrefab.name; var enBallName1 = enBallPrefab.name + "(Clone)";
                if (other.gameObject.name == enBallName || other.gameObject.name == enBallName1) { energy += energyBallGet; }

                var CoinName = CoinPrefab.name; var CoinName1 = CoinPrefab.name + "(Clone)";
                if (other.gameObject.name == CoinName || other.gameObject.name == CoinName1) { gameSession.coins += 1; }


                var armorName = armorPwrupPrefab.name; var armorName1 = armorPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == armorName || other.gameObject.name == armorName1) { health += 25; energy += medkitEnergyGet; }
                var armorUName = armorUPwrupPrefab.name; var armorUName1 = armorUPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == armorUName || other.gameObject.name == armorUName1) { health += 58; energy += medkitUEnergyGet; }

                var flipName = flipPwrupPrefab.name; var flipName1 = flipPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == flipName || other.gameObject.name == flipName1) { flip=true; flipTimer = flipTime; }

                var gcloverName = gcloverPwrupPrefab.name; var gcloverName1 = gcloverPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == gcloverName || other.gameObject.name == gcloverName1) { gclover=true; gcloverTimer = gcloverTime;
                    //GameObject gcloverexVFX = Instantiate(gcloverVFX, new Vector2(0, 0), Quaternion.identity);
                    GameObject gcloverexOVFX = Instantiate(gcloverOVFX, new Vector2(0, 0), Quaternion.identity);
                    //Destroy(gcloverexVFX, 1f);
                    Destroy(gcloverexOVFX, 1f);
                }



                var laser2Name = laser2PwrupPrefab.name; var laser2Name1 = laser2PwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == laser2Name || other.gameObject.name == laser2Name1) { powerup = "laser2"; energy += pwrupEnergyGet; }

                var laser3Name = laser3PwrupPrefab.name; var laser3Name1 = laser3PwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == laser3Name || other.gameObject.name == laser3Name1) { powerup = "laser3"; energy += pwrupEnergyGet; }

                var phaserName = phaserPwrupPrefab.name; var phaserName1 = phaserPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == phaserName || other.gameObject.name == phaserName1) { powerup = "phaser"; energy += pwrupEnergyGet; }
                
                var hrocketName = hrocketPwrupPrefab.name; var hrocketName1 = hrocketPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == hrocketName || other.gameObject.name == hrocketName1) { powerup = "hrockets"; energy += pwrupEnergyGet; }
                
                var minilaserName = mlaserPwrupPrefab.name; var minilaserName1 = mlaserPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == minilaserName || other.gameObject.name == minilaserName1) { powerup = "mlaser"; energy += pwrupEnergyGet; }
                
                var lsaberName = lsaberPwrupPrefab.name; var lsaberName1 = lsaberPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == lsaberName || other.gameObject.name == lsaberName1) { powerup = "lsaber"; energy += pwrupEnergyGet; }


                if (other.gameObject.name == enBallName || other.gameObject.name == enBallName1){
                    AudioSource.PlayClipAtPoint(energyBallSFX, new Vector2(transform.position.x, transform.position.y));
                }
                else if(other.gameObject.name == CoinName || other.gameObject.name == CoinName1)
                {
                    AudioSource.PlayClipAtPoint(coinSFX, new Vector2(transform.position.x, transform.position.y));
                } else if(other.gameObject.name == gcloverName || other.gameObject.name == gcloverName1)
                {
                    AudioSource.PlayClipAtPoint(gcloverSFX, new Vector2(transform.position.x, transform.position.y));
                } else{
                    AudioSource.PlayClipAtPoint(powerupSFX, new Vector2(transform.position.x, transform.position.y));
                }
                Destroy(other.gameObject, 0.05f);
            }
            #endregion
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
