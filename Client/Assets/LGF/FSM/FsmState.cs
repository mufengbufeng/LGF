using System;

namespace LGF.FSM
{
    public abstract class FsmState<T> where T : class
    {
        public FsmState()
        {
        }
        
        protected internal virtual void OnInit(IFsm<T> fsm)
        {
        }
        
        protected internal virtual void OnEnter(IFsm<T> fsm)
        {
        }
        
        /// <summary>
        /// 有限状态机状态轮询时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用</param>
        /// <param name="elapseSeconds">逻辑流逝事件，s</param>
        /// <param name="realElapseSeconds">真实流逝时间，s</param>
        protected internal virtual void OnUpdate(IFsm<T> fsm, float elapseSeconds, float realElapseSeconds)
        {
        }
        
        /// <summary>
        /// 有限状态机离开时调用
        /// </summary>
        /// <param name="fsm">有限状态机引用</param>
        /// <param name="isShutdown">是否销毁有限状态机</param>
        protected internal virtual void OnLeave(IFsm<T> fsm, bool isShutdown)
        {
        }
        
        
        protected internal virtual void OnDestroy(IFsm<T> fsm)
        {
        }
        
        /// <summary>
        /// 切换当前有限状态机状态
        /// </summary>
        /// <param name="fsm"></param>
        /// <typeparam name="TState">要切换到的状态类型</typeparam>
        /// <exception cref="Exception"></exception>
        protected void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new Exception("FSM is invalid.");
            }
            fsmImplement.ChangeState<TState>();
        }

        protected void ChangeState(IFsm<T> fsm, Type stateType)
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new Exception("FSM is invalid");
            }

            if (stateType == null)
            {
                throw new Exception("State type is invalid");
            }
            
            if(!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new Exception("State type is invalid");
            }
            
            fsmImplement.ChangeState(stateType);
        }
    }
}