using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Powerup Spawner Config")]
public class PowerupsSpawnerConfig : ScriptableObject{//, ISpawnerConfig{//,IEnumerable{
    [SerializeField] public spawnReqsType spawnReqsType;
    [SerializeReference] public spawnReqs spawnReqs=new spawnReqs();
    
    [Button("VaildateSpawnReqs")][ContextMenu("VaildateSpawnReqs")]void VaildateSpawnReqs(){spawnReqsMono.Validate(ref spawnReqs, ref spawnReqsType);}
}
