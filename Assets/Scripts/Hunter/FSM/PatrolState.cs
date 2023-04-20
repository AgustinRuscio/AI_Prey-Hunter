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
    
    public PatrolState(Transform transform, Transform[] waypoints,float viewRadius, float waypointDetectionRadius ,Hunter agent, LayerMask preyMask)
    {
        _transform = transform;
        _waypoints = waypoints;
        _chaseTargetViewRadius = viewRadius;
        _waypointsRadius = waypointDetectionRadius;
        _agent = agent;
        _preyMask = preyMask;
    }

    public override void OnStart()
    {
        _agent.ApplyForce(_agent.Seek(Waypoints()));
    }


    public override void Update()
    {
        Debug.Log("Entre al patrol");
        PatrolEnergy();

        ViewPrey();
        
        _agent.ApplyForce(_agent.Seek(Waypoints()));
    }

    private void PatrolEnergy()
    {
        _agent.ReduceEnergy();
      
        if (_agent._actualEnergy <= 0)
        {
            Debug.Log("cambie de estado a rest");
            finiteStateMach.ChangeState(AgentStates.Rest);
        }

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

        //_tranform.position += (_waypoints[_currentWaypoint].position - _tranform.position).normalized * _agent._speed * Time.deltaTime;
        //_agent.ApplyForce((_waypoints[_currentWaypoint].position - _tranform.position).normalized * _agent._speed);
        //return new Vector3(_waypoints[_currentWaypoint].position.x,0, _waypoints[_currentWaypoint].position.z);
    }


    private void ViewPrey()
    {
        //veo a la presa y cambio a chase state
        
        Collider[] preyDetector = Physics.OverlapSphere(_transform.position, _chaseTargetViewRadius, _preyMask);

        for (int i = 0; i < preyDetector.Length; i++)
        {
            if (preyDetector[i] != null)
            {
                Debug.Log("Cambie a Chase");
                finiteStateMach.ChangeState(AgentStates.Chase);
            }
           
        }
    
    }

    public override void OnStop() { }
}
