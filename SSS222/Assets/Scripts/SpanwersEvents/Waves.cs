using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    //[SerializeField] int[] waveConfigsWeights;
    [SerializeField] int startingWave = 0;
    public int waveIndex = 0;
    public WaveConfig currentWave;
    [SerializeField] bool looping = true;
    [SerializeField] bool progressiveWaves = false;
    [SerializeField] float mTimeSpawns = 2f;
    public float timeSpawns = 0f;

    WaveDisplay waveDisplay;
    LootTableWaves lootTable;
    GameSession gameSession;
    Player player;

    public float sum=0;

    private void Awake()
    {
        //foreach (WaveConfig waveConfig in waveConfigs){sum += waveConfig.spawnRate;}
    }
    IEnumerator Start()
    {
        waveDisplay = FindObjectOfType<WaveDisplay>();
        lootTable = FindObjectOfType<LootTableWaves>();
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
        do
        {
            yield return StartCoroutine(SpawnWaves());
        }
        while (looping);
    }

    public WaveConfig GetRandomWave(){
        if(currentWave==null)return lootTable.itemList[startingWave].lootItem;
        else{
            //currentWave=lootTable.GetItem();
            return lootTable.GetItem();
        }
        /*else{
            float randomWeight = 0;
            do
            {
                //No weight on any number?
                if (sum == 0)return null;
                randomWeight = Random.Range(0, sum);
            } while (randomWeight == sum);
            foreach (WaveConfig waveConfig in waveConfigs)
            {
                if (randomWeight < waveConfig.spawnRate)return waveConfig;
                randomWeight -= waveConfig.spawnRate;
            }
            return null;
        }*/
    }
    public IEnumerator SpawnWaves()
    {
            if (timeSpawns<=0 && timeSpawns>-4){
                if (currentWave == null) currentWave=lootTable.itemList[startingWave].lootItem;
                //currentWave = GetRandomWave();
                yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
                    timeSpawns = -4;
                if (progressiveWaves == true){if (waveIndex<waveConfigs.Count){ waveIndex++; } }
                else{if(gameSession.EVscore>=50){ /*WaveRandomize();*/ gameSession.EVscore = 0; currentWave=GetRandomWave(); } }//waveIndex = Random.Range(0, waveConfigs.Count);  } }
            }
    }

    public IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        var RpathIndex = Random.Range(0, waveConfig.pathsRandom.Count);
        if (waveConfig.randomPath == false && waveConfig.between2PtsPath==false && waveConfig.shipPlace== false && waveConfig.randomPoint == false){
            for (int enCount = 0; enCount < waveConfig.GetNumberOfEnemies(); enCount++)
            {
                var newEnemy = Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypoints()[enCount].transform.position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().enemyIndex = enCount;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
        }else if (waveConfig.randomPath == true || waveConfig.between2PtsPath == true) {
            if(waveConfig.between2PtsPath==true){
                for (int enCount = 0; enCount < waveConfig.GetNumberOfEnemies(); enCount++)
                {
                    var newEnemy = Instantiate(
                        waveConfig.GetEnemyPrefab(),
                        waveConfig.GetWaypoints()[0].transform.position,
                        Quaternion.identity);
                    newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                   // newEnemy.GetComponent<EnemyPathing>().enemyIndex = enCount;
                    yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
                }
            }
            if(waveConfig.randomPath == true){
                for (int enCount = 0; enCount < waveConfig.GetNumberOfEnemies(); enCount++)
                {
                    var newEnemy = Instantiate(
                        waveConfig.GetEnemyPrefab(),
                        waveConfig.GetWaypointsRandomPath(RpathIndex)[0].transform.position,
                        Quaternion.identity);
                    newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                    newEnemy.GetComponent<EnemyPathing>().enemyIndex = RpathIndex;
                    yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
                }
            }
        }else if (waveConfig.randomPoint == true)
        {
            for (int enCount = 0; enCount < waveConfig.GetNumberOfEnemies(); enCount++)
            {
                var waveWaypoints = new List<Transform>();
                foreach (Transform child in waveConfig.pathsRandom[0].transform) { waveWaypoints.Add(child); }
                var pointIndex = Random.Range(0, waveWaypoints.Count);
                var newEnemy = Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveWaypoints[Random.Range(0, pointIndex)].transform.position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().waypointIndex = pointIndex;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
        }else if(waveConfig.shipPlace==true){
            for (int enCount = 0; enCount < waveConfig.GetNumberOfEnemies(); enCount++)
            {
                var newEnemy = Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    new Vector2(player.transform.position.x, 7.2f),
                Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
        }
        else { yield return new WaitForSeconds(waveConfig.GetTimeSpawn()); }
    }

    /*public void WaveRandomize()
    {
        var weights = new Dictionary<WaveConfig, int>();
        for (int index = 0; index < waveConfigs.Count; index++){
            weights.Add(waveConfigs[index], waveConfigsWeights[index]);
        }

        WaveConfig selected = WeightedRandomizer.From(weights).TakeOne(); // Strongly-typed object returned. No casting necessary.
    }*/
    public string GetWaveName(){
        return currentWave.waveName;
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale>0.0001f){
            if (timeSpawns > -0.01) { timeSpawns -= Time.deltaTime; }
            else if (timeSpawns == -4) { timeSpawns = currentWave.timeSpawnWave; }
            else if (timeSpawns <=0 && timeSpawns > -4) {SpawnAllEnemiesInWave(currentWave); timeSpawns = currentWave.timeSpawnWave; }

            if (progressiveWaves==true){if (waveIndex >= waveConfigs.Count) { waveIndex = startingWave; } }
            else{if (gameSession.EVscore >= 50) { waveDisplay.enableText = true; waveDisplay.timer = waveDisplay.showTime;
                timeSpawns = 0; currentWave=GetRandomWave();//waveIndex = Random.Range(0, waveConfigs.Count); currentWave = waveConfigs[waveIndex];
                gameSession.EVscore = 0; } }
            //if (timeSpawns <= 0) {timeSpawns = mTimeSpawns; }
            //Debug.Log(timeSpawns);
        }
    }
}
