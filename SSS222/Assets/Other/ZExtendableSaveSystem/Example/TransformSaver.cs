using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.ExtendableSaveSystem
{
    public class TransformSaver : MonoBehaviour, ISavableComponent
    {
        [SerializeField] private int _uniqueID;
        [SerializeField] private int _executionOrder;

        public int uniqueID
        {
            get
            {
                return _uniqueID;
            }
        }
        public int executionOrder
        {
            get
            {
                return _executionOrder;
            }
        }


        private void Reset()
        {
            _uniqueID = GetHashCode();
        }

        public ComponentData Serialize()
        {
            ExtendedComponentData data = new ExtendedComponentData();

            data.SetTransform("transform", transform);

            return data;
        }

        public void Deserialize(ComponentData data)
        {
            ExtendedComponentData unpacked = (ExtendedComponentData)data;

            unpacked.GetTransform("transform", transform);
        }
    }
}
