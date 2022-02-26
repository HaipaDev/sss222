using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Waves : MonoBehaviour{
    [Header("SpawnReqs")]
    [InlineButton("ValidateWaveSpawnReqs","Validate")]
    [SerializeField] public spawnReqsType waveSpawnReqsType=spawnReqsType.score;
    [SerializeReference] public spawnReqs waveSpawnReqs;
    [Header("Config")]
    [DisableIf("startingWaveRandom")][SerializeField] public int startingWave=0;
    [SerializeField] public bool startingWaveRandom=false;
    public int waveIndex=0;
    public WaveConfig currentWave;
    [SerializeField] public bool uniqueWaves=true;
    float checkSpawns=3f;
    
    [Header("Current Values")]
    public float timeSpawns=0f;
    float checkSpawnsTimer=0f;

    WaveDisplay waveDisplay;
    LootTableWaves lootTable;

    void Start(){
        //yield return new WaitForSeconds(0.05f);
        waveDisplay=FindObjectOfType<WaveDisplay>();
        lootTable=GetComponent<LootTableWaves>();
        if(startingWaveRandom){currentWave=GetRandomWave();startingWave=waveIndex;}
    }
    /*public IEnumerator CallRandomizeWave(){
        RandomizeWave();
        GameSession.instance.RandomizeWaveScoreMax();
        yield return null;
    }*/

    [Button("Spawn RandomizeWave")][ContextMenu("RandomizeWave")]public void RandomizeWaveCall(){StartCoroutine(RandomizeWave());}
    public IEnumerator RandomizeWave(){
        if(waveDisplay!=null){waveDisplay.enableText=true;waveDisplay.timer=waveDisplay.showTime;}
        currentWave=GetRandomWave();
        if(GameRules.instance.xpOn){GameSession.instance.DropXP(GameRules.instance.xp_wave,new Vector2(0,7),3f);}else{GameSession.instance.AddXP(GameRules.instance.xp_wave);}
        GameSession.instance.RandomizeWaveScoreMax();
        spawnReqsMono.AddWaves();
        yield return null;
    }
    public WaveConfig GetRandomWave(){
    if(lootTable!=null){
        if(!startingWaveRandom&&(currentWave==null&&lootTable.itemList!=null)){if(lootTable.itemList.Count>0){return lootTable.itemList[startingWave].lootItem;}else{return null;}}
        else{
            if(uniqueWaves){
                WaveConfig wave;
                do{
                    wave=lootTable.GetItem();
                    return wave;
                }while(wave==currentWave);
            }else{return lootTable.GetItem();}
        }
    }else{return null;}}
    
    void CheckSpawnReqs(){
        if(waveSpawnReqs!=GameRules.instance.waveSpawnReqs)waveSpawnReqs=GameRules.instance.waveSpawnReqs;
        if(waveSpawnReqsType!=GameRules.instance.waveSpawnReqsType)waveSpawnReqsType=GameRules.instance.waveSpawnReqsType;
        spawnReqs x=waveSpawnReqs;
        spawnReqsType xt=waveSpawnReqsType;
        spawnReqsMono.instance.CheckSpawns(x,xt,this,"RandomizeWave");
    }
    void Update(){
        if(!GameSession.GlobalTimeIsPaused){
            CheckSpawnReqs();

            if(currentWave==null&&lootTable.itemList.Count>0)currentWave=lootTable.itemList[startingWave].lootItem;
            if(timeSpawns>0){timeSpawns-=Time.deltaTime;}
            else if(timeSpawns==-4){timeSpawns=currentWave.timeSpawnWave;}
            else if(timeSpawns<=0&&timeSpawns>-4&&currentWave!=null){StartCoroutine(SpawnAllEnemiesInWave(currentWave));timeSpawns=currentWave.timeSpawnWave;}
            

            //Check if no Enemies for 3s, force a wave spawn
            if(FindObjectsOfType<Enemy>().Length==0){
                if(checkSpawnsTimer==-4)checkSpawnsTimer=checkSpawns;
                if(checkSpawnsTimer>0)checkSpawnsTimer-=Time.deltaTime;
                else if(checkSpawnsTimer<=0&&checkSpawnsTimer>-4){
                    Debug.LogWarning("No enemies found, forcing a spawn!");
                    currentWave=GetRandomWave();
                    if(timeSpawns==-4){timeSpawns=currentWave.timeSpawnWave;}
                    //StartCoroutine(SpawnWave());
                    checkSpawnsTimer=checkSpawns;
                }
            }else{checkSpawnsTimer=-4;}
        }
    }

    #region//SpawnAllEnemiesInWave
    public IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig){
        spawnReqsMono.AddWaveCounts(waveConfig);
    switch(waveConfig.wavePathType){
        case wavePathType.startToEnd:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                GameObject go=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypointsStart()[enCount].position,
                    Quaternion.identity);
                if(go.GetComponent<EnemyPathing>()!=null){
                    go.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                    go.GetComponent<EnemyPathing>().enemyIndex=enCount;
                }else{Debug.LogWarning("No EnemyPathing for "+go.name);}
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.btwn2Pts:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                Vector2 pos;
                pos.x=Random.Range(waveConfig.GetWaypointsSingle()[0].position.x,waveConfig.GetWaypointsSingle()[1].position.x);
                pos.y=Random.Range(waveConfig.GetWaypointsSingle()[0].position.y,waveConfig.GetWaypointsSingle()[1].position.y);
                GameObject go=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    pos,
                    Quaternion.identity);
                if(go.GetComponent<EnemyPathing>()!=null){
                    go.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                    // go.GetComponent<EnemyPathing>().enemyIndex=enCount;
                }
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.randomPath:
            var pR=(WaveConfig.pathRandom)waveConfig.wavePaths;
            var RpathIndex=Random.Range(0, pR.pathsRandom.Count);
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                GameObject go=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypointsRandomPath(RpathIndex)[0].position,
                    Quaternion.identity);
                if(go.GetComponent<EnemyPathing>()!=null){
                    go.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                    go.GetComponent<EnemyPathing>().enemyIndex=RpathIndex;
                }else{Debug.LogWarning("No EnemyPathing for "+go.name);}
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.randomPoint:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                //var waveWaypoints=new List<Transform>();
                //foreach(Transform child in waveConfig.GetWaypointsRandomPoint()){waveWaypoints.Add(child);}
                //var pointIndex=Random.Range(0, waveWaypoints.Count);
                var waypoints=waveConfig.GetWaypointsRandomPoint();
                var pos=waveConfig.GetWaypointRandomPoint().position;
                var w=(WaveConfig.pathRandomPoint)waveConfig.wavePaths;
                if(w.closestToPlayer&&Player.instance!=null){pos=waveConfig.GetWaypointClosestToPlayer().position;}
                GameObject go=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    pos,
                    Quaternion.identity);
                if(go.GetComponent<EnemyPathing>()!=null){
                    go.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                    go.GetComponent<EnemyPathing>().waypointIndex=Random.Range(0,waypoints.Count);
                    //go.GetComponent<EnemyPathing>().waypointIndex=enCount;
                }else{Debug.LogWarning("No EnemyPathing for "+go.name);}
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.shipPlace:
            if(Player.instance!=null){
                var pS=(WaveConfig.shipPlace)waveConfig.wavePaths;
                for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                    GameObject go=Instantiate(
                        waveConfig.GetEnemyPrefab(),
                        waveConfig.GetShipPlaceCoords(waveConfig),
                    Quaternion.identity);
                    if(go.GetComponent<EnemyPathing>()!=null){if(!go.GetComponent<EnemyPathing>().off){
                        go.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                    }}else{Debug.LogWarning("No EnemyPathing for "+go.name);}
                    yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
                }
            }
            break;
        case wavePathType.loopPath:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                GameObject go=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypointsSingle()[enCount].position,
                    Quaternion.identity);
                if(go.GetComponent<EnemyPathing>()!=null){
                    go.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                    go.GetComponent<EnemyPathing>().enemyIndex=enCount;
                }else{Debug.LogWarning("No EnemyPathing for "+go.name);}
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        default: yield return new WaitForSeconds(waveConfig.GetTimeSpawn());break;
    }}
    #endregion
    public string GetWaveName(){return currentWave.waveName;}

    [ContextMenu("ValidateWaveSpawnReqs")]void ValidateWaveSpawnReqs(){spawnReqsMono.Validate(ref waveSpawnReqs, ref waveSpawnReqsType);}
}
