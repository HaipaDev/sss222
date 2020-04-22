using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnShip : MonoBehaviour{
    Vector2 playerPosX;
    Vector2 posY;
    Vector2 selfPos;
    Transform target;
    [SerializeField] float speedFollow = 2f;
    [SerializeField] float vspeed = 0.1f;
    [SerializeField] float distY = 1.3f;
    [SerializeField] float distX = 0.3f;

    Player player;
    Enemy enemy;
    // rb;
    //GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
        //rb = GetComponent<Rigidbody2D>();
        //gameSession = FindObjectOfType<GameSession>();

        posY = new Vector2(transform.position.x, transform.position.y - distY);
    }

    // Update is called once per frame
    void Update()
    {
        float stepY = vspeed * Time.deltaTime;
        float stepX = speedFollow * Time.deltaTime;
        playerPosX = new Vector2(player.transform.position.x, transform.position.y);
        selfPos = new Vector2(transform.position.x,transform.position.y);
        var dist = Vector2.Distance(playerPosX, selfPos);

        if (transform.position.y>posY.y){ transform.position = new Vector2(player.transform.position.x, transform.position.y - vspeed); }
        else {
            /*if(transform.position.x < player.transform.position.x+ distX)
            {
                transform.position = new Vector2(transform.position.x + speedFollow, transform.position.y);
            }else if(transform.position.x > player.transform.position.x+distX)
            {
                transform.position = new Vector2(transform.position.x - speedFollow, transform.position.y);
            }
            else{}*/
            //if (!target) { return; }

            var dir = (playerPosX - selfPos).normalized;
            selfPos += dir * speedFollow * Time.deltaTime;
            transform.position = selfPos;

        }

        //Debug.Log(stepY);
    }
}
