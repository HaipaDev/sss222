using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOffOnParentToggle : MonoBehaviour{
    void Start(){StartCoroutine(StartI());}
    IEnumerator StartI(){
        yield return new WaitForSecondsRealtime(0.3f);
        gameObject.SetActive(GetComponentInParent<Toggle>().isOn);
        Destroy(this);
    }
}
