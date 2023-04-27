using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;

public class SaveSerial : MonoBehaviour{	public static SaveSerial instance;
	void Awake(){if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);gameObject.name=gameObject.name.Split('(')[0];}}
	IEnumerator Start(){
		yield return new WaitForSecondsRealtime(0.01f);
		playerData.highscore=new Highscore[CoreSetup.GetGamerulesetsPrefabsLength()];
		for(int i=0;i<playerData.highscore.Length;i++){playerData.highscore[i]=new Highscore();}
		//playerData.achievsCompleted=new AchievData[StatsAchievsManager._AchievsListCount()];
		statsData.statsGamemodesList=new StatsGamemode[StatsAchievsManager.GetStatsGMListCount()];
		
		if(String.IsNullOrEmpty(playerData.skinName)||AssetsManager.instance.GetSkin(playerData.skinName)==null){playerData.skinName="def";}
		if(String.IsNullOrEmpty(playerData.trailName)||AssetsManager.instance.GetTrail(playerData.trailName)==null){playerData.trailName="def";}
		if(String.IsNullOrEmpty(playerData.flaresName)||AssetsManager.instance.GetFlares(playerData.flaresName)==null){playerData.flaresName="def";}
		if(String.IsNullOrEmpty(playerData.deathFxName)||AssetsManager.instance.GetDeathFx(playerData.deathFxName)==null){playerData.deathFxName="def";}
		if(String.IsNullOrEmpty(playerData.musicName)||AssetsManager.instance.GetMusic(playerData.musicName)==null){playerData.musicName=CstmzMusic._cstmzMusicDef;}

		foreach(CstmzLockbox lb in AssetsManager.instance.lockboxes){if(!playerData.lockboxesInventory.Exists(x=>x.name==lb.name)){playerData.lockboxesInventory.Add(new LockboxCount{name=lb.name,count=0});}}

		/*settingsData.masterVolume=Mathf.Clamp(settingsData.masterVolume,0,2);
		settingsData.soundVolume=Mathf.Clamp(settingsData.soundVolume,0,2);
		settingsData.ambienceVolume=Mathf.Clamp(settingsData.ambienceVolume,0,2);
		settingsData.musicVolume=Mathf.Clamp(settingsData.musicVolume,0,2);*/
	}
	[SerializeField] string filenameLogin = "hyperGamerLogin";
	[SerializeField] string filename = "playerData";
	[SerializeField] string filenameStats = "statsData";
	[SerializeField] string filenameAdventure = "adventureData";
	[SerializeField] string filenameSettings = "gameSettings";
	public static int maxRegisteredHyperGamers=3;

#region//HyperGamerLogin
	public HyperGamerLoginData hyperGamerLoginData=new HyperGamerLoginData();
	[System.Serializable]public class HyperGamerLoginData{
		public int registeredCount;
		public bool loggedIn;
		public string username;
		public string password;
		public DateTime lastLoggedIn;
	}
	public string _loginDataPath(){return Application.persistentDataPath+"/"+filenameLogin+".hyper";}
	public void SetLogin(string username, string password){
		hyperGamerLoginData.loggedIn=true;
		hyperGamerLoginData.username=username;
		hyperGamerLoginData.password=password;
		hyperGamerLoginData.lastLoggedIn=DateTime.Now;
		Debug.Log("Login data set");
	}
	public void LogOut(){
		hyperGamerLoginData.loggedIn=false;
		hyperGamerLoginData.username="";
		hyperGamerLoginData.password="";
		Debug.Log("Logged out");
	}
	public void SaveLogin(){
		var settings=new ES3Settings(_loginDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
		ES3.Save("hyperGamerLoginData",hyperGamerLoginData,settings);
		Debug.Log("Login saved");
	}
	public void LoadLogin(){
		if(ES3.FileExists(_loginDataPath())){
			var settings=new ES3Settings(_loginDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
			if(ES3.KeyExists("hyperGamerLoginData",settings)){ES3.LoadInto<HyperGamerLoginData>("hyperGamerLoginData",hyperGamerLoginData,settings);}
			else{Debug.LogWarning("Key for hyperGamerLoginData not found in: "+_loginDataPath());}
		}else Debug.LogWarning("Login Data file not found in "+_loginDataPath());
	}
	void AutoLogin(){
		//TimeSpan tsSession=DateTime.Now.Subtract(hyperGamerLoginData.lastLoggedIn);
		//if(tsSession.TotalDays>=14){LogOut();}
		//else{
			TryLogin(hyperGamerLoginData.username,hyperGamerLoginData.password);
		//}
	}
	public void TryLogin(string username, string password){
		//try{
			if(DBAccess.instance!=null){if(hyperGamerLoginData.username!="")DBAccess.instance.LoginHyperGamer(hyperGamerLoginData.username,hyperGamerLoginData.password);}
			else{Debug.Log("No DBAccess, cant try to login");}
		//}catch{}
	}
#endregion
#region//Player Data
	public PlayerData playerData=new PlayerData();
	public float buildFirstLoaded;
	public float buildLastLoaded;
	[System.Serializable]public class PlayerData{
		public Highscore[] highscore=new Highscore[0];
		public string skinName="def";
		public Color overlayColor=Color.white;
		public string trailName="def";
		public string flaresName="def";
		public string deathFxName="def";
		public string musicName=CstmzMusic._cstmzMusicDef;
		public int dynamCelestStars;
		public int starshards;
		public List<LockboxCount> lockboxesInventory;
		public List<string> skinsUnlocked;
		public List<string> trailsUnlocked;
		public List<string> flaresUnlocked;
		public List<string> deathFxUnlocked;
		public List<string> musicUnlocked;
		public AchievData[] achievsCompleted=new AchievData[0];
	}

	public string _playerDataPath(){return Application.persistentDataPath+"/"+filename+".hyper";}
	public void Save(){
        var settings=new ES3Settings(_playerDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
		if(!ES3.KeyExists("buildFirstLoaded",settings)){buildFirstLoaded=GameManager.instance.buildVersion;ES3.Save("buildFirstLoaded",buildFirstLoaded,settings);}
		buildLastLoaded=GameManager.instance.buildVersion;ES3.Save("buildLastLoaded",buildLastLoaded,settings);
		ES3.Save("playerData",playerData,settings);
		Debug.Log("Game Data saved");
	}
	public void Load(){
		if(ES3.FileExists(_playerDataPath())){
			var settings=new ES3Settings(_playerDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);

			if(ES3.KeyExists("buildFirstLoaded",settings)){buildFirstLoaded=ES3.Load<float>("buildFirstLoaded",8,settings);}
			else{Debug.LogWarning("Key for buildFirstLoaded not found in: "+_playerDataPath());}

			if(ES3.KeyExists("buildLastLoaded",settings)){buildLastLoaded=ES3.Load<float>("buildLastLoaded",8,settings);}
			else{Debug.LogWarning("Key for buildLastLoaded not found in: "+_playerDataPath());}

			if(ES3.KeyExists("playerData",settings)){ES3.LoadInto<PlayerData>("playerData",playerData,settings);}
			else{Debug.LogWarning("Key for playerData not found in: "+_playerDataPath());}
			//var hi=-1;foreach(int h in playerData.highscore){hi++;if(h!=0)playerData.highscore[hi]=h;}
			Debug.Log("Game Data loaded");
		}else Debug.LogWarning("Game Data file not found in: "+_playerDataPath());
	}
	public void Delete(){
		playerData=new PlayerData(){highscore=new Highscore[CoreSetup.GetGamerulesetsPrefabsLength()]/*,achievsCompleted=new AchievData[StatsAchievsManager._AchievsListCount()]*/};
		Debug.Log("Game Data reset");
		GC.Collect();
		if(ES3.FileExists(_playerDataPath())){
			ES3.DeleteFile(_playerDataPath());
			Debug.Log("Game Data deleted!");
		}
	}
#endregion
#region//Stats Data
	public StatsData statsData=new StatsData();
	[System.Serializable]public class StatsData{
		public StatsGamemode[] statsGamemodesList=new StatsGamemode[0];
		public float sandboxTime=0;
		public List<string> uniquePowerups;
	}

	public string _statsDataPath(){return Application.persistentDataPath+"/"+filenameStats+".hyper";}
	public void SaveStats(){
		var settings=new ES3Settings(_statsDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
		ES3.Save("statsData",statsData,settings);
		Debug.Log("Stats Data saved");
	}
	public void LoadStats(){
		if(ES3.FileExists(_statsDataPath())){
			var settings=new ES3Settings(_statsDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
			if(ES3.KeyExists("statsData",settings)){ES3.LoadInto<StatsData>("statsData",statsData,settings);Debug.Log("Stats data loaded");}
			else Debug.LogWarning("Key for statsData not found in: "+_statsDataPath());
		}else Debug.LogWarning("Stats Data file not found in: "+_statsDataPath());
	}
	public void DeleteStats(){
		statsData=new StatsData(){statsGamemodesList=new StatsGamemode[StatsAchievsManager.GetStatsGMListCount()]};
		Debug.Log("Stats Data reset");
		GC.Collect();
		if(ES3.FileExists(_statsDataPath())){
			ES3.DeleteFile(_statsDataPath());
			Debug.Log("Stats Data deleted!");
		}
	}
#endregion
#region //Adventure Data
	public AdventureData advD=new AdventureData();
	[System.Serializable]public class AdventureData{
		public float buildLastSaved;
		public int cores=0;
		public float xp=0;
		public bool _coreSpawnedPreAscend=false;
		public int zoneSelected=0;
		public int zoneToTravelTo=-1;
		public float travelTimeLeft=-4;
		//public float GameManagerTime;
		public List<string> defeatedBosses;
		public List<string> lockedZones;

		public int holo_crystalsStored=0;
		public Powerup holo_powerupStored=null;
		public int holo_timeAt=0;
		public float holo_posX=0;
		public float holo_zoneSelected=0;
		public int distanceTraveled;
		public int breakWaveCount;
		public bool calledBreak;
		
		public float health=0;
		public float healthStart=0;
		public float hpAbsorpAmnt=0;
		public float energy=0;
		public float enAbsorpAmnt=0;
		public List<Powerup> powerups;
		public int powerupCurID=0;
		public List<StatusFx> statuses=new List<StatusFx>();

		public int shipLvl=0;
		public int shipLvlFraction=0;
		public bool autoAscend=true;
		public bool autoLvl=true;
		public List<string> moduleSlots=new List<string>();
		public List<string> skillsSlots=new List<string>(2);
		public List<Module> modulesList=new List<Module>();
		public List<Skill> skillsList=new List<Skill>();
		public int bodyUpgraded;
		public int engineUpgraded;
		public int blastersUpgraded;
	}

	public string _advDataPath(){return Application.persistentDataPath+"/"+filenameAdventure+".hyper";}
	public void SaveAdventure(){
		//var settings=new ES3Settings(_advDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
		var settings=new ES3Settings(_advDataPath(),ES3.EncryptionType.None);
		ES3.Save("advData",advD,settings);
		Debug.Log("Adventure Data saved");
	}
	public void LoadAdventure(){
		if(ES3.FileExists(_advDataPath())){
			//var settings=new ES3Settings(_advDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
			var settings=new ES3Settings(_advDataPath(),ES3.EncryptionType.None);
			if(ES3.KeyExists("advData",settings))ES3.LoadInto<AdventureData>("advData",advD,settings);
			else Debug.LogWarning("Key for advData not found in: "+_advDataPath());
		}else Debug.LogWarning("Adventure Data file not found in: "+_advDataPath());
	}
	public void ResetAdventure(){
		if(advD==null){Debug.LogError("AdventureData = null");}else{Debug.Log("AdventureData not empty");}
		advD=new AdventureData();
		Debug.Log("Adventure Data reset");
	}
	public void DeleteAdventure(){
		ResetAdventure();
		GC.Collect();
		if(ES3.FileExists(_advDataPath())){
			ES3.DeleteFile(_advDataPath());
			Debug.Log("Adventure Data deleted");
		}
	}
#endregion
#region//Settings Data
	public string lastEditedSandbox;
	public SettingsData settingsData=new SettingsData();
	[System.Serializable]public class SettingsData{
		public InputType inputType;
		public JoystickType joystickType;
		public float joystickSize=1;
		public bool lefthand;
		public bool dtapMouseShoot;
		public bool scbuttons;
		public bool vibrations=true;
		public bool pauseWhenOOF=true;
		public bool discordRPC=true;
		public bool autoselectNewItem=true;
		public bool alwaysReplaceCurrentSlot=false;
		public bool autoUseMedkitsIfLow=true;
		public bool allowSelectingEmptySlots=true;
		public bool allowScrollingEmptySlots=false;
		public bool autosubmitScores=true;
		
		public float masterVolume=0.95f;
		public float masterOOFVolume=0.25f;
		public float soundVolume=0.95f;
		public float ambienceVolume=-0.55f;
		public float musicVolume=0.66f;
		public bool windDownMusic=true;
		public bool bossVolumeTurnUp=true;
		
		
		public PlaneDir playfieldRot=PlaneDir.vert;
		public int windowMode=1;
		public Vector2Int resolution=new Vector2Int(1920,1080);
		public bool vSync=false;
		public bool lockCursor=false;
		public int quality=4;
		public bool pprocessing;
		public bool screenshake=true;
		public bool dmgPopups=true;
		public bool particles=true;		
		public bool screenflash=true;
		
		public bool classicHUD=false;
		public bool playerWeaponsFade=true;
		public float hudVis_graphics=0.9f;
		public float hudVis_barText=1f;
		public float hudVis_text=1f;
		public float hudVis_barFill=1f;
		public float hudVis_absorpFill=0.5f;
		public float hudVis_popups=0.9f;
		public float hudVis_notif=1f;

		public float popupSumTime=0.25f;
		public bool hpPopupsSum;
		public bool hpAbsorpPopupsSum;
		public bool enPopupsSum;
		public bool enAbsorpPopupsSum;
		public bool ammoPopupsSum;
		public bool xpPopupsSum;
		public bool coinPopupsSum;
		public bool corePopupsSum;
		public bool scorePopupsSum;
	}
	
	public string _settingsDataPath(){return Application.persistentDataPath+"/"+filenameSettings+".json";}
	public void SaveSettings(){
		var settings=new ES3Settings(_settingsDataPath(),ES3.EncryptionType.None);
		ES3.Save("settingsData",settingsData,settings);
		ES3.Save("lastEditedSandbox",lastEditedSandbox,settings);
		Debug.Log("Settings saved");
	}
	public void LoadSettings(){
		if(ES3.FileExists(_settingsDataPath())){
		var settings=new ES3Settings(_settingsDataPath(),ES3.EncryptionType.None);
			if(ES3.KeyExists("settingsData",settings)){ES3.LoadInto<SettingsData>("settingsData",settingsData,settings);}
			else{Debug.LogWarning("Key for settingsData not found in: "+_settingsDataPath());}

			if(ES3.KeyExists("lastEditedSandbox",settings)){lastEditedSandbox=ES3.Load<string>("lastEditedSandbox",settings);}
			else{Debug.LogWarning("Key for lastEditedSandbox not found in: "+_settingsDataPath());}
		}else Debug.LogWarning("Settings file not found in: "+_settingsDataPath());
	}
	public void DeleteSettings(){
		settingsData=new SettingsData();
		GC.Collect();
		if(ES3.FileExists(_settingsDataPath())){
			ES3.DeleteFile(_settingsDataPath());
			Debug.Log("Settings deleted");
		}
	}
#endregion
}

[System.Serializable]
public class Highscore{
	public int score;
	public int playtime;
	public string version;
	public float build;
	public DateTime date;
}
[System.Serializable]
public class LockboxCount{public string name;public int count;}