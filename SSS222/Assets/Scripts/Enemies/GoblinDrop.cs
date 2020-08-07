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
    void Start()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        myAudioSource = GetComponent<AudioSource>();
        questionMarkObj=transform.GetChild(0).gameObject;
        questionMarkObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (powerup != null){
            rb.velocity = new Vector2(Random.Range(2.5f,3f),Random.Range(2.5f,3f));
        }
        if(bossForm)BossAI();
    }
    
    public void DropPowerup(){
        if(powerup.Count>0){
        foreach(GameObject pwrup in powerup){
            if(pwrup!=null){
                //Instantiate(powerup,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
                pwrup.SetActive(true);
                pwrup.transform.position=transform.position;
                if(GetComponent<FallDown>()!=null)pwrup.GetComponent<Rigidbody2D>().velocity = Vector2.down*pwrup.GetComponent<FallDown>().GetVSpeed();
            }
        }
        }
        if(bossForm!=true)AudioManager.instance.Play("GoblinDeath");
        else AudioManager.instance.Play("GoblinDeathTransf");
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(bossForm!=true){
            if(other.CompareTag("Powerups")&&(!other.gameObject.name.Contains("EnergyBall")&&!other.gameObject.name.Contains("Coin")&&!other.gameObject.name.Contains("Powercore"))){
                AudioManager.instance.Play("GoblinSteal");
                powerup.Add(other.gameObject);
                other.gameObject.SetActive(false);
                questionMarkObj.SetActive(false);
                confused=false;
            }else if(other.gameObject.name.Contains("EnergyBall")||other.gameObject.name.Contains("Coin")){
                if(confused==false){
                    AudioManager.instance.Play("GoblinConfused");
                    questionMarkObj.SetActive(true);
                    confused=true;
                }
            }else if(other.gameObject.name.Contains("Powercore")){//Transform
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
                pos.x=transform.position.x;
                transform.rotation=new Quaternion(0,0,0,0);
            }
        }
        #region//Old
            //Destroy(other.gameObject,0.002f);
            /*var armorName = armorPwrupPrefab.name; var armorName1 = armorPwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == armorName || other.gameObject.name == armorName1) { powerup = armorPwrupPrefab; Destroy(other.gameObject); }
            var armorUName = armorUPwrupPrefab.name; var armorUName1 = armorUPwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == armorUName || other.gameObject.name == armorUName1) { powerup = armorUPwrupPrefab; Destroy(other.gameObject); }

            var flipName = flipPwrupPrefab.name; var flipName1 = flipPwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == flipName || other.gameObject.name == flipName1) { powerup= flipPwrupPrefab; Destroy(other.gameObject); }

            var gcloverName = gcloverPwrupPrefab.name; var gcloverName1 = gcloverPwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == gcloverName || other.gameObject.name == gcloverName1){ powerup = gcloverPwrupPrefab; Destroy(other.gameObject); }

            var shadowName = shadowPwrupPrefab.name; var shadowName1 = shadowPwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == shadowName || other.gameObject.name == shadowName1){ powerup = shadowPwrupPrefab; Destroy(other.gameObject); }


            var laser2Name = laser2PwrupPrefab.name; var laser2Name1 = laser2PwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == laser2Name || other.gameObject.name == laser2Name1) { powerup = laser2PwrupPrefab; Destroy(other.gameObject); }

            var laser3Name = laser3PwrupPrefab.name; var laser3Name1 = laser3PwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == laser3Name || other.gameObject.name == laser3Name1) { powerup = laser3PwrupPrefab; Destroy(other.gameObject); }

            var phaserName = phaserPwrupPrefab.name; var phaserName1 = phaserPwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == phaserName || other.gameObject.name == phaserName1) { powerup = phaserPwrupPrefab; Destroy(other.gameObject); }

            var hrocketName = hrocketPwrupPrefab.name; var hrocketName1 = hrocketPwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == hrocketName || other.gameObject.name == hrocketName1) { powerup = hrocketPwrupPrefab; Destroy(other.gameObject); }

            var minilaserName = mlaserPwrupPrefab.name; var minilaserName1 = mlaserPwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == minilaserName || other.gameObject.name == minilaserName1) { powerup = mlaserPwrupPrefab; Destroy(other.gameObject); }

            var lsaberName = lsaberPwrupPrefab.name; var lsaberName1 = lsaberPwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == lsaberName || other.gameObject.name == lsaberName1) { powerup = lsaberPwrupPrefab; Destroy(other.gameObject); }

            var shadowbtName = shadowBTPwrupPrefab.name; var shadowbtName1 = shadowBTPwrupPrefab.name + "(Clone)";
            if (other.gameObject.name == shadowbtName || other.gameObject.name == shadowbtName1) { powerup = shadowBTPwrupPrefab; Destroy(other.gameObject); }*/
        //}
        #endregion
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
