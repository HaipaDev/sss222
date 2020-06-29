using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoShip : MonoBehaviour{
    [SerializeField] public int repMinus=2;
    [SerializeField] float speed=2;
    public bool tagged;
    public bool visited;
    void Start(){
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    void Update(){
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Shop.shopOpen=true;
            visited=true;
            GetComponent<Collider2D>().enabled=false;
        }if(other.CompareTag("PlayerWeapons")){
            Destroy(other.gameObject);
            if(tagged==false){Shop.instance.RepMinus(repMinus);tagged=true;AudioManager.instance.Play("CargoHit");gameObject.transform.GetChild(0).gameObject.SetActive(true);}
        }
    }
    private void OnDestroy() {
        if(visited!=true){
            Shop.instance.purchasedNotTimes++;
        }
    }
}