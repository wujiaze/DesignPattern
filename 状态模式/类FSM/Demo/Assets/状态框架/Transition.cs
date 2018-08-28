using System;

namespace FSM
{
    public class Transition
    {
        #region 属性
        /// <summary>
        /// 状态过渡名字
        /// </summary>
        public TransitionName Name { get; private set; }
        /// <summary>
        /// 下一个状态
        /// </summary>
        public State ToState { get; private set; }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toState"></param>
        public Transition(TransitionName name, State toState)
        {
            Name = name;
            ToState = toState;
        }

        #region 委托
        /// <summary>
        /// 检测条件
        /// </summary>
        public Func<bool> onCheck { get; set; }

        /// <summary>
        /// 转换过程的回调
        ///     可以是一种延时方法
        /// </summary>
        public Func<bool> onTransition { get; set; }

        #endregion


        #region 方法
        /// <summary>
        /// 检查本状态过渡是否满足过渡条件
        /// </summary>
        /// <returns>false：不满足条件  true：满足条件</returns>
        public bool Check()
        {
            if (onCheck != null)
                return onCheck();
            return false;
        }

        /// <summary>
        /// 状态过渡的回调
        /// </summary>
        /// <returns>false：状态过渡没有完成 true：状态过渡完成</returns>
        public bool TransitionCallBack()
        {
            if (onTransition != null)
                return onTransition();
            return true;
        }


        #endregion


    }
}
