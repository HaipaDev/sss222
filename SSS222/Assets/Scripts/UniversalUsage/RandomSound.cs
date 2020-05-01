using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour{
    [SerializeField] public bool forThisObject=true;
    [SerializeField] public bool playLimitForThis=true;
    [SerializeField] public int thisSoundLimit;
    [SerializeField] bool playAtStart=true;
    [SerializeField] AudioClip[] sounds;
    public int id;
    public int id2;
    public AudioClip sound;
    public AudioClip sound2;

    AudioSource audioSource;
    void Start(){
        audioSource=GetComponent<AudioSource>();

        if(playLimitForThis){
            id=Random.Range(0,thisSoundLimit);
            id2=Random.Range(thisSoundLimit+1,sounds.Length);
        }else{
            id=Random.Range(0,sounds.Length);
        }
        sound=sounds[id];
        sound2=sounds[id2];
        if(forThisObject)audioSource.clip=sound;
        if(playAtStart){audioSource.Play();}
    }
}
