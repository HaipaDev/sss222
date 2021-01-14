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
	[SerializeField] string filename = "playerData";
	bool dataEncode=true;
	[SerializeField] string filenameAdventure = "adventureData";
	bool adventureEncode=false;
	[SerializeField] string filenameSettings = "gameSettings.cfg";
	bool settingsEncode=false;
#region//Player Data
	public PlayerData playerData=new PlayerData();
	[System.Serializable]public class PlayerData{
		public int[] highscore=new int[GameSession.gameModeMaxID];
		public int skinID;
		public float[] chameleonColor=new float[3];
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
		public float xp=0;
		public int total_UpgradesCount=0;
		public int total_UpgradesLvl=0;
		public int maxHealth_UpgradesCount=0;
		public int maxHealth_UpgradesLvl=0;
		public int maxEnergy_UpgradesCount=0;
		public int maxEnergy_UpgradesLvl=0;
		public int speed_UpgradesCount=0;
		public int speed_UpgradesLvl=0;
		public int hpRegen_UpgradesCount=0;
		public int hpRegen_UpgradesLvl=0;
		public int enRegen_UpgradesCount=0;
		public int enRegen_UpgradesLvl=0;
		public int luck_UpgradesCount=0;
		public int luck_UpgradesLvl=0;
		public int defaultPowerup_upgradeCount=0;
		public int energyRefill_upgraded=0;
		public int magneticPulse_upgraded=0;
		public int teleport_upgraded=0;
		public int overhaul_upgraded=0;
	}
	public void SaveAdventure(){
		SaveGame.Encode = adventureEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filename, advD);
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
		public string gameVersion="0.4";
		public bool moveByMouse=true;
		public bool fullscreen=true;
		public bool pprocessing;
		public bool scbuttons;
		public int quality=4;
		public float masterVolume=0;
		public float soundVolume=0;
		public float musicVolume=-25;
		public JoystickType joystickType;
		public float joystickSize=1;
		public bool lefthand;
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
#region//Singleton
	private void Awake(){
		SetUpSingleton();
		instance=this;
		playerData.highscore=new int[GameSession.gameModeMaxID];
	}
	private void SetUpSingleton(){
		int numberOfObj = FindObjectsOfType<GameSession>().Length;
		if (numberOfObj > 1)
		{
			Destroy(gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
		}
	}
#endregion
}