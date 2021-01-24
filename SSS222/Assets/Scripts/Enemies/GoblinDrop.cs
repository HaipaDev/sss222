using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GoblinDrop : MonoBehaviour{
    [SerializeField] Sprite bossSprite;
    [SerializeField] float bossHp;
    //[SerializeField] AudioClip goblinStealSFX;
    //[SerializeField] AudioClip goblinDeathSFX;
    public List<GameObject> powerup;
    /*
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
    [SerializeField] GameObject shadowPwrupPrefab;
    [SerializeField] GameObject shadowBTPwrupPrefab;
    */

    Enemy enemy;
    Rigidbody2D rb;
    AudioSource myAudioSource;
    GameObject questionMarkObj;
    bool confused=false;
    bool bossForm=false;
    float yMax=6.6f;
    float yMin=-4.1f;
    float vspeed=0.09f;
    bool moveDown;
    Vector2 pos;
    // Start is called before the first frame update
    void Start(){
        var i=GameRules.instance;if(i!=null){bossHp=i.goblinBossHP;}
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        myAudioSource = GetComponent<AudioSource>();
        questionMarkObj=transform.GetChild(0).gameObject;
        questionMarkObj.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        if(powerup != null){
            rb.velocity = new Vector2(Random.Range(2.5f,3f),Random.Range(2.5f,3f));
        }
        if(bossForm)BossAI();
    }
    
    public void DropPowerup(bool sound){
        if(powerup.Count>0){
        foreach(GameObject pwrup in powerup){
            if(pwrup!=null){
                //Instantiate(powerup,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
                pwrup.SetActive(true);
                pwrup.transform.position=transform.position;
                if(pwrup.GetComponent<FallDown>()!=null)pwrup.GetComponent<Rigidbody2D>().velocity = Vector2.down*pwrup.GetComponent<FallDown>().GetVSpeed();
            }
        }
        }
        if(sound){
        if(bossForm!=true)AudioManager.instance.Play("GoblinDeath");
        else AudioManager.instance.Play("GoblinDeathTransf");
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(bossForm!=true){
            if(other.CompareTag("Powerups")&&(!other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)&&!other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)&&!other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name))){
                AudioManager.instance.Play("GoblinSteal");
                powerup.Add(other.gameObject);
                other.gameObject.SetActive(false);
                questionMarkObj.SetActive(false);
                confused=false;
            }else if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)||other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)){
                if(confused==false){
                    AudioManager.instance.Play("GoblinConfused");
                    questionMarkObj.SetActive(true);
                    confused=true;
                }
            }else if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name)){//Transform
                AudioManager.instance.Play("GoblinTransform");
                powerup.Add(other.gameObject);
                other.gameObject.SetActive(false);
                GetComponent<Follow>().enabled=false;
                GetComponent<BackflameEffect>().enabled=false;
                if(transform.GetChild(1)!=false)Destroy(transform.GetChild(1).gameObject);
                GetComponent<SpriteRenderer>().sprite=bossSprite;
                GetComponent<Enemy>().shooting=true;
                GetComponent<Enemy>().health=bossHp;
                bossForm=true;
                questionMarkObj.SetActive(false);
                confused=false;
                pos.x=transform.position.x;
                transform.rotation=new Quaternion(0,0,0,0);
            }
        }
    }
    void BossAI(){
        if(Time.timeScale>0.0001){
            float curSpeed=vspeed*Time.timeScale;
            if(!moveDown&&pos.y<yMax)pos.y+=curSpeed;
            if(pos.y>=yMax)moveDown=true;
            if(moveDown)pos.y-=curSpeed;
            if(pos.y<=yMin)moveDown=false;
            transform.position=pos;
        }
    }
}
