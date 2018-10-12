using System;
using System.Collections.Generic;

namespace FSM
{
    public abstract class State
    {
        #region 委托

        public Action onEnter { get; set; }
        public Action onExit { get; set; }
        public Action<float> onStayUpdate { get; set; }
        public Action<float> onStayLateUpdate { get; set; }
        public Action onStayFixedUpdate { get; set; }

        #endregion
        #region 属性
        /// <summary>
        /// 状态名字
        /// </summary>
        public StateName Name { get; private set; }
        /// <summary>
        /// 状态标签
        /// </summary>
        public StateTag Tag { get; set; }
        /// <summary>
        /// 所属的状态机
        /// </summary>
        public StateMachine Machine { get; private set; }
        /// <summary>
        /// 状态过渡
        /// </summary>
        public Dictionary<TransitionName, Transition> Transitions { get; private set; }
        /// <summary>
        /// 当前状态已经进行的时长
        /// </summary>
        public float DurationTime { get; private set; }

        #endregion

        // 构造函数
        /// <summary>
        /// 用于内部状态的泛型构造
        /// </summary>
        protected State()
        {
        }

        /// <summary>
        /// 初始化状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="machine"></param>
        public virtual void InitState(StateName name, StateMachine machine)
        {
            Name = name;
            Machine = machine;
            Transitions = new Dictionary<TransitionName, Transition>();
            DurationTime = 0;
            RemoveStateEvent();
        }

        /// <summary>
        /// 初始化所有的状态过渡
        /// </summary>
        public abstract void InitTransitions(StateManager manager);

        /// <summary>
        /// 设置单一的状态过渡
        ///     同一个状态机下的状态之间的状态过渡
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="toStateName"></param>
        /// <param name="transitionName"></param>
        /// <returns></returns>
        protected Transition AddTransition(StateManager manager, StateName toStateName, TransitionName transitionName)
        {
            StateMachine machine = manager.GetMachineWithName(Machine.Name);
            State toState = machine.GetStateWithName(toStateName);
            if (toState == null)
                throw new Exception(toStateName + "为null");
            if (Transitions.ContainsKey(transitionName))
                throw new Exception("此过渡状态已添加");
            Transition transition = new Transition(transitionName, toState);
            Transitions.Add(transitionName, transition);
            return transition;
        }

        /// <summary>
        /// 传入状态中需要使用的引用
        /// </summary>
        /// <param name="obj"></param>
        public abstract void SetObject(object obj);



        /* 以下几个方法，在新的State子类中需要重写 */

        public virtual void OnStateEnter()
        {
            DurationTime = 0;
            if (onEnter != null)
                onEnter();
        }

        public virtual void OnStateExit()
        {
            DurationTime = 0;
            if (onExit != null)
                onExit();
        }

        public virtual void OnStateUpdate(float deltatime)
        {
            DurationTime += deltatime;
            if (onStayUpdate != null)
                onStayUpdate(deltatime);
        }
        public virtual void OnStateLateUpdate(float deltatime)
        {
            //DurationTime += deltatime;       // 与Update方法 只能存在一个
            if (onStayLateUpdate != null)
                onStayLateUpdate(deltatime);
        }
        public virtual void OnStateFixedUpdate()
        {
            if (onStayFixedUpdate != null)
                onStayFixedUpdate();
        }
        /// <summary>
        /// 移除所有事件（根据具体的项目添加具体的移除方法）
        /// </summary>
        public virtual void RemoveStateEvent()
        {
            onEnter = null;
            onExit = null;
            onStayUpdate = null;
            onStayLateUpdate = null;
            onStayFixedUpdate = null;
        }

    }
}
