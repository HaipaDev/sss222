using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour{
    [SerializeField] GameObject glowVFX;
    GameObject glow;
    ParticleSystem ps;
    public Color colorGlow = Color.red;
    [SerializeField] float sizeGlow = 1f;
    [SerializeField] float alphaGlow = 0.5f;
    [SerializeField] float emissionSpeed = 7.63f;
    //[SerializeField] float speed=
    [SerializeField] float xx = 0;
    [SerializeField] float yy = 0;
    // Start is called before the first frame update
    void Start(){
        glow = Instantiate(glowVFX, new Vector2(transform.position.x+xx,transform.position.y+yy), Quaternion.identity);
        glow.transform.parent = transform;
        //transform.position=new Vector3(transform.position.x,transform.position.y,transform.position.z-0.01f);
        ps = glow.GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.enabled = true;
        colorGlow.a = alphaGlow;
        col.color = colorGlow;
        var size = ps.sizeOverLifetime;
        size.enabled = true;
        size.size = sizeGlow;
        var emission = ps.emission;
        emission.rateOverTime = emissionSpeed;
        //ps = glow.GetComponent<ParticleSystem>();
        //glow = Instantiate(glowVFX, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update(){
        
        //else Destroy(glow);
        //if (ps){if (!ps.IsAlive()){ Destroy(gameObject);}}
    }
}
