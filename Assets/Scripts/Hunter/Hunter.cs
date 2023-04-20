using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Agent
{
    protected FiniteStateMachine _finiteStateMach;

    [SerializeField]
    protected Transform[] _waypoints;

    [SerializeField]
    protected int _actualEnergy;


    protected override void Start()
    {
        

        base.Start();
        
        _finiteStateMach = new FiniteStateMachine();
        _finiteStateMach.AddState(AgentStates.Patrol, new PatrolState(transform, _waypoints, _actualEnergy, _velocity, this));
        _finiteStateMach.AddState(AgentStates.Chase, new ChaseState());
        




    }

    protected override void Update()
    {
        base.Update();

        _finiteStateMach.Update();

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;


    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _viewRadius);

    }


}
