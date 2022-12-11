using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine
{
    private State currentState;
    private State queuedState;

    private Stack<State> automaton;
    private Dictionary<Type, State> stateDictionary = new Dictionary<Type, State>();

    public StateMachine(object owner, State[] states)
    {
        foreach (State state in states)
        {
            var NewState = UnityEngine.Object.Instantiate(state);
            NewState.owner = owner;
            NewState.stateMachine = this;
            stateDictionary.Add(NewState.GetType(), NewState);

            currentState ??= NewState;

        }
        queuedState = currentState;
        currentState.Enter();
    }

    public void TransitionTo<T>() where T : State
    {
        
        queuedState = stateDictionary[typeof(T)];
        
    }

    public void TranasitionBack()
    {

    }

    public void Run()
    {
        if(currentState != queuedState)
        {
            currentState.Exit();
            currentState = queuedState;
            currentState.Enter();
  
        }
        UpdateState();
        currentState.Run();
    }

    private void UpdateState()
    {
        if (queuedState != currentState)
        {
            currentState?.Exit();
            currentState = queuedState;
            currentState.Enter();
        }
    }
}
