using System;
using System.Collections.Generic;

namespace NGS.ExtendableSaveSystem
{
    [Serializable]
    public class ComponentData
    {
        protected Dictionary<string, float> _floats = new Dictionary<string, float>();
        protected Dictionary<string, int> _integers = new Dictionary<string, int>();
        protected Dictionary<string, string> _strings = new Dictionary<string, string>();


        public virtual void SetFloat(string uniqueName, float value)
        {
            _floats.Add(uniqueName, value);
        }

        public virtual void SetInt(string uniqueName, int value)
        {
            _integers.Add(uniqueName, value);
        }

        public virtual void SetInt(string uniqueName, string value)
        {
            _strings.Add(uniqueName, value);
        }


        public virtual float GetFloat(string uniqueName)
        {
            return _floats[uniqueName];
        }

        public virtual int GetInt(string uniqueName)
        {
            return _integers[uniqueName];
        }

        public virtual string GetString(string uniqueName)
        {
            return _strings[uniqueName];
        }
    }
}
