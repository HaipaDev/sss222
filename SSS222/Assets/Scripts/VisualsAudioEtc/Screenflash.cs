using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Screenflash : MonoBehaviour{   public static Screenflash instance;
    [Header("Prefab etc")]
    [AssetsOnly][SerializeField] GameObject screenflashImgPrefab;
    [Header("Damage")]
    [SerializeField] Sprite damageFlashSprite;
    [SerializeField] Color damageFlashColor;
    [SerializeField] float damageFlashSpeed;
    [Header("Heal")]
    [SerializeField] Sprite healFlashSprite;
    [SerializeField] Color healFlashColor;
    [SerializeField] float healFlashSpeed;
    [Header("Shadow")]
    [SerializeField] Sprite shadowFlashSprite;
    [SerializeField] Color shadowFlashColor;
    [SerializeField] float shadowFlashSpeed;
    [Header("Flame")]
    [SerializeField] Sprite flameFlashSprite;
    [SerializeField] Color flameFlashColor;
    [SerializeField] float flameFlashSpeed;
    [Header("Electicify")]
    [SerializeField] Sprite electrcFlashSprite;
    [SerializeField] Color electrcFlashColor;
    [SerializeField] float electrcFlashSpeed;
    [Header("Freeze")]
    [SerializeField] Sprite frozenFlashSprite;
    [SerializeField] Color frozenFlashColor;
    [SerializeField] float frozenFlashSpeed;
    void Start(){
        if(Screenflash.instance==null)instance=this;
    }

    public void Damage(){
        GameObject go=Instantiate(screenflashImgPrefab,transform);var simg=go.GetComponent<ScreenflashImg>();
        simg.Setup(damageFlashSprite,damageFlashColor,damageFlashSpeed);
    }
    public void Heal(){
        GameObject go=Instantiate(screenflashImgPrefab,transform);var simg=go.GetComponent<ScreenflashImg>();
        simg.Setup(healFlashSprite,healFlashColor,healFlashSpeed);
    }
    public void Shadow(){
        GameObject go=Instantiate(screenflashImgPrefab,transform);var simg=go.GetComponent<ScreenflashImg>();
        simg.Setup(shadowFlashSprite,shadowFlashColor,shadowFlashSpeed);
    }
    public void Flame(){
        GameObject go=Instantiate(screenflashImgPrefab,transform);var simg=go.GetComponent<ScreenflashImg>();
        simg.Setup(flameFlashSprite,flameFlashColor,flameFlashSpeed);
    }
    public void Electrc(){
        GameObject go=Instantiate(screenflashImgPrefab,transform);var simg=go.GetComponent<ScreenflashImg>();
        simg.Setup(electrcFlashSprite,electrcFlashColor,electrcFlashSpeed);
    }
    public void Freeze(){
        GameObject go=Instantiate(screenflashImgPrefab,transform);var simg=go.GetComponent<ScreenflashImg>();
        simg.Setup(frozenFlashSprite,frozenFlashColor,frozenFlashSpeed);
    }
}
