using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Encoders;
using BayatGames.SaveGameFree.Serializers;

public class SaveSerial : MonoBehaviour{
	public static SaveSerial instance;
	void Awake(){if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}playerData.highscore=new int[GameSession.gameModeMaxID];}
	[SerializeField] string filenameLogin = "hyperGamerLogin";
	bool loginEncode=false;
	[SerializeField] string filename = "playerData";
	bool dataEncode=false;
	//bool dataEncodeValues=true;
	[SerializeField] string filenameAdventure = "adventureData";
	bool adventureEncode=false;
	//bool adventureEncodeValues=true;
	[SerializeField] string filenameSettings = "gameSettings.cfg";
	bool settingsEncode=false;
	public int maxRegisteredHyperGamers=3;

#region//HyperGamerLogin
	public HyperGamerLoginData hyperGamerLoginData=new HyperGamerLoginData();
	[System.Serializable]public class HyperGamerLoginData{
		public int registeredCount;
		public bool loggedIn;
		public string username;
		public string password;
	}
	public void SetLogin(string username, string password){
		hyperGamerLoginData.loggedIn=true;
		hyperGamerLoginData.username=username;
		hyperGamerLoginData.password=password;
	}
	public void LogOut(){
		hyperGamerLoginData.loggedIn=false;
		hyperGamerLoginData.username="";
		hyperGamerLoginData.password="";
	}
	public void SaveLogin(){
		SaveGame.Encode = loginEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filenameLogin, hyperGamerLoginData);
		Debug.Log("Login saved");
	}
	public void LoadLogin(){
		if(File.Exists(Application.persistentDataPath + "/"+filenameLogin)){
			SaveGame.Encode = loginEncode;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			hyperGamerLoginData = SaveGame.Load<HyperGamerLoginData>(filenameLogin);

			Debug.Log("Login loaded");
		}else Debug.Log("Login Data file not found in "+Application.persistentDataPath+"/"+filename);
	}
#endregion
#region//Player Data
	public PlayerData playerData=new PlayerData();
	[System.Serializable]public class PlayerData{
		public int[] highscore=new int[GameSession.gameModeMaxID];
		public string skinName="Mk.22";
		public float[] chameleonColor=new float[3]{1,1,1};
	}
	public void Save(){
		SaveGame.Encode = dataEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filename, playerData);
		Debug.Log("Game Data saved");
	}
	public void Load(){
		if (File.Exists(Application.persistentDataPath + "/"+filename)){
			SaveGame.Encode = dataEncode;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			playerData = SaveGame.Load<PlayerData>(filename);

			var hi=-1;foreach(int h in playerData.highscore){hi++;if(h!=0)playerData.highscore[hi] = h;}
			Debug.Log("Game Data loaded");
		}else Debug.Log("Game Data file not found in "+Application.persistentDataPath+"/"+filename);
	}
	public void Delete(){
		playerData=new PlayerData();
		GC.Collect();
		if (File.Exists(Application.persistentDataPath + "/"+filename)){
			File.Delete(Application.persistentDataPath + "/"+filename);
			Debug.Log("Game Data deleted");
		}else Debug.Log("Game Data file not found in "+Application.persistentDataPath+"/"+filename);
	}
#endregion
#region //Adventure Data
	public AdventureData advD=new AdventureData();
	[System.Serializable]public class AdventureData{
		public int total_UpgradesCount=0;
		public int total_UpgradesLvl=0;
		public int healthMax_UpgradesCount=0;
		public int healthMax_UpgradesLvl=0;
		public int energyMax_UpgradesCount=0;
		public int energyMax_UpgradesLvl=0;
		public int speed_UpgradesCount=0;
		public int speed_UpgradesLvl=0;
		public int luck_UpgradesCount=0;
		public int luck_UpgradesLvl=0;
		//
		public int defaultPowerup_upgradeCount=0;
		public int energyRefill_upgraded=0;
		public int mPulse_upgraded=0;
		public int teleport_upgraded=0;
		public int overhaul_upgraded=0;
		public int crMend_upgraded=0;
		public int enDiss_upgraded=0;
	}
	public void SaveAdventure(){
		SaveGame.Encode = adventureEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filenameAdventure, advD);
		Debug.Log("Adventure Data saved");
	}
	public void LoadAdventure(){
		if (File.Exists(Application.persistentDataPath + "/"+filenameAdventure)){
			SaveGame.Encode = adventureEncode;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			advD = SaveGame.Load<AdventureData>(filenameAdventure);
			Debug.Log("Adventure Data loaded");
		}else Debug.Log("Adventure Data file not found in "+Application.persistentDataPath+"/"+filenameAdventure);
	}
	public void ResetAdventure(){
		if(advD==null){Debug.LogError("AdventureData null");}else{Debug.Log("AdventureData not empty");}
		advD=new AdventureData();
		GC.Collect();
		Debug.Log("Adventure Data reset");
	}
	public void DeleteAdventure(){
		ResetAdventure();
		if(File.Exists(Application.persistentDataPath + "/"+filenameAdventure)){
			File.Delete(Application.persistentDataPath + "/"+filenameAdventure);
			Debug.Log("Adventure Data deleted");
		}else Debug.Log("Adventure Data file not found in "+Application.persistentDataPath+"/"+filenameAdventure);
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
		public bool vibrations;
		
		public float masterVolume=0;
		public float soundVolume=0;
		public float musicVolume=-25;
		
		
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
		public bool enPopupsSum;
		public bool ammoPopupsSum;
		public bool xpPopupsSum;
		public bool coinPopupsSum;
		public bool corePopupsSum;
		public bool scorePopupsSum;
		
		public float hudVis_graphics=0.9f;
		public float hudVis_text=1f;
		public float hudVis_barFill=1f;
		public float hudVis_absorpFill=0.5f;
		public float hudVis_popups=0.9f;
		public float hudVis_notif=1f;
	}
	
	public void SaveSettings(){
		SaveGame.Encode = settingsEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filenameSettings, settingsData);
		Debug.Log("Settings saved");
	}
	public void LoadSettings(){
		if (File.Exists(Application.persistentDataPath + "/"+filenameSettings)){
			SettingsData data = new SettingsData();
			SaveGame.Encode = settingsEncode;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			settingsData = SaveGame.Load<SettingsData>(filenameSettings);
			Debug.Log("Settings loaded");
		}
		else Debug.Log("Settings file not found in " + Application.persistentDataPath + "/" + filenameSettings);
	}
	public void ResetSettings(){
		settingsData=new SettingsData();
		GC.Collect();
		if (File.Exists(Application.persistentDataPath + "/"+filenameSettings)){
			File.Delete(Application.persistentDataPath + "/"+filenameSettings);
			Debug.Log("Settings Data deleted");
		}else Debug.Log("Settings file not found in "+Application.persistentDataPath+"/"+filenameSettings);
	}
#endregion
}
public enum InputType{mouse,touch,keyboard,drag}
public enum PlaneDir{vert,horiz}