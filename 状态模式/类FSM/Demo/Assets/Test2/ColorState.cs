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
        AddTransition(color2Intensity);
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


    public override void OnStateEnter()
    {
        IsAnimation = false;
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate(float deltatime)
    {
        base.OnStateUpdate(deltatime);

        if (IsAnimation)
        {
            _colorTimer += Time.deltaTime * 1;
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

    public override void OnStateLateUpdate(float detaltime)
    {
        
    }

    public override void OnStateFixedUpdate()
    {
        
    }


}
