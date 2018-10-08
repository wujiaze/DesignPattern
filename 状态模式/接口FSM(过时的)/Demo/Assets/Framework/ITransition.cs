

namespace FSM
{
    /// <summary>
    /// 用于状态过渡 的接口
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        IState FromState { get; set; }
        /// <summary>
        /// 下一个状态
        /// </summary>
        IState ToState { get; set; }

        /// <summary>
        /// 过渡状态的名字
        /// </summary>
        string Name { get; set; }


        /// <summary>
        /// 判断能否开始过渡
        /// </summary>
        /// <returns>true：可以过渡 false:还不能过渡</returns>
        bool CheckTransition();

        /// <summary>
        /// 过渡的回调
        /// </summary>
        /// <returns>true:过渡完成 false：过渡进行中</returns>
        bool TransitionCallBack();

       
    }



}
