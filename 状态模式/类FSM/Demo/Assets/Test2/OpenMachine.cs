using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class OpenMachine : StateMachine
{
    private Test _test;
    private Light _light;
    private Transition open2close;

    public override void InitTransitions(StateManager manager)
    {
        open2close = SetTransition(manager, StateName.Close, TransitionName.Open2Close);
        open2close.onCheck += OnCheck;
        open2close.onTransition += OnTransition;
        AddTransition(open2close);
        onExit += OnExit;
    }

    


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
        return !_test.IsOpen;
    }
    private bool OnTransition()
    {
        return FadeTo(0);
    }
    private void OnExit()
    {
        _test.IsColor = false;
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


}
