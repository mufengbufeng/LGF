using System;
using System.Collections.Generic;

namespace LGF.FSM
{
    internal sealed class Fsm<T> : FsmBase, IFsm<T> where T : class
    {
        private T m_Owner;
        private readonly Dictionary<Type, FsmState<T>> m_States;
        private Dictionary<string, Variable> m_Datas;
        private FsmState<T> m_CurrentState;
        private float m_CurrentStateTime;
        private bool m_IsDestroyed;

        public Fsm()
        {
            m_Owner = null;
            m_States = new Dictionary<Type, FsmState<T>>();
            m_Datas = new Dictionary<string, Variable>();
            m_CurrentState = null;
            m_CurrentStateTime = 0f;
            m_IsDestroyed = false;
        }

        public override Type OwnerType { get; } = typeof(T);

        public T Owner => m_Owner;
        public override int FsmStateCount => m_States.Count;
        public override bool IsRunning => m_CurrentState != null;
        public override bool IsDestroyed => m_IsDestroyed;
        public FsmState<T> CurrentState => m_CurrentState;
        public float CurrentStateTime => m_CurrentStateTime;
        public override string CurrentStateName => m_CurrentState?.GetType().FullName;

        /// <summary>
        /// 创建有限状态机j
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <returns>创建有限状态机</returns>
        public static Fsm<T> Create(string name, T owner, params FsmState<T>[] states)
        {
            if (owner == null)
            {
                throw new Exception("FSM owner is invalid.");
            }

            if (states == null || states.Length < 1)
            {
                throw new Exception("FSM states is invalid.");
            }

            Fsm<T> fsm = new Fsm<T>();
            fsm.Name = name;
            fsm.m_Owner = owner;
            fsm.m_IsDestroyed = false;
            foreach (var state in states)
            {
                if (state == null)
                {
                    throw new Exception("FSM states is invalid.");
                }

                Type stateType = state.GetType();
                if (fsm.m_States.ContainsKey(stateType))
                {
                    throw new Exception("FSM states is invalid.");
                }

                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态机集合</param>
        /// <returns>创建的有限状态机</returns>
        public static Fsm<T> Create(string name, T owner, List<FsmState<T>> states)
        {
            if (owner == null)
            {
                throw new Exception("FSM owner is invalid.");
            }

            if (states == null || states.Count < 1)
            {
                throw new Exception("FSM states is invalid.");
            }

            Fsm<T> fsm = new Fsm<T>();
            fsm.Name = name;
            fsm.m_Owner = owner;
            fsm.m_IsDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new Exception("FSM states is invalid.");
                }

                Type stateType = state.GetType();
                if (fsm.m_States.ContainsKey(stateType))
                {
                    throw new Exception("FSM states is invalid.");
                }

                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        /// <summary>
        /// 清理有限状态机
        /// </summary>
        public void Clear()
        {
            m_CurrentState?.OnLeave(this, true);
            foreach (var state in m_States)
            {
                state.Value.OnDestroy(this);
            }

            Name = null;
            m_Owner = null;
            m_States.Clear();

            if (m_Datas != null)
            {
                //  因为现在没有实现引用池，所以这里不需要回收数据
                // TODO:如果实现了引用池，这里需要回收数据
                m_Datas.Clear();
            }

            m_CurrentState = null;
            m_CurrentStateTime = 0f;
            m_IsDestroyed = true;
        }

        /// <summary>
        /// 开始有限状态机 
        /// </summary>
        /// <typeparam name="TState">要开始的有限状态机状态类型</typeparam>
        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new Exception("FSM is running, can not start again.");
            }

            FsmState<T> state = GetState<TState>();
            if (state == null)
            {
                throw new Exception("FSM state is invalid.");
            }

            m_CurrentState = state;
            m_CurrentStateTime = 0f;
            m_CurrentState.OnEnter(this);
        }

        /// <summary>
        /// 开始有限状态机
        /// </summary>
        /// <param name="stateType">要开始的有限状态机类型</param>
        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new Exception("FSM is running, can not start again.");
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new Exception("FSM state is invalid.");
            }

            m_CurrentState = state;
            m_CurrentStateTime = 0f;
            m_CurrentState.OnEnter(this);
        }

        /// <summary>
        /// 是否存在有限状态机状态
        /// </summary>
        /// <typeparam name="TState">要检查的有限状态机机类型</typeparam>
        /// <returns>是否存在有限状态机状态</returns>
        public bool HasState<TState>() where TState : FsmState<T>
        {
            return m_States.ContainsKey(typeof(TState));
        }

        /// <summary>
        /// 是否存在有限状态机状态
        /// </summary>
        /// <param name="stateType">要检查的有限状态机状态</param>
        /// <returns>是否存在有限状态机状态</returns>
        public bool HasState(Type stateType)
        {
            return m_States.ContainsKey(stateType);
        }

        /// <summary>
        /// 获取有限状态机状态
        /// </summary>
        /// <typeparam name="TState">要获取的有限状态机状态类型</typeparam>
        /// <returns>要获取的有限状态机状态</returns>
        public TState GetState<TState>() where TState : FsmState<T>
        {
            FsmState<T> state = null;
            if (m_States.TryGetValue(typeof(TState), out state))
            {
                return (TState)state;
            }

            return null;
        }

        /// <summary>
        /// 获取有限状态机状态
        /// </summary>
        /// <param name="stateType">要获取的有限状态机状态类型</param>
        /// <returns>要获取的有限状态机状态</returns>
        public FsmState<T> GetState(Type stateType)
        {
            FsmState<T> state = null;
            if (m_States.TryGetValue(stateType, out state))
            {
                return state;
            }

            return null;
        }

        /// <summary>
        /// 获取有限状态机所有状态
        /// </summary>
        /// <returns></returns>
        public FsmState<T>[] GetAllStates()
        {
            int index = 0;
            FsmState<T>[] results = new FsmState<T>[m_States.Count];
            foreach (var state in m_States)
            {
                results[index++] = state.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取有限状态机所有状态
        /// </summary>
        public void GetAllStates(List<FsmState<T>> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var state in m_States)
            {
                results.Add(state.Value);
            }
        }

        /// <summary>
        /// 是否存在有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <returns>有限状态机是否存在</returns>
        public bool HasData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                return false;
            }

            return m_Datas.ContainsKey(name);
        }

        public TData GetData<TData>(string name) where TData : Variable
        {
            return (TData)GetData(name);
        }

        public Variable GetData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                return null;
            }

            Variable data = null;
            if (m_Datas.TryGetValue(name, out data))
            {
                return data;
            }

            return null;
        }

        public void SetData<TData>(string name, TData data) where TData : Variable
        {
            SetData(name, (Variable)data);
        }

        public void SetData(string name, Variable data)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                throw new Exception("FSM is invalid.");
            }

            // TODO: 移除池中的状态
            // Variable oldData = GetData(name);
            // if (oldData != null)
            // {
            //     
            // }

            m_Datas[name] = data;
        }

        /// <summary>
        /// 移除有限状态机数据
        /// </summary>
        /// <param name="name">有限状态机数据名称</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Data name is invalid");
            }

            if (m_Datas == null)
            {
                return false;
            }

            // TODO: 移除池中的状态
            // Variable oldData = GetData(name);
            // if (oldData != null)
            // {
            //          
            // }
            return m_Datas.Remove(name);
        }


        /// <summary>
        /// 有限状态机轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间,s</param>
        /// <param name="realElapseSeconds">真实流逝时间,s</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_CurrentState == null)
            {
                return;
            }

            m_CurrentStateTime += elapseSeconds;
            m_CurrentState.OnUpdate(this, elapseSeconds, realElapseSeconds);
        }

        internal override void Shutdown()
        {
            // TODO: 没有池子，暂时先销毁自己
            this.Clear();
        }

        internal void ChangeState<TState>() where TState : FsmState<T>
        {
            ChangeState(typeof(TState));
        }

        internal void ChangeState(Type stateType)
        {
            if (m_CurrentState == null)
            {
                throw new Exception("Current state is invalid");
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new Exception("FSM state is invalid");
            }

            m_CurrentState.OnLeave(this, false);
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
    }
}