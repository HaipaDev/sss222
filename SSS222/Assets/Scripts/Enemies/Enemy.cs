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
    [SerializeField] float enBallchanceInit = 30f;
    [SerializeField] float CoinchanceInit = 3f;
    public float enBallchance;
    public float Coinchance;
    //[SerializeField] int scoreValue = 1;
    [SerializeField] public bool givePts = true;
    [SerializeField] int scoreValueStart = 1;
    [SerializeField] int scoreValueEnd = 10;
    [SerializeField] bool randomizeWaveDeath = false;
    [HeaderAttribute("Effects")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] GameObject explosionSmallVFX;
    [SerializeField] GameObject flareHitVFX;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] AudioClip enemyHitSFX;
    [SerializeField] AudioClip mlaserHitSFX;
    [SerializeField] AudioClip hrocketHitSFX;
    [SerializeField] AudioClip phaserHitSFX;
    [SerializeField] AudioClip lsaberHitSFX;
    [SerializeField] AudioClip shadowbtHitSFX;
    [HeaderAttribute("Damage Dealers")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject mlaserPrefab;
    [SerializeField] GameObject hrocketPrefab;
    [SerializeField] GameObject phaserPrefab;
    [SerializeField] GameObject lsaberPrefab;
    [SerializeField] GameObject shadowbtPrefab;
    [SerializeField] GameObject hlaserPrefab;
    [HeaderAttribute("Drops")]
    [SerializeField] GameObject energyBallPrefab;
    [SerializeField] GameObject coinPrefab;

    AudioSource myAudioSource;
    GameSession gameSession;
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
        if (enBallchance <= enBallchanceInit){ enBallchance = 1; }
        if (Coinchance <= CoinchanceInit) { Coinchance = 1; }
        shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
        myAudioSource = GetComponent<AudioSource>();
        gameSession = FindObjectOfType<GameSession>();
        shake = GameObject.FindObjectOfType<Shake>().GetComponent<Shake>();

        mixer = Resources.Load("MainMixer") as AudioMixer;
        _OutputMixer = "SoundVolume";
        //GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups(_OutputMixer)[0];
    }

    // Update is called once per frame
    void Update(){
        if (shooting == true){ Shoot(); }
        Die();
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

    public void Die(){
        if (health <= 0){
            int scoreValue = Random.Range(scoreValueStart,scoreValueEnd);
            if(givePts==true){gameSession.AddToScore(scoreValue);}
            GameObject explosion = Instantiate(explosionVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            PlayClipAt(explosionSFX, new Vector2(transform.position.x, transform.position.y));
            if(enBallchance==1){ Instantiate(energyBallPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
            if(Coinchance==1){ Instantiate(coinPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
            if(randomizeWaveDeath==true){ gameSession.EVscore = 50; }
            if (GetComponent<GoblinDrop>() != null) GetComponent<GoblinDrop>().DropPowerup();
            Destroy(explosion, 0.5f);
            Destroy(gameObject);
            shake.CamShake();
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(!other.CompareTag(tag)){
            DamageDealer damageDealer=other.gameObject.GetComponent<DamageDealer>();
            if(!damageDealer){ return; }
            var dmg = damageDealer.GetDamage();

            var Lname = laserPrefab.name; var Lname1 = laserPrefab.name + "(Clone)";
            if (other.gameObject.name == Lname || other.gameObject.name == Lname1) { dmg = damageDealer.GetDamageLaser(); Destroy(other.gameObject); PlayClipAt(enemyHitSFX, new Vector2(transform.position.x, transform.position.y)); }
            
            var MLname = mlaserPrefab.name; var MLname1 = mlaserPrefab.name + "(Clone)";
            if (other.gameObject.name == MLname || other.gameObject.name == MLname1) { dmg = damageDealer.GetDamageMiniLaser(); Destroy(other.gameObject); PlayClipAt(mlaserHitSFX, new Vector2(transform.position.x, transform.position.y)); }
            
            var HRname = hrocketPrefab.name; var HRname1 = hrocketPrefab.name + "(Clone)";
            if (other.gameObject.name == HRname || other.gameObject.name == HRname1) { dmg = damageDealer.GetDamageHRocket(); Destroy(other.gameObject); PlayClipAt(hrocketHitSFX, new Vector2(transform.position.x, transform.position.y));
                var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }

            var LSabername = lsaberPrefab.name; var LSabername1 = lsaberPrefab.name + "(Clone)";
            if (other.gameObject.name == LSabername || other.gameObject.name == LSabername1){ dmg = damageDealer.GetDamageLSaber(); PlayClipAt(lsaberHitSFX, new Vector2(transform.position.x, transform.position.y)); }

            var shadowbtName = shadowbtPrefab.name; var shadowbtName1 = shadowbtPrefab.name + "(Clone)";
            if (other.gameObject.name == shadowbtName || other.gameObject.name == shadowbtName1) { dmg = damageDealer.GetDamageShadowBT(); PlayClipAt(shadowbtHitSFX, new Vector2(transform.position.x, transform.position.y));}



            /*var Pname = phaserPrefab.name; var Pname1 = phaserPrefab.name + "(Clone)";
            if (other.gameObject.name != Pname && other.gameObject.name != Pname1){Destroy(other.gameObject,0.05f);}*/

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
            else { dmg = 0; }

            var LSabername = lsaberPrefab.name; var LSabername1 = lsaberPrefab.name + "(Clone)";
            if (other.gameObject.name == LSabername || other.gameObject.name == LSabername1) { dmg = damageDealer.GetDamageLSaber(); PlayClipAt(enemyHitSFX, new Vector2(transform.position.x, transform.position.y)); }
            else { dmg = 0; }
            health -= dmg;
            //Destroy(other.gameObject, 0.05f);

            //AudioSource.PlayClipAtPoint(enemyHitSFX, new Vector2(transform.position.x, transform.position.y));
            var flare = Instantiate(flareHitVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity);
            Destroy(flare.gameObject, 0.3f);
        }
    }
}
