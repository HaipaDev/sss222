using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOn : MonoBehaviour{
    [SerializeField] bool on=false;
    public void AnimationSet(bool on){
        this.on=on;
        if(on==true){
            GetComponent<Animator>().enabled=true;
            GetComponent<Animator>().SetTrigger("on");
        }else{
            GetComponent<Animator>().enabled=false;
        }
    }
}
