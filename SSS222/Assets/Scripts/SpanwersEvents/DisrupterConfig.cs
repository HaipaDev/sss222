using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Disrupter Config")]
public class DisrupterConfig:ScriptableObject{
    [HeaderAttribute("Properties")]
    public WaveConfig waveConfig;
    public disrupterType disrupterType;
    [SerializeReference]public disrupterSpawnProps spawnProps;
    [System.Serializable]public class disrupterSpawnProps{
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
    [System.Serializable]public class spawnEnergy:disrupterSpawnProps{
        public float energyNeeded=100;
        public float energy;
    }
    //[System.Serializable]public class spawnMissed:spawnEnergy{}
    [System.Serializable]public class spawnPwrups:disrupterSpawnProps{
        public int pwrupsNeeded=2;
        public int pwrups;
    }
    [System.Serializable]public class spawnKills:disrupterSpawnProps{
        public float killsNeeded=50;
        public float kills;
    }
    [System.Serializable]public class spawnDmg:disrupterSpawnProps{
        public float dmgNeeded=200;
        public float dmg;
    }
    [System.Serializable]public class spawnCounts:disrupterSpawnProps{
        public WaveConfig countsWave;
        public float countsNeeded=3;
        public float counts;
    }
    
    [ContextMenu("Validate")]void Vaildate(){
        if(disrupterType==disrupterType.time){spawnProps=new disrupterSpawnProps();spawnProps.secondEnabled=false;spawnProps.bothNeeded=false;}
        if(disrupterType==disrupterType.energy){spawnProps=new spawnEnergy();}
        if(disrupterType==disrupterType.missed){spawnProps=new spawnEnergy();}
        if(disrupterType==disrupterType.pwrups){spawnProps=new spawnPwrups();}
        if(disrupterType==disrupterType.kills){spawnProps=new spawnKills();}
        if(disrupterType==disrupterType.dmg){spawnProps=new spawnDmg();}
        if(disrupterType==disrupterType.counts){spawnProps=new spawnCounts();}
    }
}
public enum disrupterType{time,energy,missed,pwrups,kills,dmg,counts}