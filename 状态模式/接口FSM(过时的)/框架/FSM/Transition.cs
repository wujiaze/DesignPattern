using System;
namespace FSM
{
    /// <summary>
    /// 用于状态过渡
    /// </summary>
    public class Transition : ITransition
    {
        // 事件
        public Func<bool> onTransition;     // 过渡中的回调
        public Func<bool> onCheck;          // 开始过渡的条件

        // 字段
        private IState _fromState;          // from状态
        private IState _toState;            // to状态
        private string _name;               // 过渡状态名字

        // 属性
        public IState FromState
        {
            get { return _fromState; }
            set { _fromState = value; }
        }

        public IState ToState
        {
            get { return _toState; }
            set { _toState = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //构造函数
        public Transition(string name, IState fromState, IState toState)
        {
            _name = name;
            _fromState = fromState;
            _toState = toState;
        }

        /// <summary>
        /// 判断能否开始过渡
        /// </summary>
        /// <returns>true：可以过渡 false:还不能过渡</returns>
        public bool CheckTransition()
        {
            if (onCheck != null)
                return onCheck();
            else
                return false;
        }

        /// <summary>
        /// 过渡的回调
        ///     从当前状态 进入 过渡状态 时的回调
        /// </summary>
        /// <returns>true:过渡完成 false：过渡进行中</returns>
        public bool TransitionCallBack()
        {
            if (onTransition != null)
                return onTransition();
            return true;
        }
    }
}
