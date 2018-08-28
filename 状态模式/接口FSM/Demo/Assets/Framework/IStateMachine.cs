

namespace FSM
{
    /// <summary>
    /// 状态机接口
    /// </summary>
    public interface IStateMachine
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        IState DefalutState { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        IState CurrentState { get; }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state">添加的状态</param>
        void AddState(IState state);
        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="state">移除的状态</param>
        void RemoveState(IState state);
        /// <summary>
        /// 查找状态
        /// </summary>
        /// <param name="tag">查找状态的tag</param>
        /// <returns></returns>
        IState GetStateWithTag(string tag);
        /// <summary>
        /// 查找状态
        /// </summary>
        /// <param name="name">查找状态的name</param>
        /// <returns></returns>
        IState GetStateWithName(string name);
    }

}


