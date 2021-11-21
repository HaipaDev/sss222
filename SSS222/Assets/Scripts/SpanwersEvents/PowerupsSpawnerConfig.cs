using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerup Spawner Config")]
public class PowerupsSpawnerConfig : ScriptableObject{//, ISpawnerConfig{//,IEnumerable{
    [SerializeField] public spawnReqsType spawnReqsType;
    [SerializeReference] public spawnReqs spawnReqs=new spawnReqs();
    
    /*[Button("Validate")]*/[ContextMenu("Validate")]void Vaildate(){
        if(spawnReqsType==spawnReqsType.time){spawnReqs=new spawnReqs();spawnReqs.secondEnabled=false;spawnReqs.bothNeeded=false;}
        if(spawnReqsType==spawnReqsType.energy){spawnReqs=new spawnEnergy();}
        if(spawnReqsType==spawnReqsType.missed){spawnReqs=new spawnEnergy();}
        if(spawnReqsType==spawnReqsType.pwrups){spawnReqs=new spawnPwrups();}
        if(spawnReqsType==spawnReqsType.kills){spawnReqs=new spawnKills();}
        if(spawnReqsType==spawnReqsType.dmg){spawnReqs=new spawnDmg();}
        if(spawnReqsType==spawnReqsType.counts){spawnReqs=new spawnCounts();}
    }
}
