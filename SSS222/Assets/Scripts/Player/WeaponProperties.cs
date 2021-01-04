using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponProperties:ISerializationCallbackReceiver {
    [SerializeField] public string name;
    [SerializeField] public string assetName;
    [SerializeField] public weaponType weaponType;
    [SerializeReference] public weaponTypeProperties weaponTypeProperties;
    /*[SerializeField] public T GenericMethod<T>(T param){
        if(weaponType==weaponType.held){}
        return param;
    }*/
    [SerializeField] public costType costType;
    [SerializeField] public float ammoSize;
    [SerializeField] public float cost;
    [SerializeField] public float ovheat;
    void ISerializationCallbackReceiver.OnBeforeSerialize()=>this.OnValidate();
    void ISerializationCallbackReceiver.OnAfterDeserialize(){}
    void OnValidate(){
        if(weaponType==weaponType.bulletSingle){weaponTypeProperties=new weaponTypeBulletSingle();}
        if(weaponType==weaponType.bulletDouble){weaponTypeProperties=new weaponTypeBulletDouble();}
        if(weaponType==weaponType.bulletMore){weaponTypeProperties=new weaponTypeBulletMore();}
        if(weaponType==weaponType.held){weaponTypeProperties=new weaponTypeHeld();}
        //weaponTypeProperties=GenericMethod<weaponType>(weaponType);
    }
}
public enum costType{energy,ammo}
public enum weaponType{bulletSingle,bulletDouble,bulletMore,held}
[System.Serializable]public class weaponTypeProperties{}
[System.Serializable]public class weaponTypeBulletSingle:weaponTypeProperties{
    public bool isDouble=false;
}
[System.Serializable]public class weaponTypeBulletDouble:weaponTypeProperties{
    public bool isDouble=true;
}
[System.Serializable]public class weaponTypeBulletMore:weaponTypeProperties{
    public int bulletAmmount;
}
[System.Serializable]public class weaponTypeHeld:weaponTypeProperties{
    
}