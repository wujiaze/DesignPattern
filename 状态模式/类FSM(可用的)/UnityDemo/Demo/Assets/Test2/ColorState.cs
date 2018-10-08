using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class ColorState : State
{

    private Transition color2Intensity;
    public override void InitTransitions(StateManager manager)
    {
        color2Intensity = SetTransition(manager, StateName.Intensity, TransitionName.Color2Intensity);
        color2Intensity.onCheck += OnCheck;
        AddTransition(color2Intensity);
        onEnter += OnEnter;
        onStayUpdate += OnStayUpdate;
    }



    private bool IsAnimation;
    private float _colorTimer = 0;
    private Light _light;
    private Color _targetColor;
    private Color _startColor;
    private Test _test;
    public override void SetObject(object obj)
    {
        _test = (Test)obj;
        _light = _test._light;
    }
    /// <summary>
    /// A2B 的转换条件
    /// </summary>
    /// <returns></returns>
    private bool OnCheck()
    {
        return !_test.IsColor;
    }
    private void OnStayUpdate(float deltatime)
    {
        if (IsAnimation)
        {
            _colorTimer += deltatime * 1;
            if (_colorTimer >= 1)
            {
                _light.color = _targetColor;
                IsAnimation = false;
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
            IsAnimation = true;
        }
    }

    private void OnEnter()
    {
        IsAnimation = false;
    }
}
