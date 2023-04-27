using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : States
{
    private Transform[] _waypoints;

    private Transform _transform;
    
    private int _currentWaypoint = 0;

    private float _waypointsRadius;
    
    private Hunter _agent;

    private float _chaseTargetViewRadius;

    private LayerMask _preyMask;
    
    
    public PatrolState(Hunter agent) => _agent = agent;
    
    #region Builder

    public PatrolState SetPatrolAgentTransform(Transform transform)
    {
        _transform = transform;
        return this;
    }
    public PatrolState SetWayPoints(Transform[] waypoints)
    {
        _waypoints = waypoints;
        return this;
    }
    
    public PatrolState SetWaypointsViewRadius(float waypointDetectionRadius)
    {
        _waypointsRadius = waypointDetectionRadius;
        return this;
    }

    public PatrolState SetPreyViewRadius(float radius)
    {
        _chaseTargetViewRadius = radius;
        return this;
    }
    
    public PatrolState SetLayerMask(LayerMask layer)
    {
        _preyMask = layer;
        return this;
    }

    #endregion

    public override void OnStart(params object[] parameters) => _agent.ApplyForce(_agent.Seek(Waypoints()));
    
    
    public override void Update()
    {
        PatrolEnergy();  

        ViewPrey();
        
        _agent.ApplyForce(_agent.Seek(Waypoints()));
    }

    private void PatrolEnergy()
    {
        _agent.ReduceEnergy();
      
        if (_agent._actualEnergy <= 0)
            finiteStateMach.ChangeState(AgentStates.Rest);
    }


    private Vector3 Waypoints()
    {
        if (Vector3.Distance(_transform.position, _waypoints[_currentWaypoint].position) <= _waypointsRadius)
        {
            _currentWaypoint++;
            
            if (_currentWaypoint >= _waypoints.Length)
                _currentWaypoint = 0;
        }

        return _waypoints[_currentWaypoint].position;
    }


    private void ViewPrey()
    {
        Collider[] preyDetector = Physics.OverlapSphere(_transform.position, _chaseTargetViewRadius, _preyMask);

        for (int i = 0; i < preyDetector.Length; i++)
        {
            if (preyDetector[i] != null)
            { 
                _agent.GetTarget(preyDetector[i].GetComponent<Prey>());
                finiteStateMach.ChangeState(AgentStates.Chase);
            }
        }
    }

    public override void OnStop() { }
}