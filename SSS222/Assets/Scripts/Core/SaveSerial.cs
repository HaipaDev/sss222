using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Encoders;
using BayatGames.SaveGameFree.Serializers;

public class SaveSerial : MonoBehaviour{	public static SaveSerial instance;
	void Awake(){if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
	IEnumerator Start(){
		yield return new WaitForSecondsRealtime(0.01f);
		playerData.highscore=new Highscore[GameCreator.GetGamerulesetsPrefabsLength()];
		for(int i=0;i<playerData.highscore.Length;i++){playerData.highscore[i]=new Highscore();}
		//playerData.achievsCompleted=new AchievData[StatsAchievsManager._AchievsListCount()];
		statsData.statsGamemodesList=new StatsGamemode[StatsAchievsManager.GetStatsGMListCount()];
		
		if(String.IsNullOrEmpty(playerData.skinName)||GameAssets.instance.GetSkin(playerData.skinName)==null){playerData.skinName="def";}
		if(String.IsNullOrEmpty(playerData.trailName)||GameAssets.instance.GetTrail(playerData.trailName)==null){playerData.trailName="def";}
		if(String.IsNullOrEmpty(playerData.flaresName)||GameAssets.instance.GetFlares(playerData.flaresName)==null){playerData.flaresName="def";}
		if(String.IsNullOrEmpty(playerData.deathFxName)||GameAssets.instance.GetDeathFx(playerData.deathFxName)==null){playerData.deathFxName="def";}
		if(String.IsNullOrEmpty(playerData.musicName)||GameAssets.instance.GetMusic(playerData.musicName)==null){playerData.musicName=CstmzMusic._cstmzMusicDef;}

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
		if(File.Exists(Application.persistentDataPath+"/"+filenameLogin)){//Legacy loading
			SaveGame.Encode=false;SaveGame.Serializer=new SaveGameJsonSerializer();
			hyperGamerLoginData = SaveGame.Load<HyperGamerLoginData>(filenameLogin);

			File.Delete(Application.persistentDataPath+"/"+filenameLogin);
			Debug.Log("Login (legacy) loaded and replaced");
			AutoLogin();
			SaveLogin();
		}
		if(ES3.FileExists(_loginDataPath())){
			var settings=new ES3Settings(_loginDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
			if(ES3.KeyExists("hyperGamerLogin",settings))ES3.LoadInto<HyperGamerLoginData>("hyperGamerLoginData",settings);
		}else Debug.LogWarning("Login Data file not found in "+Application.persistentDataPath+"/"+filenameLogin);
	}
	void AutoLogin(){
		//TimeSpan tsSession=DateTime.Now.Subtract(hyperGamerLoginData.lastLoggedIn);
		if(false/*tsSession.TotalDays>=14*/){LogOut();}
		else{TryLogin(hyperGamerLoginData.username,hyperGamerLoginData.password);}
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
		public Highscore[] highscore=new Highscore[GameCreator.GetGamerulesetsPrefabsLength()];
		public string skinName="def";
		public Color overlayColor=Color.white;
		public string trailName="def";
		public string flaresName="def";
		public string deathFxName="def";
		public string musicName=CstmzMusic._cstmzMusicDef;
		public AchievData[] achievsCompleted=new AchievData[0];
	}

	public string _playerDataPath(){return Application.persistentDataPath+"/"+filename+".hyper";}
	public void Save(){
        var settings=new ES3Settings(_playerDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
		if(!ES3.KeyExists("buildFirstLoaded",settings))ES3.Save("buildFirstLoaded",GameSession.instance.buildVersion,settings);
		ES3.Save("buildLastLoaded",GameSession.instance.buildVersion,settings);
		ES3.Save("playerData",playerData,settings);
		Debug.Log("Game Data saved");
	}
	public void Load(){
		if(File.Exists(Application.persistentDataPath+"/"+filename)){//Legacy loading
			SaveGame.Encode=false;SaveGame.Serializer=new SaveGameJsonSerializer();
			playerData = SaveGame.Load<PlayerData>(filename);
			var hi=-1;foreach(Highscore h in playerData.highscore){hi++;if(h.score!=0)playerData.highscore[hi]=h;}

			File.Delete(Application.persistentDataPath+"/"+filename);
			Debug.Log("Game Data (legacy) loaded and replaced");
			Save();
		}
		if(ES3.FileExists(_playerDataPath())){
			var settings=new ES3Settings(_playerDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
			if(ES3.KeyExists("buildFirstLoaded",settings))buildFirstLoaded=ES3.Load<float>("buildFirstLoaded",settings);
			else Debug.LogWarning("Key for buildFirstLoaded not found in: "+_playerDataPath());
			if(ES3.KeyExists("buildLastLoaded",settings))buildLastLoaded=ES3.Load<float>("buildLastLoaded",settings);
			else Debug.LogWarning("Key for buildLastLoaded not found in: "+_playerDataPath());
			if(ES3.KeyExists("playerData",settings))ES3.LoadInto<PlayerData>("playerData",playerData,settings);
			else Debug.LogWarning("Key for playerData not found in: "+_playerDataPath());
			//var hi=-1;foreach(int h in playerData.highscore){hi++;if(h!=0)playerData.highscore[hi]=h;}
			Debug.Log("Game Data loaded");
		}else Debug.LogWarning("Game Data file not found in: "+_playerDataPath());
	}
	public void Delete(){
		playerData=new PlayerData(){highscore=new Highscore[GameCreator.GetGamerulesetsPrefabsLength()]/*,achievsCompleted=new AchievData[StatsAchievsManager._AchievsListCount()]*/};
		GC.Collect();
		if(ES3.FileExists(_playerDataPath())){
			ES3.DeleteFile(_playerDataPath());
			Debug.Log("Game Data deleted");
		}
	}
#endregion
#region//Stats Data
	public StatsData statsData=new StatsData(){statsGamemodesList=new StatsGamemode[StatsAchievsManager.GetStatsGMListCount()]};
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
		if(File.Exists(Application.persistentDataPath+"/"+filenameStats)){//Legacy loading
			SaveGame.Encode=false;SaveGame.Serializer=new SaveGameJsonSerializer();
			statsData = SaveGame.Load<StatsData>(filenameStats);

			File.Delete(Application.persistentDataPath+"/"+filenameStats);
			Debug.Log("Stats Data (legacy) loaded and replaced");
			SaveStats();
		}
		if(ES3.FileExists(_statsDataPath())){
			var settings=new ES3Settings(_statsDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
			if(ES3.KeyExists("statsData",settings))ES3.LoadInto<StatsData>("statsData",statsData,settings);
			else Debug.LogWarning("Key for statsData not found in: "+_statsDataPath());
		}else Debug.LogWarning("Stats Data file not found in: "+_statsDataPath());
	}
	public void DeleteStats(){
		statsData=new StatsData(){statsGamemodesList=new StatsGamemode[StatsAchievsManager.GetStatsGMListCount()]};
		GC.Collect();
		if(ES3.FileExists(_statsDataPath())){
			ES3.DeleteFile(_statsDataPath());
			Debug.Log("Stats Data deleted");
		}
	}
#endregion
#region //Adventure Data
	public AdventureData advD=new AdventureData();
	[System.Serializable]public class AdventureData{
		public int cores=0;
		public float xp=0;
		public int zoneSelected=0;
		public int zoneToTravelTo=-1;
		public float travelTimeLeft=-4;

		public int holo_crystalsStored=0;
		public int holo_timeAt=0;
		public float holo_posX=0;
		
		public float health=0;
		public float hpAbsorpAmnt=0;
		public float energy=0;
		public float enAbsorpAmnt=0;
		public Powerup[] powerups;
		public int powerupCurID=0;
		public List<StatusFx> statuses=new List<StatusFx>();

		public int shipLvl=0;
		public int shipLvlFraction=0;
		public bool autoAscend;
		public List<string> moduleSlots=new List<string>();
		public List<string> skillsSlots=new List<string>(2);
		public List<Module> modulesList=new List<Module>();
		public List<Skill> skillsList=new List<Skill>();
	}

	public string _advDataPath(){return Application.persistentDataPath+"/"+filenameAdventure+".hyper";}
	public void SaveAdventure(){
		var settings=new ES3Settings(_advDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
		ES3.Save("advData",advD,settings);
		Debug.Log("Adventure Data saved");
	}
	public void LoadAdventure(){
		if(File.Exists(Application.persistentDataPath+"/"+filenameAdventure)){//Legacy loading
			SaveGame.Encode=false;SaveGame.Serializer=new SaveGameJsonSerializer();
			advD = SaveGame.Load<AdventureData>(filenameAdventure);

			File.Delete(Application.persistentDataPath+"/"+filenameAdventure);
			Debug.Log("Adventure Data (legacy) loaded and replaced");
			SaveAdventure();
		}
		if(ES3.FileExists(_advDataPath())){
			var settings=new ES3Settings(_advDataPath(),ES3.EncryptionType.AES,gitignoreScript.savefilesEncryptionKey);
			if(ES3.KeyExists("advData",settings))ES3.LoadInto<AdventureData>("advData",advD,settings);
			else Debug.LogWarning("Key for advData not found in: "+_statsDataPath());
		}else Debug.LogWarning("Adventure Data file not found in: "+_statsDataPath());
	}
	public void ResetAdventure(){
		if(advD==null){Debug.LogError("AdventureData null");}else{Debug.Log("AdventureData not empty");}
		advD=new AdventureData();
		GC.Collect();
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
	public SettingsData settingsData=new SettingsData();
	[System.Serializable]public class SettingsData{
		public InputType inputType;
		public JoystickType joystickType;
		public float joystickSize=1;
		public bool lefthand;
		public bool dtapMouseShoot;
		public bool scbuttons;
		public bool vibrations=true;
		public bool discordRPC=true;
		public bool autoselectNewItem=true;
		public bool alwaysReplaceCurrentSlot=false;
		public bool autoUseMedkitsIfLow=false;
		public bool allowSelectingEmptySlots=true;
		public bool allowScrollingEmptySlots=false;
		public bool autosubmitScores=true;
		
		public float masterVolume=0.95f;
		public float soundVolume=0.95f;
		public float ambienceVolume=-0.55f;
		public float musicVolume=0.66f;
		public bool windDownMusic=true;
		
		
		public PlaneDir playfieldRot=PlaneDir.vert;
		public int quality=4;
		public bool fullscreen=true;
		public bool pprocessing;
		public bool screenshake=true;
		public bool dmgPopups=true;
		public bool particles=true;		
		public bool screenflash=true;

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
		
		public bool playerWeaponsFade=true;
		public float hudVis_graphics=0.9f;
		public float hudVis_text=1f;
		public float hudVis_barFill=1f;
		public float hudVis_absorpFill=0.5f;
		public float hudVis_popups=0.9f;
		public float hudVis_notif=1f;
	}
	
	public string _settingsDataPath(){return Application.persistentDataPath+"/"+filenameSettings+".json";}
	public void SaveSettings(){
		var settings=new ES3Settings(_settingsDataPath());
		ES3.Save("settingsData",settingsData,settings);
		Debug.Log("Settings saved");
	}
	public void LoadSettings(){
		if(File.Exists(Application.persistentDataPath+"/"+filenameSettings+".cfg")){//Legacy loading
			SaveGame.Encode=false;SaveGame.Serializer=new SaveGameJsonSerializer();
			settingsData = SaveGame.Load<SettingsData>(filenameSettings+".cfg");
			
			File.Delete(Application.persistentDataPath+"/"+filenameSettings+".cfg");
			Debug.Log("Settings (legacy) loaded and replaced");
			settingsData.masterVolume=(float)System.Math.Round(GameAssets.Normalize(settingsData.masterVolume,-50,15),2);
			settingsData.soundVolume=(float)System.Math.Round(GameAssets.Normalize(settingsData.soundVolume,-50,15),2);
			settingsData.ambienceVolume=(float)System.Math.Round(GameAssets.Normalize(settingsData.ambienceVolume,-50,15),2);
			settingsData.musicVolume=(float)System.Math.Round(GameAssets.Normalize(settingsData.musicVolume,-50,15),2);
			SaveSettings();
		}
		if(ES3.FileExists(_settingsDataPath())){
		var settings=new ES3Settings(_settingsDataPath());
			if(ES3.KeyExists("settingsData",settings))ES3.LoadInto<SettingsData>("settingsData",settingsData,settings);
			else Debug.LogWarning("Key for settingsData not found in: "+_statsDataPath());
		}else Debug.LogWarning("Settings file not found in: "+_settingsDataPath());
	}
	public void ResetSettings(){
		settingsData=new SettingsData();
		GC.Collect();
		if(ES3.FileExists(_settingsDataPath())){
			ES3.DeleteFile(_settingsDataPath());
		}
	}
#endregion
}

[System.Serializable]
public class Highscore{
	public int score;
	public float playtime;
	public string version;
	public float build;
	public DateTime date;
}