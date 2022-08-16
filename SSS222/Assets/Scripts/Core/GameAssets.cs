using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameAssets : MonoBehaviour{	public static GameAssets instance;
	[Header("Special game-related")]
    [AssetsOnly]public GameObject powerupSpawnerPrefab;
    [AssetsOnly]public GameObject waveSpawnerPrefab;
    [AssetsOnly]public GameObject disrupterSpawnerPrefab;
	[Header("Main")]
	[AssetsOnly,Searchable]public List<GObject> objects;
	[AssetsOnly,Searchable]public List<GObject> vfx;
	[AssetsOnly,Searchable]public List<GSprite> sprites;
	[AssetsOnly,Searchable]public List<GMaterial> materials;
	[AssetsOnly,Searchable][DisableInEditorMode]public List<PowerupItem> powerupItems;
	[AssetsOnly,Searchable][DisableInEditorMode]public List<WaveConfig> waveConfigs;
	[AssetsOnly,Searchable][DisableInEditorMode]public List<GObject> enemyBullets;
	[Header("Customization")]
	[AssetsOnly,Searchable]public List<CstmzSkin> skins;
	[AssetsOnly,Searchable]public List<CstmzTrail> trails;
	[AssetsOnly,Searchable]public List<CstmzFlares> flares;
	[AssetsOnly,Searchable]public List<CstmzDeathFx> deathFxs;
	[AssetsOnly,Searchable]public List<CstmzMusic> musics;
	[AssetsOnly,Searchable]public List<CstmzLockbox> lockboxes;
    
    void Awake(){if(instance!=null){Destroy(gameObject);}else{DontDestroyOnLoad(gameObject);instance=this;}}
	void Start(){
		var waveConfigsArr=Resources.LoadAll("ScriptableObjects/WaveConfigs", typeof(WaveConfig));
		foreach(UnityEngine.Object o in waveConfigsArr){waveConfigs.Add((WaveConfig)o);}
		var powerupItemsArr=Resources.LoadAll("ScriptableObjects/PowerupItems", typeof(PowerupItem));
		foreach(UnityEngine.Object o in powerupItemsArr){powerupItems.Add((PowerupItem)o);}
	}

#region//Main
    public GameObject Make(string str, Vector2 pos){
		GObject o=objects.Find(x=>x.name==str);
		if(o==null){
			Debug.LogWarning("Object: " + str + " not found!");
			return null;
		}
        GameObject objref=Instantiate(o.gobj,pos,Quaternion.identity);
        return objref;
	}
    public GameObject MakeSpread(string str, Vector2 pos, int amnt, float rangeX=0.5f, float rangeY=0.5f){
		GObject o=objects.Find(x=>x.name==str);
		if(o==null){
			Debug.LogWarning("Object: " + str + " not found!");
			return null;
		}
		GameObject objref=Instantiate(o.gobj,pos,Quaternion.identity);
		for(var i=1;i<amnt-1;i++){
		var rndX=UnityEngine.Random.Range(-rangeX,rangeX);
		var rndY=UnityEngine.Random.Range(-rangeY,rangeY);
		var poss=pos+new Vector2(rndX,rndY);
        Instantiate(o.gobj,poss,Quaternion.identity);
		}
        return objref;
	}
    public GameObject VFX(string str, Vector2 pos, float duration=0){
		GObject o=vfx.Find(x=>x.name==str);
		if(o==null){
			Debug.LogWarning("Object: " + str + " not found!");
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
    public GameObject Get(string str,bool ignoreWarning=false){
		GObject o=objects.Find(x=>x.name==str);
		if(o==null){
			if(!String.IsNullOrEmpty(str)&&!ignoreWarning)Debug.LogWarning("Object: " + str + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
        return gobj;
	}
	public Sprite GetObjSpr(string str,bool ignoreWarning=false){
		GObject o=objects.Find(x=>x.name==str);
		if(o==null){
			if(!String.IsNullOrEmpty(str)&&!ignoreWarning)Debug.LogWarning("Object: " + str + " not found!");
			return null;
		}
		Sprite spr=null;
		if(o.gobj.GetComponent<SpriteRenderer>()!=null)spr=o.gobj.GetComponent<SpriteRenderer>().sprite;
		if(o.gobj.GetComponent<UnityEngine.UI.Image>()!=null)spr=o.gobj.GetComponent<UnityEngine.UI.Image>().sprite;
        return spr;
	}
	public GameObject GetVFX(string str){
		GObject o=vfx.Find(x=>x.name==str);
		if(o==null){
			if(!String.IsNullOrEmpty(str))Debug.LogWarning("VFX: " + str + " not found!");
			return null;
		}
		GameObject gobj=o.gobj;
		return gobj;
        //if(SaveSerial.instance.settingsData.particles)return gobj; else return null;
	}
    public Sprite Spr(string str,bool ignoreWarning=false){
		GSprite gs=sprites.Find(x=>x.name==str);
		if(gs==null){
			if(!String.IsNullOrEmpty(name)&&!ignoreWarning)Debug.LogWarning("Sprite: " + str + " not found!");
			return null;
		}
		Sprite s=gs.spr;
        return s;
	}
    public Sprite SprAny(string str){Sprite _spr;
		_spr=Spr(str,true);
		if(_spr==null)_spr=GetObjSpr(str,true);
		if(_spr==null)Debug.LogWarning("Sprite not found in the library of sprites nor for the object by name: "+str);
        return _spr;
	}
    public GameObject GetEnemyBullet(string str){
		GObject gobj=enemyBullets.Find(x=>x.name==str);
		GameObject go=null;if(gobj!=null)go=gobj.gobj;
		if(go==null){
			if(!String.IsNullOrEmpty(str))Debug.LogWarning("Enemy Bullet by name: " + str + " not found! Trying to look for other objects");
			go=Get(str);
		}
        return go;
	}

	public Material GetMat(string mat,bool instantiate=false){
		GMaterial gm=materials.Find(x=>x.name==mat);
		if(gm==null){
			if(!String.IsNullOrEmpty(mat))Debug.LogWarning("Material: " + mat + " not found!");
			return null;
		}
		Material m=gm.mat;
		if(instantiate){m=Instantiate(m);}
        return m;
	}
	public Material UpdateShaderMatProps(Material mat,ShaderMatProps shaderMatProps,bool isUI=false){	Material _mat=mat;
		if(_mat!=null&&!_mat.shader.name.Contains("AllIn1SpriteShader")){
        	if(!isUI){if(GameAssets.instance.GetMat("AIOShaderMat")!=null)_mat=Instantiate(GameAssets.instance.GetMat("AIOShaderMat"));}
			else{if(GameAssets.instance.GetMat("AIOShaderMat_UI")!=null)_mat=Instantiate(GameAssets.instance.GetMat("AIOShaderMat_UI"));}
		}
		_mat.SetTexture("_MainTex",shaderMatProps.text);
		_mat.SetFloat("_HsvShift",shaderMatProps.hue*360);
        _mat.SetFloat("_HsvSaturation",shaderMatProps.saturation*2);
        _mat.SetFloat("_HsvBright",shaderMatProps.value*2);
        _mat.SetFloat("_NegativeAmount",shaderMatProps.negative);
        _mat.SetFloat("_PixelateSize",Mathf.Clamp(shaderMatProps.pixelate*512,4,512));
        _mat.SetFloat("_BlurIntensity",shaderMatProps.blur*100);
        _mat.SetFloat("_BlurHD",Convert.ToSingle(shaderMatProps.lowResBlur));
		return _mat;
	}
	public PowerupItem GetPowerupItem(string str){
		PowerupItem o=powerupItems.Find(x=>x.name==str);
		if(o==null){
			if(!String.IsNullOrEmpty(str))Debug.LogWarning("Powerup by name: " + str + " not found!");
			return null;
		}
        return o;
	}
	public PowerupItem GetPowerupItemByAsset(string str){
		PowerupItem o=powerupItems.Find(x=>x.assetName==str);
		if(o==null){
			if(!String.IsNullOrEmpty(str))Debug.LogWarning("Powerup by assetName: " + str + " not found!");
			return null;
		}
        return o;
	}
	public WaveConfig GetWaveConfig(string str){
		WaveConfig o=waveConfigs.Find(x=>x.name==str);
		if(o==null){
			if(!String.IsNullOrEmpty(str))Debug.LogWarning("WaveConfig by name: " + str + " not found!");
			return null;
		}
        return o;
	}
	public NotifUI FindNotifUIByType(notifUI_type type){
		List<NotifUI> list=new List<NotifUI>();foreach(NotifUI n in FindObjectsOfType<NotifUI>()){list.Add(n);}
        return list.Find(x=>x.type==type);
	}
#endregion

#region//Customizables
	public CstmzSkin GetSkin(string str){
		CstmzSkin s=skins.Find(x=>x.name==str);
        //Sprite gs=s.str;
		//Sprite s=skins[i];
		if(s==null){
			Debug.LogWarning("Skin: " + str + " not found!");
			return null;
		}
        return s;
	}
	public CstmzSkinVariant GetSkinVariant(string str,int id){
		CstmzSkinVariant s=skins.Find(x=>x.name==str).variants[id];
		if(s==null){
			Debug.LogWarning("SkinVariant by id: " + id + " not found!");
			return null;
		}
        return s;
	}
	public int GetSkinID(string str){
		int i=skins.FindIndex(x=>x.name==str);
		if(skins.Find(x=>x.name==str)==null){
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
		CstmzTrail s=trails.Find(x=>x.name==str);
		if(s==null){
			Debug.LogWarning("Trail: " + str + " not found!");
			return null;
		}
        return s;
	}

	public CstmzFlares GetFlares(string str){
		CstmzFlares s=flares.Find(x=>x.name==str);
		if(s==null){
			Debug.LogWarning("Flares: " + str + " not found!");
			return null;
		}
        return s;
	}
	public GameObject GetFlareFirst(string str){
		CstmzFlares s=flares.Find(x=>x.name==str);
		if(s==null){
			Debug.LogWarning("Flares: " + str + " not found!");
			return null;
		}
        return s.parts[0];
	}
	public GameObject GetFlareRandom(string str){
		CstmzFlares s=flares.Find(x=>x.name==str);
		GameObject go=s.parts[UnityEngine.Random.Range(0,s.parts.Length)];
		if(s==null){
			Debug.LogWarning("Flares: " + str + " not found!");
			return null;
		}
        return go;
	}

	public CstmzDeathFx GetDeathFx(string str){
		CstmzDeathFx s=deathFxs.Find(x=>x.name==str);
		if(s==null){
			Debug.LogWarning("DeathFx: " + str + " not found!");
			return null;
		}
        return s;
	}

	public CstmzMusic GetMusic(string str){
		CstmzMusic s=musics.Find(x=>x.name==str);
		if(s==null){
			Debug.LogWarning("Music: " + str + " not found!");
			return null;
		}
        return s;
	}
	public CstmzMusic GetMusicRandom(string str){
		CstmzMusic s=musics[UnityEngine.Random.Range(0,musics.Count)];
		if(s==null){
			Debug.LogWarning("Music: " + str + " not found!");
			return null;
		}
        return s;
	}
	
	public string GetDisplayName(string str,CstmzType cstmzType){
		string ss=null;
		if(cstmzType==CstmzType.skin){ss=skins.Find(x=>x.name==str).displayName;}
		if(cstmzType==CstmzType.trail){ss=trails.Find(x=>x.name==str).displayName;}
		if(cstmzType==CstmzType.flares){ss=flares.Find(x=>x.name==str).displayName;}
		if(cstmzType==CstmzType.deathFx){ss=deathFxs.Find(x=>x.name==str).displayName;}
		if(cstmzType==CstmzType.music){ss=musics.Find(x=>x.name==str).displayName;}
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
		if(s.GetType()==typeof(CstmzSkin)){ss=Array.Find(skins,x=>x.name==str).artist;}
		if(s.GetType()==typeof(CstmzMusic)){ss=Array.Find(musics,x=>x.name==str).artist;}
		if(ss==null){
			Debug.LogWarning("Artist for: " + str + " not found!");
			return null;
		}
        return ss;
	}*/
	public string GetArtist(string str,CstmzType cstmzType){
		string ss=null;
		if(cstmzType==CstmzType.skin){ss=skins.Find(x=>x.name==str).artist;}
		if(cstmzType==CstmzType.music){ss=musics.Find(x=>x.name==str).artist;}
		if(ss==null){
			//Debug.LogWarning("Artist for: " + str + "of type("+cstmzType+") not found!");
			return null;
		}
        return ss;
	}
	public static bool CaseInsStrCmpr(string str,string toComp){return str.IndexOf(toComp, StringComparison.OrdinalIgnoreCase) >= 0;}
	public static int BoolToInt(bool b){if(b){return 1;}else{return 0;}}
#endregion

#region//Public functions
	public void TransformIntoUIParticle(GameObject go,float mult=0,float dur=-4,bool multShape=false){
		if(go.GetComponent<UnityEngine.UI.Extensions.UIParticleSystem>()==null){
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
			psUI.raycastTarget=false;
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
	}
	public static Quaternion QuatRotateTowards(Vector3 target, Vector3 curPos, float rotModif=90){
		Vector3 vectorToTarget = target - curPos;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotModif;
		return Quaternion.AngleAxis(angle, Vector3.forward);
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
	//public static float DeltaPercentMinMax(float cur,float max){return (cur-max)/100;}
	public static float Normalize(float val,float min,float max){return (val-min)/(max-min);}
	public static float InvertNormalized(float val){return 1-val;}
	public static float InvertNormalizedAbs(float val){return Mathf.Abs(InvertNormalized(val));}
	public static float InvertNormalizedMin(float val,float min){return InvertNormalized(val)*min;}
	public static float RandomFloat(float min,float max){return (float)System.Math.Round(UnityEngine.Random.Range(min, max), 2);}
	public static bool CheckChance(float chance){return chance>=RandomFloat(0f,100f);}

	

    public static void SetActiveAllChildren(Transform transform, bool value){
        foreach (Transform child in transform){
            child.gameObject.SetActive(value);
 
            SetActiveAllChildren(child, value);
        }
    }
#endregion
}

[System.Serializable]
public class GObject{
	public string name;
	[AssetsOnly]public GameObject gobj;
}
[System.Serializable]
public class GSprite{
	public string name;
	public Sprite spr;
}
[System.Serializable]
public class GTextSprite{
	public string name;
	public Texture2D text;
	public Rect rect;
	public Vector2 pivot;
}
[System.Serializable]
public class GMaterial{
	public string name;
	public Material mat;
}
[System.Serializable]
public class ShaderMatProps{
	//public string name;
	public Texture2D text;
    [Range(0,1)]public float hue=0;
    [Range(0,1)]public float saturation=0.5f;
    [Range(0,1)]public float value=0.5f;
    [Range(0,1)]public float negative=0;
    [Range((4/512),1)]public float pixelate=1;
    [Range(0,1)]public float blur=0;
    public bool lowResBlur=true;
}
[System.Serializable]
public class SimpleAnim{
	public Sprite spr;
	public float delay=0.02f;
}
[System.Serializable]
public class ListOfSimpleAnims{
	public string name;
	public List<SimpleAnim> anim;
}
[System.Serializable]
public class TransformAndPos{
	public Transform trans;
	public Vector2 pos;
}
[System.Serializable]
public class RectTransformAndPos{
	public RectTransform trans;
	public Vector2 pos;
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
	[ShowIf("animated")]public List<SimpleAnim> animVals;
	public Sprite sprOverlay;
	//public int variantDefault=-1;
	public List<CstmzSkinVariant> variants;
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
[System.Serializable]
public class CstmzLockbox:_CstmzTypable{
	[Header("Display")]
	public string displayName;
	[Header("Properties")]
	public string name;
	public Sprite icon;
	//public List<CstmzSkin> skinDrops;
}