using System;
using System.Collections.Generic;
using LGF.MVC;
using LGF.Path;
using Unity.VisualScripting;
using UnityEngine;

namespace LGF.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public static Delegate GetViewPath; // 获取View路径
        public static Delegate GetPrefabPath; // 获取Prefab路径
        public static Delegate CheckViewExist; // 检查View是否存在
        private readonly Dictionary<string, LGFView> m_UIForms = new();
        private GameObject _canvas;

        protected override void Awake()
        {
            base.Awake();
            _canvas = GameObject.Find("Canvas");
        }

        public LGFView Open<T>(object userData = null, int uiDepth = 1) where T : LGFView
        {
            string uiFormName = typeof(T).ToString();
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

            GameObject uiObj = Game.ResManager.LoadGameObject(uiPath, _canvas.transform);

            T uiForm = uiObj.AddComponent<T>();
            uiForm.Init(uiFormName, uiDepth, userData);
            uiForm.transform.SetParent(_canvas.transform);

            m_UIForms.Add(uiFormName, uiForm);

            Game.EventManager.Trigger(UIEvent.UI_OPEN, uiFormName);
            Debug.Log($"UIManager Open: {uiFormName}");
            return uiForm;
        }

        public LGFView Open(string uiFormName, object useData = null, int uiDepth = 1)
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

            GameObject uiObj = Game.ResManager.LoadGameObject(uiPath, _canvas.transform);
            // 通过名字反射得到类
            Type type = Type.GetType(uiFormName);

            LGFView uiForm = null;
            if (type != null)
            {
                uiForm = uiObj.GetComponent(type) as LGFView;
                if (uiForm == null && typeof(MonoBehaviour).IsAssignableFrom(type))
                {
                    uiForm = uiObj.AddComponent(type) as LGFView;
                }
            }

            uiForm?.Init(uiFormName, uiDepth, useData);
            uiObj.transform.SetParent(_canvas.transform);
            m_UIForms.Add(uiFormName, uiForm);
            Game.EventManager.Trigger(UIEvent.UI_OPEN, uiFormName);
            Debug.Log($"UIManager Open: {uiFormName}");
            return uiForm;
        }

        public void Close<T>(string uiFormName) where T : LGFView
        {
            if (m_UIForms.ContainsKey(uiFormName))
            {
                m_UIForms[uiFormName].OnClose();

                Remove(m_UIForms[uiFormName]);

                Game.EventManager.Trigger(UIEvent.UI_CLOSE, uiFormName);
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

        public void Remove(LGFView uiITem)
        {
            if (m_UIForms.ContainsKey(uiITem.UIName))
            {
                uiITem.OnDestroy();
                m_UIForms.Remove(uiITem.UIName);
            }
        }
    }
}