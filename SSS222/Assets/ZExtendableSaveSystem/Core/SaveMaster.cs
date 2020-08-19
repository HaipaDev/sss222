using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace NGS.ExtendableSaveSystem
{
    public class SaveMaster : MonoBehaviour
    {
        protected ISavableComponent[] GetOrderedSavableComponents()
        {
            return FindObjectsOfTypeAll(typeof(Component))
                .Where(c => c is ISavableComponent)
                .Select(c => (ISavableComponent)c)
                .OrderBy(c => c.executionOrder)
                .ToArray(); 
        }

        public virtual void Save(string folderPath, string fileName, string fileFormat)
        {
            if (!folderPath.EndsWith("/")) folderPath += "/";
            if (!fileFormat.StartsWith(".")) fileFormat = "." + fileFormat;

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            Dictionary<int, ComponentData> componentsData = new Dictionary<int, ComponentData>();

            foreach (var savableComponent in GetOrderedSavableComponents())
                componentsData.Add(savableComponent.uniqueID, savableComponent.Serialize());

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(folderPath + fileName + fileFormat, FileMode.Create))
                formatter.Serialize(stream, componentsData);
        }

        public virtual void Load(string folderPath, string fileName, string fileFormat)
        {
            if (!folderPath.EndsWith("/")) folderPath += "/";
            if (!fileFormat.StartsWith(".")) fileFormat = "." + fileFormat;

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException("SaveMaster::Directory '" + folderPath + "' not found");

            Dictionary<int, ComponentData> componentsData = null;

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(folderPath + fileName + fileFormat, FileMode.Open))
                componentsData = (Dictionary<int, ComponentData>) formatter.Deserialize(stream);

            foreach (var savableComponent in GetOrderedSavableComponents())
                if (componentsData.ContainsKey(savableComponent.uniqueID))
                    savableComponent.Deserialize(componentsData[savableComponent.uniqueID]);
        }
    }
}
