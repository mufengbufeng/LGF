using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using YooAsset;

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

    /// <summary>
    /// 获取资源包
    /// </summary>
    /// <param name="location"></param>
    /// <param name="packageName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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


    private void OnDestroy()
    {
        YooAssets.Destroy();
    }
}