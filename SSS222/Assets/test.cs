using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    float kuba = 2.3f;
    string nazwisko = "Ruchniewicz";
    int kubaI=11;
    bool madry=true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        kuba+=Time.deltaTime;

        if(madry)kubaI=13;
        else kubaI=12;

        if(kuba>2.3f){madry=true;}
        else madry=false;
    }
}
