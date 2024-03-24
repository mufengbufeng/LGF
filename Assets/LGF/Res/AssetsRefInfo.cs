using System;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using Object = UnityEngine.Object;
using UnityEngine;

namespace LGF.Res
{
    [Serializable]
    public struct AssetsRefInfo
    {
        public int instanceID; // 资源实例ID
        public Object refObject; // 资源引用对象

        public AssetsRefInfo(Object refObject)
        {
            this.refObject = refObject;
            instanceID = this.refObject.GetInstanceID();
        }
    }

    public sealed class AssetsReference : MonoBehaviour
    {
        [SerializeField] private GameObject _sourceGameObject; // 资源引用对象
        [SerializeField] private List<AssetsRefInfo> _refAssetInfoList; // 资源引用信息

        private ResManager _resManager;

        private void OnDestroy()
        {
            if (_resManager == null)
            {
                _resManager = Game.resManager;
            }

            if (_sourceGameObject != null)
            {
                _resManager.UnloadAsset(_sourceGameObject);
            }

            if (_refAssetInfoList != null)
            {
                foreach (var assetRefInfo in _refAssetInfoList)
                {
                    _resManager.UnloadAsset(assetRefInfo.refObject);
                }

                _refAssetInfoList.Clear();
            }
        }

        public AssetsReference Ref(GameObject source, ResManager resourceManager = null)
        {
            if (source == null)
            {
                throw new Exception("source is null");
            }

            if (source.scene.name != null)
            {
                throw new Exception("source GameObject is in scene");
            }

            _resManager = resourceManager;
            _sourceGameObject = source;
            return this;
        }

        public AssetsReference Ref<T>(T source, ResManager resManager = null) where T : UnityEngine.Object
        {
            if (source == null)
            {
                throw new Exception("source object is null");
            }

            _resManager = resManager;
            _refAssetInfoList = new List<AssetsRefInfo>();
            _refAssetInfoList.Add(new AssetsRefInfo(source));
            return this;
        }

        public static AssetsReference Instantiate(GameObject source, Transform parent = null,
            ResManager resourceManager = null)
        {
            if (source == null)
            {
                throw new Exception($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new Exception($"Source gameObject is in scene.");
            }

            GameObject instance = Object.Instantiate(source, parent);
            return instance.AddComponent<AssetsReference>().Ref(source, resourceManager);
        }

        public static AssetsReference Ref(GameObject source, GameObject instance, ResManager resourceManager = null)
        {
            if (source == null)
            {
                throw new Exception($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new Exception($"Source gameObject is in scene.");
            }

            return instance.GetOrAddComponent<AssetsReference>().Ref(source, resourceManager);
        }

        public static AssetsReference Ref<T>(T source, GameObject instance, ResManager resourceManager = null)
            where T : Object
        {
            if (source == null)
            {
                throw new Exception($"Source gameObject is null.");
            }

            return instance.GetOrAddComponent<AssetsReference>().Ref(source, resourceManager);
        }
    }
}