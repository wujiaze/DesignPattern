using System.Collections.Generic;

namespace FSM
{
    public class StateMachine : State, IStateMachine
    {
        // 字段
        private IState _defalutState;           // 默认状态
        private IState _currentState;           // 当前状态
        private List<IState> _statesList;       // 管理的状态列表
        private bool _isTransition;             // 是否正在过渡
        private ITransition _currentTransition; // 当前正在执行的状态过渡

        // 属性
        public IState DefalutState
        {
            get { return _defalutState; }
            set
            {
                AddState(value);
                _defalutState = value;
            }
        }

        public IState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        //构造函数
        public StateMachine(string name, IState defaultState) : base(name)
        {
            _defalutState = defaultState;
            _currentState = null;
            _isTransition = false;
            _currentTransition = null;
            _statesList = new List<IState>();

        }


        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state">状态</param>
        public void AddState(IState state)
        {
            if (state != null && !_statesList.Contains(state))
            {
                _statesList.Add(state);     // 加入状态机的状态列表
                state.StateMachine = this;  // 设置状态的状态机为当前状态机
                if (_defalutState == null)
                    _defalutState = state;
            }
        }
        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="state"></param>
        public void RemoveState(IState state)
        {
            if (state == _currentState)                        // 不可删除当前进行的状态
                return;
            if (state != null && _statesList.Contains(state))
            {
                _statesList.Remove(state);
                state.StateMachine = null;
                if (state == _defalutState)
                    _defalutState = _statesList.Count > 0 ? _statesList[0] : null;
            }
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IState GetStateWithName(string name)
        {
            foreach (IState state in _statesList)
            {
                if (state.Name == name)
                    return state;
            }
            return null;
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public IState GetStateWithTag(string tag)
        {
            foreach (IState state in _statesList)
            {
                if (state.Tag == tag)
                    return state;
            }
            return null;
        }


        /// <summary>
        /// 状态机的回调（Update）
        ///      也是状态机的启动方法
        /// </summary>
        /// <param name="deltaTime">即Time.deltaTime</param>
        public override void OnUpdateExecute(float deltaTime)
        {
            // 判断是否正在进行状态过渡
            if (_isTransition)
            {
                // 判断状态过渡的回调是否执行完毕
                if (_currentTransition.TransitionCallBack())
                {
                    // 执行状态过渡（就是状态的退出 和 进入方法）
                    DoTransition(_currentTransition);
                    _isTransition = false;
                }
                return;
            }
            // 开始运行时，给_currentState 赋值
            if (_currentState == null)
                _currentState = _defalutState;
            base.OnUpdateExecute(deltaTime);                   // 内部就是一个计时，实际运用查看是否需要
            //遍历 当前状态的 状态过渡列表：查看满足哪个条件，就执行过渡
            foreach (ITransition transition in _currentState.TransitionList)
            {
                if (transition.CheckTransition())
                {
                    _isTransition = true;
                    _currentTransition = transition;
                    return;
                }
            }
            // 没有满足任何一个过渡条件，就执行当前状态的 Update 回调
            _currentState.OnUpdateExecute(deltaTime);
        }
        /// <summary>
        /// 状态机的回调（LateUpdate）
        /// </summary>
        /// <param name="deltaTime">即Time.deltaTime</param>
        public override void OnLateUpdateExecute(float deltaTime)
        {
            // 判断是否正在进行状态过渡
            if (_isTransition)
            {
                // 判断状态过渡的回调是否执行完毕
                if (_currentTransition.TransitionCallBack())
                {
                    // 执行状态过渡（就是状态的退出 和 进入方法）
                    DoTransition(_currentTransition);
                    _isTransition = false;
                }
                return;
            }
            // 开始运行时，给_currentState 赋值
            if (_currentState == null)
                _currentState = _defalutState;
            base.OnLateUpdateExecute(deltaTime);                  // 内部就是一个计时，实际运用查看是否需要
            //遍历 当前状态的 状态过渡列表：查看满足哪个条件，就执行过渡
            foreach (ITransition transition in _currentState.TransitionList)
            {
                if (transition.CheckTransition())
                {
                    _isTransition = true;
                    _currentTransition = transition;
                    return;
                }
            }
            // 没有满足任何一个过渡条件，就执行当前状态的 LateUpdate 回调
            _currentState.OnLateUpdateExecute(deltaTime);
        }

        /// <summary>
        /// 状态机的回调（FixedUpdate）
        /// </summary>
        public override void OnFixedUpdateExecute()
        {
            // 判断是否正在进行状态过渡
            if (_isTransition)
            {
                // 判断状态过渡的回调是否执行完毕
                if (_currentTransition.TransitionCallBack())
                {
                    // 执行状态过渡（就是状态的退出 和 进入方法）
                    DoTransition(_currentTransition);
                    _isTransition = false;
                }
                return;
            }
            // 开始运行时，给_currentState 赋值
            if (_currentState == null)
                _currentState = _defalutState;
            base.OnFixedUpdateExecute();
            //遍历 当前状态的 状态过渡列表：查看满足哪个条件，就执行过渡
            foreach (ITransition transition in _currentState.TransitionList)
            {
                if (transition.CheckTransition())
                {
                    _isTransition = true;
                    _currentTransition = transition;
                    return;
                }
            }
            // 没有满足任何一个过渡条件，就执行当前状态的 FixedUpdate 回调
            _currentState.OnFixedUpdateExecute();
        }

        /// <summary>
        /// 进行状态过渡
        /// </summary>
        /// <param name="transition"></param>
        private void DoTransition(ITransition transition)
        {
            _currentState.OnExitState();
            _currentState = transition.ToState;
            _currentState.OnEnterState();
        }




    }
}
