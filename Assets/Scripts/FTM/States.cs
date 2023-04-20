using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class States 
{


    public FiniteStateMachine finiteStateMach;

    public abstract void Update();

    public abstract void OnStart();

    public abstract void OnStop();



}
