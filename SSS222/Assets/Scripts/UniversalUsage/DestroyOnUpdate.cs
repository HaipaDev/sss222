using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnUpdate : MonoBehaviour{
    void Update(){
        Destroy(this.gameObject);
    }
}
