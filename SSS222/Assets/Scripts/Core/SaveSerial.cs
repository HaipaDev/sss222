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
	[SerializeField] string filename = "playerData";
	[SerializeField] string filenameSettings = "gameSettings.cfg";
	[HeaderAttribute("PlayerData")]
	public int highscore;
	public float[] chameleonColor = new float[3];
	[HeaderAttribute("SettingsData")]
	public string gameVersion;
	public bool moveByMouse;
	public bool fullscreen;
	public int quality;
	public float masterVolume;
	public float soundVolume;
	public float musicVolume;
	public class PlayerData
	{
		public int highscore;
		public float[] chameleonColor=new float[3];
		
	}public class SettingsData
	{
		public string gameVersion;
		public bool moveByMouse;
		public bool fullscreen;
		public int quality;
		public float masterVolume;
		public float soundVolume;
		public float musicVolume;
	}

	public void Save()
	{
		PlayerData data = new PlayerData();
		data.highscore = highscore;
		data.chameleonColor[0] = chameleonColor[0];
		data.chameleonColor[1] = chameleonColor[1];
		data.chameleonColor[2] = chameleonColor[2];

		// Saving the data
		SaveGame.Encode = true;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filename, data);
		Debug.Log("Game Data saved");
	}
	public void SaveSettings()
	{
		SettingsData data = new SettingsData();
		data.gameVersion=gameVersion;
		data.moveByMouse = moveByMouse;
		data.fullscreen = fullscreen;
		data.quality = quality;
		data.masterVolume = masterVolume;
		data.soundVolume = soundVolume;
		data.musicVolume = musicVolume;

		// Saving the data
		SaveGame.Encode = false;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filenameSettings, data);
		Debug.Log("Settings saved");
	}
	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/"+filename)){
			PlayerData data = new PlayerData();
			SaveGame.Encode = true;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			data = SaveGame.Load<PlayerData>(filename);

			if(data.highscore!=0)highscore = data.highscore;
			chameleonColor[0] = data.chameleonColor[0];
			chameleonColor[1] = data.chameleonColor[1];
			chameleonColor[2] = data.chameleonColor[2];
			Debug.Log("Game Data loaded");
		}else Debug.Log("Game Data file not found in "+Application.persistentDataPath+"/"+filename);
	}
	public void LoadSettings()
	{
		if (File.Exists(Application.persistentDataPath + "/"+filenameSettings)){
			SettingsData data = new SettingsData();
			SaveGame.Encode = false;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			data = SaveGame.Load<SettingsData>(filenameSettings);

			gameVersion=data.gameVersion;
			moveByMouse = data.moveByMouse;
			fullscreen = data.fullscreen;
			quality = data.quality;
			masterVolume = data.masterVolume;
			soundVolume = data.soundVolume;
			musicVolume = data.musicVolume;
			Debug.Log("Settings loaded");
		}
		else Debug.Log("Settings file not found in " + Application.persistentDataPath + "/" + filenameSettings);
	}
	#region//Singleton
	private void Awake()
	{
		SetUpSingleton();
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
}