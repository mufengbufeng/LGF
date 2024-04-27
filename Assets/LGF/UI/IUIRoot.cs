using System;
using UnityEngine;

namespace LGF.UI
{
    public abstract class IUIRoot : MonoBehaviour
    {
        public int SerialId { get; }
        public string UIName { get; private set; }
        public object Handle { get; private set; }
        public int UIDepth { get; private set; }

        public void Init(string uiName, int uiDepth, object userData)
        {
            UIName = uiName;
            UIDepth = uiDepth;
            Handle = userData;
            OnInit(uiName, uiDepth, userData);
        }

        /**
         *  UI 初始化
         * @param uiName UI 名称
         * @param uiDepth UI 深度
         * @param userData UI 参数
         */
        protected abstract void OnInit(string uiName, int uiDepth, object userData);

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnRecycle()
        {
        }

        // public abstract void OnOpen(object userData);
        public abstract void OnClose();


        protected virtual void DepthChange(int uiDepth)
        {
        }

        public abstract void OnDestroy();
    }
}