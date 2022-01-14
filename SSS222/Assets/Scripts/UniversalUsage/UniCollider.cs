using System;
//using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniCollider : MonoBehaviour{
    public static List<colliTypes> colliTypesForEn=new List<colliTypes>{colliTypes.player,colliTypes.playerWeapons,colliTypes.world};
    public static List<colliTypes> colliTypesForPl=new List<colliTypes>{colliTypes.enemies,colliTypes.enemyWeapons,colliTypes.world,colliTypes.zone};

    public static float TriggerCollision(Collider2D other, Transform transform, List<colliTypes> collis, bool triggerStay=false){
        float dmg=0;
        if(other.GetComponent<Player>()==null&&other.GetComponent<Tag_Collectible>()==null&&other.GetComponent<Shredder>()==null){
            DamageValues dmgVal;
            dmgVal=GetDmgVal(other.gameObject.name);
            if(dmgVal==null){dmgVal=GetDmgValAbs(other.gameObject.name.Split('_')[0]);}
            if(dmgVal!=null){if(collis.Contains(dmgVal.colliType)){
                dmg=dmgVal.dmg;if(triggerStay){dmg=dmgVal.dmgPhase;}
                if(dmgVal.dmgBySize&&!dmgVal.dmgBySpeed){dmg*=((other.gameObject.transform.localScale.x+other.gameObject.transform.localScale.y)/2);}
                else if(!dmgVal.dmgBySize&&dmgVal.dmgBySpeed){dmg*=Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.magnitude);}
                else if(dmgVal.dmgBySize&&dmgVal.dmgBySpeed){dmg*=((other.gameObject.transform.localScale.x+other.gameObject.transform.localScale.y)/2)*Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.magnitude);}
                
                foreach(colliEvents co in dmgVal.colliEvents){
                    if(!String.IsNullOrEmpty(co.vfx)){if(GameAssets.instance.GetVFX(co.vfx)!=null)Instantiate(GameAssets.instance.GetVFX(co.vfx),(Vector2)transform.position+co.vfxPos,Quaternion.identity);}
                    if(co.dmgPlayer!=0){if(Player.instance!=null){Player.instance.Damage(co.dmgPlayer,co.dmgPlayerType);}}
                    if(!String.IsNullOrEmpty(co.assetMake)){if(GameAssets.instance.Get(co.assetMake)!=null)GameAssets.instance.Make(co.assetMake,(Vector2)transform.position+co.assetPos);}
                    if(co.healBeamPlayer!=0){
                        GameObject go=Instantiate(GameAssets.instance.Get("HealBeam"),(Vector2)transform.position+co.assetPos,Quaternion.identity);
                        HealBeam hb=go.GetComponent<HealBeam>();
                        if(co.healBeamPlayer>0){hb.value=co.healBeamPlayer;}
                        else if(co.healBeamPlayer==-1||co.healBeamPlayer==-7){hb.value=dmg;}
                        else if(co.healBeamPlayer==-2||co.healBeamPlayer==-8){hb.value=dmg/2;}
                        else if(co.healBeamPlayer==-3||co.healBeamPlayer==-9){hb.value=dmg/4;}
                        else if(co.healBeamPlayer==-4||co.healBeamPlayer==-10){hb.value=dmg/8;}
                        else if(co.healBeamPlayer==-5||co.healBeamPlayer==-11){hb.value=dmg/12;}
                        else if(co.healBeamPlayer==-6||co.healBeamPlayer==-12){hb.value=dmg/16;}
                        if(co.healBeamPlayer<=-7){hb.absorp=false;}
                    }
                }
                
                if(!dmgVal.phase){Destroy(other.gameObject,0.01f);}
                else{var dmgPhaseFreq=other.GetComponent<Tag_DmgPhaseFreq>();if(dmgPhaseFreq==null){dmgPhaseFreq=other.gameObject.AddComponent<Tag_DmgPhaseFreq>();}
                    dmgPhaseFreq.phaseFreqFirst=dmgVal.phaseFreqFirst;
                    dmgPhaseFreq.phaseFreq=dmgVal.phaseFreq;
                    dmgPhaseFreq.phaseCountLimit=dmgVal.phaseCountLimit;
                    dmgPhaseFreq.soundPhase=dmgVal.soundPhase;
                    if(dmgVal.soundPhase=="-"){dmgPhaseFreq.soundPhase=dmgVal.sound;}
                    else if(dmgVal.soundPhase=="."){dmgPhaseFreq.soundPhase="EnemyHit";}
                }
                if(!triggerStay){if(!String.IsNullOrEmpty(dmgVal.sound))AudioManager.instance.Play(dmgVal.sound);}
                //else{other.GetComponent<Tag_DmgPhaseFreq>().SetTimer();}//well that doesnt work
            }}
        }
        return dmg;
    }

    public static void DMG_VFX(int type,Collider2D other, Transform transform, float dmg, bool crit=false, int colorDef=0){
        int color=colorDef;float scale=1f;
        if(colorDef==0){color=ColorInt32.dmgColor;}

        if(crit){color=ColorInt32.orange;AudioManager.instance.Play("CritHit");}
    if(other.GetComponent<Player>()==null/*&&other.GetComponent<Tag_Collectible>()==null*/&&other.GetComponent<Shredder>()==null){
        //DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
        if(type==0){//Enemy - TriggerEnter
            MakeFlare();
            if(GameSession.instance.dmgPopups==true&&dmg>0){
                DamageValues dmgVal=UniCollider.GetDmgVal(other.gameObject.name);
                if(dmgVal!=null&&dmgVal.dispDmgCount){
                    if(transform.GetComponent<Enemy>()!=null){
                    transform.GetComponent<Enemy>().dmgCount+=dmg;
                    if(transform.GetComponent<Enemy>().dmgCounted==false)transform.GetComponent<Enemy>().DispDmgCount(other.transform.position);
                    }
                }else{
                    //default
                }
            }else if(GameSession.instance.dmgPopups==true&&dmg<0){
                color=ColorInt32.dmgHealColor;
            }
        }else if(type==1){//Enemy - TriggerStay
            MakeFlare();
            if(GameSession.instance.dmgPopups==true&&dmg>0){
                color=ColorInt32.dmgPhaseColor;
            }else if(GameSession.instance.dmgPopups==true&&dmg<0){
                color=ColorInt32.dmgHealColor;
            }
        }else if(type==2){//Player - TriggerEnter
            var player=Player.instance;
            if(GameSession.instance.dmgPopups==true&&dmg>0&&!player.gclover&&!player.dashing){color=ColorInt32.dmgPlayerHitColor;scale=2;}
            else if(dmg<0){color=ColorInt32.dmgPlayerHealColor;scale=1.5f;}
        }else if(type==3){//Player - TriggerStay
            var player=Player.instance;
            if(GameSession.instance.dmgPopups==true&&dmg>0&&!player.gclover&&!player.dashing){color=ColorInt32.dmgPlayerPhaseColor;scale=2;}
            else if(dmg<0){color=ColorInt32.dmgPlayerHealColor;scale=1.4f;}
        }else if(type==4){//Player - Absorp
            var player=Player.instance;
            dmg*=-1;color=ColorInt32.dmgHealColor;scale=1.4f;
        }else if(type==-1){//Default / Cargo
            if(GameSession.instance.dmgPopups==true&&dmg>0){color=ColorInt32.grey;scale=2;}
            else if(dmg<0){color=ColorInt32.dmgHealColor;scale=1.4f;}
        }

        if(dmg!=0)WorldCanvas.instance.DMGPopup(dmg,other.transform.position,ColorInt32.Int2Color(color),scale,crit);
        void MakeFlare(){
            GameObject flare=GameAssets.instance.VFX("FlareHit",new Vector2(other.transform.position.x,other.transform.position.y));
            Vector2 flareScale=Vector2.one;
            if(other.gameObject.GetComponent<SpriteRenderer>()!=null)flareScale=new Vector2(other.gameObject.GetComponent<SpriteRenderer>().bounds.size.x,other.gameObject.GetComponent<SpriteRenderer>().bounds.size.y)*3;flareScale*=other.transform.localScale;
            flare.transform.localScale=flareScale;
        }
    }}
    public static DamageValues GetDmgVal(string objName){
        DamageValues dmgVal=null;
        List<GObject> assets=new List<GObject>();
        foreach(GObject gobj in GameAssets.instance.objects){assets.Add(gobj);}
        foreach(GObject vfx in GameAssets.instance.vfx){assets.Add(vfx);}
        var asset=assets.Find(x=>objName.Contains(x.gobj.name));
        if(asset!=null)dmgVal=GameRules.instance.dmgValues.Find(x=>x.name==asset.name);
        if(dmgVal==null){dmgVal=GetDmgValAbs(objName.Split('_')[0]);}
        if(dmgVal!=null)return dmgVal;
        else Debug.LogWarning("DamageValues not defined for "+objName);return null;
    }
    public static DamageValues GetDmgValAbs(string dmgValName){
        DamageValues dmgVal=null;
        List<GObject> assets=new List<GObject>();
        dmgVal=GameRules.instance.dmgValues.Find(x=>x.name==dmgValName);
        if(dmgVal!=null)return dmgVal;
        else Debug.LogWarning("DamageValuesAbs not defined for "+dmgValName);return null;
    }
    public static int GetArmorPenetr(int value,int defense){
        int armorPenetr=value;
        if(value>0){}
        else if(value==-1){armorPenetr=defense;}
        else if(value==-2){armorPenetr=defense/2;}
        else if(value==-3){armorPenetr=defense/4;}
        else if(value==-4){armorPenetr=defense/8;}
        return armorPenetr;
    }
}

public enum colliTypes{player,playerWeapons,enemies,enemyWeapons,world,zone}
public enum dmgType{normal,silent,flame,shadow,decay,electr,heal,healSilent}