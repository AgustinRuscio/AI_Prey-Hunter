using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : States
{
    private Agent _chaseTarget; //Hacer que el target vaya cambiadno
    
    private Hunter _myAgent;

    private float _KillRadius;
    
    
    public ChaseState(Hunter agent, float killRadius, Agent chase)
    {
        _myAgent = agent;
        _KillRadius = killRadius;
        _chaseTarget = chase;
    }

    public override void OnStart(params object[] parameters)
    {
        Debug.Log("chase");
        
        //if(parameters[0] != null) _chaseTarget = (Transform)parameters[0];
        
        _myAgent.ApplyForce(_myAgent.Persuit(_chaseTarget.transform)* _myAgent._speed);
        EventManager.Trigger(EventEnum.HuntingAnims, true);
    }

    public override void OnStop()
    {
        _chaseTarget = null;
    }

    public override void Update()
    {
        _myAgent.ReduceEnergy();
        
        if (_myAgent._actualEnergy <= 0)
        {
            Debug.Log("Cambie a Rest ");
            finiteStateMach.ChangeState(AgentStates.Rest); // no me deja cambias de estados
        }


        if (Vector3.Distance(_myAgent.transform.position, _chaseTarget.transform.position) >= _KillRadius)
        {
            _myAgent.ApplyForce(_myAgent.Persuit(_chaseTarget.transform) * _myAgent._speed);
        }
        else
        {
            EventManager.Trigger(EventEnum.HuntingAnims, false);
            EventManager.Trigger(EventEnum.PreyDeath); //Hacer que el Prey se muera con animacion
            finiteStateMach.ChangeState(AgentStates.Patrol);
        }
    }
    
}