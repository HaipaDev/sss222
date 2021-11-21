using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Disrupter Config")]
public class DisrupterConfig:ScriptableObject{//, ISpawnerConfig{//,IEnumerable{
    [HeaderAttribute("Properties")]
    public WaveConfig waveConfig;
    public spawnReqsType spawnReqsType;
    [SerializeReference]public spawnReqs spawnReqs;
    /*[Button("Validate)]*//*[ContextMenu("Validate")]void Vaildate(){
        if(spawnReqsType==spawnReqsType.time){spawnReqs=new spawnReqs();spawnReqs.secondEnabled=false;spawnReqs.bothNeeded=false;}
        if(spawnReqsType==spawnReqsType.energy){spawnReqs=new spawnEnergy();}
        if(spawnReqsType==spawnReqsType.missed){spawnReqs=new spawnEnergy();}
        if(spawnReqsType==spawnReqsType.pwrups){spawnReqs=new spawnPwrups();}
        if(spawnReqsType==spawnReqsType.kills){spawnReqs=new spawnKills();}
        if(spawnReqsType==spawnReqsType.dmg){spawnReqs=new spawnDmg();}
        if(spawnReqsType==spawnReqsType.counts){spawnReqs=new spawnCounts();}
    }*/
    /*public class disrupterSpawnProps{
        public bool timeEnabled=true;
        public float timeS=20;
        public float timeE=50;
        public float time=-4;
        public int repeat=1;
        public float repeatInterval=0.75f;
        public bool secondEnabled=true;
        public bool bothNeeded=true;
        public bool startTimeAfterSecond=false;
    }
    public class spawnEnergy:disrupterSpawnProps{
        public float energyNeeded=100;
        public float energy;
    }
    public class spawnPwrups:disrupterSpawnProps{
        public int pwrupsNeeded=2;
        public int pwrups;
    }
    public class spawnKills:disrupterSpawnProps{
        public float killsNeeded=50;
        public float kills;
    }
    public class spawnDmg:disrupterSpawnProps{
        public float dmgNeeded=200;
        public float dmg;
    }
    public class spawnCounts:disrupterSpawnProps{
        public WaveConfig countsWave;
        public float countsNeeded=3;
        public float counts;
    }*/
}
public enum disrupterType{time,energy,missed,pwrups,kills,dmg,counts}