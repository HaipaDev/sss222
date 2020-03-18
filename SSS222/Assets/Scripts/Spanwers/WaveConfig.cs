using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject{
    [HeaderAttribute("Properties")]
    [SerializeField] public string waveName = "";
    [SerializeField] List<GameObject> enemies;
    [SerializeField] public float timeSpawn = 0.5f;
    [SerializeField] public float timeSpawnWave = 6f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 3;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] public bool randomSpeed = false;
    [SerializeField] float moveSpeedS = 1f;
    [SerializeField] float moveSpeedE = 3f;
    [HeaderAttribute("StartToEnd Path")]
    [SerializeField] public bool startToEndPath;
    [SerializeField] public bool between2PtsPath;
    [SerializeField] GameObject pathStartPrefab;
    [SerializeField] GameObject pathEndPrefab;
    [HeaderAttribute("Random Paths")]
    [SerializeField] public bool randomPath;
    [SerializeField] public bool randomPathEach;
    [SerializeField] public List<GameObject> pathsRandom;
    [HeaderAttribute("Other")]
    [SerializeField] public bool shipPlace;


    public GameObject GetEnemyPrefab(){ var enIndex = Random.Range(0, enemies.Count); return enemies[enIndex]; }
    public List<Transform> GetWaypoints(){
        var waveWaypoints=new List<Transform>();
        foreach (Transform child in pathStartPrefab.transform){
            waveWaypoints.Add(child);
        }
        return waveWaypoints; 
    }

    public List<Transform> GetWaypointsEnd()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathEndPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }

    public List<Transform> GetWaypointsRandomPath(int pathIndex)
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathsRandom[pathIndex].transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }
    public List<Transform> GetWaypointsRandomPathEach()
    {
        var waveWaypoints = new List<Transform>();
        var pathIndex = Random.Range(0, pathsRandom.Count);
        foreach (Transform child in pathsRandom[pathIndex].transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }

    public List<Transform> GetWaypointsBetween()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathStartPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }

    public float GetTimeSpawn(){ return timeSpawn; }
    public float GetTimeSpawnWave(){ return timeSpawnWave; }
    public float GetSpawnRandomFactor(){ return spawnRandomFactor; }
    public int GetNumberOfEnemies(){ return numberOfEnemies; }
    public float GetMoveSpeed(){ return moveSpeed; }
    public float GetMoveSpeedS(){ return moveSpeedS; }
    public float GetMoveSpeedE(){ return moveSpeedE; }

}
