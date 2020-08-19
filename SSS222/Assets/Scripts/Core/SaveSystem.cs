using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SaveSystem : MonoBehaviour
{
    /*public static void SaveData(GameSession.SavableData savableData){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.sav";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        formatter.Serialize(stream, savableData);
        stream.Close();
    }
    public static GameSession.SavableData LoadData(){
        string path = Application.persistentDataPath + "/save.sav";
        if (File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameSession.SavableData savableData = formatter.Deserialize(stream) as GameSession.SavableData;
            stream.Close();

            return savableData;
        }
        else{
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }*/
}