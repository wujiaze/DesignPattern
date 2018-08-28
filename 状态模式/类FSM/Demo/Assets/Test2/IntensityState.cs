using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class IntensityState : State
{
    private Transition intensity2Color;
    public override void InitTransitions(StateManager manager)
    {
        intensity2Color = SetTransition(manager, StateName.Color, TransitionName.Intensity2Color);
        AddTransition(intensity2Color);
    }

    public override Transition SetTransition(StateManager manager, StateName toName, TransitionName transitionName)
    {
        StateMachine machine = manager.GetMachineWithName(Machine.Name);
        State toState = machine.GetStateWithName(toName);
        if (toState == null)
            throw new Exception(toName + "为null");
        Transition temp = new Transition(transitionName, toState);
        temp.onCheck += OnCheck;
        return temp;
    }

    private bool IsIntenMax;
    private Light _light;
    private float _speed = 1;
    private int _maxinten = 2;
    private Test _test;
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
        return _test.IsColor;
    }
    public override void OnStateEnter()
    {
        IsIntenMax = false;
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate(float deltatime)
    {
        if (!IsIntenMax)
        {
            if (_light.intensity < _maxinten)
            {
                _light.intensity += deltatime * _speed;
            }
            else
            {
                _light.intensity = _maxinten;
                IsIntenMax = true;
            }
        }
        else
        {
            if (_light.intensity > 0)
            {
                _light.intensity -= deltatime * _speed;
            }
            else
            {
                _light.intensity = 0;
                IsIntenMax = false;
            }
        }
    }


}
