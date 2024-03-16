using System;
using System.Collections.Generic;
using LGF.Res;
using UnityEngine;
using YooAsset;

namespace LGF.Res
{
    public class ResManager : MonoSingleton<ResManager>
    {
        /// <summary>
        /// 资源服务器地址
        /// </summary>
        public string HostServerUrl = ""; // TODO: 觉得有用，预留以后更改

        private string m_ApplicableGameVersion;
        private int m_InternalResourceVersion;

        /// <summary>
        /// 获取当前资源适用的游戏版本号。
        /// </summary>
        public string ApplicableGameVersion => m_ApplicableGameVersion;

        /// <summary>
        /// 获取当前内部资源版本号。
        /// </summary>
        public int InternalResourceVersion => m_InternalResourceVersion;

        /// <summary>
        /// 默认资源包。
        /// </summary>
        internal ResourcePackage DefaultPackage { get; private set; }

        /// <summary>
        ///  资源包 
        /// </summary>
        private Dictionary<string, ResourcePackage> m_PackageDic { get; } = new();

        /// <summary>
        /// 所有资源信息
        /// </summary>
        private readonly Dictionary<string, AssetInfo> _assetInfoDic = new();

        public override void Awake()
        {
            base.Awake();
            YooAssets.Initialize();
            DefaultPackage = YooAssets.CreatePackage("DefaultPackage");
            YooAssets.SetDefaultPackage(DefaultPackage);
        }

        #region 获取资源信息

        /// <summary>
        /// 获取资源包
        /// </summary>
        /// <param name="location">资源定位地址</param>
        /// <param name="packageName">资源包名</param>
        /// <returns></returns>
        /// <exception cref="Exception">资源信息</exception>
        public AssetInfo GetAssetInfo(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new Exception($"{nameof(location)} is null or empty !");
            }

            if (string.IsNullOrEmpty(packageName))
            {
                if (_assetInfoDic.TryGetValue(location, out AssetInfo assetInfo))
                    return assetInfo; // 从缓存中获取

                // 从默认资源包中获取
                assetInfo = YooAssets.GetAssetInfo(location);
                _assetInfoDic[location] = assetInfo;
                return assetInfo;
            }
            else
            {
                string key = $"{packageName}/{location}";
                if (_assetInfoDic.TryGetValue(key, out AssetInfo assetInfo))
                    return assetInfo;

                var package = YooAssets.GetPackage(packageName);
                if (package == null)
                    throw new Exception($"The package does not exist. Package Name :{packageName}");

                assetInfo = package.GetAssetInfo(location);
                _assetInfoDic[key] = assetInfo;
                return assetInfo;
            }
        }

        /// <summary>
        /// 获取资源信息列表。
        /// </summary>
        /// <param name="tag">资源标签。</param>
        /// <param name="packageName">资源包名称。</param>
        /// <returns>资源信息列表。</returns>
        public AssetInfo[] GetAssetInfos(string tag, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.GetAssetInfos(tag);
            }
            else
            {
                var package = YooAssets.GetPackage(packageName);
                return package.GetAssetInfos(tag);
            }
        }

        /// <summary>
        /// 获取资源信息列表。
        /// </summary>
        /// <param name="tags">资源标签列表。</param>
        /// <param name="packageName">资源包名称。</param>
        /// <returns>资源信息列表。</returns>
        public AssetInfo[] GetAssetInfos(string[] tags, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.GetAssetInfos(tags);
            }
            else
            {
                var package = YooAssets.GetPackage(packageName);
                return package.GetAssetInfos(tags);
            }
        }


        /// <summary>
        /// 检查资源定位地址是否有效。
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        /// <param name="packageName">资源包名称。</param>
        public bool CheckLocationValid(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.CheckLocationValid(location);
            }
            else
            {
                var package = YooAssets.GetPackage(packageName);
                return package.CheckLocationValid(location);
            }
        }

        /// <summary>
        /// 是否需要从远端更新下载。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="packageName">资源包名称。</param>
        public bool IsNeedDownloadFromRemote(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.IsNeedDownloadFromRemote(location);
            }
            else
            {
                var package = YooAssets.GetPackage(packageName);
                return package.IsNeedDownloadFromRemote(location);
            }
        }

        /// <summary>
        /// 是否需要从远端更新下载。
        /// </summary>
        /// <param name="assetInfo">资源信息。</param>
        /// <param name="packageName">资源包名称。</param>
        public bool IsNeedDownloadFromRemote(AssetInfo assetInfo, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.IsNeedDownloadFromRemote(assetInfo);
            }
            else
            {
                var package = YooAssets.GetPackage(packageName);
                return package.IsNeedDownloadFromRemote(assetInfo);
            }
        }

        /// <summary>
        ///  检测资源包是否存在
        /// </summary>
        /// <param name="location">资源地址</param>
        /// <param name="packageName">资源包名</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CheckAssetResult CheckAssetExists(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                throw new Exception($"{nameof(packageName)} is null or empty !");
            }

            AssetInfo assetInfo = GetAssetInfo(location, packageName);
            if (!CheckLocationValid(location, packageName))
            {
                return CheckAssetResult.NotExist;
            }

            if (assetInfo == null)
            {
                return CheckAssetResult.NotExist;
            }

            if (IsNeedDownloadFromRemote(location, packageName))
            {
                return CheckAssetResult.AssetOnline;
            }

            return CheckAssetResult.AssetOnDisk;
        }

        /// <summary>
        ///  获取资源包key
        /// </summary>
        /// <param name="location">资源定位地址</param>
        /// <param name="packageName">资源包名</param>
        /// <returns>key</returns>
        private string GetCharacterKey(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return location;
            }

            return $"{packageName}/{location}";
        }

        #endregion


        #region 获取资源句柄

        /// <summary>
        /// 获取同步资源句柄。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <typeparam name="T">资源类型。</typeparam>
        /// <returns>资源句柄。</returns>
        private AssetHandle GetHandleSync<T>(string location, string packageName = "") where T : UnityEngine.Object
        {
            return GetHandleSync(location, typeof(T), packageName);
        }

        private AssetHandle GetHandleSync(string location, Type assetType, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.LoadAssetSync(location, assetType);
            }

            var package = YooAssets.GetPackage(packageName);
            return package.LoadAssetSync(location, assetType);
        }

        /// <summary>
        /// 获取异步资源句柄。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <typeparam name="T">资源类型。</typeparam>
        /// <returns>资源句柄。</returns>
        private AssetHandle GetHandleAsync<T>(string location, string packageName = "") where T : UnityEngine.Object
        {
            return GetHandleAsync(location, typeof(T), packageName);
        }

        private AssetHandle GetHandleAsync(string location, Type assetType, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.LoadAssetAsync(location, assetType);
            }

            var package = YooAssets.GetPackage(packageName);
            return package.LoadAssetAsync(location, assetType);
        }

        #endregion

        public T LoadAsset<T>(string location, string packageName = "") where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new Exception("location is null or empty !");
            }

            string assetObjectKey = GetCharacterKey(location, packageName);
            // TODO: 从缓存中获取
            AssetHandle handle = GetHandleSync<T>(location, packageName);
            T ret = handle.AssetObject as T;
            // TODO: 缓存资源
            return ret;
        }

        public GameObject LoadGameObject(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new Exception("location is null or empty !");
            }

            string assetObjectKey = GetCharacterKey(location, packageName);
            // TODO 从缓存获取
            AssetHandle handle = GetHandleSync<GameObject>(location, packageName);
            // GameObject obj = 
            return null;
        }

        private void OnDestroy()
        {
            YooAssets.Destroy();
        }
    }
}