using System;
using System.Collections;
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
	[AssetsOnly]public CstmzFlares[] flares;
	[AssetsOnly]public CstmzDeathFx[] deathFxs;
	[AssetsOnly]public CstmzMusic[] musics;
    
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
	public CstmzFlares GetFlares(string str){
		CstmzFlares s = Array.Find(flares, item => item.name == str);
		if (s == null){
			Debug.LogWarning("Flares: " + str + " not found!");
			return null;
		}
        return s;
	}
	public GameObject GetFlareFirst(string str){
		CstmzFlares s = Array.Find(flares, item => item.name == str);
		if (s == null){
			Debug.LogWarning("Flares: " + str + " not found!");
			return null;
		}
        return s.parts[0];
	}
	public GameObject GetFlareRandom(string str){
		CstmzFlares s = Array.Find(flares, item => item.name == str);
		GameObject go=s.parts[UnityEngine.Random.Range(0,s.parts.Length)];
		if (s == null){
			Debug.LogWarning("Flares: " + str + " not found!");
			return null;
		}
        return go;
	}

	
	public CstmzDeathFx GetDeathFx(string str){
		CstmzDeathFx s = Array.Find(deathFxs, item => item.name == str);
		if (s == null){
			Debug.LogWarning("DeathFx: " + str + " not found!");
			return null;
		}
        return s;
	}

	public CstmzMusic GetMusic(string str){
		CstmzMusic s = Array.Find(musics, item => item.name == str);
		if (s == null){
			Debug.LogWarning("Music: " + str + " not found!");
			return null;
		}
        return s;
	}
	public CstmzMusic GetMusicRandom(string str){
		CstmzMusic s = musics[UnityEngine.Random.Range(0,musics.Length)];
		if (s == null){
			Debug.LogWarning("Music: " + str + " not found!");
			return null;
		}
        return s;
	}


	public void TransformIntoUIParticle(GameObject go,float sizeMult=0,float dur=-4){
		var ps=go.GetComponent<ParticleSystem>();var psMain=ps.main;
		if(sizeMult==0){
			if(ps.startSize<=1){sizeMult=100;}
			if(ps.startSize<=10&&ps.startSize>1){sizeMult=10;}
		}
		if(dur>0){Destroy(go,dur);}
		else if(dur==0){Destroy(go,psMain.duration);}
		else if(dur==-1){Destroy(go,psMain.duration*2);}
		var min=psMain.startSize.constantMin;var max=psMain.startSize.constantMax;
		if(min==0){min=max;}
		psMain.startSize=new ParticleSystem.MinMaxCurve(min*sizeMult, max*sizeMult);
		var psShape=ps.shape;psShape.radius*=sizeMult;
		Material mat=new Material(Shader.Find("UI Extensions/Particles/Additive"));mat.SetTexture("_MainTex",ps.GetComponent<Renderer>().material.GetTexture("_MainTex"));
		var psUI=go.AddComponent<UnityEngine.UI.Extensions.UIParticleSystem>();
		psUI.material=mat;
	}
	public void MakeParticleLooping(ParticleSystem pt){var ptMain=pt.main;ptMain.loop=true;ptMain.stopAction=ParticleSystemStopAction.None;}

	public class CoroutineWithData {
		public Coroutine coroutine { get; private set; }
		public object result;
		private IEnumerator target;
		public CoroutineWithData(MonoBehaviour owner, IEnumerator target) {
			this.target = target;
			this.coroutine = owner.StartCoroutine(Run());
		}
	
		private IEnumerator Run() {
			while(target.MoveNext()) {
				result = target.Current;
				yield return result;
			}
		}
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
[System.Serializable]
public class SimpleAnim{
	public Sprite spr;
	public float delay;
}

public enum CstmzRarity{def,common,rare,epic,legend}
public enum CstmzCategory{special,shop,reOne,twoPiece}
public enum CstmzType{skin,trail,flares,deathFx,music,bg}
[System.Serializable]
public class CstmzSkin{
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
public class CstmzSkinVariant{
	public Sprite spr;
	public Sprite sprOverlay;
}
[System.Serializable]
public class CstmzTrail{
	public string name;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
	[AssetsOnly]public GameObject part;
}
[System.Serializable]
public class CstmzFlares{
	public string name;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
	[AssetsOnly]public GameObject[] parts;
}
[System.Serializable]
public class CstmzDeathFx{
	public string name;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
	[AssetsOnly]public GameObject obj;
	public string sound="Death";
}
[System.Serializable]
public class CstmzMusic{
	public string name;
	public CstmzRarity rarity=CstmzRarity.common;
	public CstmzCategory category;
	public AudioClip track;
	public Sprite icon;
}