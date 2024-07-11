using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace LGF.FSM
{
    /// <summary>
    /// 有限状态机管理类
    /// </summary>
    internal sealed class FsmManager : IFsmManager
    {
        public readonly Dictionary<string, FsmBase> m_Fsms;
        private readonly List<FsmBase> m_TempFsms;


        /// <summary>
        /// 初始化有限状态机
        /// </summary>
        public FsmManager()
        {
            m_Fsms = new Dictionary<string, FsmBase>();
            m_TempFsms = new List<FsmBase>();
        }

        // TODO: 模块轮询有限级 后续研究
        internal int Proirity => 1;
        /// <summary>
        /// 获取有限状态机数量
        /// </summary>
        public int Count => m_Fsms.Count;

        /// <summary>
        /// 有限状态机轮询机
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，s</param>
        /// <param name="realElapseSeconds">真实流逝时间，s</param>
        internal void Update(float elapseSeconds, float realElapseSeconds)
        {
            m_TempFsms.Clear();
            if (m_Fsms.Count <= 0)
            {
                return;
            }

            foreach (KeyValuePair<string, FsmBase> fsm in this.m_Fsms)
            {
                m_TempFsms.Add(fsm.Value);
            }

            foreach (FsmBase tempFsm in this.m_TempFsms)
            {
                if (!tempFsm.IsDestroyed)
                {
                    tempFsm.Update(elapseSeconds, realElapseSeconds);
                }
            }
        }

        /// <summary>
        /// 关闭并清理有限状态机管理器
        /// </summary>
        internal void Shutdown()
        {
            foreach (KeyValuePair<string, FsmBase> fsm in this.m_Fsms)
            {
                fsm.Value.Shutdown();
            }

            this.m_Fsms.Clear();
            this.m_TempFsms.Clear();
        }
        
        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns></returns>
        public bool HasFsm<T>() where T : class
        {
            return InternalHasFsm(typeof(T).Name);
        }

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns></returns>
        public bool HasFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalHasFsm(ownerType.Name);
        }

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns></returns>
        public bool HasFsm<T>(string name) where T : class
        {
            return InternalHasFsm(typeof(T).Name + name);
        }

        public bool HasFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalHasFsm(ownerType.Name + name);
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns></returns>
        public IFsm<T> GetFsm<T>() where T : class
        {
            return (IFsm<T>)InternalGetFsm(typeof(T).Name);
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <returns></returns>
        public FsmBase GetFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalGetFsm(ownerType.Name);
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns></returns>
        public IFsm<T> GetFsm<T>(string name) where T : class
        {
            return (IFsm<T>)InternalGetFsm(typeof(T).Name + name);
        }

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns></returns>
        public FsmBase GetFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalGetFsm(ownerType.Name + name);
        }

        /// <summary>
        /// 获取所有的有限状态机
        /// </summary>
        /// <returns>所有的有限状态机</returns>
        public FsmBase[] GetAllFsms()
        {
            m_TempFsms.Clear();
            foreach (KeyValuePair<string, FsmBase> fsm in this.m_Fsms)
            {
                m_TempFsms.Add(fsm.Value);
            }

            return m_TempFsms.ToArray();
        }

        /// <summary>
        /// 获取所有的有限状态机
        /// </summary>
        /// <param name="results">所有的有限状态机</param>
        public void GetAllFsms(List<FsmBase> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, FsmBase> fsm in this.m_Fsms)
            {
                results.Add(fsm.Value);
            }
        }

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <param name="owner">有限状态机持有者对象</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <typeparam name="T">有限状态机持有者</typeparam>
        /// <returns></returns>
        public IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            return CreateFsm(string.Empty, owner, states);
        }
        
        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        public IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class
        {
            string typeName = typeof(T).Name + name;
            if (HasFsm<T>(name))
            {
                throw new Exception("Already exist FSM.");
            }

            Fsm<T> fsm = Fsm<T>.Create(name, owner, states);
            m_Fsms.Add(typeName, fsm);
            return fsm;
        }

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <returns></returns>
        public IFsm<T> CreateFsm<T>(T owner, List<FsmState<T>> states) where T : class
        {
            return CreateFsm(string.Empty, owner, states);
        }

        public IFsm<T> CreateFsm<T>(string name, T owner, List<FsmState<T>> states) where T : class
        {
            string typeName = typeof(T).Name + name;
            if (HasFsm<T>(name))
            {
                throw new Exception("Already exist FSM.");
            }

            Fsm<T> fsm = Fsm<T>.Create(name, owner, states);
            m_Fsms.Add(typeName, fsm);
            return fsm;
        }

        public bool DestroyFsm<T>() where T : class
        {
            return InternalDestroyFsm(typeof(T).Name);
        }

        public bool DestroyFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalDestroyFsm(ownerType.Name);
        }

        public bool DestroyFsm<T>(string name) where T : class
        {
            return InternalDestroyFsm(typeof(T).Name + name);
        }

        public bool DestroyFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalDestroyFsm(ownerType.Name + name);
        }

        private bool InternalHasFsm(string typeName)
        {
            return m_Fsms.ContainsKey(typeName);
        }

        private FsmBase InternalGetFsm(string typeName)
        {
            FsmBase fsm = null;
            if (m_Fsms.TryGetValue(typeName, out fsm))
            {
                return fsm;
            }

            return fsm;
        }

        private bool InternalDestroyFsm(string typeName)
        {
            FsmBase fsm = null;
            if (m_Fsms.TryGetValue(typeName, out fsm))
            {
                fsm.Shutdown();
                return m_Fsms.Remove(typeName);
            }

            return false;
        }
    }
}