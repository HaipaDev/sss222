using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameAssets : MonoBehaviour{
    public static GameAssets instance;
    [AssetsOnly]public GameObject powerupSpawnerPrefab;
    [AssetsOnly]public GameObject waveSpawnerPrefab;
    [AssetsOnly]public GameObject disrupterSpawnerPrefab;
	[AssetsOnly]public GObject[] objects;
	[AssetsOnly]public GObject[] vfx;
	[AssetsOnly]public GSprite[] sprites;
	[Header("Customization")]
	[AssetsOnly]public CstmzSkin[] skins;
	[AssetsOnly]public CstmzTrail[] trails;
    
    void Awake(){if(instance!=null){Destroy(gameObject);}else{DontDestroyOnLoad(gameObject);instance=this;}}

    public GameObject Make(string obj, Vector2 pos){
		GObject o = Array.Find(objects, item => item.name == obj);
		if (o == null){
			Debug.LogWarning("Object: " + name + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
        GameObject objref = Instantiate(gobj,pos,Quaternion.identity);
        return objref;
	}
    public GameObject MakeSpread(string obj, Vector2 pos, int amnt=3, float rangeX=0.5f, float rangeY=0.5f){
		GObject o = Array.Find(objects, item => item.name == obj);
		if (o == null){
			Debug.LogWarning("Object: " + name + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
		GameObject objref=Instantiate(gobj,pos,Quaternion.identity);
		for(var i=1;i<amnt-1;i++){
		var rndmX=UnityEngine.Random.Range(-rangeX,rangeX);
		var rndmY=UnityEngine.Random.Range(-rangeY,rangeY);
		var poss=pos+new Vector2(rndmX,rndmY);
        Instantiate(gobj,poss,Quaternion.identity);
		}
        return objref;
	}
    public GameObject VFX(string obj, Vector2 pos, float duration=0){
		GObject o = Array.Find(vfx, item => item.name == obj);
		if (o == null){
			Debug.LogWarning("Object: " + obj + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
        GameObject objref;
		if(SaveSerial.instance.settingsData.particles){
			objref=Instantiate(gobj,pos,Quaternion.identity);
			if(duration!=0)Destroy(objref,duration);
			return objref;
		}else return null;
	}
    public GameObject Get(string obj){
		GObject o = Array.Find(objects, item => item.name == obj);
		if (o == null){
			Debug.LogWarning("Object: " + obj + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
        return gobj;
	}public GameObject GetVFX(string obj){
		GObject o = Array.Find(vfx, item => item.name == obj);
		if (o == null){
			Debug.LogWarning("VFX: " + obj + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
		return gobj;
        //if(SaveSerial.instance.settingsData.particles)return gobj; else return null;
	}

    public Sprite Spr(string spr){
		GSprite s = Array.Find(sprites, item => item.name == spr);
		if (s == null){
			Debug.LogWarning("Sprite: " + spr + " not found!");
			return null;
		}
		Sprite gs=s.spr;
        return gs;
	}
	
	public CstmzSkin GetSkin(string str){
		CstmzSkin s = Array.Find(skins, item => item.name == str);
        //Sprite gs=s.str;
		//Sprite s=skins[i];
		if (s == null){
			Debug.LogWarning("Skin: " + str + " not found!");
			return null;
		}
        return s;
	}
	public CstmzSkinVariant GetSkinVariant(string str,int id){
		CstmzSkinVariant s = Array.Find(skins, item => item.name == str).variants[id];
		if (s == null){
			Debug.LogWarning("SkinVariant by id: " + id + " not found!");
			return null;
		}
        return s;
	}
	public int GetSkinID(string str){
		int i = Array.FindIndex(skins, item => item.name == str);
		if (Array.Find(skins,item => item.name == str) == null){
			Debug.LogWarning("Skin: " + str + " not found!");
			return 0;
		}
        return i;
	}public CstmzSkin GetSkinByID(int i){
		if (skins[i] == null){
			Debug.LogWarning("Skin by ID: " + i + " not found!");
			return null;
		}
        return skins[i];
	}

	
	public CstmzTrail GetTrail(string str){
		CstmzTrail s = Array.Find(trails, item => item.name == str);
		if (s == null){
			Debug.LogWarning("Trail: " + str + " not found!");
			return null;
		}
        return s;
	}
}

[System.Serializable]
public class GObject{
	public string name;
	public GameObject gobj;
}
[System.Serializable]
public class GSprite{
	public string name;
	public Sprite spr;
}
public enum CstmzRarity{def,common,rare,epic,legend}
public enum CstmzCategory{special,shop,reOne,twoPiece}
public enum CstmzType{skin,trail,flare,deathFx,music,bg}
[System.Serializable]
public class CstmzSkin{
	public string name;
	public Sprite spr;
	public Sprite sprOverlay;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
	public int variantSelected=-1;
	public CstmzSkinVariant[] variants;
}
[System.Serializable]
public class CstmzSkinVariant{
	public Sprite spr;
	public Sprite sprOverlay;
}
[System.Serializable]
public class CstmzTrail{
	public string name;
	public GameObject part;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
}