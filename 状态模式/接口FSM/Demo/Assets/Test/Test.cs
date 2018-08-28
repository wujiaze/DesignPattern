using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed = 1;
    private bool _isMove = false;

    private StateMachine _fsm;
    private State _idle;
    private State _move;
    private Transition _idle2move;
    private Transition _move2idle;

	void Start () {
        // 状态1
	    _idle = new State("idle");
        _idle.onEnter += IdleOnOnEnter;
        // 状态2
        _move = new State("move");
	    _move.onStayUpdate += MoveOnOnStayUpdate;
	    _move.onEnter += IdleOnOnEnter;
        // 状态过渡1
        _idle2move = new Transition("idel2move",_idle,_move);
	    _idle2move.onCheck += OnCheckState;
	    _idle.AddTransition(_idle2move);
        // 状态过渡2
        _move2idle = new Transition("move2idle",_move,_idle);
	    _move2idle.onCheck += () => !_isMove;
	    _move.AddTransition(_move2idle);
        // 状态机
	    _fsm = new StateMachine("root",_idle);
	    _fsm.AddState(_move);

    }

    private void IdleOnOnEnter(IState state) 
    {
        Debug.Log("进入：" + state.Name + " 状态");
    }
    private void MoveOnOnStayUpdate(float f)
    {
        transform.position += transform.forward * f * speed;
    }

    private bool OnCheckState()
    {
        return _isMove;
    }

    void Update () {
        _fsm.OnUpdateExecute(Time.deltaTime);
    }

     void OnGUI()
    {
        if (GUILayout.Button("Move"))
        {
            _isMove = true;                 // 实际项目中，就需要用其他的方法来判断这个值
        }
        if (GUILayout.Button("Stop"))
        {
            _isMove = false;
        }
    }

}
