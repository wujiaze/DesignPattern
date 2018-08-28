using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class OpenMachine : StateMachine
{
    private Transition open2close;
    public override void InitTransitions(StateManager manager)
    {
        open2close = SetTransition(manager, StateName.Close, TransitionName.Open2Close);
        AddTransition(open2close);
    }

    public override Transition SetTransition(StateManager manager, StateName toName, TransitionName transitionName)
    {
        StateMachine machine = manager.GetMachineWithName(Machine.Name);
        State toState = machine.GetStateWithName(toName);
        if (toState == null)
            throw new Exception(toName + "为null");
        Transition temp = new Transition(transitionName, toState);
        temp.onCheck += OnCheck;
        temp.onTransition += () => FadeTo(0);
        return temp;
    }
    private Test _test;
    private Light _light;
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

    public override void OnStateEnter()
    {

    }
    public override void OnStateExit()
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
