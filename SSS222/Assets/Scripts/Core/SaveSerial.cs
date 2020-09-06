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
	[HeaderAttribute("PlayerData")]
	public int[] highscore=new int[GameSession.gameModeMaxID];
	public int skinID;
	public float[] chameleonColor = new float[3];
	public class PlayerData{
		public int[] highscore=new int[GameSession.gameModeMaxID];
		public int skinID;
		public float[] chameleonColor=new float[3];
	}
	public void Save(){
		PlayerData data = new PlayerData();
		data.highscore = highscore;
		data.skinID = skinID;
		data.chameleonColor[0] = chameleonColor[0];
		data.chameleonColor[1] = chameleonColor[1];
		data.chameleonColor[2] = chameleonColor[2];
		

		// Saving the data
		SaveGame.Encode = dataEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filename, data);
		Debug.Log("Game Data saved");
	}
	public void Load(){
		if (File.Exists(Application.persistentDataPath + "/"+filename)){
			PlayerData data = new PlayerData();
			SaveGame.Encode = dataEncode;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			data = SaveGame.Load<PlayerData>(filename);

			var hi=-1;foreach(int h in data.highscore){hi++;if(h!=0)highscore[hi] = h;}
			skinID = data.skinID;
			chameleonColor[0] = data.chameleonColor[0];
			chameleonColor[1] = data.chameleonColor[1];
			chameleonColor[2] = data.chameleonColor[2];
			Debug.Log("Adventure Data loaded");
		}else Debug.Log("Game Data file not found in "+Application.persistentDataPath+"/"+filename);
	}
	public void Delete(){
		if (File.Exists(Application.persistentDataPath + "/"+filename)){
			File.Delete(Application.persistentDataPath + "/"+filename);
		}else Debug.Log("Game Data file not found in "+Application.persistentDataPath+"/"+filename);
	}
#endregion
#region //Adventure Data
	[HeaderAttribute("AdventureData")]
	public float xp;
	public int total_UpgradesCount;
	public int total_UpgradesLvl;
	public int maxHealth_UpgradesCount;
	public int maxHealth_UpgradesLvl;
	public int maxEnergy_UpgradesCount;
	public int maxEnergy_UpgradesLvl;
	public int speed_UpgradesCount;
	public int speed_UpgradesLvl;
	public int hpRegen_UpgradesCount;
	public int hpRegen_UpgradesLvl;
	public int enRegen_UpgradesCount;
	public int enRegen_UpgradesLvl;
	public int luck_UpgradesCount;
	public int luck_UpgradesLvl;
	public int defaultPowerup_upgradeCount;
	public int energyRefill_upgraded;
	public int magneticPulse_upgraded;
	public int teleport_upgraded;
	public int overhaul_upgraded;
	public class AdventureData{
		public float xp;
		public int total_UpgradesCount;
		public int total_UpgradesLvl;
		public int maxHealth_UpgradesCount;
		public int maxHealth_UpgradesLvl;
		public int maxEnergy_UpgradesCount;
		public int maxEnergy_UpgradesLvl;
		public int speed_UpgradesCount;
		public int speed_UpgradesLvl;
		public int hpRegen_UpgradesCount;
		public int hpRegen_UpgradesLvl;
		public int enRegen_UpgradesCount;
		public int enRegen_UpgradesLvl;
		public int luck_UpgradesCount;
		public int luck_UpgradesLvl;
		public int defaultPowerup_upgradeCount;
		public int energyRefill_upgraded;
		public int magneticPulse_upgraded;
		public int teleport_upgraded;
		public int overhaul_upgraded;
	}
	public void SaveAdventure(){
		AdventureData data = new AdventureData();
		data.xp=xp;
		data.total_UpgradesCount=total_UpgradesCount;
		data.total_UpgradesLvl=total_UpgradesLvl;
		data.maxHealth_UpgradesCount=maxHealth_UpgradesCount;
		data.maxHealth_UpgradesLvl=maxHealth_UpgradesLvl;
		data.maxEnergy_UpgradesCount=maxEnergy_UpgradesCount;
		data.maxEnergy_UpgradesLvl=maxEnergy_UpgradesLvl;
		data.speed_UpgradesCount=speed_UpgradesCount;
		data.speed_UpgradesLvl=speed_UpgradesLvl;
		data.hpRegen_UpgradesCount=hpRegen_UpgradesCount;
		data.hpRegen_UpgradesLvl=hpRegen_UpgradesLvl;
		data.enRegen_UpgradesCount=enRegen_UpgradesCount;
		data.enRegen_UpgradesLvl=enRegen_UpgradesLvl;
		data.luck_UpgradesCount=luck_UpgradesCount;
		data.luck_UpgradesLvl=luck_UpgradesLvl;

		data.defaultPowerup_upgradeCount=defaultPowerup_upgradeCount;
		data.energyRefill_upgraded=energyRefill_upgraded;
		data.magneticPulse_upgraded=magneticPulse_upgraded;
		data.teleport_upgraded=teleport_upgraded;
		data.overhaul_upgraded=overhaul_upgraded;
		
		// Saving the data
		SaveGame.Encode = adventureEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filenameAdventure, data);
		Debug.Log("Adventure Data saved");
	}
	public void LoadAdventure(){
		if (File.Exists(Application.persistentDataPath + "/"+filenameAdventure)){
			AdventureData data = new AdventureData();
			SaveGame.Encode = adventureEncode;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			data = SaveGame.Load<AdventureData>(filenameAdventure);

			xp=data.xp;
			total_UpgradesCount=data.total_UpgradesCount;
			total_UpgradesLvl=data.total_UpgradesLvl;
			maxHealth_UpgradesCount=data.maxHealth_UpgradesCount;
			maxHealth_UpgradesLvl=data.maxHealth_UpgradesLvl;
			maxEnergy_UpgradesCount=data.maxEnergy_UpgradesCount;
			maxEnergy_UpgradesLvl=data.maxEnergy_UpgradesLvl;
			speed_UpgradesCount=data.speed_UpgradesCount;
			speed_UpgradesLvl=data.speed_UpgradesLvl;
			hpRegen_UpgradesCount=data.hpRegen_UpgradesCount;
			hpRegen_UpgradesLvl=data.hpRegen_UpgradesLvl;
			enRegen_UpgradesCount=data.enRegen_UpgradesCount;
			enRegen_UpgradesLvl=data.enRegen_UpgradesLvl;
			luck_UpgradesCount=data.luck_UpgradesCount;
			luck_UpgradesLvl=data.luck_UpgradesLvl;

			defaultPowerup_upgradeCount=data.defaultPowerup_upgradeCount;
			energyRefill_upgraded=data.energyRefill_upgraded;
			magneticPulse_upgraded=data.magneticPulse_upgraded;
			teleport_upgraded=data.teleport_upgraded;
			overhaul_upgraded=data.overhaul_upgraded;
			Debug.Log("Adventure Data loaded");
		}else Debug.Log("Adventure Data file not found in "+Application.persistentDataPath+"/"+filenameAdventure);
	}
	public void DeleteAdventure(){
		if (File.Exists(Application.persistentDataPath + "/"+filenameAdventure)){
			File.Delete(Application.persistentDataPath + "/"+filenameAdventure);
		}else Debug.Log("Adventure Data file not found in "+Application.persistentDataPath+"/"+filenameAdventure);
	}
#endregion
#region//Settings Data
	[HeaderAttribute("SettingsData")]
	public string gameVersion;
	public bool moveByMouse;
	public bool fullscreen;
	public bool pprocessing;
	public bool scbuttons;
	public int quality;
	public float masterVolume;
	public float soundVolume;
	public float musicVolume;
	public JoystickType joystickType=JoystickType.Dynamic;
	public float joystickSize=1f;
	public class SettingsData{
		public string gameVersion;
		public bool moveByMouse;
		public bool fullscreen;
		public bool pprocessing;
		public bool scbuttons;
		public int quality;
		public float masterVolume;
		public float soundVolume;
		public float musicVolume;
	}	
	public void SaveSettings(){
		SettingsData data = new SettingsData();
		data.gameVersion=gameVersion;
		data.moveByMouse = moveByMouse;
		data.fullscreen = fullscreen;
		data.pprocessing = pprocessing;
		data.scbuttons = scbuttons;
		data.quality = quality;
		data.masterVolume = masterVolume;
		data.soundVolume = soundVolume;
		data.musicVolume = musicVolume;

		// Saving the data
		SaveGame.Encode = settingsEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filenameSettings, data);
		Debug.Log("Settings saved");
	}
	public void LoadSettings(){
		if (File.Exists(Application.persistentDataPath + "/"+filenameSettings)){
			SettingsData data = new SettingsData();
			SaveGame.Encode = settingsEncode;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			data = SaveGame.Load<SettingsData>(filenameSettings);

			gameVersion=data.gameVersion;
			moveByMouse = data.moveByMouse;
			fullscreen = data.fullscreen;
			pprocessing = data.pprocessing;
			scbuttons = data.scbuttons;
			quality = data.quality;
			masterVolume = data.masterVolume;
			soundVolume = data.soundVolume;
			musicVolume = data.musicVolume;
			Debug.Log("Settings loaded");
		}
		else Debug.Log("Settings file not found in " + Application.persistentDataPath + "/" + filenameSettings);
	}
	public void ResetSettings(){
		if (File.Exists(Application.persistentDataPath + "/"+filenameSettings)){
			File.Delete(Application.persistentDataPath + "/"+filenameSettings);
		}else Debug.Log("Settings file not found in "+Application.persistentDataPath+"/"+filenameSettings);
	}
	#endregion
#region//Singleton
	private void Awake()
	{
		SetUpSingleton();
		instance=this;
		highscore=new int[GameSession.gameModeMaxID];
	}
	private void SetUpSingleton()
	{
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
#region//Old
	/*public int highscore;
    public void SaveGame()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath
					 + "/SaveData.dat", FileMode.OpenOrCreate);
		SaveData data = new SaveData();
		data.savedHscore = highscore;
		bf.Serialize(file, data);
		file.Close();
		Debug.Log("Game data saved!");
	}
	public void LoadGame()
	{
		if (File.Exists(Application.persistentDataPath
					   + "/SaveData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file =
					   File.Open(Application.persistentDataPath
					   + "/SaveData.dat", FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();
			highscore = data.savedHscore;
			Debug.Log("Game data loaded!");
		}
		else
			Debug.LogError("There is no save data!");
	}
	public void ResetData()
	{
		if (File.Exists(Application.persistentDataPath
					  + "/SaveData.dat"))
		{
			File.Delete(Application.persistentDataPath
							  + "/MySaveData.dat");
			highscore = 0;
			Debug.Log("Data reset complete!");
		}
		else
			Debug.LogError("No save data to delete.");
	}


[Serializable]
class SaveData{
    public int savedHscore;
}*/
#endregion
}