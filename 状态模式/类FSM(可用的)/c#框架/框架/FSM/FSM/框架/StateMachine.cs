using System;
using System.Collections.Generic;
namespace FSM
{
    public abstract class StateMachine : State
    {
        #region 属性
        /// <summary>
        /// 默认状态
        /// </summary>
        public State DefaultState { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public State CurrentState { get; private set; }
        /// <summary>
        /// 拥有的状态
        /// </summary>
        public Dictionary<StateName, State> DictStates { get; private set; }

        /// <summary>
        /// 当前正在进行的状态过渡
        ///     null:当前没有进行状态过渡
        ///     有值:当前正在进行状态过渡
        /// </summary>
        public Transition CurrentTransition { get; private set; }
        /// <summary>
        /// 是否进入状态过渡
        /// </summary>
        public bool IsTransition { get; private set; }

        #endregion
        // 构造函数
        /// <summary>
        /// 用于状态机的内部泛型构造
        /// </summary>
        protected StateMachine()
        {

        }
        /// <summary>
        /// 构造函数
        ///    使用构造函数创建 StateMachine 一般是最顶层,就不需要再有父状态机
        ///    但是，如不是最顶层，需要添加父状态机
        /// </summary>
        /// <param name="name">状态机的名字</param>
        /// <param name="machine">状态机所属的父状态机</param>
        protected StateMachine(StateName name, StateMachine machine = null)
        {
            this.InitState(name, machine);
        }
        /// <summary>
        /// 初始化状态机
        /// </summary>
        /// <param name="name"></param>
        /// <param name="machine"></param>
        public override void InitState(StateName name, StateMachine machine)
        {
            base.InitState(name, machine);
            DefaultState = null;
            CurrentState = null;
            DictStates = new Dictionary<StateName, State>();
            CurrentTransition = null;
        }

        /// <summary>
        /// 创建并添加状态
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">添加的状态名</param>
        /// <param name="isDefault">是否设置为默认状态</param>
        /// <returns>添加的状态</returns>
        public T AddState<T>(StateName name, bool isDefault = false) where T : State, new()
        {
            if (DictStates == null)
                throw new Exception("DictStates 为 null");
            if (DictStates.ContainsKey(name))
                throw new Exception("DictStates 中已存在  " + name);
            T state = new T();
            state.InitState(name, this);
            DictStates.Add(name, state);
            if (isDefault)
                DefaultState = state;
            return state;
        }

        /// <summary>
        /// 移除状态/状态机
        /// 不能是当前状态和默认状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns>false：移除失败  true：移除成功</returns>
        public bool RemoveState(StateName name)
        {
            bool result = false;
            if (CurrentState != null && CurrentState.Name != name && DefaultState != null && DefaultState.Name != name)
            {
                if (DictStates != null && DictStates.ContainsKey(name))
                {
                    // 删除状态
                    DictStates.Remove(name);
                    // 删除过渡到本状态的状态过渡
                    foreach (State state in DictStates.Values)
                    {
                        List<TransitionName> templist = new List<TransitionName>();
                        foreach (Transition transition in state.Transitions.Values)
                        {
                            if (transition.ToState.Name == name)
                            {
                                templist.Add(transition.Name);
                            }
                        }
                        foreach (TransitionName transitionName in templist)
                        {
                            state.Transitions.Remove(transitionName);
                        }
                    }
                    result = true;
                }
            }
            return result;
        }


        /// <summary>
        /// 根据名字获取状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns>null：没有该状态</returns>
        public State GetStateWithName(StateName name)
        {
            State state = null;
            DictStates.TryGetValue(name, out state);
            return state;
        }

        /// <summary>
        /// 根据Tag获取状态
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>null：没有该状态</returns>
        public State GetStateWithTag(StateTag tag)
        {
            foreach (State temp in DictStates.Values)
            {
                if (temp.Tag == tag)
                {
                    return temp;
                }
            }
            return null;
        }

        /// <summary>
        ///  重新初始化，清空 StateMachine 的运行后产生的内容
        /// </summary>
        public override void ReInit()
        {
            base.ReInit();
            CurrentState = null;
            CurrentTransition = null;
            IsTransition = false;
        }

        /* 以下两个方法在 状态机的子类中可以修改*/

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }

        /* 以下三个方法 一般不修改，作为状态机的启动函数*/
        public override void OnStateUpdate(float deltatime)
        {
            base.OnStateUpdate(deltatime);
            // 是否正在进行状态转换
            bool result = DoTransition();
            // 调用状态的Update方法
            if (result == false)
                CurrentState.OnStateUpdate(deltatime);
        }

        public override void OnStateLateUpdate(float deltatime)
        {
            base.OnStateLateUpdate(deltatime);
            // 是否正在进行状态转换
            bool result = DoTransition();
            // 调用状态的Update方法
            if (result == false)
                CurrentState.OnStateLateUpdate(deltatime);
        }

        public override void OnStateFixedUpdate()
        {
            base.OnStateFixedUpdate();
            // 是否正在进行状态转换
            bool result = DoTransition();
            // 调用状态的Update方法
            if (result == false)
                CurrentState.OnStateFixedUpdate();
        }

        /// <summary>
        /// 判断 状态过渡 是否进行
        /// </summary>
        /// <returns>true: 本帧不再执行当前状态的 OnStateUpdate 的方法 false：本帧执行当前状态的 OnStateUpdate 的方法 </returns>
        private bool DoTransition()
        {
            bool result = false;
            if (CurrentState == null)
            {
                if (DefaultState == null)
                    throw new Exception("默认状态不能为空");
                CurrentState = DefaultState;
            }
            // 判断是否正在进行状态过渡
            if (IsTransition)
            {
                result = true;
                // 判断状态过渡的回调是否执行完毕
                if (CurrentTransition.TransitionCallBack())
                {
                    // 执行状态过渡（就是状态的退出 和 进入方法）
                    CurrentState.OnStateExit();
                    CurrentState = CurrentTransition.ToState;
                    CurrentState.OnStateEnter();
                    IsTransition = false;
                    if (CurrentState.IsRunContineStartAndUpdate)
                        result = false;
                }
                return result;
            }
            // 当多个转换条件都满足，则只执行第一个转换
            foreach (Transition transition in CurrentState.Transitions.Values)
            {
                // 如果过渡动作执行到中途，取消过渡动作，则直接停止过渡动作，执行当前状态的 OnStateUpdate/OnStateLateUpdate/OnStateFixedUpdate 方法
                // 即可以中断/快速切换，不必等过渡动作完成
                if (transition.Check())
                {
                    result = true;
                    IsTransition = true;
                    CurrentTransition = transition;
                    /*
                     *  判断过渡动作是否执行完毕
                     *  true ：过渡动作完成，    本帧继续执行 旧状态的 OnStateExit 和 新状态的 OnStateEnter 方法
                     *  false：过渡动作正在进行， 本帧不执行 当前状态(旧状态)的 OnStateUpdate/OnStateLateUpdate/OnStateFixedUpdate 方法
                     *                          只执行当前的过渡动作
                     */
                    if (CurrentTransition.TransitionCallBack())// true：过渡动作完成  false：正在进行过渡动作
                    {
                        CurrentState.OnStateExit();
                        CurrentState = CurrentTransition.ToState;
                        CurrentState.OnStateEnter();
                        IsTransition = false;
                        /*
                         *  此时刚刚进入了新状态，IsRunContineStartAndUpdate 属性
                        *      1、true  ：本帧 执行   新状态的 OnStateUpdate/OnStateLateUpdate/OnStateFixedUpdate 方法
                        *      2、false ：本帧 不执行 新状态的 OnStateUpdate/OnStateLateUpdate/OnStateFixedUpdate 方法
                        */
                        if (CurrentState.IsRunContineStartAndUpdate)
                            result = false;
                    }
                    return result; // true 本帧不再执行当前状态的 OnStateUpdate 的方法，false 本帧执行当前状态的 OnStateUpdate 的方法
                }
            }
            return result;
        }
    }
}
