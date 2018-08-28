using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FSM;
using UnityEngine;

public class Test : MonoBehaviour
{
    private StateMachine _fsm;              // 主状态机
    private CloseState _close;              // 关闭状态
    private OpenMachine _openfsm;           // 打开的子状态机
    private IntensityState _intensity;      // 强度状态
    private ColorState _color;              // 颜色状态

    // UI
    public Light _light;                   // 对象
    void Start()
    {
        Application.runInBackground = true;
        _light = GameObject.Find("Directional Light").GetComponent<Light>();


        #region 状态机的设置
        // 状态机 和 状态 
        _fsm = new MainStateMachine(StateName.MainMachine, null);
        _close = _fsm.AddState<CloseState>(StateName.Close);
        // 子状态
        _openfsm = _fsm.AddState<OpenMachine>(StateName.OpenMachine);
        _intensity = _openfsm.AddState<IntensityState>(StateName.Intensity);
        _color = _openfsm.AddState<ColorState>(StateName.Color);
        // 默认状态
        _openfsm.DefaultState = _intensity;
        _fsm.DefaultState = _close;
        // 设置所有的状态转换
        StateManager manager = new StateManager();
        manager.AddState(_fsm, _openfsm, _intensity, _color, _close);
        manager.SetTrasitions();
        #endregion

        // 具体项目的设置  将改变的对象传入
        manager.SetObject(this);
    }

    public bool IsOpen { get; set; }

    public bool IsColor { get; set; }

    void Update()
    {
        _fsm.OnStateUpdate(Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.A))
        {
            IsOpen = !IsOpen;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            IsColor = !IsColor;
        }
    }
}
