using System;
using System.Collections.Generic;

namespace FSM
{
    public class State : IState
    {
        // 事件
        public event Action<IState> onEnter;        // 进入状态的回调
        public event Action<IState> onExit;         // 离开状态的回调
        public event Action<float> onStayUpdate;    // 停留在状态的(Update)回调
        public event Action<float> onStayLateUpdate;// 停留在状态的(LateUpdate)回调
        public event Action onStayFixedUpdate;      // 停留在状态的(FixedUpdate)回调

        // 字段
        private string _name;                       // 名字
        private string _tag;                        // 标签
        private IStateMachine _stateMachine;        // 所属的状态机
        private float _timerFromEnter;              // 计时器 Update中使用
        private List<ITransition> _transitionsList; // 状态过渡列表

        // 属性
        public string Name
        {
            get { return _name; }
        }
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public IStateMachine StateMachine
        {
            get { return _stateMachine; }
            set { _stateMachine = value; }
        }

        public float TimerFromEnter
        {
            get { return _timerFromEnter; }
        }

        public List<ITransition> TransitionList
        {
            get { return _transitionsList; }
        }

        // 构造
        public State(string name)
        {
            _name = name;
            _stateMachine = null;
            _timerFromEnter = 0;
            _transitionsList = new List<ITransition>();
            RemoveStateEvent();
        }


        /* 方法 */

        /// <summary>
        /// 添加状态过渡
        /// </summary>
        /// <param name="transition">过渡状态</param>
        public void AddTransition(ITransition transition)
        {
            if (transition != null && !_transitionsList.Contains(transition) && transition.FromState == this)
                _transitionsList.Add(transition);
        }

        /// <summary>
        /// 进入状态的回调
        /// </summary>
        public virtual void OnEnterState()
        {
            _timerFromEnter = 0;
            if (onEnter != null)
                onEnter(this);
        }

        /// <summary>
        /// 离开状态的回调
        /// </summary>
        public virtual void OnExitState()
        {
            if (onExit != null)
                onExit(this);
            _timerFromEnter = 0;
        }

        /// <summary>
        /// 停留状态的回调(Update)
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void OnUpdateExecute(float deltaTime)
        {
            _timerFromEnter += deltaTime; //累计时间
            if (onStayUpdate != null)
                onStayUpdate(deltaTime);
        }
        /// <summary>
        /// 停留状态的回调(LateUpdate)
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void OnLateUpdateExecute(float deltaTime)
        {
            // _timerFromEnter 已经在Update中累计了，所以这里就不用累计
            if (onStayLateUpdate != null)
                onStayLateUpdate(deltaTime);
        }

        /// <summary>
        /// 停留状态的回调(FixedUpdate)
        /// </summary>
        public virtual void OnFixedUpdateExecute()
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
