using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour{
    [SerializeField] public int startingWave=0;
    [SerializeField] public bool startingWaveRandom=false;
    public int waveIndex=0;
    public WaveConfig currentWave;
    [SerializeField] public bool uniqueWaves=true;
    [SerializeField] float mTimeSpawns=2f;
    public float timeSpawns=0f;
    float checkSpawns=5f;
    float checkSpawnsTimer=0f;

    WaveDisplay waveDisplay;
    LootTableWaves lootTable;

    IEnumerator Start(){
        yield return new WaitForSeconds(0.15f);
        waveDisplay=FindObjectOfType<WaveDisplay>();
        lootTable=GetComponent<LootTableWaves>();
        if(startingWaveRandom){currentWave=GetRandomWave();startingWave=waveIndex;}
        do{yield return StartCoroutine(SpawnWaves());}while(true);
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
                }while(wave!=currentWave);
            }else{return lootTable.GetItem();}
        }
    }else{return null;}}
    public IEnumerator SpawnWaves(){
        if(!GameSession.GlobalTimeIsPaused&&timeSpawns<=0&&timeSpawns>-4){
            if(currentWave==null)currentWave=lootTable.itemList[startingWave].lootItem;
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            timeSpawns=-4;
        }
    }

    public IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig){
        if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddCounts(waveConfig);
    switch(waveConfig.wavePathType){
        case wavePathType.startToEnd:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                var newEnemy=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypointsStart()[enCount].position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().enemyIndex=enCount;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.btwn2Pts:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                Vector2 pos;
                pos.x=Random.Range(waveConfig.GetWaypointsSingle()[0].position.x,waveConfig.GetWaypointsSingle()[1].position.x);
                pos.y=Random.Range(waveConfig.GetWaypointsSingle()[0].position.y,waveConfig.GetWaypointsSingle()[1].position.y);
                var newEnemy=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    pos,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                // newEnemy.GetComponent<EnemyPathing>().enemyIndex=enCount;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.randomPath:
            var pR=(WaveConfig.pathRandom)waveConfig.wavePaths;
            var RpathIndex=Random.Range(0, pR.pathsRandom.Count);
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                var newEnemy=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypointsRandomPath(RpathIndex)[0].position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().enemyIndex=RpathIndex;
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
                var newEnemy=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    pos,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().waypointIndex=Random.Range(0,waypoints.Count);
                //newEnemy.GetComponent<EnemyPathing>().waypointIndex=enCount;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.shipPlace:
            if(Player.instance!=null){
                var pS=(WaveConfig.shipPlace)waveConfig.wavePaths;
                for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                    var newEnemy=Instantiate(
                        waveConfig.GetEnemyPrefab(),
                        waveConfig.GetShipPlaceCoords(waveConfig),
                    Quaternion.identity);
                    if(GetComponent<EnemyPathing>()!=null)newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                    yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
                }
            }
            break;
        case wavePathType.loopPath:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                var newEnemy=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypointsSingle()[enCount].position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().enemyIndex=enCount;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        default: yield return new WaitForSeconds(waveConfig.GetTimeSpawn());break;
    }}
    public string GetWaveName(){return currentWave.waveName;}
    void Update(){
        if(!GameSession.GlobalTimeIsPaused){
            if(timeSpawns>-0.01){timeSpawns-=Time.deltaTime;}
            else if(timeSpawns==-4){timeSpawns=currentWave.timeSpawnWave;}
            else if(timeSpawns<=0&&timeSpawns>-4&&currentWave!=null){SpawnAllEnemiesInWave(currentWave);GameSession.instance.RandomizeEVScoreMax();timeSpawns=currentWave.timeSpawnWave;}
        }
        if(GameSession.instance!=null)if(GameSession.instance.EVscoreMax!=-5&&GameSession.instance.EVscore>=GameSession.instance.EVscoreMax){if(waveDisplay!=null){waveDisplay.enableText=true;waveDisplay.timer=waveDisplay.showTime;}
            timeSpawns=0;currentWave=GetRandomWave();
            GameSession.instance.EVscore=0;if(GameRules.instance.xpOn){GameSession.instance.DropXP(GameRules.instance.xp_wave,new Vector2(0,7),3f);}else{GameSession.instance.AddXP(GameRules.instance.xp_wave);}
        }

        //Check every 5s if no Enemies, force a wave spawn
        if(checkSpawnsTimer>0)checkSpawnsTimer-=Time.deltaTime;
        else if(checkSpawns>0){
            if(FindObjectsOfType<Enemy>().Length==0){
                if(currentWave==null){currentWave=GetRandomWave();}
                StartCoroutine(SpawnWaves());
            }
            checkSpawnsTimer=checkSpawns;
        }
    }
}
