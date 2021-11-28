using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Powerup Spawner Config")]
public class PowerupsSpawnerConfig : ScriptableObject{//, ISpawnerConfig{//,IEnumerable{
    [InlineButton("VaildateSpawnReqs","Validate")]
    [SerializeField] public spawnReqsType spawnReqsType;
    [SerializeReference] public spawnReqs spawnReqs=new spawnReqs();
    
    [ContextMenu("VaildateSpawnReqs")]void VaildateSpawnReqs(){spawnReqsMono.Validate(ref spawnReqs, ref spawnReqsType);}
}
