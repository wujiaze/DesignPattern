
using FSM;
using UnityEngine;

public class Test : MonoBehaviour
{
    private StateManager manager;
    private StateMachine _fsm;              // 主状态机
    private CloseState _close;              // 关闭状态
    private OpenMachine _openfsm;           // 打开的子状态机
    private IntensityState _intensity;      // 强度状态
    private ColorState _color;              // 颜色状态

    // UI
    [HideInInspector]
    public Light _light;                   // 对象
    void Start()
    {
        Application.runInBackground = true;
        _light = GameObject.Find("Directional Light").GetComponent<Light>();

        #region 状态机的设置
        // 状态机
        _fsm = new MainStateMachine(StateName.MainMachine);
        // “关闭”状态 
        _close = _fsm.AddState<CloseState>(StateName.Close,true);
        // “打开”子状态机
        _openfsm = _fsm.AddState<OpenMachine>(StateName.OpenMachine);
        _intensity = _openfsm.AddState<IntensityState>(StateName.Intensity,true);// “亮度” 状态
        _color = _openfsm.AddState<ColorState>(StateName.Color);// “颜色” 状态
        // 设置所有的状态转换
        manager = new StateManager(_fsm, _openfsm, _intensity, _color, _close);
        // 具体项目的设置  将改变的对象传入
        manager.SetObject(this);
        #endregion
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            // 状态机的内容
            manager.ReInit();
            IsOpen = false;
            IsColor = false;
            // 具体的内容
            _light.color = Color.red;
            _light.intensity = 0;
        }
    }
}
