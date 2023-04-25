using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Agent
{
    private FiniteStateMachine _finiteStateMach;

    #region Rest Status Variables

       
    public float _actualEnergy;

    
    public float _maxEnergy;

    #endregion

    #region Patrol State Variables

    [SerializeField]
    private Transform[] _waypoints;

    [SerializeField] 
    private float _waypointDetectionRadius;

    [SerializeField] 
    private LayerMask _preyLayerMask;
    
    #endregion
    
    #region Chase State Variables
    
    [SerializeField] 
    private float _KillRadius;

    
    public Prey _target;

        
    #endregion

    private bool _resting;

    private void Awake()
    {
        EventManager.Subscribe(EventEnum.HunterRest, CheckRestState);
    }

    protected override void Start()
    {
        _finiteStateMach = new FiniteStateMachine();
        
        _finiteStateMach.AddState(AgentStates.Patrol, new PatrolState(this).SetLayerMask(_preyLayerMask).SetWayPoints(_waypoints)
                                                .SetPatrolAgentTransform(transform).SetPreyViewRadius(_generalViewRadius).SetWaypointsViewRadius(_waypointDetectionRadius));
        
        _finiteStateMach.AddState(AgentStates.Chase, new ChaseState(this, _KillRadius));
        _finiteStateMach.AddState(AgentStates.Rest, new RestState(this));
    }

    protected override void Update()
    {
        _finiteStateMach.Update();
        
        if(_resting) return;
        
        base.Update();
        
        Move();

        if (ObstacleAvoidanceMovement(true))
        {
            ObstacleAvoidanceMovement(true);
            return;
        }
        
    }

    public Prey GetTarget(Prey target)
    {
        _target = target;

        return _target;
    }


    #region Rest State Methods

    public void ReduceEnergy() => _actualEnergy -= Time.deltaTime;
    public void RecoverEnergy() => _actualEnergy += Time.deltaTime;
    private void CheckRestState(params object[] isResting) => _resting = (bool)isResting[0];

    #endregion

    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventEnum.HunterRest, CheckRestState);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _generalViewRadius);

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, _KillRadius);
        
        
        //ObstaclesTectection
        Gizmos.color =Color.magenta;

        Vector3 orpos = (transform.position + Vector3.up) + transform.right/2;
        
        Gizmos.DrawLine(orpos, orpos+transform.forward * _viewObstacleRadius);
        
        Vector3 o2rpos = (transform.position + Vector3.up) - transform.right/2;
        
        Gizmos.DrawLine(o2rpos, o2rpos+transform.forward * _viewObstacleRadius);
        
        //Second OPtion
        Gizmos.color =Color.magenta;
        //Gizmos.DrawWireSphere(transform.position + new Vector3(0,1,0) + transform.forward, _viewObstacleRadius);
        
        //WaypointsDetecttion
        Gizmos.color =Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _waypointDetectionRadius);

        //Fence radius
        Gizmos.color =Color.white;
        Gizmos.DrawWireSphere(transform.position + transform.forward, _viewFenceRadius);
    }
}