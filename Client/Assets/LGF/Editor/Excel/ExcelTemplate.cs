namespace LGF.Editor
{
    public static class ExcelTemplate
    {
        public const string ExcelDataTemplate = @"
    [Serializable]
    public class Excel{0}Data : AExData
    {{
        {1}
    }}   
 ";

        public const string AExDataTemplate = @"
	[Serializable]
    public abstract class AExData
    {
        public int id;
    }  
";

        public const string ExcelDataSerializeTemplate = @"
    [Serializable]
    public class ExcelDataSerialize
    {{
{0} 
    }}     
";

        public const string ExcelLoaderTemplate = @"
using System.Collections.Generic;
using UnityEngine;
namespace LGF.Data
{{
    public static partial class ExcelDataLoader
    {{
        {0}
        public static ExcelDataSerialize _excelDataSerialize;
        {1}
        private static void InitDict<T>(Dictionary<int, T> dict, T[] datas) where T : AExData
        {{
            dict.Clear();
            if(datas == null) return;
            foreach (var data in datas)
            {{
                if(dict.ContainsKey(data.id))
                {{
                    Debug.LogError($""{{0}} id:{{data.id}} is already exist"");
                    return;
                }}
                else
                {{
                    dict.Add(data.id, data);
                }}
            }}
        }}
        public static void Sericalize(string jsonData)
        {{
            _excelDataSerialize = Json.FromJson<ExcelDataSerialize>(jsonData);
            if(_excelDataSerialize == null)
            {{
                Debug.LogError(""ExcelDataSerialize is null"");
            }}

            {2}
            Game.EventManager.Trigger(GameEvent.ExcelDataLoadedFinish);
        }}

        {3}
    }}
}}
";
    }
}