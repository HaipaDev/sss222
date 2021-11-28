using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName="Disrupter Config")]
public class DisrupterConfig:ScriptableObject{//, ISpawnerConfig{//,IEnumerable{
    [HeaderAttribute("Properties")]
    [InlineButton("VaildateSpawnReqs","Validate")]
    public WaveConfig waveConfig;
    public spawnReqsType spawnReqsType;
    [SerializeReference]public spawnReqs spawnReqs;
    [ContextMenu("VaildateSpawnReqs")]void VaildateSpawnReqs(){spawnReqsMono.Validate(ref spawnReqs, ref spawnReqsType);}
}
public enum disrupterType{time,energy,missed,pwrups,kills,dmg,counts}