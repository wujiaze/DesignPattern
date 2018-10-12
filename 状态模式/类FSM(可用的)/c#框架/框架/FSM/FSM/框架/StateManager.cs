/*
 *      本框架的使用方法
 *               1、添加具体状态类 继承 State
 *                  在 InitTransitions 中使用 AddTransition 方法添加需要的状态过渡，并且设置好状态过渡的检测条件
 *                  在 SetObject 中设置好状态中需要使用到的引用
 *               2、添加具体状态机类 继承 StateMachine
 *                  在 InitTransitions 中使用 AddTransition 方法添加需要的状态过渡，并且设置好状态过渡的检测条件
 *                  在 SetObject 中设置好状态中需要使用到的引用
 *               3、需要一个 MachineManager 的对象收集 某一个父状态机相关联的所有状态机及状态
 *               
 *               
 *      注意事项：1、一个 MachineManager 中的同一父状态机下的状态名字不能相同，不同状态机之间的状态名字可以相同
 *               2、AddState 方法既可以添加状态，也可以添加状态机
 *               3、本框架只适用于两层架构  MainMachine + 子状态机  todo 不是很确定
 *      修改框架的注意点： State 类中 OnStateUpdate 和 OnStateLateUpdate ，_durationTime 只能出现一个
 */
using System.Collections.Generic;
namespace FSM
{
    public class StateManager
    {
        #region 框架
        public List<State> States { get; private set; }

        public StateManager()
        {
            States = new List<State>();
        }

        /// <summary>
        /// 初始化该状态机中所有状态的所有状态过渡
        /// 一定要在所有状态设置完毕之后，再使用本方法
        /// </summary>
        public void SetTrasitions()
        {
            foreach (State state in States)
            {
                state.InitTransitions(this);
            }
        }
        /// <summary>
        /// 给状态设置必要的引用
        /// </summary>
        /// <param name="obj"></param>
        public void SetObject(object obj)
        {
            foreach (State state in States)
            {
                state.SetObject(obj);
            }
        }
        /// <summary>
        /// 根据名字获取状态机
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StateMachine GetMachineWithName(StateName name)
        {
            StateMachine result = null;
            foreach (State machine in States)
            {
                if (machine.Name == name)
                {
                    result = (StateMachine)machine;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="states"></param>
        public void AddState(params State[] states)
        {
            foreach (State state in states)
            {
                if (!States.Contains(state))
                    States.Add(state);
            }
        }
        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="machines"></param>
        public void RemoveMachine(params State[] states)
        {
            foreach (State state in states)
            {
                if (States.Contains(state))
                    States.Remove(state);
            }
        }


        #endregion

    }
}