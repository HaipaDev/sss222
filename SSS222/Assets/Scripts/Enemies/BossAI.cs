using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BossAI : MonoBehaviour{
    public List<BossPhaseInfo> phasesInfo;
    [ReadOnly]public int phase;
    Enemy en;
    void Start(){
        phase=-1;
        en=GetComponent<Enemy>();
        en.size=Vector2.zero;
        var b=GameRules.instance.bossInfo;
        phasesInfo=b.phasesInfo;
        for(int i=0;i<phasesInfo.Count;i++){phasesInfo[i].name="Phase "+(i+1);}

        en.name=b.name;
        en.type=b.type;
        if(b.scaleUpOnSpawn){en.health=1f;}else{en.health=b.healthStart;}en.healthMax=b.healthMax;
        en.defense=phasesInfo[0].defense;
        en.sprMatProps=phasesInfo[0].sprMatProps;
        en.shooting=false;

        en.randomizeWaveDeath=false;
        en.flyOff=false;
        en.killOnDash=false;
        en.destroyOut=false;

        en.xpAmnt=b.xpAmnt;
        en.xpChance=b.xpChance;
        en.accumulateXp=false;
        en.drops=b.drops;

        ChangePhase(0);
        //Spawn FX
        /*AudioManager.instance.Play(phasesInfo[0].audioAsset);
        AssetsManager.instance.VFX(phasesInfo[0].vfxAsset,transform.position);
        Shake.instance.CamShake(phasesInfo[0].camShakeStrength,phasesInfo[0].camShakeSpeed);*/
        SetBossSpecificVars();
    }
    void Update(){
        if(!GameManager.GlobalTimeIsPaused){
            if(phase==-1&&GameRules.instance.bossInfo.scaleUpOnSpawn){
                var scaleUpSpeed=phasesInfo[0].delay/10f*Time.deltaTime;
                if(Vector2.Distance(en.size,phasesInfo[0].size)>scaleUpSpeed){
                    en.spr=phasesInfo[0].anims[0].spr;en.size+=new Vector2(scaleUpSpeed,scaleUpSpeed);
                    if(en.health<=GameRules.instance.bossInfo.healthStart){en.health+=en.healthMax*scaleUpSpeed;}
                }
            }
            if(phase>=0){
                en.defense=phasesInfo[phase].defense;
                en.size=phasesInfo[phase].size;
                en.sprMatProps=phasesInfo[phase].sprMatProps;

                en.spr=phasesInfo[phase].anims[0].spr;//Animate later
            }

            if(_isMOL()){MoonOfLunacyAI();}
        }
    }
    public void Die(){StartCoroutine(DieI());}
    IEnumerator DieI(){
        phase=-1;
        en.defense=-1;
        GetComponent<PointPathing>().enabled=false;
        Jukebox.instance.PauseBossMusic();
        AudioManager.instance.Play(GameRules.instance.bossInfo.preDeathAudio);
        AssetsManager.instance.VFX(GameRules.instance.bossInfo.preDeathVFX,transform.position,3f);
        yield return new WaitForSeconds(GameRules.instance.bossInfo.deathLength);
        AssetsManager.instance.VFX(GameRules.instance.bossInfo.deathVFX,transform.position,3f);
        Shake.instance.CamShake(GameRules.instance.bossInfo.deathShakeStrength,GameRules.instance.bossInfo.deathShakeSpeed);
        SaveSerial.instance.advD.defeatedBosses.Add(en.name);
        StatsAchievsManager.instance.BossDefeated(en.name);
        UpgradeMenu.instance.OpenZoneMapPostBoss();
        Destroy(gameObject);
    }
    /*Coroutine anim;int iAnim=0;Sprite animSpr;
    IEnumerator AnimateBoss(SimpleAnim anim){Sprite spr;
        if(anim.delay>0){yield return new WaitForSeconds(anim.delay);}
        else{yield return new WaitForSeconds(anim[iAnim].delay);}
        spr=anim.animVals[iAnim].spr;
        if(iAnim==skin.animVals.Count-1)iAnim=0;
        if(iAnim<skin.animVals.Count)iAnim++;
        animSpr=spr;
        if(skinName==skin.name)anim=StartCoroutine(AnimateBoss(skin));
        else{if(anim!=null)StopCoroutine(anim);anim=null;iAnim=0;}
    }*/
    
    bool CheckName(string name){return GetComponent<Enemy>().name.Contains(name);}
    void SetBossSpecificVars(){
        if(_isMOL()){
            if(phase==0){
                _molP1_attack1Timer=_molP1_attack1Time/2;
                _molP1_attack1Count=0;
                _molP1_attack1CountFor2=Mathf.RoundToInt(Random.Range((float)_molP1_attack1CountFor2Range.x,(float)_molP1_attack1CountFor2Range.y));
                GameRules.instance.cometSettings.lunarCometChance=1;
            }else if(phase==1){
                _molP2_attack1Timer=_molP2_attack1Time/2;
                _molP2_attack1Count=0;
                _molP2_attack1CountFor2=Mathf.RoundToInt(Random.Range((float)_molP2_attack1CountFor2Range.x,(float)_molP2_attack1CountFor2Range.y));
            }
        }
    }
bool _preAttackVFXSpawned;
#region ///Moon of Lunacy
bool _isMOL(){return CheckName("Moon of Lunacy");}
[Header("Phase1")]
    [ShowIf("@this._isMOL()")][SerializeField]float _molP1_attack1Time=4.5f;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP1_attack1Timer=-4;
    [ShowIf("@this._isMOL()")][SerializeField]int _molP1_attack1Count;
    [ShowIf("@this._isMOL()")][SerializeField]int _molP1_attack1CountFor2=3;
    [ShowIf("@this._isMOL()")][SerializeField]Vector2 _molP1_attack1CountFor2Range=new Vector2(3,4);
    [ShowIf("@this._isMOL()")][SerializeField]float _molP1_attack2Time=1.5f;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP1_attack2Timer=-4;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP1_attack2SubTime1=0.5f;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP1_attack2SubTime1Limit=1.5f;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP1_attack2DistanceForForce=2.7f;
[Header("Phase2")]
    [ShowIf("@this._isMOL()")][SerializeField]float _molP2_attack1Time=3f;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP2_attack1Timer=-4;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP2_attack1Count;
    [ShowIf("@this._isMOL()")][SerializeField]int _molP2_attack1CountFor2=3;
    [ShowIf("@this._isMOL()")][SerializeField]Vector2 _molP2_attack1CountFor2Range=new Vector2(2,4);
    [ShowIf("@this._isMOL()")][SerializeField]float _molP2_attack2Time=2.5f;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP2_attack2Timer=-4;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP2_attack2SubTime1=0.25f;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP2_attack2SubTime1Limit=2f;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP2_attack2FollowSpeed=2.5f;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP2_attack2Duration=5;
    [ShowIf("@this._isMOL()")][SerializeField]float _molP2_attack2DurationTimer=-4;
    [ShowIf("@this._isMOL()")][ReadOnly][SerializeField]float _molDistToPlayer;
    void MoonOfLunacyAI(){
        if(phase==0){
            if(_molP1_attack1Timer>0){_molP1_attack1Timer-=Time.deltaTime;if(_molP1_attack1Timer<=1&&!_preAttackVFXSpawned){AssetsManager.instance.VFX("MoonOfLunacy-P1_Attack1",transform.position,3f);AudioManager.instance.Play("MoonOfLunacy-Rumble");_preAttackVFXSpawned=true;}}
            else if(_molP1_attack1Timer!=-4){//Attack 1
                FindObjectOfType<Waves>().currentWave=AssetsManager.instance.GetWaveConfig("Comet Barrage");
                FindObjectOfType<Waves>().SpawnAllEnemiesInCurrentWave();
                _molP1_attack1Count++;
                _molP1_attack1Timer=_molP1_attack1Time;
                FindObjectOfType<Waves>().currentWave=null;
                _preAttackVFXSpawned=false;
            }
            if(_molP1_attack1Count>=_molP1_attack1CountFor2){//Countdown to Attack 2
                _molP1_attack1Timer=-4;
                _molP1_attack1Count=0;
                _molP1_attack1CountFor2=Mathf.RoundToInt(Random.Range((float)_molP1_attack1CountFor2Range.x,(float)_molP1_attack1CountFor2Range.y));
                _molP1_attack2Timer=_molP1_attack2Time;
            }
            if(_molP1_attack2Timer>0){_molP1_attack2Timer-=Time.deltaTime;if(_molP1_attack2Timer<=1&&!_preAttackVFXSpawned){AssetsManager.instance.VFX("MoonOfLunacy-P1_Attack2",transform.position,3f);AudioManager.instance.Play("MoonOfLunacy-Growl");_preAttackVFXSpawned=true;}}
            else if(_molP1_attack2Timer!=-4){//Attack 2
                AssetsManager.instance.Make("LunarPulse",transform.position);
                if(_molP1_attack1Time>_molP1_attack2SubTime1Limit)_molP1_attack1Time-=_molP1_attack2SubTime1;
                _molP1_attack1Timer=_molP1_attack1Time;
                _molP1_attack2Timer=-4;
                _preAttackVFXSpawned=false;
            }
            if(Player.instance!=null){_molDistToPlayer=Vector2.Distance(transform.position,Player.instance.transform.position);if(_molDistToPlayer<=_molP1_attack2DistanceForForce&&_molP1_attack2Timer==-4){_preAttackVFXSpawned=false;_molP1_attack2Timer=1f;_molP1_attack1Timer=-4;}}

            if(en.health<=en.healthMax/2){en.health=Mathf.FloorToInt(en.healthMax/2);ChangePhase(1);}
        }else if(phase==1){
            if(_molP2_attack1Timer>0){_molP2_attack1Timer-=Time.deltaTime;}//if(_molP2_attack1Timer<=1&&!_preAttackVFXSpawned){AssetsManager.instance.VFX("MoonOfLunacy-P2_Attack1",transform.position,3f);_preAttackVFXSpawned=true;}}
            else if(_molP2_attack1Timer!=-4){//Attack 1
                var xRange=new Vector2(-2.5f,2.5f);
                var yRange=new Vector2(-6f,6f);
                var objPos=new Vector2(Random.Range(xRange.x,xRange.y),Random.Range(yRange.x,yRange.y));
                AssetsManager.instance.Make("LunarSickle",objPos);
                _molP2_attack1Count++;
                _molP2_attack1Timer=_molP2_attack1Time;
                _preAttackVFXSpawned=false;
            }
            if(_molP2_attack1Count>=_molP2_attack1CountFor2){//Countdown to Attack 2
                _molP2_attack1Timer=-4;
                _molP2_attack1Count=0;
                _molP2_attack1CountFor2=Mathf.RoundToInt(Random.Range((float)_molP2_attack1CountFor2Range.x,(float)_molP2_attack1CountFor2Range.y));
                _molP2_attack2Timer=_molP2_attack2Time;
            }
            if(_molP2_attack2Timer>0){_molP2_attack2Timer-=Time.deltaTime;if(_molP2_attack2Timer<=1&&!_preAttackVFXSpawned){AssetsManager.instance.VFX("MoonOfLunacy-P2_Attack2",transform.position,3f);AudioManager.instance.Play("MoonOfLunacy-Laugh");_preAttackVFXSpawned=true;}}
            else if(_molP2_attack2Timer!=-4){//Start Attack 2
                if(GetComponent<Follow>()==null){var f=gameObject.AddComponent<Follow>();
                    f.target=Player.instance.gameObject;f.followAfterOOR=true;f.dirYYUp=true;//f.rotateTowards=true;
                    f.speedFollow=_molP2_attack2FollowSpeed;f.distReq=60f;
                    f.hspeed=0;f.vspeed=0;
                }
                GetComponent<Follow>().enabled=true;
                GetComponent<PointPathing>().enabled=false;
                if(GetComponent<Glow>()==null){var g=gameObject.AddComponent<Glow>();
                    g.size=new Vector2(3f,3f);g.alignToDirection=true;g.color=new Color(70,250,200);
                }
                GetComponent<Glow>().enabled=true;
                en._dmgHeals=true;
                _molP2_attack2DurationTimer=_molP2_attack2Duration;
                _molP2_attack2Timer=-4;
                en.name="Moon of Lunacy - Spin";
                _preAttackVFXSpawned=false;
            }
            if(_molP2_attack2DurationTimer>0){_molP2_attack2DurationTimer-=Time.deltaTime;var rotSpeed=_molP2_attack2FollowSpeed*360*Time.deltaTime;AudioManager.instance.Play("Spin");transform.Rotate(new Vector3(0,0,rotSpeed));}
            else if(_molP2_attack2DurationTimer!=-4){//Stop Attack 2
                if(GetComponent<Follow>()!=null)GetComponent<Follow>().enabled=false;
                GetComponent<PointPathing>().enabled=true;
                GetComponent<Glow>().enabled=false;
                foreach(ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())Destroy(ps.gameObject);
                en._dmgHeals=false;
                if(_molP2_attack1Time>_molP2_attack2SubTime1Limit)_molP2_attack1Time-=_molP2_attack2SubTime1;
                _molP2_attack1Timer=_molP2_attack1Time;
                transform.rotation=Quaternion.identity;
                en.name="Moon of Lunacy";
                _molP2_attack2DurationTimer=-4;
            }
        }
        if(en.health<=0){
            _molP2_attack2DurationTimer=-4;
            if(GetComponent<Follow>()!=null)GetComponent<Follow>().enabled=false;
            GetComponent<PointPathing>().enabled=false;
            en.name="Moon of Lunacy";
            foreach(FadeOutBullet fob in FindObjectsOfType<FadeOutBullet>()){if(fob.gameObject.name.Contains("LunarSickle")){if(fob.timer>fob.timeStartFade)fob.timer=fob.timeStartFade;}}
            //foreach(GloomyScythe gs in FindObjectsOfType<GloomyScythe>()){if(gs.gameObject.name.Contains("LunarSickle")){Destroy(gs.gameObject);}}
        }
    }
#endregion


    IEnumerator _phaseCor;
    public void ChangePhase(int p){if(phasesInfo.Count>p){if(_phaseCor==null){_phaseCor=ChangePhaseI(p);StartCoroutine(_phaseCor);}}else{Debug.LogError("Cant change to phase: "+p);}}
    public IEnumerator ChangePhaseI(int p){
        phase=-1;
        en.defense=-1;
        GetComponent<PointPathing>().enabled=false;
        if(GetComponent<Follow>()!=null)GetComponent<Follow>().enabled=false;
        if(!System.String.IsNullOrEmpty(phasesInfo[p].audioOnChangeStartAsset))AudioManager.instance.Play(phasesInfo[p].audioOnChangeStartAsset);
        if(!System.String.IsNullOrEmpty(phasesInfo[p].vfxOnChangeStartAsset))AssetsManager.instance.VFX(phasesInfo[p].vfxOnChangeStartAsset,transform.position,3f);
        if(Jukebox.instance!=null&&GameRules.instance.bossInfo.pauseOstOnPhaseChange)Jukebox.instance.PauseBossMusicFor(phasesInfo[p].delay);
        yield return new WaitForSeconds(phasesInfo[p].delay);
        if(p==0){
            if(Jukebox.instance==null){Instantiate(CoreSetup.instance.GetJukeboxPrefab());}
            if(Jukebox.instance!=null)Jukebox.instance.SetBossMusic(GameRules.instance.bossInfo.ost);
            if(SaveSerial.instance.settingsData.bossVolumeTurnUp){GameManager.instance._preBossMusicVolume=SaveSerial.instance.settingsData.musicVolume;SaveSerial.instance.settingsData.musicVolume=1f;}
            en.health=GameRules.instance.bossInfo.healthStart;
            FindObjectOfType<BossTitleDisplay>().TurnOnBossDisplay();
        }
        if(!System.String.IsNullOrEmpty(phasesInfo[p].audioOnChangeEndAsset))AudioManager.instance.Play(phasesInfo[p].audioOnChangeEndAsset);
        if(!System.String.IsNullOrEmpty(phasesInfo[p].vfxOnChangeEndAsset))AssetsManager.instance.VFX(phasesInfo[p].vfxOnChangeEndAsset,transform.position,3f);
        Shake.instance.CamShake(phasesInfo[p].camShakeStrength,phasesInfo[p].camShakeSpeed);
        GetComponent<PointPathing>().enabled=true;
        phase=p;
        SetBossSpecificVars();
        _phaseCor=null;
    }
}