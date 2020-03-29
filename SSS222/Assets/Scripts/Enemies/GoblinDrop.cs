using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GoblinDrop : MonoBehaviour{
    [SerializeField] AudioClip goblinStealSFX;
    [SerializeField] AudioClip goblinDeathSFX;
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
    public GameObject powerup;

    Enemy enemy;
    Rigidbody2D rb;
    AudioSource myAudioSource;
    AudioMixer mixer;
    string _OutputMixer;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        myAudioSource = GetComponent<AudioSource>();
        mixer = Resources.Load("MainMixer") as AudioMixer;
        _OutputMixer = "SoundVolume";
    }

    // Update is called once per frame
    void Update()
    {
        if (powerup != null){
            rb.velocity = new Vector2(Random.Range(2.5f,3f),Random.Range(2.5f,3f));
        }
    }
    
    public void DropPowerup(){
        if(powerup!=null){
            Instantiate(powerup,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
        }
        PlayClipAt(goblinDeathSFX, transform.position);
    }
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

    private void OnTriggerEnter2D(Collider2D other){
        if(CompareTag("Powerups")){
            PlayClipAt(goblinStealSFX,transform.position);
            var armorName = armorPwrupPrefab.name; var armorName1 = armorPwrupPrefab.name + "(Clone)";
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
            if (other.gameObject.name == shadowbtName || other.gameObject.name == shadowbtName1) { powerup = shadowBTPwrupPrefab; Destroy(other.gameObject); }
        }
    }
}
