using UnityEngine;
using System.Collections;
using FSM;
using System.Collections.Generic;

public class MainStateMachine : StateMachine
{


    public override void SetObject(object obj)
    {

    }

    public override void InitTransitions(StateManager manager)
    {
       
    }

    public override Transition SetTransition(StateManager manager, StateName toName, TransitionName transitionName)
    {
        return null;
    }

    public MainStateMachine(StateName name, StateMachine machine) :base( name, machine)
    {

    }
}
