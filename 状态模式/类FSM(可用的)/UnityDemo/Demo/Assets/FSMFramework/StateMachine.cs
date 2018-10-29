﻿using System;
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
        /// </summary>
        /// <param name="name">状态机的名字</param>
        /// <param name="machine">状态机所属的父状态机</param>
        protected StateMachine(StateName name, StateMachine machine)
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
        /// <returns>添加的状态</returns>
        public T AddState<T>(StateName name) where T : State, new()
        {
            if (DictStates == null)
                return null;
            if (DictStates.ContainsKey(name))
                return null;
            T state = new T();
            state.InitState(name, this);
            DictStates.Add(name, state);
            return state;
        }

        /// <summary>
        /// 移除状态
        /// 不能是当前状态和默认状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns>false：移除失败  true：移除成功</returns>
        public bool RemoveState(StateName name)
        {
            bool result = false;
            if (CurrentState.Name != name && DefaultState.Name != name)
            {
                if (DictStates != null && DictStates.ContainsKey(name))
                {
                    DictStates.Remove(name);
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取状态
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
        /// 获取状态
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

        /* 以下两个方法在 状态机的子类中可以修改*/

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }
        /* 以下三个方法 一般不修改*/
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
        /// 判断 状态过度 是否进行
        /// </summary>
        /// <returns>true: 状态转换了 false：还是当前状态</returns>
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
            foreach (Transition transition in CurrentState.Transitions.Values)
            {
                if (transition.Check())
                {
                    result = true;
                    IsTransition = true;
                    CurrentTransition = transition;
                    // 判断是否需要进行状态过渡方法, 如果是持续性方法则当前帧执行转换，就不再执行 OnStateUpdate 的方法
                    // 如果是 瞬时性方法 就可以直接执行，旧状态的 OnStateExit 和 新状态的 OnStateEnter 方法
                    // 若是本帧只需要 执行 OnStateEnter 方法，而不想执行 Update 方法，将 IsRunContineStartAndUpdate 设为 false
                    // 若本帧 新状态 需要连续执行 OnStateEnter 和 OnUpdate  方法，就是用默认的 true
                    if (CurrentTransition.TransitionCallBack())
                    {
                        CurrentState.OnStateExit();
                        CurrentState = CurrentTransition.ToState;
                        CurrentState.OnStateEnter();
                        if (CurrentState.IsRunContineStartAndUpdate)
                            result = false;
                        IsTransition = false;
                    }
                    return result; // 当前帧执行转换，就不再执行 OnStateUpdate 的方法
                }
            }
            return result;
        }
    }
}
