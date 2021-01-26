using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptersSpawner : MonoBehaviour{
    public List<DisrupterConfig> disruptersList;
    /*[HeaderAttribute("Mecha Leech")]
    public bool spawnLeech=true;
    [SerializeField] WaveConfig cfgLeech;
    [SerializeField] float mSTimeSpawnsLeech = 55f;
    [SerializeField] float mETimeSpawnsLeech = 80f;
    public float timeSpawnsLeech = 0f;
    [HeaderAttribute("Homing Laser")]
    public bool spawnHlaser=true;
    [SerializeField] WaveConfig cfgHlaser;
    [SerializeField] float mSTimeSpawnsHlaser = 30f;
    [SerializeField] float mETimeSpawnsHlaser = 60f;
    public float timeSpawnsHlaser = 0f;
    [HeaderAttribute("Goblin Thief")]
    public bool spawnGoblin=true;
    [SerializeField] WaveConfig cfgGoblin;
    [SerializeField] int mPowerupsGoblin=2;
    [SerializeField] public int powerupsGoblin;
    [SerializeField] float mSTimeSpawnsGoblin = 40f;
    [SerializeField] float mETimeSpawnsGoblin = 50f;
    public float timeSpawnsGoblin = 0f;
    [HeaderAttribute("Healing Drone")]
    public bool spawnHealDrone=true;
    [SerializeField] WaveConfig cfgHealDrone;
    [SerializeField] int mEnemiesCountHealDrone = 50;
    public int EnemiesCountHealDrone = 0;
    [SerializeField] float mSTimeSpawnsHealDrone = 40f;
    [SerializeField] float mETimeSpawnsHealDrone = 50f;
    public float timeSpawnsHealDrone = 0f;
    [HeaderAttribute("Vortex Wheel")]
    public bool spawnVortexWheel=true;
    [SerializeField] WaveConfig cfgVortexWheel;
    [SerializeField] float mEnergyCountVortexWheel = 200;
    public float EnergyCountVortexWheel = 0;
    [SerializeField] float mSTimeSpawnsVortexWheel = 40f;
    [SerializeField] float mETimeSpawnsVortexWheel = 50f;
    public float timeSpawnsVortexWheel = 0f;
    [HeaderAttribute("Glare Devil")]
    public bool spawnGlareDevil=true;
    [SerializeField] WaveConfig cfgGlareDevil;
    [SerializeField] float mEnergyCountGlareDevil = 20;
    public float EnergyCountGlareDevil = 0;
    [SerializeField] float mSTimeSpawnsGlareDevil = 40f;
    [SerializeField] float mETimeSpawnsGlareDevil = 50f;
    public float timeSpawnsGlareDevil = 0f;*/
    private void Awake() {
        StartCoroutine(SetValues());
    }
    public IEnumerator SetValues(){
        //Set values
        yield return new WaitForSecondsRealtime(0.1f);
        var i=GameRules.instance;
        if(i!=null){
            disruptersList=i.disrupterList;
            /*spawnLeech=i.spawnLeech;
            mSTimeSpawnsLeech=i.mSTimeSpawnsLeech;
            mETimeSpawnsLeech=i.mETimeSpawnsLeech;
            spawnHlaser=i.spawnHlaser;
            mSTimeSpawnsHlaser=i.mSTimeSpawnsHlaser;
            mETimeSpawnsHlaser=i.mETimeSpawnsHlaser;
            spawnGoblin=i.spawnGoblin;
            mPowerupsGoblin=i.mPowerupsGoblin;
            mSTimeSpawnsGoblin=i.mSTimeSpawnsGoblin;
            mETimeSpawnsGoblin=i.mETimeSpawnsGoblin;
            spawnHealDrone=i.spawnHealDrone;
            mEnemiesCountHealDrone=i.mEnemiesCountHealDrone;
            mSTimeSpawnsHealDrone=i.mSTimeSpawnsHealDrone;
            mETimeSpawnsHealDrone=i.mETimeSpawnsHealDrone;
            spawnVortexWheel=i.spawnVortexWheel;
            mEnergyCountVortexWheel=i.mEnergyCountVortexWheel;
            EnergyCountVortexWheel=i.EnergyCountVortexWheel;
            mSTimeSpawnsVortexWheel=i.mSTimeSpawnsVortexWheel;
            mETimeSpawnsVortexWheel=i.mETimeSpawnsVortexWheel;
            spawnGlareDevil=i.spawnGlareDevil;
            mEnergyCountGlareDevil=i.mEnergyCountGlareDevil;
            mSTimeSpawnsGlareDevil=i.mSTimeSpawnsGlareDevil;
            mETimeSpawnsGlareDevil=i.mETimeSpawnsGlareDevil;*/
        }
        yield return new WaitForSecondsRealtime(0.05f);
        var di=0;
        List<DisrupterConfig> dcs=new List<DisrupterConfig>();
        foreach(DisrupterConfig dc in disruptersList){
            Debug.Log(dc.name+di);
            var da=Instantiate(dc);
            dcs.Add(da);
            di++;
        }
        this.disruptersList=dcs;
        yield return new WaitForSecondsRealtime(0.01f);
        foreach(DisrupterConfig dc in disruptersList){var d=dc.spawnProps;if(d.timeEnabled){RestartTime(d);}}
    }
    IEnumerator Start(){
        /*if(spawnLeech==true)timeSpawnsLeech = Random.Range(mSTimeSpawnsLeech,mETimeSpawnsLeech);
        if(spawnHlaser==true)timeSpawnsHlaser = Random.Range(mSTimeSpawnsHlaser, mETimeSpawnsHlaser);
        if(spawnGoblin==true)timeSpawnsGoblin = Random.Range(mSTimeSpawnsGoblin, mETimeSpawnsGoblin);
        if(spawnHealDrone==true)if(mEnemiesCountHealDrone==-1)timeSpawnsHealDrone = Random.Range(mSTimeSpawnsHealDrone, mETimeSpawnsHealDrone);
        if(spawnVortexWheel==true)if(EnergyCountVortexWheel==-1)timeSpawnsVortexWheel = Random.Range(mSTimeSpawnsVortexWheel, mETimeSpawnsVortexWheel);
        if(spawnGlareDevil==true)if(EnergyCountGlareDevil==-1)timeSpawnsGlareDevil = Random.Range(mSTimeSpawnsGlareDevil, mETimeSpawnsGlareDevil);*/
        do{yield return StartCoroutine(CheckSpawns());}while(true);
    }

    public IEnumerator CheckSpawns(){
        foreach(DisrupterConfig dc in disruptersList){
            var d=dc.spawnProps;
            var dt=dc.disrupterType;
            if(d.timeEnabled&&d.time<=0&&d.time>-4&&!d.bothNeeded){yield return StartCoroutine(SpawnWave(dc));}
            else if((d.secondEnabled&&!d.bothNeeded)||(d.secondEnabled&&d.bothNeeded&&d.timeEnabled&&d.time<=0&&d.time>-4)){
                if(dt==disrupterType.energy||dt==disrupterType.missed){
                    var ds=(DisrupterConfig.spawnEnergy)d;
                    if(ds.energy>=ds.energyNeeded){ds.energy=0;yield return StartCoroutine(SpawnWave(dc));}
                }
                else if(dt==disrupterType.pwrups){
                    var ds=(DisrupterConfig.spawnPwrups)d;
                    if(ds.pwrups>=ds.pwrupsNeeded){ds.pwrups=0;yield return StartCoroutine(SpawnWave(dc));}
                }
                else if(dt==disrupterType.kills){
                    var ds=(DisrupterConfig.spawnKills)d;
                    if(ds.kills>=ds.killsNeeded){ds.kills=0;yield return StartCoroutine(SpawnWave(dc));}
                }
                else if(dt==disrupterType.dmg){
                    var ds=(DisrupterConfig.spawnDmg)d;
                    if(ds.dmg>=ds.dmgNeeded){ds.dmg=0;yield return StartCoroutine(SpawnWave(dc));}
                }
                else if(dt==disrupterType.counts){
                    var ds=(DisrupterConfig.spawnCounts)d;
                    if(ds.counts>=ds.countsNeeded){ds.counts=0;yield return StartCoroutine(SpawnWave(dc));}
                }
            }
        }
        #region//Old
        /*if(spawnLeech==true){
            if(timeSpawnsLeech<=0&&timeSpawnsLeech>-4&&FindObjectOfType<Player>()!=null){
                //currentWave = cfgLeech;
                yield return StartCoroutine(SpawnWave(cfgLeech));
                timeSpawnsLeech=-4;
            }
            //if(progressiveWaves == true){if (waveIndex<waveConfigs.Count){ waveIndex++; } }
            //else{if(GameSession.instace.EVscore>=50){ //WaveRandomize();
            //waveIndex = Random.Range(0, waveConfigs.Count); GameSession.instace.EVscore = 0; } }
        }
        if(spawnHlaser==true){
            if(timeSpawnsHlaser<=0&&timeSpawnsHlaser>-4){
                yield return StartCoroutine(SpawnWave(cfgHlaser));
                timeSpawnsHlaser=-4;
            }
        }
        if(spawnGoblin==true){
            if(GameObject.FindGameObjectWithTag("Powerups")!=null){
                if(timeSpawnsGoblin<=0&&timeSpawnsGoblin>-4){
                    yield return StartCoroutine(SpawnWave(cfgGoblin));
                    powerupsGoblin=0;
                    timeSpawnsGoblin=-4;
                }
            }
        }
        if(spawnHealDrone==true){
            if((EnemiesCountHealDrone>=mEnemiesCountHealDrone)||(mEnemiesCountHealDrone==0&&timeSpawnsHealDrone<=0&&timeSpawnsHealDrone>-4)){
                if(FindObjectOfType<HealingDrone>()==null){
                    yield return StartCoroutine(SpawnWave(cfgHealDrone));
                    timeSpawnsHealDrone=-4;
                    EnemiesCountHealDrone=0;
                }
            }
        }if(spawnVortexWheel==true){
            if((EnergyCountVortexWheel>=mEnergyCountVortexWheel)||(mEnergyCountVortexWheel==0&&timeSpawnsVortexWheel<=0&&timeSpawnsVortexWheel>-4)){
                if(FindObjectOfType<VortexWheel>()==null){
                    yield return StartCoroutine(SpawnWave(cfgVortexWheel));
                    timeSpawnsVortexWheel=-4;
                    EnergyCountVortexWheel=0;
                }
            }
        }if(spawnGlareDevil==true){
            if((EnergyCountGlareDevil>=mEnergyCountGlareDevil&&timeSpawnsGlareDevil<=0&&timeSpawnsGlareDevil!=-4)||(mEnergyCountGlareDevil==0&&timeSpawnsGlareDevil<=0&&timeSpawnsGlareDevil>-4)){
                if(FindObjectOfType<GlareDevil>()==null){
                    yield return StartCoroutine(SpawnWave(cfgGlareDevil));
                    timeSpawnsGlareDevil=-4;
                    EnergyCountGlareDevil=0;
                }
            }
        }*/
        #endregion
    }
    void RestartTime(DisrupterConfig.disrupterSpawnProps d){d.time=Random.Range(d.timeS,d.timeE);}
    IEnumerator SpawnWave(DisrupterConfig dc){
        var d=dc.spawnProps;
        if(d.timeEnabled&&d.time<=0){RestartTime(d);}
        yield return StartCoroutine(FindObjectOfType<Waves>().SpawnAllEnemiesInWave(dc.waveConfig));
    }
    void Update(){
        if(Time.timeScale>0.0001f){foreach(DisrupterConfig dc in disruptersList){var d=dc.spawnProps;if(d.timeEnabled){if(d.time>0)d.time-=Time.deltaTime;}}}
        #region//Old
        /*Mathf.Clamp(EnergyCountVortexWheel,0,mEnergyCountVortexWheel);
        if(Time.timeScale>0.0001f){
            if(spawnLeech==true){
                if(timeSpawnsLeech>-0.01f){timeSpawnsLeech-=Time.deltaTime;}
                else if(timeSpawnsLeech==-4){timeSpawnsLeech=Random.Range(mSTimeSpawnsLeech, mETimeSpawnsLeech);}
            }
            if(spawnHlaser==true){
                if(timeSpawnsHlaser>-0.01f){timeSpawnsHlaser-=Time.deltaTime;}
                else if(timeSpawnsHlaser==-4){timeSpawnsHlaser=Random.Range(mSTimeSpawnsHlaser, mETimeSpawnsHlaser);}
            }
            if(spawnGoblin==true){
                if(powerupsGoblin>=mPowerupsGoblin&&timeSpawnsGoblin>=0){timeSpawnsGoblin-=Time.deltaTime;}
                else if(timeSpawnsGoblin==-4){timeSpawnsGoblin=Random.Range(mSTimeSpawnsGoblin, mETimeSpawnsGoblin);}
            }
            if(spawnHealDrone==true){//} && mEnemiesCountHealDrone!=0){
                if(timeSpawnsHealDrone>=0){timeSpawnsHealDrone-=Time.deltaTime;}
                else if(timeSpawnsHealDrone==-4){timeSpawnsHealDrone=Random.Range(mSTimeSpawnsHealDrone, mETimeSpawnsHealDrone);}
            }
            if(spawnVortexWheel==true){//} && mEnergyCountVortexWheel!=0){
                if(timeSpawnsVortexWheel>=0){timeSpawnsVortexWheel-=Time.deltaTime; }
                else if(timeSpawnsVortexWheel==-4){timeSpawnsVortexWheel=Random.Range(mSTimeSpawnsVortexWheel, mETimeSpawnsVortexWheel);}
            }
            if(spawnGlareDevil==true){//} && mEnergyCountGlareDevil!=0){
                if(timeSpawnsGlareDevil>=0){timeSpawnsGlareDevil-=Time.deltaTime;}
                else if(timeSpawnsGlareDevil==-4){timeSpawnsGlareDevil=Random.Range(mSTimeSpawnsGlareDevil, mETimeSpawnsGlareDevil);}
            }
        }*/
        #endregion
    }public void AddEnergy(float val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.energy){var ds=(DisrupterConfig.spawnEnergy)dc.spawnProps;ds.energy+=val;}}}
    public void AddMissed(float val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.missed){var ds=(DisrupterConfig.spawnEnergy)dc.spawnProps;ds.energy+=val;}}}
    public void AddPwrups(int val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.pwrups){var ds=(DisrupterConfig.spawnPwrups)dc.spawnProps;ds.pwrups+=val;}}}
    public void AddKills(int val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.kills){var ds=(DisrupterConfig.spawnKills)dc.spawnProps;ds.kills+=val;}}}
    public void AddDmg(float val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.dmg){var ds=(DisrupterConfig.spawnDmg)dc.spawnProps;ds.dmg+=val;}}}
    public void AddCounts(float val){foreach(DisrupterConfig dc in disruptersList){if(dc.disrupterType==disrupterType.counts){var ds=(DisrupterConfig.spawnCounts)dc.spawnProps;ds.counts+=val;}}}
    public void DestroyAll(){foreach(DisrupterConfig dc in disruptersList){Destroy(dc);}disruptersList.Clear();}
}
