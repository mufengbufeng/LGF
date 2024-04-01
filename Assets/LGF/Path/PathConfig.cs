using System;
using System.IO;
using LGF.Res;
using Unity;
using UnityEngine;

namespace LGF.Path
{
    public static class PathConfig
    {
        private static string AssetsPath = Application.dataPath;

        private static string StreamingAssetsPath = Application.streamingAssetsPath;

        private static string _hotfixPath = $"{Game.resManager.DefaultPackage.GetPackageSandboxRootDirectory()}";
        private static string _buildinRootPath = $"{Game.resManager.DefaultPackage.GetPackageBuildinRootDirectory()}";

        //D:/game/LGF/Assets/Art/UIPrefabs/Root/RootCube
        public static string ArtPath = $"{AssetsPath}/GameMain/Art";

        //获取资源路径
        public static string GetMaterialPath(string assetName)
        {
            string assetPath = $"{ArtPath}/Materials/{GetFirstWord(assetName)}/{assetName}";
            Debug.Log(assetPath);
            return assetName;
        }

        public static string GetUIPrefabPath(string assetName)
        {
            string assetPath = "";
            assetPath = $"{_buildinRootPath}/UIPrefabs/{GetFirstWord(assetName)}/{assetName}";

            return assetPath;
        }

        // 获取大驼峰第一个单词长度
        public static int GetFirstWordLength(string str)
        {
            int length = 0;
            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    length = i;
                    break;
                }
            }

            return length;
        }

        // 将大驼峰第一个单词提取并返回
        public static string GetFirstWord(string str)
        {
            int length = GetFirstWordLength(str);
            return str.Substring(0, length);
        }
    }
}