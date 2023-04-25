using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    
    private States _currentState;
    
    private Dictionary<AgentStates, States> _allstates = new Dictionary<AgentStates, States>();
    
    public void AddState(AgentStates key, States state)
    {
        if (_allstates.ContainsKey(key)) 
            _allstates[key] = state;
        
        _allstates.Add(key, state);
        
        if (_currentState == null) 
            ChangeState(key);
        
        state.finiteStateMach = this;
    }


    public void ChangeState(AgentStates state, params object[] parameters)
    {
        if (!_allstates.ContainsKey(state)) return;

        if (_currentState != null) _currentState.OnStop();
        _currentState = _allstates[state];
        _currentState.OnStart(parameters);
    }
    
    
    public void Update() =>_currentState.Update();
}