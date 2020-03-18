using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerOld : MonoBehaviour{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    WaveConfig currentWave;
    [SerializeField] bool looping = false;
    [SerializeField] float mTimeSpawns = 3f;
    float timeSpawns = 0f;
    // Start is called before the first frame update
    IEnumerator Start(){
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
    }

    public IEnumerator SpawnAllWaves(){
        if(timeSpawns <= 0){
            for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++){
                currentWave = waveConfigs[waveIndex];
                yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            }
            timeSpawns = mTimeSpawns;
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig){
        for(int enCount=0; enCount < waveConfig.GetNumberOfEnemies(); enCount++){
            var newEnemy=Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
        }
    }
    private void Update(){
        //if(currentWave >= waveConfigs.Count)
            timeSpawns -= Time.deltaTime;
        Debug.Log(timeSpawns);
    }
}
