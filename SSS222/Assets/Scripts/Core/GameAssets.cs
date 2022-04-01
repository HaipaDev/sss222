using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameAssets : MonoBehaviour{	public static GameAssets instance;
	[Header("Special game-related")]
    [AssetsOnly]public GameObject powerupSpawnerPrefab;
    [AssetsOnly]public GameObject waveSpawnerPrefab;
    [AssetsOnly]public GameObject disrupterSpawnerPrefab;
	[Header("Main")]
	[AssetsOnly,Searchable]public GObject[] objects;
	[AssetsOnly,Searchable]public GObject[] vfx;
	[AssetsOnly,Searchable]public GSprite[] sprites;
	[AssetsOnly,Searchable]public PowerupItem[] powerupItems;
	[Header("Customization")]
	[AssetsOnly,Searchable]public CstmzSkin[] skins;
	[AssetsOnly,Searchable]public CstmzTrail[] trails;
	[AssetsOnly,Searchable]public CstmzFlares[] flares;
	[AssetsOnly,Searchable]public CstmzDeathFx[] deathFxs;
	[AssetsOnly,Searchable]public CstmzMusic[] musics;
    
    void Awake(){if(instance!=null){Destroy(gameObject);}else{DontDestroyOnLoad(gameObject);instance=this;}}

#region//Main
    public GameObject Make(string obj, Vector2 pos){
		GObject o=Array.Find(objects, item => item.name == obj);
		if(o==null){
			Debug.LogWarning("Object: " + name + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
        GameObject objref=Instantiate(gobj,pos,Quaternion.identity);
        return objref;
	}
    public GameObject MakeSpread(string obj, Vector2 pos, int amnt=3, float rangeX=0.5f, float rangeY=0.5f){
		GObject o=Array.Find(objects, item => item.name == obj);
		if(o==null){
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
		GObject o=Array.Find(vfx, item => item.name == obj);
		if(o==null){
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
		GObject o=Array.Find(objects, item => item.name == obj);
		if(o==null){
			Debug.LogWarning("Object: " + obj + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
        return gobj;
	}
	public GameObject GetVFX(string obj){
		GObject o=Array.Find(vfx, item => item.name == obj);
		if(o==null){
			Debug.LogWarning("VFX: " + obj + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
		return gobj;
        //if(SaveSerial.instance.settingsData.particles)return gobj; else return null;
	}
    public Sprite Spr(string spr){
		GSprite s=Array.Find(sprites, item => item.name == spr);
		if(s==null){
			Debug.LogWarning("Sprite: " + spr + " not found!");
			return null;
		}
		Sprite gs=s.spr;
        return gs;
	}
	public PowerupItem GetPowerupItem(string obj){
		PowerupItem o=Array.Find(powerupItems, item => item.name == obj);
		if(o==null){
			Debug.LogWarning("Powerup by name: " + obj + " not found!");
			return null;
		}
        return o;
	}
	public PowerupItem GetPowerupItemByAsset(string obj){
		PowerupItem o=Array.Find(powerupItems, item => item.assetName == obj);
		if(o==null){
			Debug.LogWarning("Powerup by assetName: " + obj + " not found!");
			return null;
		}
        return o;
	}
#endregion

#region//Customizables
	public CstmzSkin GetSkin(string str){
		CstmzSkin s=Array.Find(skins, item => item.name == str);
        //Sprite gs=s.str;
		//Sprite s=skins[i];
		if(s==null){
			Debug.LogWarning("Skin: " + str + " not found!");
			return null;
		}
        return s;
	}
	public CstmzSkinVariant GetSkinVariant(string str,int id){
		CstmzSkinVariant s=Array.Find(skins, item => item.name == str).variants[id];
		if(s==null){
			Debug.LogWarning("SkinVariant by id: " + id + " not found!");
			return null;
		}
        return s;
	}
	public int GetSkinID(string str){
		int i=Array.FindIndex(skins, item => item.name == str);
		if(Array.Find(skins,item => item.name == str)==null){
			Debug.LogWarning("Skin: " + str + " not found!");
			return 0;
		}
        return i;
	}public CstmzSkin GetSkinByID(int i){
		if(skins[i]==null){
			Debug.LogWarning("Skin by ID: " + i + " not found!");
			return null;
		}
        return skins[i];
	}

	public CstmzTrail GetTrail(string str){
		CstmzTrail s=Array.Find(trails, item => item.name == str);
		if(s==null){
			Debug.LogWarning("Trail: " + str + " not found!");
			return null;
		}
        return s;
	}

	public CstmzFlares GetFlares(string str){
		CstmzFlares s=Array.Find(flares, item => item.name == str);
		if(s==null){
			Debug.LogWarning("Flares: " + str + " not found!");
			return null;
		}
        return s;
	}
	public GameObject GetFlareFirst(string str){
		CstmzFlares s=Array.Find(flares, item => item.name == str);
		if(s==null){
			Debug.LogWarning("Flares: " + str + " not found!");
			return null;
		}
        return s.parts[0];
	}
	public GameObject GetFlareRandom(string str){
		CstmzFlares s=Array.Find(flares, item => item.name == str);
		GameObject go=s.parts[UnityEngine.Random.Range(0,s.parts.Length)];
		if(s==null){
			Debug.LogWarning("Flares: " + str + " not found!");
			return null;
		}
        return go;
	}

	public CstmzDeathFx GetDeathFx(string str){
		CstmzDeathFx s=Array.Find(deathFxs, item => item.name == str);
		if(s==null){
			Debug.LogWarning("DeathFx: " + str + " not found!");
			return null;
		}
        return s;
	}

	public CstmzMusic GetMusic(string str){
		CstmzMusic s=Array.Find(musics, item => item.name == str);
		if(s==null){
			Debug.LogWarning("Music: " + str + " not found!");
			return null;
		}
        return s;
	}
	public CstmzMusic GetMusicRandom(string str){
		CstmzMusic s=musics[UnityEngine.Random.Range(0,musics.Length)];
		if(s==null){
			Debug.LogWarning("Music: " + str + " not found!");
			return null;
		}
        return s;
	}
	
	public string GetDisplayName(string str,CstmzType cstmzType){
		string ss=null;
		if(cstmzType==CstmzType.skin){ss=Array.Find(skins,item=>item.name==str).displayName;}
		if(cstmzType==CstmzType.trail){ss=Array.Find(trails,item=>item.name==str).displayName;}
		if(cstmzType==CstmzType.flares){ss=Array.Find(flares,item=>item.name==str).displayName;}
		if(cstmzType==CstmzType.deathFx){ss=Array.Find(deathFxs,item=>item.name==str).displayName;}
		if(cstmzType==CstmzType.music){ss=Array.Find(musics,item=>item.name==str).displayName;}
		if(ss==null){
			Debug.LogWarning("DisplayName for: " + str + "of type("+cstmzType+") not found!");
			return null;
		}
        return ss;
	}

	/*public _CstmzTypable GetCustomizationTypeFromEnum<CstmzType>(){
		_CstmzTypable t;
		if(enType==CstmzType.skin){t=(CstmzSkin)type;}
		return t;
	}*/
	/*public string GetArtist<T>(string str) where T:_ArtistInfo{_ArtistInfo t=new _ArtistInfo();T s=(T)t;
		string ss=null;
		if(s.GetType()==typeof(CstmzSkin)){ss=Array.Find(skins,item=>item.name==str).artist;}
		if(s.GetType()==typeof(CstmzMusic)){ss=Array.Find(musics,item=>item.name==str).artist;}
		if(ss==null){
			Debug.LogWarning("Artist for: " + str + " not found!");
			return null;
		}
        return ss;
	}*/
	public string GetArtist(string str,CstmzType cstmzType){
		string ss=null;
		if(cstmzType==CstmzType.skin){ss=Array.Find(skins,item=>item.name==str).artist;}
		if(cstmzType==CstmzType.music){ss=Array.Find(musics,item=>item.name==str).artist;}
		if(ss==null){
			//Debug.LogWarning("Artist for: " + str + "of type("+cstmzType+") not found!");
			return null;
		}
        return ss;
	}
#endregion

#region//Public functions
	public void TransformIntoUIParticle(GameObject go,float mult=0,float dur=-4,bool multShape=false){
		var ps=go.GetComponent<ParticleSystem>();var psMain=ps.main;
		if(mult==0){
			if(ps.startSize<=1){mult=100;}
			if(ps.startSize<=10&&ps.startSize>1){mult=10;}
		}
		if(dur>0){Destroy(go,dur);}
		else if(dur==0){Destroy(go,psMain.startLifetime.constantMax+psMain.duration);}
		else if(dur==-1){Destroy(go,psMain.startLifetime.constantMax+psMain.duration*2);}
		var startSize=psMain.startSize;
		var sizeMin=startSize.constantMin;var sizeMax=startSize.constantMax;if(sizeMin==0){sizeMin=sizeMax;}
		var startSpeed=psMain.startSpeed;
		var speedMin=startSpeed.constantMin;var speedMax=startSpeed.constantMax;if(speedMin==0){speedMin=speedMax;}
		var startColor=new ParticleSystem.MinMaxGradient(psMain.startColor.colorMin,psMain.startColor.colorMax);
		var _color=startColor.colorMin;if(startColor.colorMin.r<0){_color=startColor.colorMax;}
		if(_color==Color.clear){_color=Color.white;}
		//psMain.startColor=new ParticleSystem.MinMaxGradient(_color,psMain.startColor.colorMax);
		var colorBySpeed=ps.colorBySpeed;
		var colorMin=colorBySpeed.range.x;var colorMax=colorBySpeed.range.y;
		psMain.startSize=new ParticleSystem.MinMaxCurve(sizeMin*mult, sizeMax*mult);
		psMain.startSpeed=new ParticleSystem.MinMaxCurve(speedMin*mult, speedMax*mult);
		//colorBySpeed.range=new Vector2(colorMin*mult, colorMax*mult);
		//colorBySpeed.range=new Vector2(colorMin*30, colorMax*30);
		var psShape=ps.shape;if(multShape){psShape.scale*=mult;}

		var psUI=go.AddComponent<UnityEngine.UI.Extensions.UIParticleSystem>();
		var _tex=ps.GetComponent<Renderer>().material.GetTexture("_MainTex");
		Material mat=new Material(Shader.Find("UI Extensions/Particles/Additive"));
		/*Debug.Log(go.name+" | ColorMin: "+startColor.colorMin);
		Debug.Log(go.name+" | ColorMax: "+startColor.colorMax);
		float H,S,V;Color.RGBToHSV(_color,out H,out S,out V);
		Debug.Log(go.name+" | _color: "+_color + " | HSV("+H+", "+S+", "+V+")");*/
		if(_isColorDark(_color)){
			//Debug.Log(go.name+" - IsDark");
			mat=new Material(Shader.Find("UI Extensions/Particles/Alpha Blended"));
		}
		mat.SetTexture("_MainTex",_tex);
		psUI.material=mat;
	}
	public static bool _isColorDark(Color color){bool b=false;float H,S,V;Color.RGBToHSV(color,out H,out S,out V);if(V<=0.3f){b=true;}return b;}
	public void MakeParticleLooping(ParticleSystem ps){var psMain=ps.main;psMain.loop=true;psMain.stopAction=ParticleSystemStopAction.None;}

	public class CoroutineWithData {
		public Coroutine coroutine { get; private set; }
		public object result;
		private IEnumerator target;
		public CoroutineWithData(MonoBehaviour owner, IEnumerator target){
			this.target=target;
			this.coroutine=owner.StartCoroutine(Run());
		}
	
		private IEnumerator Run(){
			while(target.MoveNext()){
				result=target.Current;
				yield return result;
			}
		}
 	}
#endregion
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
[System.Serializable]
public class SimpleAnim{
	public Sprite spr;
	public float delay;
}

public enum CstmzRarity{def,common,rare,epic,legend}
public enum CstmzCategory{special,shop,reOne,twoPiece}
public enum CstmzType{skin,trail,flares,deathFx,music,bg}
[System.Serializable]
public class _ArtistInfo{[PropertyOrder(-1)]public string artist;}
public interface _CstmzTypable{}
[System.Serializable]
public class CstmzSkin:_ArtistInfo,_CstmzTypable{
	[Header("Display")]
	[PropertyOrder(-2)]
	public string displayName;
	[Header("Properties")]
	public string name;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
	public bool animated=false;
	[HideIf("animated")]public Sprite spr;
	[ShowIf("animated")]public float animSpeed=0.05f;//Leave at 0 to make each frame speed controlable
	[ShowIf("animated")]public SimpleAnim[] animVals;
	public Sprite sprOverlay;
	//public int variantDefault=-1;
	public CstmzSkinVariant[] variants;
}
[System.Serializable]
public class CstmzSkinVariant:_CstmzTypable{
	public Sprite spr;
	public Sprite sprOverlay;
}
[System.Serializable]
public class CstmzTrail:_CstmzTypable{
	[Header("Display")]
	[PropertyOrder(-2)]
	public string displayName;

	[Header("Properties")]
	public string name;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
	[AssetsOnly]public GameObject part;
}
[System.Serializable]
public class CstmzFlares:_CstmzTypable{
	[Header("Display")]
	[PropertyOrder(-2)]
	public string displayName;

	[Header("Properties")]
	public string name;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
	[AssetsOnly]public GameObject[] parts;
}
[System.Serializable]
public class CstmzDeathFx:_CstmzTypable{
	[Header("Display")]
	[PropertyOrder(-2)]
	public string displayName;

	[Header("Properties")]
	public string name;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
	[AssetsOnly]public GameObject obj;
	public string sound="Death";
}
[System.Serializable]
public class CstmzMusic:_ArtistInfo,_CstmzTypable{
	[Header("Display")]
	[PropertyOrder(-2)]
	public string displayName;

	public static string _cstmzMusicDef="findYou";
	[Header("Properties")]
	public string name;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
	public AudioClip track;
	public Sprite icon;
}