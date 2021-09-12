using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoShip : MonoBehaviour{
    dir cargoDirSpawn=dir.up;
    [SerializeField] public int repMinus=2;
    [SerializeField] float speed=2;
    public bool tagged;
    public bool visited;
    void Start(){
        //GetComponent<Rigidbody2D>().velocity=new Vector2(0,-speed);
        transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")&&visited==false){
            Shop.shopOpen=true;
            visited=true;
            tagged=true;
            //GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
        }if(other.CompareTag("PlayerWeapons")&&other.GetComponent<HealthLeech>()==null){
            Destroy(other.gameObject);
            if(tagged==false){Shop.instance.RepChange(repMinus,false);tagged=true;AudioManager.instance.Play("CargoHit");transform.GetChild(0).gameObject.SetActive(true);}
        }else if(other.GetComponent<HealthLeech>()!=null){
            if(tagged==false){Shop.instance.reputation+=3;tagged=true;}
        }
        if(other.GetComponent<Shredder>()!=null){Destroy(gameObject);}
    }
    private void OnDestroy() {
        if(visited==false&&Shop.instance.subbed==false){
            Shop.instance.purchasedNotTimes++;
            Shop.instance.subbed=true;
        }
    }
    public void SetCargoSpawnDir(dir dir){StartCoroutine(SetCargoSpawnDirI(dir));}
    IEnumerator SetCargoSpawnDirI(dir dir){
        cargoDirSpawn=dir;
        float hspeed=0;
        float vspeed=0;
        float zRot=0;
        //GameObject pt=GameAssets.instance.GetVFX("BFlameUp");
        //float ptRot=180;
        //float ptxx=0.3f;
        //float ptyy=0.5f;
        switch(cargoDirSpawn){
            case dir.up:vspeed=-speed;zRot=0;break;
            case dir.down:vspeed=speed;zRot=180;break;
            case dir.left:hspeed=speed;zRot=90;break;
            case dir.right:hspeed=-speed;zRot=-90;break;
        }
        //foreach(BackflameEffect co in GetComponents<BackflameEffect>()){co.part=pt;}//co.BFlame.transform.eulerAngles=new Vector3(0,0,ptRot);}//co.xx=ptxx;co.yy=ptyy;}
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().velocity=new Vector2(hspeed,vspeed);
        transform.eulerAngles=new Vector3(transform.rotation.x,transform.rotation.y,zRot);
    }
}