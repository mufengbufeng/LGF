using System;
using System.Collections.Generic;

namespace LGF.FSM
{
    public interface IFsmManager
    {
        /// <summary>
        /// 获取有限状态机数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <typeparam name="T">持有者类型</typeparam>
        /// <returns></returns>
        bool HasFsm<T>() where T : class;

        /// <summary>
        /// 检查是否存在有限状态机 
        /// </summary>
        /// <param name="ownerType">持有者类型</param>
        /// <returns></returns>
        bool HasFsm(Type ownerType);

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <typeparam name="T">持有者类型</typeparam>
        /// <returns></returns>
        bool HasFsm<T>(string name) where T : class;

        /// <summary>
        /// 检查是否存在有限状态机
        /// </summary>
        /// <param name="ownerType">持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns></returns>
        bool HasFsm(Type ownerType, string name);

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <typeparam name="T">持有者类型</typeparam>
        /// <returns></returns>
        IFsm<T> GetFsm<T>() where T : class;

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">持有者类型</param>
        /// <returns></returns>
        FsmBase GetFsm(Type ownerType);

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="name">持有者类型</param>
        /// <typeparam name="T">有限状态机名称</typeparam>
        /// <returns></returns>
        IFsm<T> GetFsm<T>(string name) where T : class;

        /// <summary>
        /// 获取有限状态机
        /// </summary>
        /// <param name="ownerType">持有者类型</param>
        /// <param name="name">有限状态机名称</param>
        /// <returns></returns>
        FsmBase GetFsm(Type ownerType, string name);

        /// <summary>
        /// 获取所有有限状态机
        /// </summary>
        /// <returns></returns>
        FsmBase[] GetAllFsms();


        public void GetAllFsms(List<FsmBase> results);

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <param name="owner">持有者</param>
        /// <param name="states">状态集合</param>
        /// <typeparam name="T">持有者类型</typeparam>
        /// <returns></returns>
        IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class;

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        /// <typeparam name="T">持有者类型</typeparam>
        /// <param name="name">有限状态机名称</param>
        /// <param name="owner">有限状态机持有者</param>
        /// <param name="states">有限状态机状态集合</param>
        /// <returns></returns>
        IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class;

        /// <summary>
        /// 创建有限状态机 
        /// </summary>
        /// <typeparam name="T">持有者类型</typeparam>
        /// <param name="owner">持有者</param>
        /// <param name="states">状态集合</param>
        /// <returns></returns>
        IFsm<T> CreateFsm<T>(T owner, List<FsmState<T>> states) where T : class;


        public IFsm<T> CreateFsm<T>(string name, T owner, List<FsmState<T>> states) where T : class;
        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <typeparam name="T">持有者类型</typeparam>
        /// <returns></returns>
        bool DestroyFsm<T>() where T : class;

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="name">有限状态机名称</param>
        /// <typeparam name="T">持有者类型</typeparam>
        /// <returns></returns>
        bool DestroyFsm<T>(string name) where T : class;

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="ownerType">持有者类型</param>
        /// <returns></returns>
        bool DestroyFsm(Type ownerType);

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        /// <param name="ownerType">持有者类型</param>
        /// <param name="name">邮箱状态机名称</param>
        /// <returns></returns>
        bool DestroyFsm(Type ownerType, string name);
    }
}