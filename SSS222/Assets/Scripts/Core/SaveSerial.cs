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
	public int highscore;
	public bool moveByMouse=true;
	public class PlayerData
	{
		public int highscore;
		public bool moveByMouse;
	}

	public void Save()
	{
		PlayerData data = new PlayerData();
		data.highscore = highscore;
		data.moveByMouse = moveByMouse;

		// Saving the data
		SaveGame.Encode = true;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save("playerData", data);
	}
	public void Load()
	{
		PlayerData data = new PlayerData();
		SaveGame.Encode = true;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		data = SaveGame.Load<PlayerData>("playerData");
		highscore = data.highscore;
		moveByMouse = data.moveByMouse;
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