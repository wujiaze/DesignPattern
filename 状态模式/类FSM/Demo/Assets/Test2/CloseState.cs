using System;
using UnityEngine;
using System.Collections;
using FSM;
using System.Collections.Generic;

public class CloseState : State
{
    private Test _test;
    private Light _light;
    private Transition close2Open;

    public override void InitTransitions(StateManager manager)
    {
        close2Open = SetTransition(manager,StateName.OpenMachine,TransitionName.Close2Open);
        close2Open.onCheck += OnCheck;
        close2Open.onTransition += OnTransition;
        AddTransition(close2Open);
    }

   
    public override void SetObject(object obj)
    {
        _test = (Test) obj;
        _light = _test._light;
    }

    /// <summary>
    /// A2B 的转换条件
    /// </summary>
    /// <returns></returns>
    private bool OnCheck()
    {
        return _test.IsOpen;
    }
    private bool OnTransition()
    {
        return FadeTo(2);
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
