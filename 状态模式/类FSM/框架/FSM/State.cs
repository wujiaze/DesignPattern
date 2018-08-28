using System.Collections.Generic;

namespace FSM
{
    public abstract class State
    {

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
        /// 用于状态机的构造
        /// </summary>
        /// <param name="name"></param>
        /// <param name="machine"></param>
        protected State(StateName name, StateMachine machine)
        {
            InitState(name, machine);
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
        }

        /// <summary>
        /// 初始化所有的状态过渡
        /// </summary>
        public abstract void InitTransitions(StateManager manager);

        /// <summary>
        /// 设置单一的状态过渡
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="toName"></param>
        /// <param name="transitionName"></param>
        /// <returns></returns>
        public abstract Transition SetTransition(StateManager manager, StateName toName, TransitionName transitionName);

        /// <summary>
        /// 传入状态中需要使用的引用
        /// </summary>
        /// <param name="obj"></param>
        public abstract void SetObject(object obj);

        /// <summary>
        /// 添加状态过渡
        ///     辅助：InitTransitions 方法
        /// </summary>
        /// <param name="transition">状态过渡</param>
        protected void AddTransition(Transition transition)
        {
            if (Transitions != null && !Transitions.ContainsKey(transition.Name))
            {
                Transitions.Add(transition.Name, transition);
            }
        }


        /* 以下几个方法，在新的State子类中需要重写 */

        public virtual void OnStateEnter()
        {
            DurationTime = 0;
        }

        public virtual void OnStateExit()
        {
            DurationTime = 0;
        }

        public virtual void OnStateUpdate(float deltatime)
        {
            DurationTime += deltatime;
        }
        public virtual void OnStateLateUpdate(float deltatime)
        {
            //DurationTime += deltatime;       // 与Update方法 只能存在一个
        }
        public virtual void OnStateFixedUpdate()
        {
        }


    }
}
