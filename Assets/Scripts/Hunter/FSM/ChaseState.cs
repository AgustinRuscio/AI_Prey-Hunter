using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : States
{
    private Agent _chaseTarget; //Hacer que el target vaya cambiadno
    
    private Hunter _myAgent;

    private float _KillRadius;
    
    
    public ChaseState(Agent target, Hunter agent, float killRadius)
    {
        _chaseTarget = target;
        _myAgent = agent;
        _KillRadius = killRadius;
    }

    public override void OnStart()
    {
        Debug.Log("chase");
        _myAgent.ApplyForce(Persuit(_chaseTarget));
    }
    
    public override void OnStop() { }

    public override void Update()
    {
        _myAgent.ReduceEnergy();
        
        if (_myAgent._actualEnergy <= 0)
        {
            Debug.Log("Cambie a Rest ");
            finiteStateMach.ChangeState(AgentStates.Rest); // no me deja cambias de estados
        }
        

        if(Vector3.Distance(_myAgent.transform.position, _chaseTarget.transform.position) >= _KillRadius)
            _myAgent.ApplyForce(Persuit(_chaseTarget));
        else
        {
            _chaseTarget.AutoDestruction();
            finiteStateMach.ChangeState(AgentStates.Patrol);
        }
    }
    
    private Vector3 Persuit(Agent target)
    {
        if (target == null) return Vector3.zero;

        Vector3 futurePos = (target.transform.position + target._velocity);
        Vector3 desired = futurePos - _myAgent.transform.position;
        
        return _myAgent.CalculateStreering(desired);
    }
}