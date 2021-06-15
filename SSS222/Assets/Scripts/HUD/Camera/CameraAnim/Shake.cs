using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour{
    public Animator camAnim;
    [SerializeField] bool debug;
    [HideInInspector]public float mult;
    [SerializeField]float x=0;
    [SerializeField]float y=0;
    public void CamShake(float multiplier, float speed){
    if(SaveSerial.instance.settingsData.screenshake){
        if(debug==true)Debug.Log("Mult Before: "+mult);
        if(multiplier>mult||camAnim.GetBool("shake")!=true){
        camAnim.ResetTrigger("shake");
        camAnim.SetTrigger("shake");
        camAnim.speed=speed;
        mult=multiplier;
        if(SaveSerial.instance.settingsData.vibrations)Vibrator.Vibrate((int)(22*(mult/speed)));
        if(debug==true)Debug.Log("Mult After: "+mult);
        }
        //camAnim.transform.position=new Vector3(x,y,10);
    }
    }
    private void Update() {
        camAnim.transform.position=new Vector3(x*mult,y*mult,-10);
    }
}
