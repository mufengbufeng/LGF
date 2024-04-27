using System;
using System.Collections.Generic;
using LGF.Path;
using UnityEngine;

namespace LGF.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public static Delegate GetViewPath; // 获取View路径
        public static Delegate GetPrefabPath; // 获取Prefab路径
        public static Delegate CheckViewExist; // 检查View是否存在
        private readonly Dictionary<string, IUIRoot> m_UIForms = new();
        private GameObject _canvas;

        public override void Awake()
        {
            base.Awake();
            _canvas = GameObject.Find("Canvas");
        }

        public IUIRoot Open<T>(string uiFormName = null, object userData = null, int uiDepth = 1) where T : IUIRoot
        {
            if (string.IsNullOrEmpty(uiFormName)) uiFormName = typeof(T).ToString();

            if (m_UIForms.TryGetValue(uiFormName, out var open))
            {
                return open;
            }

            string uiPath = PathConfig.GetUIPrefabPath(uiFormName);
            if (string.IsNullOrEmpty(uiPath))
            {
                Debug.LogError($"UIManager Open Error: uiPath is null, uiFormName = {uiFormName}");
                return null;
            }

            GameObject uiObj = Game.resManager.LoadGameObject(uiPath, _canvas.transform);

            T uiForm = gameObject.AddComponent<T>();
            uiForm.Init(uiFormName, uiDepth, userData);
            uiForm.transform.SetParent(_canvas.transform);

            m_UIForms.Add(uiFormName, uiForm);

            Game.eventManager.Trigger(UIEvent.UI_OPEN, uiFormName);
            Debug.Log($"UIManager Open: {uiFormName}");
            return uiForm;
        }

        public IUIRoot Open(string uiFormName, object useData = null, int uiDepth = 1)
        {
            if (m_UIForms.TryGetValue(uiFormName, out var open))
            {
                return open;
            }

            string uiPath = PathConfig.GetUIPrefabPath(uiFormName);
            if (string.IsNullOrEmpty(uiPath))
            {
                Debug.LogError($"UIManager Open Error: uiPath is null, uiFormName = {uiFormName}");
                return null;
            }

            IUIRoot uiForm = Game.resManager.LoadAsset<IUIRoot>(uiPath);
            uiForm.Init(uiFormName, uiDepth, useData);
            uiForm.transform.SetParent(_canvas.transform);

            m_UIForms.Add(uiFormName, uiForm);
            Game.eventManager.Trigger(UIEvent.UI_OPEN, uiFormName);
            Debug.Log($"UIManager Open: {uiFormName}");
            return uiForm;
        }

        public void Close<T>(string uiFormName) where T : IUIRoot
        {
            if (m_UIForms.ContainsKey(uiFormName))
            {
                m_UIForms[uiFormName].OnClose();

                Remove(m_UIForms[uiFormName]);

                Game.eventManager.Trigger(UIEvent.UI_CLOSE, uiFormName);
                Debug.Log($"UIManager Close: {uiFormName}");
            }
        }

        public void CloseAll()
        {
            foreach (var uiForm in m_UIForms)
            {
                uiForm.Value.OnClose();
            }

            m_UIForms.Clear();
        }

        public void Remove(IUIRoot uiITem)
        {
            if (m_UIForms.ContainsKey(uiITem.UIName))
            {
                uiITem.OnDestroy();
                m_UIForms.Remove(uiITem.UIName);
            }
        }
    }
}