using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerup Spawner Config")]
public class PowerupsSpawnerConfig : ScriptableObject{
    [SerializeField] public powerupSpawnerType powerupSpawnerType;
    [SerializeReference] public powerupSpawnerTypesPoly powerupSpawner=new powerupSpawnerTypesPoly();
}