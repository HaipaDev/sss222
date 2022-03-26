using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Config")]
public class WeaponProperties:ScriptableObject{
    [SerializeField] new public string name;
    [SerializeField] public string assetName;
    [SerializeField] public weaponType weaponType;
    [SerializeReference] public weaponTypeProperties weaponTypeProperties;
    [SerializeField] public costType costType;
    [SerializeReference] public costTypeProperties costTypeProperties;
    [SerializeField] public float ovheat;
    [SerializeField] public float duration=10;
    [ContextMenu("Validate")]void Vaildate(){
        if(weaponType==weaponType.bullet){weaponTypeProperties=new weaponTypeBullet();}
        if(weaponType==weaponType.melee){weaponTypeProperties=new weaponTypeMelee();}
    }
    [ContextMenu("ValidateCost")]void VaildateCost(){
        if(costType==costType.energy){costTypeProperties=new costTypeEnergy();}
        if(costType==costType.ammo){costTypeProperties=new costTypeAmmo();}
        if(costType==costType.crystalAmmo){costTypeProperties=new costTypeCrystalAmmo();}
        if(costType==costType.blackEnergy){costTypeProperties=new costTypeBlackEnergy();}
    }
}
public enum costType{energy,ammo,crystalAmmo,blackEnergy}
[System.Serializable]public class costTypeProperties{public float cost;}
[System.Serializable]public class costTypeEnergy:costTypeProperties{}
[System.Serializable]public class costTypeAmmo:costTypeProperties{public int ammoSize=20;}
[System.Serializable]public class costTypeCrystalAmmo:costTypeProperties{public float regularEnergyCost=0;public int crystalCost=1;public int crystalAmmoCrafted=20;}
[System.Serializable]public class costTypeBlackEnergy:costTypeProperties{public float regularEnergyCost=0;}
[System.Serializable]public enum weaponType{bullet,melee}
[System.Serializable]public class weaponTypeProperties{}
[System.Serializable]public class weaponTypeBullet:weaponTypeProperties{
    public bool leftSide=true;
    public Vector2 leftAnchor=new Vector2(-0.35f,0);
    public Vector2 leftAnchorE=new Vector2(0,0);
    public bool rightSide=true;
    public Vector2 rightAnchor=new Vector2(0.35f,0);
    public Vector2 rightAnchorE=new Vector2(0,0);
    public bool randomSide=false;
    public int bulletAmount=1;
    public Vector2 speed=new Vector2(0,9);
    public Vector2 speedE=new Vector2(0,0);
    public Vector2 serialOffsetSpeed=new Vector2(0.55f,0);
    public Vector2 serialOffsetSpeedE=new Vector2(0,0);
    public Vector2 serialOffsetAngle=new Vector2(0,5);
    public float serialOffsetSound=0.03f;
    public float shootDelay=0.34f;
    public float holdDelayMulti=1;
    public float tapDelayMulti=1;
    public float recoilStrength=0;
    public float recoilTime=0;
    public bool flare=true;
    public float flareDur=0.3f;
}
[System.Serializable]public class weaponTypeMelee:weaponTypeProperties{
    public Vector2 offset=new Vector2(0,1);
    public float costPeriod=0.15f;
}