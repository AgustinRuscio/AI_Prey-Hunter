using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class States 
{
    protected FiniteStateMachine finiteStateMach;

    public abstract void OnStart(params object[] parameters);
    
    public abstract void Update();

    public abstract void OnStop();
}