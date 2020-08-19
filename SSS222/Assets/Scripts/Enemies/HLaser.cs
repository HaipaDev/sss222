using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HLaser : MonoBehaviour{
    [SerializeField] GameObject exclamationPrefab;
    [SerializeField] GameObject hlaserChargingPrefab;
    [SerializeField] GameObject hlaserPrefab;
    [SerializeField] float timerExcl=1f;
    [SerializeField] float timerHlaserCharging=1.12f;
    [SerializeField] float timerHlaser=3.3f;
    public float timer=-4;
    // Start is called before the first frame update
    void Awake()
    {
        if(gameObject.name==exclamationPrefab.name || gameObject.name==exclamationPrefab.name+"(Clone)"){
            timer = timerExcl;
        }else if (gameObject.name == hlaserPrefab.name || gameObject.name == hlaserPrefab.name + "(Clone)"){
            timer = timerHlaserCharging;
        }else if(gameObject.name== hlaserPrefab.name || gameObject.name== hlaserPrefab.name+"(Clone)"){
            timer = timerHlaser;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == exclamationPrefab.name || gameObject.name == exclamationPrefab.name + "(Clone)"){
            timer -= Time.deltaTime;
            if(timer <= 0 && timer>-4){
                GameObject obj = Instantiate(hlaserChargingPrefab, new Vector2(transform.position.x,transform.position.y+0.24f),Quaternion.identity);
                obj.GetComponent<HLaser>().timer = timerHlaserCharging;
                Destroy(gameObject);
            }
        }else if (gameObject.name == hlaserChargingPrefab.name || gameObject.name == hlaserChargingPrefab.name + "(Clone)"){
            timer -= Time.deltaTime;
            if (timer <= 0 && timer > -4)
            {
                GameObject obj = Instantiate(hlaserPrefab, new Vector2(transform.position.x, transform.position.y+0.74f), Quaternion.identity);
                obj.GetComponent<HLaser>().timer = timerHlaser;
                Destroy(gameObject);
            }
        }else if (gameObject.name == hlaserPrefab.name || gameObject.name == hlaserPrefab.name + "(Clone)"){
            timer -= Time.deltaTime;
            if (timer <= 0 && timer > -4)
            {
                Destroy(gameObject);
            }
        }
    }
}
