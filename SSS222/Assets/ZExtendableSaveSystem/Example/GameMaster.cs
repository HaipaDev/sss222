using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.ExtendableSaveSystem
{
    [RequireComponent(typeof(SaveMaster))]
    public class GameMaster : MonoBehaviour
    {
        public void SaveGame()
        {
            GetComponent<SaveMaster>().Save("Assets/Saves/", "save", ".data");
            Debug.Log("Game saved");
        }

        public void LoadGame()
        {
            GetComponent<SaveMaster>().Load("Assets/Saves/", "save", ".data");
            Debug.Log("Game loaded");
        }
    }
}
