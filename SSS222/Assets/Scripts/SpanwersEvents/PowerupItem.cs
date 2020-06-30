using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum rarityPowerup{
    Common, 
    Rare, 
    Legendary
    };
[CreateAssetMenu(menuName = "Powerup Item")]
public class PowerupItem : ScriptableObject{
    [HeaderAttribute("Properties")]
    [SerializeField] public string name;
    [SerializeField] public GameObject item;
    [SerializeField] public float dropChance=1;
    [SerializeField] public rarityPowerup rarity;
    [SerializeField] public int levelReq=0;
}
