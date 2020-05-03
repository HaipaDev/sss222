using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour{
    [SerializeField] WaveConfig waveConfig;
    List<Transform> waypointsS;
    List<Transform> waypointsE;
    List<Transform> waypointsR;
    public int waypointIndex = 0;
    public int enemyIndex = 0;

    Rigidbody2D rb;
    Player player;
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        if (waveConfig.startToEndPath == true) {
            waypointsS = waveConfig.GetWaypoints();
            waypointsE = waveConfig.GetWaypointsEnd();
            waypointIndex = enemyIndex;
        }else{
            if(waveConfig.randomPath==true){
                waypointsR = waveConfig.GetWaypointsRandomPath(enemyIndex);
            }else if(waveConfig.randomPathEach==true){
                waypointsR = waveConfig.GetWaypointsRandomPathEach();
            }else if (waveConfig.randomPoint == true){
                waypointsR = waveConfig.GetWaypointsRandomPoint();
            }else{
                waypointsS = waveConfig.GetWaypoints();
            }
        }
        if (waveConfig.randomPath == true || waveConfig.randomPathEach==true || waveConfig.randomPoint==true){ transform.position = waypointsR[waypointIndex].transform.position; 
            if(waveConfig.randomPoint==true && (waveConfig.GetMoveSpeed()!=0 || waveConfig.randomSpeed==true)){
                if(waveConfig.randomSpeed==false){rb.velocity = new Vector2(0f, -waveConfig.GetMoveSpeed()); }
                else{ rb.velocity = new Vector2(0f, Random.Range(-waveConfig.GetMoveSpeedS(), -waveConfig.GetMoveSpeedE())); }
            }
        }
        else if(waveConfig.between2PtsPath==true){ var p0 = waypointsS[0].transform.position; var p1 = waypointsS[1].transform.position;
            Vector3 v = p1 - p0;
            transform.position = p0 + Random.value * v;
            if(waveConfig.randomSpeed==false){rb.velocity = new Vector2(0f, -waveConfig.GetMoveSpeed()); }
            else{ rb.velocity = new Vector2(0f, Random.Range(-waveConfig.GetMoveSpeedS(), -waveConfig.GetMoveSpeedE())); }
        }
        else if(waveConfig.shipPlace==true){ transform.position = new Vector2(player.transform.position.x, 7.2f);
            if (waveConfig.randomSpeed == false) { rb.velocity = new Vector2(0f, -waveConfig.GetMoveSpeed()); }
            else { rb.velocity = new Vector2(0f, Random.Range(-waveConfig.GetMoveSpeedS(), -waveConfig.GetMoveSpeedE())); }
        }
        else { transform.position = waypointsS[waypointIndex].transform.position; }
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
        if(waveConfig.startToEndPath==true)
        {
            if (transform.position!= waypointsE[waypointIndex].transform.position)
            {
                var targetPos = waypointsE[waypointIndex].transform.position;
                var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPos, movementThisFrame);
                //if (transform.position == targetPos)waypointIndex++;
            }
            else{ Destroy(gameObject); }
        }else if(waveConfig.between2PtsPath==true){
            if(transform.position.y<-7.5f){ Destroy(gameObject); }
        }
        else
        {
            if(waveConfig.randomPath==true || waveConfig.randomPathEach==true){
                if (waypointIndex < waypointsR.Count)
                {
                    var targetPos = waypointsR[waypointIndex].transform.position;
                    var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, movementThisFrame);
                    if (transform.position == targetPos)waypointIndex++;
                }else { Destroy(gameObject); }
            }else if(waveConfig.randomPoint==true){
            } else{
                if(waveConfig.shipPlace==false){
                    if (waypointIndex < waypointsS.Count)
                    {
                        var targetPos = waypointsS[waypointIndex].transform.position;
                        var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
                        transform.position = Vector2.MoveTowards(transform.position, targetPos, movementThisFrame);
                        if (transform.position == targetPos) waypointIndex++;
                    }
                    else { Destroy(gameObject); }
                }
            }
        }
    }
}
