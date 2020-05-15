using UnityEngine;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour{
    [HeaderAttribute("Enemy")]
    [SerializeField] public float health = 100f;
    float shotCounter;
    [SerializeField] bool shooting = false;
    [SerializeField] float minTimeBtwnShots=0.2f;
    [SerializeField] float maxTimeBtwnShots=1f;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed = 8f;
    [SerializeField] bool DBullets = false;
    [SerializeField] float bulletDist=0.35f;
    [SerializeField] bool randomizeWaveDeath = false;
    [SerializeField] bool flyOff = false;
    [HeaderAttribute("Drops & Points")]
    //[SerializeField] int scoreValue = 1;
    [SerializeField] public bool givePts = true;
    [SerializeField] int scoreValueStart = 1;
    [SerializeField] int scoreValueEnd = 10;
    [SerializeField] float enBallchanceInit = 30f;
    [SerializeField] float CoinchanceInit = 3f;
    [SerializeField] float powercoreChanceInit = 0f;
    [HideInInspector] public float enBallchance;
    [HideInInspector] public float Coinchance;
    [HideInInspector] public float powercoreChance;
    
    [HeaderAttribute("Effects")]
    [SerializeField] public GameObject explosionVFX;
    [SerializeField] public GameObject explosionSmallVFX;
    [SerializeField] public GameObject flareHitVFX;
    [SerializeField] public AudioClip explosionSFX;
    [SerializeField] public AudioClip enemyHitSFX;
    [SerializeField] public AudioClip mlaserHitSFX;
    [SerializeField] public AudioClip hrocketHitSFX;
    [SerializeField] public AudioClip phaserHitSFX;
    [SerializeField] public AudioClip lsaberHitSFX;
    [SerializeField] public AudioClip lclawsHitSFX;
    [SerializeField] public AudioClip shadowbtHitSFX;
    [SerializeField] public AudioClip qrocketHitSFX;
    [SerializeField] public AudioClip procketHitSFX;
    [SerializeField] public AudioClip cbulletHitSFX;
    [HeaderAttribute("Damage Dealers")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject mlaserPrefab;
    [SerializeField] GameObject hrocketPrefab;
    [SerializeField] GameObject phaserPrefab;
    [SerializeField] GameObject lsaberPrefab;
    [SerializeField] GameObject shadowbtPrefab;
    [SerializeField] GameObject hlaserPrefab;
    [SerializeField] GameObject qrocketPrefab;
    [SerializeField] GameObject procketPrefab;
    [SerializeField] GameObject procketExplPrefab;
    [SerializeField] GameObject cbulletPrefab;
    [SerializeField] GameObject lclawsPrefab;
    [SerializeField] GameObject lclawsPartPrefab;
    [HeaderAttribute("Drops")]
    [SerializeField] GameObject energyBallPrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject powercorePrefab;
    [HeaderAttribute("Others")]
    //[SerializeField] public bool cTagged=false;
    [SerializeField] public bool yeeted=false;

    AudioSource myAudioSource;
    Rigidbody2D rb;
    GameSession gameSession;
    Player player;
    Shake shake;
    AudioMixer mixer;
    string _OutputMixer;
    AudioSource PlayClipAt(AudioClip clip, Vector2 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
                             // set other aSource properties here, if desired
        _OutputMixer = "SoundVolume";
        aSource.outputAudioMixerGroup = myAudioSource.outputAudioMixerGroup;
        aSource.Play(); // start the sound
        MonoBehaviour.Destroy(tempGO, aSource.clip.length); // destroy object after clip duration (this will not account for whether it is set to loop)
        return aSource; // return the AudioSource reference
    }

    // Start is called before the first frame update
    void Start(){
        enBallchance = Random.Range(0f, 100f);
        Coinchance = Random.Range(0f, 100f);
        powercoreChance = Random.Range(0f, 100f);
        if (enBallchance <= enBallchanceInit && enBallchanceInit>0){ enBallchance = 1; }
        if (Coinchance <= CoinchanceInit && CoinchanceInit>0) { Coinchance = 1; }
        if (powercoreChance <= powercoreChanceInit && powercoreChanceInit>0) { powercoreChance = 1; }
        shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
        myAudioSource = GetComponent<AudioSource>();
        rb=GetComponent<Rigidbody2D>();
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
        shake = GameObject.FindObjectOfType<Shake>().GetComponent<Shake>();

        mixer = Resources.Load("MainMixer") as AudioMixer;
        _OutputMixer = "SoundVolume";
        //GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups(_OutputMixer)[0];
    }

    // Update is called once per frame
    void Update(){
        if (shooting){Shoot();}
        if(flyOff){FlyOff();}
        Die();
        DestroyOutside();
    }
    
    private void Shoot(){
        shotCounter -= Time.deltaTime;
        if(shotCounter<=0f){
            if(DBullets!=true){
                var bt=Instantiate(bullet, transform.position,Quaternion.identity) as GameObject;
                bt.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
                shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
            }else{
                var pos1 = new Vector2(transform.position.x+bulletDist,transform.position.y);
                var bt1 = Instantiate(bullet, pos1, Quaternion.identity) as GameObject;
                bt1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
                var pos2 = new Vector2(transform.position.x - bulletDist, transform.position.y);
                var bt2 = Instantiate(bullet, pos2, Quaternion.identity) as GameObject;
                bt2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
                shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
            }
        }
    }
    private void FlyOff(){
        if(player==null){
            shooting=false;
            rb.velocity=new Vector2(0,3f);
        }
    }
    
    public void Die(){
        if (health <= 0){
            int scoreValue = Random.Range(scoreValueStart,scoreValueEnd);
            if(givePts==true){
                gameSession.AddToScore(scoreValue);
                if(enBallchance==1){ Instantiate(energyBallPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
                if(Coinchance==1){ Instantiate(coinPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
                if(powercoreChance==1){ Instantiate(powercorePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
            }
            GameObject explosion = Instantiate(explosionVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            PlayClipAt(explosionSFX, new Vector2(transform.position.x, transform.position.y));
            if (GetComponent<GoblinDrop>()!=null)GetComponent<GoblinDrop>().DropPowerup();
            Destroy(explosion, 0.5f);
            Destroy(gameObject);
            shake.CamShake();
            gameSession.AddEnemyCount();
        }
    }
    private void OnDestroy() {
        if (GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber);
        if(randomizeWaveDeath==true){ gameSession.EVscore = gameSession.EVscoreMax; }
    }
    private void DestroyOutside(){
        if((transform.position.x>6.5f || transform.position.x<-6.5f) || (transform.position.y>10f || transform.position.y<-10f)){ if(yeeted==true){givePts=true; health=-1; Die();} else{ Destroy(gameObject,0.001f); if(GetComponent<GoblinDrop>()!=null){Destroy(GetComponent<GoblinDrop>().powerup);}}}
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(!other.CompareTag(tag)){
            DamageDealer damageDealer=other.gameObject.GetComponent<DamageDealer>();
            if(!damageDealer){ return; }
            var dmg = damageDealer.GetDamage();

            var Lname = laserPrefab.name; var Lname1 = laserPrefab.name + "(Clone)";
            if (other.gameObject.name == Lname || other.gameObject.name == Lname1) { dmg = damageDealer.GetDamageLaser(); Destroy(other.gameObject); PlayClipAt(enemyHitSFX, new Vector2(transform.position.x, transform.position.y)); }
            
            var MLname = mlaserPrefab.name; var MLname1 = mlaserPrefab.name + "(Clone)";
            if (other.gameObject.name == MLname || other.gameObject.name == MLname1) { 
                PlayClipAt(mlaserHitSFX, new Vector2(transform.position.x, transform.position.y));
                /*var mlaserHitSound = other.GetComponent<RandomSound>().sound;
                if(other.GetComponent<RandomSound>().playLimitForThis==true){mlaserHitSound=other.GetComponent<RandomSound>().sound2;}
                PlayClipAt(mlaserHitSound, new Vector2(transform.position.x, transform.position.y));*/
                dmg = damageDealer.GetDamageMiniLaser(); Destroy(other.gameObject);}
            
            var HRname = hrocketPrefab.name; var HRname1 = hrocketPrefab.name + "(Clone)";
            if (other.gameObject.name == HRname || other.gameObject.name == HRname1) { dmg = damageDealer.GetDamageHRocket(); Destroy(other.gameObject); PlayClipAt(hrocketHitSFX, new Vector2(transform.position.x, transform.position.y));
                var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }

            var LSabername = lsaberPrefab.name; var LSabername1 = lsaberPrefab.name + "(Clone)";
            if (other.gameObject.name == LSabername || other.gameObject.name == LSabername1){ dmg = damageDealer.GetDamageLSaber()*9f; PlayClipAt(lsaberHitSFX, new Vector2(transform.position.x, transform.position.y)); }

            var LClawsname = lclawsPrefab.name; var LClawsname1 = lclawsPrefab.name + "(Clone)";
            if (other.gameObject.name == LClawsname || other.gameObject.name == LClawsname1){ dmg = damageDealer.GetDamageLSaber()/3; PlayClipAt(lclawsHitSFX, new Vector2(transform.position.x, transform.position.y)); FindObjectOfType<Player>().energy-=1f;}
            var LClawsPartname = lclawsPartPrefab.name; var LClawsPartname1 = lclawsPartPrefab.name + "(Clone)";
            if (other.gameObject.name == LClawsPartname || other.gameObject.name == LClawsPartname1){ dmg = damageDealer.GetDamageLClaws(); PlayClipAt(lclawsHitSFX, new Vector2(transform.position.x, transform.position.y)); }

            var shadowbtName = shadowbtPrefab.name; var shadowbtName1 = shadowbtPrefab.name + "(Clone)";
            if (other.gameObject.name == shadowbtName || other.gameObject.name == shadowbtName1) { dmg = damageDealer.GetDamageShadowBT(); PlayClipAt(shadowbtHitSFX, new Vector2(transform.position.x, transform.position.y));}
            

            var QRname = qrocketPrefab.name; var QRname1 = qrocketPrefab.name + "(Clone)";
            if (other.gameObject.name == QRname || other.gameObject.name == QRname1) { dmg = damageDealer.GetDamageQRocket(); Destroy(other.gameObject); PlayClipAt(qrocketHitSFX, new Vector2(transform.position.x, transform.position.y));
                var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }
            var PRname = procketPrefab.name; var PRname1 = procketPrefab.name + "(Clone)";
            if (other.gameObject.name == PRname || other.gameObject.name == PRname1) { dmg = damageDealer.GetDamagePRocket(); //PlayClipAt(hrocketHitSFX, new Vector2(transform.position.x, transform.position.y));
                //var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }

            /*var Pname = phaserPrefab.name; var Pname1 = phaserPrefab.name + "(Clone)";
            if (other.gameObject.name != Pname && other.gameObject.name != Pname1){Destroy(other.gameObject,0.05f);}*/
            var PRExplname = procketExplPrefab.name; var PRExplname1 = procketExplPrefab.name + "(Clone)";
            if (other.gameObject.name == PRExplname || other.gameObject.name == PRExplname1) { dmg = damageDealer.GetDamagePRocketExpl(); GetComponent<Rigidbody2D>().velocity = Vector2.up*6f; yeeted=true; }// PlayClipAt(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}

            var cBulletname = cbulletPrefab.name; var cBulletname1 = cbulletPrefab.name + "(Clone)";
            if (other.gameObject.name == cBulletname || other.gameObject.name == cBulletname1) { dmg = damageDealer.GetDamageCBullet(); PlayClipAt(cbulletHitSFX, new Vector2(transform.position.x, transform.position.y));}

            health -= dmg;
            //AudioSource.PlayClipAtPoint(enemyHitSFX, new Vector2(transform.position.x, transform.position.y));
            var flare = Instantiate(flareHitVFX, new Vector2(transform.position.x,transform.position.y - 0.5f), Quaternion.identity);
            Destroy(flare.gameObject, 0.3f);
        }else if(other.CompareTag(tag)){
            var hlaserName = hlaserPrefab.name; var hlaserName1 = hlaserPrefab.name + "(Clone)";
            if (other.gameObject.name == hlaserName || other.gameObject.name == hlaserName1) { this.givePts = false; this.health = -1; this.Die(); }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(tag))
        {
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (!damageDealer) { return; }
            float dmg = damageDealer.GetDamage();

            var Pname = phaserPrefab.name; var Pname1 = phaserPrefab.name + "(Clone)";
            if (other.gameObject.name == Pname || other.gameObject.name == Pname1) {dmg = damageDealer.GetDamagePhaser(); PlayClipAt(phaserHitSFX, new Vector2(transform.position.x, transform.position.y)); }
            //else { dmg = 0; }

            var LSabername = lsaberPrefab.name; var LSabername1 = lsaberPrefab.name + "(Clone)";
            if (other.gameObject.name == LSabername || other.gameObject.name == LSabername1) { dmg = damageDealer.GetDamageLSaber(); PlayClipAt(enemyHitSFX, new Vector2(transform.position.x, transform.position.y)); }
            //else { dmg = 0; }
            

            var PRExplname = procketExplPrefab.name; var PRExplname1 = procketExplPrefab.name + "(Clone)";
            if (other.gameObject.name == PRExplname || other.gameObject.name == PRExplname1) { dmg = damageDealer.GetDamagePRocketExpl(); }//PlayClipAt(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}
            //else {dmg=0;}

            var cBulletname = cbulletPrefab.name; var cBulletname1 = cbulletPrefab.name + "(Clone)";
            if (other.gameObject.name == cBulletname || other.gameObject.name == cBulletname1) { dmg = damageDealer.GetDamageCBullet(); PlayClipAt(cbulletHitSFX, new Vector2(transform.position.x, transform.position.y));}
            //else {dmg=0;}

            var LClawsname = lclawsPrefab.name; var LClawsname1 = lclawsPrefab.name + "(Clone)";
            if (other.gameObject.name == LClawsname || other.gameObject.name == LClawsname1){ dmg = damageDealer.GetDamageLSaber()/3; FindObjectOfType<Player>().energy-=0.1f;}//PlayClipAt(lclawsHitSFX, new Vector2(transform.position.x, transform.position.y)); }


            health -= dmg;
            //Destroy(other.gameObject, 0.05f);

            //AudioSource.PlayClipAtPoint(enemyHitSFX, new Vector2(transform.position.x, transform.position.y));
            var flare = Instantiate(flareHitVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity);
            Destroy(flare.gameObject, 0.3f);
        }
    }
}
