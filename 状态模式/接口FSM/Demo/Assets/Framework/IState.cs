
using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 状态名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 状态标签
        /// </summary>
        string Tag { get; }

        /// <summary>
        /// 当前状态所属的状态机
        /// </summary>
        IStateMachine StateMachine { get; set; }

        /// <summary>
        /// 进入状态累计的时长
        /// </summary>
        float TimerFromEnter { get; }


        /// <summary>
        /// 状态过渡
        ///     返回当前状态的所有可能的过渡
        /// </summary>
        List<ITransition> TransitionList { get; }





        /// <summary>
        /// 添加过渡状态
        /// </summary>
        /// <param name="transition"></param>
        void AddTransition(ITransition transition);


        /// <summary>
        /// 进入状态的回调
        /// </summary>
        void OnEnterState();

        /// <summary>
        /// 退出状态的回调
        /// </summary>
        void OnExitState();

        /// <summary>
        /// Update 方法中的回调
        /// </summary>
        /// <param name="deltaTime">TIme.deltaTime</param>
        void OnUpdateExecute(float deltaTime);

        /// <summary>
        /// LateUpdate 方法中的回调
        /// </summary>
        /// <param name="deltaTime">TIme.deltaTime</param>
        void OnLateUpdateExecute(float deltaTime);

        /// <summary>
        /// FixedUpdate 方法中的回调
        /// </summary>
        void OnFixedUpdateExecute();

        /// <summary>
        /// 移除事件
        /// </summary>
        void RemoveStateEvent();

    }


}
