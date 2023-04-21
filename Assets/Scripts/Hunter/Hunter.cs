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
    private float _KillRadius;

    [SerializeField] private Agent _target;
    
    
    protected override void Start()
    {
        _finiteStateMach = new FiniteStateMachine();
        
        //_finiteStateMach.AddState(AgentStates.Patrol, new PatrolState(this).SetLayerMask(_preyLayerMask).SetWayPoints(_waypoints)
                        //                        .SetPatrolAgentTransform(transform).SetPreyViewRadius(_viewRadius).SetWaypointsViewRadius(_waypointDetectionRadius));
        
        _finiteStateMach.AddState(AgentStates.Chase, new ChaseState(this, _KillRadius, _target));
        //_finiteStateMach.AddState(AgentStates.Rest, new RestState(this));
        
    }

    protected override void Update()
    {
        Move();

        if (ObstacleAvoidanceMovement(false))
            ObstacleAvoidanceMovement(false);
        else
        _finiteStateMach.Update();

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
        
        Gizmos.color =Color.magenta;

        Vector3 orpos = (transform.position + new Vector3(0,1,0)) + transform.right/2;
        
        Gizmos.DrawLine(orpos, orpos+transform.forward * _viewRadius);
        
        Vector3 o2rpos = (transform.position + new Vector3(0,1,0)) - transform.right/2;
        
        Gizmos.DrawLine(o2rpos, o2rpos+transform.forward * _viewRadius);
        
        Gizmos.color =Color.magenta;
        //Gizmos.DrawWireSphere(transform.position + new Vector3(0,1,0) + transform.forward, _viewRadius);
    }


}
