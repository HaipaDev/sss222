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
        transform.GetChild(0).gameObject.SetActive(false);
    }
    void Update(){
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")&&visited==false){
            Shop.shopOpen=true;
            visited=true;
            tagged=true;
            GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled=false;
        }if(other.CompareTag("PlayerWeapons")){
            Destroy(other.gameObject);
            if(tagged==false){Shop.instance.RepMinus(repMinus);tagged=true;AudioManager.instance.Play("CargoHit");gameObject.transform.GetChild(0).gameObject.SetActive(true);}
        }if(other.GetComponent<Shredder>()!=null){Destroy(gameObject);}
    }
    private void OnDestroy() {
        if(visited==false&&Shop.instance.subbed==false){
            Shop.instance.purchasedNotTimes++;
            Shop.instance.subbed=true;
        }
    }
}