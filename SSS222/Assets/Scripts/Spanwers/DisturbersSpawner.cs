using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbersSpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int[] waveConfigsWeights;
    [SerializeField] int startingWave = 0;
    public int waveIndex = 0;
    WaveConfig currentWave;
    [SerializeField] bool looping = true;
    [SerializeField] bool progressiveWaves = false;
    [SerializeField] float mTimeSpawns = 2f;
    float timeSpawns = 0f;

    WaveDisplay waveDisplay;
    GameSession gameSession;
    Player player;
    // Start is called before the first frame update
    #region//GetRandomWeightedIndex
        public int GetRandomWeightedIndex(int[] weights)
        {
            if (weights == null || weights.Length == 0) return -1;

            int w=0;
            int i;
            for (i = 0; i < weights.Length; i++)
            {
                if (weights[i] >= 0) w += weights[i];
            }

            float r = Random.value;
            float s = 0f;

            for (i = 0; i < weights.Length; i++)
            {
                if (weights[i] <= 0f) continue;

                s += (float)weights[i] / waveConfigsWeights.Length;
                if (s >= r) return i;
            }

            return -1;
        }
    #endregion
    IEnumerator Start()
    {
        waveDisplay = FindObjectOfType<WaveDisplay>();
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
        do
        {
            yield return StartCoroutine(SpawnWaves());
        }
        while (looping);
    }

    public IEnumerator SpawnWaves()
    {
            if (timeSpawns<=0 && timeSpawns>-4){
                currentWave = waveConfigs[waveIndex];
                yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
                timeSpawns = -4;
            if (progressiveWaves == true){if (waveIndex<waveConfigs.Count){ waveIndex++; } }
            else{if(gameSession.EVscore>=50){ /*WaveRandomize();*/ waveIndex = Random.Range(0, waveConfigs.Count); gameSession.EVscore = 0; } }
            }
    }

    public IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        var RpathIndex = Random.Range(0, waveConfig.pathsRandom.Count);
        if (waveConfig.randomPath == false && waveConfig.between2PtsPath==false && waveConfig.shipPlace==false){
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
        if(timeSpawns>-0.01){timeSpawns -= Time.deltaTime; }
        else if(timeSpawns==-4){ timeSpawns = currentWave.timeSpawnWave; }
        if(progressiveWaves==true){if (waveIndex >= waveConfigs.Count) { waveIndex = startingWave; } }
        else{if (gameSession.EVscore >= 50) { waveDisplay.enableText = true; waveDisplay.timer = waveDisplay.showTime;
                timeSpawns = 0; waveIndex = Random.Range(0, waveConfigs.Count); currentWave = waveConfigs[waveIndex];
                gameSession.EVscore = 0; } }
        //if (timeSpawns <= 0) {timeSpawns = mTimeSpawns; }
        //Debug.Log(timeSpawns);
    }
}
