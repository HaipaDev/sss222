using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParticle : MonoBehaviour{
    [SerializeField] bool enemies=true;
    [SerializeField] float dmgEnemy=16;
    [SerializeField] bool player=true;
    [SerializeField] dmgType dmgType=dmgType.normal;
    [SerializeField] float dmgPlayer=16;
    void Start(){
        //Destroy(gameObject,0.07f);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")&&other.gameObject.GetComponent<VortexWheel>()==null&&enemies==true){
            var enemy=other.GetComponent<Enemy>();
            if(enemy.health<=dmgEnemy)enemy.givePts=false;
            enemy.health-=dmgEnemy;
            GetComponent<Collider2D>().enabled=false;
            Destroy(gameObject);
        }else if(other.gameObject.CompareTag("Player")&&player==true){
            var player=other.GetComponent<Player>();
            player.Damage(dmgPlayer,dmgType);
            if(FindObjectOfType<GameSession>().dmgPopups==true){
                GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(GameAssets.instance.GetVFX("DMGPopup"),transform.position);
                dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;
                dmgpopup.transform.localScale=new Vector2(2,2);
                dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=dmgPlayer.ToString();
            }
            //player.DMGPopUpHUD(dmgPlayer);
            GetComponent<Collider2D>().enabled=false;
            Destroy(gameObject);
        }
    }
}
