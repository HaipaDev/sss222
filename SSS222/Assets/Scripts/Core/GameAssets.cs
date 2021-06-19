using System;
using UnityEngine;

public class GameAssets : MonoBehaviour{
//GameAssets.instance.Make("",);
//GameAssets.instance.Get("");
//GameAssets.instance.Spr("");
    public static GameAssets instance;
	public GObject[] objects;
	public GObject[] vfx;
	public GSprite[] sprites;
	public Sprite[] skins;
	public Sprite[] skinOverlays;
    
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
    public GameObject MakeSpread(string obj, Vector2 pos, int amnt, float rangeX=0.5f, float rangeY=0.5f){
		GObject o = Array.Find(objects, item => item.name == obj);
		if (o == null){
			Debug.LogWarning("Object: " + name + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
		GameObject objref=Instantiate(gobj,pos,Quaternion.identity);
		for(var i=1;i<amnt;i++){
		var rndmX=UnityEngine.Random.Range(-rangeX,rangeX);
		var rndmY=UnityEngine.Random.Range(-rangeY,rangeY);
		var poss=pos+new Vector2(rndmX,rndmY);
        Instantiate(gobj,poss,Quaternion.identity);
		}
        return objref;
	}
    public GameObject VFX(string obj, Vector2 pos, float duration){
		GObject o = Array.Find(vfx, item => item.name == obj);
		if (o == null){
			Debug.LogWarning("Object: " + name + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
        GameObject objref;
		if(SaveSerial.instance.settingsData.particles){
			objref=Instantiate(gobj,pos,Quaternion.identity);
			Destroy(objref,duration);
			return objref;
		}else return null;
	}
    public GameObject Get(string obj){
		GObject o = Array.Find(objects, item => item.name == obj);
		if (o == null){
			Debug.LogWarning("Object: " + name + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
        return gobj;
	}public GameObject GetVFX(string obj){
		GObject o = Array.Find(vfx, item => item.name == obj);
		if (o == null){
			Debug.LogWarning("Object: " + name + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
		return gobj;
        //if(SaveSerial.instance.settingsData.particles)return gobj; else return null;
	}

    public Sprite Spr(string spr){
		GSprite s = Array.Find(sprites, item => item.name == spr);
		if (s == null){
			Debug.LogWarning("Sprite: " + name + " not found!");
			return null;
		}
		Sprite gs=s.spr;
        return gs;
	}
	
	public Sprite GetSkin(int i){
		//GSprite s = Array.Find(skins, item => item.name == spr);
        //Sprite gs=s.spr;
		Sprite s=skins[i];
		if (s == null){
			Debug.LogWarning("Skin: " + i + " not found!");
			return null;
		}
        return s;
	}public Sprite GetOverlay(int i)
{
		//GSprite s = Array.Find(skins, item => item.name == spr);
        //Sprite gs=s.spr;
		Sprite s=skinOverlays[i];
		if (s == null){
			Debug.LogWarning("Overlay: " + i + " not found!");
			return null;
		}
        return s;
	}
}

[System.Serializable]
public class GObject {
	public string name;

	public GameObject gobj;
}
[System.Serializable]
public class GSprite {
	public string name;

	public Sprite spr;
}
