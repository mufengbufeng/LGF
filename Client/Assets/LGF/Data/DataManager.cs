using System;
using System.Collections.Generic;
using LGF.Path;
using UnityEngine;

namespace LGF.Data
{
    [Serializable]
    public class ConfigEx
    {
        public List<string> paths;
    }

    public class DataManager : MonoSingleton<DataManager>
    {
        private Dictionary<string, object> _dataDic = new();

        protected override void Awake()
        {
            base.Awake();
            Init("ExcelData");
            // GetDataRow<ExcelTestExData>("1");
        }

        private void Init(string fileName)
        {
            
            TextAsset res = Game.ResManager.LoadAsset<TextAsset>(PathConfig.GetJsonDataPath(fileName));
            if (res == null) return;
            ExcelDataLoader.Sericalize(res.text); 
            // ExcelDataSerialize infos = Json.FromJson<ExcelDataSerialize>(res.text);
        }

        // private void OnLoadToSub(TextAsset textAsset, string url)
        // {
        // var dataName = $"Excel{textAsset.name}Data";
        // var type = Type.GetType($"LGF.Data.{dataName}");
        // object dataInfos = JsonUtility.FromJson(textAsset.text, type);
        // _dataDic.Add(dataName, dataInfos);
        // }

        public object GetDataRow<T>(string key)
        {
            var dataName = typeof(T).Name;
            if (_dataDic.TryGetValue(dataName, out var dataValue))
            {
                Debug.Log(dataValue);
            }

            return null;
        }

        public object getData<T>(string dataName, string value)
        {
            // if (_dataDic.TryGetValue())
            return null;
        }
    }
}