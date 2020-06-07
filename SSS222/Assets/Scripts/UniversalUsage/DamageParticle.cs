using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParticle : MonoBehaviour{
    [SerializeField] bool enemies=true;
    [SerializeField] float dmgEnemy=16;
    [SerializeField] bool player=true;
    [SerializeField] float dmgPlayer=16;
    void Start(){
        Destroy(gameObject,0.07f);
    }
    void Update(){
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")&&other.gameObject.GetComponent<VortexWheel>()==null&&enemies==true){
            var enemy=other.GetComponent<Enemy>();
            if(enemy.health<=dmgEnemy)enemy.givePts=false;
            enemy.health-=dmgEnemy;
            GetComponent<Collider2D>().enabled=false;
        }else if(other.gameObject.CompareTag("Player")&&player==true){
            var player=other.GetComponent<Player>();
            player.health-=dmgPlayer;
            player.damaged = true;
            AudioManager.instance.Play("ShipHit");
            if(FindObjectOfType<GameSession>().dmgPopups==true){
                GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(other.GetComponent<PlayerCollider>().dmgPopupPrefab,transform.position);
                dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;
                dmgpopup.transform.localScale=new Vector2(2,2);
                dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=dmgPlayer.ToString();
            }
            player.DMGPopUpHud(dmgPlayer);
            GetComponent<Collider2D>().enabled=false;
        }
    }
}
