using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Wave Config")]
public class WaveConfig:ScriptableObject{
    [HeaderAttribute("Properties")]
    [SerializeField] public string waveName="";
    //[SerializeField] public float spawnRate=10f;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] public float timeSpawn=0.5f;
    [SerializeField] public float timeSpawnWave=6f;
    [SerializeField] float spawnRandomFactor=0.3f;
    [SerializeField] int numberOfEnemies=3;
    [SerializeField] float moveSpeed=2f;
    [SerializeField] public bool randomSpeed=false;
    [SerializeField] float moveSpeedS=1f;
    [SerializeField] float moveSpeedE=3f;
    //[HeaderAttribute("StartToEnd Path")]
    public wavePathType wavePathType;
    [SerializeReference]public wavePathTypeProps wavePaths;
    [System.Serializable]public class wavePathTypeProps{}
    [System.Serializable]public class pathSingle:wavePathTypeProps{
        public GameObject path;
    }
    [System.Serializable]public class pathStartToEnd:wavePathTypeProps{
        public GameObject pathStartPrefab;
        public GameObject pathEndPrefab;
    }
    [System.Serializable]public class pathRandom:wavePathTypeProps{
        public List<GameObject> pathsRandom;
    }
    [System.Serializable]public class pathRandomPoint:wavePathTypeProps{
        public GameObject path;
        public bool closestToPlayer;
    }
    [System.Serializable]public class shipPlace:wavePathTypeProps{
        public spawnSide _spawnSide;
        public enum spawnSide{up,down,left,right,lOrR}
        public Vector2 coords;
    }
    
    [ContextMenu("Validate")]void Vaildate(){
        //if(validate){
        if(wavePathType==wavePathType.btwn2Pts||wavePathType==wavePathType.loopPath){wavePaths=new pathSingle();}
        if(wavePathType==wavePathType.startToEnd){wavePaths=new pathStartToEnd();}
        if(wavePathType==wavePathType.randomPath||wavePathType==wavePathType.randomPathEach){wavePaths=new pathRandom();}
        if(wavePathType==wavePathType.randomPoint){wavePaths=new pathRandomPoint();}
        if(wavePathType==wavePathType.shipPlace){if(!typeof(shipPlace).IsInstanceOfType(wavePaths))wavePaths=new shipPlace();
        var w=(shipPlace)wavePaths;
        if(w._spawnSide==shipPlace.spawnSide.up){w.coords=new Vector2(0,7.2f);}
        else if(w._spawnSide==shipPlace.spawnSide.down){w.coords=new Vector2(0,-7.2f);}
        else if(w._spawnSide==shipPlace.spawnSide.left||w._spawnSide==shipPlace.spawnSide.lOrR){w.coords=new Vector2(-3.9f,0);}
        else if(w._spawnSide==shipPlace.spawnSide.right){w.coords=new Vector2(3.9f,0);}
        
        }
        //if(costType==costType.boomerang){cost=1;}
        //validate=false;}
    }
#region //Getters
    public GameObject GetEnemyPrefab(){return enemies[0];}
    public GameObject GetEnemyPrefabRandom(){var enIndex=Random.Range(0, enemies.Count);return enemies[enIndex];}
    public List<Transform> GetWaypointsSingle(){
        var waveWaypoints=new List<Transform>();
        var p=(pathSingle)wavePaths;
        foreach(Transform child in p.path.transform){waveWaypoints.Add(child);}
        return waveWaypoints;
    }
    public List<Transform> GetWaypointsStart(){
        var waveWaypoints=new List<Transform>();
        var p=(pathStartToEnd)wavePaths;
        foreach(Transform child in p.pathStartPrefab.transform){waveWaypoints.Add(child);}
        return waveWaypoints; 
    }

    public List<Transform> GetWaypointsEnd(){
        var waveWaypoints=new List<Transform>();
        var p=(pathStartToEnd)wavePaths;
        foreach(Transform child in p.pathEndPrefab.transform){waveWaypoints.Add(child);}
        return waveWaypoints;
    }

    public List<Transform> GetWaypointsRandomPath(int pathIndex){
        var waveWaypoints=new List<Transform>();
        var p=(pathRandom)wavePaths;
        foreach(Transform child in p.pathsRandom[pathIndex].transform){waveWaypoints.Add(child);}
        return waveWaypoints;
    }
    public List<Transform> GetWaypointsRandomPathEach(){
        var waveWaypoints=new List<Transform>();
        var p=(pathRandom)wavePaths;
        var pathIndex=Random.Range(0, p.pathsRandom.Count);
        foreach(Transform child in p.pathsRandom[pathIndex].transform){waveWaypoints.Add(child);}
        return waveWaypoints;
    }
    public List<Transform> GetWaypointsRandomPoint(){
        var waveWaypoints=new List<Transform>();
        var p=(pathRandomPoint)wavePaths;
        //var pathIndex=Random.Range(0, waveWaypoints.Count);
        foreach(Transform child in p.path.transform){waveWaypoints.Add(child);}
        return waveWaypoints;
    }
    public Transform GetWaypointRandomPoint(){
        var waveWaypoints=new List<Transform>();
        var p=(pathRandomPoint)wavePaths;
        //var pathIndex=Random.Range(0, waveWaypoints.Count);
        foreach(Transform child in p.path.transform){waveWaypoints.Add(child);}
        return waveWaypoints[Random.Range(0, waveWaypoints.Count)];
    }
    public Transform GetWaypointClosestToPlayer(){
        KdTree<Transform> waveWaypoints=new KdTree<Transform>();
        var p=(pathRandomPoint)wavePaths;
        //var pathIndex=Random.Range(0, waveWaypoints.Count);
        foreach(Transform child in p.path.transform){waveWaypoints.Add(child);}
        Transform closest=waveWaypoints.FindClosest(Player.instance.transform.position);
        return closest;
    }
    public Vector2 GetShipPlaceCoords(WaveConfig waveConfig){
        var w=(shipPlace)wavePaths;
        Vector2 coords=w.coords;
        if(w._spawnSide==shipPlace.spawnSide.up||w._spawnSide==shipPlace.spawnSide.down){coords.x=Player.instance.transform.position.x;}
        else if(w._spawnSide==shipPlace.spawnSide.left||w._spawnSide==shipPlace.spawnSide.right||w._spawnSide==shipPlace.spawnSide.lOrR){coords.y=Player.instance.transform.position.y;}
        else if(w._spawnSide==shipPlace.spawnSide.lOrR){if(Random.Range(0,100)<50){w.coords.x*=-1;}}
        return coords;
    }
    public float GetTimeSpawn(){return timeSpawn;}
    public float GetTimeSpawnWave(){return timeSpawnWave;}
    public float GetSpawnRandomFactor(){return spawnRandomFactor;}
    public int GetNumberOfEnemies(){return numberOfEnemies;}
    public float GetMoveSpeed(){return moveSpeed;}
    public float GetMoveSpeedS(){return moveSpeedS;}
    public float GetMoveSpeedE(){return moveSpeedE;}
#endregion
}
public enum wavePathType{startToEnd,btwn2Pts,randomPath,randomPathEach,randomPoint,shipPlace,loopPath}