using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class LightTest : MonoBehaviour
{
    // 状态机
    private StateMachine _fsm;
    // 状态
    private State _close;
    private StateMachine _open; // 子状态机
    private State _intensity;
    private State _color;
    // 状态的切换
    private Transition _open2close;
    private Transition _close2open;
    // 子状态的切换
    private Transition _intensity2color;
    private Transition _color2intensity;

    // 字段
    public bool _isOpen;
    public bool _isColor;
    public float _speed;
    private Light _light;
    private float _maxinten = 2;
    private bool _isIntenMax; // 是否最亮了
    private bool _isAnimation;
    private Color _targetColor;
    private Color _startColor;
    private float _colorTimer;
    private void Awake()
    {
        Application.runInBackground = true;
        _light = GetComponent<Light>();
    }

    void Start()
    {
        // 初始化 状态
        _close = new State("close");
        _intensity = new State("intensity");
        _color = new State("color");
        _open = new StateMachine("open", _intensity);
        _open.AddState(_color);
        // 初始化 状态过渡
        _open2close = new Transition("_open2close", _open, _close);
        _open.AddTransition(_open2close);
        _close2open = new Transition("_close2open", _close, _open);
        _close.AddTransition(_close2open);
        _intensity2color = new Transition("_intensity2color", _intensity, _color);
        _intensity.AddTransition(_intensity2color);
        _color2intensity = new Transition("_color2intensity", _color, _intensity);
        _color.AddTransition(_color2intensity);
        // 初始化 状态机
        _fsm = new StateMachine("_fsm", _close);
        _fsm.AddState(_open);
        // 设置状态机事件
        _open.onExit += state => _isColor = false;
        // 设置状态的事件
        _intensity.onEnter += IntensityOnOnEnter;
        _intensity.onStayUpdate += IntensityOnOnStayUpdate;
        _color.onEnter += ColorOnOnEnter;
        _color.onStayUpdate += ColorOnOnStayUpdate;

        // 设置状态过渡事件
        _close2open.onCheck += () => _isOpen;
        _close2open.onTransition += () => FadeTo(2);
        _open2close.onCheck += () => !_isOpen;
        _open2close.onTransition += () => FadeTo(0);
        // 子状态之间过渡事件
        _intensity2color.onCheck += () => _isColor;
        _color2intensity.onCheck += () => !_isColor;
    }

    private bool FadeTo(float value)
    {
        if (Mathf.Abs(_light.intensity - value) <= 0.03f)
        {
            _light.intensity = value;
            return true;
        }
        else
        {
            if (value == 0)
                _light.intensity -= Time.deltaTime * 2;
            else
                _light.intensity += Time.deltaTime * 2;
            return false;
        }
    }

    private void ColorOnOnEnter(IState state)
    {
        _isAnimation = false;
    }

    private void IntensityOnOnEnter(IState state)
    {
        _isIntenMax = false;
    }

    private void IntensityOnOnStayUpdate(float timer)
    {
        if (!_isIntenMax)
        {
            if (_light.intensity < _maxinten)
            {
                _light.intensity += timer * _speed;
            }
            else
            {
                _light.intensity = _maxinten;
                _isIntenMax = true;
            }
        }
        else
        {
            if (_light.intensity > 0)
            {
                _light.intensity -= timer * _speed;
            }
            else
            {
                _light.intensity = 0;
                _isIntenMax = false;
            }
        }
    }

    private void ColorOnOnStayUpdate(float timer)
    {
        if (_isAnimation)
        {
            _colorTimer += Time.deltaTime * 1;
            if (_colorTimer >= 1)
            {
                _light.color = _targetColor;
                _isAnimation = false;
                return;
            }
            _light.color = Color.Lerp(_startColor, _targetColor, _colorTimer);
        }
        else
        {
            _startColor = _light.color;
            float r = UnityEngine.Random.Range(0f, 1f);
            float g = UnityEngine.Random.Range(0f, 1f);
            float b = UnityEngine.Random.Range(0f, 1f);
            _targetColor = new Color(r, g, b);
            _colorTimer = 0;
            _isAnimation = true;
        }
    }



    void Update()
    {
        _fsm.OnUpdateExecute(Time.deltaTime);
    }

    void OnGUI()
    {

    }

}
