using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalSound : MonoBehaviour{
    [SerializeField] public float interval=0f;
    void Update()
    {
        if(interval>-4)interval -= Time.deltaTime;
        if(interval <= 0 && interval > -4){
            GetComponent<AudioSource>().Play(0);
            interval = -4;
        }
    }
}
