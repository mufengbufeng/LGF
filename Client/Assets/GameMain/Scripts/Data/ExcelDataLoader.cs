
using System.Collections.Generic;
using UnityEngine;
namespace LGF.Data
{
    public static partial class ExcelDataLoader
    {
        
        public static ExcelDataSerialize _excelDataSerialize;
        
        private static void InitDict<T>(Dictionary<int, T> dict, T[] datas) where T : AExData
        {
            dict.Clear();
            if(datas == null) return;
            foreach (var data in datas)
            {
                if(dict.ContainsKey(data.id))
                {
                    Debug.LogError($"{0} id:{data.id} is already exist");
                    return;
                }
                else
                {
                    dict.Add(data.id, data);
                }
            }
        }
        public static void Sericalize(string jsonData)
        {
            _excelDataSerialize = Json.FromJson<ExcelDataSerialize>(jsonData);
            if(_excelDataSerialize == null)
            {
                Debug.LogError("ExcelDataSerialize is null");
            }

            
            Game.EventManager.Trigger(GameEvent.ExcelDataLoadedFinish);
        }

        
    }
}
