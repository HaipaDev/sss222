using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour{
    [SerializeField] public spawnerType spawnerType;
    //[SerializeField] List<WaveConfig> waveConfigs;
    //[SerializeField] int[] waveConfigsWeights;
    [SerializeField] public int startingWave=0;
    [SerializeField] public bool startingWaveRandom=false;
    public int waveIndex=0;
    public WaveConfig currentWave;
    //[SerializeField] bool progressiveWaves=false;
    [SerializeField] public bool uniqueWaves=true;//Unique Wave Randomization?
    [SerializeField] float mTimeSpawns=2f;
    public float timeSpawns=0f;

    WaveDisplay waveDisplay;
    LootTableWaves lootTable;

    public float sum=0;
    //private void Awake(){//foreach (WaveConfig waveConfig in waveConfigs){sum += waveConfig.spawnRate;}}
    IEnumerator Start(){
        yield return new WaitForSeconds(0.15f);
        waveDisplay=FindObjectOfType<WaveDisplay>();
        lootTable=GetComponent<LootTableWaves>();
        if(startingWaveRandom){currentWave=GetRandomWave();startingWave=waveIndex;}//Random.Range(0,lootTable.itemList.Count-1);}
        do{yield return StartCoroutine(SpawnWaves());}while(true);
    }
    public WaveConfig GetRandomWave(){
        if(currentWave==null&&!startingWaveRandom)return lootTable.itemList[startingWave].lootItem;
        else{
            //currentWave=lootTable.GetItem();
            if(uniqueWaves){
                WaveConfig wave;
                do{
                    wave=lootTable.GetItem();
                    return wave;
                }while(wave!=currentWave);
            }else{return lootTable.GetItem();}
        }
        /*else{
            float randomWeight=0;
            do
            {
                //No weight on any number?
                if (sum == 0)return null;
                randomWeight=Random.Range(0, sum);
            } while (randomWeight == sum);
            foreach (WaveConfig waveConfig in waveConfigs)
            {
                if (randomWeight < waveConfig.spawnRate)return waveConfig;
                randomWeight -= waveConfig.spawnRate;
            }
            return null;
        }*/
    }
    public IEnumerator SpawnWaves(){
        if(timeSpawns<=0&&timeSpawns>-4){
            if(currentWave==null)currentWave=lootTable.itemList[startingWave].lootItem;
            //currentWave=GetRandomWave();
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            timeSpawns=-4;
            //if(progressiveWaves==true){if(waveIndex<GetComponent<LootTableWaves>().itemList.Count){waveIndex++;}}
            //else{}//if(GameSession.instance.EVscore>=GameSession.instance.EVscoreMax){ GameSession.instance.AddXP(GameSession.instance.xp_wave); currentWave=GetRandomWave(); GameSession.instance.EVscore=0;} }//waveIndex=Random.Range(0, waveConfigs.Count);  } }
        }
    }

    public IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig){
        if(FindObjectOfType<DisruptersSpawner>()!=null)FindObjectOfType<DisruptersSpawner>().AddCounts(waveConfig);
    switch(waveConfig.wavePathType){
        case wavePathType.startToEnd:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                var newEnemy=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypointsStart()[enCount].transform.position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().enemyIndex=enCount;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.btwn2Pts:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                Vector2 pos;
                pos.x=Random.Range(waveConfig.GetWaypointsSingle()[0].transform.position.x,waveConfig.GetWaypointsSingle()[1].transform.position.x);
                pos.y=Random.Range(waveConfig.GetWaypointsSingle()[0].transform.position.y,waveConfig.GetWaypointsSingle()[1].transform.position.y);
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
                    waveConfig.GetWaypointsRandomPath(RpathIndex)[0].transform.position,
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
                var pos=waveConfig.GetWaypointRandomPoint().position;
                var w=(WaveConfig.pathRandomPoint)waveConfig.wavePaths;
                if(w.closestToPlayer&&FindObjectOfType<Player>()!=null){pos=waveConfig.GetWaypointClosestToPlayer().position;}
                var newEnemy=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    pos,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().waypointIndex=enCount;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.shipPlace:
            if(FindObjectOfType<Player>()!=null){
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
                    waveConfig.GetWaypointsSingle()[enCount].transform.position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().enemyIndex=enCount;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        default: yield return new WaitForSeconds(waveConfig.GetTimeSpawn());break;
    }
    }
    /*public void WaveRandomize(){
        var weights=new Dictionary<WaveConfig, int>();
        for (int index=0; index < waveConfigs.Count; index++){
            weights.Add(waveConfigs[index], waveConfigsWeights[index]);
        }

        WaveConfig selected=WeightedRandomizer.From(weights).TakeOne(); // Strongly-typed object returned. No casting necessary.
    }*/
    public string GetWaveName(){return currentWave.waveName;}
    // Update is called once per frame
    void Update(){
        if(Time.timeScale>0.0001f){
            if(timeSpawns>-0.01){timeSpawns -= Time.deltaTime;}
            else if(timeSpawns==-4){timeSpawns=currentWave.timeSpawnWave;}
            else if(timeSpawns<=0&&timeSpawns>-4&&currentWave!=null){SpawnAllEnemiesInWave(currentWave);timeSpawns=currentWave.timeSpawnWave;}
        }
        //if(progressiveWaves==true){if(waveIndex>=GetComponent<LootTableWaves>().itemList.Count){waveIndex=startingWave;}}
        //else{
        if(GameSession.instance!=null)if(GameSession.instance.EVscoreMax!=-5&&GameSession.instance.EVscore>=GameSession.instance.EVscoreMax){if(waveDisplay!=null){waveDisplay.enableText=true;waveDisplay.timer=waveDisplay.showTime;}
            timeSpawns=0; currentWave=GetRandomWave();//waveIndex=Random.Range(0, waveConfigs.Count); currentWave=waveConfigs[waveIndex];
            GameSession.instance.EVscore=0; GameSession.instance.AddXP(GameSession.instance.xp_wave);//XP For Wave
            }
        //}
        //if (timeSpawns <= 0) {timeSpawns=mTimeSpawns; }
        //Debug.Log(timeSpawns);
    }
}
