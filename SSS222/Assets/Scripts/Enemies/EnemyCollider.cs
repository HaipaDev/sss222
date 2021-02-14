using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour{
    public float dmgTimer;
    void OnTriggerEnter2D(Collider2D other){
        //if(FindObjectOfType<Player>().shadowRaycast[FindObjectOfType<Player>().shadowRaycast.FindIndex(FindObjectOfType<Player>().shadowRaycast.Count,(x) => x == this)]==this){Die();}
        if(!other.CompareTag(tag)&&!other.CompareTag("EnemyBullet")&&other.GetComponent<Tag_OutsideZone>()==null){
            DamageDealer damageDealer=other.GetComponent<DamageDealer>();
            DamageValues damageValues=DamageValues.instance;
            if(gameObject!=null&&other.gameObject!=null)if(!damageDealer||!damageValues){/*Debug.LogWarning("No DamageDealer component or DamageValues instance")*/;return;}
            var dmg=damageValues.GetDmg();

            if(other.GetComponent<Player>()!=null){if(other.GetComponent<Player>().dashing==true){GetComponent<Enemy>().Die();}}

            if(other.gameObject.name.Contains(GameAssets.instance.Get("Laser").name)){dmg=damageValues.GetDmgLaser(); Destroy(other.gameObject); AudioManager.instance.Play("EnemyHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MLaser").name)){ 
                AudioManager.instance.Play("MLaserHit");
                /*var mlaserHitSound=other.GetComponent<RandomSound>().sound;
                if(other.GetComponent<RandomSound>().playLimitForThis==true){mlaserHitSound=other.GetComponent<RandomSound>().sound2;}
                AudioSource.PlayClipAtPoint(mlaserHitSound, new Vector2(transform.position.x, transform.position.y));*/
                dmg=damageValues.GetDmgMiniLaser(); Destroy(other.gameObject);
            }
            if (other.gameObject.name.Contains(GameAssets.instance.Get("HRocket").name)){dmg=damageValues.GetDmgHRocket(); Destroy(other.gameObject); AudioManager.instance.Play("HRocketHit");
                GameAssets.instance.VFX("ExplosionSmall", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            }
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LSaber").name)){dmg=(float)System.Math.Round(damageValues.GetDmgLSaber()*9f,2); AudioManager.instance.Play("LSaberHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClaws").name)){ dmg=(float)System.Math.Round(damageValues.GetDmgLSaber()/3,2); AudioManager.instance.Play("LClawsHit"); if(FindObjectOfType<Player>()!=null)FindObjectOfType<Player>().energy-=1f;}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClawsVFX").name)){ dmg=damageValues.GetDmgLClaws(); AudioManager.instance.Play("LClawsHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("ShadowBt").name)){dmg=damageValues.GetDmgShadowBT(); FindObjectOfType<Player>().Damage(1,dmgType.shadow);}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("CBullet").name)){dmg=damageValues.GetDmgCBullet(); AudioManager.instance.Play("CStreamHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("QRocket").name)){dmg=damageValues.GetDmgQRocket(); Destroy(other.gameObject); AudioManager.instance.Play("QRocketHit");
                GameAssets.instance.VFX("ExplosionSmall", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            }
            if(other.gameObject.name.Contains(GameAssets.instance.Get("PRocket").name)){dmg=damageValues.GetDmgPRocket(); /*AudioSource.PlayClipAtPoint(hrocketHitSFX, new Vector2(transform.position.x, transform.position.y));*/}
                //var explosionSmall=Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            if(other.gameObject.name.Contains(GameAssets.instance.Get("PLaser").name)){dmg=other.GetComponent<DamageOverDist>().dmg; Destroy(other.gameObject); AudioManager.instance.Play("PLaserHit");//AudioSource.PlayClipAtPoint(hrocketHitSFX, new Vector2(transform.position.x, transform.position.y));
                //var explosionSmall=Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Plasma").name)){dmg=damageValues.GetDmgPRocketExpl(); GetComponent<Rigidbody2D>().velocity=Vector2.up*6f; GetComponent<Enemy>().yeeted=true;}// AudioSource.PlayClipAtPoint(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MPulse").name)){dmg=damageValues.GetDmgMPulse(); AudioManager.instance.Play("MLaserHit");}

            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameDMG").name)){dmg=damageValues.GetDmgFlame();}

            if(GetComponent<VortexWheel>()!=null){
                if(!other.gameObject.name.Contains(GameAssets.instance.Get("HRocket").name)&&!other.gameObject.name.Contains(GameAssets.instance.Get("QRocket").name)){
                    dmg/=3;
                }
            }
            if(FindObjectOfType<Player>()!=null)dmg*=FindObjectOfType<Player>().dmgMulti;
            GetComponent<Enemy>().health-=dmg;
            //AudioSource.PlayClipAtPoint(enemyHitSFX, new Vector2(transform.position.x, transform.position.y));
            GameAssets.instance.VFX("FlareHit", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            if(GameSession.instance.dmgPopups==true&&dmg!=0){
                if(!other.gameObject.name.Contains(GameAssets.instance.Get("PLaser").name)){
                    GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(GameAssets.instance.GetVFX("DMGPopup"),other.transform.position);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg,1).ToString();
                }else{
                    GetComponent<Enemy>().dmgCount+=dmg;
                    if(GetComponent<Enemy>().dmgCounted==false)GetComponent<Enemy>().DispDmgCount(other.transform.position);
                }
            }
        }else if(other.CompareTag(tag)){
            if(other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)||other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)){GetComponent<Enemy>().givePts=false; GetComponent<Enemy>().health=-1; GetComponent<Enemy>().Die();}
        }
    }
    void OnTriggerStay2D(Collider2D other){
    if(!other.CompareTag(tag)&&other.GetComponent<Tag_OutsideZone>()==null){
    if(dmgTimer<=0){
            DamageDealer damageDealer=other.GetComponent<DamageDealer>();
            DamageValues damageValues=DamageValues.instance;
            if(gameObject!=null&&other.gameObject!=null)if(!damageDealer||!damageValues){/*Debug.LogWarning("No DamageDealer component or DamageValues instance");*/return;}
            float dmg=damageValues.GetDmg();
            
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Phaser").name)){dmg=damageValues.GetDmgPhaser(); AudioManager.instance.Play("PhaserHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LSaber").name)){dmg=damageValues.GetDmgLSaber(); AudioManager.instance.Play("EnemyHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("Plasma").name)){dmg=damageValues.GetDmgPRocketExpl();}//AudioSource.PlayClipAtPoint(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("CBullet").name)){dmg=damageValues.GetDmgCBullet(); AudioManager.instance.Play("CStreamHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("LClaws").name)){ dmg=(float)System.Math.Round(damageValues.GetDmgLSaber()/3,2); FindObjectOfType<Player>().energy-=0.1f;}//AudioSource.PlayClipAtPoint(lclawsHitSFX, new Vector2(transform.position.x, transform.position.y));}
            if(other.gameObject.name.Contains(GameAssets.instance.Get("MPulse").name)){dmg=0; AudioManager.instance.Play("PRocketHit");}
            if(other.gameObject.name.Contains(GameAssets.instance.GetVFX("BFlameDMG").name)){dmg=damageValues.GetDmgFlame();}

            if(FindObjectOfType<Player>()!=null)dmg*=FindObjectOfType<Player>().dmgMulti;
            GetComponent<Enemy>().health-=dmg;
            //Destroy(other.gameObject, 0.05f);

            //AudioSource.PlayClipAtPoint(enemyHitSFX, new Vector2(transform.position.x, transform.position.y));
            GameAssets.instance.VFX("FlareHit", new Vector2(transform.position.x,transform.position.y-0.5f),0.3f);
            if(GameSession.instance.dmgPopups==true&&dmg!=0){
                GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(GameAssets.instance.GetVFX("DMGPopup"),transform.position);
                dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg,2).ToString();
            }
            if(other.GetComponent<Tag_DmgPhaseFreq>()!=null)dmgTimer=other.GetComponent<Tag_DmgPhaseFreq>().dmgFreq;
    }else{dmgTimer-=Time.deltaTime;}
    }
    }
}