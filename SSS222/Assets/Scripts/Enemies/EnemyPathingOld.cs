using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathingOld : MonoBehaviour{
    [SerializeField] WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;

    // Start is called before the first frame update
    void Start(){
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[0].transform.position;
    }

    // Update is called once per frame
    void Update(){
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig){
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPos = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, movementThisFrame);
            if (transform.position == targetPos) waypointIndex++;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
