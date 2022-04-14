using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum rarityPowerup{
    Common, 
    Rare, 
    Legendary
}
public enum powerupType{weapon,status}
[CreateAssetMenu(menuName = "Powerup Item")]
public class PowerupItem : ScriptableObject{
    [HeaderAttribute("Properties")]
    [SerializeField] new public string name;
    [SerializeField] public string assetName;
    [SerializeField] public powerupType powerupType;
    [SerializeField] public bool slottable;
}
