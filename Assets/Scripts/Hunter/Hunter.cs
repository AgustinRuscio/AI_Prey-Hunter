using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Agent
{
    private FiniteStateMachine _finiteStateMach;

    //--------------------Rest Variables

   
    public float _actualEnergy;

    
    public float _maxEnergy;
    
    //--------------------Patrol Variables
    [SerializeField]
    private Transform[] _waypoints;

    [SerializeField] 
    private float _waypointDetectionRadius;

    [SerializeField] 
    private LayerMask _preyLayerMask;

    //--------------------Chase Variables
    [SerializeField] 
    private Agent _chaseTarget;

    [SerializeField] 
    private float _KillRadius;

    
    
    protected override void Start()
    {
        _finiteStateMach = new FiniteStateMachine();
        
        //_finiteStateMach.AddState(AgentStates.Patrol, new PatrolState(transform, _waypoints, _viewRadius, _waypointDetectionRadius, this,_preyLayerMask));
        _finiteStateMach.AddState(AgentStates.Chase, new ChaseState(_chaseTarget, this, _KillRadius));
        _finiteStateMach.AddState(AgentStates.Rest, new RestState(_actualEnergy, _maxEnergy, this));
        
    }

    protected override void Update()
    {
        _finiteStateMach.Update();
       
        Move();
    }

    public void ReduceEnergy()
    {
        _actualEnergy -= Time.deltaTime;
    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, _KillRadius);
        
    }


}
